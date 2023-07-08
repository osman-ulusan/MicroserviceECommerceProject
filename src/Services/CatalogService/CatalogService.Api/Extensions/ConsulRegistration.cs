using Consul;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;

namespace CatalogService.Api.Extensions
{
    public static class ConsulRegistration
    {
        public static IServiceCollection ConfigureConsul(this IServiceCollection services, IConfiguration configuration)
        {
            //ne zaman IConsuleClient istenirse bir tane consuleclient oluşturulacak ve appsettingteki address alınıp kullanılacak
            services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
            {
                var address = configuration["ConsulConfig:Address"]; //appsettings
                consulConfig.Address = new Uri(address);
            }));

            return services;
        }

        public static IApplicationBuilder RegisterWithConsul(this IApplicationBuilder app, IHostApplicationLifetime lifetime)
        {
            var consulClient = app.ApplicationServices.GetRequiredService<IConsulClient>();//burada consulclient istiyoruz
            var loggingFactory = app.ApplicationServices.GetRequiredService<ILoggerFactory>();
            var logger = loggingFactory.CreateLogger<IApplicationBuilder>();

            // Get server IP Address -> host bilgisini almak için bunları kullanmamız gerekiyor .net core bize sağlıyor bunu

            var features = app.Properties["server.Features"] as FeatureCollection;
            var addresses = features.Get<IServerAddressesFeature>();
            var address = addresses.Addresses.First();

            // Register service with consul
            var uri = new Uri(address);
            var registration = new AgentServiceRegistration()
            {
                ID = $"CatalogService",
                Name = $"CatalogService", //ocelotta verdiğimiz servicename burada
                Address = $"{uri.Host}",
                Port = uri.Port,
                Tags = new[] { "Catalog Service", "Catalog" }
            };

            logger.LogInformation("Registering with Consul");
            consulClient.Agent.ServiceDeregister(registration.ID).Wait(); //eğer daha önce kaydedilmişsse sil
            consulClient.Agent.ServiceRegister(registration).Wait(); //tekrar yeni bilgilerle kaydet

            //app kapanırsa veya kapatılırsa git consuldan kendini sil çalışmadıgı belli olsun diyoruz
            lifetime.ApplicationStopping.Register(() =>
            {
                logger.LogInformation("Deregistering from Consul");
                consulClient.Agent.ServiceDeregister(registration.ID).Wait();
            });

            return app;

            //buradan sonra program.cs de işlemleri tamamlıyoruz.
        }
    }
}
