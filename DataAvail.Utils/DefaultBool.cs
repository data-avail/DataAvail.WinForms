using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.Utils
{
    [System.ComponentModel.TypeConverter(typeof(DefaultBooleanConverter))]
    public enum DefaultBoolType
    {
        True,
        False,
        Default
    }

    public class DefaultBooleanConverter : System.ComponentModel.EnumConverter
    {
        public DefaultBooleanConverter(Type type)
            :base(type)
        {
        }

        public override bool CanConvertFrom(System.ComponentModel.ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(bool) ? true : base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(System.ComponentModel.ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(bool) ? true : base.CanConvertTo(context, destinationType);
        }

        public override object ConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value.GetType() == typeof(bool))
            {
                return (bool)value ? DefaultBoolType.True : DefaultBoolType.False;
            }
            else
            {
                return base.ConvertFrom(context, culture, value);
            }
        }

        public override object ConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (value.GetType() == typeof(DefaultBoolType) && destinationType == typeof(bool))
            {
                return ((DefaultBoolType)value) == DefaultBoolType.True ? true : false;
            }
            else
            {
                return base.ConvertTo(context, culture, value, destinationType);
            }
        }
    }

    public struct DefaultBool
    {
        public DefaultBool(DefaultBoolType Value)
        {
            value = Value;
        }

        public DefaultBool(string Value)
        {
            value = DefaultBoolType.Default;

            this.Parse(Value);
        }

        public static implicit operator DefaultBool (string Value)
        {
            return new DefaultBool(Value);
        }

        public static implicit operator DefaultBool(bool Value)
        {
            return new DefaultBool(Value ? DefaultBoolType.True : DefaultBoolType.False);
        }

        public static implicit operator bool(DefaultBool Value)
        {
            return Value.value == DefaultBoolType.True ? true : false;
        }

        public static implicit operator DefaultBool(DefaultBoolType Value)
        {
            return new DefaultBool(Value);
        }

        public static implicit operator DefaultBoolType(DefaultBool Value)
        {
            return Value.value;
        }

        public static DefaultBoolType Convert(bool Value)
        {
            return Value ? DefaultBoolType.True : DefaultBoolType.False;
        }

        public static bool Convert(DefaultBoolType Value)
        {
            return Convert(Value, true);
        }

        public static bool Convert(DefaultBoolType Value, bool DefaultAsTrue)
        {
            return Value == DefaultBoolType.True || (Value == DefaultBoolType.Default && DefaultAsTrue)? true : false;
        }


        public void Parse(string Value)
        {
            switch (Value)
            { 
                case "true":
                    value = DefaultBoolType.True;
                    break;
                case "false":
                    value = DefaultBoolType.False;
                    break;
                default:
                    value = DefaultBoolType.Default;
                    break;
            }
        }

        public DefaultBoolType value;
    }

}
