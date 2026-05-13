using restaurant_management_system._2.Domain.Entities;
using restaurant_management_system._2.Infrastructure;
using restaurant_management_system._2.Infrastructure.Repositories;
using restaurant_management_system._2.Service;
using System;
using System.Collections.Generic;

FileStorage storage = new FileStorage();

FileTableRepository tableRepository = new FileTableRepository(storage);

TableService tableService = new TableService(tableRepository);

while (true)
  {
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
            TableManagementMenu(tableService);
            break;

        case "2":
            ComingSoon("Menu management");
            break;

        case "3":
            ComingSoon("Order management");
            break;

        case "4":
            ComingSoon("Reservation management");
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

static void TableManagementMenu(TableService tableService)
{
    while (true)
    {
        Console.WriteLine("======================================");
        Console.WriteLine("          TABLE MANAGEMENT");
        Console.WriteLine("======================================");
        Console.WriteLine("1. Add table");
        Console.WriteLine("2. Show all tables");
        Console.WriteLine("3. Occupy table");
        Console.WriteLine("4. Free table");
        Console.WriteLine("5. Reserve table");
        Console.WriteLine("6. Show free tables");
        Console.WriteLine("0. Back");
        Console.WriteLine("======================================");

        Console.Write("Choose option: ");

        string? choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                AddTableUI(tableService);
                break;

            case "2":
                ShowAllTablesUI(tableService);
                break;

            case "3":
                ComingSoon("Occupy table");
                break;

            case "4":
                ComingSoon("Free table");
                break;

            case "5":
                ComingSoon("Reserve table");
                break;

            case "6":
                ComingSoon("Show free tables");
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

static void AddTableUI(TableService tableService)
{
    Console.WriteLine();
    Console.WriteLine("======================================");
    Console.WriteLine("               ADD TABLE");
    Console.WriteLine("======================================");

    try
    {
        Console.Write("Table number: ");
        int number = int.Parse(Console.ReadLine()!);

        Console.Write("Capacity: ");
        int capacity = int.Parse(Console.ReadLine()!);

        string location = ChooseTableLocation();

        Table table = tableService.AddTable(number, capacity, location);

        Console.WriteLine();
        Console.WriteLine("Table added successfully!");
        Console.WriteLine($"ID: {table.Id}");
        Console.WriteLine($"Number: {table.Number}");
        Console.WriteLine($"Capacity: {table.Capacity}");
        Console.WriteLine($"Location: {table.Location}");
        Console.WriteLine($"Occupied: {table.IsOccupied}");
    }
    catch (Exception ex)
    {
        Console.WriteLine();
        Console.WriteLine("Error: " + ex.Message);
    }

    Pause();
}

static void ShowAllTablesUI(TableService tableService)
{
    Console.WriteLine("======================================");
    Console.WriteLine("              ALL TABLES");
    Console.WriteLine("======================================");

    List<Table> tables = tableService.GetAllTables();

    if (tables.Count == 0)
    {
        Console.WriteLine("No tables found.");
    }
    else
    {
        foreach (Table table in tables)
        {
            Console.WriteLine("--------------------------------------");
            Console.WriteLine($"ID: {table.Id}");
            Console.WriteLine($"Number: {table.Number}");
            Console.WriteLine($"Capacity: {table.Capacity}");
            Console.WriteLine($"Location: {table.Location}");
            Console.WriteLine($"Occupied: {table.IsOccupied}");
        }
    }

    Pause();
}
static string ChooseTableLocation()
{
    Console.WriteLine();
    Console.WriteLine("Choose location:");
    Console.WriteLine("1. Main hall");
    Console.WriteLine("2. Window");
    Console.WriteLine("3. Terrace");
    Console.WriteLine("4. Garden");
    Console.WriteLine("5. VIP area");
    Console.Write("Option: ");

    string? choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            return "Main hall";
        case "2":
            return "Window";
        case "3":
            return "Terrace";
        case "4":
            return "Garden";
        case "5":
            return "VIP area";
        default:
            throw new ArgumentException("Invalid location option.");
    }
}
static void ComingSoon(string featureName)
{
    Console.WriteLine("This feature is not implemented yet.");
    Pause();
}

static void Pause()
{
    Console.WriteLine();
    Console.WriteLine("Press any key to continue...");
    Console.ReadKey();
}