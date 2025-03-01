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
            Graphe myGraphe = Fichier.LoadGraph("grapheSimple.txt");
            Assert.IsTrue(myGraphe.GetNbrNoeud() == 8);
        }

        [TestMethod()]
        public void GrapheTest()
        {
            Graphe myGraphe = Fichier.LoadGraph("grapheSimple.txt");
            Assert.IsTrue(myGraphe != null);
        }

        [TestMethod()]
        public void ConnexeTest()
        {
            Graphe myGraphe = Fichier.LoadGraph("grapheSimple.txt");
            Assert.IsTrue(myGraphe.Connexe() == false);
        }

        [TestMethod()]
        public void ContientCycleTest()
        {
            Graphe myGraphe = Fichier.LoadGraph("grapheSimple.txt");
            Assert.IsTrue(myGraphe.ContientCycle(myGraphe.ListNoeud[0]) == true);
        }

        [TestMethod()]
        public void OrdreDuGrapheTest()
        {
            Graphe myGraphe = Fichier.LoadGraph("grapheSimple.txt");
            Assert.IsTrue(myGraphe.OrdreDuGraphe() == 8);
        }

        [TestMethod()]
        public void TailleDuGrapheTest()
        {
            Graphe myGraphe = Fichier.LoadGraph("grapheSimple.txt");
            Assert.IsTrue(myGraphe.TailleDuGraphe() == 9);
        }

        [TestMethod()]
        public void PondereTest()
        {
            Graphe myGraphe = Fichier.LoadGraph("grapheSimple.txt");
            Assert.IsTrue(myGraphe.Pondere() == false);
        }

        [TestMethod()]
        public void OrienteTest()
        {
            Graphe myGraphe = Fichier.LoadGraph("grapheSimple.txt");
            Assert.IsTrue(myGraphe.Oriente() == false);
        }
    }
}