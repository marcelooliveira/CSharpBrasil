using CSharpBrasil;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CSharpBrasil.Core.Test
{
    [TestClass]
    public class CPFTest
    {
        private CPFValidator cpfValidator;

        [TestInitialize()]
        public void Initialize()
        {
            cpfValidator = new CPFValidator();
        }

        [TestMethod]
        public void ShouldValidateValidCPF()
        {
            cpfValidator.IsValid("11144477735");
            cpfValidator.IsValid("88641577947");
            cpfValidator.IsValid("34608514300");
            cpfValidator.IsValid("47393545608");
        }

        [TestMethod]
        public void ShouldValidateNullCPF()
        {
            cpfValidator.IsValid(null);
        }

        [TestMethod]
        public void ShouldValidateCPFWithLeadingZeros()
        {
            cpfValidator.IsValid("01169538452");
        }

        [TestMethod]
        [ExpectedException(typeof(CPFInvalidDigits))]
        public void ShouldNotValidateCPFWithLessDigitsThanAllowed()
        {
            cpfValidator.IsValid("1234567890");
        }

        [TestMethod]
        [ExpectedException(typeof(CPFInvalidDigits))]
        public void ShouldNotValidateCPFWithMoreDigitsThanAlowed()
        {
            cpfValidator.IsValid("123456789012");
        }

        [TestMethod]
        [ExpectedException(typeof(CPFInvalidDigits))]
        public void ShouldNotValidateCPFWithInvalidCharacter()
        {
            cpfValidator.IsValid("1111111a111");
        }
    }
}
