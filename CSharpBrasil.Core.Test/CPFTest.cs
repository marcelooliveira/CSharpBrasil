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
        public void ShouldNotValidateCPFWithLessDigitsThanAllowed()
        {
            try
            {
                cpfValidator.IsValid("1234567890");
                Assert.Fail();
            }
            catch (InvalidStateException e)
            {
                AssertMessage(e, CPFError.InvalidDigits);
            }
        }

        [TestMethod]
        public void ShouldNotValidateCPFWithMoreDigitsThanAlowed()
        {
            try
            {
                cpfValidator.IsValid("123456789012");
                Assert.Fail();
            }
            catch (InvalidStateException e)
            {
                AssertMessage(e, CPFError.InvalidDigits);
            }
        }

        [TestMethod]
        public void ShouldNotValidateCPFWithInvalidCharacter()
        {
            try
            {
                cpfValidator.IsValid("1111111a111");
                Assert.Fail();
            }
            catch (InvalidStateException e)
            {
                Assert.IsTrue(e.GetErrors().Count == 1);
                AssertMessage(e, CPFError.InvalidDigits);
            }
        }

        private void AssertMessage(InvalidStateException invalidStateException
            , String expected)
        {
            Assert.IsTrue(invalidStateException
                .GetErrors().Contains(expected));
        }
    }
}
