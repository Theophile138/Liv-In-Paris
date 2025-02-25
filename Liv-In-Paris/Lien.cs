using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liv_In_Paris
{
    public class Lien
    {

        Noeud noeud1;
        Noeud noeud2;
        int poid;
        int direction;

        public Lien(Noeud noeud1, Noeud noeud2, int direction = 0 , int poid = 0)
        {
            this.noeud1 = noeud1;
            this.noeud2 = noeud2;
            this.direction = direction;
            this.poid = poid;
        }

    }
}
