using Liv_In_Paris;
using MySql.Data.MySqlClient;
using SQL;
using System.Diagnostics;
using System.DirectoryServices.ActiveDirectory;
using System.Runtime.InteropServices;

namespace Liv_In_Paris
{

    internal class Program
    {
        [DllImport("kernel32.dll")]
        static extern bool AllocConsole();

        [STAThread]
        static void Main(string[] args)
        {
            AllocConsole(); // Ouvre la console
            Console.WriteLine("Choisissez une partie :\n1 - BDD\n2 - Graphe");
            string value = Console.ReadLine();
            while(value != "1" && value != "2")
            {
                Console.WriteLine("Choisissez une partie :\n1 - BDD\n2 - Graphe");
                value = Console.ReadLine();
            }
            if (value == "2")
            {
                test();
            }

            if (value == "1")
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
        }

        /// <summary>
        /// fonction annexe au Main qui permet de faire l'affichge
        /// </summary>
        public static void test()
        {



            Console.Write("Fichier à charger (grapheSimple ou metro) : ");
            string fichier = Console.ReadLine();
            Graphe<int> graphe;

            if (fichier == "grapheSimple")
            {
                graphe = Fichier<int>.LoadGraphTxt("grapheSimple.Txt");
                while (true)
                {
                    Console.WriteLine("Choisissez un algorithme :\n1 - Dijkstra\n2 - Bellman-Ford\n3 - Floyd-Warshall\nA - Affichergraphe\nQ - Quitter");
                    string choix = Console.ReadLine();
                    if (choix.ToLower() == "q") break;
                    if (choix.ToLower() == "a") Application.Run(new InterFaceGraphique<int>(graphe) { Width = 1800, Height = 1000 });

                    if (choix.ToLower() != "q" && (choix.ToLower() != "a")){

                        Console.Write("Entrez le nœud de départ : ");
                        int debut = -1;
                        while (debut < 0 || debut > graphe.TailleDuGraphe())
                        {
                            debut = int.Parse(Console.ReadLine());
                        }
                        Console.Write("Entrez le nœud de fin : ");
                        int fin = -1;
                        while (fin < 0 || fin > graphe.TailleDuGraphe())
                        {
                            fin = int.Parse(Console.ReadLine());
                        }

                        List<int> chemin = null;

                        switch (choix)
                        {
                            case "1":
                                Stopwatch stopwatch = new Stopwatch();
                                stopwatch.Start();
                                chemin = graphe.Djikstra(debut, fin);
                                stopwatch.Stop();
                                Console.WriteLine($"Temps écoulé pour djikstra : {stopwatch.ElapsedMilliseconds} ms");
                                break;
                            case "2":
                                stopwatch = new Stopwatch();

                                stopwatch.Start();
                                chemin = Graphe<int>.BellmanFord(graphe.MatriceAdj(), debut, fin);
                                stopwatch.Stop();
                                Console.WriteLine($"Temps écoulé pour Bellman-Ford : {stopwatch.ElapsedMilliseconds} ms");
                                break;
                            case "3":
                                stopwatch = new Stopwatch();

                                stopwatch.Start();
                                chemin = graphe.FloydWarshall(debut, fin);
                                stopwatch.Stop();
                                Console.WriteLine($"Temps écoulé pour FloydWarshall : {stopwatch.ElapsedMilliseconds} ms");
                                break;
                            default:
                                Console.WriteLine("Choix invalide.");
                                continue;
                        }
                        if (chemin.Count == 1 && debut != fin) { Console.WriteLine(int.MinValue); }
                        else if (fin == debut) { Console.WriteLine("Noeud de depart egal au noeud d'arrivé"); }
                        else
                        {
                            Console.WriteLine("Chemin le plus court : " + string.Join(" -> ", chemin));
                        }
                    }
                }
            }
            else
            {
                Graphe<Station> myGraphe = Fichier<Station>.LoadGraphCsv("Arc_Metro.csv");
                Station.setStationListeNoeud(myGraphe, "Noeud_Metro.csv");
                while (true)
                {
                    Console.WriteLine("Choisissez un algorithme :\n1 - Dijkstra\n2 - Bellman-Ford\n3 - Floyd-Warshall\nA - Afficher graphe\nW - Welsh-Powell\nQ - Quitter");
                    string choix = Console.ReadLine();
                    if (choix.ToLower() == "q") break;
                    if (choix.ToLower() == "w")
                    {


                    }
                    if (choix.ToLower() == "a")
                    {
                        Console.WriteLine("Pour retourner a la console, fermer l'affichage du graphe !");
                        int[,] tab = myGraphe.WelshPowell();
                        
                            for (int o = 0; o < tab.GetLength(0); o++) {
                            Console.WriteLine(tab[o, 0] + "  " + tab[o, 1]);
                            }
                        
                        Application.Run(new InterFaceGraphique<Station>(myGraphe) { Width = 1800, Height = 1000 });
                    }
                        if (choix.ToLower() != "a")
                    {
                        Console.Write("Entrez le nœud de départ : ");
                        int debut = -1;
                        while (debut < 0 || debut > myGraphe.TailleDuGraphe())
                        {
                            debut = int.Parse(Console.ReadLine());
                        }
                        Console.Write("Entrez le nœud de fin : ");
                        int fin = -1;
                        while (fin < 0 || fin > myGraphe.TailleDuGraphe())
                        {
                            fin = int.Parse(Console.ReadLine());
                        }


                        List<int> chemin = null;

                    switch (choix)
                    {
                        case "1":
                            chemin =myGraphe.Djikstra(debut, fin);
                            break;
                        case "2":
                            chemin = Graphe<int>.BellmanFord(myGraphe.MatriceAdj(), debut, fin);
                            break;
                        case "3":
                            chemin = myGraphe.FloydWarshall(debut, fin);
                            break;
                        default:
                            Console.WriteLine("Choix invalide.");
                            continue;
                    }
                    if (chemin.Count == 1 && debut != fin) { Console.WriteLine(int.MinValue); }
                    else if (fin == debut) { Console.WriteLine("Noeud de depart egal au noeud d'arrivé"); }
                    else
                    {
                        Console.WriteLine("Chemin le plus court : " + string.Join(" -> ", chemin));
                        List<string> strings = new List<string>();
                            for (int i = 0; i < chemin.Count; i++)
                            {
                                strings.Add(numberToGare(myGraphe, chemin[i]));
                            }
                            Console.WriteLine("Cela correspond aux gare :" + string.Join(" -> " , strings));

                        }
                    //Application.Run(new InterFaceGraphique<Station>(myGraphe) { Width = 1800, Height = 1000 });

                }
            }

            
               
        }

        static string numberToGare(Graphe<Station> myGraphe, int number)
        {
                string result = "";
                for(int i=0; i < myGraphe.ListNoeud.Length; i++)
                {
                    if (myGraphe.ListNoeud[i].Numero == number)
                    {
                        result = myGraphe.ListNoeud[i].Value.Nom;
                    }
                }

                return result;
            }




            static void annexe(string[] args)
            {
                AllocConsole(); // Ouvre la console

                //Graphe myGraphe = Fichier.LoadGraphTxt("grapheSimple.txt");
                //Graphe myGraphe = Fichier.LoadGraph("soc-karate.mtx");
                //Graphe myGraphe = Fichier.LoadGraph("grapheSimple");

                Graphe<Station> myGraphe = Fichier<Station>.LoadGraphCsv("Arc_Metro.csv");

                myGraphe.AfficherMatriceAdj();

                Console.WriteLine("Selectionne un noeud de depart pour tester nos fonctions");
                int NoeudDepart = int.Parse(Console.ReadLine());

                Noeud<Station> NoeudDep = myGraphe.FindNoeud(NoeudDepart);


                Console.WriteLine("Parcours en Largeur du graphe :");
                myGraphe.ParcoursLargeur(NoeudDep);

                Console.WriteLine("Parcours en Profondeur du graphe :");
                myGraphe.ParcoursProfondeur(NoeudDep);


                if (myGraphe.ContientCycle(NoeudDep) == true)
                {
                    Console.WriteLine("Le graphe contient des cycles");
                }
                else
                {
                    Console.WriteLine("Le graphe ne contient pas de cycles");
                }

                if (myGraphe.Connexe() == true)
                {
                    Console.WriteLine("Le graphe est connexe");
                }
                else
                {
                    Console.WriteLine("Le graphe n'est pas connexe");
                }
                Console.WriteLine("Console activée !");
                //Noeud NoeudDep = myGraphe.TrouverNoeudParNumero(NoeudDepart);

                myGraphe.AfficherMatriceAdj();
                //Graphe myGraphe = Fichier.LoadGraph("soc-karate.mtx");
                Console.WriteLine(myGraphe.TailleDuGraphe() + "  " + myGraphe.OrdreDuGraphe());

                Console.WriteLine("La taille du graphe est : " + myGraphe.TailleDuGraphe() + " l'orde du graphe est : " + myGraphe.OrdreDuGraphe());

                if (myGraphe.Oriente() == true)
                {
                    Console.WriteLine("Le graphe est oriente");
                }
                else
                {
                    Console.WriteLine("Le graphe n'est pas oriente");
                }

                if (myGraphe.Pondere() == true)
                {
                    Console.WriteLine("Le graphe est pondere");
                }
                else
                {
                    Console.WriteLine("Le graphe n'est pas pondere");
                }

                Application.Run(new InterFaceGraphique<Station>(myGraphe) { Width = 1800, Height = 1000 });
            }
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
                                 Console.WriteLine("║ 6. Exporter les données (JSON/XML)         ║");
                                 Console.WriteLine("║ 0. Quitter                                 ║");
                                 Console.WriteLine("╚════════════════════════════════════════════╝");

                                int choix = Utilitaires.DemanderChoixMenu("Choix : ", 0, 6);
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
                                    case 6:
                                    ExporterToutesLesDonnees(conn);
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

                        
        public static void ExporterToutesLesDonnees(MySqlConnection conn)
        {
            Console.WriteLine("\n--- Export des données en cours... ---\n");

            var clients = Client.GetAllClients(conn);
            Client.ExporterClientsEnJson(clients, "clients.json");
            Client.ExporterClientsEnXml(clients, "clients.xml");

            Console.WriteLine("\n Données exportées avec succès !");
            Pause();
         }

     }

    


}


