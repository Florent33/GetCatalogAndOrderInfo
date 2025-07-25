using GetCatalogAndOrderInfo.Models;

namespace GetCatalogAndOrderInfo.Repository
{
    public class GetCatalogNameRepository
    {
        public IEnumerable<CatalogModel> GetCatalogName()
        {
            List<CatalogModel> catList = new List<CatalogModel>();

            try
            {
                string folderPath = Path.GetFullPath(@"~\GetCatalogAndOrderInfo\GetCatalogAndOrderInfo\FichierCSV\input\");
                string fileName = "catalogList";
                string fileExtension = ".csv";
                string fileCpltd = Path.GetFullPath(folderPath + fileName + fileExtension);

                // Vérification si le fichier existe
                if (!File.Exists(fileCpltd))
                {
                    Console.WriteLine($"Erreur : Le fichier spécifié n'existe pas : {fileCpltd}");
                    return catList;
                }

                // Lecture du fichier CSV
                using (var reader = new StreamReader(fileCpltd))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();

                        // Vérification de la ligne null ou vide
                        if (string.IsNullOrWhiteSpace(line)) continue;

                        var values = line.Split(';');

                        // Vérifie si la colonne du catalogue est valide
                        if (values.Length > 0 && !string.IsNullOrEmpty(values[0]))
                        {
                            catList.Add(new CatalogModel
                            {
                                CatalogName = values[0]
                            });
                        }
                    }
                    reader.Close();
                }

            }
            catch (Exception e)
            {
                string error = e.Message;
                Console.WriteLine(error);
            }
            
            return catList;
        }
    }
}
