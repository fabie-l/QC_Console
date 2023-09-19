using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data.Entity;
using System.Security.Cryptography.X509Certificates;

namespace QC_Console
{
    public class CSQLite
    {
        public static SQLiteConnection OpenConnection(string db)
        {
            SQLiteConnection connection;
            connection = new SQLiteConnection($"Data Source={db};");
            
             try
            {
                connection.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return connection;
        }

        public static void CloseConnection(SQLiteConnection connection)
        {
            try
            {
                connection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public static void InsertData(SQLiteConnection connection, string insertQuery)
        {
            try
            {
                SQLiteCommand insertCommand = new SQLiteCommand(insertQuery, connection);
                

                int rowsAffected = insertCommand.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    Console.WriteLine("Row inserted successfully.");
                }
                else
                {
                    Console.WriteLine("Row insertion failed.");
                }
            }
            catch (SQLiteException e)
            {
                Console.WriteLine(e.ToString());
            }

        }
    }
}
