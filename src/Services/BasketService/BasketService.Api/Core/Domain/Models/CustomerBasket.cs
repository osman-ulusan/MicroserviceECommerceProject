using System.Collections.Generic;

namespace BasketService.Api.Core.Domain.Models
{
    public class CustomerBasket
    {
        public string BuyerId { get; set; }
        public List<BasketItem> BasketItems { get; set; } = new List<BasketItem>();
        public CustomerBasket() 
        { 

        }

        public CustomerBasket(string customerId)
        {
            BuyerId = customerId;
        }
    }
}
