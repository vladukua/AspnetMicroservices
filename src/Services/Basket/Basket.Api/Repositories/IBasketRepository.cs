using Basket.Api.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Basket.Api.Repositories
{
    public interface IBasketRepository
    {
        Task<ShoppingCart> GetBasketAsync(string userName, CancellationToken cancellationToken);

        Task<ShoppingCart> UpdateBasketAsync(ShoppingCart basket, CancellationToken cancellationToken);

        Task DeleteBasketAsync(string userName, CancellationToken cancellationToken);
    }
}
