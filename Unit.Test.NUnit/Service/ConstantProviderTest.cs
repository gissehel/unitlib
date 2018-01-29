using NUnit.Framework;
using System;
using Unit.Lib.Core.DomainModel.Enumeration;
using Unit.Lib.Core.Exceptions;
using Unit.Lib.Core.Service;
using Unit.Lib.Service;

namespace Unit.Test.NUnit.Service
{
    [TestFixture]
    public class ConstantProviderTest
    {
        private IConstantProvider ConstantProvider { get; set; }

        [SetUp]
        public void SetUp()
        {
            ConstantProvider = new ConstantProvider();
        }

        [TearDown]
        public void TearDown()
        {
        }

        [Test]
        public void GetUnitByNameTest_metre()
        {
            var unit = ConstantProvider.GetUnitByName("metre");
            Assert.AreEqual("metre", unit.Name);
            Assert.AreEqual("m", unit.Symbol);
            Assert.AreEqual("m", unit.AsciiSymbol);
            Assert.AreEqual("m", unit.AsString);
            Assert.AreEqual("m", unit.AsAsciiString);
            Assert.AreEqual("SI/metre", unit.FqName);
            Assert.AreEqual("L", unit.Dimension.AsString);
            Assert.AreEqual(1, unit.Factor);
            Assert.AreEqual(1, unit.Dimension.QuantityCount);
            Assert.True(unit.Dimension.HasQuantity(UnitBaseQuantity.Length));
            Assert.AreEqual(1, unit.Dimension.GetPower(UnitBaseQuantity.Length));
        }

        [Test]
        public void GetUnitByNameTest_gram()
        {
            var unit = ConstantProvider.GetUnitByName("gram");
            Assert.AreEqual("gram", unit.Name);
            Assert.AreEqual("g", unit.Symbol);
            Assert.AreEqual("g", unit.AsciiSymbol);
            Assert.AreEqual("g", unit.AsString);
            Assert.AreEqual("g", unit.AsAsciiString);
            Assert.AreEqual("SI/gram", unit.FqName);
            Assert.AreEqual("M", unit.Dimension.AsString);
            Assert.AreEqual(1, unit.Factor);
            Assert.AreEqual(1, unit.Dimension.QuantityCount);
            Assert.True(unit.Dimension.HasQuantity(UnitBaseQuantity.Mass));
            Assert.AreEqual(1, unit.Dimension.GetPower(UnitBaseQuantity.Mass));
        }

        [Test]
        public void GetUnitByNameTest_hertz()
        {
            var unit = ConstantProvider.GetUnitByName("hertz");
            Assert.AreEqual("hertz", unit.Name);
            Assert.AreEqual("Hz", unit.Symbol);
            Assert.AreEqual("Hz", unit.AsciiSymbol);
            Assert.AreEqual("Hz", unit.AsString);
            Assert.AreEqual("Hz", unit.AsAsciiString);
            Assert.AreEqual("SI_derivative/hertz", unit.FqName);
            Assert.AreEqual("T-1", unit.Dimension.AsString);
            Assert.AreEqual(1, unit.Factor);
            Assert.AreEqual(1, unit.Dimension.QuantityCount);
            Assert.True(unit.Dimension.HasQuantity(UnitBaseQuantity.Time));
            Assert.AreEqual(-1, unit.Dimension.GetPower(UnitBaseQuantity.Time));
        }

        [Test]
        public void GetUnitByNameTest_newton()
        {
            var unit = ConstantProvider.GetUnitByName("newton");
            Assert.AreEqual("newton", unit.Name);
            Assert.AreEqual("N", unit.Symbol);
            Assert.AreEqual("N", unit.AsciiSymbol);
            Assert.AreEqual("N", unit.AsString);
            Assert.AreEqual("N", unit.AsAsciiString);
            Assert.AreEqual("SI_derivative/newton", unit.FqName);
            Assert.AreEqual("L.M.T-2", unit.Dimension.AsString);
            Assert.AreEqual(1000f, unit.Factor);
            Assert.AreEqual(3, unit.Dimension.QuantityCount);
            Assert.True(unit.Dimension.HasQuantity(UnitBaseQuantity.Length));
            Assert.True(unit.Dimension.HasQuantity(UnitBaseQuantity.Time));
            Assert.True(unit.Dimension.HasQuantity(UnitBaseQuantity.Mass));
            Assert.AreEqual(1, unit.Dimension.GetPower(UnitBaseQuantity.Length));
            Assert.AreEqual(1, unit.Dimension.GetPower(UnitBaseQuantity.Mass));
            Assert.AreEqual(-2, unit.Dimension.GetPower(UnitBaseQuantity.Time));
        }

        [Test]
        public void GetUnitByNameTest_none()
        {
            try
            {
                var unit = ConstantProvider.GetUnitByName("none");
                Assert.Fail("GetUnitByName(\"none\") should throw an exception");
            }
            catch (UnitNotFoundException)
            {
                Assert.Pass();
            }
            catch (Exception ex)
            {
                Assert.Fail("GetUnitByName(\"none\") should thrown UnitNotFoundException, not {0}", ex);
            }
        }

        [Test]
        public void GetUnitBySymbolTest_none()
        {
            try
            {
                var unit = ConstantProvider.GetUnitBySymbol("none");
                Assert.Fail("GetUnitBySymbol(\"none\") should throw an exception");
            }
            catch (UnitNotFoundException)
            {
                Assert.Pass();
            }
            catch (Exception ex)
            {
                Assert.Fail("GetUnitBySymbol(\"none\") should thrown UnitNotFoundException, not {0}", ex);
            }
        }

        [Test]
        public void GetUnitBySymbolTest_metre()
        {
            var unit = ConstantProvider.GetUnitBySymbol("m");
            Assert.AreEqual("metre", unit.Name);
            Assert.AreEqual("m", unit.Symbol);
            Assert.AreEqual("m", unit.AsciiSymbol);
            Assert.AreEqual("m", unit.AsString);
            Assert.AreEqual("m", unit.AsAsciiString);
            Assert.AreEqual("SI/metre", unit.FqName);
            Assert.AreEqual(1, unit.Factor);
            Assert.AreEqual(1, unit.Dimension.QuantityCount);
            Assert.True(unit.Dimension.HasQuantity(UnitBaseQuantity.Length));
            Assert.AreEqual(1, unit.Dimension.GetPower(UnitBaseQuantity.Length));
        }

        [Test]
        public void GetUnitBySymbolTest_gram()
        {
            var unit = ConstantProvider.GetUnitBySymbol("g");
            Assert.AreEqual("gram", unit.Name);
            Assert.AreEqual("g", unit.Symbol);
            Assert.AreEqual("g", unit.AsciiSymbol);
            Assert.AreEqual("g", unit.AsString);
            Assert.AreEqual("g", unit.AsAsciiString);
            Assert.AreEqual("SI/gram", unit.FqName);
            Assert.AreEqual(1, unit.Factor);
            Assert.AreEqual(1, unit.Dimension.QuantityCount);
            Assert.True(unit.Dimension.HasQuantity(UnitBaseQuantity.Mass));
            Assert.AreEqual(1, unit.Dimension.GetPower(UnitBaseQuantity.Mass));
        }

        [Test]
        public void GetPrefixByNameTest_none()
        {
            var prefix = ConstantProvider.GetPrefixByName("");
            Assert.AreEqual("", prefix.Name);
            Assert.AreEqual("", prefix.Symbol);
            Assert.AreEqual("", prefix.AsciiSymbol);
            Assert.AreEqual("", prefix.AsString);
            Assert.AreEqual("", prefix.AsAsciiString);
            Assert.AreEqual("/", prefix.FqName);
            Assert.AreEqual(1, prefix.Factor);
            Assert.AreEqual(false, prefix.Invert);
        }

        [Test]
        public void GetPrefixByNameTest_kilo()
        {
            var prefix = ConstantProvider.GetPrefixByName("kilo");
            Assert.AreEqual("kilo", prefix.Name);
            Assert.AreEqual("k", prefix.Symbol);
            Assert.AreEqual("k", prefix.AsciiSymbol);
            Assert.AreEqual("k", prefix.AsString);
            Assert.AreEqual("k", prefix.AsAsciiString);
            Assert.AreEqual("SI/kilo", prefix.FqName);
            Assert.AreEqual(1000, prefix.Factor);
            Assert.AreEqual(false, prefix.Invert);
        }

        [Test]
        public void GetPrefixByNameTest_milli()
        {
            var prefix = ConstantProvider.GetPrefixByName("milli");
            Assert.AreEqual("milli", prefix.Name);
            Assert.AreEqual("m", prefix.Symbol);
            Assert.AreEqual("m", prefix.AsciiSymbol);
            Assert.AreEqual("m", prefix.AsString);
            Assert.AreEqual("m", prefix.AsAsciiString);
            Assert.AreEqual("SI/milli", prefix.FqName);
            Assert.AreEqual(1000, prefix.Factor);
            Assert.AreEqual(true, prefix.Invert);
        }

        [Test]
        public void GetPrefixBySymbolTest_none()
        {
            var prefix = ConstantProvider.GetPrefixBySymbol("");
            Assert.AreEqual("", prefix.Name);
            Assert.AreEqual("", prefix.Symbol);
            Assert.AreEqual("", prefix.AsciiSymbol);
            Assert.AreEqual("", prefix.AsString);
            Assert.AreEqual("", prefix.AsAsciiString);
            Assert.AreEqual("/", prefix.FqName);
            Assert.AreEqual(1, prefix.Factor);
            Assert.AreEqual(false, prefix.Invert);
        }

        [Test]
        public void GetPrefixBySymbolTest_k()
        {
            var prefix = ConstantProvider.GetPrefixBySymbol("k");
            Assert.AreEqual("kilo", prefix.Name);
            Assert.AreEqual("k", prefix.Symbol);
            Assert.AreEqual("k", prefix.AsciiSymbol);
            Assert.AreEqual("k", prefix.AsString);
            Assert.AreEqual("k", prefix.AsAsciiString);
            Assert.AreEqual("SI/kilo", prefix.FqName);
            Assert.AreEqual(1000, prefix.Factor);
            Assert.AreEqual(false, prefix.Invert);
        }

        [Test]
        public void GetPrefixBySymbolTest_m()
        {
            var prefix = ConstantProvider.GetPrefixBySymbol("m");
            Assert.AreEqual("milli", prefix.Name);
            Assert.AreEqual("m", prefix.Symbol);
            Assert.AreEqual("m", prefix.AsciiSymbol);
            Assert.AreEqual("m", prefix.AsString);
            Assert.AreEqual("m", prefix.AsAsciiString);
            Assert.AreEqual("SI/milli", prefix.FqName);
            Assert.AreEqual(1000, prefix.Factor);
            Assert.AreEqual(true, prefix.Invert);
        }

        [Test]
        public void GetPrefixBySymbolTest_M()
        {
            var prefix = ConstantProvider.GetPrefixBySymbol("M");
            Assert.AreEqual("mega", prefix.Name);
            Assert.AreEqual("M", prefix.Symbol);
            Assert.AreEqual("M", prefix.AsciiSymbol);
            Assert.AreEqual("M", prefix.AsString);
            Assert.AreEqual("M", prefix.AsAsciiString);
            Assert.AreEqual("SI/mega", prefix.FqName);
            Assert.AreEqual(1000000, prefix.Factor);
            Assert.AreEqual(false, prefix.Invert);
        }
    }
}