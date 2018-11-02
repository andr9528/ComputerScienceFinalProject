using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers
{
    public static class Extensions
    {
        public static DateTime FirstDayOfMonth(this DateTime value)
        {
            return new DateTime(value.Year, value.Month, 1);
        }

        public static DateTime LastDayOfMonth(this DateTime value, int monthOffset = 1)
        {
            return value.FirstDayOfMonth()
                .AddMonths(monthOffset)
                .AddMinutes(-1);
        }


        // https://stackoverflow.com/questions/31162576/entity-framework-add-if-not-exist-without-update
        public static EntityEntry<TEnt> AddIfNotExists<TEnt, TKey>(this DbSet<TEnt> dbSet, TEnt entity, Func<TEnt, TKey> predicate) where TEnt : class
        {
            var exists = dbSet.Any(c => predicate(entity).Equals(predicate(c)));
            return exists
                ? null
                : dbSet.Add(entity);
        }

        public static void AddRangeIfNotExists<TEnt, TKey>(this DbSet<TEnt> dbSet, IEnumerable<TEnt> entities, Func<TEnt, TKey> predicate) where TEnt : class
        {
            var entitiesExist = from ent in dbSet
                where entities.Any(add => predicate(ent).Equals(predicate(add)))
                select ent;

            dbSet.AddRange(entities.Except(entitiesExist));
        }

        // https://stackoverflow.com/questions/3453274/using-linq-to-get-the-last-n-elements-of-a-collection
        public static IEnumerable<T> TakeLast<T>(this IEnumerable<T> source, int N)
        {
            return source.Skip(Math.Max(0, source.Count() - N));
        }
    }
}
