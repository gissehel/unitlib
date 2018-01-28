using System;
using System.Globalization;
using System.Linq;
using Unit.Lib.Core.DomainModel;
using Unit.Lib.Core.Exceptions;
using Unit.Lib.Core.Service;

namespace Unit.Lib.Service
{
    public abstract class UnitService<T> : IUnitService<T> where T : struct
    {
        private bool IsSpaceALike(char c) => c == ' ' || c == '\t' || c == '\r' || c == '\n';

        private bool IsNotValueAnymore(char c) => IsSpaceALike(c) || !((c >= '0' && c <= '9') || (c == ',' || c == '_' || c == '.' || c == '+' || c == '-'));

        private bool IsPower(char c) => (c >= '0' && c <= '9') || (c == '-');

        private bool IsElementSepator(char c) => (c == '*' || c == '.' || c == '/');

        public abstract T ParseValue(string data);

        private long parsePower(string data)
        {
            try
            {
                return long.Parse(data);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Parsing error : Impossible to parse [{0}] as a power", data), ex);
            }
        }

        public UnitValue<T> Parse(string data)
        {
            const int noPosition = -1;
            int valueStartPos = noPosition;
            int valueStopPos = noPosition;
            int unitStartPos = noPosition;
            int unitStopPos = noPosition;
            int powerStartPos = noPosition;
            int powerStopPos = noPosition;
            bool divideNextElement = false;
            T? value = null;
            UnitElement<T> unitElement = null;

            for (int index = 0; index < data.Length; index++)
            {
                char item = data[index];
                if (valueStartPos == noPosition)
                {
                    if (!IsSpaceALike(item))
                    {
                        valueStartPos = index;
                    }
                }
                else if (valueStopPos == noPosition)
                {
                    if (IsNotValueAnymore(item))
                    {
                        valueStopPos = index;
                        if (valueStartPos == valueStopPos)
                        {
                            throw new Exception(string.Format("Parsing error : no valid value found (Start value pos ({0}) == Stop value pos ({1}))", valueStartPos, valueStopPos));
                        }
                        value = new T?(ParseValue(data.Substring(valueStartPos, valueStopPos - valueStartPos)));
                    }
                }
                else if (value.HasValue && unitStartPos == noPosition)
                {
                    if (!IsSpaceALike(item))
                    {
                        unitStartPos = index;
                    }
                }
                else if (value.HasValue && unitStartPos != noPosition && unitStopPos == noPosition)
                {
                    if (IsSpaceALike(item))
                    {
                        unitStopPos = index;
                    }
                    else if (IsPower(item))
                    {
                        unitStopPos = index;
                        powerStartPos = index;
                    }
                }
                else if (value.HasValue && unitStopPos == noPosition && powerStartPos != noPosition)
                {
                    if (!IsPower(item))
                    {
                        powerStopPos = index;
                    }
                }
                else if (value.HasValue && unitStopPos != noPosition && powerStartPos == noPosition)
                {
                    if (IsPower(item))
                    {
                        powerStartPos = index;
                    }
                }
                else if (value.HasValue && unitStopPos != noPosition && powerStartPos != noPosition && powerStopPos == noPosition)
                {
                    if (!IsPower(item))
                    {
                        powerStopPos = index;
                    }
                }

                if (value.HasValue && unitStartPos != noPosition)
                {
                    if (IsElementSepator(item))
                    {
                        if (unitStopPos == noPosition)
                        {
                            unitStopPos = index;
                        }
                        unitElement = ExtractUnitElement(unitElement, data, noPosition, unitStartPos, unitStopPos, powerStartPos, powerStopPos, divideNextElement);

                        unitStartPos = noPosition;
                        unitStopPos = noPosition;
                        powerStartPos = noPosition;
                        powerStopPos = noPosition;

                        divideNextElement = (item == '/');
                    }
                }
            }
            if (value.HasValue && unitStartPos != noPosition)
            {
                if (powerStartPos != noPosition && powerStopPos == noPosition)
                {
                    powerStopPos = data.Length;
                }
                if (unitStartPos != noPosition && unitStopPos == noPosition)
                {
                    unitStopPos = data.Length;
                }
                unitElement = ExtractUnitElement(unitElement, data, noPosition, unitStartPos, unitStopPos, powerStartPos, powerStopPos, divideNextElement);
            }
            if (!value.HasValue)
            {
                throw new Exception("No value found");
            }
            if (unitElement == null)
            {
                throw new Exception("No unit found");
            }
            return new UnitValue<T>(value.Value, unitElement);
        }

        private UnitElement<T> ExtractUnitElement(UnitElement<T> unitElement, string data, int noPosition, int unitStartPos, int unitStopPos, int powerStartPos, int powerStopPos, bool divideNextElement)
        {
            long power = 1;
            if (powerStartPos == noPosition && powerStopPos != noPosition)
            {
                throw new Exception(string.Format("Incohenrent power state : power stop pos = ({0}) while there is no power start pos", powerStopPos));
            }
            if (powerStartPos != noPosition && powerStopPos == noPosition)
            {
                throw new Exception(string.Format("Incohenrent power state : power start pos = ({0}) while there is no power stop pos", powerStartPos));
            }
            if (powerStartPos != noPosition && powerStopPos != noPosition)
            {
                power = parsePower(data.Substring(powerStartPos, powerStopPos - powerStartPos));
            }
            var unitName = GetUnitName(data.Substring(unitStartPos, unitStopPos - unitStartPos));
            var unitNamePower = new UnitNamePower<T>(unitName, divideNextElement ? -power : power);

            if (unitElement == null)
            {
                unitElement = new UnitElement<T>(unitNamePower);
            }
            else
            {
                unitElement = new UnitElement<T>(unitElement.GetUnitNamePowers().Union(new UnitNamePower<T>[] { unitNamePower }));
            }
            return unitElement;
        }

        protected abstract UnitName<T> GetUnitName(string data);

        public UnitElement<T> Multiply(UnitElement<T> unit1, UnitElement<T> unit2)
        {
            throw new NotImplementedException();
        }

        public UnitElement<T> Divide(UnitElement<T> unit1, UnitElement<T> unit2)
        {
            throw new NotImplementedException();
        }

        public UnitValue<T> Add(UnitValue<T> unit1, UnitValue<T> unit2)
        {
            throw new NotImplementedException();
        }

        public UnitValue<T> Substract(UnitValue<T> unit1, UnitValue<T> unit2)
        {
            throw new NotImplementedException();
        }

        public UnitValue<T> Multiply(UnitValue<T> unit1, UnitValue<T> unit2)
        {
            throw new NotImplementedException();
        }

        public UnitValue<T> Divide(UnitValue<T> unit1, UnitValue<T> unit2)
        {
            throw new NotImplementedException();
        }
    }

    public class UnitService : UnitService<float>, IUnitService
    {
        private IConstantProvider ConstantProvider { get; set; }

        public UnitService(IConstantProvider constantProvider)
        {
            ConstantProvider = constantProvider;
        }

        public override float ParseValue(string data)
        {
            try
            {
                return float.Parse(data, CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Parsing error : Impossible to parse [{0}] as a value", data), ex);
            }
        }

        protected override UnitName<float> GetUnitName(string data)
        {
            UnitPrefix prefix = null;
            UnitBaseName baseName = null;
            for (int index = 0; index < data.Length; index++)
            {
                var stringPrefix = data.Substring(0, index);
                var stringSuffix = data.Substring(index, data.Length - index);
                try
                {
                    prefix = ConstantProvider.GetPrefixBySymbol(stringPrefix);
                    baseName = ConstantProvider.GetUnitBySymbol(stringSuffix);
                }
                catch (UnitNotFoundException)
                {
                }
            }
            if (prefix == null || baseName == null)
            {
                throw new Exception(string.Format("Can't find any prefix+unit called [{0}].", data));
            }
            return new UnitName { Prefix = prefix, BaseName = baseName };
        }
    }
}