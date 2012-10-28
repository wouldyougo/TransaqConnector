using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.ComponentModel;

namespace StockSharp.Transaq.Serialization
{
    public class ValueParser:TypeConverter
    {
        private Dictionary<Type, MethodInfo> _parsers;
        

        public  ValueParser(CultureInfo culture)
        {
            _parsers = new Dictionary<Type, MethodInfo>();
            Culture = culture;
            MethodInfo[] methods= typeof(ValueParser).GetMethods();
            MethodInfo curM;
            object[] attribs;
            for (int i = 0; i < methods.Length; i++)
            {
                curM = methods[i];
                attribs = curM.GetCustomAttributes(typeof(TypeParserAttribute), true);
                if (attribs == null || attribs.Length == 0) continue;
                _parsers.Add(((TypeParserAttribute)attribs[0]).Type, curM);
            }
        }

        protected Dictionary<Type, MethodInfo> Parsers
        {
            get
            {
                return _parsers;
            }
        }

        protected CultureInfo Culture
        {
            get;
            set;
        }

        [TypeParser(typeof(double?))]
        public virtual double? ToDoubleNullable(String s)
        {
            double result;
            if (String.IsNullOrEmpty(s) || !double.TryParse(s,NumberStyles.Float,Culture.NumberFormat, out result)) return null;
            else
                return result;
        }

        [TypeParser(typeof(decimal?))]
        public virtual decimal? ToDecimalNullable(String s)
        {
            decimal result;
            if (String.IsNullOrEmpty(s) || !decimal.TryParse(s, NumberStyles.AllowDecimalPoint, Culture.NumberFormat, out result)) return null;
            else
                return result;
        }

        [TypeParser(typeof(int?))]
        public virtual int? ToIntNullable(String s)
        {
            int result;
            if (String.IsNullOrEmpty(s) || !int.TryParse(s, NumberStyles.Number, Culture.NumberFormat, out result)) return null;
            else
                return result;
     
        }

        [TypeParser(typeof(long?))]
        public virtual long? ToLongNullable(String s)
        {
            long result;
            if (String.IsNullOrEmpty(s) || !long.TryParse(s, NumberStyles.Number, Culture.NumberFormat, out result)) return null;
            else
                return result;

        }

        [TypeParser(typeof(DateTime?))]
        public virtual DateTime? ToDateTimeNullable(String s)
        {
            DateTime result;
            if (String.IsNullOrEmpty(s) || !DateTime.TryParse(s, Culture.DateTimeFormat,DateTimeStyles.AssumeLocal, out result)) return null;
            else
                return result;
        }

        [TypeParser(typeof(bool?))]
        public virtual Boolean? ToBooleanNullable(String s)
        {
            bool result;
            if (String.IsNullOrEmpty(s) || !bool.TryParse(s, out result)) return null;
            else
                return result;
        }

        [TypeParser(typeof(bool))]
        public bool ToBool(String s)
        {
            bool? result = ToBooleanNullable(s);
            if (result == null)
                throw new ArgumentOutOfRangeException();
            else
                return (bool)result;
        }

        [TypeParser(typeof(int))]
        public int ToInt(String s)
        {
            int? result = ToIntNullable(s);
            if (result == null)
                return 0;
            else
                return (int)result;
        }
       
        [TypeParser(typeof(long))]
        public long ToLong(String s)
        {
            long? result = ToLongNullable(s);
            if (result == null)
                return 0;
            else
                return (long)result;
        }

        [TypeParser(typeof(DateTime))]
        public DateTime ToDateTime(String s)
        {
            DateTime? result = ToDateTimeNullable(s);
            if (result == null)
                return new DateTime();
            else
                return (DateTime)result;
        }

        [TypeParser(typeof(double))]
        public double ToDouble(String s)
        {
            double? result = ToDoubleNullable(s);
            if (result == null)
                return 0;
            else
                return (double)result;
        }

        [TypeParser(typeof(decimal))]
        public decimal ToDecimal(String s)
        {
            decimal? result = ToDecimalNullable(s);
            if (result == null)
                return 0;
            else
                return (decimal)result;
        }


        [TypeParser(typeof(String))]
        public String ToString(String s)
        {
            return s;
        }

        public virtual object ToEnum(Type enumType,String s)
        {
            if (String.IsNullOrEmpty(s)) return null;
            return Enum.Parse(enumType, s);
        }

        public override bool  CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return CanParse(destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            return Parse(destinationType, (String)value);
        }

        public bool CanParse(Type t)
        {
            if (t.BaseType == typeof(Enum) || (t.IsGenericType&& t.GetGenericTypeDefinition()==typeof(Nullable<>) 
                && Nullable.GetUnderlyingType(t).BaseType == typeof(Enum)))
                
                return true;
            return _parsers.ContainsKey(t);
        }

        public object Parse(Type t, String s)
        {
            if (t.BaseType == typeof(Enum))
                return ToEnum(t, s);
            if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>) )
            {
                Type enumType = Nullable.GetUnderlyingType(t);
                if (enumType.BaseType == typeof(Enum))
                    return ToEnum(enumType, s);
            }
            if (!_parsers.ContainsKey(t)) throw new ArgumentOutOfRangeException("t");
            else
                return _parsers[t].Invoke(this, new object[]{s});
            
        }

       
    }

   
}
