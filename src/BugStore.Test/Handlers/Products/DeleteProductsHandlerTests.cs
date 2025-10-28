using BugStore.Handlers.Products;
using BugStore.Models;
using BugStore.Requests.Products;
using BugStore.Test.Repositories;
using BugStore.Test.Services;

namespace BugStore.Test.Handlers.Products
{
    [TestClass]
    public class DeleteProductsHandlerTests
    {
        private FakeProductsService _fakeProductsService;
        private DeleteProductsHandler _handler;

        [TestInitialize]
        public void Setup()
        {
            var fakeRepo = new FakeProductsRepository();
            _fakeProductsService = new FakeProductsService(fakeRepo);

            _handler = new DeleteProductsHandler(_fakeProductsService);
        }

        [TestMethod]
        [TestCategory("DeleteProductsHandler")]
        public async Task Dado_um_produto_existente_deve_deletar_o_produto()
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

            var request = new DeleteProductsRequest
            {
                Id = product.Id
            };

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.IsTrue(response.Success);
        }

        [TestMethod]
        [TestCategory("DeleteProductsHandler")]
        public async Task Dado_um_produto_inexistente_deve_retornar_sem_sucesso()
        {
            // Arrange
            var request = new DeleteProductsRequest
            {
                Id = Guid.NewGuid()
            };

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.IsFalse(response.Success);
        }
    }
}
