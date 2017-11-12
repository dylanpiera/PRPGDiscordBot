using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PRPGDiscordBot.Models
{
    [Serializable]
    public class Moves : IEnumerable<Move>, ICollection<Move>
    {
        private List<Move> moves;

        public Moves()
        {
            moves = new List<Move>(4);
        }

        #region Implementing List components.
        public int Count => moves.Count;

        public bool IsReadOnly => false;

        public void Add(Move item)
        {
            if(this.Count >= 4)
            {
                moves.RemoveAt(0);
            }
            moves.Add(item);
        }

        public void Clear()
        {
            moves.Clear();
        }

        public bool Contains(Move item)
        {
            return moves.Contains(item);
        }

        public void CopyTo(Move[] array, int arrayIndex)
        {
            moves.CopyTo(array, arrayIndex);
        }

        public IEnumerator<Move> GetEnumerator()
        {
            return moves.GetEnumerator();
        }

        public bool Remove(Move item)
        {
            return moves.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }
        #endregion
    }
}