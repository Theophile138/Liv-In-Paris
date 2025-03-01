using Microsoft.VisualStudio.TestTools.UnitTesting;
using Liv_In_Paris;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.ApplicationServices;

namespace Liv_In_Paris.Tests
{
    [TestClass()]
    public class LienTests
    {
        [TestMethod()]
        public void LienTest()
        {
            Noeud noeud1 = new Noeud(1);
            Noeud noeud2 = new Noeud(2);

            Lien lien1 = new Lien(noeud1, noeud2, 0, 0);
            Assert.IsTrue(lien1 != null);
        }

        [TestMethod()]
        public void ContientTest()
        {
            Noeud noeud1 = new Noeud(1);
            Noeud noeud2 = new Noeud(2);

            Lien lien1 = new Lien(noeud1, noeud2, 0, 0);
            Assert.IsTrue(lien1.Contient(noeud1,noeud2)== true);
        }

        [TestMethod()]
        public void BonneDirectionTest()
        {
            Noeud noeud1 = new Noeud(1);
            Noeud noeud2 = new Noeud(2);

            Lien lien1 = new Lien(noeud1, noeud2, 0, 0);
            Assert.IsTrue(lien1.BonneDirection(noeud1,noeud2) == true);
        }
    }
}