using System.Runtime.InteropServices;

namespace Liv_In_Paris
{
    internal class Program
    {
        [DllImport("kernel32.dll")]
        static extern bool AllocConsole();

        [STAThread]
        static void Main(string[] args) {
            AllocConsole(); // Ouvre la console


            // Graphe<Station> myGraphe = Fichier<Station>.LoadGraphCsv("Arc_Metro.csv");
            //Station.setStationListeNoeud(myGraphe, "Noeud_Metro.csv");
             Graphe<int> myGraphe = Fichier<int>.LoadGraphTxt("grapheSimple.txt");

            myGraphe.AfficherMatriceAdj();

            int debut = int.Parse(Console.ReadLine());
            int fin = int.Parse(Console.ReadLine());

            List<int> liste =myGraphe.Djikstra(debut, fin);


          
        
            Console.WriteLine("Chemin le plus court : " + string.Join(" -> ", liste));


            //Graphe<Station> myGraphe = Fichier<Station>.LoadGraphCsv("Arc_Metro.csv");
            //Station.setStationListeNoeud(myGraphe, "Noeud_Metro.csv");

            Application.Run(new InterFaceGraphique<int>(myGraphe) { Width = 1800, Height = 1000 });


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
            Console.WriteLine(myGraphe.TailleDuGraphe()+"  "+myGraphe.OrdreDuGraphe());

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
