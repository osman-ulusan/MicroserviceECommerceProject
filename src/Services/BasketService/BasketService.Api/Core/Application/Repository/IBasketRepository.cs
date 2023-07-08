using BasketService.Api.Core.Domain.Models;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BasketService.Api.Core.Application.Repository
{
    public interface IBasketRepository
    {
        Task<CustomerBasket> GetBasketAsync(string customerId); //sepetin kendisini getiren
        IEnumerable<string> GetUsers(); //redis içerisinde bulunan bütün kullanıcıları getiren

        Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket); //basketi update eden sepette güncelleme yapıldıysa

        Task<bool> DeleteBasketAsync(string id);// sepeti boşaltan
    }
}
