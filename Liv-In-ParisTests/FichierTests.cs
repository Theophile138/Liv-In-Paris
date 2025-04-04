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
    public class FichierTests
    {
        [TestMethod()]
        public void ReadFile_TabTxtTest()
        {
            string[] myFile = Fichier<object>.ReadFile_TabTxt("test_unit.txt");

            Assert.IsTrue(myFile[0] == "salut");

        }

        [TestMethod()]
        public void CleanStringTabTest()
        {
            string[] myFile = Fichier<object>.ReadFile_TabTxt("test_unit.txt");

            string[][] file = Fichier<object>.CleanStringTab(myFile);

            if (file.Length != 2)
            {
                Assert.Fail("Pas la bonne taille");
            }

            if (file[1][0] != file[1][1])
            {
                Assert.Fail("Le split c'est pas bien fait");
            }

            Assert.IsTrue(true);

        }
    }
}