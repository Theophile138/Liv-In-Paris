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

        /// <summary>
        /// permet de calculer la distance entre deux points donnes 
        /// </summary>
        /// <param name="lat1">latitude premier point en radian</param>
        /// <param name="lat2">latitude deuxieme point en radians </param>
        /// <param name="long1">longitude premier point en radian</param>
        /// <param name="long2">longitude deuxieme point en radian</param>
        /// <returns></returns>
        public double distance(double lat1, double lat2, double long1, double long2) { 
            double distance = 2*6371*Math.Asin(Math.Sqrt(Math.Pow(Math.Sin((lat1 - lat2) / 2), 2) + Math.Cos(lat1) * Math.Cos(lat2) * Math.Pow(Math.Sin((long1 - long2) / 2), 2)));
            return distance;
        }



        /// <summary>
        /// algo de djikstra
        /// </summary>
        /// <param name="debut">numero noeud de debut</param>
        /// <param name="fin">numero  noeud de fin  </param>
        /// <returns>liste des numeros de tous les points a parcourir pour le chemin le plus court entre les deux points</returns>
       public List<int> Djikstra(int debut, int fin)
{
    int[,] matadj = MatriceAdj();
    bool[] ouvert = new bool[matadj.GetLength(0)];
    int[] predecesseur = new int[matadj.GetLength(0)];
    for (int i = 0; i < matadj.GetLength(0); i++)
    {
        ouvert[i] = true;
        predecesseur[i] = -1;
    }
    
    int[] Djikstra = new int[matadj.GetLength(0)];
    for (int i = 0; i < matadj.GetLength(0); i++) {
        Djikstra[i] = int.MaxValue;
    }
    Djikstra[debut] = 0;
   
    int numNoeud = debut;
    
    while (numNoeud != -1)
    {
        Iteration(Djikstra, predecesseur, numNoeud, matadj);
        numNoeud = TrouverPlusPetiteValeur(Djikstra, ouvert);
    }
    
    List<int> chemin = ConstruireChemin(predecesseur, fin);
    return chemin;
}


        /// <summary>
        /// permet de trouver la plus petite valeur du tableau
        /// </summary>
        /// <param name="Djikstra">tableau des resultats</param>
        /// <param name="ouvert">tableau permettant de savoir si les noeuds sont ouverts</param>
        /// <returns></returns>
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
    if (noeud > -1)
    { 
        ouvert[noeud] = false; 
    }
    return noeud;
}

        /// <summary>
        /// permet de faire une iteration de l'algorithme de djikstra
        /// </summary>
        /// <param name="distances">tableau des distance a tous les points</param>
        /// <param name="predecesseur">tableau des predecesseurs</param>
        /// <param name="numNoeud">numero du noeud a partir duquel on fait l'itération</param>
        /// <param name="matadj">matrice d'adjacence du graphe que l'on traite</param>
public void Iteration(int[] distances, int[] predecesseur, int numNoeud, int[,] matadj)
{
    int n = distances.Length;

    for (int i = 0; i < n; i++)
    {
        if (matadj[numNoeud, i] != 0) 
        {
            int nouvelleDistance = distances[numNoeud] + matadj[numNoeud, i];
            if (nouvelleDistance < distances[i])
            {
                distances[i] = nouvelleDistance;
                predecesseur[i] = numNoeud;
            }
        }
    }
}


/// <summary>
/// reconstruit une liste a partir d'un tableau
/// </summary>
/// <param name="predecesseur">tableau des predecesseurs</param>
/// <param name="fin">numero du noeud de fin</param>
/// <returns>liste des predecesseurs</returns>
public List<int> ConstruireChemin(int[] predecesseur, int fin)
{
    List<int> chemin = new List<int>();
    for (int at = fin; at != -1; at = predecesseur[at])
    {
        chemin.Add(at);
    }
    chemin.Reverse();
    return chemin;
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
        /// algo de bellmann ford
        /// </summary>
        /// <param name="adj">matrice d'adjacence du graphe</param>
        /// <param name="debut">numero noeud de debut</param>
        /// <param name="fin">numero noeud de fin</param>
        /// <returns>liste des numeros de tous les points a parcourir pour le chemin le plus court entre les deux points</returns>
        public static List<int> BellmanFord(int[,] adj, int debut, int fin)
        {
            int n = adj.GetLength(0);
            int[] dist = new int[n];
            int[] pred = new int[n];
            for (int i = 0; i < n; i++)
            {
                dist[i] = int.MaxValue;
                pred[i] = -1;
            }
            dist[debut] = 0;

            for (int i = 0; i < n - 1; i++)
            {
                for (int u = 0; u < n; u++)
                {
                    for (int v = 0; v < n; v++)
                    {
                        if (adj[u, v] != 0 && dist[u] != int.MaxValue && dist[u] + adj[u, v] < dist[v])
                        {
                            dist[v] = dist[u] + adj[u, v];
                            pred[v] = u;
                        }
                    }
                }
            }

            List<int> chemin = new List<int>();
            for (int at = fin; at != -1; at = pred[at])
            {
                chemin.Insert(0, at);
            }
            return chemin;
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
        /// <returns>nombre de noeud du graphe</returns>
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
        /// <returns>booleen selon connexite du graphe</returns>
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
        /// algorithme de floydWarhall
        /// </summary>
        /// <param name="debut">numero noeud de debut</param>
        /// <param name="fin">numero noeud de fin</param>
        /// <returns>liste des numeros de tous les points a parcourir pour le chemin le plus court entre les deux points</returns>
        public List<int> FloydWarshall(int debut, int fin)
        {
            int[,] matadj = MatriceAdj();
            int n = matadj.GetLength(0);
            int[,] dist = new int[n, n];
            int[,] next = new int[n, n];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i == j) dist[i, j] = 0;
                    else if (matadj[i, j] != 0) dist[i, j] = matadj[i, j];
                    else dist[i, j] = int.MaxValue;

                    if (matadj[i, j] != 0) next[i, j] = j;
                    else next[i, j] = -1;
                }
            }

            for (int k = 0; k < n; k++)
            {
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (dist[i, k] != int.MaxValue && dist[k, j] != int.MaxValue && dist[i, k] + dist[k, j] < dist[i, j])
                        {
                            dist[i, j] = dist[i, k] + dist[k, j];
                            next[i, j] = next[i, k];
                        }
                    }
                }
            }

            List<int> chemin = new List<int>();
            if (next[debut, fin] == -1) return chemin; 

            int actuel = debut;
            while (actuel != fin)
            {
                chemin.Add(actuel);
                actuel = next[actuel, fin];
            }
            chemin.Add(fin);

            return chemin;
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

        /// <summary>
        /// Implémente l'algorithme de Welsh-Powell pour colorier les sommets du graphe.
        /// Retourne un dictionnaire associant chaque numéro de noeud à une couleur (entier).
        /// </summary>
        public int[,] WelshPowell()
        {
            int n = GetNbrNoeud();
            int[,] matAdj = MatriceAdj();
            int[] couleurs = new int[n]; 
            for (int i = 0; i < n; i++)
            {
                couleurs[i] = -1; 
            }

            List<(int noeud, int degre)> degres = new List<(int, int)>();
            for (int i = 0; i < n; i++)
            {
                int degre = 0;
                for (int j = 0; j < n; j++)
                {
                    if (matAdj[i, j] != 0) degre++;
                }
                degres.Add((i, degre));
            }

            degres.Sort((a, b) => b.degre.CompareTo(a.degre));

            int couleurActuelle = 0;

            while (true)
            {
                bool changement = false;

                foreach (var (noeud, _) in degres)
                {
                    if (couleurs[noeud] != -1) continue; 

                    bool peutColorier = true;
                    for (int j = 0; j < n; j++)
                    {
                        if (matAdj[noeud, j] != 0 && couleurs[j] == couleurActuelle)
                        {
                            peutColorier = false;
                            break;
                        }
                    }

                    if (peutColorier)
                    {
                        couleurs[noeud] = couleurActuelle;
                        changement = true;
                    }
                }

                if (!changement)
                {
                    break; 
                }

                couleurActuelle++;
            }
            int[,] tableauretour = new int[couleurs.Length, 2];
            for (int i = 0; i < couleurs.Length; i++) {
                tableauretour[i, 1] = couleurs[i];
                tableauretour[i, 0] = i;
            
            }
            return tableauretour;
        }

        /// <summary>
        /// identifie si un graphe est biparti (si le nombre de couleurs vaut 2)
        /// </summary>
        /// <returns></returns>
        public bool biparti()
        { bool b = false;
            int maximum = 0;
            int[,] tab = WelshPowell();
            for (int i = 0; i < tab.GetLength(0); i++) {
                if (tab[i, 1] > maximum) {
                    maximum = tab[i, 1];
                } 
            }
            maximum++;
            if (maximum == 2) { b = true; }
            return b;
        }


        /// <summary>
        /// permet de verifier si un graphe et planaire (sans certitude car le nombre de couleur est infeieur est inferieur ou egale a 4, il n est pas forcement planaire, cependant un nomre de couleurs strictement supérieur a 4 permet d affirmer que le graphe n'est pas planaire
        /// </summary>
        /// <returns>boolean </returns>
        public bool planaire()
        {
            bool b = true;
            int maximum = 0;
            int[,] tab = WelshPowell();
            for (int i = 0; i < tab.GetLength(0); i++)
            {
                if (tab[i, 1] > maximum)
                {
                    maximum = tab[i, 1];
                }
            }
            maximum++;
            if (maximum > 4) { b = false; }
            return b;
        }

        /// <summary>
        /// identifie les groupes independants (aillant la meme couleur)
        /// </summary>
        /// <returns>un tableau des listes de personne dans chaque groupe indépendant</returns>
        public Dictionary<int, List<int>> GroupesIndependants()
        {
            int[,] coloration = WelshPowell();  // [sommet, couleur]
            Dictionary<int, List<int>> groupes = new Dictionary<int, List<int>>();

            for (int i = 0; i < coloration.GetLength(0); i++)
            {
                int sommet = coloration[i, 0];
                int couleur = coloration[i, 1];

                if (!groupes.ContainsKey(couleur))
                {
                    groupes[couleur] = new List<int>();
                }

                groupes[couleur].Add(sommet);
            }

            return groupes;
        }



    }
}
