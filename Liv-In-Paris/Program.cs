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
            // Console.WriteLine("Salut !");

            AllocConsole(); // Ouvre la console
            Console.WriteLine("Console activée !");

            Graphe myGraphe = Fichier.LoadGraph("soc-karate.mtx");

            //Console.WriteLine("Fini");
            
            int[,] tab2D = new int[3, 2]{
                               {3, 3},
                               {1, 1},
                               {2, 2}
                            };
            int[,] tab = Graphe.Matriceadj();
            Graphe.affichermatriceadj(tab, tab2D);
            Graphe.Listeadjacence(tab2D);

            Application.Run(new InterFaceGraphique(myGraphe) { Width = 1000, Height = 1000 });
            
        }
    }
}
