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
            Graphe myGraphe = Fichier.LoadGraph("grapheSimple.txt");

            //Graphe myGraphe = Fichier.LoadGraph("soc-karate.mtx");

            Console.WriteLine("Console activée !");
            Noeud NoeudDep = myGraphe.TrouverNoeudParNumero(NoeudDepart);


            //Graphe myGraphe = Fichier.LoadGraph("soc-karate.mtx");
            Console.WriteLine(myGraphe.TailleDuGraphe()+"  "+myGraphe.OrdreDuGraphe());

            //Console.WriteLine("Fini");

            int[,] tab2D = new int[3, 2]{
                               {3, 3},
                               {1, 1},
                               {2, 2}
                            };

            Application.Run(new InterFaceGraphique(myGraphe) { Width = 1000, Height = 1000 });
            
        }
    }
}
