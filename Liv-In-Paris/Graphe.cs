using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Liv_In_Paris
{
    public class Graphe
    {

        public static int[,] Matriceadj(int[,] t) {

            int[,] matadj = new int[getnbrnoeud(t) + 1, getnbrnoeud(t) + 1];

            /// Remplissage de la matrice
            for (int i = 0; i < t.GetLength(0); i++)
            {
                matadj[t[i,0], t[i,1]] = 1;
            }
            return matadj;

        }
        public static void Listeadjacence(int[,] t)
        {
            for (int m = 1; m <= getnbrnoeud(t); m++)
            {
                Console.Write(m + " --> ");
                for (int i = 0; i < t.GetLength(0); i++)
                { // Parcours des lignes
                    if (t[i, 0] == m)
                    {
                        Console.Write(t[i, 1] + " ");
                    }
                }
                Console.WriteLine();
            }
        }

        public static int getnbrnoeud(int[,] t) {
            int compteur = 0;
            for (int i = 0; i < t.GetLength(0); i++){
                if (t[i,0] > compteur)
                {
                    compteur = t[i,0];
                }            
            }
            return compteur;
        }
        public static void affichermatriceadj(int[,] matadj, int[,] t) {
            Console.WriteLine("Matrice d'adjacence :");
            for (int i =1; i <= getnbrnoeud(t); i++)
            {
                for (int j = 1; j <= getnbrnoeud(t); j++)
                {
                    Console.Write(matadj[i, j] + " ");
                }
                Console.WriteLine();
            }
        }

            public Noeud[] ListNoeud;
            public Lien[] ListLien;

            public Graphe(Noeud[] ListNoeud , Lien[] ListLien) 
            { 
                this.ListNoeud = ListNoeud;
                this.ListLien = ListLien;
            }
        public void ParcoursLargeur(Noeud depart)
        {
            Queue<Noeud> file = new Queue<Noeud>();
            HashSet<Noeud> visite = new HashSet<Noeud>();

            file.Enqueue(depart);
            visite.Add(depart);

            while (file.Count > 0)
            {
                Noeud courant = file.Dequeue();
                Console.WriteLine($"Visite : {courant.Numero}");

                foreach (Lien lien in ListLien)
                {
                    Noeud voisin = null;

                    if (lien.Noeud1 == courant && (lien.Direction == 0 || lien.Direction == 1))
                        voisin = lien.Noeud2;
                    else if (lien.Noeud2 == courant && (lien.Direction == 0 || lien.Direction == 2))
                        voisin = lien.Noeud1;

                    if (voisin != null && !visite.Contains(voisin))
                    {
                        visite.Add(voisin);
                        file.Enqueue(voisin);
                    }
                }
            }
        }

        public void ParcoursProfondeur(Noeud depart)
        {
            HashSet<Noeud> visite = new HashSet<Noeud>();
            ParcoursProfondeurRecursive(depart, visite);
        }
        public Noeud TrouverNoeudParNumero(int numero)
        {
            foreach (Noeud noeud in ListNoeud)
            {
                if (noeud.Numero == numero)
                {
                    return noeud;
                }
            }
            return null; 
        }
        private void ParcoursProfondeurRecursive(Noeud courant, HashSet<Noeud> visite)
        {
            visite.Add(courant);
            Console.WriteLine($"Visite : {courant.Numero}");

            foreach (Lien lien in ListLien)
            {
                Noeud voisin = null;

                if (lien.Noeud1 == courant && (lien.Direction == 0 || lien.Direction == 1))
                    voisin = lien.Noeud2;
                else if (lien.Noeud2 == courant && (lien.Direction == 0 || lien.Direction == 2))
                    voisin = lien.Noeud1;

                if (voisin != null && !visite.Contains(voisin))
                {
                    ParcoursProfondeurRecursive(voisin, visite);
                }
            }
        }
    }
}
