using MySqlConnector;
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

        public List<OrderDetail> FindByOrders(List<Order> orders, ApplicationDbContext db)
        {
            connection.Open();
            List<OrderDetail> orderDetails = new List<OrderDetail>();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            foreach (var order in orders)
            {
                cmd.CommandText = "SELECT * FROM OrderDetails WHERE OrderId = " + order.Id;
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var orderDetail = new OrderDetail()
                    {
                        Order = order,
                        Product = db.Products.Find(int.Parse(reader["ProductId"].ToString())),
                        Amount = int.Parse(reader["Amount"].ToString())
                    };
                    orderDetails.Add(orderDetail);
                    order.Details.Add(orderDetail);
                }
                reader.Close();
            }
            connection.Close();
            return orderDetails;
        }
    }
}
