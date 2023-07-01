using EventBus.Base;
using EventBus.Base.Abstraction;
using EventBus.Factory;
using EventBus.UnitTest.Events.EventHandlers;
using EventBus.UnitTest.Events.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace EventBus.UnitTest
{
    [TestClass]
    public class EventBusTests
    {

        private ServiceCollection services;

        public EventBusTests()
        {
            services = new ServiceCollection();
            services.AddLogging(configure=>configure.AddConsole());
        }

        [TestMethod]
        public void subscribe_event_on_rabbitmq_test()
        {
            services.AddSingleton<IEventBus>(sp => //her ýeventbus istediðinde çalýþacak
            {
                return EventBusFactory.Create(GetRabbitMQConfig(), sp);
            });
            var sp = services.BuildServiceProvider();

            var eventBus = sp.GetRequiredService<IEventBus>(); //bura çalýþtýðýnda yukarýsý çalýþacak.

            eventBus.Subscribe<OrderCreatedIntergrationEvent,OrderCreatedIntegrationEventHandler>();
            //eventBus.UnSubscribe<OrderCreatedIntergrationEvent,OrderCreatedIntegrationEventHandler>();

        }

        [TestMethod]
        public void subscribe_event_on_azure_test()
        {
            services.AddSingleton<IEventBus>(sp => //her ýeventbus istediðinde çalýþacak
            {
                return EventBusFactory.Create(GetAzureConfig(), sp);
            });
            var sp = services.BuildServiceProvider();


            var eventBus = sp.GetRequiredService<IEventBus>(); //bura çalýþtýðýnda yukarýsý çalýþacak.

            eventBus.Subscribe<OrderCreatedIntergrationEvent, OrderCreatedIntegrationEventHandler>();
            //eventBus.UnSubscribe<OrderCreatedIntergrationEvent, OrderCreatedIntegrationEventHandler>();


            Task.Delay(2000).Wait(); // cloudda olduugu için ekledik biraz zaman
        }


        [TestMethod]
        public void send_message_to_rabbitmq()
        {
            services.AddSingleton<IEventBus>(sp =>
            {
                return EventBusFactory.Create(GetRabbitMQConfig(), sp);
            });
            var sp = services.BuildServiceProvider();

            var eventBus = sp.GetRequiredService<IEventBus>();

            eventBus.Publish(new OrderCreatedIntergrationEvent(1));
        }

        [TestMethod]
        public void send_message_to_azure_test()
        {
            services.AddSingleton<IEventBus>(sp =>
            {
                return EventBusFactory.Create(GetAzureConfig(), sp);
            });
            var sp = services.BuildServiceProvider();

            var eventBus = sp.GetRequiredService<IEventBus>();

            eventBus.Publish(new OrderCreatedIntergrationEvent(1));
        }

        private EventBusConfig GetAzureConfig()
        {
            return new EventBusConfig()
            {
                ConnectionRetryCount = 5,
                SubscriberClientAppName = "EventBus.UnitTest",
                DefaultTopicName = "MicroserviceETicaretProjesiTopicName",
                EventBusType = EventBusType.AzureServiceBus,
                EventNameSuffix = "IntegrationEvent",
                EventBusConnectionString = "Endpoint=sb://microserviceeticaret.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=J+6rdAIVyuTPiC6PcrqV5W3C6VwwtvEbC+ASbKLWAZA="
            };
        }

        private EventBusConfig GetRabbitMQConfig()
        {
            return new EventBusConfig()
            {
                ConnectionRetryCount = 5,
                SubscriberClientAppName = "EventBus.UnitTest",
                DefaultTopicName = "MicroserviceETicaretProjesiTopicName",
                EventBusType = EventBusType.RabbitMQ,
                EventNameSuffix = "IntegrationEvent",
                //Connection = new ConnectionFactory()
                //{
                //    HostName = "localhost",
                //    Port = 15672,
                //    UserName = "guest",
                //    Password = "guest"
                //}
            };
        }
    }
}