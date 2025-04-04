using MySql.Data.MySqlClient;
using System;

namespace SQL
{
    internal class Exploration_Culinaire
    {
        public static void RecommanderNouvelleNationalite(MySqlConnection conn)
        {
            int id_client = Utilitaires.DemanderEntier("ID du client : ");

            string query = @"
            SELECT DISTINCT p.nationalite_plat FROM Plat p
            WHERE p.nationalite_plat NOT IN (
                SELECT DISTINCT p2.nationalite_plat
                FROM Commande c
                JOIN Commande_Plat cp ON c.numero_commande = cp.numero_commande
                JOIN Plat p2 ON cp.id_plat = p2.id_plat
                WHERE c.id_client = @id_client
            )
            LIMIT 1";

            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id_client", id_client);
            var result = cmd.ExecuteScalar();
            if (result != null)
                Console.WriteLine($" Suggestion : découvrez un plat de nationalité '{result}' !");
            else
                Console.WriteLine(" Vous avez goûté à toutes les nationalités disponibles !");
        }

        public static void AfficherPlatsJamaisCommandes(MySqlConnection conn)
        {
            string query = @"
            SELECT p.nom, p.nationalite_plat FROM Plat p
            WHERE p.id_plat NOT IN (SELECT id_plat FROM Commande_Plat)";

            using var cmd = new MySqlCommand(query, conn);
            using var reader = cmd.ExecuteReader();
            Console.WriteLine(" Plats jamais commandés :");
            while (reader.Read())
            {
                Console.WriteLine($"- {reader["nom"]} ({reader["nationalite_plat"]})");
            }
        }

        public static void ClientsCurieux(MySqlConnection conn)
        {
            string query = @"
            SELECT c.id_client, COUNT(DISTINCT p.nationalite_plat) AS nb_nationalites
            FROM Commande c
            JOIN Commande_Plat cp ON c.numero_commande = cp.numero_commande
            JOIN Plat p ON cp.id_plat = p.id_plat
            GROUP BY c.id_client
            ORDER BY nb_nationalites DESC";

            using var cmd = new MySqlCommand(query, conn);
            using var reader = cmd.ExecuteReader();
            Console.WriteLine("Classement des clients curieux:");
            while (reader.Read())
            {
                Console.WriteLine($"Client {reader["id_client"]} : {reader["nb_nationalites"]} nationalités différentes");
            }
        }

        public static void PlatsNationauxPopulaires(MySqlConnection conn)
        {
            string query = @"
            SELECT p.nationalite_plat, p.nom, COUNT(*) AS commandes
            FROM Plat p
            JOIN Commande_Plat cp ON p.id_plat = cp.id_plat
            GROUP BY p.nationalite_plat, p.nom
            ORDER BY commandes DESC";

            using var cmd = new MySqlCommand(query, conn);
            using var reader = cmd.ExecuteReader();
            Console.WriteLine(" Plats nationaux populaires :");
            while (reader.Read())
            {
                Console.WriteLine($"{reader["nom"]} ({reader["nationalite_plat"]}) - {reader["commandes"]} commandes");
            }
        }

        public static void StatistiquesExploration(MySqlConnection conn)
        {
            int id_client = Utilitaires.DemanderEntier("ID du client : ");

            string totalQuery = "SELECT COUNT(DISTINCT nationalite_plat) FROM Plat";
            string clientQuery = @"
            SELECT COUNT(DISTINCT p.nationalite_plat)
            FROM Commande c
            JOIN Commande_Plat cp ON c.numero_commande = cp.numero_commande
            JOIN Plat p ON cp.id_plat = p.id_plat
            WHERE c.id_client = @id_client";

            using var cmdTotal = new MySqlCommand(totalQuery, conn);
            int total = Convert.ToInt32(cmdTotal.ExecuteScalar());

            using var cmdClient = new MySqlCommand(clientQuery, conn);
            cmdClient.Parameters.AddWithValue("@id_client", id_client);
            int clientTotal = Convert.ToInt32(cmdClient.ExecuteScalar());

            double pourcentage = total > 0 ? (clientTotal * 100.0) / total : 0;
            Console.WriteLine($" Nationalités testées : {clientTotal}/{total} ({pourcentage:F2}%)");
        }
    }
}
