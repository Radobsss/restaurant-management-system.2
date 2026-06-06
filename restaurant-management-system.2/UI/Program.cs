using System.Linq;
using restaurant_management_system._2.Application.Interface;
using restaurant_management_system._2.Domain.Entities;
using restaurant_management_system._2.Infrastructure;
using restaurant_management_system._2.Infrastructure.Data;
using restaurant_management_system._2.Service;

namespace restaurant_management_system._2.UI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            RestaurantDbContext db = new RestaurantDbContext();

            SeedTables(db);

            ITableRepository tableRepository = new FileTableRepository(db);
            IReservationRepository reservationRepository = new FileReservationRepository(db);

            TableService tableService = new TableService(tableRepository);

            ReservationService reservationService = new ReservationService(
                reservationRepository,
                tableRepository);

            RestaurantUI ui = new RestaurantUI(tableService, reservationService);

            ui.Run();
        }

        static void SeedTables(RestaurantDbContext db)
        {
            if (db.Tables.Any())
                return;

            db.Tables.AddRange(
                new Table { Number = 1, Capacity = 2, Location = "Window", IsOccupied = false, IsReserved = true, ReservedBy = "Pesho" },
                new Table { Number = 2, Capacity = 2, Location = "Window", IsOccupied = false, IsReserved = false, ReservedBy = null },
                new Table { Number = 3, Capacity = 4, Location = "Main hall", IsOccupied = true, IsReserved = false, ReservedBy = null },
                new Table { Number = 4, Capacity = 4, Location = "Main hall", IsOccupied = false, IsReserved = false, ReservedBy = null },
                new Table { Number = 5, Capacity = 6, Location = "Terrace", IsOccupied = true, IsReserved = false, ReservedBy = null },
                new Table { Number = 6, Capacity = 6, Location = "Terrace", IsOccupied = false, IsReserved = false, ReservedBy = null },
                new Table { Number = 7, Capacity = 8, Location = "VIP area", IsOccupied = true, IsReserved = false, ReservedBy = null },
                new Table { Number = 8, Capacity = 8, Location = "VIP area", IsOccupied = false, IsReserved = false, ReservedBy = null },
                new Table { Number = 9, Capacity = 2, Location = "Garden", IsOccupied = false, IsReserved = true, ReservedBy = "Ivan Ivanov" },
                new Table { Number = 10, Capacity = 2, Location = "Garden", IsOccupied = false, IsReserved = false, ReservedBy = null },
                new Table { Number = 11, Capacity = 4, Location = "Main hall", IsOccupied = false, IsReserved = false, ReservedBy = null },
                new Table { Number = 12, Capacity = 4, Location = "Main hall", IsOccupied = false, IsReserved = false, ReservedBy = null },
                new Table { Number = 13, Capacity = 6, Location = "Terrace", IsOccupied = false, IsReserved = false, ReservedBy = null },
                new Table { Number = 14, Capacity = 6, Location = "Terrace", IsOccupied = false, IsReserved = false, ReservedBy = null },
                new Table { Number = 15, Capacity = 8, Location = "VIP area", IsOccupied = false, IsReserved = false, ReservedBy = null },
                new Table { Number = 16, Capacity = 8, Location = "VIP area", IsOccupied = true, IsReserved = false, ReservedBy = null },
                new Table { Number = 17, Capacity = 2, Location = "Window", IsOccupied = false, IsReserved = false, ReservedBy = null },
                new Table { Number = 18, Capacity = 4, Location = "Garden", IsOccupied = false, IsReserved = false, ReservedBy = null },
                new Table { Number = 19, Capacity = 6, Location = "Main hall", IsOccupied = true, IsReserved = false, ReservedBy = null },
                new Table { Number = 20, Capacity = 10, Location = "VIP area", IsOccupied = false, IsReserved = true, ReservedBy = "Riana" }
            );

            db.SaveChanges();
        }
    }
}