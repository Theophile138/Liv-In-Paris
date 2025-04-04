using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQL
{
    internal class Utilitaires
    {
        public static int DemanderChoixMenu(string message, int min, int max)
        {
            int choix;
            while (true)
            {
                Console.Write(message);
                string input = Console.ReadLine();

                if (!int.TryParse(input, out choix))
                {
                    Console.WriteLine(" Veuillez entrer un entier valide.");
                    continue;
                }

                if (choix < min || choix > max)
                {
                    Console.WriteLine($" Veuillez entrer un nombre entre {min} et {max}.");
                    continue;
                }

                return choix;
            }
        }

        public static int DemanderEntier(string message)
        {
            int valeur;
            while (true)
            {
                Console.Write(message);
                string input = Console.ReadLine();

                if (!int.TryParse(input, out valeur))
                {
                    Console.WriteLine(" Veuillez entrer un nombre entier valide.");
                    continue;
                }

                return valeur;
            }
        }










    }
}
