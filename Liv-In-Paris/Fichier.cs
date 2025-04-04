using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Liv_In_Paris
{
    public class Fichier<T>
    {
        /// <summary>
        /// Retourne le chemin du fichier demandé a partir du dossier ou se trouve projet
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="dirName"></param>
        /// <returns></returns>
        public static string Getpath(string fileName, string dirName)
        {
            string cheminActuel = GetParentLoop(AppDomain.CurrentDomain.BaseDirectory,5);
            string cheminComplet = cheminActuel + "\\Liv-In-Paris" + "\\" + dirName  + "\\" + fileName;
            return cheminComplet;
        }
        /// <summary>
        /// Trouve le dossier parent en remontant de dossier n fois
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string GetParentLoop(string Path, int number)
        {
            string result = Path;
            for (int i = 0; i < number; i++)
            {
                result = Directory.GetParent(result).FullName;
            }
            return result;
        }
        /// <summary>
        /// Lire le fichier et retourne un tableau
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="dirName"></param>
        /// <returns></returns>
        public static string[] ReadFile_TabTxt(string fileName, string dirName = "ressource")
        {
            string Path = null;

            Path = Getpath(fileName, dirName);
     
            string[] result = null;

            try
            {
                result = File.ReadAllLines(Path);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la lecture du fichier : {ex.Message}");
            }

            return result;
        }

        /// <summary>
        /// Supprime le ligne contenant % et transforme les espaces en tableau de tableau
        /// </summary>
        /// <param name="strTab"></param>
        /// <returns></returns>
        public static string[][] CleanStringTab(string[] strTab)
        {
            string[][] result = null;

            if (strTab != null)
            {

                int Length = 0;
                for (int i = 0; i < strTab.Length; i++)
                {
                    if (strTab[i][0] != '%')
                    {
                        Length++;
                    }
                }

                result = new string[Length][];

                int index = 0;

                for (int i = 0; i < strTab.Length; i++)
                {

                    if (strTab[i][0] != '%')
                    {
                        string[] splitResult = strTab[i].Split(" ");
                        result[index] = splitResult;
                        index++;
                    }

                }
            }
            return result;
        }

        public static string[][] CleanStringCsv(string[] strTab)
        {
            string[][] result = null;

            if (strTab != null)
            {

                int Length = 0;
                for (int i = 0; i < strTab.Length; i++)
                {
                    if (strTab[i][0] != '%')
                    {
                        Length++;
                    }
                }

                result = new string[Length][];

                int index = 0;

                for (int i = 0; i < strTab.Length; i++)
                {

                    if (strTab[i][0] != '%')
                    {
                        string[] splitResult = strTab[i].Split(";");
                        result[index] = splitResult;
                        index++;
                    }

                }
            }

            return result;
        }

        public static string[][] getCleanTabCsv(string fileName, string dirName = "ressource")
        {
            string[] fileString = ReadFile_TabTxt(fileName, dirName);
            string[][] grapheString = CleanStringCsv(fileString);
            return grapheString;
        }

        /// <summary>
        /// Crée un graphe a partir d'un fichier
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="dirName"></param>
        /// <returns></returns>
        public static Graphe<T> LoadGraphTxt(string fileName, string dirName = "ressource")
        {
            Graphe<T> result = null;

            string[] fileString = ReadFile_TabTxt(fileName, dirName);

            string[][] grapheString = CleanStringTab(fileString);

            int numberNoeud = int.Parse(grapheString[0][0]);

            Noeud<T>[] ListNoeud = new Noeud<T>[numberNoeud];

            for(int i = 0; i < ListNoeud.Length ; i++) 
            {
                ListNoeud[i] = new Noeud<T>(i+1);
            }

            int numberLien = int.Parse(grapheString[0][2]);

            Lien<T>[] ListLien = new Lien<T>[numberLien];

            int index = 0;

            for(int i = 1; i < grapheString.Length; i++)
            {
                int Noeud1 = int.Parse(grapheString[i][0]);
                int Noeud2 = int.Parse(grapheString[i][1]);
                
                ListLien[index] = new Lien<T>(ListNoeud[Noeud1-1], ListNoeud[Noeud2-1], 0, 0);
                index++;
            }

            result = new Graphe<T>(ListNoeud, ListLien);

            return result;
        }

        public static Graphe<T> LoadGraphCsv(string fileName, string dirName = "ressource")
        {
            Graphe<T> result = null;

            string[] fileString = ReadFile_TabTxt(fileName, dirName);

            string[][] grapheString = CleanStringCsv(fileString);

            //int numberNoeud = int.Parse(grapheString[0][0]);

            //Fontion pour obtenir le nombre max de noeud
            int numberNoeud = 0;
            for(int i = 0; i < grapheString.Length; i++)
            {
                if (int.Parse(grapheString[i][0]) > numberNoeud)
                {
                    numberNoeud = int.Parse(grapheString[i][0]);
                }
            }

            Noeud<T>[] ListNoeud = new Noeud<T>[numberNoeud];

            for (int i = 0; i < ListNoeud.Length; i++)
            {
                ListNoeud[i] = new Noeud<T>(i + 1);
            }


            List<Lien<T>> liens = new List<Lien<T>>();

            int positionStationSuiv = 3;
            int positionStationPrec = 2;

            int positionTempsEntreStation = 4;

            for (int i = 0; i < grapheString.Length; i++)
            {
                int Noeud1 = int.Parse(grapheString[i][0]);

                if (int.TryParse(grapheString[i][positionStationSuiv].ToString(), out int value2))
                {
                    int Noeud2 = value2; // Plus besoin de refaire un int.Parse
                    liens.Add(new Lien<T>(ListNoeud[Noeud1 - 1], ListNoeud[Noeud2 - 1], 1, int.Parse(grapheString[i][positionTempsEntreStation])));
                }

                if (int.TryParse(grapheString[i][positionStationPrec].ToString(), out int value_2))
                {
                    int Noeud2 = value_2; // Plus besoin de refaire un int.Parse
                    liens.Add(new Lien<T>(ListNoeud[Noeud1 - 1], ListNoeud[Noeud2 - 1], 1, int.Parse(grapheString[i][positionTempsEntreStation])));
                }
            }

            int Temps_de_Changement = 4;

            for (int i = 0; i < grapheString.Length; i++)
            {
                for (int j = 0; j < grapheString.Length; j++)
                {
                    if ((grapheString[i][1] == grapheString[j][1])&& (j != i))
                    {

                        int Noeud1 = int.Parse(grapheString[i][0]);
                        int Noeud2 = int.Parse(grapheString[j][0]);

                        liens.Add(new Lien<T>(ListNoeud[Noeud1 - 1], ListNoeud[Noeud2 - 1], 0, int.Parse(grapheString[i][Temps_de_Changement])));
                    }

                }

                    
                
            }

            Lien<T>[] ListLien = new Lien<T>[liens.Count];

            for(int i = 0; i < liens.Count; i++)
            {
                ListLien[i] = liens[i];
            }
            
            result = new Graphe<T>(ListNoeud, ListLien);

            return result;
        }

    }
}
