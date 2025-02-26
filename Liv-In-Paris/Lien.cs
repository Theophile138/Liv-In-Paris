﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Liv_In_Paris
{
    public class Lien
    {

        Noeud noeud1;
        Noeud noeud2;
        int poid;
        int direction;

        public Noeud Noeud1 { get { return noeud1; } }
        public Noeud Noeud2 { get { return noeud2; } }

        public int Direction { get { return direction; } }
        public int Poid {  get { return poid; } }

        public Lien(Noeud noeud1, Noeud noeud2, int direction = 0 , int poid = 0)
        {
            this.noeud1 = noeud1;
            this.noeud2 = noeud2;
            this.direction = direction;
            this.poid = poid;
        }

    }
}
