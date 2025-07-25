using Microsoft.Data.SqlClient;
using GetCatalogAndOrderInfo.Secret;

namespace GetCatalogAndOrderInfo.Repository
{
    public class DeleteTableHistoInfoRepository
    {
        public void DeleteHistoInfo()
        {
            string deleteCat = @"DELETE FROM paas_export_catalog_order_info";

            try
            {
                using (SqlConnection connection = new SqlConnection(GetCredentialsDatabaseConnection.connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(deleteCat, connection);
                    {
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"Erreur SQL : {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                Console.WriteLine(error);
            }
        }
    }
}