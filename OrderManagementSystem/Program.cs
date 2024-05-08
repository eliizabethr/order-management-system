using System;
using System.Collections.Generic;
using System.Linq;

namespace OrderManagementSystem;

class Program
{
    static OrderDao orderDao = new OrderDao();

    static void Main()
    {
        PrintIntro();

        while (true)
        {
            Console.WriteLine("Now you can manage orders.\n");

            Console.WriteLine("Enter option number:");
            Console.WriteLine("1. Print all orders");
            Console.WriteLine("2. Create a new order");
            Console.WriteLine("3. Edit an order");
            Console.WriteLine("4. Delete an order");
            Console.WriteLine("0. Quit");
            Console.WriteLine();

            var input = Console.ReadLine();

            if (int.TryParse(input, out int option))
            {
                switch (option)
                {
                    case 0:
                        PrintOutro();
                        return;
                    case 1:
                        PrintOrders();
                        PrintContinuePrompt();
                        continue;
                    case 2:
                        CreateNewOrder();
                        PrintContinuePrompt();
                        continue;
                    case 3:
                        EditOrder();
                        continue;
                    case 4:
                        DeleteOrder();
                        PrintContinuePrompt();
                        continue;
                }
            }

            PrintErrorMessage("Incorrect option number, try again.\n");
        }
    }

    static void PrintIntro()
    {
        Console.WriteLine("This program is designed to manage orders.\n");
    }

    static void CreateNewOrder()
    {
        Console.WriteLine("Now you can create a new order.\n");

        var customerName = ReadCustomerName();
        var customerAddress = ReadCustomerAddress();
        var customerPhone = ReadCustomerPhone();

        var order = new Order(customerName, customerAddress, customerPhone);

        orderDao.Add(order);

        Console.WriteLine("Order has been created successfully.\n");

        EditOrder(order);
    }

    static void PrintOrders()
    {
        var orders = orderDao.GetAllOrders();
        PrintOrders(orders, printTotal: true);
    }

    static void PrintOrders(List<Order> orders, bool printTotal = false)
    {
        Console.WriteLine("                                                                Orders                                                                 \n");

        var horizontalLine = "|-----|---------------|------------------|----------------|-------------------|-----|-------------|---------------|-------------------|";

        Console.WriteLine(horizontalLine);

        Console.WriteLine("|  #  | Customer name | Customer address | Customer phone |       Title       | Qty |    Price    |  Total price  |     Order sum     |");

        if (orders.Count > 0)
        {
            foreach (var order in orders)
            {
                Console.WriteLine(horizontalLine);

                if (order.Items.Count > 0)
                {
                    var sum = order.Items.Sum(orderItem => orderItem.Price * orderItem.Quantity);

                    for (int i = 0; i < order.Items.Count; i++)
                    {
                        var orderItem = order.Items[i];
                        var totalPrice = orderItem.Price * orderItem.Quantity;
                        Console.WriteLine("| {0,3} | {1,-13} | {2,-16} | {3,-14} | {4,-17} | {5,3} | {6,11:F2} | {7,13:F2} | {8,17:F2} |", i == 0 ? order.Id : "", i == 0 ? order.CustomerName : "", i == 0 ? order.CustomerAddress : "", i == 0 ? order.CustomerPhone : "", orderItem.Title, orderItem.Quantity, orderItem.Price, totalPrice, i + 1 == order.Items.Count ? sum : "");
                    }
                }
                else
                {
                    Console.WriteLine("| {0,3} | {1,-13} | {2,-16} | {3,-14} | {4,-17} | {5,-3} | {6,-11:F2} | {7,-13:F2} | {8,-17:F2} |", order.Id, order.CustomerName, order.CustomerAddress, order.CustomerPhone, "-", "-", "-", "-", "-");
                }
            }

            Console.WriteLine(horizontalLine);
        }
        else
        {
            Console.WriteLine(horizontalLine);
            Console.WriteLine("| {0,-3} | {1,-13} | {2,-16} | {3,-14} | {4,-17} | {5,-3} | {6,-11} | {7,-13} | {8,-17:F2} |", "-", "-", "-", "-", "-", "-", "-", "-", "-");
            Console.WriteLine(horizontalLine);
        }

        if (printTotal)
        {
            var total = orders.Sum(order => order.Items.Sum(orderItem => orderItem.Price * orderItem.Quantity));
            Console.WriteLine("| {0,3} | {1,-13} | {2,-16} | {3,-14} | {4,-17} | {5,3} | {6,11} | {7,13} | {8,17:F2} |", "", "", "", "", "", "", "", "TOTAL:", total);
            Console.WriteLine(horizontalLine);
        }
    }

    static void EditOrder()
    {
        var orders = orderDao.GetAllOrders();
        PrintOrders(orders);

        while (true)
        {
            var orderId = ReadOrderId("\nEnter order ID to edit:");
            var order = orderDao.GetOrderById(orderId);

            if (order == null)
            {
                PrintErrorMessage("Invalid order ID. Try again.");
                continue;
            }

            EditOrder(order);

            break;
        }
    }

    static void EditOrder(Order order)
    {
        while (true)
        {
            Console.WriteLine($"Now you can choose what you want to do with the order.\n");

            Console.WriteLine("Enter option number:");
            Console.WriteLine("1. Print all order items");
            Console.WriteLine("2. Add a new order item");
            Console.WriteLine("3. Edit an order item");
            Console.WriteLine("4. Delete an order item");
            Console.WriteLine("0. Back");
            Console.WriteLine();

            var input = Console.ReadLine();

            if (int.TryParse(input, out var option))
            {
                switch (option)
                {
                    case 0:
                        return;
                    case 1:
                        PrintOrderItems(order);
                        PrintContinuePrompt();
                        continue;
                    case 2:
                        CreateNewOrderItem(order);
                        PrintContinuePrompt();
                        continue;
                    case 3:
                        EditOrderItem(order);
                        PrintContinuePrompt();
                        continue;
                    case 4:
                        DeleteOrderItem(order);
                        PrintContinuePrompt();
                        continue;
                }
            }

            PrintErrorMessage("Incorrect option number, try again.\n");
        }
    }

    static void DeleteOrder()
    {
        var orders = orderDao.GetAllOrders();
        PrintOrders(orders);

        while (true)
        {
            var orderId = ReadOrderId("\nEnter order ID to delete:");
            var order = orderDao.GetOrderById(orderId);

            if (order == null)
            {
                PrintErrorMessage("Invalid order ID. Try again.");
                continue;
            }

            orderDao.Remove(order);

            Console.WriteLine($"Order # {orderId} has been deleted successfully.");

            break;
        }
    }

    static void PrintOrderItems(Order order)
    {
        if (order.Items.Count == 0)
        {
            Console.WriteLine("There is no order items in this order.");
            return;
        }

        PrintOrderItems(order.Items);
    }

    static void CreateNewOrderItem(Order order)
    {
        Console.WriteLine("Now you can create a new order item.\n");

        var title = ReadOrderItemTitle("Enter order item title:", out bool _);
        var price = ReadOrderItemPrice("Enter order item price:", out bool _);
        var quantity = ReadOrderItemQuantity("Enter order item quantity:", out bool _);

        var orderItem = new OrderItem(title, price, quantity);

        order.Items.Add(orderItem);

        orderDao.Update(order);

        Console.WriteLine("Order item has been created successfully.");
    }

    static void PrintOrderItems(List<OrderItem> orderItems)
    {
        Console.WriteLine("                          Order items                          \n");

        var horizontalLine = "|-----|-------------------|-----|-------------|---------------|";

        Console.WriteLine(horizontalLine);
        Console.WriteLine("|  #  |       Title       | Qty |    Price    |  Total price  |");

        foreach (var orderItem in orderItems)
        {
            Console.WriteLine(horizontalLine);
            var totalPrice = orderItem.Price * orderItem.Quantity;
            Console.WriteLine("| {0,3} | {1,-17} | {2,3} | {3,11:F2} | {4,13:F2} |", orderItem.Id, orderItem.Title, orderItem.Quantity, orderItem.Price, totalPrice);
        }

        Console.WriteLine(horizontalLine);
    }

    static void EditOrderItem(Order order)
    {
        if (order.Items.Count == 0)
        {
            Console.WriteLine("There is no order items in this order.");
            return;
        }

        PrintOrderItems(order.Items);

        while (true)
        {
            var orderItemId = ReadOrderItemId("\nEnter order item ID to edit:");
            var orderItem = orderDao.GetOrderItemById(order, orderItemId);

            if (orderItem == null)
            {
                PrintErrorMessage("Invalid order item ID. Try again.");
                continue;
            }

            EditOrderItem(orderItem);

            break;
        }
    }

    static void EditOrderItem(OrderItem orderItem)
    {
        Console.WriteLine("Now you can update fields of the order item.\n");

        var newTitle = ReadOrderItemTitle("Enter new title (or press Enter to skip):", out bool isNewTitleEmpty);
        var newPrice = ReadOrderItemPrice("Enter new price (or press Enter to skip):", out bool isNewPriceEmpty);
        var newQuantity = ReadOrderItemQuantity("Enter new quantity (or press Enter to skip):", out bool isQuantityEmpty);

        if (!isNewTitleEmpty)
        {
            orderItem.Title = newTitle;
        }

        if (!isNewPriceEmpty)
        {
            orderItem.Price = newPrice;
        }

        if (!isQuantityEmpty)
        {
            orderItem.Quantity = newQuantity;
        }

        orderDao.Update(orderItem);

        Console.WriteLine("Order item has been edited successfully.");
    }

    static void DeleteOrderItem(Order order)
    {
        if (order.Items.Count == 0)
        {
            Console.WriteLine("There is no order items in this order.");
            return;
        }

        PrintOrderItems(order.Items);

        while (true)
        {
            var orderItemId = ReadOrderItemId("\nEnter order item ID to delete:");
            var orderItem = orderDao.GetOrderItemById(order, orderItemId);

            if (orderItem == null)
            {
                PrintErrorMessage("Invalid order item ID. Try again.");
                continue;
            }

            orderDao.Remove(orderItem);

            Console.WriteLine($"Order item # {orderItem.Id} has been deleted successfully.");

            break;
        }
    }

    static void PrintContinuePrompt()
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write("\nPress any key to continue...");
        Console.ReadKey();
        Console.WriteLine("\n");
        Console.ResetColor();
    }

    static void PrintErrorMessage(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    static void PrintOutro()
    {
        Console.WriteLine("Thank you for using our program!");
    }

    static int ReadOrderId(string message)
    {
        while (true)
        {
            Console.WriteLine(message);

            string input = Console.ReadLine();

            try
            {
                return Order.ValidateId(input);
            }
            catch (ArgumentException exception)
            {
                PrintErrorMessage($"{exception.Message} Try again.");
            }
        }
    }

    static string ReadCustomerName()
    {
        while (true)
        {
            Console.WriteLine("Enter customer name:");

            string input = Console.ReadLine();

            try
            {
                return Order.ValidateCustomerName(input);
            }
            catch (ArgumentException exception)
            {
                PrintErrorMessage($"{exception.Message} Try again.\n");
            }
        }
    }

    static string ReadCustomerAddress()
    {
        while (true)
        {
            Console.WriteLine("Enter customer address:");

            string input = Console.ReadLine();

            try
            {
                return Order.ValidateCustomerAddress(input);
            }
            catch (ArgumentException exception)
            {
                PrintErrorMessage($"{exception.Message} Try again.\n");
            }
        }
    }

    static string ReadCustomerPhone()
    {
        while (true)
        {
            Console.WriteLine("Enter customer phone:");

            string input = Console.ReadLine();

            try
            {
                return Order.ValidateCustomerPhone(input);
            }
            catch (ArgumentException exception)
            {
                PrintErrorMessage($"{exception.Message} Try again.\n");
            }
        }
    }

    static int ReadOrderItemId(string message)
    {
        while (true)
        {
            Console.WriteLine(message);

            string input = Console.ReadLine();

            try
            {
                return OrderItem.ValidateId(input);
            }
            catch (ArgumentException exception)
            {
                PrintErrorMessage($"{exception.Message} Try again.");
            }
        }
    }

    static string ReadOrderItemTitle(string message, out bool isEmpty)
    {
        while (true)
        {
            Console.WriteLine(message);

            string input = Console.ReadLine();

            if (string.IsNullOrEmpty(input))
            {
                isEmpty = true;
                return input;
            }

            try
            {
                isEmpty = false;
                return OrderItem.ValidateTitle(input);
            }
            catch (ArgumentException exception)
            {
                PrintErrorMessage($"{exception.Message} Try again.\n");
            }
        }
    }

    static decimal ReadOrderItemPrice(string message, out bool isEmpty)
    {
        while (true)
        {
            Console.WriteLine(message);

            string input = Console.ReadLine();

            if (string.IsNullOrEmpty(input))
            {
                isEmpty = true;
                return 0;
            }

            try
            {
                isEmpty = false;
                return OrderItem.ValidatePrice(input);
            }
            catch (ArgumentException exception)
            {
                PrintErrorMessage($"{exception.Message} Try again.\n");
            }
        }
    }

    static int ReadOrderItemQuantity(string message, out bool isEmpty)
    {
        while (true)
        {
            Console.WriteLine(message);

            string input = Console.ReadLine();

            if (string.IsNullOrEmpty(input))
            {
                isEmpty = true;
                return 0;
            }

            try
            {
                isEmpty = false;
                return OrderItem.ValidateQuantity(input);
            }
            catch (ArgumentException exception)
            {
                PrintErrorMessage($"{exception.Message} Try again.\n");
            }
        }
    }
}