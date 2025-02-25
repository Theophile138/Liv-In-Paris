using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liv_In_Paris
{
    public class Graphe
    {

        Noeud[] ListNoeud;
        Lien[] ListLien;

        public Graphe(Noeud[] ListNoeud , Lien[] ListLien) 
        { 
            this.ListNoeud = ListNoeud;
            this.ListLien = ListLien;
        }


    }
}
