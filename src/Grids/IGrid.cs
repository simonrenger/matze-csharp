using System.Collections.Generic;
using Matze.Utils;

namespace Matze.Grids
{
    public interface IGrid { }

    public interface IIndexable<TKey, TVal>
    {
        TVal this[TKey key] { get; }
    }
    public abstract class GridList<T> : IGrid
    {
        protected List<List<T>> grid;

        public List<T> this[int index]
        {
            get { return grid[index]; }
        }

        public int Width
        {
            get
            {
                return grid[0].Count;
            }
        }

        public int Height
        {
            get { return grid.Count; }
        }

        public int Size()
        {
            return grid.Size();
        }

        internal List<List<T>> Grid => grid;
    }
}