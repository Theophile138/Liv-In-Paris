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
            Noeud<int> noeud1 = new Noeud<int>(1);
            Noeud<int> noeud2 = new Noeud<int>(2);

            var lien1 = new Lien<int>(noeud1, noeud2, 0, 0);
            Assert.IsTrue(lien1 != null);
        }

        [TestMethod()]
        public void ContientTest()
        {
            Noeud<int> noeud1 = new Noeud<int>(1);
            Noeud<int> noeud2 = new Noeud<int>(2);

            var lien1 = new Lien<int>(noeud1, noeud2, 0, 0);
            Assert.IsTrue(lien1.Contient(noeud1, noeud2) == true);
        }

        [TestMethod()]
        public void BonneDirectionTest()
        {
            Noeud<int> noeud1 = new Noeud<int>(1);
            Noeud<int> noeud2 = new Noeud<int>(2);

            var lien1 = new Lien<int>(noeud1, noeud2, 0, 0);
            Assert.IsTrue(lien1.BonneDirection(noeud1, noeud2) == true);
        }
    }
}