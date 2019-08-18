using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matze.Utils
{
    public static class ListExtensions
    {
        public static void Shuffle<T>(this IList<T> ts, ref Random rand)
        {
            var count = ts.Count;
            var last = count - 1;
            for (var i = 0; i < last; ++i)
            {
                var r = rand.Next(i, count);
                var tmp = ts[i];
                ts[i] = ts[r];
                ts[r] = tmp;
            }
        }
        public static T Pop<T>(this IList<T> ts)
        {
            var pop = ts[ts.Size()];
            ts.RemoveAt(ts.Size());
            return pop;
        }
        /**
         * @brief Requests the Size of the List minus 1
         */
        public static int Size<T>(this IList<T> ts)
        {
            return ts.Count - 1;
        }
        // Source https://stackoverflow.com/questions/2094239/swap-two-items-in-listt
        public static void Swap<T>(IList<T> list, int indexA, int indexB)
        {
            T tmp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = tmp;
        }
    }
}
