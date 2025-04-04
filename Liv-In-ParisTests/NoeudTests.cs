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
    public class NoeudTests
    {
        [TestMethod()]
        public void NoeudTest()
        {
            Noeud<int> noeud1 = new(1);
            Assert.IsTrue(noeud1 != null);
        }

        [TestMethod()]
        public void toStringTest()
        {
            Noeud<int> noeud1 = new(1);
            Assert.IsTrue("1" == noeud1.toString());
        }
    }
}