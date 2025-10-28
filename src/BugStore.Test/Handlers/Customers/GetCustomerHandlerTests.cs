using BugStore.Handlers.Customers;
using BugStore.Requests.Customers;
using BugStore.Test.Repositories;
using BugStore.Test.Services;

namespace BugStore.Test.Handlers.Customers;

[TestClass]
public class GetCustomerHandlerTests
{
    private FakeCustomerService _fakeCustomerService = null!;
    private GetCustomerHandler _handler = null!;

    [TestInitialize]
    public void Setup()
    {
        var fakeRepo = new FakeCustomerRepository();
        _fakeCustomerService = new FakeCustomerService(fakeRepo);
        _handler = new GetCustomerHandler(_fakeCustomerService);
    }

    [TestMethod]
    [TestCategory("GetCustomerHandler")]
    public async Task Dado_clientes_existentes_deve_retornar_uma_lista()
    {
        // Arrange
        var customer1 = new BugStore.Models.Customer
        {
            Id = Guid.NewGuid(),
            Name = "Jaqueline",
            Email = "jaqueline@example.com"
        };
        var customer2 = new BugStore.Models.Customer
        {
            Id = Guid.NewGuid(),
            Name = "Denise",
            Email = "denise@example.com"
        };

        await _fakeCustomerService.AddCustomerAsync(customer1);
        await _fakeCustomerService.AddCustomerAsync(customer2);

        var request = new GetCustomerRequest();

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.IsTrue(response.Success);
        Assert.AreEqual(2, response.Customers.Count);
    }

    [TestMethod]
    [TestCategory("GetCustomerHandler")]
    public async Task Dado_lista_vazia_deve_retornar_sucesso_com_lista_vazia()
    {
        // Arrange
        var request = new GetCustomerRequest();

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.IsTrue(response.Success);
        Assert.AreEqual(0, response.Customers.Count);
    }
}
