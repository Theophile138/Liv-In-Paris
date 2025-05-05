using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json; 
using System.Xml.Serialization; 


namespace SQL
{
    internal class Client
    {
        public int id_client { get; set; }
        public string mot_de_passe { get; set; }

        public int id_particulier { get; set; }
        public string nom { get; set; }
        public string prenom { get; set; }
        public string rue { get; set; }
        public string numero { get; set; }
        public string code_postal { get; set; }
        public string ville { get; set; }
        public string tel { get; set; }
        public string email { get; set; }
        public string metro_le_plus_proche { get; set; }
       
        public string plat_deja_commande { get; set; }

        public string nom_entreprise { get; set; }
        public string nom_referent { get; set; }
        public string email_entreprise { get; set; }
        public double montant_achats { get; set; } = 0;
        public string type_client { get; set; }
        public long no_siret { get; set; }



        public Client(int id_client, string mot_de_passe, int id_particulier, long no_siret,
                      string nom, string prenom, string rue, string numero, string code_postal, string ville,
                      string tel, string email, string metro, string nom_entreprise, string nom_referent, string email_entreprise, string plat_deja_commande)
        {
            this.id_client = id_client;
            this.mot_de_passe = mot_de_passe;
            this.id_particulier = id_particulier;
            this.no_siret = no_siret;
            this.nom = nom;
            this.prenom = prenom;
            this.rue = rue;
            this.numero = numero;
            this.code_postal = code_postal;
            this.ville = ville;
            this.tel = tel;
            this.email = email;
            this.metro_le_plus_proche = metro;
            this.plat_deja_commande = plat_deja_commande;
            this.nom_entreprise = nom_entreprise;
            this.nom_referent = nom_referent;
            this.email_entreprise = email_entreprise;
            this.type_client = nom_entreprise != null ? "Entreprise" : "Particulier";
            
        }

        public static List<Client> GetAllClients(MySqlConnection conn)
        {
            List<Client> clients = new List<Client>();
            string query = @"SELECT c.id_client, c.mot_de_passe,
                            p.nom, p.prenom, p.rue, p.numero, p.code_postal, p.ville, p.tel, p.email, p.metro_le_plus_proche, p.plat_deja_commande, p.id_particulier,
                            e.nom AS nom_entreprise, e.nom_referent, e.email AS email_entreprise, e.no_siret
                     FROM Client c
                     JOIN Particulier p ON c.id_particulier = p.id_particulier
                     JOIN Entreprise e ON c.no_siret = e.no_siret";

            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    clients.Add(new Client(
                        reader.GetInt32("id_client"),
                        reader.GetString("mot_de_passe"),
                        reader.GetInt32("id_particulier"),
                        reader.GetInt64("no_siret"),
                        reader.GetString("nom"),
                        reader.GetString("prenom"),
                        reader["rue"]?.ToString(),
                        reader["numero"]?.ToString(),
                        reader["code_postal"].ToString(),
                        reader["ville"].ToString(),
                        reader["tel"]?.ToString(),
                        reader["email"].ToString(),
                        reader["metro_le_plus_proche"].ToString(),
                        reader["plat_deja_commande"]?.ToString(),
                        reader["nom_entreprise"]?.ToString(),
                        reader["nom_referent"]?.ToString(),
                        reader["email_entreprise"]?.ToString()
                    ));
                }
            }

            return clients;
        }


        public static void AjouterClient(MySqlConnection conn)
        {
            
            int id_client;
            while (true)
            {
                Console.Write("ID Client : ");
                string input = Console.ReadLine();

                if (!int.TryParse(input, out id_client))
                {
                    Console.WriteLine(" Veuillez entrer un nombre entier valide.");
                    continue;
                }

                string checkClientQuery = "SELECT COUNT(*) FROM Client WHERE id_client = @id";
                using (var checkCmd = new MySqlCommand(checkClientQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@id", id_client);
                    long exists = (long)checkCmd.ExecuteScalar();

                    if (exists > 0)
                    {
                        Console.WriteLine(" Cet ID Client est déjà utilisé. Veuillez en choisir un autre.");
                    }
                    else
                    {
                        break; 
                    }
                }
            }

            Console.Write("Mot de passe : "); string mot_de_passe = Console.ReadLine();
            Console.Write("Nom : "); string nom = Console.ReadLine();
            Console.Write("Prénom : "); string prenom = Console.ReadLine();
            Console.Write("Rue : "); string rue = Console.ReadLine();
            Console.Write("Numéro : "); string numero = Console.ReadLine();
            Console.Write("Code postal : "); string code_postal = Console.ReadLine();
            Console.Write("Ville : "); string ville = Console.ReadLine();
            Console.Write("Téléphone : "); string tel = Console.ReadLine();
            Console.Write("Email : "); string email = Console.ReadLine();
            Console.Write("Métro le plus proche : "); string metro = Console.ReadLine();
            Console.Write("Plat déjà commandé : "); string plat_deja_commande = Console.ReadLine();

            Console.Write("Type de client (Particulier/Entreprise) : ");
            string type = Console.ReadLine();

            long no_siret = 99999999999999;
            string nom_entreprise = null, nom_referent = null, email_entreprise = null;

            if (type.ToLower() == "entreprise")
            {
                Console.Write("Nom de l'entreprise : "); nom_entreprise = Console.ReadLine();
                Console.Write("Nom du référent : "); nom_referent = Console.ReadLine();
                Console.Write("Email de l'entreprise : "); email_entreprise = Console.ReadLine();

                while (true)
                {
                    Console.Write("Numéro SIRET de l'entreprise : ");
                    if (!long.TryParse(Console.ReadLine(), out no_siret))
                    {
                        Console.WriteLine(" Numéro SIRET invalide.");
                        continue;
                    }

                    string checkSiretQuery = "SELECT COUNT(*) FROM Entreprise WHERE no_siret = @siret";
                    using (var checkCmd = new MySqlCommand(checkSiretQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@siret", no_siret);
                        if ((long)checkCmd.ExecuteScalar() == 0) break;

                        Console.WriteLine("Ce numéro SIRET existe déjà. Veuillez en saisir un autre.");
                    }
                }

                string insertEntreprise = "INSERT INTO Entreprise (no_siret, nom, nom_referent, email, code_postal, ville) " +
                                          "VALUES (@siret, @nom, @ref, @mail, @cp, @ville)";
                using (var cmd = new MySqlCommand(insertEntreprise, conn))
                {
                    cmd.Parameters.AddWithValue("@siret", no_siret);
                    cmd.Parameters.AddWithValue("@nom", nom_entreprise);
                    cmd.Parameters.AddWithValue("@ref", nom_referent);
                    cmd.Parameters.AddWithValue("@mail", email_entreprise);
                    cmd.Parameters.AddWithValue("@cp", code_postal);
                    cmd.Parameters.AddWithValue("@ville", ville);
                    cmd.ExecuteNonQuery();
                }
            }
            else
            {

                string insertDefaultEntreprise = "INSERT IGNORE INTO Entreprise (no_siret, nom) VALUES (99999999999999, 'Particulier')";
                using (var cmd = new MySqlCommand(insertDefaultEntreprise, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }


            string insertParticulier = "INSERT INTO Particulier (nom, prenom, tel, email, rue, numero, code_postal, ville, metro_le_plus_proche, plat_deja_commande) " +
                                       "VALUES (@nom, @prenom, @tel, @mail, @rue, @num, @cp, @ville, @metro, @plat)";
            long id_particulier;
            using (var cmd = new MySqlCommand(insertParticulier, conn))
            {
                cmd.Parameters.AddWithValue("@nom", nom);
                cmd.Parameters.AddWithValue("@prenom", prenom);
                cmd.Parameters.AddWithValue("@tel", tel);
                cmd.Parameters.AddWithValue("@mail", email);
                cmd.Parameters.AddWithValue("@rue", rue);
                cmd.Parameters.AddWithValue("@num", numero);
                cmd.Parameters.AddWithValue("@cp", code_postal);
                cmd.Parameters.AddWithValue("@ville", ville);
                cmd.Parameters.AddWithValue("@metro", metro);
                cmd.Parameters.AddWithValue("@plat", plat_deja_commande);
                cmd.ExecuteNonQuery();

                id_particulier = cmd.LastInsertedId;
            }

            string insertClient = "INSERT INTO Client (id_client, mot_de_passe, id_particulier, no_siret) " +
                                  "VALUES (@id_client, @mdp, @id_particulier, @siret)";
            using (var cmd = new MySqlCommand(insertClient, conn))
            {
                cmd.Parameters.AddWithValue("@id_client", id_client);
                cmd.Parameters.AddWithValue("@mdp", mot_de_passe);
                cmd.Parameters.AddWithValue("@id_particulier", id_particulier);
                cmd.Parameters.AddWithValue("@siret", no_siret);
                cmd.ExecuteNonQuery();
            }

            Console.WriteLine("Client ajouté avec succès !");
        }



        public static void SupprimerClient(MySqlConnection conn)
        {
            Console.Write("id_client à supprimer : ");
            int id_client = int.Parse(Console.ReadLine());

            string query = "DELETE FROM Client WHERE id_client = @id";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@id", id_client);
                int rows = cmd.ExecuteNonQuery();
                Console.WriteLine(rows > 0 ? "Client supprimé avec succès." : "Aucun client trouvé.");
            }
        }

        public static void AfficherTousLesClients(MySqlConnection conn)
        {
            string query = @"
        SELECT c.id_client, c.mot_de_passe,
               p.nom AS nom_particulier, p.prenom, p.tel, p.email, p.rue, p.numero, p.code_postal, p.ville, p.metro_le_plus_proche, p.plat_deja_commande,
               e.nom AS nom_entreprise, e.nom_referent, e.email AS email_entreprise, e.no_siret
        FROM Client c
        JOIN Particulier p ON c.id_particulier = p.id_particulier
        JOIN Entreprise e ON c.no_siret = e.no_siret
        ORDER BY c.id_client;
    ";

            using (var cmd = new MySqlCommand(query, conn))
            using (var reader = cmd.ExecuteReader())
            {
                Console.Clear();
                Console.WriteLine("\n=========  Liste des Clients  =========\n");

                while (reader.Read())
                {
                    Console.WriteLine($" ID Client        : {reader["id_client"]}");
                    Console.WriteLine($" Mot de passe     : {reader["mot_de_passe"]}");
                    Console.WriteLine($" Nom              : {reader["nom_particulier"]} {reader["prenom"]}");
                    Console.WriteLine($" Téléphone        : {reader["tel"]}");
                    Console.WriteLine($" Email            : {reader["email"]}");
                    Console.WriteLine($" Adresse          : {reader["numero"]} {reader["rue"]}, {reader["code_postal"]} {reader["ville"]}");
                    Console.WriteLine($" Métro proche     : {reader["metro_le_plus_proche"]}");
                    Console.WriteLine($" Plat commandé    : {reader["plat_deja_commande"]}");

                    string nomEntreprise = reader["nom_entreprise"]?.ToString();
                    bool estParticulier = nomEntreprise == "Particulier" || reader["no_siret"].ToString() == "99999999999999";

                    if (estParticulier)
                    {
                        Console.WriteLine($" Type de client   : Particulier");
                    }
                    else
                    {
                        Console.WriteLine($" Type de client   : Entreprise");
                        Console.WriteLine($" Nom entreprise   : {reader["nom_entreprise"]}");
                        Console.WriteLine($" Référent         : {reader["nom_referent"]}");
                        Console.WriteLine($" Email entreprise : {reader["email_entreprise"]}");
                        Console.WriteLine($" N° SIRET         : {reader["no_siret"]}");
                    }

                    Console.WriteLine(new string('-', 60));
                }

                Console.WriteLine("\n Fin de la liste des clients.\n");
                Console.WriteLine("Appuyez sur une touche pour revenir au menu...");
                Console.ReadKey();
            }
        }


        public static List<Client> TrierParNom(List<Client> clients)
        {
            clients.Sort((a, b) => a.nom.CompareTo(b.nom));
            return clients;
        }

        public static List<Client> TrierParRue(List<Client> clients)
        {
            clients.Sort((a, b) => a.rue.CompareTo(b.rue));
            return clients;
        }

        public static List<Client> TrierParMontantAchats(List<Client> clients)
        {
            clients.Sort((a, b) => a.montant_achats.CompareTo(b.montant_achats));
            return clients;
        }

        public static void ModifierClient(MySqlConnection conn)
        {
            Console.Write("id_client à modifier : ");
            int id_client = int.Parse(Console.ReadLine());

            Console.Write("Nouveau nom : "); string nom = Console.ReadLine();
            Console.Write("Nouveau prénom : "); string prenom = Console.ReadLine();
            Console.Write("Nouvelle rue : "); string rue = Console.ReadLine();
            Console.Write("Nouveau numéro : "); string numero = Console.ReadLine();
            Console.Write("Nouveau code postal : "); string code_postal = Console.ReadLine();
            Console.Write("Nouvelle ville : "); string ville = Console.ReadLine();
            Console.Write("Nouveau téléphone : "); string tel = Console.ReadLine();
            Console.Write("Nouvel email : "); string email = Console.ReadLine();

            string queryParticulier = @"UPDATE Particulier 
            SET nom = @nom, prenom = @prenom, rue = @rue, numero = @numero,
                code_postal = @cp, ville = @ville, tel = @tel, email = @mail
            WHERE id_particulier = (SELECT id_particulier FROM Client WHERE id_client = @id_client)";

            using (MySqlCommand cmd = new MySqlCommand(queryParticulier, conn))
            {
                cmd.Parameters.AddWithValue("@nom", nom);
                cmd.Parameters.AddWithValue("@prenom", prenom);
                cmd.Parameters.AddWithValue("@rue", rue);
                cmd.Parameters.AddWithValue("@numero", numero);
                cmd.Parameters.AddWithValue("@cp", code_postal);
                cmd.Parameters.AddWithValue("@ville", ville);
                cmd.Parameters.AddWithValue("@tel", tel);
                cmd.Parameters.AddWithValue("@mail", email);
                cmd.Parameters.AddWithValue("@id_client", id_client);
                cmd.ExecuteNonQuery();
            }

            string getSiretQuery = "SELECT no_siret FROM Client WHERE id_client = @id_client";
            long no_siret = 0;
            using (MySqlCommand cmd = new MySqlCommand(getSiretQuery, conn))
            {
                cmd.Parameters.AddWithValue("@id_client", id_client);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                        no_siret = reader.GetInt64("no_siret");
                }
            }

            if (no_siret != 99999999999999)
            {
                Console.Write("Nouveau nom_entreprise : "); string nom_entreprise = Console.ReadLine();
                Console.Write("Nouveau nom_referent : "); string nom_referent = Console.ReadLine();
                Console.Write("Nouvel email_entreprise : "); string email_entreprise = Console.ReadLine();

                string queryEntreprise = @"UPDATE Entreprise 
                SET nom = @nomEnt, nom_referent = @ref, email = @mailEnt 
                WHERE no_siret = @siret";

                using (MySqlCommand cmd = new MySqlCommand(queryEntreprise, conn))
                {
                    cmd.Parameters.AddWithValue("@nomEnt", nom_entreprise);
                    cmd.Parameters.AddWithValue("@ref", nom_referent);
                    cmd.Parameters.AddWithValue("@mailEnt", email_entreprise);
                    cmd.Parameters.AddWithValue("@siret", no_siret);
                    cmd.ExecuteNonQuery();
                }

                Console.WriteLine("Entreprise mise à jour avec succès !");
            }

            Console.WriteLine("Client mis à jour !");
        }

        public static void ExporterClientsEnJson(List<Client> clients, string cheminFichier)
      {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(clients, options);
        File.WriteAllText(cheminFichier, json);
        Console.WriteLine($"Export JSON terminé : {cheminFichier}");
     }
    
        public static void ExporterClientsEnXml(List<Client> clients, string cheminFichier)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Client>));
            using (TextWriter writer = new StreamWriter(cheminFichier))
            {
            serializer.Serialize(writer, clients);
            }
            Console.WriteLine($"Export XML terminé : {cheminFichier}");
        }    






        
    }
}
