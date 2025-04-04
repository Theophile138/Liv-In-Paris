
using MySql.Data.MySqlClient;

namespace SQL
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var conn = OuvrirLaConnexion.ObtenirConnexion();
            if (conn == null)
            {
                Console.WriteLine("Appuyez sur une touche pour quitter...");
                Console.ReadKey();
                return;
            }

            Program.MenuPrincipal(conn);
        }



        public static void MenuPrincipal(MySqlConnection conn)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("╔════════════════════════════════════════════╗");
                Console.WriteLine("║              MENU PRINCIPAL                ║");
                Console.WriteLine("╠════════════════════════════════════════════╣");
                Console.WriteLine("║ 1. Gestion des Clients                     ║");
                Console.WriteLine("║ 2. Gestion des Cuisiniers                  ║");
                Console.WriteLine("║ 3. Gestion des Commandes                   ║");
                Console.WriteLine("║ 4. Statistiques Générales                  ║");
                Console.WriteLine("║ 5. Explorateur de Goûts                    ║");
                Console.WriteLine("║ 0. Quitter                                 ║");
                Console.WriteLine("╚════════════════════════════════════════════╝");

                int choix = Utilitaires.DemanderChoixMenu("Choix : ", 0, 5);
                Console.Clear();

                switch (choix)
                {
                    case 1:
                        MenuClients(conn);
                        break;
                    case 2:
                        MenuCuisiniers(conn);
                        break;
                    case 3:
                        MenuCommandes(conn);
                        break;
                    case 4:
                        MenuStatistiques(conn);
                        break;
                    case 5:
                        MenuExplorateur(conn);
                        break;
                    case 0:
                        Console.WriteLine("Fermeture du programme...");
                        return;
                }
            }
        }

        public static void MenuClients(MySqlConnection conn)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("╔══════════════════════════════╗");
                Console.WriteLine("║        MENU CLIENTS          ║");
                Console.WriteLine("╠══════════════════════════════╣");
                Console.WriteLine("║ 1. Ajouter un client         ║");
                Console.WriteLine("║ 2. Modifier un client        ║");
                Console.WriteLine("║ 3. Supprimer un client       ║");
                Console.WriteLine("║ 4. Afficher tous les clients ║");
                Console.WriteLine("║ 5. Retour au menu principal  ║");
                Console.WriteLine("╚══════════════════════════════╝");

                int choix = Utilitaires.DemanderChoixMenu("Choix : ", 1, 5);

                switch (choix)
                {
                    case 1: Client.AjouterClient(conn); break;
                    case 2: Client.ModifierClient(conn); break;
                    case 3: Client.SupprimerClient(conn); break;
                    case 4: Client.AfficherTousLesClients(conn); break;
                    case 5: return; // 🔁 Sortie de la boucle → retour au menu principal
                }

                Console.WriteLine("\nAppuyez sur une touche pour revenir au menu...");
                Console.ReadKey();
            }
        }

        public static void MenuCuisiniers(MySqlConnection conn)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("╔══════════════════════════════════════╗");
                Console.WriteLine("║          MENU CUISINIERS             ║");
                Console.WriteLine("╠══════════════════════════════════════╣");
                Console.WriteLine("║ 1. Ajouter un cuisinier              ║");
                Console.WriteLine("║ 2. Modifier un cuisinier             ║");
                Console.WriteLine("║ 3. Supprimer un cuisinier            ║");
                Console.WriteLine("║ 4. Voir clients servis               ║");
                Console.WriteLine("║ 5. Voir plats réalisés (fréquence)   ║");
                Console.WriteLine("║ 6. Voir plat du jour                 ║");
                Console.WriteLine("║ 7. Retour au menu principal          ║");
                Console.WriteLine("╚══════════════════════════════════════╝");

                int choix = Utilitaires.DemanderChoixMenu("Choix : ", 1, 7);

                switch (choix)
                {
                    case 1: Cuisinier.AjouterCuisinier(conn); break;
                    case 2: Cuisinier.ModifierCuisinier(conn); break;
                    case 3: Cuisinier.SupprimerCuisinier(conn); break;
                    case 4:
                        int idCuisinier = Utilitaires.DemanderEntier("Entrez l'ID du cuisinier : ");
                        Cuisinier.AfficherClientsServis(conn, idCuisinier);
                        break;
                    case 5:
                        int idCuisinierFreq = Utilitaires.DemanderEntier("Entrez l'ID du cuisinier : ");
                        Cuisinier.AfficherPlatsRealisesParFrequence(conn, idCuisinierFreq);
                        break;
                    case 6:
                        int idCuisinierJour = Utilitaires.DemanderEntier("Entrez l'ID du cuisinier : ");
                        Cuisinier.AfficherPlatDuJour(conn, idCuisinierJour);
                        break;
                    case 7: return; 
                }

                Console.WriteLine("\nAppuyez sur une touche pour revenir au menu...");
                Console.ReadKey();
            }
        }
        public static void MenuCommandes(MySqlConnection conn)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
                Console.WriteLine("║                        MENU COMMANDES                      ║");
                Console.WriteLine("╠════════════════════════════════════════════════════════════╣");
                Console.WriteLine("║ 1. Créer une commande                                      ║");
                Console.WriteLine("║ 2. Afficher le prix d'une commande                         ║");
                Console.WriteLine("║ 3. Simuler l'itinéraire de livraison                       ║");
                Console.WriteLine("║ 4. Afficher les commandes triées par prix décroissant      ║");
                Console.WriteLine("║ 5. Retour au menu principal                                ║");
                Console.WriteLine("╚════════════════════════════════════════════════════════════╝");

                int choix = Utilitaires.DemanderChoixMenu("Choix : ", 1, 5);

                switch (choix)
                {
                    case 1: Cmd.CreerCommande(conn); break;
                    case 2: Cmd.AfficherPrixCommande(conn); break;
                    case 3: Cmd.SimulerItineraireLivraison(conn); break;
                    case 4: Cmd.AfficherCommandesAvecPlats(conn); break;
                    case 5: return; // Retour au menu principal
                }

                Console.WriteLine("\nAppuyez sur une touche pour revenir au menu...");
                Console.ReadKey();
            }
        }
        public static void MenuStatistiques(MySqlConnection conn)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
                Console.WriteLine("║                     MENU STATISTIQUES                      ║");
                Console.WriteLine("╠════════════════════════════════════════════════════════════╣");
                Console.WriteLine("║ 1. Nombre de livraisons par cuisinier                      ║");
                Console.WriteLine("║ 2. Commandes sur une période                               ║");
                Console.WriteLine("║ 3. Moyenne des prix des commandes (commandes valides)      ║");
                Console.WriteLine("║ 4. Moyenne des montants clients (à partir des commandes)   ║");
                Console.WriteLine("║ 5. Commandes d'un client par nationalité de plat           ║");
                Console.WriteLine("║ 6. Retour au menu principal                                ║");
                Console.WriteLine("║                                                            ║");
                Console.WriteLine("╚════════════════════════════════════════════════════════════╝");

                int choix = Utilitaires.DemanderChoixMenu("Choix : ", 1, 7);

                switch (choix)
                {
                    case 1: Stat.NombreLivraisonsParCuisinier(conn); break;
                    case 2: Stat.AfficherCommandesParPeriode(conn); break;
                    case 3: Stat.AfficherMoyennePrixCommandes(conn); break;
                    case 4: Stat.AfficherMoyenneMontantsClients(conn); break;
                    case 5: Stat.AfficherCommandesParClientParNationalite(conn); break;
                    case 6: return;
                }
                
                Console.WriteLine("\nAppuyez sur une touche pour revenir au menu...");
                Console.ReadKey();


            }
        }
        public static void MenuExplorateur(MySqlConnection conn)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
                Console.WriteLine("║                   MENU EXPLORATION CULINAIRE               ║");
                Console.WriteLine("╠════════════════════════════════════════════════════════════╣");
                Console.WriteLine("║ 1. Recommander une nationalité inconnue                    ║");
                Console.WriteLine("║ 2. Plats jamais commandés                                  ║");
                Console.WriteLine("║ 3. Classement des clients curieux                          ║");
                Console.WriteLine("║ 4. Plats nationaux les plus populaires                     ║");
                Console.WriteLine("║ 5. Statistiques d'exploration d'un client                  ║");
                Console.WriteLine("║ 6. Retour au menu principal                                ║");
                Console.WriteLine("╚════════════════════════════════════════════════════════════╝");

                int choix = Utilitaires.DemanderChoixMenu("Choix : ", 1, 6);

                Console.Clear();

                switch (choix)
                {
                    case 1:
                        int id1 = Utilitaires.DemanderEntier("Entrez l'ID du client : ");
                        Console.WriteLine("\n╔═ RECOMMANDATION DE NATIONALITÉ ═══════════════════════════╗\n");
                        Exploration_Culinaire.RecommanderNouvelleNationalite(conn);
                        break;

                    case 2:
                        Console.WriteLine("\n╔═ PLATS JAMAIS COMMANDÉS ══════════════════════════════════╗\n");
                        Exploration_Culinaire.AfficherPlatsJamaisCommandes(conn);
                        break;

                    case 3:
                        Console.WriteLine("\n╔═ CLASSEMENT DES CLIENTS CURIEUX ══════════════════════════╗\n");
                        Exploration_Culinaire.ClientsCurieux(conn);
                        break;

                    case 4:
                        Console.WriteLine("\n╔═ PLATS NATIONAUX POPULAIRES ══════════════════════════════╗\n");
                        Exploration_Culinaire.PlatsNationauxPopulaires(conn);
                        break;

                    case 5:
                        int id5 = Utilitaires.DemanderEntier("Entrez l'ID du client : ");
                        Console.WriteLine("\n╔═ STATISTIQUES D'EXPLORATION DU CLIENT ════════════════════╗\n");
                        Exploration_Culinaire.StatistiquesExploration(conn);
                        break;

                    case 6:
                        return; 
                }

                Console.WriteLine("\nAppuyez sur une touche pour revenir au menu...");
                Console.ReadKey();
            }
        }


        public static void Pause()
        {
            Console.WriteLine("\nAppuyez sur une touche pour revenir au menu...");
            Console.ReadKey();
        }
    }


   







}
