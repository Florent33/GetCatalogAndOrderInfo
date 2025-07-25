using GetCatalogAndOrderInfo.Models;
using GetCatalogAndOrderInfo.Secret;
using Microsoft.Data.SqlClient;

namespace GetCatalogAndOrderInfo.Repository
{
    public class GetHistoInfoRepository
    {
        public IEnumerable<CatalogModel> GetHistoInfo(string catalog)
        {
            List<CatalogModel> catalogList = new List<CatalogModel>();

            string queryCat =
                @"SELECT
					CAST(v.dateCommande as date) as 'Order date',
					v.client,
					c.Catalog,
					c.siteID,
					v.numCommande,
					v.sku,
					h.Prix_ref,
					v.montantCommande,
                    ROUND(h.Prix_ref - v.montantCommande, 2) as 'Difference price',
                    v.msgErr
				FROM
					tm_mad_ventes v (nolock)
					INNER JOIN TM_MAD_Liste_catalog c ON v.SiteID = c.siteID
					INNER JOIN TM_MAD_Export_Catalog_HISTO2 h ON h.ProductReferenceId = v.sku and h.Catalog = c.Catalog and CAST(v.dateCommande as date) = CAST(h.DateTime as date)
				WHERE
					1=1 and
					h.Catalog = @catalog and
					CAST(v.dateCommande as date) >= DATEADD(DAY, -60, GETDATE())
				ORDER BY
					CAST(v.dateCommande as date) DESC";

            try
            {
                using (SqlConnection connection = new SqlConnection(GetCredentialsDatabaseConnection.connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(queryCat, connection);
                    {
                        command.Parameters.AddWithValue("@catalog", catalog);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CatalogModel MyCatalog = new CatalogModel();
                                if (!string.IsNullOrEmpty(queryCat))
                                {
                                    MyCatalog.DateCommande = reader["Order date"] == DBNull.Value
                                        ? (DateTime?)null
                                        : Convert.ToDateTime(reader["Order date"]);
                                    MyCatalog.Client = reader["client"].ToString();
                                    MyCatalog.Catalog = reader["catalog"].ToString();
                                    MyCatalog.SiteID = reader["siteID"].ToString();
                                    MyCatalog.NumCommande = reader["numCommande"].ToString();
                                    MyCatalog.SKU = reader["sku"].ToString();
                                    MyCatalog.PrixRef = Convert.ToDecimal(reader["Prix_ref"].ToString());
                                    MyCatalog.MontantCommande = Convert.ToDecimal(reader["montantCommande"].ToString());
                                    MyCatalog.DifferencePrice = Convert.ToDecimal(reader["Difference price"].ToString());
                                    MyCatalog.MsgErr = reader["msgErr"].ToString();
                                    if (Math.Round(MyCatalog.DifferencePrice, 2) < 0)
                                    {
                                        MyCatalog.StatusPrice = "Trop cher";
                                    }
                                    else if (Math.Round(MyCatalog.DifferencePrice, 2) > 0)
                                    {
                                        MyCatalog.StatusPrice = "Pas assez cher";
                                    }
                                    else
                                    {
                                        MyCatalog.StatusPrice = "ISO";
                                    }
                                }
                                catalogList.Add(MyCatalog);
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                Console.WriteLine(error);
            }

            return catalogList;
        }
    }
}