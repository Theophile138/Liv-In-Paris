using Liv_In_Paris;
using MySql.Data.MySqlClient;
using SQL;
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


            // Partie pour load le metro
            //----------------------------------------------------------------------------------------------------------
            //Graphe<Station> myGraphe = Fichier<Station>.LoadGraphCsv("Arc_Metro.csv");
            //Station.setStationListeNoeud(myGraphe, "Noeud_Metro.csv");
            //Application.Run(new InterFaceGraphique<Station>(myGraphe) { Width = 1800, Height = 1000 });
            //----------------------------------------------------------------------------------------------------------


            // Partie pour load un graphe simple
            //----------------------------------------------------------------------------------------------------------

            test();


            

            Program.MenuPrincipal(conn);


            //----------------------------------------------------------------------------------------------------------






        }

        /// <summary>
        /// fonction annexe au Main qui permet de faire l'affichge
        /// </summary>
        public static void test()
        {



            Console.Write("Fichier à charger (grapheSimple ou metro) : ");
            string fichier = Console.ReadLine();
            Graphe<int> graphe;

            if (fichier == "graphesimple")
            {
                graphe = Fichier<int>.LoadGraphTxt("grapheSimple.Txt");
                while (true)
                {
                    Console.WriteLine("Choisissez un algorithme :\n1 - Dijkstra\n2 - Bellman-Ford\n3 - Floyd-Warshall\nA - Affichergraphe\nQ - Quitter");
                    string choix = Console.ReadLine();
                    if (choix.ToLower() == "q") break;
                    if (choix.ToLower() == "a") Application.Run(new InterFaceGraphique<int>(graphe) { Width = 1800, Height = 1000 });


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
            else
            {
                Graphe<Station> myGraphe = Fichier<Station>.LoadGraphCsv("Arc_Metro.csv");
                Station.setStationListeNoeud(myGraphe, "Noeud_Metro.csv");
                while (true)
                {
                    Console.WriteLine("Choisissez un algorithme :\n1 - Dijkstra\n2 - Bellman-Ford\n3 - Floyd-Warshall\nA - Afficher graphe\nQ - Quitter");
                    string choix = Console.ReadLine();
                    if (choix.ToLower() == "q") break;
                    if (choix.ToLower() == "a")
                    {
                        Console.WriteLine("Pour retourner a la console, fermer l'affichage du graphe !");

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
                    }
                    Application.Run(new InterFaceGraphique<Station>(myGraphe) { Width = 1800, Height = 1000 });

                }
            }
            
               
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
    }
}
