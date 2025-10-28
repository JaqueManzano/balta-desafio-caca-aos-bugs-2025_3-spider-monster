using BugStore.Handlers.Customers;
using BugStore.Models;
using BugStore.Requests.Customers;
using BugStore.Test.Repositories;
using BugStore.Test.Services;

namespace BugStore.Test.Handlers.Customers;

[TestClass]
public class UpdateCustomerHandlerTests
{
    private FakeCustomerService _fakeCustomerService = null!;
    private UpdateCustomerHandler _handler = null!;

    [TestInitialize]
    public void Setup()
    {
        var fakeRepo = new FakeCustomerRepository();
        _fakeCustomerService = new FakeCustomerService(fakeRepo);
        _handler = new UpdateCustomerHandler(_fakeCustomerService);
    }

    [TestMethod]
    [TestCategory("UpdateCustomerHandler")]
    public async Task Dado_um_cliente_existente_deve_atualizar_com_sucesso()
    {
        // Arrange
        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            Name = "Jaqueline",
            Email = "jaqueline@example.com",
            Phone = "123456789"
        };
        await _fakeCustomerService.AddCustomerAsync(customer);

        var request = new UpdateCustomerRequest
        {
            Id = customer.Id,
            Name = "Jaqueline Santos",
            Email = "jaqueline.santos@example.com",
            Phone = "987654321",
            BirthDate = new DateTime(1997, 03, 20)
        };

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.IsTrue(response.Success);
        Assert.IsNotNull(response.Customer);
        Assert.AreEqual(request.Name, response.Customer!.Name);
        Assert.AreEqual(request.Email, response.Customer.Email);
        Assert.AreEqual(request.Phone, response.Customer.Phone);
    }

    [TestMethod]
    [TestCategory("UpdateCustomerHandler")]
    public async Task Dado_um_cliente_inexistente_nao_deve_retornar_sucesso()
    {
        // Arrange
        var request = new UpdateCustomerRequest
        {
            Id = Guid.NewGuid(),
            Name = "Cliente Inexistente"
        };

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.IsFalse(response.Success);
        Assert.IsNull(response.Customer);
    }
}
