using Repository.Core;
using System;

namespace Helpers
{
    public class Comparerer<T> where T : class, IEntity
    {
        private static Comparerer<T> _Singleton;
        public static Comparerer<T> Singleton
        {
            get
            {
                if (default(Comparerer<T>) == _Singleton)
                    _Singleton = new Comparerer<T>();
                return _Singleton;
            }
        }

        private Comparerer()
        {

        }
        public bool Compare(T x, T y)
        {
            return Compare(x, y, arg => arg.Id);
        }

        public bool Compare<TProp>(T x, T y, Func<T, TProp> property)
        {
            if (default(T) == x || default(T) == y)
                throw new ArgumentException();

            return property(x).Equals(property(y));
        }
    }
}
