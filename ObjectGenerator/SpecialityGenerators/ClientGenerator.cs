using Domain.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ObjectGenerator.SpecialityGenerators
{
    public class ClientGenerator : BaseGenerator<Client>
    {
        public override Client PropertiesSetter(Client obj)
        {
            Type type = obj.GetType();

            foreach (var info in type.GetProperties())
            {
                if (info.Name == "Ip")
                    GenerateIp(obj, info);
            }

            return base.PropertiesSetter(obj);
        }

        private void GenerateIp(Client obj, PropertyInfo info)
        {
            string value = "";

            for (int i = 0; i < 4; i++)
            {
                value += random.Next(1, 255) + ".";
            }

            value = value.Remove(value.Length-1);

            info.SetValue(obj, value);
        }
    }
}
