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
        private Mock<IConstantProvider> ConstantProviderMock { get; set; }
        private IUnitService UnitService { get; set; }

        [SetUp]
        public void SetUp()
        {
            ConstantProviderMock = new Mock<IConstantProvider>();

            UnitPrefix unitPrefixNone = new UnitPrefix { Name = "", Symbol = "", Invert = false, Factor = 1 };
            UnitPrefix unitPrefixKilo = new UnitPrefix { Name = "kilo", Symbol = "k", Invert = false, Factor = 1000 };
            UnitPrefix unitPrefixMilli = new UnitPrefix { Name = "milli", Symbol = "m", Invert = true, Factor = 1000 };
            UnitBaseName unitBaseNameMetre = new UnitBaseName { Name = "metre", Symbol = "m", Factor = 1, Dimension = UnitDimension.Length };
            UnitBaseName unitBaseNameSecond = new UnitBaseName { Name = "second", Symbol = "s", Factor = 1, Dimension = UnitDimension.Time };
            UnitBaseName unitBaseNameKelvin = new UnitBaseName { Name = "kelvin", Symbol = "K", Factor = 1, Dimension = UnitDimension.Temperature };
            UnitBaseName unitBaseNameHour = new UnitBaseName { Name = "hour", Symbol = "h", Factor = 60 * 60, Dimension = UnitDimension.Time };
            ConstantProviderMock.Setup(constantProvider => constantProvider.GetPrefixBySymbol("")).Returns(unitPrefixNone);
            ConstantProviderMock.Setup(constantProvider => constantProvider.GetPrefixBySymbol("k")).Returns(unitPrefixKilo);
            ConstantProviderMock.Setup(constantProvider => constantProvider.GetPrefixBySymbol("m")).Returns(unitPrefixMilli);
            ConstantProviderMock.Setup(constantProvider => constantProvider.GetUnitBySymbol("m")).Returns(unitBaseNameMetre);
            ConstantProviderMock.Setup(constantProvider => constantProvider.GetUnitBySymbol("s")).Returns(unitBaseNameSecond);
            ConstantProviderMock.Setup(constantProvider => constantProvider.GetUnitBySymbol("K")).Returns(unitBaseNameKelvin);
            ConstantProviderMock.Setup(constantProvider => constantProvider.GetUnitBySymbol("h")).Returns(unitBaseNameHour);
            ConstantProviderMock.Setup(constantProvider => constantProvider.GetUnitBySymbol("km")).Throws<UnitNotFoundException>();

            UnitService = new UnitService(ConstantProviderMock.Object);
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