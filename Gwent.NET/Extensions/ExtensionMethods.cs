using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Gwent.NET.Model;
using Gwent.NET.Model.Enums;

namespace Gwent.NET.Extensions
{
    public static class ExtensionMethods
    {
        public static void Shuffle<T>(this IList<T> list)
        {
            Random random = new Random();
            for (var i = 0; i < list.Count; i++)
            {
                list.Swap(i, random.Next(i, list.Count));
            }
        }

        private static void Swap<T>(this IList<T> list, int i, int j)
        {
            var temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }

        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                collection.Add(item);
            }
        }
        public static void AddRange<TEntity>(this IDbSet<TEntity> set, IEnumerable<TEntity> items) where TEntity : class
        {
            foreach (var item in items)
            {
                set.Add(item);
            }
        }
    }
}