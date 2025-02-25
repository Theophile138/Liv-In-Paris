namespace Liv_In_Paris
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Salut !");

            Graphe myGraphe = Fichier.LoadGraph("soc-karate.mtx");

            Console.WriteLine("Fini");
        }
    }
}
