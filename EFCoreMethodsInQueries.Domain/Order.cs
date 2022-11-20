namespace EFCoreMethodsInQueries.Domain;

public class Order
{
    public Order()
    {
    }

    public Order(DateTime orderDate, OrderState state)
    {
        Id = Guid.NewGuid();
        OrderDate = orderDate;
        State = state;
    }

    public Order(Guid id, DateTime orderDate, OrderState state)
    {
        Id = id;
        OrderDate = orderDate;
        State = state;
    }

    public Guid Id { get; }
    public DateTime OrderDate { get; }
    public OrderState State { get; }

    public bool QualifiesForReprocessing()
    {
        return QualifiesForNewOrderReprocessing() || QualifiesForInProgressOrderReprocessing();
    }

    private bool QualifiesForNewOrderReprocessing()
    {
        return State == OrderState.New && DateTime.Now.AddHours(-1) < OrderDate;
    }

    private bool QualifiesForInProgressOrderReprocessing()
    {
        return State == OrderState.InProgress && DateTime.Now.AddHours(-12) < OrderDate;
    }
}