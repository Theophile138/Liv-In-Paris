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
    public class Graphe
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
               Noeud myNoeud2 = FindNoeud(i+1);

                for(int j = 0; j <  GetNbrNoeud(); j++)
                {
                    Noeud myNoeud1 = FindNoeud(j+1);

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
            debut--;
            int[,] matadj = MatriceAdj();
            bool[] ouvert = new bool[matadj.GetLength(0)];
            for (int i = 0; i < matadj.GetLength(0); i++)
            {
                ouvert[i] = true;
            }
            ouvert[debut] = false;
            int[] Djikstra = new int[ matadj.GetLength(0)];
            for (int i = 0; i < matadj.GetLength(0); i++) {
                Djikstra[i]=int.MaxValue;
            }
            Djikstra[debut] = 0;
           
            int numNoeud = debut;
            bool ok = true;
            int test = 8;
            while (test>3) {
                ok = false;
                for (int i = 0; i < matadj.GetLength(0); i++) { if (ouvert[i] == true) { ok = true; } }
                Djikstra = Iteration( Djikstra, numNoeud);
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
                if (ouvert[i] && Djikstra[i] < min)
                {
                    min = Djikstra[i];
                    noeud = i;
                }
            }
            return noeud;
        }
        public int[] Iteration(int[] Djikstra, int numnoeud)
        {
            int[,] matadj = MatriceAdj();
            for (int i = 1; i < Djikstra.GetLength(0); i++)
            {
                if (matadj[numnoeud, i] != 0)
                {

                    if (Djikstra[i] == int.MaxValue && matadj[numnoeud, i] != 0)
                    {
                        Djikstra[i] = matadj[numnoeud, i];
                    }
                    else
                    {
                        int nouvelleDistance = Djikstra[numnoeud] + matadj[numnoeud, i];
                        if (nouvelleDistance < Djikstra[i])
                        {
                            Djikstra[i] = nouvelleDistance;
                        }
                    }
                }

            }

            return Djikstra;
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

        public Noeud FindNoeud(int num)
        {
            Noeud myNoeud = null;
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
        public bool LienExiste(Noeud noeud1 , Noeud noeud2)
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

        public Noeud[] ListNoeud;
        public Lien[] ListLien;

        public Graphe(Noeud[] ListNoeud , Lien[] ListLien) 
        { 
            this.ListNoeud = ListNoeud;
            this.ListLien = ListLien;
        }

        /// <summary>
        /// Parcours en profondeur du graphe
        /// </summary>
        /// <param name="depart">Noeuf de départ du parcours</param>
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

        /// <summary>
        /// Retourne vrai si le graphe est connexe
        /// </summary>
        /// <returns></returns>
        public bool Connexe() {
            Stack<Noeud> pile = ParcoursProfondeur(ListNoeud[0],false);
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
        public Stack<Noeud> ParcoursProfondeur(Noeud depart, bool showConsole = true)
        {
            Stack<Noeud> pile = new Stack<Noeud>();
            HashSet<Noeud> visite = new HashSet<Noeud>();
            Stack<Noeud> resultat = new Stack<Noeud>();
            pile.Push(depart);
            visite.Add(depart);

            while (pile.Count > 0)
            {
                Noeud courant = pile.Pop();
                if (showConsole == true)
                {
                    Console.WriteLine("Sommet : " + courant.Numero);
                }
                resultat.Push(courant);

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
                        pile.Push(voisin);
                    }
                }
            }

            return resultat;
        }
        public Stack<Noeud> ParcoursProfondeurSansWriteLine(Noeud depart)
        {
            Stack<Noeud> pile = new Stack<Noeud>();
            HashSet<Noeud> visite = new HashSet<Noeud>();
            Stack<Noeud> resultat = new Stack<Noeud>();
            pile.Push(depart);
            visite.Add(depart);

            while (pile.Count > 0)
            {
                Noeud courant = pile.Pop();
                
                resultat.Push(courant);

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
        public bool ContientCycle(Noeud depart)
        {
            HashSet<Noeud> visites = new HashSet<Noeud>();
            Stack<Noeud> pile = new Stack<Noeud>();

            Dictionary<Noeud, Noeud> parents = new Dictionary<Noeud, Noeud>();
            pile.Push(depart);
            visites.Add(depart);
            parents[depart] = null;

            while (pile.Count > 0)
            {
                Noeud courant = pile.Pop();
                foreach (Lien lien in ListLien)
                {
                    Noeud voisin = null;

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
