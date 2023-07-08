using BasketService.Api.Core.Application.Repository;
using BasketService.Api.Core.Application.Services;
using BasketService.Api.Extensions;
using BasketService.Api.Infrastructure;
using BasketService.Api.IntegrationEvents.EventHandlers;
using BasketService.Api.IntegrationEvents.Events;
using EventBus.Base;
using EventBus.Base.Abstraction;
using EventBus.Factory;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureAuth(builder.Configuration);//jwt imp
builder.Services.AddSingleton(sp => sp.ConfigureRedis(builder.Configuration)); //redis con imp
builder.Services.ConfigureConsul(builder.Configuration); //ConsulRegistration.cs deki method.

//eventbus ile basketservice aras�nda ba�lant� olaca�� i�in eventbus imp yap�yoruz
builder.Services.AddSingleton<IEventBus>(sp =>
{
    EventBusConfig config = new()
    {
        ConnectionRetryCount = 5,
        EventNameSuffix = "IntegrationEvent",
        SubscriberClientAppName = "BasketService",
        EventBusType = EventBusType.RabbitMQ
    };

    return EventBusFactory.Create(config, sp);
});

builder.Services.AddHttpContextAccessor();//IdentityServicede kullanm��t�k, buraya ekliyoruz

builder.Services.AddScoped<IBasketRepository, BasketRepository>(); //ne zaman IBaskerRepositoryi kullanmak istersek bize BasketRepository d�necek
builder.Services.AddTransient<IIdentityService, IdentityService>(); //ne zaman IIdentityService kullanmak istersek bize IdentityService d�necek

builder.Services.AddTransient<OrderCreatedIntegrationEventHandler>();

var app = builder.Build();

IEventBus eventBus = app.Services.GetRequiredService<IEventBus>();
eventBus.Subscribe<OrderCreatedIntegrationEvent, OrderCreatedIntegrationEventHandler>(); //bunlar� dinlemeye ba�layacak eventbus

var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>(); //ConsulRegistration i�in

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication(); //controllerda [Authorize] lar�n aktif olmas� i�in
app.UseAuthorization();

app.MapControllers();

app.Start(); //app.Run() k�sm� ile de�i�ildi ConsulRegistrationda var address = addresses.Addresses.First(); null geliyordu..

app.RegisterWithConsul(lifetime); //ConsulRegistration.cs deki method.

app.WaitForShutdown(); // eklendi.
