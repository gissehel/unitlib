using NUnit.Framework;
using System.Collections.Generic;
using Unit.Lib.Core.DomainModel;
using Unit.Lib.Core.Service;
using Unit.Lib.Service;

namespace Unit.Test.NUnit.All
{
    [TestFixture]
    internal class UnitValueComputeTest
    {
        private IConstantProvider<ScalarFloat, float> ConstantProvider { get; set; }

        private IUnitService<ScalarFloat, float> UnitService { get; set; }

        [SetUp]
        public void SetUp()
        {
            ConstantProvider = new ConstantProviderFloat();
            UnitService = new UnitService<ScalarFloat, float>(ConstantProvider);
        }

        protected UnitName<ScalarFloat, float> GetUnitName_km() => new UnitName<ScalarFloat, float>(ConstantProvider.GetPrefixBySymbol("k"), ConstantProvider.GetUnitBySymbol("m"));

        protected UnitName<ScalarFloat, float> GetUnitName_s() => new UnitName<ScalarFloat, float>(ConstantProvider.GetPrefixBySymbol(""), ConstantProvider.GetUnitBySymbol("s"));

        protected UnitNamePower<ScalarFloat, float> GetUnitNamePower_km() => GetUnitNamePower_km(1);

        protected UnitNamePower<ScalarFloat, float> GetUnitNamePower_s() => GetUnitNamePower_s(1);

        protected UnitNamePower<ScalarFloat, float> GetUnitNamePower_per_km() => GetUnitNamePower_km(-1);

        protected UnitNamePower<ScalarFloat, float> GetUnitNamePower_per_s() => GetUnitNamePower_s(-1);

        protected UnitNamePower<ScalarFloat, float> GetUnitNamePower_km(long power) => new UnitNamePower<ScalarFloat, float>(GetUnitName_km(), power);

        protected UnitNamePower<ScalarFloat, float> GetUnitNamePower_s(long power) => new UnitNamePower<ScalarFloat, float>(GetUnitName_s(), power);

        protected UnitElement<ScalarFloat, float> CreateUnitElement(params UnitNamePower<ScalarFloat, float>[] namePowers) => new UnitElement<ScalarFloat, float>(namePowers);

        protected UnitValue<ScalarFloat, float> CreateUnit(float value, UnitElement<ScalarFloat, float> unitElement) => new UnitValue<ScalarFloat, float>(new ScalarFloat(value), unitElement);

        protected IEnumerable<X> CreateEnumerable<X>(params X[] xs)
        {
            return xs;
        }

        [Test]
        public void UnitValueFormatTest()
        {
            var unitElement = CreateUnitElement(GetUnitNamePower_km(), GetUnitNamePower_s(-1));

            var unitValue = CreateUnit(7.23f, unitElement);

            AssertUnitValueAsStringFormat("7.23 km.s-1", unitValue);

            unitElement = CreateUnitElement(GetUnitNamePower_s(), GetUnitNamePower_km(-1));

            unitValue = CreateUnit(0.86f, unitElement);

            AssertUnitValueAsStringFormat("0.86 s.km-1", unitValue);

            unitElement = CreateUnitElement(GetUnitNamePower_s(2), GetUnitNamePower_km(-1));

            unitValue = CreateUnit(0.86f, unitElement);

            AssertUnitValueAsStringFormat("0.86 s2.km-1", unitValue);

            unitElement = CreateUnitElement(GetUnitNamePower_s(0), GetUnitNamePower_km(-1));

            unitValue = CreateUnit(0.86f, unitElement);

            AssertUnitValueAsStringFormat("0.86 km-1", unitValue);

            unitElement = CreateUnitElement(GetUnitNamePower_s(-1), GetUnitNamePower_km(-1));

            unitValue = CreateUnit(0.86f, unitElement);

            AssertUnitValueAsStringFormat("0.86 km-1.s-1", unitValue);
        }

        public void AssertUnitValueAsStringFormat(string expectedResult, UnitValue<ScalarFloat, float> unitValue)
        {
            Assert.AreEqual(expectedResult, unitValue.AsString);
        }

        [Test]
        public void ConvertTest()
        {
            var unit = UnitService.Parse("3.7 in.min-1");
            Assert.AreEqual("3.7 in.min-1", unit.AsString);
            var result = UnitService.Convert(unit);
            Assert.AreEqual("0.001566333 m.s-1", result.AsString);
        }

        [Test]
        public void ConvertToNoneTest()
        {
            var unit = UnitService.Parse("60 min.h-1");
            Assert.AreEqual("60 min.h-1", unit.AsString);
            Assert.AreEqual("1", UnitService.Convert(unit).AsString);
        }

        // [Test]
        public void ConvertTo_in_to_cm()
        {
            var unit = UnitService.Parse("1 in");
            Assert.AreEqual("1 in", unit.AsString);
            var element = UnitService.ParseUnit("cm");
            var result = UnitService.Convert(unit, element);
            Assert.AreEqual("0.000254 cm", result.AsString);
        }
    }
}