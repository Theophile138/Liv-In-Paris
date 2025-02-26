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

        public InterFaceGraphique(Graphe graphe)
        {
            this.graphe = graphe;
            this.Width = 800;
            this.Height = 600;
            this.Text = "Visualisation du Graphe";
            this.DoubleBuffered = true; // Réduit le scintillement

            PositionNodesCircular();  // Positionner les nœuds sans chevauchement
            this.Paint += new PaintEventHandler(DrawGraph);
        }

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

        private void DrawGraph(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            Pen pen = new Pen(Color.Black, 2);
            int nodeRadius = 30;

            // Dessiner les liens
            foreach (Lien lien in graphe.ListLien)
            {
                Point p1 = positions[lien.Noeud1.Numero];
                Point p2 = positions[lien.Noeud2.Numero];
                g.DrawLine(pen, p1, p2);

                // Dessiner la flèche si le lien est dirigé
                if (lien.Direction == 1)
                {
                    DrawArrow(g, p1, p2);
                }
            }

            // Dessiner les nœuds
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



