using CatalogService.Api.Extensions;
using CatalogService.Api.Infrastructure;
using CatalogService.Api.Infrastructure.Context;
using Microsoft.AspNetCore.Hosting;

//var builder = WebApplication.CreateBuilder(args);

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    WebRootPath = "Pics",
    ContentRootPath = Directory.GetCurrentDirectory()
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<CatalogSettings>(builder.Configuration.GetSection("CatalogSettings"));
builder.Services.ConfigureDbContext(builder.Configuration);

//builder.WebHost.UseWebRoot("Pics");
//builder.WebHost.UseContentRoot(Directory.GetCurrentDirectory());

var app = builder.Build();

app.MigrateDbContext<CatalogContext>((context, services) =>
{
    var env = services.GetService<IWebHostEnvironment>();
    var logger = services.GetService<ILogger<CatalogContextSeed>>();

    new CatalogContextSeed()
        .SeedAsync(context, env, logger)
        .Wait();
});

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
