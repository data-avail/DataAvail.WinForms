using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace DataAvail.Utils
{
    public static class Reflection
    {
        public static object Parse(Type Type, string Str)
        {
            if (Type == typeof(Type))
            {
                return Type.GetType(GetSysTypeNameByShortName(Str), true, true);
            }
            else if (Type != typeof(string))
                return Type.GetMethod("Parse", new Type[] { typeof(string) }).Invoke(null, new object[] { Str });
            else
                return Str;
            //http://stackoverflow.com/questions/8625/generic-type-conversion-from-string
            //(T)Convert.ChangeType(base.Value, typeof(T)); 
        }

        public static string GetSysTypeNameByShortName(string ShortName)
        {
            switch (ShortName.ToLower())
            {
                case "int":
                    return "System.Int32";
                case "uint":
                    return "System.UInt32";
                case "bool":
                    return "System.Boolean";
                case "float":
                    return "System.Single";
                case "long":
                    return "System.Int64";
                case "ulong":
                    return "System.UInt64";
                case "short":
                    return "System.Int16";
                case "ushort":
                    return "System.UInt16";
                case "date":
                    return "System.DateTime";
            }

            return string.Format("System.{0}", ShortName);
        }

        public static Type GetItemType(Type CollectionType)
        {
            Type custType = CollectionType;

            if (custType.IsArray)
            {
                custType = custType.GetElementType();
            }
            else if (custType.IsGenericType)
            {
                custType = custType.GetGenericArguments()[0];
            }

            return custType;
        }

        public static T GetCustomAttribute<T>(System.Reflection.PropertyInfo PropertyInfo) where T : System.Attribute
        {
            return PropertyInfo.GetCustomAttributes(typeof(T), true).SingleOrDefault() as T;
        }


        public static IEnumerable<System.Reflection.PropertyInfo> GetGetters(System.Type Type)
        {
            return GetGetters(Type, System.Reflection.BindingFlags.Instance);
        }

        /// <summary>
        /// Returns public properties which have getters.
        /// </summary>
        /// <param name="Type">Type from where properties are extracted.</param>
        /// <param name="BindingFlags">Static or Instance</param>
        /// <returns>Properties which have been extracted</returns>
        public static IEnumerable<System.Reflection.PropertyInfo> GetGetters(System.Type Type, System.Reflection.BindingFlags BindingFlags) 
        {
            return GetPublicProperties(Type, BindingFlags | System.Reflection.BindingFlags.GetProperty);
        }

        public static IEnumerable<System.Reflection.PropertyInfo> GetPublicProperties(System.Type Type)
        {
            return GetPublicProperties(Type, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.GetProperty | System.Reflection.BindingFlags.SetProperty);
        }

        public static IEnumerable<System.Reflection.PropertyInfo> GetPublicProperties(System.Type Type, System.Reflection.BindingFlags BindingFlags)
        {
            BindingFlags = BindingFlags | System.Reflection.BindingFlags.Public;

            return Type.GetProperties(BindingFlags).Cast<System.Reflection.PropertyInfo>();
        }

        /// <summary>
        /// Copy public proeprties from one object to another.
        /// </summary>
        public static void CopyProperties<T>(T SrcObject, T DestObject)
        {
            foreach (PropertyInfo pi in GetPublicProperties(SrcObject.GetType()))
                CopyProperty(SrcObject, pi, DestObject);

        }

        public static PropertyInfo GetSameProperty<T>(PropertyInfo AnotherObjProperty, T Obj)
        {
            return Obj.GetType().GetProperties().SingleOrDefault(p => p.Name == AnotherObjProperty.Name);
        }

        public static void CopyProperty<T>(T SrcObject, PropertyInfo SrcProperty, T DestObject)
        {
            PropertyInfo spi = GetSameProperty(SrcProperty, DestObject);

            if (spi != null)
                CopyProperty(SrcObject, SrcProperty, DestObject, spi);
        }

        public static void CopyProperty<T>(T SrcObject, PropertyInfo SrcProperty, T DestObject, PropertyInfo DestProperty)
        {
            SetValue(DestObject, DestProperty, GetValue(SrcObject, SrcProperty));
        }

        public static void SetValue<T>(T Object, PropertyInfo Property, object Value)
        {
            Property.SetValue(Object, Value, null);
        }

        public static object GetValue<T>(T Object, PropertyInfo Property)
        {
            return Property.GetValue(Object, null);
        }

        public static bool CompareByProperties<T>(T ObjA, T ObjB)
        {
            foreach (PropertyInfo pi in GetPublicProperties(ObjA.GetType()))
            {
                PropertyInfo spi = GetSameProperty(pi, ObjB);

                if (spi != null)
                    if (!GetValue(ObjA, pi).Equals(GetValue(ObjB, spi))) return false;
            }

            return true;
        }
    }
}
