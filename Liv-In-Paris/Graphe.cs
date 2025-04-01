using Microsoft.VisualBasic.Devices;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Liv_In_Paris
{
    public class Graphe<T>
    {

        /// <summary>
        /// Retourne la matrice d'adjacence du graphe
        /// </summary>
        /// <returns></returns>
        public int[,] MatriceAdj()
        {

            int[,] matAdj = new int[GetNbrNoeud(), GetNbrNoeud()];

            // Remplissage de la matrice
            for (int i = 0; i < GetNbrNoeud(); i++)
            {
               Noeud<T> myNoeud2 = FindNoeud(i+1);

                for(int j = 0; j <  GetNbrNoeud(); j++)
                {
                    Noeud<T> myNoeud1 = FindNoeud(j+1);

                    if (LienExiste(myNoeud1,myNoeud2) == true)
                    {
                        matAdj[i, j] = 1;
                    }
                    else
                    {
                        matAdj[i, j] =  0;
                    }

                }
            }
            return matAdj;

        }

        public int[] Djikstra(int debut)
        {
            
            int[,] matadj = MatriceAdj();
            bool[] ouvert = new bool[matadj.GetLength(0)];
            for (int i = 0; i < matadj.GetLength(0); i++)
            {
                ouvert[i] = true;
            }
            
            int[] Djikstra = new int[ matadj.GetLength(0)];
            for (int i = 0; i < matadj.GetLength(0); i++) {
                Djikstra[i]=int.MaxValue;
            }
            Djikstra[debut] = 0;
           
            int numNoeud = debut;
            bool ok = true;
            int test = 8;
            while (numNoeud!=-1) {
                ok = false;
                for (int i = 0; i < matadj.GetLength(0); i++) { if (ouvert[i] == true) { ok = true; } }
                Iteration( Djikstra, numNoeud, matadj);
                numNoeud=TrouverPlusPetiteValeur(Djikstra, ouvert);
                test--;
            }
            return Djikstra;
        }
        public static int TrouverPlusPetiteValeur(int[] Djikstra, bool[] ouvert)
        {
            int min = int.MaxValue;
            int noeud = -1;

            for (int i = 0; i < Djikstra.Length; i++)
            {
                if (ouvert[i] == true && Djikstra[i] < min)
                {
                    min = Djikstra[i];
                    noeud = i;
                }
            }
            if (noeud > -1) { 
            ouvert[noeud] = false; }
            return noeud;
        }
        public void Iteration(int[] distances, int numNoeud, int[,] matadj)
        {
            int n = distances.Length;

            for (int i = 0; i < n; i++)
            {
                if (matadj[numNoeud, i] != 0) // Si une arête existe
                {
                    int nouvelleDistance = distances[numNoeud] + matadj[numNoeud, i];
                    if (nouvelleDistance < distances[i])
                    {
                        distances[i] = nouvelleDistance;
                    }
                }
            }
        }

        /// <summary>
        /// Affiche la matrice d'adjacence du graphe
        /// </summary>
        public void AfficherMatriceAdj()
        {
            int[,] matadj = MatriceAdj();
            
            Console.WriteLine("Matrice d'adjacence :");
            for (int i = 0; i < GetNbrNoeud(); i++)
            {
                for (int j = 0; j < GetNbrNoeud(); j++)
                {
                    Console.Write(matadj[i, j] + " ");
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Retourne le noeud correspondant au numero
        /// </summary>
        /// <param name="num">numéro du noeud que l'on souhaite retourner</param>
        /// <returns></returns>
        public Noeud<T> FindNoeud(int num)
        {
            Noeud<T> myNoeud = null;
            for (int i = 0; i < ListNoeud.Length; i++)
            {

                if (ListNoeud[i].Numero == num)
                {
                    myNoeud = ListNoeud[i];
                }

            }
            return myNoeud;
        }

        /// <summary>
        /// Retourne vrai si le lien existe entre les deux noeuds
        /// </summary>
        /// <param name="noeud1">premier noeud</param>
        /// <param name="noeud2">deuxieme noeud</param>
        /// <returns></returns>
        public bool LienExiste(Noeud<T> noeud1 , Noeud<T> noeud2)
        {
            bool result = false;
            for (int i = 0; i < ListLien.Length; i++)
            {
                if (ListLien[i].Contient(noeud1, noeud2) == true)
                {
                    if (ListLien[i].BonneDirection(noeud1,noeud2) == true)
                    {
                        result = true;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Retourne le nombre de noeud du graphe
        /// </summary>
        /// <returns></returns>
        public int GetNbrNoeud()
        {
            return ListNoeud.Length;
        }

        public Noeud<T>[] ListNoeud;
        public Lien<T>[] ListLien;

        public Graphe(Noeud<T> [] ListNoeud , Lien<T>[] ListLien) 
        { 
            this.ListNoeud = ListNoeud;
            this.ListLien = ListLien;
        }

        /// <summary>
        /// Parcours en profondeur du graphe
        /// </summary>
        /// <param name="depart">Noeuf de départ du parcours</param>
        public void ParcoursLargeur(Noeud<T> depart)
        {
            Queue<Noeud<T>> file = new Queue<Noeud<T>>();
            HashSet<Noeud<T>> visite = new HashSet<Noeud<T>>();

            file.Enqueue(depart);
            visite.Add(depart);

            while (file.Count > 0)
            {
                Noeud<T> courant = file.Dequeue();
                Console.WriteLine($"Visite : {courant.Numero}");

                foreach (Lien<T> lien in ListLien)
                {
                    Noeud<T> voisin = null;

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

        /// <summary>
        /// Retourne vrai si le graphe est connexe
        /// </summary>
        /// <returns></returns>
        public bool Connexe() {
            Stack<Noeud<T>> pile = ParcoursProfondeur(ListNoeud[0],false);
            int nbrnoeud = GetNbrNoeud();
            if (pile.Count == nbrnoeud)
            {
                return true;
            }
            else { return false; }
        }


        /// <summary>
        /// Parcours en profondeur du graphe
        /// </summary>
        /// <param name="depart"></param>
        /// <returns></returns>
        public Stack<Noeud<T>> ParcoursProfondeur(Noeud<T> depart, bool showConsole = true)
        {
            Stack<Noeud<T>> pile = new Stack<Noeud<T>>();
            HashSet<Noeud<T>> visite = new HashSet<Noeud<T>>();
            Stack<Noeud<T>> resultat = new Stack<Noeud<T>>();
            pile.Push(depart);
            visite.Add(depart);

            while (pile.Count > 0)
            {
                Noeud<T> courant = pile.Pop();
                if (showConsole == true)
                {
                    Console.WriteLine("Sommet : " + courant.Numero);
                }
                resultat.Push(courant);

                foreach (Lien<T> lien in ListLien)
                {
                    Noeud<T> voisin = null;

                    if (lien.Noeud1 == courant && (lien.Direction == 0 || lien.Direction == 1))
                        voisin = lien.Noeud2;
                    else if (lien.Noeud2 == courant && (lien.Direction == 0 || lien.Direction == 2))
                        voisin = lien.Noeud1;

                    if (voisin != null && !visite.Contains(voisin))
                    {
                        visite.Add(voisin);  
                        pile.Push(voisin);
                    }
                }
            }

            return resultat;
        }
        public Stack<Noeud<T>> ParcoursProfondeurSansWriteLine(Noeud<T> depart)
        {
            Stack<Noeud<T>> pile = new Stack<Noeud<T>>();
            HashSet<Noeud<T>> visite = new HashSet<Noeud<T>>();
            Stack<Noeud<T>> resultat = new Stack<Noeud<T>>();
            pile.Push(depart);
            visite.Add(depart);

            while (pile.Count > 0)
            {
                Noeud<T> courant = pile.Pop();
                
                resultat.Push(courant);

                foreach (Lien<T> lien in ListLien)
                {
                    Noeud<T> voisin = null;

                    if (lien.Noeud1 == courant && (lien.Direction == 0 || lien.Direction == 1))
                        voisin = lien.Noeud2;
                    else if (lien.Noeud2 == courant && (lien.Direction == 0 || lien.Direction == 2))
                        voisin = lien.Noeud1;

                    if (voisin != null && !visite.Contains(voisin))
                    {
                        visite.Add(voisin);
                        pile.Push(voisin);
                    }
                }
            }

            return resultat;
        }

        /// <summary>
        /// Retourne vrai si le graphe contient un cycle
        /// </summary>
        /// <param name="depart">Noeud de départ pour chercher les cycles</param>
        /// <returns></returns>
        public bool ContientCycle(Noeud<T> depart)
        {
            HashSet<Noeud<T>> visites = new HashSet<Noeud<T>>();
            Stack<Noeud<T>> pile = new Stack<Noeud<T>>();

            Dictionary<Noeud<T>, Noeud<T>> parents = new Dictionary<Noeud<T>, Noeud<T>>();
            pile.Push(depart);
            visites.Add(depart);
            parents[depart] = null;

            while (pile.Count > 0)
            {
                Noeud<T> courant = pile.Pop();
                foreach (Lien<T> lien in ListLien)
                {
                    Noeud<T> voisin = null;

                    if (lien.Noeud1 == courant && (lien.Direction == 0 || lien.Direction == 1))
                        voisin = lien.Noeud2;
                    else if (lien.Noeud2 == courant && (lien.Direction == 0 || lien.Direction == 2))
                        voisin = lien.Noeud1;

                    if (voisin != null)
                    {
                        if (visites.Contains(voisin) && parents[courant] != voisin)
                        {
                            return true;
                        }

                        if (!visites.Contains(voisin))
                        {
                            pile.Push(voisin);
                            visites.Add(voisin);
                            parents[voisin] = courant;
                        }
                    }
                }
            }


            return false;
        }



        /// <summary>
        /// Retourne l'ordre du graphe (nombre de sommets)
        /// </summary>
        /// <returns></returns>
        public int OrdreDuGraphe()
        {
            return ListNoeud.Length;
        }

        /// <summary>
        /// Retourne la taille du graphe (nombre d'arêtes)
        /// </summary>
        /// <returns></returns>
        public int TailleDuGraphe() {
            return ListLien.Length;
        }

        /// <summary>
        /// Retourne true si le graphe est pondéré
        /// </summary>
        /// <returns></returns>
        public bool Pondere()
        {
            int compteur = 0;
            for (int i = 0; i < ListLien.Length; i++) {
                compteur += ListLien[i].Poid;
            }
            if (compteur != 0)
            { 
                return true; 
            }
            else 
            {
                return false; 
            }
        }

        /// <summary>
        /// Retourne true si le graphe est orienté
        /// </summary>
        /// <returns></returns>
        public bool Oriente()
        {
            bool b = false;
            for (int i = 0; i < ListLien.Length; i++)
            {
                if(ListLien[i].Direction != 0) {
                    b = true;
                }
            }
            return b;
        }
    }
}
