using System;
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
        public bool Connexe() {
            Stack<Noeud> pile = ParcoursProfondeurAvecPile(ListNoeud[0]);
            int nbrnoeud = GetNbrNoeud();
            if (pile.Count == nbrnoeud)
            {
                return true;
            }
            else { return false; }
        }
        public Stack<Noeud> ParcoursProfondeurAvecPile(Noeud depart)
        {
            Stack<Noeud> pile = new Stack<Noeud>();
            HashSet<Noeud> visite = new HashSet<Noeud>();
            Stack<Noeud> resultat = new Stack<Noeud>();  

            pile.Push(depart);

            while (pile.Count > 0)
            {
                Noeud courant = pile.Pop();

                if (!visite.Contains(courant))
                {
                    visite.Add(courant);
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
                            pile.Push(voisin);
                        }
                    }
                }
            }

            return resultat; 
        }


        public bool ContientCycleAvecPile(Noeud depart)
        {
            // Un ensemble pour garder une trace des nœuds visités
            HashSet<Noeud> visites = new HashSet<Noeud>();

            // Une pile pour effectuer le parcours DFS
            Stack<Noeud> pile = new Stack<Noeud>();

            // Une map pour suivre le parent de chaque nœud dans le parcours
            Dictionary<Noeud, Noeud> parents = new Dictionary<Noeud, Noeud>();

            // On commence le DFS depuis le nœud de départ
            pile.Push(depart);
            visites.Add(depart);
            parents[depart] = null;  // Le parent du nœud de départ est null

            while (pile.Count > 0)
            {
                Noeud courant = pile.Pop();  // On prend le nœud du dessus de la pile

                // Exploration des voisins du nœud courant
                foreach (Lien lien in ListLien)
                {
                    Noeud voisin = null;

                    // Vérifier si l'arête correspond au nœud courant et au voisin
                    if (lien.Noeud1 == courant && (lien.Direction == 0 || lien.Direction == 1))
                        voisin = lien.Noeud2;
                    else if (lien.Noeud2 == courant && (lien.Direction == 0 || lien.Direction == 2))
                        voisin = lien.Noeud1;

                    // Si le voisin a déjà été visité et ce n'est pas le parent, alors il y a un cycle
                    if (voisin != null)
                    {
                        if (visites.Contains(voisin) && parents[courant] != voisin)
                        {
                            return true;  // Cycle détecté
                        }

                        // Si le voisin n'a pas encore été visité, l'ajouter à la pile et marquer comme visité
                        if (!visites.Contains(voisin))
                        {
                            pile.Push(voisin);
                            visites.Add(voisin);
                            parents[voisin] = courant;  // Le parent du voisin est le nœud courant
                        }
                    }
                }
            }

            // Aucun cycle détecté
            return false;
        }
    }
}
