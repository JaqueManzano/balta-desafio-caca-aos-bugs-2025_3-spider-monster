using BugStore.Requests.Products;
using BugStore.Test.Repositories;
using BugStore.Test.Services;

namespace BugStore.Test.Handlers.Products
{
    [TestClass]
    public class UpdateProductHandlerTests
    {
        private FakeProductsService _fakeProductsService;
        private UpdateProductHandler _handler;

        [TestInitialize]
        public void Setup()
        {
            var fakeRepo = new FakeProductsRepository();
            _fakeProductsService = new FakeProductsService(fakeRepo);

            _handler = new UpdateProductHandler(_fakeProductsService);
        }

        [TestMethod]
        [TestCategory("UpdateProductHandler")]
        public async Task Dado_um_produto_existente_deve_atualizar_o_produto()
        {
            // Arrange
            var product = new BugStore.Models.Product
            {
                Id = Guid.NewGuid(),
                Title = "Produto Desafio Balta",
                Description = "Descrição do Produto Teste desafio Balta",
                Slug = "produto-desafio-balta",
                Price = 99.99m
            };

            await _fakeProductsService.AddProductAsync(product);

            var request = new UpdateProductsRequest
            {
                Id = product.Id,
                Slug = "produto-desafio-balta-atualizado",
                Price = 150m
            };

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.IsNotNull(response.Product);
            Assert.AreEqual(response.Product.Slug, request.Slug);
            Assert.IsTrue(response.Success);
        }

        [TestMethod]
        [TestCategory("UpdateProductHandler")]
        public async Task Dado_um_produto_inexistente_deve_retornar_sem_sucesso()
        {
            // Arrange
            var request = new UpdateProductsRequest
            {
                Id = Guid.NewGuid(),
                Slug = "produto-inexistente",
                Price = 200m
            };

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.IsNull(response.Product);
            Assert.IsFalse(response.Success);
        }
    }
}
