using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

[Table("order")]
public class Order
{
    [Required]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("customer_name")]
    public string CustomerName { get; set; }

    [Required]
    [Column("customer_address")]
    public string CustomerAddress { get; set; }

    [Required]
    [Column("customer_phone")]
    public string CustomerPhone { get; set; }

    public List<OrderItem> Items { get; set; } = new List<OrderItem>();

    public Order(string customerName, string customerAddress, string customerPhone)
    {
        CustomerName = customerName;
        CustomerAddress = customerAddress;
        CustomerPhone = customerPhone;
    }

    public static int ValidateId(string input)
    {
        if (!int.TryParse(input, out int id) || id < 1)
        {
            throw new ArgumentException("Invalid order ID.");
        }

        return id;
    }

    public static string ValidateCustomerName(string input)
    {
        if (input.Length < 2 || input.Length > 13)
        {
            throw new ArgumentException("Customer name length must be between 2 and 13 characters.");
        }

        return input;
    }

    public static string ValidateCustomerAddress(string input)
    {
        if (input.Length < 2 || input.Length > 16)
        {
            throw new ArgumentException("Customer address length must be between 2 and 16 characters.");
        }

        return input;
    }

    public static string ValidateCustomerPhone(string input)
    {
        if (!Regex.IsMatch(input, @"^\+380\d{9}$"))
        {
            throw new ArgumentException("Incorrect phone number. It should be in the form of +380XXXXXXXXX.");
        }

        return input;
    }
}