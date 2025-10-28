using BugStore.Handlers.Customers;
using BugStore.Requests.Customers;
using BugStore.Test.Repositories;
using BugStore.Test.Services;

namespace BugStore.Test.Handlers.Customers;

[TestClass]
public class CreateCustomerHandlerTests
{
    private FakeCustomerService _fakeCustomerService;
    private CreateCustomerHandler _handler;
    private CreateCustomerRequest _baseCustomerRequest;

    [TestInitialize]
    public void Setup()
    {
        var fakeRepo = new FakeCustomerRepository();
        _fakeCustomerService = new FakeCustomerService(fakeRepo);

        _baseCustomerRequest = new CreateCustomerRequest
        {
            Name = "Jaqueline Manzano",
            Email = "jaqueline@example.com",
            Phone = "123456789",
            BirthDate = new DateTime(1997, 03, 20)
        };

        _handler = new CreateCustomerHandler(_fakeCustomerService);
    }

    [TestMethod]
    [TestCategory("CreateCustomerHandler")]
    public async Task Dado_um_request_valido_deve_criar_um_cliente()
    {
        // Act
        var response = await _handler.Handle(_baseCustomerRequest, CancellationToken.None);

        // Assert
        Assert.IsNotNull(response.Customer);
    }
}