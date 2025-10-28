using BugStore.Handlers.Orders;
using BugStore.Models;
using BugStore.Requests.Orders;
using BugStore.Test.Repositories;
using BugStore.Test.Services;

namespace BugStore.Test.Handlers.Orders
{
    [TestClass]
    public class GetByIdOrdersHandlerTests
    {
        private FakeOrderRepository _fakeOrderRepository;
        private FakeOrderService _fakeOrderService;
        private GetByIdOrdersHandler _handler;
        private Guid _existingOrderId;

        [TestInitialize]
        public void Setup()
        {
            _fakeOrderRepository = new FakeOrderRepository();
            _fakeOrderService = new FakeOrderService(_fakeOrderRepository);

            _existingOrderId = Guid.NewGuid();
            Order? fakeOrder = new Order
            {
                Id = _existingOrderId,
                CustomerId = Guid.NewGuid(),
                Lines = new List<OrderLine>
                {
                    new OrderLine { ProductId = Guid.NewGuid(), Quantity = 2 },
                    new OrderLine { ProductId = Guid.NewGuid(), Quantity = 1 }
                }
            };

            _fakeOrderRepository.AddAsync(fakeOrder, CancellationToken.None).Wait();

            _handler = new GetByIdOrdersHandler(_fakeOrderService);
        }

        [TestMethod]
        [TestCategory("GetByIdOrdersHandler")]
        public async Task Dado_um_pedido_existente_deve_retornar_o_pedido()
        {
            // Arrange
            var request = new GetByIdOrdersRequest { Id = _existingOrderId };

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.IsNotNull(response?.Order);
        }

        [TestMethod]
        [TestCategory("GetByIdOrdersHandler")]
        public async Task Dado_um_pedido_inexistente_deve_retornar_null()
        {
            // Arrange
            var request = new GetByIdOrdersRequest { Id = Guid.NewGuid() };

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.IsNull(response.Order);
        }
    }
}
