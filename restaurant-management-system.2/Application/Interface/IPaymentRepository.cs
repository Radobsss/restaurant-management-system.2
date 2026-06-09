using restaurant_management_system._2.Domain.Entities;

namespace restaurant_management_system._2.Application.Interface
{
    public interface IPaymentRepository
    {
        List<Payment> GetAll();

        Payment? GetById(int id);

        List<Payment> GetByOrderId(int orderId);

        void Add(Payment payment);
    }
}