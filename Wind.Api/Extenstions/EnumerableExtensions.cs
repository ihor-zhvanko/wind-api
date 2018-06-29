using System;
using System.Linq;
using System.Collections.Generic;

namespace Wind.Api.Extenstions
{
  public static class EnumerableExtenstions
  {
    public static IEnumerable<T> Distinct<T>(this IEnumerable<T> collection, Func<T, T, bool> comparer)
    {
      var result = new List<T>();
      foreach (var item in collection)
      {
        if (result.FirstOrDefault(x => comparer(x, item)) == null)
        {
          result.Add(item);
        }
      }

      return result;
    }
  }
}