using System;
using System.Collections.Generic;
using System.Linq;
using restaurant_management_system._2.Application.Interface;
using restaurant_management_system._2.Domain.Entities;
using restaurant_management_system._2.Domain.Enums;

namespace restaurant_management_system._2.Service
{
    public class PaymentService
    {
        private readonly IPaymentRepository paymentRepository;
        private readonly IOrderRepository orderRepository;

        public PaymentService(
            IPaymentRepository paymentRepository,
            IOrderRepository orderRepository)
        {
            this.paymentRepository = paymentRepository;
            this.orderRepository = orderRepository;
        }

        public Payment RegisterPayment(int orderId, PaymentMethod method)
        {
            Order? order = orderRepository.GetById(orderId);

            if (order == null)
                throw new ArgumentException("Order not found.");

            if (order.Status != OrderStatus.Closed)
                throw new ArgumentException("Only closed orders can be paid.");

            if (order.TotalAmount <= 0)
                throw new ArgumentException("Order total amount must be greater than 0.");

            bool alreadyPaid = paymentRepository
                .GetByOrderId(orderId)
                .Any();

            if (alreadyPaid)
                throw new ArgumentException("This order is already paid.");

            Payment payment = new Payment
            {
                OrderId = orderId,
                Amount = order.TotalAmount,
                PaidAt = DateTime.Now,
                Method = method
            };

            paymentRepository.Add(payment);

            return payment;
        }

        public List<Payment> GetAllPayments()
        {
            return paymentRepository.GetAll()
                .OrderByDescending(p => p.PaidAt)
                .ToList();
        }

        public List<Payment> GetPaymentsForOrder(int orderId)
        {
            return paymentRepository.GetByOrderId(orderId);
        }
        public List<Order> GetOrdersAvailableForPayment()
        {
            return orderRepository.GetAll()
                .Where(o =>
                    o.Status == OrderStatus.Closed &&
                    o.TotalAmount > 0 &&
                    !paymentRepository.GetByOrderId(o.Id).Any())
                .OrderBy(o => o.CreatedAt)
                .ToList();
        }

        public List<Order> GetClosedOrders()
        {
            return orderRepository.GetAll()
                .Where(o => o.Status == OrderStatus.Closed)
                .OrderByDescending(o => o.CreatedAt)
                .ToList();
        }
    }
}