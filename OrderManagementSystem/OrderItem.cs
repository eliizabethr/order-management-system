using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("order_item")]
public class OrderItem
{
    [Required]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("title")]
    public string Title { get; set; }

    [Required]
    [Column("price")]
    public decimal Price { get; set; }

    [Required]
    [Column("quantity")]
    public int Quantity { get; set; }

    [Required]
    [Column("order_id")]
    public int OrderId { get; set; }

    public OrderItem(string title, decimal price, int quantity)
    {
        Title = title;
        Price = price;
        Quantity = quantity;
    }

    public static int ValidateId(string input)
    {
        if (!int.TryParse(input, out int id) || id < 1)
        {
            throw new ArgumentException("Invalid order item ID.");
        }

        return id;
    }

    public static string ValidateTitle(string input)
    {
        if (input.Length < 2 || input.Length > 17)
        {
            throw new ArgumentException("Title length must be between 2 and 17 characters.");
        }

        return input;
    }

    public static decimal ValidatePrice(string input)
    {
        if (!decimal.TryParse(input, out decimal price) || price < 0)
        {
            throw new ArgumentException("Price must be greater than or equal to 0.");
        }

        return price;
    }

    public static int ValidateQuantity(string input)
    {
        if (!int.TryParse(input, out int quantity) || quantity < 1)
        {
            throw new ArgumentException("Quantity must be greater than or equal to 1.");
        }

        return quantity;
    }
}