using System;
using System.Collections.Generic;
using System.Linq;
using Unit.Lib.Core.DomainModel;
using Unit.Lib.Core.DomainModel.Enumeration;
using Unit.Lib.Core.Exceptions;
using Unit.Lib.Core.Service;

namespace Unit.Lib.Service
{
    public class UnitService<S, T> : IUnitService<S, T> where S : class, IScalar<T>, new()
    {
        public UnitService(IConstantProvider<S, T> constantProvider)
        {
            ConstantProvider = constantProvider;
        }

        protected S NoScalar { get; } = new S();

        protected IConstantProvider<S, T> ConstantProvider { get; set; }

        private bool IsSpaceALike(char c) => c == ' ' || c == '\t' || c == '\r' || c == '\n';

        private bool IsNotValueAnymore(char c) => IsSpaceALike(c) || !((c >= '0' && c <= '9') || (c == ',' || c == '_' || c == '.' || c == '+' || c == '-'));

        private bool IsPower(char c) => (c >= '0' && c <= '9') || (c == '-');

        private bool IsElementSepator(char c) => (c == '*' || c == '.' || c == '/');

        public S ParseValue(string data) => (NoScalar.Parse(data)) as S;

        private long ParsePower(string data)
        {
            try
            {
                return long.Parse(data);
            }
            catch (Exception ex)
            {
                throw new UnitParserException(string.Format("Parsing error : Impossible to parse [{0}] as a power", data), ex);
            }
        }

        public UnitElement<S, T> ParseUnit(string data)
        {
            const int noPosition = -1;
            int unitStartPos = noPosition;
            int unitStopPos = noPosition;
            int powerStartPos = noPosition;
            int powerStopPos = noPosition;
            bool divideNextElement = false;
            UnitElement<S, T> unitElement = null;

            for (int index = 0; index < data.Length; index++)
            {
                char item = data[index];
                if (unitStartPos == noPosition)
                {
                    if (!IsSpaceALike(item))
                    {
                        unitStartPos = index;
                    }
                }
                else if (unitStartPos != noPosition && unitStopPos == noPosition)
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
                else if (unitStopPos == noPosition && powerStartPos != noPosition)
                {
                    if (!IsPower(item))
                    {
                        powerStopPos = index;
                    }
                }
                else if (unitStopPos != noPosition && powerStartPos == noPosition)
                {
                    if (IsPower(item))
                    {
                        powerStartPos = index;
                    }
                }
                else if (unitStopPos != noPosition && powerStartPos != noPosition && powerStopPos == noPosition)
                {
                    if (!IsPower(item))
                    {
                        powerStopPos = index;
                    }
                }

                if (unitStartPos != noPosition)
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
            if (unitStartPos != noPosition)
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
            if (unitElement == null)
            {
                throw new UnitParserException("No unit found");
            }
            return unitElement;
        }

        public UnitValue<S, T> Parse(string data)
        {
            const int noPosition = -1;
            int valueStartPos = noPosition;
            int valueStopPos = noPosition;
            int unitStartPos = noPosition;
            int unitStopPos = noPosition;
            int powerStartPos = noPosition;
            int powerStopPos = noPosition;
            bool divideNextElement = false;
            S value = null;
            UnitElement<S, T> unitElement = null;

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
                            throw new UnitParserException(string.Format("Parsing error : no valid value found (Start value pos ({0}) == Stop value pos ({1}))", valueStartPos, valueStopPos));
                        }
                        value = ParseValue(data.Substring(valueStartPos, valueStopPos - valueStartPos));
                    }
                }
                else if (value != null && unitStartPos == noPosition)
                {
                    if (!IsSpaceALike(item))
                    {
                        unitStartPos = index;
                    }
                }
                else if (value != null && unitStartPos != noPosition && unitStopPos == noPosition)
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
                else if (value != null && unitStopPos == noPosition && powerStartPos != noPosition)
                {
                    if (!IsPower(item))
                    {
                        powerStopPos = index;
                    }
                }
                else if (value != null && unitStopPos != noPosition && powerStartPos == noPosition)
                {
                    if (IsPower(item))
                    {
                        powerStartPos = index;
                    }
                }
                else if (value != null && unitStopPos != noPosition && powerStartPos != noPosition && powerStopPos == noPosition)
                {
                    if (!IsPower(item))
                    {
                        powerStopPos = index;
                    }
                }

                if (value != null && unitStartPos != noPosition)
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
            if (value != null && unitStartPos != noPosition)
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
            if (value == null)
            {
                throw new UnitParserException("No value found");
            }
            if (unitElement == null)
            {
                throw new UnitParserException("No unit found");
            }
            return new UnitValue<S, T>(value, unitElement);
        }

        private UnitElement<S, T> ExtractUnitElement(UnitElement<S, T> unitElement, string data, int noPosition, int unitStartPos, int unitStopPos, int powerStartPos, int powerStopPos, bool divideNextElement)
        {
            long power = 1;
            if (powerStartPos == noPosition && powerStopPos != noPosition)
            {
                throw new UnitParserException(string.Format("Incohenrent power state : power stop pos = ({0}) while there is no power start pos", powerStopPos));
            }
            if (powerStartPos != noPosition && powerStopPos == noPosition)
            {
                throw new UnitParserException(string.Format("Incohenrent power state : power start pos = ({0}) while there is no power stop pos", powerStartPos));
            }
            if (powerStartPos != noPosition && powerStopPos != noPosition)
            {
                power = ParsePower(data.Substring(powerStartPos, powerStopPos - powerStartPos));
            }
            var unitName = GetUnitName(data.Substring(unitStartPos, unitStopPos - unitStartPos));
            var unitNamePower = new UnitNamePower<S, T>(unitName, divideNextElement ? -power : power);

            if (unitElement == null)
            {
                unitElement = new UnitElement<S, T>(unitNamePower);
            }
            else
            {
                unitElement = new UnitElement<S, T>(unitElement.GetUnitNamePowers().Union(new UnitNamePower<S, T>[] { unitNamePower }));
            }
            return unitElement;
        }

        protected UnitValue<S, T> GetNone() => new UnitValue<S, T>
            (
                NeutralScalar,
                new UnitElement<S, T>(new UnitNamePower<S, T>(ConstantProvider.GetPrefixByName(""), ConstantProvider.GetUnitByName("")))
            );

        protected UnitName<S, T> GetUnitName(string data)
        {
            UnitPrefix<S, T> prefix = null;
            UnitBaseName<S, T> baseName = null;
            for (int index = 0; index < data.Length; index++)
            {
                var stringPrefix = data.Substring(0, index);
                var stringSuffix = data.Substring(index, data.Length - index);
                try
                {
                    prefix = ConstantProvider.GetPrefixBySymbol(stringPrefix);
                    baseName = ConstantProvider.GetUnitBySymbol(stringSuffix);
                    if (prefix != null && baseName != null)
                    {
                        break;
                    }
                }
                catch (UnitNotFoundException)
                {
                }
            }
            if (prefix == null || baseName == null)
            {
                throw new UnitParserException(string.Format("Can't find any prefix+unit called [{0}].", data));
            }
            return new UnitName<S, T>(prefix, baseName: baseName);
        }

        public UnitValue<S, T> Multiply(UnitValue<S, T> unit1, UnitValue<S, T> unit2)
        {
            var result = new UnitValue<S, T>(MultiplyScalar(unit1.Value, unit2.Value), new UnitElement<S, T>(unit1.UnitElement.GetUnitNamePowers().Union(unit2.UnitElement.GetUnitNamePowers())));
            return result;
        }

        public UnitValue<S, T> Divide(UnitValue<S, T> unit1, UnitValue<S, T> unit2)
        {
            var result = new UnitValue<S, T>(DivideScalar(unit1.Value, unit2.Value), new UnitElement<S, T>(unit1.UnitElement.GetUnitNamePowers().Union(unit2.UnitElement.GetUnitNamePowers().Select(unp => new UnitNamePower<S, T>(unp.UnitName, -unp.Power)))));
            return result;
        }

        public UnitValue<S, T> Add(UnitValue<S, T> unit1, UnitValue<S, T> unit2)
        {
            throw new NotImplementedException();
        }

        public UnitValue<S, T> Substract(UnitValue<S, T> unit1, UnitValue<S, T> unit2)
        {
            throw new NotImplementedException();
        }

        protected S NeutralScalar => NoScalar.GetNeutral() as S;

        protected S MultiplyScalar(S t1, S t2) => t1.Multiply(t2) as S;

        protected S DivideScalar(S t1, S t2) => t1.Divide(t2) as S;

        protected S ApplyScalarPower(S t1, S t2, long power) => t1.Multiply(t2).ApplyPower(power) as S;

        protected S MultiplyScalars(IEnumerable<S> scalars) => scalars.Aggregate(NeutralScalar, (x, y) => MultiplyScalar(x, y));

        protected Dictionary<UnitBaseQuantity, UnitValue<S, T>> ReferenceByQuantity => ConstantProvider.ReferenceByQuantity.ToDictionary(x => x.Key, x => new UnitValue<S, T>(NeutralScalar, x.Value as UnitElement<S, T>));

        public UnitValue<S, T> Convert(UnitValue<S, T> value)
        {
            var dimension = value.GetDimension();
            var result = GetNone();
            var valueRest = value;
            foreach (var quantity in UnitDimension.UnitBaseQuantities)
            {
                var power = dimension.GetPower(quantity);
                var reference = ReferenceByQuantity[quantity];
                var powerRef = reference.GetDimension().GetPower(quantity);
                var count = power / powerRef;
                if (count > 0)
                {
                    while (count > 0)
                    {
                        result = Multiply(result, reference);
                        valueRest = Divide(valueRest, reference);
                        count -= 1;
                    }
                }
                else if (count < 0)
                {
                    while (count < 0)
                    {
                        result = Divide(result, reference);
                        valueRest = Multiply(valueRest, reference);
                        count += 1;
                    }
                }
            }
            var x = MultiplyScalars
                (
                    valueRest
                        .UnitElement
                        .GetUnitNamePowers()
                        .Select
                        (
                            unitNamePower =>
                                ApplyScalarPower
                                (
                                    unitNamePower.UnitName.Prefix.Factor,
                                    unitNamePower.UnitName.BaseName.Factor,
                                    unitNamePower.Power
                                )
                        )
                );
            result.Value = MultiplyScalar(result.Value, x);
            result.Value = MultiplyScalar(result.Value, valueRest.Value);
            result.UnitElement.Simplify();
            return result;
        }

        public UnitValue<S, T> Convert(UnitValue<S, T> value, UnitElement<S, T> unitElement)
        {
            throw new NotImplementedException();
        }
    }
}