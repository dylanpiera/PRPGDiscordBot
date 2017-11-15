using Discord;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PRPGDiscordBot.Helpers
{
    public static partial class DatabaseHelper
    {
        /// <summary>
        /// Gets you a copy of the database connection (Closed)
        /// </summary>
        public static MySqlConnection GetClosedConnection()
        {
            string connStr = $"Server={Sneaky.DatabaseUrl};Uid={Sneaky.User};Database=PRPG;port=3306;Password={Sneaky.Password}";
            MySqlConnection conn = new MySqlConnection(connStr);
            return conn;
        }
        #region getXMLGeneric
        public static async Task<string> GetXMLFromDatabaseAsync(this MySqlConnection connection, string columnName, string tableName, ulong UUID)
        {
            string xml = "";
            try
            {
                await connection.OpenAsync();

                string cmdString = $"SELECT {columnName} FROM {tableName} WHERE UUID = {UUID}";
                MySqlCommand cmd = new MySqlCommand(cmdString, connection);

                using (MySqlDataReader reader = (MySqlDataReader)await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        xml = reader.GetString(reader.GetOrdinal(columnName));
                    }
                }
            }
            catch (Exception e)
            {
                await Program.Log(new LogMessage(LogSeverity.Error, "XML Serializer", e.ToString()));
                return null;
            }
            finally
            {
                await connection.CloseAsync();
            }

            return xml;
        }
#endregion

        public static Dictionary<ulong, bool> cachedRegistry = new Dictionary<ulong, bool>();

        public static async Task<bool> IsUserRegistered(this MySqlConnection connection, ulong uuid)
        {
            if (cachedRegistry.ContainsKey(uuid))
            {
                return cachedRegistry[uuid];
            }

            bool IsRegistered = false;

            try
            {

                await connection.OpenAsync();

                string cmdString = $"SELECT COUNT(UUID) FROM Trainers WHERE UUID = '{uuid}'";
                MySqlCommand cmd = new MySqlCommand(cmdString, connection);

                using (MySqlDataReader reader = (MySqlDataReader)await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        IsRegistered = reader.GetInt16(0) > 0 ? true : false;
                    }
                }
            }
            catch (Exception e)
            {
                await Program.Log(new LogMessage(LogSeverity.Error, "IsRegistered", e.ToString()));
            }
            finally
            {
                await connection.CloseAsync();
            }
            cachedRegistry.Add(uuid, IsRegistered);
            return IsRegistered;
        }
    }

    public static class XMLHelper
    {
        public static string Serialize<T>(this T toSerialize)
        {
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(toSerialize.GetType());
                StringWriter stringWriter = new StringWriter();

                xmlSerializer.Serialize(stringWriter, toSerialize);
                string str = stringWriter.ToString();
                
                return str;
            }
            catch (Exception e)
            {
                Task.Run(async () => await Program.Log(new LogMessage(LogSeverity.Error, "XML Serializer", e.ToString())));
                return null;
            }
        }

        public static T Deserialize<T>(this string toDeserialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            StringReader stringReader = new StringReader(toDeserialize);
            return (T)xmlSerializer.Deserialize(stringReader);
        }

        public static T Deserialize<T>(this Task<string> toDeserialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            StringReader stringReader = new StringReader(toDeserialize.ToString());
            return (T)xmlSerializer.Deserialize(stringReader);
        }
    }
}
