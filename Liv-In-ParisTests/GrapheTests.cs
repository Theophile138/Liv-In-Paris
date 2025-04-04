using Microsoft.VisualStudio.TestTools.UnitTesting;
using Liv_In_Paris;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liv_In_Paris.Tests
{
    [TestClass()]
    public class GrapheTests
    {

        [TestMethod()]
        public void GetNbrNoeudTest()
        {
            Graphe<int> myGraphe = Fichier<int>.LoadGraphTxt("grapheSimple.txt");
            Assert.IsTrue(myGraphe.GetNbrNoeud() == 8);
        }

        [TestMethod()]
        public void GrapheTest()
        {
            Graphe<int> myGraphe = Fichier<int>.LoadGraphTxt("grapheSimple.txt");
            Assert.IsTrue(myGraphe != null);
        }

        [TestMethod()]
        public void testDistance()
        {
            double lat1 = 48.8566; // Paris
            double lon1 = 2.3522;
            double lat2 = 51.5074; // Londres
            double lon2 = -0.1278;
            Graphe<int> myGraphe = Fichier<int>.LoadGraphTxt("grapheSimple.txt");

            double expectedDistance = 343.4; // Distance approximative en km
            double actualDistance = myGraphe.distance(lat1, lat2, lon1, lon2);

            Assert.IsTrue(Math.Abs(expectedDistance - actualDistance) < 0.1, "La distance calculée est incorrecte.");
        }


        [TestMethod()]
        public void ConnexeTest()
        {
            Graphe<int> myGraphe = Fichier<int>.LoadGraphTxt("grapheSimple.txt");
            Assert.IsTrue(myGraphe.Connexe() == false);
        }

        [TestMethod()]
        public void ContientCycleTest()
        {
            Graphe<int> myGraphe = Fichier<int>.LoadGraphTxt("grapheSimple.txt");
            Assert.IsTrue(myGraphe.ContientCycle(myGraphe.ListNoeud[0]) == true);
        }

        [TestMethod()]
        public void OrdreDuGrapheTest()
        {
            Graphe<int> myGraphe = Fichier<int>.LoadGraphTxt("grapheSimple.txt");
            Assert.IsTrue(myGraphe.OrdreDuGraphe() == 8);
        }

        [TestMethod()]
        public void TailleDuGrapheTest()
        {
            Graphe<int> myGraphe = Fichier<int>.LoadGraphTxt("grapheSimple.txt");
            Assert.IsTrue(myGraphe.TailleDuGraphe() == 9);
        }

        [TestMethod()]
        public void PondereTest()
        {
            Graphe<int> myGraphe = Fichier<int>.LoadGraphTxt("grapheSimple.txt");
            Assert.IsTrue(myGraphe.Pondere() == false);
        }

        [TestMethod()]
        public void OrienteTest()
        {
            Graphe<int> myGraphe = Fichier<int>.LoadGraphTxt("grapheSimple.txt");
            Assert.IsTrue(myGraphe.Oriente() == false);
        }
        [TestMethod()]
        public void AffiicherpluspetitTest()
        {
            int[] tab = { 1, 2, 3, 4, 5 };
            bool[] ouvert = { false, false, true, true, true }; 
            Assert.IsTrue(Graphe<int>.TrouverPlusPetiteValeur(tab, ouvert) == 2);

        }
        [TestMethod()]

        public void TestConstruireChemin()
        {
            int[] predecesseur = { -1, 0, 1, 2, 3 };
            int fin = 4;
            List<int> expectedPath = new List<int> { 0, 1, 2, 3, 4 };
            Graphe<int> myGraphe = Fichier<int>.LoadGraphTxt("grapheSimple.txt");
            List<int> actualPath = myGraphe.ConstruireChemin(predecesseur, fin);
            CollectionAssert.AreEqual(expectedPath, actualPath);
        }
    }
}