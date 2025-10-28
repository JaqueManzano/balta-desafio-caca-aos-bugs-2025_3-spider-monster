using BugStore.Handlers.Products;
using BugStore.Models;
using BugStore.Requests.Products;
using BugStore.Test.Repositories;
using BugStore.Test.Services;

namespace BugStore.Test.Handlers.Products
{
    [TestClass]
    public class GetByIdProductHandlerTests
    {
        private FakeProductsService _fakeProductsService;
        private GetByIdProductHandler _handler;

        [TestInitialize]
        public void Setup()
        {
            var fakeRepo = new FakeProductsRepository();
            _fakeProductsService = new FakeProductsService(fakeRepo);

            _handler = new GetByIdProductHandler(_fakeProductsService);
        }

        [TestMethod]
        [TestCategory("GetByIdProductHandler")]
        public async Task Dado_um_produto_existente_deve_retornar_o_produto()
        {
            // Arrange
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Title = "Produto Desafio Balta",
                Description = "Descrição do Produto Teste desafio Balta",
                Slug = "produto-desafio-balta",
                Price = 99.99m
            };

            await _fakeProductsService.AddProductAsync(product);

            var request = new GetByIdProductsRequest
            {
                Id = product.Id
            };

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.IsNotNull(response.Product);
        }

        [TestMethod]
        [TestCategory("GetByIdProductHandler")]
        public async Task Dado_um_produto_inexistente_deve_retornar_nulo()
        {
            // Arrange
            var request = new GetByIdProductsRequest
            {
                Id = Guid.NewGuid() 
            };

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.IsNull(response.Product);
        }
    }
}
