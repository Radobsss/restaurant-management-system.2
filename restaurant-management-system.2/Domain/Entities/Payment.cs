using System;
using restaurant_management_system._2.Domain.Enums;

namespace restaurant_management_system._2.Domain.Entities
{
    public class Payment
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        public Order? Order { get; set; }

        public decimal Amount { get; set; }

        public DateTime PaidAt { get; set; }

        public PaymentMethod Method { get; set; }
    }
}