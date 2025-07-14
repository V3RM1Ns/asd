
using Orm.Core.Entities;
using Orm.BL.Dtos.MenuItemDtos;
using Orm.BL.Dtos.OrderDtos;
using Orm.BL.Dtos.OrderItemDtos;
using Orm.BL.Services.Concretes;
using Orm.BL.Services.Interfaces;
using Orm.DAL.DataStorage.Contexts;

public class Program
{
    public static async Task Main(string[] args)
    {
        await RunApplication();
    }

    private static async Task RunApplication()
    {
        bool exitSystem = false;
        while (!exitSystem)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔══════════════════════════════════════════╗");
            Console.WriteLine("║               MAIN MENU                  ║");
            Console.WriteLine("╠══════════════════════════════════════════╣");
            Console.WriteLine("║ 1. Perform operations on Menu Items      ║");
            Console.WriteLine("║ 2. Perform operations on Orders          ║");
            Console.WriteLine("║ 0. Exit System                           ║");
            Console.WriteLine("╚══════════════════════════════════════════╝");
            Console.ResetColor();
            Console.Write("\nEnter your choice (0-2): ");

            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    using (var context = new AppDbContex())
                    {
                        var menuItemService = new MenuItemService(context);
                        await HandleMenuItemOperations(menuItemService);
                    }
                    break;
                case "2":
                    using (var context = new AppDbContex())
                    {
                        var orderService = new OrderService(context);
                        var menuItemServiceForOrder = new MenuItemService(context);
                        await HandleOrderOperations(orderService, menuItemServiceForOrder);
                    }
                    break;
                case "0":
                    exitSystem = true;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\nExiting system. Goodbye!");
                    Console.ResetColor();
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error: Invalid choice! Please enter 0, 1, or 2.");
                    Console.ResetColor();
                    await Task.Delay(1500);
                    break;
            }
        }
    }

    private static async Task HandleMenuItemOperations(IMenuItemService menuItemService)
    {
        bool backToMainMenu = false;
        while (!backToMainMenu)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("╔═══════════════════════════════════════════════╗");
            Console.WriteLine("║             MENU ITEM OPERATIONS              ║");
            Console.WriteLine("╠═══════════════════════════════════════════════╣");
            Console.WriteLine("║ 1. Add new menu item                          ║");
            Console.WriteLine("║ 2. Edit existing menu item                    ║");
            Console.WriteLine("║ 3. Delete menu item                           ║");
            Console.WriteLine("║ 4. Show all menu items                        ║");
            Console.WriteLine("║ 5. Show menu items by category                ║");
            Console.WriteLine("║ 6. Show menu items by price range             ║");
            Console.WriteLine("║ 7. Search menu items by name                  ║");
            Console.WriteLine("║ 0. Back to previous menu                      ║");
            Console.WriteLine("╚═══════════════════════════════════════════════╝");
            Console.ResetColor();
            Console.Write("\nEnter your choice (0-7): ");

            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await AddNewMenuItem(menuItemService);
                    break;
                case "2":
                    await EditMenuItem(menuItemService);
                    break;
                case "3":
                    await DeleteMenuItem(menuItemService);
                    break;
                case "4":
                    await ShowAllMenuItems(menuItemService);
                    break;
                case "5":
                    await ShowMenuItemsByCategory(menuItemService);
                    break;
                case "6":
                    await ShowMenuItemsByPriceRange(menuItemService);
                    break;
                case "7":
                    await SearchMenuItemsByName(menuItemService);
                    break;
                case "0":
                    backToMainMenu = true;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error: Invalid choice! Please select a valid option.");
                    Console.ResetColor();
                    await Task.Delay(1500);
                    break;
            }

            if (!backToMainMenu)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\nPress any key to continue...");
                Console.ResetColor();
                Console.ReadKey();
            }
        }
    }

    private static async Task AddNewMenuItem(IMenuItemService menuItemService)
    {
        Console.WriteLine("\n--- Add New Menu Item ---");
        Console.Write("Enter Name: ");
        string? name = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(name))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error: Name cannot be empty.");
            Console.ResetColor();
            return;
        }

        Console.Write("Enter Price: ");
        int price;
        while (!int.TryParse(Console.ReadLine(), out price) || price <= 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Error: Invalid price! Please enter a positive number: ");
            Console.ResetColor();
        }

        Console.WriteLine("\nSelect Category:");
        foreach (Category category in Enum.GetValues(typeof(Category)))
        {
            Console.WriteLine($"  {(int)category}. {category}");
        }

        Console.Write("Enter Category number: ");
        Category selectedCategory;
        while (!Enum.TryParse(Console.ReadLine(), out selectedCategory))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Error: Invalid category! Please enter a number from the list: ");
            Console.ResetColor();
        }

        var menuItemCreateDto = new MenuItemCreateDto
        {
            Name = name,
            Price = price,
            Category = selectedCategory
        };

        try
        {
            await menuItemService.AddMenuItem(menuItemCreateDto);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Menu item added successfully!");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"An error occurred: {ex.Message}");
            Console.ResetColor();
        }
    }

    private static async Task EditMenuItem(IMenuItemService menuItemService)
    {
        Console.WriteLine("\n--- Edit Menu Item ---");
        Console.Write("Enter the ID of the Menu Item to edit: ");
        int id;
        while (!int.TryParse(Console.ReadLine(), out id) || id <= 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Error: Invalid ID! Please enter a positive integer: ");
            Console.ResetColor();
        }

        var existingItem = await menuItemService.GetById(id);
        if (existingItem == null)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Info: Menu item with the given ID was not found.");
            Console.ResetColor();
            return;
        }

        Console.WriteLine($"\nCurrent Details:");
        Console.WriteLine($"  ID: {existingItem.Id}");
        Console.WriteLine($"  Name: {existingItem.Name}");
        Console.WriteLine($"  Price: {existingItem.Price:C}");
        Console.WriteLine($"  Category: {existingItem.Category}");

        Console.Write("\nEnter New Name (leave empty to keep current): ");
        string? newName = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(newName))
        {
            newName = existingItem.Name;
        }

        Console.Write("Enter New Price (leave empty to keep current): ");
        string? newPriceStr = Console.ReadLine();
        decimal newPrice = existingItem.Price;
        if (!string.IsNullOrWhiteSpace(newPriceStr))
        {
            while (!decimal.TryParse(newPriceStr, out newPrice) || newPrice <= 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Error: Invalid price! Please enter a positive number: ");
                Console.ResetColor();
                newPriceStr = Console.ReadLine();
            }
        }

        Console.WriteLine("\nSelect New Category (leave empty to keep current):");
        foreach (Category category in Enum.GetValues(typeof(Category)))
        {
            Console.WriteLine($"  {(int)category}. {category}");
        }

        Console.Write("Enter Category number: ");
        string? newCategoryStr = Console.ReadLine();
        Category newCategory = existingItem.Category;
        if (!string.IsNullOrWhiteSpace(newCategoryStr))
        {
            while (!Enum.TryParse(newCategoryStr, out newCategory))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Error: Invalid category! Please enter a number from the list: ");
                Console.ResetColor();
                newCategoryStr = Console.ReadLine();
            }
        }

        var itemToUpdate = new MenuItem
        {
            Id = id,
            Name = newName,
            Price = (int)newPrice,
            Category = newCategory
        };

        try
        {
            await menuItemService.EditMenuItem(itemToUpdate);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Menu item updated successfully!");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"An error occurred: {ex.Message}");
            Console.ResetColor();
        }
    }

    private static async Task DeleteMenuItem(IMenuItemService menuItemService)
    {
        Console.WriteLine("\n--- Delete Menu Item ---");
        Console.Write("Enter the ID of the Menu Item to delete: ");
        int id;
        while (!int.TryParse(Console.ReadLine(), out id) || id <= 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Error: Invalid ID! Please enter a positive integer: ");
            Console.ResetColor();
        }

        var itemToDelete = await menuItemService.GetById(id);
        if (itemToDelete == null)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Info: Menu item with the given ID was not found.");
            Console.ResetColor();
            return;
        }

        try
        {
            var menuItemEntity = new MenuItem { Id = itemToDelete.Id };
            await menuItemService.DeleteMenuItem(menuItemEntity);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Menu item deleted successfully!");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"An error occurred: {ex.Message}");
            Console.ResetColor();
        }
    }

    private static async Task ShowAllMenuItems(IMenuItemService menuItemService)
    {
        Console.WriteLine("\n--- All Menu Items ---");
        var menuItems = await menuItemService.GetAllAsync();
        DisplayMenuItems(menuItems);
    }

    private static async Task ShowMenuItemsByCategory(IMenuItemService menuItemService)
    {
        Console.WriteLine("\n--- Menu Items by Category ---");
        Console.WriteLine("Available Categories:");
        foreach (Category category in Enum.GetValues(typeof(Category)))
        {
            Console.WriteLine($"  {(int)category}. {category}");
        }

        Console.Write("Enter Category number: ");
        Category selectedCategory;
        while (!Enum.TryParse(Console.ReadLine(), out selectedCategory))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Error: Invalid category! Please enter a number from the list: ");
            Console.ResetColor();
        }

        var menuItems = await menuItemService.GetByCategory(selectedCategory);
        DisplayMenuItems(menuItems);
    }

    private static async Task ShowMenuItemsByPriceRange(IMenuItemService menuItemService)
    {
        Console.WriteLine("\n--- Menu Items by Price Range ---");
        Console.Write("Enter Minimum Price: ");
        decimal minPrice;
        while (!decimal.TryParse(Console.ReadLine(), out minPrice) || minPrice < 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Error: Invalid price! Please enter a positive number: ");
            Console.ResetColor();
        }

        Console.Write("Enter Maximum Price: ");
        decimal maxPrice;
        while (!decimal.TryParse(Console.ReadLine(), out maxPrice) || maxPrice < minPrice)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Error: Invalid price! Please enter a number greater than or equal to minimum price: ");
            Console.ResetColor();
        }

        var menuItems = await menuItemService.GetByPriceRange(minPrice, maxPrice);
        DisplayMenuItems(menuItems);
    }

    private static async Task SearchMenuItemsByName(IMenuItemService menuItemService)
    {
        Console.WriteLine("\n--- Search by Name ---");
        Console.Write("Enter search keyword: ");
        string? keyword = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(keyword))
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Info: No keyword entered for search.");
            Console.ResetColor();
            return;
        }

        var menuItems = await menuItemService.SearchByName(keyword);
        DisplayMenuItems(menuItems);
    }

    private static void DisplayMenuItems(List<MenuItemReturnDto> menuItems)
    {
        if (menuItems == null || !menuItems.Any())
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Info: No menu items found.");
            Console.ResetColor();
            return;
        }

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\n{0,-5} {1,-25} {2,-15} {3,-10}", "ID", "Name", "Category", "Price");
        Console.WriteLine("------------------------------------------------------------------");
        Console.ResetColor();
        foreach (var item in menuItems)
        {
            Console.WriteLine("{0,-5} {1,-25} {2,-15} {3,-10:C}", item.Id, item.Name, item.Category, item.Price);
        }
    }

    private static async Task HandleOrderOperations(IOrderService orderService, IMenuItemService menuItemService)
    {
        bool backToMainMenu = false;
        while (!backToMainMenu)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("╔════════════════════════════════════════════════════╗");
            Console.WriteLine("║                  ORDER OPERATIONS                  ║");
            Console.WriteLine("╠════════════════════════════════════════════════════╣");
            Console.WriteLine("║ 1. Add new order                                   ║");
            Console.WriteLine("║ 2. Cancel an order                                 ║");
            Console.WriteLine("║ 3. Display all orders                              ║");
            Console.WriteLine("║ 4. Show orders by date range                       ║");
            Console.WriteLine("║ 5. Show orders by amount range                     ║");
            Console.WriteLine("║ 6. Show orders on a specific date                  ║");
            Console.WriteLine("║ 7. Show order details by ID                        ║");
            Console.WriteLine("║ 0. Back to previous menu                           ║");
            Console.WriteLine("╚════════════════════════════════════════════════════╝");
            Console.ResetColor();
            Console.Write("\nEnter your choice (0-7): ");

            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await AddNewOrder(orderService, menuItemService);
                    break;
                case "2":
                    await CancelOrder(orderService);
                    break;
                case "3":
                    await ShowAllOrders(orderService);
                    break;
                case "4":
                    await ShowOrdersByDateRange(orderService);
                    break;
                case "5":
                    await ShowOrdersByPriceRange(orderService);
                    break;
                case "6":
                    await ShowOrdersBySpecificDate(orderService);
                    break;
                case "7":
                    await ShowOrderDetailsById(orderService);
                    break;
                case "0":
                    backToMainMenu = true;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error: Invalid choice! Please select a valid option.");
                    Console.ResetColor();
                    await Task.Delay(1500);
                    break;
            }

            if (!backToMainMenu)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\nPress any key to continue...");
                Console.ResetColor();
                Console.ReadKey();
            }
        }
    }

    private static async Task AddNewOrder(IOrderService orderService, IMenuItemService menuItemService)
    {
        Console.WriteLine("\n--- Add New Order ---");

        var orderItems = new List<OrderItemCreateDto>();
        bool addingItems = true;

        while (addingItems)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\nAvailable Menu Items:");
            Console.ResetColor();
            var availableMenuItems = await menuItemService.GetAllAsync();
            DisplayMenuItems(availableMenuItems);

            Console.Write("\nEnter the Menu Item ID to order (enter '0' to finish adding items): ");
            int menuItemId;
            while (!int.TryParse(Console.ReadLine(), out menuItemId) || menuItemId < 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Error: Invalid ID! Please enter a positive integer or '0': ");
                Console.ResetColor();
            }

            if (menuItemId == 0)
            {
                addingItems = false;
                continue;
            }

            var selectedMenuItem = await menuItemService.GetById(menuItemId);
            if (selectedMenuItem == null)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Info: Menu item with the given ID was not found. Please enter a valid ID.");
                Console.ResetColor();
                continue;
            }

            Console.Write($"Enter quantity for '{selectedMenuItem.Name}': ");
            int quantity;
            while (!int.TryParse(Console.ReadLine(), out quantity) || quantity <= 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Error: Invalid quantity! Please enter a positive integer: ");
                Console.ResetColor();
            }

            orderItems.Add(new OrderItemCreateDto
            {
                MenuItemId = menuItemId,
                Quantity = quantity
            });

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"'{selectedMenuItem.Name}' x {quantity} added to order.");
            Console.ResetColor();
        }

        if (!orderItems.Any())
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Info: No items were added to the order. Order cancelled.");
            Console.ResetColor();
            return;
        }

        var orderCreateDto = new OrderCreateDto
        {
            OrderDate = DateTime.Now,
            OrderItems = orderItems
        };

        try
        {
            await orderService.AddOrder(orderCreateDto);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Order added successfully!");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"An error occurred: {ex.Message}");
            Console.ResetColor();
        }
    }

    private static async Task CancelOrder(IOrderService orderService)
    {
        Console.WriteLine("\n--- Cancel Order ---");
        Console.Write("Enter the ID of the Order to cancel: ");
        int id;
        while (!int.TryParse(Console.ReadLine(), out id) || id <= 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Error: Invalid ID! Please enter a positive integer: ");
            Console.ResetColor();
        }

        var orderToDelete = await orderService.GetById(id);
        if (orderToDelete == null)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Info: Order with the given ID was not found.");
            Console.ResetColor();
            return;
        }

        try
        {
            var orderEntity = new Order { Id = orderToDelete.Id };
            await orderService.DeleteOrder(orderEntity);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Order cancelled successfully!");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"An error occurred: {ex.Message}");
            Console.ResetColor();
        }
    }

    private static async Task ShowAllOrders(IOrderService orderService)
    {
        Console.WriteLine("\n--- All Orders ---");
        var orders = await orderService.GetAllOrdersAsync();
        DisplayOrders(orders);
    }

    private static async Task ShowOrdersByDateRange(IOrderService orderService)
    {
        Console.WriteLine("\n--- Orders by Date Range ---");
        Console.Write("Enter Start Date (YYYY-MM-DD): ");
        DateTime startDate;
        while (!DateTime.TryParse(Console.ReadLine(), out startDate))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Error: Invalid date format! Please enter in YYYY-MM-DD format: ");
            Console.ResetColor();
        }

        Console.Write("Enter End Date (YYYY-MM-DD): ");
        DateTime endDate;
        while (!DateTime.TryParse(Console.ReadLine(), out endDate) || endDate < startDate)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(
                "Error: Invalid date format or date is before start date! Please enter in YYYY-MM-DD format: ");
            Console.ResetColor();
        }

        var orders = await orderService.GetByDateRange(startDate, endDate.AddDays(1).AddTicks(-1));
        DisplayOrders(orders);
    }

    private static async Task ShowOrdersByPriceRange(IOrderService orderService)
    {
        Console.WriteLine("\n--- Orders by Amount Range ---");
        Console.Write("Enter Minimum Amount: ");
        decimal minAmount;
        while (!decimal.TryParse(Console.ReadLine(), out minAmount) || minAmount < 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Error: Invalid amount! Please enter a positive number: ");
            Console.ResetColor();
        }

        Console.Write("Enter Maximum Amount: ");
        decimal maxAmount;
        while (!decimal.TryParse(Console.ReadLine(), out maxAmount) || maxAmount < minAmount)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Error: Invalid amount! Please enter a number greater than or equal to minimum amount: ");
                Console.ResetColor();
            }

            var orders = await orderService.GetByPriceRange(minAmount, maxAmount);
            DisplayOrders(orders);
        }

        private static async Task ShowOrdersBySpecificDate(IOrderService orderService)
        {
            Console.WriteLine("\n--- Orders on a Specific Date ---");
            Console.Write("Enter Date (YYYY-MM-DD): ");
            DateTime date;
            while (!DateTime.TryParse(Console.ReadLine(), out date))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Error: Invalid date format! Please enter in YYYY-MM-DD format: ");
                Console.ResetColor();
            }

            var orders = await orderService.GetByDateRange(date.Date, date.Date.AddDays(1).AddTicks(-1));
            DisplayOrders(orders);
        }

        private static async Task ShowOrderDetailsById(IOrderService orderService)
        {
            Console.WriteLine("\n--- Order Details (by ID) ---");
            Console.Write("Enter Order ID: ");
            int id;
            while (!int.TryParse(Console.ReadLine(), out id) || id <= 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Error: Invalid ID! Please enter a positive integer: ");
                Console.ResetColor();
            }

            var order = await orderService.GetById(id);
            if (order == null)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Info: Order with the given ID was not found.");
                Console.ResetColor();
                return;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n--- Order Details ---");
            Console.ResetColor();
            Console.WriteLine($"  ID: {order.Id}");
            Console.WriteLine($"  Date: {order.OrderDate:yyyy-MM-dd HH:mm:ss}");
            Console.WriteLine($"  Total Amount: {order.TotalOrderPrice:C}");
            Console.WriteLine($"  Number of Items: {order.TotalOrderCount}");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nOrder Items:");
            Console.ResetColor();
            if (order.OrderItems != null && order.OrderItems.Any())
            {
                Console.WriteLine("{0,-10} {1,-30} {2,-10} {3,-15}", "Item ID", "Name", "Quantity", "Amount");
                Console.WriteLine("------------------------------------------------------------------");
                foreach (var item in order.OrderItems)
                {
                    Console.WriteLine("{0,-10} {1,-30} {2,-10} {3,-15:C}", item.MenuItemId, item.MenuItemName ?? "Unknown",
                        item.Quantity, item.TotalAmount);
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Info: No items found for this order.");
                Console.ResetColor();
            }
        }

        private static void DisplayOrders(List<OrderReturnDto> orders)
        {
            if (orders == null || !orders.Any())
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Info: No orders found.");
                Console.ResetColor();
                return;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n{0,-5} {1,-25} {2,-20} {3,-15}", "ID", "Date", "Total Amount", "Item Count");
            Console.WriteLine("----------------------------------------------------------------------");
            Console.ResetColor();
            foreach (var order in orders)
            {
                Console.WriteLine("{0,-5} {1,-25:yyyy-MM-dd HH:mm} {2,-20:C} {3,-15}",
                    order.Id,
                    order.OrderDate,
                    order.TotalOrderPrice,
                    order.TotalOrderCount);
            }
        }
    }