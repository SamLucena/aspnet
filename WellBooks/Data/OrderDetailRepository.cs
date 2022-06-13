﻿using MySqlConnector;
using WellBooks.Models;

namespace WellBooks.Data
{
    public class OrderDetailRepository
    {
        private MySqlConnection connection = new MySqlConnection("server=localhost;database=aspnetdb;user=root;password=root;");
    
        public void Add(OrderDetail entity)
        {
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO OrderDetails (ProductId, OrderId, Amount) VALUES (@product, @order, @amount)", connection);
                cmd.Parameters.Add("@product", MySqlDbType.VarChar).Value = entity.Product.Id; 
                cmd.Parameters.Add("@order", MySqlDbType.VarChar).Value = entity.Order.Id; 
                cmd.Parameters.Add("@amount", MySqlDbType.VarChar).Value = entity.Amount; 
                cmd.ExecuteNonQuery();
                connection.Close();
            }catch (MySqlException e)
            {
                
            }
        }
    }
}
