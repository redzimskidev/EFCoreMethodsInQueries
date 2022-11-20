using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EFCoreMethodsInQueries.Persistence.IntegrationTests;

public class OrderRepositoryTests : IAsyncLifetime
{
    private OrderContext _orderContext;
    private OrderRepository _orderRepository;
    private MsSqlTestcontainer _sqlContainer;

    public async Task InitializeAsync()
    {
        _sqlContainer = new TestcontainersBuilder<MsSqlTestcontainer>()
            .WithDatabase(new MsSqlTestcontainerConfiguration
            {
                Password = "Your_password123"
            })
            .Build();

        await _sqlContainer.StartAsync();

        var optionsBuilder = new DbContextOptionsBuilder<OrderContext>();
        optionsBuilder.UseSqlServer($"{_sqlContainer.ConnectionString}TrustServerCertificate=true;");

        _orderContext = new OrderContext(optionsBuilder.Options);
        await _orderContext.Database.EnsureCreatedAsync();
        await _orderContext.Database.MigrateAsync();
        _orderContext.Seed();

        _orderRepository = new OrderRepository(_orderContext);
    }

    public async Task DisposeAsync()
    {
        await _sqlContainer.StopAsync();
        await _orderContext.DisposeAsync();
    }

    [Fact]
    public void GetOrdersForReprocessingWithLinq_ReturnsOrders()
    {
        var ordersToReprocess = _orderRepository.GetOrdersForReprocessingWithLinq();

        Assert.Equal(2, ordersToReprocess.Count());
    }

    [Fact]
    public void GetOrdersForReprocessingWithEntityMethod_ReturnsOrders()
    {
        var ordersToReprocess = _orderRepository.GetOrdersForReprocessingWithEntityMethod();

        Assert.Equal(2, ordersToReprocess.Count());
    }

    [Fact]
    public void GetOrdersForReprocessingWithLinqKit_ReturnsOrders()
    {
        var ordersToReprocess = _orderRepository.GetOrdersForReprocessingWithLinqKit();

        Assert.Equal(2, ordersToReprocess.Count());
    }

    [Fact]
    public void GetOrdersForReprocessingWithClearerLinqKit_ReturnsOrders()
    {
        var ordersToReprocess = _orderRepository.GetOrdersForReprocessingWithClearerLinqKit();

        Assert.Equal(2, ordersToReprocess.Count());
    }
}