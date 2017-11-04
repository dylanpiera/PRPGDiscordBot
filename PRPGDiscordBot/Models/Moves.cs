using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace PRPGDiscordBot.Models
{
    [Serializable]
    public class Moves : IMoves, IEnumerable<IMove>, ICollection<IMove>
    {
        private List<IMove> team;

        public Moves()
        {
            team = new List<IMove>(4);
        }

        #region Implementing List components.
        public int Count => team.Count;

        public bool IsReadOnly => false;

        public void Add(IMove item)
        {
            team.Add(item);
        }

        public void Clear()
        {
            team.Clear();
        }

        public bool Contains(IMove item)
        {
            return team.Contains(item);
        }

        public void CopyTo(IMove[] array, int arrayIndex)
        {
            team.CopyTo(array, arrayIndex);
        }

        public IEnumerator<IMove> GetEnumerator()
        {
            return team.GetEnumerator();
        }

        public bool Remove(IMove item)
        {
            return team.Remove(item);
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