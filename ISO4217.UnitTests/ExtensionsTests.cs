using System;
using Dodo.ISO4217;
using NUnit.Framework;

namespace ISO4217.UnitTests
{
    public class ExtensionsTests
    {
        [TestCase("643", Currency.Code.RUB)]
        [TestCase("RUB", Currency.Code.RUB)]
        [TestCase("978", Currency.Code.EUR)]
        [TestCase("EUR", Currency.Code.EUR)]
        public void GivenValidStringCurrency_WhenCalledToCurrencyCode_ThenCorrectCurrencyCodeEnumReturned(
            string @string,
            Currency.Code @enum)
        {
            Assert.AreEqual(@enum, @string.ToCurrencyCode());
        }

        [TestCase(643, Currency.Code.RUB)]
        [TestCase(978, Currency.Code.EUR)]
        public void GivenValidIntCurrency_WhenCalledToCurrencyCode_ThenCorrectCurrencyCodeEnumReturned(
            int @int,
            Currency.Code @enum)
        {
            Assert.AreEqual(@enum, @int.ToCurrencyCode());
        }

        [TestCase("-12315326")]
        [TestCase("-1")]
        [TestCase("0")]
        [TestCase("6666")]
        [TestCase("Invalid currency!")]
        [TestCase(" ")]
        public void GivenInvalidStringCurrency_WhenCalledToCurrencyCode_ThenInvalidOperationExceptionThrown(
            string @string)
        {
            Assert.Throws<InvalidOperationException>(() => @string.ToCurrencyCode());
        }

        [TestCase(int.MinValue)]
        [TestCase(0)]
        [TestCase(6666)]
        [TestCase(int.MaxValue)]
        public void GivenInvalidIntCurrency_WhenCalledToCurrencyCode_ThenInvalidOperationExceptionThrown(int @int)
        {
            Assert.Throws<InvalidOperationException>(() => @int.ToCurrencyCode());
        }

        [TestCase(Currency.Code.RUB, "Russian Ruble")]
        [TestCase(Currency.Code.EUR, "Euro")]
        public void GivenCurrencyCode_WhenCalledGetName_ThenCorrectNameReturned(
            Currency.Code @enum,
            string name)
        {
            Assert.AreEqual(name, @enum.GetName());
        }

        [TestCase(Currency.Code.RUB, 2)]
        [TestCase(Currency.Code.EUR, 2)]
        public void GivenCurrencyCode_WhenCalledGetMinorUnits_ThenCorrectMinorUnitsReturned(
            Currency.Code @enum,
            int minorUnits)
        {
            Assert.AreEqual(minorUnits, @enum.GetMinorUnits());
        }
    }
}