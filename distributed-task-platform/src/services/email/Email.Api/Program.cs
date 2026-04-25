using Email.Api.Consumers;
using Email.Api.Data;
using Email.Api.Infrastructure;
using Email.Api.Services;
using Email.Api.Services.EmailSender;
using Email.Api.Workers.EmailSender;
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
builder.Services.AddSingleton<IEventPublisher, RabbitMqEventPublisher>();
builder.Services.AddScoped<Email.Api.Services.IEmailsPendingService, Email.Api.Services.EmailsPendingService>();
builder.Services.AddScoped<Email.Api.Services.EmailSender.IEmailsPendingService, Email.Api.Services.EmailSender.EmailsPendingService>();
builder.Services.AddScoped<IEmailSenderService, EmailSenderService>();
builder.Services.AddScoped<IEmailEventPublisher, EmailEventPublisher>();

builder.Services.AddHostedService<EmailPendingWorker>();

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

