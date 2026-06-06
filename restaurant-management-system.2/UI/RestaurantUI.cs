using restaurant_management_system._2.Domain.Entities;
using restaurant_management_system._2.Service;
using System.Globalization;

namespace restaurant_management_system._2.UI
{
    internal class RestaurantUI
    {

        private readonly TableService tableService;
        private readonly ReservationService reservationService;

        public RestaurantUI(TableService tableService, ReservationService reservationService)
        {
            this.tableService = tableService;
            this.reservationService = reservationService;
        }

        public void Run()
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("======================================");
                Console.WriteLine("     RESTAURANT MANAGEMENT SYSTEM");
                Console.WriteLine("======================================");
                Console.WriteLine("1. Table management");
                Console.WriteLine("2. Menu management");
                Console.WriteLine("3. Order management");
                Console.WriteLine("4. Reservation management");
                Console.WriteLine("5. Payment management");
                Console.WriteLine("6. Reports");
                Console.WriteLine("0. Exit");
                Console.WriteLine("======================================");

                Console.Write("Choose option: ");
                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        TableManagementMenu();
                        break;

                    case "2":
                        ComingSoon("Menu management");
                        break;

                    case "3":
                        ComingSoon("Order management");
                        break;

                    case "4":
                        ReservationManagementMenu();
                        break;

                    case "5":
                        ComingSoon("Payment management");
                        break;

                    case "6":
                        ComingSoon("Reports");
                        break;

                    case "0":
                        return;

                    default:
                        Console.WriteLine("Invalid option.");
                        Pause();
                        break;
                }
            }
        }

        private void TableManagementMenu()
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("======================================");
                Console.WriteLine("          TABLE MANAGEMENT");
                Console.WriteLine("======================================");
                Console.WriteLine("1. Show all tables");
                Console.WriteLine("2. Show free tables");
                Console.WriteLine("3. Reserve table");
                Console.WriteLine("4. Occupy table");
                Console.WriteLine("5. Free table");
                Console.WriteLine("6. Cancel table reservation");
                Console.WriteLine("0. Back");
                Console.WriteLine("======================================");

                Console.Write("Choose option: ");
                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ShowAllTablesUI();
                        break;

                    case "2":
                        ShowFreeTablesUI();
                        break;

                    case "3":
                        ReserveTableUI();
                        break;

                    case "4":
                        OccupyTableUI();
                        break;

                    case "5":
                        FreeTableUI();
                        break;

                    case "6":
                        CancelTableReservationUI();
                        break;

                    case "0":
                        return;

                    default:
                        Console.WriteLine("Invalid option.");
                        Pause();
                        break;
                }
            }
        }

        private void ReservationManagementMenu()
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("======================================");
                Console.WriteLine("       RESERVATION MANAGEMENT");
                Console.WriteLine("======================================");
                Console.WriteLine("1. Create reservation");
                Console.WriteLine("2. Cancel reservation");
                Console.WriteLine("3. Show all reservations");
                Console.WriteLine("4. Show reservations by table");
                Console.WriteLine("0. Back");
                Console.WriteLine("======================================");

                Console.Write("Choose option: ");
                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        CreateReservationUI();
                        break;

                    case "2":
                        CancelReservationUI();
                        break;

                    case "3":
                        ShowAllReservationsUI();
                        break;

                    case "4":
                        ShowReservationsByTableUI();
                        break;

                    case "0":
                        return;

                    default:
                        Console.WriteLine("Invalid option.");
                        Pause();
                        break;
                }
            }
        }

        private void ShowAllTablesUI()
        {
            Console.WriteLine();
            Console.WriteLine("======================================");
            Console.WriteLine("              ALL TABLES");
            Console.WriteLine("======================================");

            List<Table> tables = tableService.GetAllTables();

            if (tables.Count == 0)
            {
                Console.WriteLine("No tables found.");
                Pause();
                return;
            }

            PrintTables(tables);
            Pause();
        }

        private void ShowFreeTablesUI()
        {
            Console.WriteLine();
            Console.WriteLine("======================================");
            Console.WriteLine("              FREE TABLES");
            Console.WriteLine("======================================");

            List<Table> tables = tableService.GetFreeTables();

            if (tables.Count == 0)
            {
                Console.WriteLine("No free tables found.");
                Pause();
                return;
            }

            PrintTables(tables);
            Pause();
        }

        private void ReserveTableUI()
        {
            Console.WriteLine();
            Console.WriteLine("======================================");
            Console.WriteLine("             RESERVE TABLE");
            Console.WriteLine("======================================");

            List<Table> reservableTables = tableService.GetReservableTables();

            if (reservableTables.Count == 0)
            {
                Console.WriteLine("No free tables available for reservation.");
                Pause();
                return;
            }

            Console.WriteLine("Tables available for reservation:");
            PrintTables(reservableTables);

            try
            {
                int tableNumber = ReadInt("Table number: ");

                Console.Write("Reservation name: ");
                string reservedBy = Console.ReadLine()!;

                Table table = tableService.ReserveTable(tableNumber, reservedBy);

                Console.WriteLine();
                Console.WriteLine($"Table {table.Number} reserved successfully for {table.ReservedBy}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine("Error: " + ex.Message);
            }

            Pause();
        }

        private void OccupyTableUI()
        {
            Console.WriteLine();
            Console.WriteLine("======================================");
            Console.WriteLine("              OCCUPY TABLE");
            Console.WriteLine("======================================");

            List<Table> availableTables = tableService.GetTablesAvailableForOccupy();

            if (availableTables.Count == 0)
            {
                Console.WriteLine("No free or reserved tables available.");
                Pause();
                return;
            }

            Console.WriteLine("Tables available for occupying:");
            PrintTables(availableTables);

            try
            {
                int tableNumber = ReadInt("Table number: ");

                Table selectedTable = availableTables
                    .FirstOrDefault(t => t.Number == tableNumber)
                    ?? throw new ArgumentException("This table is not available for occupying.");

                string? reservationName = null;

                if (selectedTable.IsReserved)
                {
                    Console.Write("Reservation name: ");
                    reservationName = Console.ReadLine();
                }

                Table table = tableService.OccupyTable(tableNumber, reservationName);

                Console.WriteLine();
                Console.WriteLine($"Table {table.Number} occupied successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine("Error: " + ex.Message);
            }

            Pause();
        }

        private void FreeTableUI()
        {
            Console.WriteLine();
            Console.WriteLine("======================================");
            Console.WriteLine("               FREE TABLE");
            Console.WriteLine("======================================");

            List<Table> occupiedTables = tableService.GetOccupiedTables();

            if (occupiedTables.Count == 0)
            {
                Console.WriteLine("No occupied tables found.");
                Pause();
                return;
            }

            Console.WriteLine("Occupied tables:");
            PrintTables(occupiedTables);

            try
            {
                int tableNumber = ReadInt("Table number: ");

                Table table = tableService.FreeTable(tableNumber);

                Console.WriteLine();
                Console.WriteLine($"Table {table.Number} freed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine("Error: " + ex.Message);
            }

            Pause();
        }

        private void CancelTableReservationUI()
        {
            Console.WriteLine();
            Console.WriteLine("======================================");
            Console.WriteLine("       CANCEL TABLE RESERVATION");
            Console.WriteLine("======================================");

            List<Table> reservedTables = tableService.GetAllTables()
                .Where(t => t.IsReserved && !t.IsOccupied)
                .OrderBy(t => t.Number)
                .ToList();

            if (reservedTables.Count == 0)
            {
                Console.WriteLine("No reserved tables found.");
                Pause();
                return;
            }

            Console.WriteLine("Reserved tables:");
            PrintTables(reservedTables);

            try
            {
                int tableNumber = ReadInt("Table number: ");

                Table table = tableService.CancelReservation(tableNumber);

                Console.WriteLine();
                Console.WriteLine($"Reservation for table {table.Number} cancelled successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine("Error: " + ex.Message);
            }

            Pause();
        }

        private void CreateReservationUI()
        {
            Console.WriteLine();
            Console.WriteLine("======================================");
            Console.WriteLine("          CREATE RESERVATION");
            Console.WriteLine("======================================");

            try
            {
                int tableNumber = ReadInt("Table number: ");
                int guestCount = ReadInt("Guest count: ");

                DateTime startTime = ReadDateTime("Start time (yyyy-MM-dd HH:mm): ");
                DateTime endTime = ReadDateTime("End time (yyyy-MM-dd HH:mm): ");

                Reservation reservation = reservationService.CreateReservation(
                    tableNumber,
                    guestCount,
                    startTime,
                    endTime);

                Console.WriteLine();
                Console.WriteLine("Reservation created successfully!");
                Console.WriteLine($"Reservation ID: {reservation.Id}");
                Console.WriteLine($"Table ID: {reservation.TableId}");
                Console.WriteLine($"Guests: {reservation.GuestCount}");
                Console.WriteLine($"Start: {reservation.StartTime}");
                Console.WriteLine($"End: {reservation.EndTime}");
                Console.WriteLine($"Status: {reservation.Status}");
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine("Error: " + ex.Message);
            }

            Pause();
        }

        private void CancelReservationUI()
        {
            Console.WriteLine();
            Console.WriteLine("======================================");
            Console.WriteLine("          CANCEL RESERVATION");
            Console.WriteLine("======================================");

            try
            {
                int reservationId = ReadInt("Reservation ID: ");

                Reservation reservation = reservationService.CancelReservation(reservationId);

                Console.WriteLine();
                Console.WriteLine("Reservation cancelled successfully!");
                Console.WriteLine($"Reservation ID: {reservation.Id}");
                Console.WriteLine($"Status: {reservation.Status}");
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine("Error: " + ex.Message);
            }

            Pause();
        }

        private void ShowAllReservationsUI()
        {
            Console.WriteLine();
            Console.WriteLine("======================================");
            Console.WriteLine("          ALL RESERVATIONS");
            Console.WriteLine("======================================");

            List<Reservation> reservations = reservationService.GetAllReservations();

            if (reservations.Count == 0)
            {
                Console.WriteLine("No reservations found.");
                Pause();
                return;
            }

            PrintReservations(reservations);
            Pause();
        }

        private void ShowReservationsByTableUI()
        {
            Console.WriteLine();
            Console.WriteLine("======================================");
            Console.WriteLine("       RESERVATIONS BY TABLE");
            Console.WriteLine("======================================");

            try
            {
                int tableNumber = ReadInt("Table number: ");

                List<Reservation> reservations = reservationService.GetReservationsByTable(tableNumber);

                if (reservations.Count == 0)
                {
                    Console.WriteLine("No reservations found for this table.");
                    Pause();
                    return;
                }

                PrintReservations(reservations);
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine("Error: " + ex.Message);
            }

            Pause();
        }

        private static void PrintTables(List<Table> tables)
        {
            Console.WriteLine();
            Console.WriteLine("{0,-5} {1,-8} {2,-10} {3,-15} {4,-10} {5,-20}",
                "ID", "Number", "Capacity", "Location", "Status", "Reserved by");

            Console.WriteLine(new string('-', 75));

            foreach (Table table in tables)
            {
                string status;

                if (table.IsOccupied)
                    status = "Occupied";
                else if (table.IsReserved)
                    status = "Reserved";
                else
                    status = "Free";

                string reservedBy = string.IsNullOrWhiteSpace(table.ReservedBy)
                    ? "-"
                    : table.ReservedBy;

                Console.WriteLine("{0,-5} {1,-8} {2,-10} {3,-15} {4,-10} {5,-20}",
                    table.Id,
                    table.Number,
                    table.Capacity,
                    table.Location,
                    status,
                    reservedBy);
            }
        }

        private static void PrintReservations(List<Reservation> reservations)
        {
            foreach (Reservation reservation in reservations)
            {
                string tableNumber = reservation.Table != null
                    ? reservation.Table.Number.ToString()
                    : reservation.TableId.ToString();

                Console.WriteLine("--------------------------------------");
                Console.WriteLine($"Reservation ID: {reservation.Id}");
                Console.WriteLine($"Table: {tableNumber}");
                Console.WriteLine($"Guests: {reservation.GuestCount}");
                Console.WriteLine($"Start: {reservation.StartTime}");
                Console.WriteLine($"End: {reservation.EndTime}");
                Console.WriteLine($"Status: {reservation.Status}");
            }
        }

        private static int ReadInt(string message)
        {
            Console.Write(message);
            return int.Parse(Console.ReadLine()!);
        }

        private static DateTime ReadDateTime(string message)
        {
            Console.Write(message);

            return DateTime.ParseExact(
                Console.ReadLine()!,
                "yyyy-MM-dd HH:mm",
                CultureInfo.InvariantCulture);
        }

        private static void ComingSoon(string featureName)
        {
            Console.WriteLine();
            Console.WriteLine("======================================");
            Console.WriteLine(featureName.ToUpper());
            Console.WriteLine("======================================");
            Console.WriteLine("This feature is not implemented yet.");

            Pause();
        }

        private static void Pause()
        {
            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}