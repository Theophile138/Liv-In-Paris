using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;

using System.Windows.Forms;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Drawing.Printing;
using System.Diagnostics;

namespace Liv_In_Paris
{
    public partial class InterFaceGraphique<T> : Form
    {
        private Graphe<T> graphe;
        private Dictionary<int, Point> positions = new Dictionary<int, Point>();

        private int nodeRadius = 10;

        /// <summary>
        /// Constructeur de la clase interface graphique, demande un graphe
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
            this.Paint += new PaintEventHandler(DrawGraph);
        }

        private Noeud<T> FindStationByName(string name)
        {
            foreach (var node in graphe.ListNoeud)
            {
                if (node.Value is Station station && station.toString() == name)
                {
                    return node;
                }
            }
            return null;
        }

        private void positionMetro()
        {
            
            int nodeCount = graphe.ListNoeud.Length;

            // Get min Longitude ; max Longitude ; min Latitude ; max Latitude

            double minLong = double.MaxValue; 
            double maxLong = double.MinValue;
            double minLat = double.MaxValue;
            double maxLat = double.MinValue;

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

            for (int i = 0; i < nodeCount; i++)
            {
                if (graphe.ListNoeud[i].Value is Station station) // Vérifier et caster
                {
                    int x = (int)(this.ClientSize.Width * (station.Longitude - minLong) / (maxLong - minLong) + nodeRadius*2);
                    int y = (int)(this.ClientSize.Height * (station.Latitude - minLat) / (maxLat - minLat) - nodeRadius * 2);
                    
                    int sym_y = this.ClientSize.Height - y; // Symétrie par rapport à l'axe des ordonnées

                    positions[graphe.ListNoeud[i].Numero] = new Point(x, sym_y);
                }
            }
        }


        /// <summary>
        /// Cette fonction met et crée les coordonnées des points des sommets de façon circulaire
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
        /// Cette methode dessine les noeuds , les liens et le boucle si un lien pointe sur lui même
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DrawGraph(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            Pen pen = new Pen(Color.Black, 2);

            foreach (Lien<T> lien in graphe.ListLien)
            {
                if (lien.Noeud1 == lien.Noeud2) 
                {
                    DrawLoop(g, positions[lien.Noeud1.Numero]);
                }
            }



            foreach (Lien<T> lien in graphe.ListLien)
            {
                Point p1 = positions[lien.Noeud1.Numero];
                Point p2 = positions[lien.Noeud2.Numero];
                //g.DrawLine(pen, p1, p2);

                if (lien.Noeud1.Value is Station station)
                {
                    Pen penMetro; // Déclaration du stylo pour la couleur

                        switch (station.Ligne)
                        {
                            case "1": penMetro = new Pen(Color.Yellow, 2); break;
                            case "2": penMetro = new Pen(Color.Blue, 2); break;
                            case "3": penMetro = new Pen(Color.Olive, 2); break;
                            case "3bis": penMetro = new Pen(Color.LightGreen, 2); break;
                            case "4": penMetro = new Pen(Color.Magenta, 2); break;
                            case "5": penMetro = new Pen(Color.Orange, 2); break;
                            case "6": penMetro = new Pen(Color.LightGreen, 2); break;
                            case "7": penMetro = new Pen(Color.Pink, 2); break;
                            case "7bis": penMetro = new Pen(Color.LightBlue, 2); break;
                            case "8": penMetro = new Pen(Color.Purple, 2); break;
                            case "9": penMetro = new Pen(Color.Gold, 2); break;
                            case "10": penMetro = new Pen(Color.SaddleBrown, 2); break;
                            case "11": penMetro = new Pen(Color.Brown, 2); break;
                            case "12": penMetro = new Pen(Color.DarkGreen, 2); break;
                            case "13": penMetro = new Pen(Color.SkyBlue, 2); break;
                            case "14": penMetro = new Pen(Color.Indigo, 2); break;
                            default: penMetro = new Pen(Color.Gray, 2); break; // Par défaut si ligne inconnue
                        }
                        g.DrawLine(penMetro, p1, p2);
                   
                  

                }
                else
                {
                    g.DrawLine(pen, p1, p2);
                }

                if (lien.Direction == 1)
                {
                    DrawArrow(g, p1, p2);
                }
            }


            foreach (var kvp in positions)
            {
                int nodeId = kvp.Key;
                Point pos = kvp.Value;

                g.FillEllipse(Brushes.White, pos.X - nodeRadius / 2, pos.Y - nodeRadius / 2, nodeRadius, nodeRadius);

                Pen thickPen = new Pen(Color.Black, 3); // Épaisseur de 3 pixels

                g.DrawEllipse(thickPen, pos.X - nodeRadius / 2, pos.Y - nodeRadius / 2, nodeRadius, nodeRadius);

                Noeud<T> myNoeud = graphe.ListNoeud[0];

                if (myNoeud.Value is not Station)
                {
                    //Afficher le numéro du nœud
                    StringFormat format = new StringFormat();
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Center;
                    g.DrawString(nodeId.ToString(), new Font("Arial", 12, FontStyle.Bold), Brushes.Black, pos, format);

                }

            }
        }

        /// <summary>
        /// fonctionne généré automatiquement mais non utilisé 
        /// </summary>
        private void InitializeComponent()
        {

        }

        /// <summary>
        /// methode qui dessine les liens des points qui pointes vers eux même
        /// </summary>
        /// <param name="g"></param>
        /// <param name="nodePos"></param>
        private void DrawLoop(Graphics g, Point nodePos)
        {
            int loopSize = 40; 
            Pen loopPen = new Pen(Color.Black, 2);

            Rectangle rect = new Rectangle(nodePos.X - loopSize / 2, nodePos.Y - loopSize - 10, loopSize, loopSize);

            g.DrawArc(loopPen, rect, 0, 360); 

        }
        /// <summary>
        /// methode qui dessine les liens entre sommets 
        /// </summary>
        /// <param name="g"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
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

            if (graphe.ListNoeud[0].Value is Station station)
            {
                Pen penMetro; // Déclaration du stylo pour la couleur

                switch (station.Ligne)
                {
                    case "1": penMetro = new Pen(Color.Yellow, 2); break;
                    case "2": penMetro = new Pen(Color.Blue, 2); break;
                    case "3": penMetro = new Pen(Color.Olive, 2); break;
                    case "3bis": penMetro = new Pen(Color.LightGreen, 2); break;
                    case "4": penMetro = new Pen(Color.Magenta, 2); break;
                    case "5": penMetro = new Pen(Color.Orange, 2); break;
                    case "6": penMetro = new Pen(Color.LightGreen, 2); break;
                    case "7": penMetro = new Pen(Color.Pink, 2); break;
                    case "7bis": penMetro = new Pen(Color.LightBlue, 2); break;
                    case "8": penMetro = new Pen(Color.Purple, 2); break;
                    case "9": penMetro = new Pen(Color.Gold, 2); break;
                    case "10": penMetro = new Pen(Color.SaddleBrown, 2); break;
                    case "11": penMetro = new Pen(Color.Brown, 2); break;
                    case "12": penMetro = new Pen(Color.DarkGreen, 2); break;
                    case "13": penMetro = new Pen(Color.SkyBlue, 2); break;
                    case "14": penMetro = new Pen(Color.Indigo, 2); break;
                    default: penMetro = new Pen(Color.Gray, 2); break; // Par défaut si ligne inconnue
                }

                g.DrawLine(penMetro, end, arrowP1);
                g.DrawLine(penMetro, end, arrowP2);

            }
            else
            {
                g.DrawLine(Pens.Black, end, arrowP1);
                g.DrawLine(Pens.Black, end, arrowP2);
            }
        }
    }
}



