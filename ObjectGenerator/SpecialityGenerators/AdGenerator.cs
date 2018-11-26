using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Domain.Concrete;

namespace ObjectGenerator.SpecialityGenerators
{
    public class AdGenerator : BaseGenerator<Ad>
    {
        private enum ValidFileExtensions { mov, flv, gif, gifv, avi, wmv, mp4 }
        public override Ad PropertiesSetter(Ad obj)
        {
            Type type = obj.GetType();

            foreach (var info in type.GetProperties())
            {
                if (info.Name == "FileExtension")
                    GenereateFileExtension(obj, info);
            }

            return base.PropertiesSetter(obj);
        }

        private void GenereateFileExtension(Ad obj, PropertyInfo info)
        {
            string value = ".";

            Array values = Enum.GetValues(typeof(ValidFileExtensions));
            ValidFileExtensions randomExtension = (ValidFileExtensions)values.GetValue(random.Next(values.Length));

            value += randomExtension;

            info.SetValue(obj, value);

        }
    }
}
