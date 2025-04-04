using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQL
{
    internal class Stat
    {
        public static void NombreLivraisonsParCuisinier(MySqlConnection conn)
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════╗");
            Console.WriteLine("║      NOMBRE DE LIVRAISONS PAR CUISINIER    ║");
            Console.WriteLine("╠════════════════════════════════════════════╣");

            string query = @"
        SELECT p.nom, p.prenom, COUNT(DISTINCT c.numero_commande) AS nb_livraisons
        FROM Cuisinier cu
        JOIN Particulier p ON cu.id_particulier = p.id_particulier
        LEFT JOIN Plat pl ON pl.id_cuisinier = cu.id_cuisinier
        LEFT JOIN Commande_Plat cp ON cp.id_plat = pl.id_plat
        LEFT JOIN Commande c ON c.numero_commande = cp.numero_commande
        GROUP BY cu.id_cuisinier, p.nom, p.prenom
        ORDER BY nb_livraisons DESC;";

            using (var cmd = new MySqlCommand(query, conn))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string nom = reader.GetString("nom");
                    string prenom = reader.GetString("prenom");
                    int nb = reader.GetInt32("nb_livraisons");

                    Console.WriteLine($"║ {prenom} {nom,-20}  {nb} livraisons");
                }
            }

            Console.WriteLine("╚════════════════════════════════════════════╝");
            Console.WriteLine("\nAppuyez sur une touche pour revenir au menu...");
            Console.ReadKey();
        }


        public static void AfficherCommandesParPeriode(MySqlConnection conn)
        {
            DateTime debut, fin;

            // Demande sécurisée de la date de début
            while (true)
            {
                Console.Write("Date de début (yyyy-MM-dd) : ");
                if (DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out debut))
                    break;
                Console.WriteLine(" Format invalide. Exemple attendu : 2025-04-01");
            }

            // Demande sécurisée de la date de fin
            while (true)
            {
                Console.Write("Date de fin (yyyy-MM-dd) : ");
                if (DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out fin))
                    break;
                Console.WriteLine(" Format invalide. Exemple attendu : 2025-04-15");
            }

            string query = @"
        SELECT c.numero_commande, c.id_client, c.prix_ttc, c.date_commande,
               CONCAT(p.prenom, ' ', p.nom) AS nom_client
        FROM Commande c
        JOIN Client cl ON c.id_client = cl.id_client
        JOIN Particulier p ON cl.id_particulier = p.id_particulier
        WHERE c.date_commande BETWEEN @debut AND @fin
        ORDER BY c.date_commande";

            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@debut", debut);
            cmd.Parameters.AddWithValue("@fin", fin);

            using var reader = cmd.ExecuteReader();

            Console.Clear();
            Console.WriteLine($" Commandes entre le {debut:dd/MM/yyyy} et le {fin:dd/MM/yyyy} :\n");

            bool hasResults = false;
            while (reader.Read())
            {
                hasResults = true;
                Console.WriteLine("╔════════════════════════════════════════════════════╗");
                Console.WriteLine($"║ Commande N° : {reader["numero_commande"],-36}║");
                Console.WriteLine($"║ Client      : {reader["nom_client"],-36}║");
                Console.WriteLine($"║ Prix TTC    : {reader["prix_ttc"],-36}║");
                Console.WriteLine($"║ Date        : {((DateTime)reader["date_commande"]).ToString("dd/MM/yyyy HH:mm"),-36}║");
                Console.WriteLine("╚════════════════════════════════════════════════════╝");
                Console.WriteLine(); // saut de ligne après le bloc

            }

            if (!hasResults)
            {
                Console.WriteLine(" Aucune commande trouvée pour cette période.");
            }

            Console.WriteLine("\nAppuyez sur une touche pour revenir au menu...");
            Console.ReadKey();
        }


       
        public static void AfficherMoyennePrixCommandes(MySqlConnection conn)
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════════════════╗");
            Console.WriteLine("║  MOYENNE DES PRIX TTC DES COMMANDES AVEC DES PLATS     ║");
            Console.WriteLine("╠════════════════════════════════════════════════════════╣");

            string query = @"
            SELECT AVG(prix_ttc)
            FROM Commande
            WHERE numero_commande IN (
                SELECT DISTINCT numero_commande FROM Commande_Plat
            )";

            using var cmd = new MySqlCommand(query, conn);
            object result = cmd.ExecuteScalar();

            if (result != DBNull.Value && decimal.TryParse(result.ToString(), out decimal moyenne))
            {
                Console.WriteLine($"║ Moyenne TTC (commandes valides) : {moyenne:F2}                ║");
            }
            else
            {
                Console.WriteLine("║ ❌ Aucune commande valide pour calculer la moyenne.         ║");
            }

            Console.WriteLine("╚════════════════════════════════════════════════════════╝");
            Console.WriteLine("\nAppuyez sur une touche pour revenir au menu...");
            Console.ReadKey();
        }



        public static void AfficherMoyenneMontantsClients(MySqlConnection conn)
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════════════════╗");
            Console.WriteLine("║         MOYENNE DES MONTANTS CLIENTS         ║");
            Console.WriteLine("╚══════════════════════════════════════════════╝");

            string query = @"SELECT AVG(prix_ttc)
                     FROM Commande
                     JOIN Client ON Commande.id_client = Client.id_client";

            using var cmd = new MySqlCommand(query, conn);
            object result = cmd.ExecuteScalar();

            if (result != DBNull.Value)
                Console.WriteLine($"\nMoyenne des montants clients : {Convert.ToDecimal(result)}");
            else
                Console.WriteLine("\nAucune commande trouvée pour le calcul.");

           
            Console.ReadKey();
        }


        public static void AfficherCommandesParClientParNationalite(MySqlConnection conn)
        {
            Console.Write("ID du client : ");
            if (!int.TryParse(Console.ReadLine(), out int idClient))
            {
                Console.WriteLine("❌ ID invalide. Veuillez entrer un entier.");
                return;
            }

            // Vérification client
            string verifClient = "SELECT COUNT(*) FROM Client WHERE id_client = @id";
            using (var checkCmd = new MySqlCommand(verifClient, conn))
            {
                checkCmd.Parameters.AddWithValue("@id", idClient);
                if ((long)checkCmd.ExecuteScalar() == 0)
                {
                    Console.WriteLine(" Ce client n'existe pas.");
                    return;
                }
            }

            // Dates
            DateTime debut, fin;
            Console.Write("Date de début (yyyy-mm-dd) : ");
            if (!DateTime.TryParse(Console.ReadLine(), out debut))
            {
                Console.WriteLine(" Date invalide.");
                return;
            }

            Console.Write("Date de fin (yyyy-mm-dd) : ");
            if (!DateTime.TryParse(Console.ReadLine(), out fin))
            {
                Console.WriteLine(" Date invalide.");
                return;
            }

            string query = @"
        SELECT p.nationalite_plat, COUNT(*) AS nb_commandes
        FROM Commande c
        JOIN Commande_Plat cp ON c.numero_commande = cp.numero_commande
        JOIN Plat p ON p.id_plat = cp.id_plat
        WHERE c.id_client = @id AND c.date_commande BETWEEN @debut AND @fin
        GROUP BY p.nationalite_plat";

            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", idClient);
            cmd.Parameters.AddWithValue("@debut", debut);
            cmd.Parameters.AddWithValue("@fin", fin);

            using var reader = cmd.ExecuteReader();

            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════╗");
            Console.WriteLine("║ COMMANDES PAR NATIONALITÉ DE PLAT          ║");
            Console.WriteLine("╠════════════════════════════════════════════╣");
            Console.WriteLine("║ Nationalité                    │ Commandes ║");
            Console.WriteLine("╠════════════════════════════════╪═══════════╣");

            bool found = false;
            while (reader.Read())
            {
                found = true;
                string nat = reader["nationalite_plat"].ToString().PadRight(30);
                string nb = reader["nb_commandes"].ToString().PadLeft(9);
                Console.WriteLine($"║ {nat}│ {nb} ║");
            }

            if (!found)
            {
            Console.WriteLine("║ Aucune commande trouvée...                 ║");
            }

            Console.WriteLine("╚════════════════════════════════════════════╝");
        }
    }
}
