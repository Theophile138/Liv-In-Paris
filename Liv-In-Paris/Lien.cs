using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Liv_In_Paris
{
    public class Lien<T>
    {

        Noeud<T> noeud1;
        Noeud<T> noeud2;
        int poid;
        int direction;

        public Noeud<T> Noeud1 { get { return noeud1; } }
        public Noeud<T> Noeud2 { get { return noeud2; } }

        public int Direction { get { return direction; } }
        public int Poid {  get { return poid; } }

        public Lien(Noeud<T> noeud1, Noeud<T> noeud2, int direction = 0 , int poid = 0)
        {
            this.noeud1 = noeud1;
            this.noeud2 = noeud2;
            this.direction = direction;
            this.poid = poid;
        }


        /// <summary>
        /// Retourne vrai si le lien contient les deux noeuds en paramètre
        /// </summary>
        /// <param name="noeud1_"></param>
        /// <param name="noeud2_"></param>
        /// <returns></returns>
        public bool Contient(Noeud<T> noeud1_ , Noeud<T> noeud2_)
        {
            bool result = false;
            if ((noeud1_ == noeud1) && (noeud2_ == noeud2)){
                result = true;
            }

            if ((noeud1_ == noeud2) && (noeud2_ == noeud1))
            {
                result = true;
            }

            return result;
        }


        /// <summary>
        /// Retourne vrai si le lien est dans la bonne direction
        /// </summary>
        /// <param name="noeud1_">premier noeud</param>
        /// <param name="noeud2_">deuxieme noeud</param>
        /// <returns>si le lien est dans la bonne direction</returns>
        public bool BonneDirection(Noeud<T>  noeud1_, Noeud<T> noeud2_)
        {
            bool result = false;
            if ((Contient(noeud1_, noeud2_) == true) && (direction == 0))
            {
                result = true;
            }
            else
            {
                if ((noeud1_ == noeud1) && (noeud2_ == noeud2) && (direction == 1))
                {
                    result = true;
                }
                if ((noeud1_ == noeud2) && (noeud2_ == noeud1) && (direction == -1))
                {
                    result = true;
                }
            }
            return result;
        }
    }
}
