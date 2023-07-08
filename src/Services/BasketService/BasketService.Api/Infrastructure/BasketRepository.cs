using BasketService.Api.Core.Application.Repository;
using BasketService.Api.Core.Domain.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasketService.Api.Infrastructure
{
    public class BasketRepository : IBasketRepository
    {
        private readonly ILogger<BasketRepository> _logger;
        private readonly ConnectionMultiplexer _redis; //program.cs de kullanabilmek için addsingleton olarak configureredis olarak eklemiştik..
        private readonly IDatabase _database; //redisten geliyor

        public BasketRepository(ILoggerFactory loggerFactory, ConnectionMultiplexer redis)
        {
            _logger = loggerFactory.CreateLogger<BasketRepository>();
            _redis = redis;
            _database = redis.GetDatabase();
        }

        public async Task<bool> DeleteBasketAsync(string id)
        {
            return await _database.KeyDeleteAsync(id);
        }

        public async Task<CustomerBasket> GetBasketAsync(string customerId)
        {
            var data = await _database.StringGetAsync(customerId);

            if (data.IsNullOrEmpty)
            {
                return null;
            }
            return JsonConvert.DeserializeObject<CustomerBasket>(data);
        }

        public IEnumerable<string> GetUsers()
        {
            var server = GetServer();
            var data = server.Keys();

            return data?.Select(k => k.ToString());
        }

        public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket)
        {
            var created = await _database.StringSetAsync(basket.BuyerId, JsonConvert.SerializeObject(basket)); // basket ile buyerı stringSETAsync ile SET ediyoruz, hiç yoksa buyerId ile ilgili kayıt created ediyor devam ediyor.
            if (!created)
            {
                _logger.LogInformation("Problem occur persisting the item");
                return null;
            }
            
            _logger.LogInformation("Basket item persisted succesfully");

            return await GetBasketAsync(basket.BuyerId); //güncelleme set işlemlerinden sonra geriye tekrar getbasketasync ile basketi dönüyoruz
        }
        
        private IServer GetServer()
        {
            var endpoint = _redis.GetEndPoints();
            return _redis.GetServer(endpoint.First());
        }
    }
}
