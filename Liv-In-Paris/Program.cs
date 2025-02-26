namespace Liv_In_Paris
{
    internal class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("Salut !");

            Graphe myGraphe = Fichier.LoadGraph("soc-karate.mtx");

            Application.Run(new InterFaceGraphique(myGraphe) { Width = 1000, Height = 1000 });
            
        }
    }
}
