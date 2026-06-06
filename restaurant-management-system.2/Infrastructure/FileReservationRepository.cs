using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using restaurant_management_system._2.Application.Interface;
using restaurant_management_system._2.Domain.Entities;
using restaurant_management_system._2.Infrastructure.Data;

namespace restaurant_management_system._2.Infrastructure
{
    public class FileReservationRepository : IReservationRepository
    {
        private readonly RestaurantDbContext db;

        public FileReservationRepository(RestaurantDbContext db)
        {
            this.db = db;
        }

        public List<Reservation> GetAll()
        {
            return db.Reservations
                .Include(r => r.Table)
                .OrderBy(r => r.StartTime)
                .ToList();
        }

        public Reservation? GetById(int id)
        {
            return db.Reservations
                .Include(r => r.Table)
                .FirstOrDefault(r => r.Id == id);
        }

        public List<Reservation> GetByTableId(int tableId)
        {
            return db.Reservations
                .Where(r => r.TableId == tableId)
                .OrderBy(r => r.StartTime)
                .ToList();
        }

        public void Add(Reservation reservation)
        {
            db.Reservations.Add(reservation);
            db.SaveChanges();
        }

        public void Update(Reservation reservation)
        {
            db.Reservations.Update(reservation);
            db.SaveChanges();
        }
    }
}