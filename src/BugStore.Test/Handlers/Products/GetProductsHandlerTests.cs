using BugStore.Handlers.Products;
using BugStore.Models;
using BugStore.Requests.Products;
using BugStore.Test.Repositories;
using BugStore.Test.Services;

namespace BugStore.Test.Handlers.Products
{
    [TestClass]
    public class GetProductsHandlerTests
    {
        private FakeProductsService _fakeProductsService;
        private GetProductsHandler _handler;

        [TestInitialize]
        public void Setup()
        {
            var fakeRepo = new FakeProductsRepository();
            _fakeProductsService = new FakeProductsService(fakeRepo);

            _handler = new GetProductsHandler(_fakeProductsService);
        }

        [TestMethod]
        [TestCategory("GetProductsHandler")]
        public async Task Dado_existirem_produtos_deve_retornar_todos_produtos()
        {
            // Arrange
            var product1 = new Product
            {
                Id = Guid.NewGuid(),
                Title = "Produto 1 Desafio Balta",
                Description = "Descrição do Produto 1 desafio Balta",
                Slug = "produto1-desafio-balta",
                Price = 50m
            };

            var product2 = new BugStore.Models.Product
            {
                Id = Guid.NewGuid(),
                Title = "Produto 2 Desafio Balta",
                Description = "Descrição do Produto 2 desafio Balta",
                Slug = "produto2-desafio-balta",
                Price = 100m
            };

            await _fakeProductsService.AddProductAsync(product1);
            await _fakeProductsService.AddProductAsync(product2);

            var request = new GetProductsRequest();

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.IsNotNull(response.Products);
            Assert.AreEqual(2, response.Products.Count);
            CollectionAssert.Contains(response.Products, product1);
            CollectionAssert.Contains(response.Products, product2);
        }

        [TestMethod]
        [TestCategory("GetProductsHandler")]
        public async Task Dado_nao_existirem_produtos_deve_retornar_lista_vazia()
        {
            // Arrange
            var request = new GetProductsRequest();

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.IsNotNull(response.Products);
            Assert.AreEqual(0, response.Products.Count);
        }
    }
}
