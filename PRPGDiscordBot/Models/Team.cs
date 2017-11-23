using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PRPGDiscordBot.Models
{
    [Serializable]
    public class Team : IEnumerable<IPokemon>, ICollection<IPokemon>
    {
        private List<IPokemon> team;

        public Team()
        {
            team = new List<IPokemon>();
        }

        #region Implementing List components.
        public int Count => team.Count;

        public bool IsReadOnly => false;

        public void Add(IPokemon item)
        {
            team.Add(item);
        }

        public void Clear()
        {
            team.Clear();
        }

        public bool Contains(IPokemon item)
        {
            return team.Contains(item);
        }

        public void CopyTo(IPokemon[] array, int arrayIndex)
        {
            team.CopyTo(array, arrayIndex);
        }

        public IEnumerator<IPokemon> GetEnumerator()
        {
            return team.GetEnumerator();
        }

        public bool Remove(IPokemon item)
        {
            return team.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }
        #endregion
    }
}