using Moq;
using NUnit.Framework;
using Unit.Lib.Core.DomainModel;
using Unit.Lib.Core.Exceptions;
using Unit.Lib.Core.Service;
using Unit.Lib.Service;

namespace Unit.Test.NUnit.Service
{
    [TestFixture]
    internal class UnitServiceTest
    {
        private Mock<IConstantProvider<ScalarFloat, float>> ConstantProviderMock { get; set; }
        private IUnitService<ScalarFloat, float> UnitService { get; set; }

        [SetUp]
        public void SetUp()
        {
            ConstantProviderMock = new Mock<IConstantProvider<ScalarFloat, float>>();

            var unitPrefixNone = new UnitPrefix<ScalarFloat, float> { Name = "", Symbol = "", Invert = false, Factor = new ScalarFloat(1) };
            var unitPrefixKilo = new UnitPrefix<ScalarFloat, float> { Name = "kilo", Symbol = "k", Invert = false, Factor = new ScalarFloat(1000) };
            var unitPrefixMilli = new UnitPrefix<ScalarFloat, float> { Name = "milli", Symbol = "m", Invert = true, Factor = new ScalarFloat(1000) };
            var unitBaseNameMetre = new UnitBaseName<ScalarFloat, float> { Name = "metre", Symbol = "m", Factor = new ScalarFloat(1), Dimension = UnitDimension.Length };
            var unitBaseNameSecond = new UnitBaseName<ScalarFloat, float> { Name = "second", Symbol = "s", Factor = new ScalarFloat(1), Dimension = UnitDimension.Time };
            var unitBaseNameKelvin = new UnitBaseName<ScalarFloat, float> { Name = "kelvin", Symbol = "K", Factor = new ScalarFloat(1), Dimension = UnitDimension.Temperature };
            var unitBaseNameHour = new UnitBaseName<ScalarFloat, float> { Name = "hour", Symbol = "h", Factor = new ScalarFloat(60 * 60), Dimension = UnitDimension.Time };
            ConstantProviderMock.Setup(constantProvider => constantProvider.GetPrefixBySymbol("")).Returns(unitPrefixNone);
            ConstantProviderMock.Setup(constantProvider => constantProvider.GetPrefixBySymbol("k")).Returns(unitPrefixKilo);
            ConstantProviderMock.Setup(constantProvider => constantProvider.GetPrefixBySymbol("m")).Returns(unitPrefixMilli);
            ConstantProviderMock.Setup(constantProvider => constantProvider.GetUnitBySymbol("m")).Returns(unitBaseNameMetre);
            ConstantProviderMock.Setup(constantProvider => constantProvider.GetUnitBySymbol("s")).Returns(unitBaseNameSecond);
            ConstantProviderMock.Setup(constantProvider => constantProvider.GetUnitBySymbol("K")).Returns(unitBaseNameKelvin);
            ConstantProviderMock.Setup(constantProvider => constantProvider.GetUnitBySymbol("h")).Returns(unitBaseNameHour);
            ConstantProviderMock.Setup(constantProvider => constantProvider.GetUnitBySymbol("km")).Throws<UnitNotFoundException>();

            UnitService = new UnitService<ScalarFloat, float>(ConstantProviderMock.Object);
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
}