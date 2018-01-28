using NUnit.Framework;
using Unit.Lib.Core.DomainModel;
using Unit.Lib.Core.DomainModel.Enumeration;

namespace Unit.Test.NUnit.Core.DomainModel
{
    [TestFixture]
    public class UnitDimensionTest
    {
        private UnitDimension UnitDimension { get; set; }

        [SetUp]
        public void SetUp()
        {
            UnitDimension = new UnitDimension();
        }

        [Test]
        public void LengthTest()
        {
            var length = UnitDimension.Length;
            Assert.AreEqual(1, length.QuantityCount);
            Assert.True(length.HasQuantity(UnitBaseQuantity.Length));
            Assert.AreEqual(1, length.GetPower(UnitBaseQuantity.Length));
        }

        public void BaseDimensionTest(UnitDimension unitDimension, UnitBaseQuantity unitBaseQuantity)
        {
            Assert.AreEqual(1, unitDimension.QuantityCount);
            Assert.True(unitDimension.HasQuantity(unitBaseQuantity));
            Assert.AreEqual(1, unitDimension.GetPower(unitBaseQuantity));
        }

        [Test]
        public void NoDimensionTest()
        {
            Assert.AreEqual(0, UnitDimension.None.QuantityCount);
        }

        [Test]
        public void OperatorMultiplyTest()
        {
            var dimension = UnitDimension.Length * UnitDimension.Time;
            Assert.AreEqual(2, dimension.QuantityCount);
            Assert.True(dimension.HasQuantity(UnitBaseQuantity.Length));
            Assert.True(dimension.HasQuantity(UnitBaseQuantity.Time));
            Assert.AreEqual(1, dimension.GetPower(UnitBaseQuantity.Length));
            Assert.AreEqual(1, dimension.GetPower(UnitBaseQuantity.Time));
        }

        [Test]
        public void OperatorMultiplyTest_Complex()
        {
            var dimension = UnitDimension.Length * UnitDimension.Time;
            dimension = dimension * UnitDimension.Time;
            Assert.AreEqual(2, dimension.QuantityCount);
            Assert.True(dimension.HasQuantity(UnitBaseQuantity.Length));
            Assert.True(dimension.HasQuantity(UnitBaseQuantity.Time));
            Assert.AreEqual(1, dimension.GetPower(UnitBaseQuantity.Length));
            Assert.AreEqual(2, dimension.GetPower(UnitBaseQuantity.Time));
        }

        [Test]
        public void OperatorDivideTest()
        {
            var dimension = UnitDimension.Length / UnitDimension.Time;
            Assert.AreEqual(2, dimension.QuantityCount);
            Assert.True(dimension.HasQuantity(UnitBaseQuantity.Length));
            Assert.True(dimension.HasQuantity(UnitBaseQuantity.Time));
            Assert.AreEqual(1, dimension.GetPower(UnitBaseQuantity.Length));
            Assert.AreEqual(-1, dimension.GetPower(UnitBaseQuantity.Time));
        }

        [Test]
        public void OperatorDivideTest_Complex1()
        {
            var dimension = UnitDimension.Length / UnitDimension.Time;
            dimension = dimension / UnitDimension.Time;
            Assert.AreEqual(2, dimension.QuantityCount);
            Assert.True(dimension.HasQuantity(UnitBaseQuantity.Length));
            Assert.True(dimension.HasQuantity(UnitBaseQuantity.Time));
            Assert.AreEqual(1, dimension.GetPower(UnitBaseQuantity.Length));
            Assert.AreEqual(-2, dimension.GetPower(UnitBaseQuantity.Time));
        }

        [Test]
        public void OperatorDivideTest_Complex2()
        {
            var dimension = UnitDimension.Length / UnitDimension.Time;
            dimension = dimension * UnitDimension.Length;
            Assert.AreEqual(2, dimension.QuantityCount);
            Assert.True(dimension.HasQuantity(UnitBaseQuantity.Length));
            Assert.True(dimension.HasQuantity(UnitBaseQuantity.Time));
            Assert.AreEqual(2, dimension.GetPower(UnitBaseQuantity.Length));
            Assert.AreEqual(-1, dimension.GetPower(UnitBaseQuantity.Time));
        }

        [Test]
        public void OperatorMultiplyAndDivideTest()
        {
            var dimension = UnitDimension.Length * UnitDimension.Time;
            dimension = dimension / UnitDimension.Time;
            Assert.AreEqual(1, dimension.QuantityCount);
            Assert.True(dimension.HasQuantity(UnitBaseQuantity.Length));
            Assert.False(dimension.HasQuantity(UnitBaseQuantity.Time));
            Assert.AreEqual(1, dimension.GetPower(UnitBaseQuantity.Length));
            Assert.AreEqual(0, dimension.GetPower(UnitBaseQuantity.Time));
        }

        [Test]
        public void AllBaseDimensionTest()
        {
            BaseDimensionTest(UnitDimension.Length, UnitBaseQuantity.Length);
            BaseDimensionTest(UnitDimension.Mass, UnitBaseQuantity.Mass);
            BaseDimensionTest(UnitDimension.Time, UnitBaseQuantity.Time);
            BaseDimensionTest(UnitDimension.ElectricCurrent, UnitBaseQuantity.ElectricCurrent);
            BaseDimensionTest(UnitDimension.Temperature, UnitBaseQuantity.Temperature);
            BaseDimensionTest(UnitDimension.AmountOfSubstance, UnitBaseQuantity.AmountOfSubstance);
            BaseDimensionTest(UnitDimension.LuminousIntensity, UnitBaseQuantity.LuminousIntensity);
        }
    }
}