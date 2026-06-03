using System;
using System.Collections.Generic;
using System.Linq;
using restaurant_management_system._2.Application.Interface;
using restaurant_management_system._2.Domain.Entities;
using restaurant_management_system._2.Infrastructure;
using restaurant_management_system._2.Infrastructure.Data;
using restaurant_management_system._2.Service;

RestaurantDbContext db = new RestaurantDbContext();

ITableRepository tableRepository = new FileTableRepository(db);
OrderService orderService = new OrderService(db);

TableService tableService = new TableService(tableRepository);

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
            OrderManagementMenu(orderService);
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

static void OrderManagementMenu(OrderService orderService)
{
    while (true)
    {
        Console.WriteLine();
        Console.WriteLine("======================================");
        Console.WriteLine("          ORDER MANAGEMENT");
        Console.WriteLine("======================================");
        Console.WriteLine("1. Create order");
        Console.WriteLine("2. Add item to order");
        Console.WriteLine("3. Remove item from order");
        Console.WriteLine("4. Calculate total");
        Console.WriteLine("5. Close order");
        Console.WriteLine("0. Back");
        Console.WriteLine("======================================");

        Console.Write("Choose option: ");
        string? choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                CreateOrderUI(orderService);
                break;
            case "2":
                AddItemToOrderUI(orderService);
                break;
            case "3":
                RemoveItemFromOrderUI(orderService);
                break;
            case "4":
                CalculateTotalUI(orderService);
                break;
            case "5":
                CloseOrderUI(orderService);
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

static void CreateOrderUI(OrderService orderService)
{
    Console.WriteLine();
    Console.WriteLine("======================================");
    Console.WriteLine("            CREATE ORDER");
    Console.WriteLine("======================================");

    try
    {
        Console.Write("Table ID: ");
        int tableId = int.Parse(Console.ReadLine()!);

        Order order = orderService.CreateOrder(tableId);

        Console.WriteLine();
        Console.WriteLine($"Order {order.Id} created for table ID {order.TableId}.");
    }
    catch (Exception ex)
    {
        Console.WriteLine();
        Console.WriteLine("Error: " + ex.Message);
    }

    Pause();
}

static void AddItemToOrderUI(OrderService orderService)
{
    Console.WriteLine();
    Console.WriteLine("======================================");
    Console.WriteLine("        ADD ITEM TO ORDER");
    Console.WriteLine("======================================");

    try
    {
        Console.Write("Order ID: ");
        int orderId = int.Parse(Console.ReadLine()!);

        Console.Write("Menu item ID: ");
        int menuItemId = int.Parse(Console.ReadLine()!);

        Console.Write("Quantity: ");
        int quantity = int.Parse(Console.ReadLine()!);

        orderService.AddItemToOrder(orderId, menuItemId, quantity);

        Console.WriteLine();
        Console.WriteLine("Item added to order.");
    }
    catch (Exception ex)
    {
        Console.WriteLine();
        Console.WriteLine("Error: " + ex.Message);
    }

    Pause();
}

static void RemoveItemFromOrderUI(OrderService orderService)
{
    Console.WriteLine();
    Console.WriteLine("======================================");
    Console.WriteLine("      REMOVE ITEM FROM ORDER");
    Console.WriteLine("======================================");

    try
    {
        Console.Write("Order item ID: ");
        int orderItemId = int.Parse(Console.ReadLine()!);

        orderService.RemoveItemFromOrder(orderItemId);

        Console.WriteLine();
        Console.WriteLine("Item removed from order.");
    }
    catch (Exception ex)
    {
        Console.WriteLine();
        Console.WriteLine("Error: " + ex.Message);
    }

    Pause();
}

static void CalculateTotalUI(OrderService orderService)
{
    Console.WriteLine();
    Console.WriteLine("======================================");
    Console.WriteLine("          CALCULATE TOTAL");
    Console.WriteLine("======================================");

    try
    {
        Console.Write("Order ID: ");
        int orderId = int.Parse(Console.ReadLine()!);

        decimal total = orderService.CalculateTotal(orderId);

        Console.WriteLine();
        Console.WriteLine($"Total: {total:F2} lv.");
    }
    catch (Exception ex)
    {
        Console.WriteLine();
        Console.WriteLine("Error: " + ex.Message);
    }

    Pause();
}

static void CloseOrderUI(OrderService orderService)
{
    Console.WriteLine();
    Console.WriteLine("======================================");
    Console.WriteLine("             CLOSE ORDER");
    Console.WriteLine("======================================");

    try
    {
        Console.Write("Order ID: ");
        int orderId = int.Parse(Console.ReadLine()!);

        orderService.CloseOrder(orderId);

        Console.WriteLine();
        Console.WriteLine("Order closed successfully.");
    }
    catch (Exception ex)
    {
        Console.WriteLine();
        Console.WriteLine("Error: " + ex.Message);
    }

    Pause();
}

static void Pause()
{
    Console.WriteLine();
    Console.WriteLine("Press any key to continue...");
    Console.ReadKey();
}

static void SeedTables(RestaurantDbContext db)
{
    if (db.Tables.Any())
        return;

    db.Tables.AddRange(
        new Table { Number = 1, Capacity = 2, Location = "Window", IsOccupied = false, IsReserved = false },
        new Table { Number = 2, Capacity = 2, Location = "Window", IsOccupied = false, IsReserved = false },
        new Table { Number = 3, Capacity = 4, Location = "Main hall", IsOccupied = true, IsReserved = false },
        new Table { Number = 4, Capacity = 4, Location = "Main hall", IsOccupied = false, IsReserved = false },
        new Table { Number = 5, Capacity = 6, Location = "Terrace", IsOccupied = false, IsReserved = true },
        new Table { Number = 6, Capacity = 6, Location = "Terrace", IsOccupied = false, IsReserved = false },
        new Table { Number = 7, Capacity = 8, Location = "VIP area", IsOccupied = true, IsReserved = false },
        new Table { Number = 8, Capacity = 8, Location = "VIP area", IsOccupied = false, IsReserved = false },
        new Table { Number = 9, Capacity = 2, Location = "Garden", IsOccupied = false, IsReserved = false },
        new Table { Number = 10, Capacity = 2, Location = "Garden", IsOccupied = false, IsReserved = true },
        new Table { Number = 11, Capacity = 4, Location = "Main hall", IsOccupied = false, IsReserved = false },
        new Table { Number = 12, Capacity = 4, Location = "Main hall", IsOccupied = true, IsReserved = false },
        new Table { Number = 13, Capacity = 6, Location = "Terrace", IsOccupied = false, IsReserved = false },
        new Table { Number = 14, Capacity = 6, Location = "Terrace", IsOccupied = false, IsReserved = false },
        new Table { Number = 15, Capacity = 8, Location = "VIP area", IsOccupied = false, IsReserved = true },
        new Table { Number = 16, Capacity = 8, Location = "VIP area", IsOccupied = true, IsReserved = false },
        new Table { Number = 17, Capacity = 2, Location = "Window", IsOccupied = false, IsReserved = false },
        new Table { Number = 18, Capacity = 4, Location = "Garden", IsOccupied = false, IsReserved = false },
        new Table { Number = 19, Capacity = 6, Location = "Main hall", IsOccupied = false, IsReserved = false },
        new Table { Number = 20, Capacity = 10, Location = "VIP area", IsOccupied = true, IsReserved = false }
    );

    db.SaveChanges();
}

