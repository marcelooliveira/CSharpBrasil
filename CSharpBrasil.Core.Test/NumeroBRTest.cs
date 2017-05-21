using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpBrasil.Core.Test
{
    [TestClass]
    public class NumeroBRTest
    {
        NumeroBR numeroBR;

        [TestInitialize]
        public void Initialize()
        {
            numeroBR = new NumeroBR();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void NegativoDeveGerarErro()
        {
            string extenso = numeroBR.Extenso(-1);
        }

        [TestMethod]
        public void Test0()
        {
            string extenso = numeroBR.Extenso(0);
            Assert.AreEqual("zero", extenso);
        }

        [TestMethod]
        public void Test1()
        {
            string extenso = numeroBR.Extenso(1);
            Assert.AreEqual("um", extenso);
        }

        [TestMethod]
        public void Test9()
        {
            string extenso = numeroBR.Extenso(9);
            Assert.AreEqual("nove", extenso);
        }

        [TestMethod]
        public void Test10()
        {
            string extenso = numeroBR.Extenso(10);
            Assert.AreEqual("dez", extenso);
        }

        [TestMethod]
        public void Test17()
        {
            string extenso = numeroBR.Extenso(17);
            Assert.AreEqual("dezessete", extenso);
        }

        [TestMethod]
        public void Test20()
        {
            string extenso = numeroBR.Extenso(20);
            Assert.AreEqual("vinte", extenso);
        }

        [TestMethod]
        public void Test30()
        {
            string extenso = numeroBR.Extenso(30);
            Assert.AreEqual("trinta", extenso);
        }

        [TestMethod]
        public void Test90()
        {
            string extenso = numeroBR.Extenso(90);
            Assert.AreEqual("noventa", extenso);
        }

        [TestMethod]
        public void Test39()
        {
            string extenso = numeroBR.Extenso(39);
            Assert.AreEqual("trinta e nove", extenso);
        }
    }
}
