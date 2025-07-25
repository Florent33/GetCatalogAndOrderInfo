using GetCatalogAndOrderInfo.Secret;
using Microsoft.Data.SqlClient;

namespace GetCatalogAndOrderInfo.Repository
{
    public class InsertTableHistoInfoRepository
    {
        public void InsertHistoInfo(string orderDt, string chId, string chName, string catName, string orderId, string productId, decimal orderPrice, decimal refPrice, decimal diffPrice, string stPrice, string msgErr)
        {
            string insertCat =
                @"INSERT INTO paas_export_catalog_order_info
                    (order_dt, channel_id, channel_name, catalog_name, order_id, product_id,
                    order_amount, price_reference, price_difference, price_status, order_status_message)
                VALUES                        
	                (@orderDt, @chId, @chName, @catName, @orderId, @productId, @orderPrice, @refPrice, @diffPrice, @stPrice, @msgErr)";

            try
            {           
                using (SqlConnection connection = new SqlConnection(GetCredentialsDatabaseConnection.connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(insertCat, connection);
                    {
                        command.Parameters.AddWithValue("@orderDt", orderDt);
                        command.Parameters.AddWithValue("@chId", chId);
                        command.Parameters.AddWithValue("@chName", chName);
                        command.Parameters.AddWithValue("@catName", catName);
                        command.Parameters.AddWithValue("@orderId", orderId);
                        command.Parameters.AddWithValue("@productId", productId);
                        command.Parameters.AddWithValue("@orderPrice", orderPrice);
                        command.Parameters.AddWithValue("@refPrice", refPrice);
                        command.Parameters.AddWithValue("@diffPrice", diffPrice);
                        command.Parameters.AddWithValue("@stPrice", stPrice);
                        command.Parameters.AddWithValue("@msgErr", msgErr);

                        command.ExecuteNonQuery();                        
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                Console.WriteLine(error);
            }
        }
    }
}