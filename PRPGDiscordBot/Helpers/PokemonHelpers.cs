using Discord;
using MySql.Data.MySqlClient;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PRPGDiscordBot.Helpers
{
    public static partial class DatabaseHelper
    {
        public static async Task<bool> RegisterUser(this MySqlConnection connection, ulong uuid, string starterXML)
        {
            bool success;
            try
            {
                await connection.OpenAsync();
                string cmdString = $"INSERT INTO Trainers (UUID, Team, Money) VALUES ('{uuid}',@starterXML,'0')";

                MySqlCommand cmd = new MySqlCommand(cmdString, connection);

                cmd.Parameters.Add("@starterXML", MySqlDbType.Text).Value = starterXML;

                await cmd.ExecuteNonQueryAsync();
                success = true;

                if (cachedRegistry.ContainsKey(uuid))
                    cachedRegistry[uuid] = true;
                else
                    cachedRegistry.Add(uuid, true);
            }
            catch (Exception e)
            {
                await Program.Log(e.ToString(), "Database -> Register User", LogSeverity.Error);
                success = false;
            }
            finally
            {
                await connection.CloseAsync();
            }
            return success;
        }
    }

    /// <summary>
    /// Creates a nickname and sanatize it.<para/>
    /// Throws: <seealso cref="FormatException"/>
    /// </summary>
    /// <exception cref="FormatException">Name denied</exception>
    public class Nickname
    {
        string name;

        /// <summary>
        /// Creates a nickname and sanatize it.<para/>
        /// Throws: <seealso cref="FormatException"/>
        /// </summary>
        /// <exception cref="FormatException">Name denied</exception>
        /// <param name="name">The Nickname</param>
        public Nickname(string name)
        {
            this.name = this.Sanitize(name);
        }

        private string Sanitize(string name)
        {
            if (Regex.IsMatch(name, @"([^a-zA-Z0-9\s-_])+"))
                throw new FormatException("Nickname does not confirm to Sanatization rules.");
            if (name.Length > 20)
                throw new FormatException("Nickname is too long");
            return name;
        }

        public override string ToString()
        {
            return name;
        }

        public static explicit operator string(Nickname nickname)
        {
            return nickname.ToString();
        }
    }
}

