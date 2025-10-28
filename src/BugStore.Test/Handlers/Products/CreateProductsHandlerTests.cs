using BugStore.Handlers.Products;
using BugStore.Requests.Products;
using BugStore.Test.Repositories;
using BugStore.Test.Services;

namespace BugStore.Test.Handlers.Products
{
    [TestClass]
    public class CreateProductsHandlerTests
    {
        private FakeProductsService _fakeProductsService;
        private CreateProductsHandler _handler;

        [TestInitialize]
        public void Setup()
        {
            var fakeRepo = new FakeProductsRepository();
            _fakeProductsService = new FakeProductsService(fakeRepo);

            _handler = new CreateProductsHandler(_fakeProductsService);
        }

        [TestMethod]
        [TestCategory("CreateProductsHandler")]
        public async Task Dado_um_request_valido_deve_criar_um_produto()
        {
            // Arrange
            var request = new CreateProductsRequest
            {
                Title = "Produto Desafio Balta",
                Description = "Descrição do Produto Teste desafio Balta",
                Slug = "produto-desafio-balta",
                Price = 99.99m
            };

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.IsNotNull(response.Product);
        }
    }
}
