using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liv_In_Paris
{
    internal class Graphe
    {
        public int[][] Matriceadj(int[][] t) {
            



        }
        public static int getnbrnoeud(int[][] t) {
            int compteur = 0;
            for (int i = 0; i < t.GetLength(1); i++){
                if (t[1][i] > compteur)
                {
                    compteur = t[1][i];
                }            
            }
            return compteur;
        }
    }
}
