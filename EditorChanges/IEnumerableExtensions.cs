using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditorChanges
{
  static class IEnumerableExtensions
  {
    public static IEnumerable<List<T>> Split<T>(this IEnumerable<T> source, Func<T, bool> isLast)
    {
      var list = new List<T>();
      foreach (var item in source)
      {
        list.Add(item);
        if (isLast(item))
        {
          yield return list;
          list = new List<T>();
        }
      }
      yield return list;
    }
  }
}
