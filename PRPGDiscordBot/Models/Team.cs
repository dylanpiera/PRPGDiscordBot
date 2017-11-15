using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PRPGDiscordBot.Models
{
    [Serializable]
    public class Team : ITeam, IEnumerable<Pokemon>, ICollection<Pokemon>
    {
        private List<Pokemon> team;

        public Team()
        {
            team = new List<Pokemon>();
        }

        #region Implementing List components.
        public int Count => team.Count;

        public bool IsReadOnly => false;

        public void Add(Pokemon item)
        {
            team.Add(item);
        }

        public void Clear()
        {
            team.Clear();
        }

        public bool Contains(Pokemon item)
        {
            return team.Contains(item);
        }

        public void CopyTo(Pokemon[] array, int arrayIndex)
        {
            team.CopyTo(array, arrayIndex);
        }

        public IEnumerator<Pokemon> GetEnumerator()
        {
            return team.GetEnumerator();
        }

        public bool Remove(Pokemon item)
        {
            return team.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }
        #endregion
    }

    public interface ITeam
    {

    }
}