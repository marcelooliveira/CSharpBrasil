using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharpBrasil.Test
{
    [TestClass]
    public class CPFTest
    {
        [TestMethod]
        public void Test_23881228217_IsValid()
        {
            Assert.AreEqual(true, CPF.IsValid("23881228217"));
        }

        [TestMethod]
        public void Test_25588580842_IsValid()
        {
            Assert.AreEqual(true, CPF.IsValid("25588580842"));
        }

        [TestMethod]
        public void Test_25588580823_IsNotValid()
        {
            Assert.AreNotEqual(true, CPF.IsValid("25588580823"));
        }
    }
}
