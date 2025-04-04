using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQL
{
    internal class Cmd

    {
        public int numero_commande { get; set; }
        public string adresse_livraison_ { get; set; }
        public decimal prix_ht { get; set; }
        public DateTime date_commande { get; set; }
        public string instructions_commande { get; set; }
        public decimal prix_livraison { get; set; }
        public decimal prix_ttc { get; set; }
        public int id_client { get; set; }

        public static void CreerCommande(MySqlConnection conn)
        {
            Console.Write("ID du client : ");

            int idClient;

            while (true)
            {
                
                string input = Console.ReadLine();

                if (!int.TryParse(input, out idClient))
                {
                    Console.WriteLine(" Veuillez entrer un identifiant numérique valide : ");
                    continue;
                }

                string verifClient = "SELECT COUNT(*) FROM Client WHERE id_client = @idClient";
                using (var cmd = new MySqlCommand(verifClient, conn))
                {
                    cmd.Parameters.AddWithValue("@idClient", idClient);
                    long count = (long)cmd.ExecuteScalar();

                    if (count == 0)
                    {
                        Console.WriteLine(" Ce client n'existe pas. Veuillez le créer d'abord.");
                        continue;
                    }
                }

                break; 
            }


                Console.Write("Adresse de livraison : ");
                string adresse = Console.ReadLine();
               
                Console.Write("Instructions de commande : ");
                string instructions = Console.ReadLine();
                
                Console.Write("Prix HT : ");
                string input2 = Console.ReadLine();
                decimal prix_ht;

                while (!decimal.TryParse(input2, NumberStyles.Any, CultureInfo.InvariantCulture, out prix_ht))
                {
                    Console.WriteLine(" Format invalide. Veuillez entrer un prix valide (ex : 18,90) : ");
                   
                    input2 = Console.ReadLine();
                }
               
                Console.Write("Prix de livraison : ");

                string inputLivraison = Console.ReadLine()?.Replace(',', '.');

                decimal prix_livraison;

                while (!decimal.TryParse(inputLivraison, NumberStyles.Any, CultureInfo.InvariantCulture, out prix_livraison))
                {
                    Console.WriteLine(" Format invalide. Utilisez un format comme 3,50 : ");
                   
                    inputLivraison = Console.ReadLine()?.Replace(',', '.');
                }

                decimal prixTTC = prix_ht + prix_livraison + prix_ht * 0.2m;
                DateTime dateCommande = DateTime.Now;

                string insert = @"INSERT INTO Commande (adresse_livraison_, prix_ht, date_commande, instructions_commande, prix_livraison, prix_ttc, id_client)
                          VALUES (@adresse, @ht, @date, @instr, @livraison, @ttc, @client)";

                using (var cmd = new MySqlCommand(insert, conn))
                {
                    cmd.Parameters.AddWithValue("@adresse", adresse);
                    cmd.Parameters.AddWithValue("@ht", prix_ht);
                    cmd.Parameters.AddWithValue("@date", dateCommande);
                    cmd.Parameters.AddWithValue("@instr", instructions);
                    cmd.Parameters.AddWithValue("@livraison", prix_livraison);
                    cmd.Parameters.AddWithValue("@ttc", prixTTC);
                    cmd.Parameters.AddWithValue("@client", idClient);
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Commande enregistrée.");
                }
            }
        
            public static void AfficherPrixCommande(MySqlConnection conn)
            {
                Console.Write("Numéro de commande : ");

                 int numero;

                 while (true)
                {
                     
                     string input = Console.ReadLine();

                        if (!int.TryParse(input, out numero))
                     {
                          Console.WriteLine(" Veuillez entrer un numéro de commande valide : ");
                         continue;
                     }

                     break;
                 }

                string query = "SELECT prix_ttc FROM Commande WHERE numero_commande = @num";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@num", numero);
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        Console.WriteLine($"Prix TTC de la commande {numero} : {result} euros ");
                    }
                    else
                    {
                        Console.WriteLine("Commande introuvable.");
                    }
                }
            }

            public static void SimulerItineraireLivraison(MySqlConnection conn)
            {
                Console.Write("Numéro de commande : ");
               
                 int numero;

                 while (true)
                {
                     
                     string input = Console.ReadLine();

                        if (!int.TryParse(input, out numero))
                     {
                          Console.WriteLine(" Veuillez entrer un numéro de commande valide : ");
                         continue;
                     }

                     break;
                 }

                string query = @"SELECT adresse_livraison_ FROM Commande WHERE numero_commande = @num";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@num", numero);
                    string adresse = (string)cmd.ExecuteScalar();

                    if (adresse != null)
                    {
                        Console.WriteLine("Itinéraire simulé depuis l'adresse du cuisinier jusqu'à :");
                        Console.WriteLine(adresse);
                        Console.WriteLine("[Simulation de calcul du chemin le plus court en cours...]");
                        Console.WriteLine("Distance estimée : 3.4 km - Durée : 12 minutes à vélo");
                    }
                    else
                    {
                        Console.WriteLine("Commande introuvable.");
                    }
                }
            }

        public static void AfficherCommandesAvecPlats(MySqlConnection conn)
        {
            const int commandesParPage = 3;
            int page = 0;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("╔══════════════════════════════════════════════════════════════════════════════════════╗");
                Console.WriteLine("║                    COMMANDES + PLATS (Tri par prix TTC décroissant)                  ║");
                Console.WriteLine("╠══════════════════════════════════════════════════════════════════════════════════════╣");

                string query = @"
            SELECT c.numero_commande, CONCAT(p.prenom, ' ', p.nom) AS nom_client,
                   c.prix_ttc, c.date_commande, pl.nom AS plat
            FROM Commande c
            JOIN Client cl ON c.id_client = cl.id_client
            JOIN Particulier p ON cl.id_particulier = p.id_particulier
            LEFT JOIN Commande_Plat cp ON c.numero_commande = cp.numero_commande
            LEFT JOIN Plat pl ON cp.id_plat = pl.id_plat
            ORDER BY c.prix_ttc DESC, nom_client ASC
            LIMIT @offset, @limite";

                Dictionary<int, List<string>> platsParCommande = new();
                List<(int numero, string client, decimal prix, DateTime date)> commandes = new();

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@offset", page * commandesParPage);
                    cmd.Parameters.AddWithValue("@limite", commandesParPage);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int num = reader.GetInt32("numero_commande");
                            string client = reader.GetString("nom_client");
                            decimal prix = reader.GetDecimal("prix_ttc");
                            DateTime date = reader.GetDateTime("date_commande");
                            string plat = reader["plat"]?.ToString();

                            if (!platsParCommande.ContainsKey(num))
                            {
                                commandes.Add((num, client, prix, date));
                                platsParCommande[num] = new List<string>();
                            }
                            if (!string.IsNullOrEmpty(plat))
                                platsParCommande[num].Add(plat);
                        }
                    }
                }

                foreach (var cmd in commandes)
                {
                    Console.WriteLine($"║ Commande #{cmd.numero} - {cmd.client} - {cmd.prix:0.00} euros - {cmd.date:dd/MM/yyyy}");
                    Console.WriteLine("║   Plats :");
                    foreach (var plat in platsParCommande[cmd.numero])
                    {
                        Console.WriteLine($"║     - {plat}");
                    }
                    Console.WriteLine("╟──────────────────────────────────────────────────────────────────────────────────────╢");
                }

                Console.WriteLine("╚══════════════════════════════════════════════════════════════════════════════════════╝");
                Console.WriteLine("← Page précédente (P) | Page suivante (S) | Retour (Entrée)");

                var key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.P && page > 0)
                    page--;
                else if (key == ConsoleKey.S)
                    page++;
                else
                    break;
            }
        }


    }
}

