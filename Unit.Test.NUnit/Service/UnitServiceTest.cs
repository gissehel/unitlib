using Moq;
using NUnit.Framework;
using Unit.Lib.Core.DomainModel;
using Unit.Lib.Core.Exceptions;
using Unit.Lib.Core.Service;
using Unit.Lib.Service;

namespace Unit.Test.NUnit.Service
{
    public class UnitServiceTest<S, T> where S : class, IScalar<T>, new()
    {
        private Mock<IConstantProvider<S, T>> ConstantProviderMock { get; set; }
        private IUnitService<S, T> UnitService { get; set; }

        private S scalarNone = new S();

        protected UnitPrefix<S, T> GetNewUnitPrefix(string name, string symbol, bool invert, float factorValue) => new UnitPrefix<S, T> { Name = name, Symbol = symbol, Invert = invert, Factor = scalarNone.GetNewFromFloat(factorValue) as S };

        protected UnitBaseName<S, T> GetNewUnitBaseName(string name, string symbol, float factorValue, UnitDimension dimension) => new UnitBaseName<S, T> { Name = name, Symbol = symbol, Factor = scalarNone.GetNewFromFloat(factorValue) as S, Dimension = dimension };

        [SetUp]
        public void SetUp()
        {
            ConstantProviderMock = new Mock<IConstantProvider<S, T>>();

            var unitPrefixNone = GetNewUnitPrefix("", "", false, 1f);
            var unitPrefixKilo = GetNewUnitPrefix("kilo", "k", false, 1000);
            var unitPrefixMilli = GetNewUnitPrefix("milli", "m", true, 1000);

            var unitBaseNameMetre = GetNewUnitBaseName("metre", "m", 1, UnitDimension.Length);
            var unitBaseNameSecond = GetNewUnitBaseName("second", "s", 1, UnitDimension.Time);
            var unitBaseNameKelvin = GetNewUnitBaseName("kelvin", "K", 1, UnitDimension.Temperature);
            var unitBaseNameHour = GetNewUnitBaseName("hour", "h", 60 * 60, UnitDimension.Time);

            ConstantProviderMock.Setup(constantProvider => constantProvider.GetPrefixBySymbol("")).Returns(unitPrefixNone);
            ConstantProviderMock.Setup(constantProvider => constantProvider.GetPrefixBySymbol("k")).Returns(unitPrefixKilo);
            ConstantProviderMock.Setup(constantProvider => constantProvider.GetPrefixBySymbol("m")).Returns(unitPrefixMilli);

            ConstantProviderMock.Setup(constantProvider => constantProvider.GetUnitBySymbol("m")).Returns(unitBaseNameMetre);
            ConstantProviderMock.Setup(constantProvider => constantProvider.GetUnitBySymbol("s")).Returns(unitBaseNameSecond);
            ConstantProviderMock.Setup(constantProvider => constantProvider.GetUnitBySymbol("K")).Returns(unitBaseNameKelvin);
            ConstantProviderMock.Setup(constantProvider => constantProvider.GetUnitBySymbol("h")).Returns(unitBaseNameHour);
            ConstantProviderMock.Setup(constantProvider => constantProvider.GetUnitBySymbol("km")).Throws<UnitNotFoundException>();

            UnitService = new UnitService<S, T>(ConstantProviderMock.Object);
        }

        [TestCase("3.1 km.s-1", "L.T-1", "3.1 km/s")]
        [TestCase("3.1 km.s-1", "L.T-1", "3.1 km.s-1")]
        [TestCase("3.1 km.s-1", "L.T-1", "3.1 s-1.km")]
        [TestCase("3.1 km.s-1", "L.T-1", "3.1 s-1.K0.km")]
        [TestCase("3.1 km.s-1", "L.T-1", "3.1 s-1.m.km.m-1")]
        [TestCase("3.1 km.m.s-1", "L2.T-1", "3.1 s-1.m.m.km.m-1")]
        [TestCase("3.1 km.m.mm.s-1", "L3.T-1", "3.1 s-1.m.mm.m.km.m-1")]
        [TestCase("3.1 h.km.m.mm.s-1", "L3", "3.1 s-1.m.mm.h.m.km.m-1")]
        public void ParseAndFormatTest(string expectedResult, string expectedDimension, string inputString)
        {
            var unit = UnitService.Parse(inputString);
            Assert.AreEqual(expectedResult, unit.AsString);
            Assert.AreEqual(expectedDimension, unit.GetDimension().AsString);
        }
    }

    [TestFixture]
    public class UnitServiceFloatTest : UnitServiceTest<ScalarFloat, float> { }

    [TestFixture]
    public class UnitServiceDoubleTest : UnitServiceTest<ScalarDouble, double> { }
}