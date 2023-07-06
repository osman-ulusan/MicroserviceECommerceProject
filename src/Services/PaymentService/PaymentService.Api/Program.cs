using EventBus.Base;
using EventBus.Base.Abstraction;
using EventBus.Factory;
using PaymentService.Api.IntegrationEvents.EventHandlers;
using PaymentService.Api.IntegrationEvents.Events;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//eklemelerim

builder.Services.AddLogging(configure => configure.AddConsole());//log iþlemleri için
builder.Services.AddTransient<OrderStartedIntegrationEventHandler>(); //baseeventbus içerisindeki ProcessEvent içerisindeki handlerýn alýnmasý ve handle metonun çalýþtýrýlabilmesi için DI olarak bunu kullanýyor
builder.Services.AddSingleton<IEventBus>(sp =>
{
    EventBusConfig config = new()
    {
        ConnectionRetryCount = 5,
        EventNameSuffix = "IntegrationEvent",
        SubscriberClientAppName = "PaymentService",
        EventBusType = EventBusType.RabbitMQ
    };

    return EventBusFactory.Create(config, sp);
});

var app = builder.Build();

IEventBus eventBus = app.Services.GetRequiredService<IEventBus>();//sistem içerisinden ýeventbus create edilecek rabbitmq ile entegre olmuþ eventbus verecek
eventBus.Subscribe<OrderStartedIntegrationEvent, OrderStartedIntegrationEventHandler>(); //bunlarý dinlemeye baþlayacak eventbus

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
