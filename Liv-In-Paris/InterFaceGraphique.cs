using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;

using System.Windows.Forms;
using System.Drawing;

namespace Liv_In_Paris
{
    public partial class InterFaceGraphique : Form
    {
        private Graphe graphe;
        private Dictionary<int, Point> positions = new Dictionary<int, Point>();

        /// <summary>
        /// Constructeur de la clase interface graphique, demande un graphe
        /// </summary>
        /// <param name="graphe">Graphe</param>
        public InterFaceGraphique(Graphe graphe)
        {
            this.graphe = graphe;
            this.Width = 1000;
            this.Height = 1000;
            this.Text = "Visualisation du Graphe";
            this.DoubleBuffered = true; // Réduit le scintillement

            PositionNodesCircular();  // Positionner les nœuds sans chevauchement
            this.Paint += new PaintEventHandler(DrawGraph);
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
            int nodeRadius = 30;


            foreach (Lien lien in graphe.ListLien)
            {
                if (lien.Noeud1 == lien.Noeud2) 
                {
                    DrawLoop(g, positions[lien.Noeud1.Numero]);
                }
            }



            foreach (Lien lien in graphe.ListLien)
            {
                Point p1 = positions[lien.Noeud1.Numero];
                Point p2 = positions[lien.Noeud2.Numero];
                g.DrawLine(pen, p1, p2);

 
                if (lien.Direction == 1)
                {
                    DrawArrow(g, p1, p2);
                }
            }


            foreach (var kvp in positions)
            {
                int nodeId = kvp.Key;
                Point pos = kvp.Value;

                g.FillEllipse(Brushes.LightBlue, pos.X - nodeRadius / 2, pos.Y - nodeRadius / 2, nodeRadius, nodeRadius);
                g.DrawEllipse(Pens.Black, pos.X - nodeRadius / 2, pos.Y - nodeRadius / 2, nodeRadius, nodeRadius);

                // Afficher le numéro du nœud
                StringFormat format = new StringFormat();
                format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Center;
                g.DrawString(nodeId.ToString(), new Font("Arial", 12, FontStyle.Bold), Brushes.Black, pos, format);
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

            g.DrawLine(Pens.Black, end, arrowP1);
            g.DrawLine(Pens.Black, end, arrowP2);
        }
    }
}



