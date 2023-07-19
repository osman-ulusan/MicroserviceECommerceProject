using OrderService.Api.IntegrationEvents.EventHandlers;

namespace OrderService.Api.Extensions.Registration.EventHandlerRegistration
{
    public static class EventHandlerRegistration
    {
        //event busa yani buradaki apimize bir event geldiğinde eventi alıp buradaki event handlerı oluşturabilsin diye .net core transint olarak ekledik.
        public static IServiceCollection ConfigureEventHandlers(this IServiceCollection services)
        {
            services.AddTransient<OrderCreatedIntegrationEventHandler>();

            return services;
        }
    }
}
