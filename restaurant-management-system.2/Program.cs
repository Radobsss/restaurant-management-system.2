using System;
using System.Collections.Generic;
using restaurant_management_system._2.Domain.Entities;
using restaurant_management_system._2.Infrastructure.Data;
using restaurant_management_system._2.Service;

RestaurantDbContext db = new RestaurantDbContext();

TableService tableService = new TableService(db);

while (true)
{
    Console.WriteLine();
    Console.WriteLine("======================================");
    Console.WriteLine("     RESTAURANT MANAGEMENT SYSTEM");
    Console.WriteLine("======================================");
    Console.WriteLine("1. Table management");
    Console.WriteLine("2. Menu management");
    Console.WriteLine("3. Order management");
    Console.WriteLine("4. Payment management");
    Console.WriteLine("5. Reports");
    Console.WriteLine("0. Exit");
    Console.WriteLine("======================================");

    Console.Write("Choose option: ");

    string? choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            TableManagementMenu(tableService);
            break;

        case "2":
            ComingSoon("Menu management");
            break;

        case "3":
            ComingSoon("Order management");
            break;

        case "4":
            ComingSoon("Payment management");
            break;

        case "5":
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

static void TableManagementMenu(TableService tableService)
{
    while (true)
    {
        Console.WriteLine();
        Console.WriteLine("======================================");
        Console.WriteLine("          TABLE MANAGEMENT");
        Console.WriteLine("======================================");
        Console.WriteLine("1. Show tables");
        Console.WriteLine("2. Reserve table");
        Console.WriteLine("3. Occupy table");
        Console.WriteLine("4. Free table");
        Console.WriteLine("5. Cancel reservation");
        Console.WriteLine("0. Back");
        Console.WriteLine("======================================");

        Console.Write("Choose option: ");

        string? choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                ShowAllTablesUI(tableService);
                break;

            case "2":
                ReserveTableUI(tableService);
                break;

            case "3":
                OccupyTableUI(tableService);
                break;

            case "4":
                FreeTableUI(tableService);
                break;

            case "5":
                CancelReservationUI(tableService);
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

static void ShowAllTablesUI(TableService tableService)
{
    Console.WriteLine();
    Console.WriteLine("==================================================");
    Console.WriteLine("                     TABLES");
    Console.WriteLine("==================================================");

    List<Table> tables = tableService.GetAllTables();

    if (tables.Count == 0)
    {
        Console.WriteLine("No tables found.");
        Pause();
        return;
    }

    Console.WriteLine("Number | Capacity | Location | Status");
    Console.WriteLine("--------------------------------------------------");

    foreach (Table table in tables)
    {
        string status;

        if (table.IsOccupied)
            status = "Occupied";
        else if (table.IsReserved)
            status = "Reserved";
        else
            status = "Free";

        Console.WriteLine($"{table.Number} | {table.Capacity} | {table.Location} | {status}");
    }

    Pause();
}

static void ShowTablesWithoutPause(TableService tableService)
{
    List<Table> tables = tableService.GetAllTables();

    Console.WriteLine();
    Console.WriteLine("Number | Capacity | Location | Status");
    Console.WriteLine("--------------------------------------------------");

    foreach (Table table in tables)
    {
        string status;

        if (table.IsOccupied)
            status = "Occupied";
        else if (table.IsReserved)
            status = "Reserved";
        else
            status = "Free";

        Console.WriteLine($"{table.Number} | {table.Capacity} | {table.Location} | {status}");
    }

    Console.WriteLine();
}

static void ReserveTableUI(TableService tableService)
{
    Console.WriteLine();
    Console.WriteLine("======================================");
    Console.WriteLine("            RESERVE TABLE");
    Console.WriteLine("======================================");

    ShowTablesWithoutPause(tableService);

    try
    {
        Console.Write("Table number: ");
        int tableNumber = int.Parse(Console.ReadLine()!);

        Table table = tableService.ReserveTable(tableNumber);

        Console.WriteLine();
        Console.WriteLine($"Table {table.Number} is now reserved.");
    }
    catch (Exception ex)
    {
        Console.WriteLine();
        Console.WriteLine("Error: " + ex.Message);
    }

    Pause();
}

static void OccupyTableUI(TableService tableService)
{
    Console.WriteLine();
    Console.WriteLine("======================================");
    Console.WriteLine("             OCCUPY TABLE");
    Console.WriteLine("======================================");

    ShowTablesWithoutPause(tableService);

    try
    {
        Console.Write("Table number: ");
        int tableNumber = int.Parse(Console.ReadLine()!);

        Table table = tableService.OccupyTable(tableNumber);

        Console.WriteLine();
        Console.WriteLine($"Table {table.Number} is now occupied.");
    }
    catch (Exception ex)
    {
        Console.WriteLine();
        Console.WriteLine("Error: " + ex.Message);
    }

    Pause();
}

static void FreeTableUI(TableService tableService)
{
    Console.WriteLine();
    Console.WriteLine("======================================");
    Console.WriteLine("              FREE TABLE");
    Console.WriteLine("======================================");

    ShowTablesWithoutPause(tableService);

    try
    {
        Console.Write("Table number: ");
        int tableNumber = int.Parse(Console.ReadLine()!);

        Table table = tableService.FreeTable(tableNumber);

        Console.WriteLine();
        Console.WriteLine($"Table {table.Number} is now free.");
    }
    catch (Exception ex)
    {
        Console.WriteLine();
        Console.WriteLine("Error: " + ex.Message);
    }

    Pause();
}

static void CancelReservationUI(TableService tableService)
{
    Console.WriteLine();
    Console.WriteLine("======================================");
    Console.WriteLine("         CANCEL RESERVATION");
    Console.WriteLine("======================================");

    ShowTablesWithoutPause(tableService);

    try
    {
        Console.Write("Table number: ");
        int tableNumber = int.Parse(Console.ReadLine()!);

        Table table = tableService.CancelReservation(tableNumber);

        Console.WriteLine();
        Console.WriteLine($"Reservation for table {table.Number} was canceled.");
    }
    catch (Exception ex)
    {
        Console.WriteLine();
        Console.WriteLine("Error: " + ex.Message);
    }

    Pause();
}

static void ComingSoon(string featureName)
{
    Console.WriteLine();
    Console.WriteLine("======================================");
    Console.WriteLine(featureName.ToUpper());
    Console.WriteLine("======================================");
    Console.WriteLine("This feature is not implemented yet.");

    Pause();
}

static void Pause()
{
    Console.WriteLine();
    Console.WriteLine("Press any key to continue...");
    Console.ReadKey();
}