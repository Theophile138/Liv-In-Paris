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
        private Dictionary<int, Color> nodeColors = new Dictionary<int, Color>();
        private int nodeRadius = 8;

        public InterFaceGraphique(Graphe<T> graphe)
        {
            this.graphe = graphe;
            this.Width = 1800;
            this.Height = 1000;
            this.Text = "Visualisation du Graphe";
            this.DoubleBuffered = true;

            if (typeof(T) == typeof(Station))
            {
                positionMetro();
            }
            else
            {
                PositionNodesCircular();
            }

            LoadNodeColors();
            this.Paint += new PaintEventHandler(DrawGraph);
        }

        private void LoadNodeColors()
        {
            int[,] tableauCouleurs = graphe.WelshPowell();
            for (int i = 0; i < tableauCouleurs.GetLength(0); i++)
            {
                int nodeId = tableauCouleurs[i, 0];
                int couleurIndex = tableauCouleurs[i, 1];
                nodeColors[nodeId] = GetColorFromIndex(couleurIndex);
            }
        }

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
            double minLong = double.MaxValue, maxLong = double.MinValue;
            double minLat = double.MaxValue, maxLat = double.MinValue;

            for (int i = 0; i < nodeCount; i++)
            {
                if (graphe.ListNoeud[i].Value is Station station)
                {
                    minLong = Math.Min(minLong, station.Longitude);
                    maxLong = Math.Max(maxLong, station.Longitude);
                    minLat = Math.Min(minLat, station.Latitude);
                    maxLat = Math.Max(maxLat, station.Latitude);
                }
            }

            for (int i = 0; i < nodeCount; i++)
            {
                if (graphe.ListNoeud[i].Value is Station station)
                {
                    int x = (int)(this.ClientSize.Width * (station.Longitude - minLong) / (maxLong - minLong) + nodeRadius * 2);
                    int y = (int)(this.ClientSize.Height * (station.Latitude - minLat) / (maxLat - minLat) - nodeRadius * 2);
                    int sym_y = this.ClientSize.Height - y;

                    positions[graphe.ListNoeud[i].Numero] = new Point(x, sym_y);
                }
            }
        }

        private void PositionNodesCircular()
        {
            int centerX = this.ClientSize.Width / 2;
            int centerY = this.ClientSize.Height / 2;
            int radius = Math.Min(centerX, centerY) - 100;

            int nodeCount = graphe.ListNoeud.Length;
            double angleStep = 2 * Math.PI / nodeCount;

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
            Pen edgePen = new Pen(Color.Black, 2);

            // Dessiner les boucles
            foreach (Lien<T> lien in graphe.ListLien)
            {
                if (lien.Noeud1 == lien.Noeud2)
                {
                    DrawLoop(g, positions[lien.Noeud1.Numero]);
                }
            }

            // Dessiner les liens
            foreach (Lien<T> lien in graphe.ListLien)
            {
                Point p1 = positions[lien.Noeud1.Numero];
                Point p2 = positions[lien.Noeud2.Numero];

                g.DrawLine(edgePen, p1, p2);

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

                Color fillColor = nodeColors.ContainsKey(nodeId) ? nodeColors[nodeId] : Color.Gray;
                using (Brush b = new SolidBrush(fillColor))
                {
                    g.FillEllipse(b, pos.X - nodeRadius, pos.Y - nodeRadius, nodeRadius * 2, nodeRadius * 2);
                }

                using (Pen borderPen = new Pen(Color.Black, 4))
                {
                    g.DrawEllipse(borderPen, pos.X - nodeRadius, pos.Y - nodeRadius, nodeRadius * 2, nodeRadius * 2);
                }
            }
        }

        private void DrawLoop(Graphics g, Point nodePos)
        {
            int loopSize = 40;
            Pen loopPen = new Pen(Color.Black, 2);
            Rectangle rect = new Rectangle(nodePos.X - loopSize / 2, nodePos.Y - loopSize - 10, loopSize, loopSize);
            g.DrawArc(loopPen, rect, 0, 360);
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

        private void InitializeComponent() { }
    }
}
