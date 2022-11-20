using System.Linq.Expressions;
using EFCoreMethodsInQueries.Domain;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace EFCoreMethodsInQueries.Persistence;

public class OrderRepository
{
    private readonly DbSet<Order> _orders;

    public OrderRepository(OrderContext orderContext)
    {
        _orders = orderContext.Orders;
    }

    private static Expression<Func<Order, bool>> IsInState(OrderState state)
    {
        return o => o.State == state;
    }

    private static Expression<Func<Order, bool>> IsOlderThan(TimeSpan age)
    {
        return o => o.OrderDate < DateTime.Now.AddMilliseconds(-age.TotalMilliseconds);
    }

    public IEnumerable<Order> GetOrdersForReprocessingWithLinq()
    {
        return _orders.Where(o =>
                // take orders that are in state New and older than 1h
                (o.State == OrderState.New && o.OrderDate < DateTime.Now.AddHours(-1))
                // or orders that are in state InProgress and older than 12h
                || (o.State == OrderState.InProgress && o.OrderDate < DateTime.Now.AddHours(-12)))
            .ToList();
    }

    public IEnumerable<Order> GetOrdersForReprocessingWithEntityMethod()
    {
        return _orders.Where(o => o.QualifiesForReprocessing())
            .ToList();
    }

    public IEnumerable<Order> GetOrdersForReprocessingWithLinqKit()
    {
        Expression<Func<Order, bool>> qualifiesForNewOrderReprocessing =
            o => o.State == OrderState.New && o.OrderDate < DateTime.Now.AddHours(-1);
        Expression<Func<Order, bool>> qualifiesForInProgressOrderReprocessing =
            o => o.State == OrderState.InProgress && o.OrderDate < DateTime.Now.AddHours(-12);

        var predicate = PredicateBuilder.New<Order>()
            .Or(qualifiesForNewOrderReprocessing)
            .Or(qualifiesForInProgressOrderReprocessing);

        return _orders.Where(predicate)
            .ToList();
    }

    public IEnumerable<Order> GetOrdersForReprocessingWithClearerLinqKit()
    {
        var predicate = PredicateBuilder.New<Order>()
            .Or(IsInState(OrderState.New).And(IsOlderThan(TimeSpan.FromHours(1))))
            .Or(IsInState(OrderState.InProgress).And(IsOlderThan(TimeSpan.FromHours(12))));

        return _orders.Where(predicate)
            .ToList();
    }
}