using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Liv_In_Paris
{
    public partial class InterFaceGraphique<T> : Form
    {
        private Graphe<T> graphe;
        private Dictionary<int, Point> positions = new Dictionary<int, Point>();
        private Dictionary<int, Color> nodeColors = new Dictionary<int, Color>(); // Dictionnaire pour les couleurs des nœuds
        private int nodeRadius = 10; // Rayon des nœuds

        /// <summary>
        /// Constructeur de la classe Interface Graphique, demande un graphe
        /// </summary>
        /// <param name="graphe">Graphe</param>
        public InterFaceGraphique(Graphe<T> graphe)
        {
            this.graphe = graphe;
            this.Width = 1800;
            this.Height = 1000;
            this.Text = "Visualisation du Graphe";
            this.DoubleBuffered = true; // Réduit le scintillement

            if (typeof(T) == typeof(Station))
            {
                positionMetro();
            }
            else
            {
                nodeRadius = 30;
                PositionNodesCircular();  // Positionner les nœuds sans chevauchement
            }

            LoadNodeColors(); // Appel pour charger les couleurs des nœuds
            this.Paint += new PaintEventHandler(DrawGraph);
        }

        private void LoadNodeColors()
        {
            // Obtenez le tableau retourné par la méthode WelshPowell
            int[,] tableauCouleurs = graphe.WelshPowell();

            // Parcourir le tableau et associer chaque nœud à sa couleur
            for (int i = 0; i < tableauCouleurs.GetLength(0); i++)
            {
                int nodeId = tableauCouleurs[i, 0];   // Le numéro du nœud
                int couleurIndex = tableauCouleurs[i, 1];  // L'indice de la couleur

                // Associer le numéro de nœud à une couleur en fonction de l'indice
                Color couleur = GetColorFromIndex(couleurIndex);
                nodeColors[nodeId] = couleur;
            }
        }

        // Fonction qui permet de convertir un indice de couleur en une couleur réelle
        private Color GetColorFromIndex(int couleurIndex)
        {
            switch (couleurIndex)
            {
                case 0: return Color.Red;
                case 1: return Color.Blue;
                case 2: return Color.Green;
                case 3: return Color.Yellow;
                case 4: return Color.Orange;
                case 5: return Color.Pink;
                case 6: return Color.Purple;
                default: return Color.Gray;
            }
        }

        private void positionMetro()
        {
            int nodeCount = graphe.ListNoeud.Length;

            // Variables pour déterminer les coordonnées minimales et maximales
            double minLong = double.MaxValue;
            double maxLong = double.MinValue;
            double minLat = double.MaxValue;
            double maxLat = double.MinValue;

            // Calcul des limites de la longitude et latitude
            for (int i = 0; i < nodeCount; i++)
            {
                if (graphe.ListNoeud[i].Value is Station station) // Vérifier et caster
                {
                    minLong = Math.Min(minLong, station.Longitude);
                    maxLong = Math.Max(maxLong, station.Longitude);
                    minLat = Math.Min(minLat, station.Latitude);
                    maxLat = Math.Max(maxLat, station.Latitude);
                }
            }

            // Calcul des positions des nœuds sur la fenêtre
            for (int i = 0; i < nodeCount; i++)
            {
                if (graphe.ListNoeud[i].Value is Station station)
                {
                    int x = (int)(this.ClientSize.Width * (station.Longitude - minLong) / (maxLong - minLong) + nodeRadius * 2);
                    int y = (int)(this.ClientSize.Height * (station.Latitude - minLat) / (maxLat - minLat) - nodeRadius * 2);
                    int sym_y = this.ClientSize.Height - y; // Symétrie par rapport à l'axe des ordonnées

                    positions[graphe.ListNoeud[i].Numero] = new Point(x, sym_y);
                }
            }
        }

        /// <summary>
        /// Fonction pour positionner les nœuds de manière circulaire
        /// </summary>
        private void PositionNodesCircular()
        {
            int centerX = this.ClientSize.Width / 2;
            int centerY = this.ClientSize.Height / 2;
            int radius = Math.Min(centerX, centerY) - 50; // Rayon du cercle

            int nodeCount = graphe.ListNoeud.Length;
            double angleStep = 2 * Math.PI / nodeCount; // Angle entre chaque nœud

            for (int i = 0; i < nodeCount; i++)
            {
                int x = (int)(centerX + radius * Math.Cos(i * angleStep));
                int y = (int)(centerY + radius * Math.Sin(i * angleStep));
                positions[graphe.ListNoeud[i].Numero] = new Point(x, y);
            }
        }

        /// <summary>
        /// Méthode qui dessine les nœuds, les liens et gère les boucles
        /// </summary>
        private void DrawGraph(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            Pen pen = new Pen(Color.Black, 2);

            // Dessiner les boucles
            foreach (Lien<T> lien in graphe.ListLien)
            {
                if (lien.Noeud1 == lien.Noeud2)
                {
                    DrawLoop(g, positions[lien.Noeud1.Numero]);
                }
            }

            // Dessiner les liens entre les nœuds
            foreach (Lien<T> lien in graphe.ListLien)
            {
                Point p1 = positions[lien.Noeud1.Numero];
                Point p2 = positions[lien.Noeud2.Numero];

                // Dessiner les liens
                g.DrawLine(pen, p1, p2);

                if (lien.Direction == 1)
                {
                    DrawArrow(g, p1, p2); // Dessiner la flèche si la direction est spécifiée
                }
            }

            // Dessiner les nœuds
            foreach (var kvp in positions)
            {
                int nodeId = kvp.Key;
                Point pos = kvp.Value;

                // Colorier les nœuds avec la couleur associée
                Brush nodeBrush = new SolidBrush(nodeColors.ContainsKey(nodeId) ? nodeColors[nodeId] : Color.Gray);
                g.FillEllipse(nodeBrush, pos.X - nodeRadius / 2, pos.Y - nodeRadius / 2, nodeRadius, nodeRadius);

                Pen thickPen = new Pen(Color.Black, 3); // Épaisseur du contour des nœuds
                g.DrawEllipse(thickPen, pos.X - nodeRadius / 2, pos.Y - nodeRadius / 2, nodeRadius, nodeRadius);

                // Afficher le numéro du nœud
                StringFormat format = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                g.DrawString(nodeId.ToString(), new Font("Arial", 12, FontStyle.Bold), Brushes.Black, pos, format);
            }
        }

        /// <summary>
        /// Méthode qui dessine les boucles (liens qui pointent vers eux-mêmes)
        /// </summary>
        private void DrawLoop(Graphics g, Point nodePos)
        {
            int loopSize = 40;
            Pen loopPen = new Pen(Color.Black, 2);

            Rectangle rect = new Rectangle(nodePos.X - loopSize / 2, nodePos.Y - loopSize - 10, loopSize, loopSize);

            g.DrawArc(loopPen, rect, 0, 360); // Dessiner un arc pour la boucle
        }

        /// <summary>
        /// Méthode qui dessine une flèche entre deux points
        /// </summary>
        private void DrawArrow(Graphics g, Point start, Point end)
        {
            double angle = Math.Atan2(end.Y - start.Y, end.X - start.X);
            int arrowSize = 10;

            Point arrowP1 = new Point(
                (int)(end.X - arrowSize * Math.Cos(angle - Math.PI / 6)),
                (int)(end.Y - arrowSize * Math.Sin(angle - Math.PI / 6))
            );

            Point arrowP2 = new Point(
                (int)(end.X - arrowSize * Math.Cos(angle + Math.PI / 6)),
                (int)(end.Y - arrowSize * Math.Sin(angle + Math.PI / 6))
            );

            g.DrawLine(Pens.Black, end, arrowP1);
            g.DrawLine(Pens.Black, end, arrowP2);
        }

        /// <summary>
        /// Fonction non utilisée mais générée automatiquement
        /// </summary>
        private void InitializeComponent()
        {
        }
    }
}
