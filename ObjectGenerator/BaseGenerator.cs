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
                if (info.Name == "Id")
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

        private void SetInt(T obj, PropertyInfo info)
        {

        }

        private void SetString(T obj, PropertyInfo info)
        {

        }
    }
}
