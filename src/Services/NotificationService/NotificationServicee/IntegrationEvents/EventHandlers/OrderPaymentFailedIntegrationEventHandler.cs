using EventBus.Base.Abstraction;
using Microsoft.Extensions.Logging;
using NotificationService.IntegrationEvents.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.IntegrationEvents.EventHandlers
{
    public class OrderPaymentFailedIntegrationEventHandler : IIntegrationEventHandler<OrderPaymentFailedIntegrationEvent>
    {
        private readonly ILogger<OrderPaymentFailedIntegrationEvent> logger;

        public OrderPaymentFailedIntegrationEventHandler(ILogger<OrderPaymentFailedIntegrationEvent> logger)
        {
            this.logger = logger;
        }

        public Task Handle(OrderPaymentFailedIntegrationEvent @event)
        {
            // Send Fail Notification (Sms, Email, Push)

            logger.LogInformation($"Order Payment failed with OrderId: {@event.OrderId}, ErrorMessage: {@event.ErrorMessage}");

            return Task.CompletedTask;
        }
    }
}
