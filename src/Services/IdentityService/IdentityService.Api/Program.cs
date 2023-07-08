using IdentityService.Api.Application.Services;
using IdentityService.Api.Extensions;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Hosting.Server;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<IIdentityService, IdentityService.Api.Application.Services.IdentityService>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureConsul(builder.Configuration); //ConsulRegistration.cs deki method.

var app = builder.Build();

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

app.Start(); //app.Run() kýsmý ile deðiþildi ConsulRegistrationda var address = addresses.Addresses.First(); null geliyordu..

app.RegisterWithConsul(lifetime); //ConsulRegistration.cs deki method.

app.WaitForShutdown(); // eklendi.
