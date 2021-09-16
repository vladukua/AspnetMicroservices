using Basket.Api.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Basket.Api.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _redisCache;

        public BasketRepository(IDistributedCache redisCache)
        {
            this._redisCache = redisCache ?? throw new ArgumentNullException(nameof(redisCache));
        }

        public async Task DeleteBasketAsync(string userName, CancellationToken cancellationToken)
        {
            if (userName == null)
            {
                throw new ArgumentNullException(nameof(userName));
            }

            await _redisCache.RemoveAsync(userName, cancellationToken);
        }

        public async Task<ShoppingCart> GetBasketAsync(string userName, CancellationToken cancellationToken)
        {
            if (userName == null)
            {
                throw new ArgumentNullException(nameof(userName));
            }

            var basket = await _redisCache.GetStringAsync(userName, cancellationToken);
            
            if (string.IsNullOrEmpty(basket))
            {
                return null;
            }

            return JsonConvert.DeserializeObject<ShoppingCart>(basket);
        }

        public async Task<ShoppingCart> UpdateBasketAsync(ShoppingCart basket, CancellationToken cancellationToken)
        {
            if (basket == null)
            {
                throw new ArgumentNullException(nameof(basket));
            }

            await _redisCache.SetStringAsync(basket.UserName, JsonConvert.SerializeObject(basket), cancellationToken);

            return await GetBasketAsync(basket.UserName, cancellationToken);
        }
    }
}
