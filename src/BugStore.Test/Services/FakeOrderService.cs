using BugStore.Models;
using BugStore.Requests.Orders;
using BugStore.Responses.Orders;
using BugStore.Services.Interfaces;
using BugStore.Test.Repositories;

namespace BugStore.Test.Services
{
    public class FakeOrderService : IOrderService
    {
        private readonly FakeOrderRepository _repo;

        public FakeOrderService(FakeOrderRepository repo)
        {
            _repo = repo;
        }

        public async Task<CreateOrdersResponse> CreateOrderAsync(CreateOrdersRequest request, CancellationToken cancellationToken)
        {
            var order = new Order
            {
                Id = Guid.NewGuid(),
                CustomerId = request.CustomerId,
                Lines = request.Lines.Select(l => new OrderLine
                {
                    ProductId = l.ProductId,
                    Quantity = l.Quantity
                }).ToList()
            };

            await _repo.AddAsync(order, cancellationToken);

            return new CreateOrdersResponse
            {
                Order = order,
                Success = true,
                Message = "Order created."
            };
        }

        public Task AddOrderAsync(Order order)
        {
            return _repo.AddAsync(order, CancellationToken.None);
        }

        public async Task<GetByIdOrdersResponse> GetOrderByIdAsync(GetByIdOrdersRequest request, CancellationToken cancellationToken)
        {
            var order = await _repo.GetByIdAsync(request.Id, cancellationToken);

            return new GetByIdOrdersResponse
            {
                Order = order,
                Message = string.Empty
            };
        }

    }
}
