using BugStore.Handlers.Customers;
using BugStore.Models;
using BugStore.Requests.Customers;
using BugStore.Test.Repositories;
using BugStore.Test.Services;

namespace BugStore.Test.Handlers.Customers;

[TestClass]
public class GetByIdCustomerHandlerTests
{
    private FakeCustomerService _fakeCustomerService;
    private GetByIdCustomerHandler _handler;
    private Customer _baseCustomer;

    [TestInitialize]
    public void Setup()
    {
        var fakeRepo = new FakeCustomerRepository();
        _fakeCustomerService = new FakeCustomerService(fakeRepo);

        _baseCustomer = new Customer
        {
            Id = Guid.NewGuid(),
            Name = "Jaqueline Manzano",
            Email = "jaqueline_manzano@example.com",
            Phone = "123456789",
            BirthDate = new DateTime(1997, 03, 20)
        };

        _handler = new GetByIdCustomerHandler(_fakeCustomerService);
    }

    [TestMethod]
    [TestCategory("GetByIdCustomerHandler")]
    public async Task Dado_um_cliente_existente_deve_retornar_o_cliente()
    {
        // Arrange
        await _fakeCustomerService.AddCustomerAsync(_baseCustomer);

        var request = new GetByIdCustomerRequest { Id = _baseCustomer.Id };

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.IsNotNull(response.Customer);
        Assert.AreEqual(_baseCustomer.Id, response.Customer!.Id);
    }

    [TestMethod]
    [TestCategory("GetByIdCustomerHandler")]
    public async Task Dado_um_cliente_inexistente_deve_retornar_customer_null()
    {
        // Arrange
        var request = new GetByIdCustomerRequest { Id = Guid.NewGuid() };

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.IsNull(response.Customer);
    }
}