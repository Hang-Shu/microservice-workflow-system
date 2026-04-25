using Microsoft.EntityFrameworkCore;
using Query.Api.Consumers;
using Query.Api.Data;
using Query.Api.Infrastructure;
using Query.Api.Services;
using Query.Api.Services.QueryServices;
using Shared.Common;
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();


builder.Services.AddDbContext<QueryDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("QueryDb")));

builder.Services.AddHostedService<TaskUpdatedEventConsumer>();
builder.Services.AddHostedService<EmailSendEventConsumer>();
builder.Services.AddHostedService<TaskCommentEventConsumer>();

builder.Services.AddSingleton<IMqTopologyInitializer, QueryMqTopologyInitializer>();
builder.Services.AddScoped<ITaskModifyLineService, TaskModifyLineService>();
builder.Services.AddScoped<IEmailSendService, EmailSendService>();
builder.Services.AddScoped<ITaskCommentService, TaskCommentService>();
builder.Services.AddScoped<IQueryTaskDatasService, QueryTaskDatasService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapHealthChecks("/health");
app.MapControllers();

// Auto refresh DB
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<QueryDbContext>();
    dbContext.Database.Migrate();
}

//Init Mq structure
using var scopes = app.Services.CreateScope();
var initializer = scopes.ServiceProvider.GetRequiredService<IMqTopologyInitializer>();
initializer.Initialize();

app.Run();
