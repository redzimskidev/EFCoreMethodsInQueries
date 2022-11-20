using EFCoreMethodsInQueries.Domain;

namespace EFCoreMethodsInQueries.Persistence.IntegrationTests;

public static class OrderContextExtensions
{
    public static void Seed(this OrderContext context)
    {
        context.Orders.Add(new Order(DateTime.Now.AddDays(-1), OrderState.Completed));
        context.Orders.Add(new Order(DateTime.Now.AddDays(-1), OrderState.InProgress));
        context.Orders.Add(new Order(DateTime.Now.AddHours(-2), OrderState.New));
        context.Orders.Add(new Order(DateTime.Now, OrderState.New));

        context.SaveChanges();
    }
}