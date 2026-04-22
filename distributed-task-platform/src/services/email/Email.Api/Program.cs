using Email.Api.Consumers;
using Email.Api.Data;
using Email.Api.Infrastructure;
using Email.Api.Services;
using Microsoft.EntityFrameworkCore;
using Shared.Common;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();


builder.Services.AddDbContext<EmailDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("EmailsDb")));

builder.Services.AddHostedService<TaskEventConsumer>();
builder.Services.AddSingleton<IMqTopologyInitializer, EmailMqTopologyInitializer>();
builder.Services.AddScoped<IEmailsPendingService, EmailsPendingService>();

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
    var dbContext = scope.ServiceProvider.GetRequiredService<EmailDbContext>();
    dbContext.Database.Migrate();
}

//Init Mq structure
using var scopes = app.Services.CreateScope();
var initializer = scopes.ServiceProvider.GetRequiredService<IMqTopologyInitializer>();
initializer.Initialize();

app.Run();

