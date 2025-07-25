using GetCatalogAndOrderInfo.Repository;
using System.Diagnostics;

namespace GetCatalogAndOrderInfo
{
    public class Program
    {
        public static void Main()
        {
            Console.Title = "Informations catalogues et commandes";

            try
            {
                Console.WriteLine("\nDémarrage du processus...\n");
                MeasureExecutionTime(() =>
                {
                    // Mesurer et afficher le temps d'exécution en secondes
                    DisplayLoadingBar(() =>
                    {
                        try
                        {
                            DeleteAllInformationFromTable();
                            GetAllInformation();
                            Console.ForegroundColor = ConsoleColor.Green;
                        }
                        catch (TimeoutException ex)
                        {
                            Console.WriteLine($"La tâche a dépassé le délai imparti : {ex.Message}");
                        }
                        catch (Exception ex)
                        {
                            // Gestion des erreurs générales
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"\nErreur inattendue : {ex.Message}");
                        }
                        finally
                        {
                            // Réinitialiser la couleur de la console
                            Console.ResetColor();
                        }
                    });
                });
                Console.WriteLine("Processus terminé !");
            }
            catch (Exception ex)
            {
                // Gestion des erreurs critiques qui empêchent le démarrage
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nErreur critique : {ex.Message}");
                Console.ResetColor();
            }
        }

        /* Fonction qui simule le chargement des données par une animation */
        public static void DisplayLoadingBar(Action task)
        {
            // Lancer la tâche dans un thread séparé
            Thread taskThread = new Thread(() => task());
            taskThread.Start();

            // Affichage de la barre de chargement
            char[] spinner = { '|', '/', '-', '\\' }; // Animation en rotation
            int spinnerIndex = 0;

            while (taskThread.IsAlive)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"\rChargement... {spinner[spinnerIndex]}");
                spinnerIndex = (spinnerIndex + 1) % spinner.Length;
                Thread.Sleep(100); // Pause de 100ms pour rendre l'animation fluide 
            }            
        }

        /* Fonction qui mesure et affiche le temps d'exécution en secondes */
        public static void MeasureExecutionTime(Action action)
        {
            // Créer une instance de Stopwatch
            Stopwatch stopwatch = new Stopwatch();

            // Démarrer le chronomètre
            stopwatch.Start();

            // Exécuter l'action
            action();

            // Arrêter le chronomètre
            stopwatch.Stop();

            // Calculer le temps écoulé en minutes et secondes
            double elapsedMinutes = Math.Floor(stopwatch.Elapsed.TotalMinutes);
            double elapsedSeconds = stopwatch.Elapsed.Seconds;

            // Afficher le temps d'exécution
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"\nTemps d'exécution : {elapsedMinutes} minutes et {elapsedSeconds} secondes\n");
        }

        /* Suppression des données de la table paas_export_catalog_order_info */
        public static void DeleteAllInformationFromTable()
        {
            DeleteTableHistoInfoRepository dthir = new();
            dthir.DeleteHistoInfo();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($" Toutes les données de la table paas_export_catalog_order_info ont été supprimées\n");
        }

        /* Récupération des informations catalogue et commande */
        public static void GetAllInformation()
        {
            // Récupérer la liste du nom des catalogues
            GetCatalogNameRepository gcnr = new GetCatalogNameRepository();
            var catName = gcnr.GetCatalogName();

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($" {catName.Count()} catalogues ont été récupérés\n");

            // Récupération des informations en BDD
            Console.WriteLine("-- Début de récupération de toutes les informations catalogues et commandes sur les 30 derniers jours --");

            // Récupération des informations en BDD
            GetHistoInfoRepository ghir = new GetHistoInfoRepository();

            // Insertion dans la table en BDD
            InsertTableHistoInfoRepository itir = new InsertTableHistoInfoRepository();

            foreach (Models.CatalogModel c in catName)
            {
                var catHist = ghir.GetHistoInfo(c.CatalogName ?? "DefaultCatalogName");

                // Si aucune donnée récupérée
                if (!catHist.Any())
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($" Aucune information récupérée pour le catalogue {c.CatalogName}");
                    continue;
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($" Informations récupérées pour le catalogue {c.CatalogName} [{catHist.Count()}]");

                // Insertion des données dans la table paas_export_catalog_order_info
                foreach (var item in catHist)
                {
                    try
                    {
                        string orderDate = item.DateCommande?.ToString("yyyy-MM-dd") ?? "0000-00-00";
                        string siteId = item.SiteID ?? "UnknownSite";
                        string clientName = item.Client ?? "UnknownClient";
                        string catalogName = item.Catalog ?? "UnknownCatalog";
                        string orderId = item.NumCommande ?? "UnknownOrder";
                        string sku = item.SKU ?? "UnknownSKU";
                        decimal orderPrice = item.MontantCommande;
                        decimal refPrice = item.PrixRef;
                        decimal diffPrice = item.DifferencePrice;
                        string statusPrice = item.StatusPrice ?? "UnknownStatus";
                        string errorMessage = item.MsgErr ?? string.Empty;

                        itir.InsertHistoInfo(orderDate, siteId, clientName, catalogName, orderId, sku, orderPrice, refPrice, diffPrice, statusPrice, errorMessage);
                    }
                    catch (Exception ex)
                    {
                        // Log de l'erreur pour déboguer ou surveiller les échecs
                        Console.WriteLine($"Erreur lors de l'insertion de l'élément : {ex.Message}");
                    }
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"    Insertion des données de {c.CatalogName} dans la table paas_export_catalog_order_info");
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("-- Fin de récupération de toutes les informations catalogues et commandes sur les 30 derniers jours --");
        }
    }
}