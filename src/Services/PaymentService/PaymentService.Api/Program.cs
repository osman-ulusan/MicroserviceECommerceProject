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

builder.Services.AddLogging(configure => configure.AddConsole());//log i�lemleri i�in
builder.Services.AddTransient<OrderStartedIntegrationEventHandler>(); //baseeventbus i�erisindeki ProcessEvent i�erisindeki handler�n al�nmas� ve handle metonun �al��t�r�labilmesi i�in DI olarak bunu kullan�yor
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

IEventBus eventBus = app.Services.GetRequiredService<IEventBus>();//sistem i�erisinden �eventbus create edilecek rabbitmq ile entegre olmu� eventbus verecek
eventBus.Subscribe<OrderStartedIntegrationEvent, OrderStartedIntegrationEventHandler>(); //bunlar� dinlemeye ba�layacak eventbus

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
