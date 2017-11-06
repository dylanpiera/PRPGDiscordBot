using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace PRPGDiscordBot.Models
{
    [Serializable]
    public class Moves : IMoves, IEnumerable<IMove>, ICollection<IMove>
    {
        private List<IMove> moves;

        public Moves()
        {
            moves = new List<IMove>(4);
        }

        #region Implementing List components.
        public int Count => moves.Count;

        public bool IsReadOnly => false;

        public void Add(IMove item)
        {
            moves.Add(item);
        }

        public void Clear()
        {
            moves.Clear();
        }

        public bool Contains(IMove item)
        {
            return moves.Contains(item);
        }

        public void CopyTo(IMove[] array, int arrayIndex)
        {
            moves.CopyTo(array, arrayIndex);
        }

        public IEnumerator<IMove> GetEnumerator()
        {
            return moves.GetEnumerator();
        }

        public bool Remove(IMove item)
        {
            return moves.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }
        #endregion
    }

    public interface IMoves
    {

    }
}