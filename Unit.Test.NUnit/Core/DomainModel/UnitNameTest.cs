using NUnit.Framework;
using Unit.Lib.Core.DomainModel;

namespace Unit.Test.NUnit.Core.DomainModel
{
    [TestFixture]
    public class UnitNameTest
    {
        [SetUp]
        public void SetUp()
        {
        }

        [TearDown]
        public void TearDown()
        {
        }

        [Test]
        public void AsStringTest()
        {
            UnitBaseName<float> unitBaseName = new UnitBaseName<float> { Name = "metre", AsciiSymbol = "m", Factor = 1, Symbol = "m" };
            UnitPrefix unitPrefix = new UnitPrefix { Name = "kilo", Symbol = "k", AsciiSymbol = "k", Factor = 1000, Invert = false };

            UnitName<float> unitName = new UnitName<float> { BaseName = unitBaseName, Prefix = unitPrefix };

            Assert.AreEqual("km", unitName.AsString);
        }
    }
}