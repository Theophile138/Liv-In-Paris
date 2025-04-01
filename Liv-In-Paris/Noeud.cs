using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;

namespace Liv_In_Paris
{
    public class Noeud<T>
    {

        private int numero;
        private T value;

        public T Value { get { return value; } }

        public int Numero { get { return numero; } }

        public Noeud(int num)
        {
            numero = num;
            value = default;
        }

        public void setValue(T value)
        {
            this.value = value;
        }

        public string toString()
        {
            return numero.ToString();
        }
    }
}
