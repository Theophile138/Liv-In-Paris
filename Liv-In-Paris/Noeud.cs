using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liv_In_Paris
{
    public class Noeud
    {

        private int numero;

        public int Numero { get { return numero; } }
        

        public Noeud(int num) 
        { 
            numero = num;
        }

        public string toString()
        {
            return numero.ToString();
        }
    }
}
