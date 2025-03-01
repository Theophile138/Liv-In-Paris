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
            Console.WriteLine("selectionne un noeud de depart pour tester nos fonctions");
            int NoeudDepart = int.Parse(Console.ReadLine());


            //Graphe myGraphe = Fichier.LoadGraph("grapheSimple.txt");
            //Graphe myGraphe = Fichier.LoadGraph("soc-karate.mtx");

            Graphe myGraphe = Fichier.LoadGraph("felix.txt");

            Console.WriteLine("Console activée !");

            Noeud NoeudDep = myGraphe.TrouverNoeudParNumero(NoeudDepart);

            myGraphe.ParcoursLargeur(NoeudDep);

            //myGraphe.ParcoursProfondeur(NoeudDep);

            myGraphe.AfficherMatriceAdj();

            Application.Run(new InterFaceGraphique(myGraphe) { Width = 1000, Height = 1000 });
            
        }
    }
}
