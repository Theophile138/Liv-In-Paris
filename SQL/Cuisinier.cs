using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQL
{
   internal class Cuisinier
    {
        public int id_cuisinier { get; set; }
        public string mot_de_passe_ { get; set; }
        public int id_particulier { get; set; }

        public static void AjouterCuisinier(MySqlConnection conn)
        {
            int id_cuisinier;

            // 🔐 Vérification que l'id_cuisinier est libre
            while (true)
            {
                Console.Write("id_cuisinier : ");
                if (!int.TryParse(Console.ReadLine(), out id_cuisinier))
                {
                    Console.WriteLine("❌ Veuillez entrer un entier valide.");
                    continue;
                }

                string checkId = "SELECT COUNT(*) FROM Cuisinier WHERE id_cuisinier = @id";
                using (var cmd = new MySqlCommand(checkId, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id_cuisinier);
                    long count = (long)cmd.ExecuteScalar();

                    if (count > 0)
                    {
                        Console.WriteLine("❌ Cet ID cuisinier est déjà utilisé. Veuillez en saisir un autre.");
                    }
                    else break;
                }
            }

            Console.Write("mot_de_passe_ : ");
            string mot_de_passe = Console.ReadLine();

            Console.Write("id_particulier : ");
            int id_particulier = int.Parse(Console.ReadLine());

            // Vérification existence du particulier
            string checkParticulier = "SELECT COUNT(*) FROM Particulier WHERE id_particulier = @id";
            using (var cmd = new MySqlCommand(checkParticulier, conn))
            {
                cmd.Parameters.AddWithValue("@id", id_particulier);
                long count = (long)cmd.ExecuteScalar();

                if (count == 0)
                {
                    Console.WriteLine("?? Ce particulier n'existe pas. Créons-le maintenant...");

                    Console.Write("Nom : "); string nom = Console.ReadLine();
                    Console.Write("Prénom : "); string prenom = Console.ReadLine();
                    Console.Write("Téléphone : "); string tel = Console.ReadLine();
                    Console.Write("Email : "); string email = Console.ReadLine();
                    Console.Write("Rue : "); string rue = Console.ReadLine();
                    Console.Write("Numéro : "); string numero = Console.ReadLine();
                    Console.Write("Code postal : "); string cp = Console.ReadLine();
                    Console.Write("Ville : "); string ville = Console.ReadLine();
                    Console.Write("Métro le plus proche : "); string metro = Console.ReadLine();
                    Console.Write("Plat déjà commandé : "); string plat = Console.ReadLine();

                    string insertParticulier = @"INSERT INTO Particulier 
            (id_particulier, nom, prenom, tel, email, rue, numero, code_postal, ville, metro_le_plus_proche, plat_deja_commande) 
            VALUES (@id, @nom, @prenom, @tel, @mail, @rue, @num, @cp, @ville, @metro, @plat)";
                    using (var insertCmd = new MySqlCommand(insertParticulier, conn))
                    {
                        insertCmd.Parameters.AddWithValue("@id", id_particulier);
                        insertCmd.Parameters.AddWithValue("@nom", nom);
                        insertCmd.Parameters.AddWithValue("@prenom", prenom);
                        insertCmd.Parameters.AddWithValue("@tel", tel);
                        insertCmd.Parameters.AddWithValue("@mail", email);
                        insertCmd.Parameters.AddWithValue("@rue", rue);
                        insertCmd.Parameters.AddWithValue("@num", numero);
                        insertCmd.Parameters.AddWithValue("@cp", cp);
                        insertCmd.Parameters.AddWithValue("@ville", ville);
                        insertCmd.Parameters.AddWithValue("@metro", metro);
                        insertCmd.Parameters.AddWithValue("@plat", plat);
                        insertCmd.ExecuteNonQuery();
                    }
                }
            }

            // ✅ Insertion du cuisinier
            string insertCuisinier = "INSERT INTO Cuisinier (id_cuisinier, mot_de_passe_, id_particulier) " +
                                     "VALUES (@id, @mdp, @part)";
            using (var cmd = new MySqlCommand(insertCuisinier, conn))
            {
                cmd.Parameters.AddWithValue("@id", id_cuisinier);
                cmd.Parameters.AddWithValue("@mdp", mot_de_passe);
                cmd.Parameters.AddWithValue("@part", id_particulier);
                cmd.ExecuteNonQuery();
            }

            Console.WriteLine("✅ Cuisinier ajouté avec succès !");
        }

        public static void ModifierCuisinier(MySqlConnection conn)
        {
            Console.Write("id_cuisinier à modifier : ");
            int id = int.Parse(Console.ReadLine());
            Console.Write("Nouveau mot de passe : ");
            string mdp = Console.ReadLine();

            string query = "UPDATE Cuisinier SET mot_de_passe_ = @mdp WHERE id_cuisinier = @id";
            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@mdp", mdp);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
                Console.WriteLine("Mot de passe mis à jour.");
            }
        }

        public static void SupprimerCuisinier(MySqlConnection conn)
        {
            Console.Write("id_cuisinier à supprimer : ");
            int id = int.Parse(Console.ReadLine());
            string query = "DELETE FROM Cuisinier WHERE id_cuisinier = @id";
            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
                Console.WriteLine("Cuisinier supprimé.");
            }
        }

        public static void AfficherClientsServis(MySqlConnection conn, int id_cuisinier)
        {
            string query = @"
        SELECT DISTINCT p.nom, p.prenom, p.email, p.tel
        FROM Commande com
        JOIN Client c ON com.id_client = c.id_client
        JOIN Particulier p ON c.id_particulier = p.id_particulier
        JOIN Commande_Plat cp ON cp.numero_commande = com.numero_commande
        JOIN Plat pl ON pl.id_plat = cp.id_plat
        WHERE pl.id_cuisinier = @id_cuisinier
    ";

            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@id_cuisinier", id_cuisinier);
                //test test

                using (var reader = cmd.ExecuteReader())
                {
                    Console.WriteLine($"\n Clients servis par le cuisinier {id_cuisinier} :\n");

                    if (!reader.HasRows)
                    {
                        Console.WriteLine(" Aucun client trouvé.");
                    }

                    while (reader.Read())
                    {
                        Console.WriteLine($" {reader["prenom"]} {reader["nom"]}");
                        Console.WriteLine($" Email : {reader["email"]}");
                        Console.WriteLine($" Téléphone : {reader["tel"]}");
                        Console.WriteLine(new string('-', 40));
                    }
                }
            }
        }


        public static void AfficherPlatsRealisesParFrequence(MySqlConnection conn, int id_cuisinier)
        {
            string query = @"SELECT nom, COUNT(*) AS frequence FROM Plat
                         WHERE id_cuisinier = @id_cuisinier
                         GROUP BY nom ORDER BY frequence DESC";

            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@id_cuisinier", id_cuisinier);
                using (var reader = cmd.ExecuteReader())
                {
                    Console.WriteLine("Plats réalisés par fréquence :");
                    while (reader.Read())
                    {
                        Console.WriteLine($"- {reader["nom"]} : {reader["frequence"]} fois");
                    }
                }
            }
        }

        public static void AfficherPlatDuJour(MySqlConnection conn, int id_cuisinier)
        {
            string query = @"SELECT nom FROM Plat 
                         WHERE id_cuisinier = @id_cuisinier AND DATE(date_fabric) = CURDATE()
                         ORDER BY date_fabric DESC LIMIT 1";

            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@id_cuisinier", id_cuisinier);
                object result = cmd.ExecuteScalar();
                if (result != null)
                    Console.WriteLine($"Plat du jour : {result}");
                else
                    Console.WriteLine("Aucun plat du jour enregistré aujourd'hui.");
            }
        }

    }
}
