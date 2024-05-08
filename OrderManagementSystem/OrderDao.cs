using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

public class OrderDao
{
    public List<Order> GetAllOrders()
    {
        using var context = new OrderManagementContext();
        return context.Orders.Include(order => order.Items.OrderBy(item => item.Id)).ToList();
    }

    public Order GetOrderById(int id)
    {
        using var context = new OrderManagementContext();
        return context.Orders.Include(order => order.Items.OrderBy(item => item.Id)).FirstOrDefault(order => order.Id == id);
    }

    public OrderItem GetOrderItemById(Order order, int id)
    {
        using var context = new OrderManagementContext();
        return context.Orders.Find(order.Id).Items.FirstOrDefault(orderItem => orderItem.Id == id);
    }

    public void Add(Order order)
    {
        using var context = new OrderManagementContext();
        context.Orders.Add(order);
        context.SaveChanges();
    }

    public void Update(Order order)
    {
        using var context = new OrderManagementContext();
        context.Orders.Update(order);
        context.SaveChanges();
    }

    public void Update(OrderItem orderItem)
    {
        using var context = new OrderManagementContext();
        context.OrderItems.Update(orderItem);
        context.SaveChanges();
    }

    public void Remove(Order order)
    {
        using var context = new OrderManagementContext();
        context.OrderItems.RemoveRange(order.Items);
        context.Orders.Remove(order);
        context.SaveChanges();
    }

    public void Remove(OrderItem orderItem)
    {
        using var context = new OrderManagementContext();
        context.OrderItems.Remove(orderItem);
        context.SaveChanges();
    }
}