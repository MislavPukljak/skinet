namespace Core.Entities.OrderAggregate;

public enum OrderStatus
{
    Pading,
    PaymentReceived,
    PaymentFailed,
    PaymentMismatch
}
