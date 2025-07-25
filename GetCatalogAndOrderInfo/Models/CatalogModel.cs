namespace GetCatalogAndOrderInfo.Models
{
    /// <summary>
    /// Modèle représentant les informations du catalogue et des commandes.
    /// </summary>
    public class CatalogModel
    {
        /// <summary>
        /// Nom du catalogue.
        /// </summary>
        public string? CatalogName { get; set; }

        /// <summary>
        /// Date de la commande.
        /// </summary>
        public DateTime? DateCommande { get; set; }

        /// <summary>
        /// Nom du client.
        /// </summary>
        public string? Client { get; set; }

        /// <summary>
        /// Nom du catalogue lié à la commande.
        /// </summary>
        public string? Catalog { get; set; }

        /// <summary>
        /// Identifiant du site.
        /// </summary>
        public string? SiteID { get; set; }

        /// <summary>
        /// Numéro de la commande.
        /// </summary>
        public string? NumCommande { get; set; }

        /// <summary>
        /// SKU du produit.
        /// </summary>
        public string? SKU { get; set; }

        /// <summary>
        /// Prix de référence du produit.
        /// </summary>
        public decimal PrixRef { get; set; }

        /// <summary>
        /// Montant total de la commande.
        /// </summary>
        public decimal MontantCommande { get; set; }

        /// <summary>
        /// Différence entre le prix attendu et le prix réel.
        /// </summary>
        public decimal DifferencePrice { get; set; }

        /// <summary>
        /// Statut de la validation du prix.
        /// </summary>
        public string? StatusPrice { get; set; }

        /// <summary>
        /// Message d'erreur éventuel.
        /// </summary>
        public string? MsgErr { get; set; }

        /// <summary>
        /// Constructeur par défaut.
        /// </summary>
        public CatalogModel() { }

        /// <summary>
        /// Constructeur avec initialisation.
        /// </summary>
        public CatalogModel(string catalogName, DateTime? dateCommande, string client, string catalog,
            string siteID, string numCommande, string sku, decimal prixRef, decimal montantCommande,
            decimal differencePrice, string statusPrice, string msgErr)
        {
            CatalogName = catalogName;
            DateCommande = dateCommande;
            Client = client;
            Catalog = catalog;
            SiteID = siteID;
            NumCommande = numCommande;
            SKU = sku;
            PrixRef = prixRef;
            MontantCommande = montantCommande;
            DifferencePrice = differencePrice;
            StatusPrice = statusPrice;
            MsgErr = msgErr;
        }
    }
}
