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

            Graphe myGraphe = Fichier.LoadGraph("grapheSimple.txt");
            //Graphe myGraphe = Fichier.LoadGraph("soc-karate.mtx");
            //Graphe myGraphe = Fichier.LoadGraph("felix.txt");

            myGraphe.AfficherMatriceAdj();

            Console.WriteLine("Selectionne un noeud de depart pour tester nos fonctions");
            int NoeudDepart = int.Parse(Console.ReadLine());

            Noeud NoeudDep = myGraphe.FindNoeud(NoeudDepart);


            Console.WriteLine("Parcours en Largeur du graphe :");
            myGraphe.ParcoursLargeur(NoeudDep);

            Console.WriteLine("Parcours en Profondeur du graphe :");
            myGraphe.ParcoursProfondeurAvecPile(NoeudDep);

 
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

            Application.Run(new InterFaceGraphique(myGraphe) { Width = 1000, Height = 1000 });
        }
    }
}
