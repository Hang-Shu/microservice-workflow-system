using Mapster;
using Microsoft.EntityFrameworkCore;
using Shared.Common;
using Shared.Contracts.Events;
using Task.Api;
using Task.Api.Data;
using Task.Api.Infrastructure;
using Task.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();
builder.Services.AddMapster();




builder.Services.AddDbContext<TaskDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("TaskDb")));


builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<ITaskEventPublisher, TaskEventPublisher>();

builder.Services.AddSingleton<IMqTopologyInitializer, TaskMqTopologyInitializer>();
builder.Services.AddSingleton<IEventPublisher, RabbitMqEventPublisher>();


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
    var dbContext = scope.ServiceProvider.GetRequiredService<TaskDbContext>();
    dbContext.Database.Migrate();
}

//Init Mq structure
using var scopes = app.Services.CreateScope();
var initializer = scopes.ServiceProvider.GetRequiredService<IMqTopologyInitializer>();
initializer.Initialize();


app.Run();

