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
            var unitBaseName = new UnitBaseName<ScalarFloat, float> { Name = "metre", AsciiSymbol = "m", Factor = new ScalarFloat(1), Symbol = "m" };
            var unitPrefix = new UnitPrefix<ScalarFloat, float> { Name = "kilo", Symbol = "k", AsciiSymbol = "k", Factor = new ScalarFloat(1000), Invert = false };

            var unitName = new UnitName<ScalarFloat, float>(unitPrefix, unitBaseName);

            Assert.AreEqual("km", unitName.AsString);
        }
    }
}