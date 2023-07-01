using EventBus.Base.Abstraction;
using EventBus.UnitTest.Events.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.UnitTest.Events.EventHandlers
{
    public class OrderCreatedIntegrationEventHandler : IIntegrationEventHandler<OrderCreatedIntergrationEvent>
    {
        public Task Handle(OrderCreatedIntergrationEvent @event)
        {
            return Task.CompletedTask;
        }
    }
}
