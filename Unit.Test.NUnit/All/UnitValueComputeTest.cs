using NUnit.Framework;
using Unit.Lib.Core.DomainModel;
using Unit.Lib.Core.Service;
using Unit.Lib.Service;

namespace Unit.Test.NUnit.All
{
    [TestFixture]
    internal class UnitValueComputeTest
    {
        private IConstantProvider ConstantProvider { get; set; }

        [SetUp]
        public void SetUp()
        {
            ConstantProvider = new ConstantProvider();
        }

        private UnitName GetUnitName_km() => new UnitName
        {
            Prefix = ConstantProvider.GetPrefixBySymbol("k"),
            BaseName = ConstantProvider.GetUnitBySymbol("m"),
        };

        private UnitName GetUnitName_s() => new UnitName
        {
            Prefix = ConstantProvider.GetPrefixBySymbol(""),
            BaseName = ConstantProvider.GetUnitBySymbol("s"),
        };

        [Test]
        public void UnitValueFormatTest()
        {
            var unitElement = new UnitElement
            (
                new UnitNamePower[] { new UnitNamePower(GetUnitName_km()), new UnitNamePower(GetUnitName_s(), -1) }
            );

            var unitValue = new UnitValue(7.23f, unitElement);

            AssertUnitValueAsStringFormat("7.23 km.s-1", unitValue);

            unitElement = new UnitElement
            (
                new UnitNamePower[] { new UnitNamePower(GetUnitName_s()), new UnitNamePower(GetUnitName_km(), -1) }
            );

            unitValue = new UnitValue(0.86f, unitElement);

            AssertUnitValueAsStringFormat("0.86 s.km-1", unitValue);

            unitElement = new UnitElement
            (
                new UnitNamePower[] { new UnitNamePower(GetUnitName_s(), 2), new UnitNamePower(GetUnitName_km(), -1) }
            );

            unitValue = new UnitValue(0.86f, unitElement);

            AssertUnitValueAsStringFormat("0.86 s2.km-1", unitValue);

            unitElement = new UnitElement
            (
                new UnitNamePower[] { new UnitNamePower(GetUnitName_s(), 0), new UnitNamePower(GetUnitName_km(), -1) }
            );

            unitValue = new UnitValue(0.86f, unitElement);

            AssertUnitValueAsStringFormat("0.86 km-1", unitValue);

            unitElement = new UnitElement
            (
                new UnitNamePower[] { new UnitNamePower(GetUnitName_s(), -1), new UnitNamePower(GetUnitName_km(), -1) }
            );

            unitValue = new UnitValue(0.86f, unitElement);

            AssertUnitValueAsStringFormat("0.86 km-1.s-1", unitValue);
        }

        public void AssertUnitValueAsStringFormat(string expectedResult, UnitValue unitValue)
        {
            Assert.AreEqual(expectedResult, unitValue.AsString);
        }
    }
}