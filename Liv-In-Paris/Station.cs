using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liv_In_Paris
{
    internal class Station
    {

        private string nom;
        private string ligne;
        private double latitude;
        private double longitude;
        private string Commune;
        private int CommuneCode;

        public double Latitude { get { return latitude; } }
        public double Longitude { get { return longitude; } }

        public string Ligne { get { return ligne; } }

        public string Nom { get { return nom; } }

        public Station(string ligne, string nom, double longitude, double latitude, string Commune, int CommuneCode)
        {
            this.nom = nom;
            this.ligne = ligne;
            this.latitude = latitude;
            this.longitude = longitude;
            this.Commune = Commune;
            this.CommuneCode = CommuneCode;
        }

        public string toString()
        {
            return nom;
        }

        public static void setStationListeNoeud(Graphe<Station> myGraphe, string fichier)
        {
            string[][] tab = Fichier<Station>.getCleanTabCsv(fichier);

            for(int i = 0; i < tab.Length; i++)
            {
                Noeud<Station> myNoeud = myGraphe.FindNoeud(int.Parse(tab[i][0]));
                Station myStation = new Station(tab[i][1], tab[i][2], double.Parse(tab[i][3]), double.Parse(tab[i][4]), tab[i][5], int.Parse(tab[i][6]));
                myNoeud.setValue(myStation);
            }

        }

    }
}
