namespace Liv_In_Paris
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Console.WriteLine("Salut !");

            // Graphe myGraphe = Fichier.LoadGraph("soc-karate.mtx");

            //Console.WriteLine("Fini");
            int[,] tab2D = new int[3, 2]{
                               {3, 3},
                               {1, 1},
                               {2, 2}
                            };
            int[,] tab = Graphe.Matriceadj(tab2D);
            Graphe.affichermatriceadj(tab, tab2D);
            Graphe.Listeadjacence(tab2D);

        }
    }
}
