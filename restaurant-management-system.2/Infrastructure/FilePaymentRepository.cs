using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using restaurant_management_system._2.Application.Interface;
using restaurant_management_system._2.Domain.Entities;
using restaurant_management_system._2.Infrastructure.Data;

namespace restaurant_management_system._2.Infrastructure
{
    public class FilePaymentRepository : IPaymentRepository
    {
        private readonly RestaurantDbContext db;

        public FilePaymentRepository(RestaurantDbContext db)
        {
            this.db = db;
        }

        public List<Payment> GetAll()
        {
            return db.Payments
                .Include(p => p.Order)
                .OrderByDescending(p => p.PaidAt)
                .ToList();
        }

        public Payment? GetById(int id)
        {
            return db.Payments
                .Include(p => p.Order)
                .FirstOrDefault(p => p.Id == id);
        }

        public List<Payment> GetByOrderId(int orderId)
        {
            return db.Payments
                .Include(p => p.Order)
                .Where(p => p.OrderId == orderId)
                .OrderByDescending(p => p.PaidAt)
                .ToList();
        }

        public void Add(Payment payment)
        {
            db.Payments.Add(payment);
            db.SaveChanges();
        }
    }
}