﻿using System.ComponentModel.DataAnnotations;
using WellBooks.Models.Enums;

namespace WellBooks.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public DateTime Moment { get; set; }
        public OrderStatus Status { get; set; }
        public List<OrderDetail> Details { get; set; }

        public double GetTotalPrice()
        {
            double total = 0.0;
            foreach (OrderDetail detail in Details)
            {
                total += detail.GetTotal();
            }
            return total;
        }
    }
}
