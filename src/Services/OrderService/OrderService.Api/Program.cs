using EventBus.Base;
using EventBus.Base.Abstraction;
using EventBus.Factory;
using OrderService.Api.Extensions;
using OrderService.Api.Extensions.Registration.EventHandlerRegistration;
using OrderService.Api.Extensions.Registration.ServiceDiscovery;
using OrderService.Api.IntegrationEvents.EventHandlers;
using OrderService.Api.IntegrationEvents.Events;
using OrderService.Application;
using OrderService.Infrastructure;
using OrderService.Infrastructure.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//eklemeler
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.AddLogging(configure => configure.AddConsole())
                .AddApplicationRegistration(typeof(Program))
                .AddPersistenceRegistration(builder.Configuration)
                .ConfigureEventHandlers()
                .AddServiceDiscoveryRegistration(builder.Configuration);

builder.Services.AddSingleton(sp =>
{
    EventBusConfig config = new()
    {
        ConnectionRetryCount = 5,
        EventNameSuffix = "IntegrationEvent",
        SubscriberClientAppName = "OrderService",
        //Connection = new ConnectionFactory(),
        EventBusType = EventBusType.RabbitMQ,

    };

    return EventBusFactory.Create(config, sp);
});

var app = builder.Build();

//eklemeler
app.MigrateDbContext<OrderDbContext>((context, services) =>
{
    //var env = services.GetService<IWebHostEnvironment>();
    var logger = services.GetService<ILogger<OrderDbContext>>();

    new OrderDbContextSeed()
        .SeedAsync(context, logger)
        .Wait();
});

IEventBus eventBus = app.Services.GetRequiredService<IEventBus>();
eventBus.Subscribe<OrderCreatedIntegrationEvent, OrderCreatedIntegrationEventHandler>(); //bunlarý dinlemeye baþlayacak eventbus

var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>(); //ConsulRegistration için

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//app.Run();

app.Start(); //app.Run() kýsmý ile deðiþildi ConsulRegistrationda var address = addresses.Addresses.First(); null geliyordu..

app.RegisterWithConsul(lifetime); //ConsulRegistration.cs deki method.

app.WaitForShutdown(); // eklendi.
