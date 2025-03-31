using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liv_In_Paris
{
    public class Noeud<T>
    {

        private int numero;
        public T Value { get;}

        public int Numero { get { return numero; } }

        public Noeud(int num, T value)
        {
            numero = num;
            Value = value;
        }

        public string toString()
        {
            return numero.ToString();
        }
    }
}
