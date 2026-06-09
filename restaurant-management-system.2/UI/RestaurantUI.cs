using restaurant_management_system._2.Domain.Entities;
using restaurant_management_system._2.Domain.Enums;
using restaurant_management_system._2.Service;

namespace restaurant_management_system._2.UI
{
    internal class RestaurantUI
    {

        private readonly TableService tableService;
        private readonly ReservationService reservationService;
        private readonly MenuService menuService;
        private readonly OrderService orderService;
        private readonly PaymentService paymentService;

        public RestaurantUI(TableService tableService, ReservationService reservationService, MenuService menuService, OrderService orderService, PaymentService paymentService)
        {
            this.tableService = tableService;
            this.reservationService = reservationService;
            this.menuService = menuService;
            this.orderService = orderService;
            this.paymentService = paymentService;
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
                        MenuManagementMenu();
                        break;

                    case "3":
                        OrderManagementMenu();
                        break;

                    case "4":
                        ReservationManagementMenu();
                        break;

                    case "5":
                        PaymentManagementMenu();
                        break;

                    case "6":
                        ReportsManagementMenu();
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
        private void ReportsManagementMenu()
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("======================================");
                Console.WriteLine("              REPORTS");
                Console.WriteLine("======================================");
                Console.WriteLine("1. Table occupancy report");
                Console.WriteLine("2. Reservation status report");
                Console.WriteLine("3. Reservations by date");
                Console.WriteLine("4. Daily revenue report");
                Console.WriteLine("5. Most ordered items report");
                Console.WriteLine("0. Back");
                Console.WriteLine("======================================");

                Console.Write("Choose option: ");
                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        TableOccupancyReportUI();
                        break;

                    case "2":
                        ReservationStatusReportUI();
                        break;

                    case "3":
                        ReservationsByDateReportUI();
                        break;

                    case "4":
                        DailyRevenueReportUI();
                        break;

                    case "5":
                        MostOrderedItemsReportUI();
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
        private void DailyRevenueReportUI()
        {
            Console.WriteLine();
            Console.WriteLine("======================================");
            Console.WriteLine("          DAILY REVENUE REPORT");
            Console.WriteLine("======================================");

            try
            {
                int year = ReadInt("Year: ");
                int month = ReadInt("Month: ");
                int day = ReadInt("Day: ");

                DateTime selectedDate = new DateTime(year, month, day);

                List<Order> orders = orderService.GetAllOrders()
                    .Where(o =>
                        o.Status == OrderStatus.Closed &&
                        o.CreatedAt.Date == selectedDate.Date)
                    .OrderBy(o => o.CreatedAt)
                    .ToList();

                if (orders.Count == 0)
                {
                    Console.WriteLine();
                    Console.WriteLine("No closed orders found for this date.");
                    Pause();
                    return;
                }

                decimal totalRevenue = orders.Sum(o => o.TotalAmount);

                Console.WriteLine();
                Console.WriteLine($"Date: {selectedDate:yyyy-MM-dd}");
                Console.WriteLine($"Closed orders: {orders.Count}");
                Console.WriteLine($"Total revenue: {totalRevenue:F2} lv.");

                Console.WriteLine();
                Console.WriteLine("{0,-8} {1,-10} {2,-20} {3,-12}",
                    "Order", "Table", "Created at", "Total");

                Console.WriteLine(new string('-', 55));

                foreach (Order order in orders)
                {
                    Console.WriteLine("{0,-8} {1,-10} {2,-20} {3,-12:F2}",
                        order.Id,
                        order.TableId,
                        order.CreatedAt.ToString("yyyy-MM-dd HH:mm"),
                        order.TotalAmount);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine("Error: " + ex.Message);
            }

            Pause();
        }

        private void MostOrderedItemsReportUI()
        {
            Console.WriteLine();
            Console.WriteLine("======================================");
            Console.WriteLine("        MOST ORDERED ITEMS REPORT");
            Console.WriteLine("======================================");

            try
            {
                List<Order> orders = orderService.GetAllOrders()
                    .Where(o => o.Status != OrderStatus.Cancelled)
                    .ToList();

                if (orders.Count == 0)
                {
                    Console.WriteLine("No orders found.");
                    Pause();
                    return;
                }

                List<OrderItem> allOrderItems = new List<OrderItem>();

                foreach (Order order in orders)
                {
                    List<OrderItem> orderItems = orderService.GetOrderItems(order.Id);
                    allOrderItems.AddRange(orderItems);
                }

                if (allOrderItems.Count == 0)
                {
                    Console.WriteLine("No ordered items found.");
                    Pause();
                    return;
                }

                var report = allOrderItems
                    .GroupBy(oi => new
                    {
                        oi.MenuItemId,
                        Name = oi.MenuItem != null ? oi.MenuItem.Name : "Unknown item"
                    })
                    .Select(group => new
                    {
                        MenuItemId = group.Key.MenuItemId,
                        Name = group.Key.Name,
                        Quantity = group.Sum(oi => oi.Quantity),
                        Revenue = group.Sum(oi => oi.Quantity * oi.UnitPrice)
                    })
                    .OrderByDescending(x => x.Quantity)
                    .ToList();

                Console.WriteLine();
                Console.WriteLine("{0,-8} {1,-25} {2,-10} {3,-12}",
                    "Item ID", "Name", "Quantity", "Revenue");

                Console.WriteLine(new string('-', 60));

                foreach (var item in report)
                {
                    Console.WriteLine("{0,-8} {1,-25} {2,-10} {3,-12:F2}",
                        item.MenuItemId,
                        item.Name,
                        item.Quantity,
                        item.Revenue);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine("Error: " + ex.Message);
            }

            Pause();
        }

        private void TableOccupancyReportUI()
        {
            Console.WriteLine();
            Console.WriteLine("======================================");
            Console.WriteLine("        TABLE OCCUPANCY REPORT");
            Console.WriteLine("======================================");

            List<Table> tables = tableService.GetAllTables();

            if (tables.Count == 0)
            {
                Console.WriteLine("No tables found.");
                Pause();
                return;
            }

            int totalTables = tables.Count;
            int occupiedTables = tables.Count(t => t.IsOccupied);
            int reservedTables = tables.Count(t => t.IsReserved);
            int freeTables = tables.Count(t => !t.IsOccupied && !t.IsReserved);

            decimal occupiedPercent = totalTables == 0
                ? 0
                : (decimal)occupiedTables / totalTables * 100;

            decimal reservedPercent = totalTables == 0
                ? 0
                : (decimal)reservedTables / totalTables * 100;

            decimal freePercent = totalTables == 0
                ? 0
                : (decimal)freeTables / totalTables * 100;

            Console.WriteLine($"Total tables:    {totalTables}");
            Console.WriteLine($"Occupied tables: {occupiedTables} ({occupiedPercent:F2}%)");
            Console.WriteLine($"Reserved tables: {reservedTables} ({reservedPercent:F2}%)");
            Console.WriteLine($"Free tables:     {freeTables} ({freePercent:F2}%)");

            Console.WriteLine();
            Console.WriteLine("Tables by status:");
            PrintTables(tables);

            Pause();
        }

        private void ReservationStatusReportUI()
        {
            Console.WriteLine();
            Console.WriteLine("======================================");
            Console.WriteLine("       RESERVATION STATUS REPORT");
            Console.WriteLine("======================================");

            List<Reservation> reservations = reservationService.GetAllReservations();

            if (reservations.Count == 0)
            {
                Console.WriteLine("No reservations found.");
                Pause();
                return;
            }

            int totalReservations = reservations.Count;
            int confirmedReservations = reservations.Count(r => r.Status == ReservationStatus.Confirmed);
            int pendingReservations = reservations.Count(r => r.Status == ReservationStatus.Pending);
            int cancelledReservations = reservations.Count(r => r.Status == ReservationStatus.Cancelled);

            Console.WriteLine($"Total reservations:     {totalReservations}");
            Console.WriteLine($"Confirmed reservations: {confirmedReservations}");
            Console.WriteLine($"Pending reservations:   {pendingReservations}");
            Console.WriteLine($"Cancelled reservations: {cancelledReservations}");

            Console.WriteLine();
            Console.WriteLine("All reservations:");
            PrintReservations(reservations);

            Pause();
        }
        private void PaymentManagementMenu()
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("======================================");
                Console.WriteLine("          PAYMENT MANAGEMENT");
                Console.WriteLine("======================================");
                Console.WriteLine("1. Register payment");
                Console.WriteLine("2. Show all payments");
                Console.WriteLine("3. Show payments by order");
                Console.WriteLine("0. Back");
                Console.WriteLine("======================================");

                Console.Write("Choose option: ");
                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        RegisterPaymentUI();
                        break;

                    case "2":
                        ShowAllPaymentsUI();
                        break;

                    case "3":
                        ShowPaymentsByOrderUI();
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

        private void RegisterPaymentUI()
        {
            Console.WriteLine();
            Console.WriteLine("======================================");
            Console.WriteLine("           REGISTER PAYMENT");
            Console.WriteLine("======================================");

            List<Order> orders = paymentService.GetOrdersAvailableForPayment();

            if (orders.Count == 0)
            {
                Console.WriteLine("No orders available for payment.");
                Pause();
                return;
            }

            Console.WriteLine("Orders available for payment:");
            PrintOrdersForPayment(orders);

            try
            {
                int orderId = ReadInt("Order ID: ");

                bool canBePaid = orders.Any(o => o.Id == orderId);

                if (!canBePaid)
                    throw new ArgumentException("This order is not available for payment.");

                PaymentMethod method = ReadPaymentMethod();

                Payment payment = paymentService.RegisterPayment(orderId, method);

                Console.WriteLine();
                Console.WriteLine("Payment registered successfully!");
                Console.WriteLine($"Payment ID: {payment.Id}");
                Console.WriteLine($"Order ID: {payment.OrderId}");
                Console.WriteLine($"Amount: {payment.Amount:F2} lv.");
                Console.WriteLine($"Method: {payment.Method}");
                Console.WriteLine($"Paid at: {payment.PaidAt:yyyy-MM-dd HH:mm}");
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine("Error: " + ex.Message);
            }

            Pause();
        }

        private void ShowAllPaymentsUI()
        {
            Console.WriteLine();
            Console.WriteLine("======================================");
            Console.WriteLine("             ALL PAYMENTS");
            Console.WriteLine("======================================");

            List<Payment> payments = paymentService.GetAllPayments();

            if (payments.Count == 0)
            {
                Console.WriteLine("No payments found.");
                Pause();
                return;
            }

            PrintPayments(payments);
            Pause();
        }

        private void ShowPaymentsByOrderUI()
        {
            Console.WriteLine();
            Console.WriteLine("======================================");
            Console.WriteLine("          PAYMENTS BY ORDER");
            Console.WriteLine("======================================");

            List<Order> orders = paymentService.GetClosedOrders();

            if (orders.Count == 0)
            {
                Console.WriteLine("No closed orders found.");
                Pause();
                return;
            }

            Console.WriteLine("Closed orders:");
            PrintOrdersForPayment(orders);

            try
            {
                int orderId = ReadInt("Order ID: ");

                bool orderExists = orders.Any(o => o.Id == orderId);

                if (!orderExists)
                    throw new ArgumentException("Order not found in closed orders.");

                List<Payment> payments = paymentService.GetPaymentsForOrder(orderId);

                if (payments.Count == 0)
                {
                    Console.WriteLine();
                    Console.WriteLine("No payments found for this order.");
                    Pause();
                    return;
                }

                PrintPayments(payments);
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine("Error: " + ex.Message);
            }

            Pause();
        }
        private void PrintOrdersForPayment(List<Order> orders)
        {
            Console.WriteLine();
            Console.WriteLine("{0,-8} {1,-8} {2,-20} {3,-12} {4,-12}",
                "Order", "Table", "Created at", "Total", "Payment");

            Console.WriteLine(new string('-', 70));

            foreach (Order order in orders)
            {
                List<Payment> payments = paymentService.GetPaymentsForOrder(order.Id);

                string paymentStatus = payments.Count > 0
                    ? "Paid"
                    : "Not paid";

                Console.WriteLine("{0,-8} {1,-8} {2,-20} {3,-12:F2} {4,-12}",
                    order.Id,
                    order.TableId,
                    order.CreatedAt.ToString("yyyy-MM-dd HH:mm"),
                    order.TotalAmount,
                    paymentStatus);
            }
        }
        private static void PrintPayments(List<Payment> payments)
        {
            Console.WriteLine();
            Console.WriteLine("{0,-8} {1,-8} {2,-12} {3,-12} {4,-20}",
                "ID", "Order", "Amount", "Method", "Paid at");

            Console.WriteLine(new string('-', 65));

            foreach (Payment payment in payments)
            {
                Console.WriteLine("{0,-8} {1,-8} {2,-12:F2} {3,-12} {4,-20}",
                    payment.Id,
                    payment.OrderId,
                    payment.Amount,
                    payment.Method,
                    payment.PaidAt.ToString("yyyy-MM-dd HH:mm"));
            }
        }

        private static PaymentMethod ReadPaymentMethod()
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("Payment method:");
                Console.WriteLine("1. Cash");
                Console.WriteLine("2. Card");

                Console.Write("Choose payment method: ");
                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        return PaymentMethod.Cash;

                    case "2":
                        return PaymentMethod.Card;

                    default:
                        Console.WriteLine("Invalid payment method.");
                        break;
                }
            }
        }
        private void ReservationsByDateReportUI()
        {
            Console.WriteLine();
            Console.WriteLine("======================================");
            Console.WriteLine("        RESERVATIONS BY DATE");
            Console.WriteLine("======================================");

            try
            {
                int year = ReadInt("Year: ");
                int month = ReadInt("Month: ");
                int day = ReadInt("Day: ");

                DateTime selectedDate = new DateTime(year, month, day);

                List<Reservation> reservations = reservationService.GetAllReservations()
                    .Where(r => r.StartTime.Date == selectedDate.Date)
                    .OrderBy(r => r.StartTime)
                    .ToList();

                if (reservations.Count == 0)
                {
                    Console.WriteLine();
                    Console.WriteLine("No reservations found for this date.");
                    Pause();
                    return;
                }

                Console.WriteLine();
                Console.WriteLine($"Reservations for {selectedDate:yyyy-MM-dd}:");
                PrintReservations(reservations);
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine("Error: " + ex.Message);
            }

            Pause();
        }
        private void OrderManagementMenu()
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("======================================");
                Console.WriteLine("            ORDER MANAGEMENT");
                Console.WriteLine("======================================");
                Console.WriteLine("1. Create order");
                Console.WriteLine("2. Add item to order");
                Console.WriteLine("3. Remove item from order");
                Console.WriteLine("4. Show order total");
                Console.WriteLine("5. Close order");
                Console.WriteLine("0. Back");
                Console.WriteLine("======================================");

                Console.Write("Choose option: ");
                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        CreateOrderUI();
                        break;

                    case "2":
                        AddItemToOrderUI();
                        break;

                    case "3":
                        RemoveItemFromOrderUI();
                        break;

                    case "4":
                        ShowOrderTotalUI();
                        break;

                    case "5":
                        CloseOrderUI();
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

        private void CreateOrderUI()
        {
            Console.WriteLine();
            Console.WriteLine("======================================");
            Console.WriteLine("              CREATE ORDER");
            Console.WriteLine("======================================");

            List<Table> occupiedTables = tableService.GetOccupiedTables();

            if (occupiedTables.Count == 0)
            {
                Console.WriteLine("No occupied tables found. Occupy a table first.");
                Pause();
                return;
            }

            Console.WriteLine("Occupied tables:");
            PrintTables(occupiedTables);

            try
            {
                int tableId = ReadInt("Table ID: ");

                Order order = orderService.CreateOrder(tableId);

                Console.WriteLine();
                Console.WriteLine("Order created successfully!");
                Console.WriteLine($"Order ID: {order.Id}");
                Console.WriteLine($"Table ID: {order.TableId}");
                Console.WriteLine($"Status: {order.Status}");
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine("Error: " + ex.Message);
            }

            Pause();
        }

        private void AddItemToOrderUI()
        {
            Console.WriteLine();
            Console.WriteLine("======================================");
            Console.WriteLine("           ADD ITEM TO ORDER");
            Console.WriteLine("======================================");

            List<MenuItem> items = menuService.GetActiveMenuItems();

            if (items.Count == 0)
            {
                Console.WriteLine("No active menu items found.");
                Pause();
                return;
            }

            Console.WriteLine("Active menu items:");
            PrintMenuItems(items);

            try
            {
                int orderId = ReadInt("Order ID: ");
                int menuItemId = ReadInt("Menu item ID: ");
                int quantity = ReadInt("Quantity: ");

                OrderItem orderItem = orderService.AddItemToOrder(
                    orderId,
                    menuItemId,
                    quantity);

                Console.WriteLine();
                Console.WriteLine("Item added successfully!");
                Console.WriteLine($"Order item ID: {orderItem.Id}");
                Console.WriteLine($"Menu item ID: {orderItem.MenuItemId}");
                Console.WriteLine($"Quantity: {orderItem.Quantity}");
                Console.WriteLine($"Unit price: {orderItem.UnitPrice:F2} lv.");
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine("Error: " + ex.Message);
            }

            Pause();
        }

        private void RemoveItemFromOrderUI()
        {
            Console.WriteLine();
            Console.WriteLine("======================================");
            Console.WriteLine("        REMOVE ITEM FROM ORDER");
            Console.WriteLine("======================================");

            try
            {
                int orderItemId = ReadInt("Order item ID: ");

                orderService.RemoveItemFromOrder(orderItemId);

                Console.WriteLine();
                Console.WriteLine("Item removed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine("Error: " + ex.Message);
            }

            Pause();
        }

        private void ShowOrderTotalUI()
        {
            Console.WriteLine();
            Console.WriteLine("======================================");
            Console.WriteLine("             ORDER TOTAL");
            Console.WriteLine("======================================");

            try
            {
                int orderId = ReadInt("Order ID: ");

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

        private void CloseOrderUI()
        {
            Console.WriteLine();
            Console.WriteLine("======================================");
            Console.WriteLine("              CLOSE ORDER");
            Console.WriteLine("======================================");

            try
            {
                int orderId = ReadInt("Order ID: ");

                Order order = orderService.CloseOrder(orderId);

                Console.WriteLine();
                Console.WriteLine("Order closed successfully!");
                Console.WriteLine($"Order ID: {order.Id}");
                Console.WriteLine($"Total: {order.TotalAmount:F2} lv.");
                Console.WriteLine($"Status: {order.Status}");
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine("Error: " + ex.Message);
            }

            Pause();
        }

        private void MenuManagementMenu()
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("======================================");
                Console.WriteLine("            MENU MANAGEMENT");
                Console.WriteLine("======================================");
                Console.WriteLine("1. Show categories");
                Console.WriteLine("2. Add category");
                Console.WriteLine("3. Show active menu items");
                Console.WriteLine("4. Add menu item");
                Console.WriteLine("5. Change menu item price");
                Console.WriteLine("6. Hide menu item");
                Console.WriteLine("0. Back");
                Console.WriteLine("======================================");

                Console.Write("Choose option: ");
                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ShowCategoriesUI();
                        break;

                    case "2":
                        AddCategoryUI();
                        break;

                    case "3":
                        ShowActiveMenuItemsUI();
                        break;

                    case "4":
                        AddMenuItemUI();
                        break;

                    case "5":
                        ChangeMenuItemPriceUI();
                        break;

                    case "6":
                        HideMenuItemUI();
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
        private void ShowCategoriesUI()
        {
            Console.WriteLine();
            Console.WriteLine("======================================");
            Console.WriteLine("              CATEGORIES");
            Console.WriteLine("======================================");

            List<Category> categories = menuService.GetAllCategories();

            if (categories.Count == 0)
            {
                Console.WriteLine("No categories found.");
                Pause();
                return;
            }

            PrintCategories(categories);
            Pause();
        }

        private void AddCategoryUI()
        {
            Console.WriteLine();
            Console.WriteLine("======================================");
            Console.WriteLine("             ADD CATEGORY");
            Console.WriteLine("======================================");

            try
            {
                Console.Write("Category name: ");
                string name = Console.ReadLine()!;

                Category category = menuService.AddCategory(name);

                Console.WriteLine();
                Console.WriteLine("Category added successfully!");
                Console.WriteLine($"ID: {category.Id}");
                Console.WriteLine($"Name: {category.Name}");
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine("Error: " + ex.Message);
            }

            Pause();
        }

        private void ShowActiveMenuItemsUI()
        {
            Console.WriteLine();
            Console.WriteLine("======================================");
            Console.WriteLine("          ACTIVE MENU ITEMS");
            Console.WriteLine("======================================");

            List<MenuItem> items = menuService.GetActiveMenuItems();

            if (items.Count == 0)
            {
                Console.WriteLine("No active menu items found.");
                Pause();
                return;
            }

            PrintMenuItems(items);
            Pause();
        }

        private void AddMenuItemUI()
        {
            Console.WriteLine();
            Console.WriteLine("======================================");
            Console.WriteLine("             ADD MENU ITEM");
            Console.WriteLine("======================================");

            List<Category> categories = menuService.GetAllCategories();

            if (categories.Count == 0)
            {
                Console.WriteLine("No categories found. Add category first.");
                Pause();
                return;
            }

            Console.WriteLine("Available categories:");
            PrintCategories(categories);

            try
            {
                Console.Write("Menu item name: ");
                string name = Console.ReadLine()!;

                decimal price = ReadDecimal("Price: ");

                MenuItemType type = ReadMenuItemType();

                int categoryId = ReadInt("Category ID: ");

                MenuItem item = menuService.AddMenuItem(
                    name,
                    price,
                    type,
                    categoryId);

                Console.WriteLine();
                Console.WriteLine("Menu item added successfully!");
                Console.WriteLine($"ID: {item.Id}");
                Console.WriteLine($"Name: {item.Name}");
                Console.WriteLine($"Price: {item.Price:F2} lv.");
                Console.WriteLine($"Type: {item.Type}");
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine("Error: " + ex.Message);
            }

            Pause();
        }

        private void ChangeMenuItemPriceUI()
        {
            Console.WriteLine();
            Console.WriteLine("======================================");
            Console.WriteLine("        CHANGE MENU ITEM PRICE");
            Console.WriteLine("======================================");

            List<MenuItem> items = menuService.GetActiveMenuItems();

            if (items.Count == 0)
            {
                Console.WriteLine("No active menu items found.");
                Pause();
                return;
            }

            PrintMenuItems(items);

            try
            {
                int menuItemId = ReadInt("Menu item ID: ");
                decimal newPrice = ReadDecimal("New price: ");

                MenuItem item = menuService.ChangeMenuItemPrice(menuItemId, newPrice);

                Console.WriteLine();
                Console.WriteLine("Price changed successfully!");
                Console.WriteLine($"Item: {item.Name}");
                Console.WriteLine($"New price: {item.Price:F2} lv.");
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine("Error: " + ex.Message);
            }

            Pause();
        }

        private void HideMenuItemUI()
        {
            Console.WriteLine();
            Console.WriteLine("======================================");
            Console.WriteLine("             HIDE MENU ITEM");
            Console.WriteLine("======================================");

            List<MenuItem> items = menuService.GetActiveMenuItems();

            if (items.Count == 0)
            {
                Console.WriteLine("No active menu items found.");
                Pause();
                return;
            }

            PrintMenuItems(items);

            try
            {
                int menuItemId = ReadInt("Menu item ID: ");

                MenuItem item = menuService.HideMenuItem(menuItemId);

                Console.WriteLine();
                Console.WriteLine($"Menu item '{item.Name}' hidden successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine("Error: " + ex.Message);
            }

            Pause();
        }

        private static void PrintCategories(List<Category> categories)
        {
            Console.WriteLine();
            Console.WriteLine("{0,-5} {1,-20}", "ID", "Name");
            Console.WriteLine(new string('-', 30));

            foreach (Category category in categories)
            {
                Console.WriteLine("{0,-5} {1,-20}",
                    category.Id,
                    category.Name);
            }
        }

        private static void PrintMenuItems(List<MenuItem> items)
        {
            Console.WriteLine();
            Console.WriteLine("{0,-5} {1,-20} {2,-10} {3,-12} {4,-15}",
                "ID", "Name", "Price", "Type", "Category");

            Console.WriteLine(new string('-', 70));

            foreach (MenuItem item in items)
            {
                string categoryName = item.Category != null
                    ? item.Category.Name
                    : "-";

                Console.WriteLine("{0,-5} {1,-20} {2,-10:F2} {3,-12} {4,-15}",
                    item.Id,
                    item.Name,
                    item.Price,
                    item.Type,
                    categoryName);
            }
        }

        private static decimal ReadDecimal(string message)
        {
            Console.Write(message);
            return decimal.Parse(Console.ReadLine()!);
        }

        private static MenuItemType ReadMenuItemType()
        {
            Console.WriteLine();
            Console.WriteLine("Choose menu item type:");
            Console.WriteLine("1. Food");
            Console.WriteLine("2. Drink");
            Console.WriteLine("3. Dessert");
            Console.WriteLine("4. Alcohol");
            Console.WriteLine("5. Other");

            int choice = ReadInt("Option: ");

            switch (choice)
            {
                case 1:
                    return MenuItemType.Food;
                case 2:
                    return MenuItemType.Drink;
                case 3:
                    return MenuItemType.Dessert;
                case 4:
                    return MenuItemType.Alcohol;
                case 5:
                    return MenuItemType.Other;
                default:
                    throw new ArgumentException("Invalid menu item type.");
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
                Console.WriteLine("3. Occupy table");
                Console.WriteLine("4. Free table");
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
                        OccupyTableUI();
                        break;

                    case "4":
                        FreeTableUI();
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

    
        private void CreateReservationUI()
        {
            Console.WriteLine();
            Console.WriteLine("======================================");
            Console.WriteLine("          CREATE RESERVATION");
            Console.WriteLine("======================================");

            List<Table> freeTables = tableService.GetFreeTables();

            if (freeTables.Count == 0)
            {
                Console.WriteLine("No free tables available for reservation.");
                Pause();
                return;
            }

            Console.WriteLine("Tables available for reservation:");
            PrintTables(freeTables);

            try
            {
                int tableNumber = ReadInt("Table number: ");

                bool tableCanBeReserved = freeTables.Any(t => t.Number == tableNumber);

                if (!tableCanBeReserved)
                    throw new ArgumentException("This table is not available for reservation.");

                Console.Write("Customer name: ");
                string customerName = Console.ReadLine()!;

                int guestCount = ReadInt("Guest count: ");

                DateTime startTime = ReadStartDateTimeParts();
                DateTime endTime = ReadEndTimeParts(startTime);

                Reservation reservation = reservationService.CreateReservation(
                    tableNumber,
                    customerName,
                    guestCount,
                    startTime,
                    endTime);

                Console.WriteLine();
                Console.WriteLine("Reservation created successfully!");
                Console.WriteLine($"Reservation ID: {reservation.Id}");
                Console.WriteLine($"Customer: {reservation.CustomerName}");
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

            List<Reservation> reservations = reservationService.GetAllReservations()
                .Where(r => r.Status != restaurant_management_system._2.Domain.Enums.ReservationStatus.Cancelled)
                .ToList();

            if (reservations.Count == 0)
            {
                Console.WriteLine("No active reservations found.");
                Pause();
                return;
            }

            Console.WriteLine("Active reservations:");
            PrintReservations(reservations);

            try
            {
                int reservationId = ReadInt("Reservation ID: ");

                Reservation reservation = reservationService.CancelReservation(reservationId);

                Console.WriteLine();
                Console.WriteLine("Reservation cancelled successfully!");
                Console.WriteLine($"Reservation ID: {reservation.Id}");
                Console.WriteLine($"Customer: {reservation.CustomerName}");
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
            Console.WriteLine();
            Console.WriteLine("{0,-5} {1,-10} {2,-18} {3,-8} {4,-18} {5,-18} {6,-12}",
                "ID", "Table", "Customer", "Guests", "Start", "End", "Status");

            Console.WriteLine(new string('-', 95));

            foreach (Reservation reservation in reservations)
            {
                string tableNumber = reservation.Table != null
                    ? reservation.Table.Number.ToString()
                    : reservation.TableId.ToString();

                Console.WriteLine("{0,-5} {1,-10} {2,-18} {3,-8} {4,-18} {5,-18} {6,-12}",
                    reservation.Id,
                    tableNumber,
                    reservation.CustomerName,
                    reservation.GuestCount,
                    reservation.StartTime.ToString("yyyy-MM-dd HH:mm"),
                    reservation.EndTime.ToString("yyyy-MM-dd HH:mm"),
                    reservation.Status);
            }
        }

        private static int ReadInt(string message)
        {
            Console.Write(message);
            return int.Parse(Console.ReadLine()!);
        }
        private static DateTime ReadStartDateTimeParts()
        {
            Console.WriteLine();
            Console.WriteLine("Start time:");

            int year = ReadInt("Year: ");
            int month = ReadInt("Month: ");
            int day = ReadInt("Day: ");
            int hour = ReadInt("Hour: ");
            int minute = ReadInt("Minute: ");

            return new DateTime(year, month, day, hour, minute, 0);
        }

        private static DateTime ReadEndTimeParts(DateTime startTime)
        {
            Console.WriteLine();
            Console.WriteLine("End time:");

            int hour = ReadInt("Hour: ");
            int minute = ReadInt("Minute: ");

            return new DateTime(
                startTime.Year,
                startTime.Month,
                startTime.Day,
                hour,
                minute,
                0);
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