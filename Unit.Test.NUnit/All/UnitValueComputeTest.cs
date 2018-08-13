using NUnit.Framework;
using System.Collections.Generic;
using Unit.Lib.Core.DomainModel;
using Unit.Lib.Core.Service;
using Unit.Lib.Service;

namespace Unit.Test.NUnit.All
{
    internal class UnitValueComputeTest<S, T> where S : class, IScalar<T>, new()
    {
        private IConstantProvider<S, T> ConstantProvider { get; set; }

        private IUnitService<S, T> UnitService { get; set; }

        private S scalarNone = new S();

        protected S GetNewScalar(float value) => scalarNone.GetNewFromFloat(value) as S;

        [SetUp]
        public void SetUp()
        {
            ConstantProvider = new ConstantProvider<S, T>();
            UnitService = new UnitService<S, T>(ConstantProvider);
        }

        protected UnitName<S, T> GetUnitName_km() => new UnitName<S, T>(ConstantProvider.GetPrefixBySymbol("k"), ConstantProvider.GetUnitBySymbol("m"));

        protected UnitName<S, T> GetUnitName_s() => new UnitName<S, T>(ConstantProvider.GetPrefixBySymbol(""), ConstantProvider.GetUnitBySymbol("s"));

        protected UnitNamePower<S, T> GetUnitNamePower_km() => GetUnitNamePower_km(1);

        protected UnitNamePower<S, T> GetUnitNamePower_s() => GetUnitNamePower_s(1);

        protected UnitNamePower<S, T> GetUnitNamePower_per_km() => GetUnitNamePower_km(-1);

        protected UnitNamePower<S, T> GetUnitNamePower_per_s() => GetUnitNamePower_s(-1);

        protected UnitNamePower<S, T> GetUnitNamePower_km(long power) => new UnitNamePower<S, T>(GetUnitName_km(), power);

        protected UnitNamePower<S, T> GetUnitNamePower_s(long power) => new UnitNamePower<S, T>(GetUnitName_s(), power);

        protected UnitElement<S, T> CreateUnitElement(params UnitNamePower<S, T>[] namePowers) => new UnitElement<S, T>(namePowers);

        protected UnitValue<S, T> CreateUnit(float value, UnitElement<S, T> unitElement) => new UnitValue<S, T>(GetNewScalar(value), unitElement);

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

        public void AssertUnitValueAsStringFormat(string expectedResult, UnitValue<S, T> unitValue)
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

        [Test]
        public void ConvertTo_in_to_cm()
        {
            var unit = UnitService.Parse("1 in");
            Assert.AreEqual("1 in", unit.AsString);
            var element = UnitService.ParseUnit("cm");
            var result = UnitService.Convert(unit, element);
            Assert.AreEqual("2.54 cm", result.AsString);
        }

        [Test]
        public void ConvertTo_in_to_cm_bis()
        {
            var unit = UnitService.Parse("1 in");
            Assert.AreEqual("1 in", unit.AsString);
            var unitTarget = UnitService.Parse("cm");
            var result = UnitService.Convert(unit, unitTarget);
            Assert.AreEqual("2.54 cm", result.AsString);
        }

        [Test]
        public void Convert_cm_to_SI()
        {
            var unit = UnitService.Parse("2  cm");
            Assert.AreEqual("2 cm", unit.AsString);
            var result = UnitService.Convert(unit);
            Assert.AreEqual("0.02 m", result.AsString);
        }

        [Test]
        public void Convert_cm_to_h()
        {
            var unit = UnitService.Parse("2  cm");
            Assert.AreEqual("2 cm", unit.AsString);
            var target = UnitService.Parse("h");
            var result = UnitService.Convert(unit, target);
            Assert.AreEqual("5.555556E-06 h.m.s-1", result.AsString);
        }

        [Test]
        public void Convert_empty()
        {
            var unit = UnitService.Parse("");
            Assert.AreEqual("1", unit.AsString);
            var result = UnitService.Convert(unit);
            Assert.AreEqual("1", result.AsString);
        }

        [Test]
        public void Convert_1cm2()
        {
            var unit = UnitService.Parse("1cm2");
            Assert.AreEqual("1 cm2", unit.AsString);
            var result = UnitService.Convert(unit);
            Assert.AreEqual("0.0001 m2", result.AsString);
        }

        [Test]
        public void Convert_1cm_2()
        {
            var unit = UnitService.Parse("1cm-2");
            Assert.AreEqual("1 cm-2", unit.AsString);
            var result = UnitService.Convert(unit);
            Assert.AreEqual("10000 m-2", result.AsString);
        }

        [Test]
        public void Convert_1g2()
        {
            var unit = UnitService.Parse("1g2");
            Assert.AreEqual("1 g2", unit.AsString);
            var result = UnitService.Convert(unit);
            Assert.AreEqual("1E-06 kg2", result.AsString);
        }

        [Test]
        public void Convert_km_per_h_into_in_per_min()
        {
            var unit = UnitService.Parse("7km/h");
            Assert.AreEqual("7 km.h-1", unit.AsString);
            var target = UnitService.Parse("in/min");
            var result = UnitService.Convert(unit, target);
            Assert.AreEqual(target.Value.GetNeutral().Value, target.Value.Value);
            Assert.AreEqual(unit.UnitElement.GetDimension(), target.UnitElement.GetDimension());
            Assert.AreEqual("4593.176 in.min-1", result.AsString);
        }

        [Test]
        public void AssignNewUnit()
        {
            var unit = UnitService.Parse("213 dft2");
            UnitService.AddUnit(unit, "grut", "R", "user");
            unit = UnitService.Parse("R2");
            var result = UnitService.Convert(unit);
            Assert.AreEqual("0.03915787 m4", result.AsString);
        }

        [Test]
        public void AssignNewPrefixNonInverted()
        {
            UnitService.AddPrefix("Dozo", "D", "user", GetNewScalar(12), false);
            UnitService.AddPrefix("dozo", "w", "user", GetNewScalar(12), true);
            var unit = UnitService.Parse("2 Dm");
            var result = UnitService.Convert(unit);
            Assert.AreEqual("24 m", result.AsString);
        }

        [Test]
        public void AssignNewPrefixInverted()
        {
            UnitService.AddPrefix("Dozo", "D", "user", GetNewScalar(12), false);
            UnitService.AddPrefix("dozo", "w", "user", GetNewScalar(12), true);
            var unit = UnitService.Parse("24 wm");
            var result = UnitService.Convert(unit);
            Assert.AreEqual("2 m", result.AsString);
        }
    }

    [TestFixture]
    internal class UnitValueComputeFloatTest : UnitValueComputeTest<ScalarFloat, float>
    {
    }

    //[TestFixture]
    //internal class UnitValueComputeDoubleTest : UnitValueComputeTest<ScalarDouble, double>
    //{
    //}
}