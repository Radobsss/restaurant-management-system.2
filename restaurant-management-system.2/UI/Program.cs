using restaurant_management_system._2.Application.Interface;
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

            ITableRepository tableRepository = new FileTableRepository(db);
            IReservationRepository reservationRepository = new FileReservationRepository(db);

            TableService tableService = new TableService(tableRepository);

            ReservationService reservationService = new ReservationService(
                reservationRepository,
                tableRepository);

            RestaurantUI ui = new RestaurantUI(tableService, reservationService);

            ui.Run();
        }
    }
}