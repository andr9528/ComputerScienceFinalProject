using Repository.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics.PerformanceData;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;

namespace ObjectGenerator
{
    public class BaseGenerator<T> where T : class, IEntity
    {
        internal const string letters = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz";
        internal const string numbers = "0123456789";
        internal Random random = new Random();

        virtual public T PropertiesSetter(T obj)
        {
            Type type = obj.GetType();

            foreach (var info in type.GetProperties())
            {
                if (info.Name == "Id" || CheckPropertyValue(obj, info))
                    continue;

                Dictionary<Type, Action> @switch = new Dictionary<Type, Action> {
                    { typeof(int), () => SetInt(obj, info)},
                    { typeof(string), () => SetString(obj, info)}
                };

                // If it's not nullable, or it is a string, then it enters.
                if (!(Nullable.GetUnderlyingType(info.PropertyType) != null) || info.PropertyType == typeof(string))
                {
                    if (@switch.ContainsKey(info.PropertyType))
                    {
                        @switch[info.PropertyType]();
                    }
                }
            }

            return obj;
        }

        private bool CheckPropertyValue(T obj, PropertyInfo info)
        {
            if (info == null)
                return false;
            if (info.GetValue(obj) == null)
                return false;

            Type propType = info.PropertyType;
            object value = info.GetValue(obj);
            switch (propType.Name)
            {
                case "Int32":
                    if ((int)value == 0 || (int)value == -1)
                        return false;
                    break;
                case "DateTime":
                    if ((DateTime)value == default(DateTime))
                        return false;
                    break;
                case "String":
                    if ((string) value == default(string) || (string) value == string.Empty)
                        return false;
                    break;
            }
            return true;
        }

        private void SetInt(T obj, PropertyInfo info)
        {
            info.SetValue(obj, random.Next(0, 1000));
        }

        private void SetString(T obj, PropertyInfo info)
        {
            string value = "";
            int charsCount = random.Next(0, 50);

            for (int i = 0; i < charsCount; i++)
            {
                value += letters[random.Next(0, letters.Length-1)];
            }

            info.SetValue(obj, value);
        }
    }
}
