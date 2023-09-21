using System.Data.SqlClient;
using System.Data.SQLite;
using System.Security.Cryptography.X509Certificates;
using IronPdf;

namespace QC_Console
{
    internal class Program
    {
        Dictionary <long, string> owners = new Dictionary <long, string> ()
        {
            
        };

        static void Main(string[] args)
        {

            /*SQLiteConnection connection = CSQLite.OpenConnection(Directory.GetCurrentDirectory() + "\\copy\\data.qda");

            Cases cases = new Cases(100000000000000, "Raj");

            for (int i = 0; i < 100; i++)
            {
                cases.Generate(connection);
            }

            CSQLite.CloseConnection(connection);*/

            /*PdfDocument pdf = PdfDocument.FromFile(Directory.GetCurrentDirectory() + "\\documents\\dummy.pdf");

            string text = pdf.ExtractAllText();

            Console.WriteLine(text);*/

            string sqlServerConnectionString = "Data Source=HP-FABIE\\SQLEXPRESS;Initial Catalog=QC_v1;Integrated Security=True;";

            /*            string sqliteConnectionString = Directory.GetCurrentDirectory() + "\\copy\\data.qda";
            */
            string db = "C:/Users/lesli/Projects/QC_Console/QC_Console/bin/debug/net6.0/copy/data.qda";
            string sqliteConnectionString = $"Data Source={db};";
            try
            {
                using (SQLiteConnection sqliteConnection = new SQLiteConnection(sqliteConnectionString))
                {
                    sqliteConnection.Open();

                    using (SQLiteCommand sqliteCommand = new SQLiteCommand("SELECT * FROM cases", sqliteConnection))
                    {
                        using (SQLiteDataReader sqliteReader = sqliteCommand.ExecuteReader())
                        {
                            using (SqlConnection sqlConnection = new SqlConnection(sqlServerConnectionString))
                            {
                                sqlConnection.Open();

                                // Check if the SQL Server database exists, and create it if it doesn't
                                string dbName = sqlConnection.Database;
                                if (!DatabaseExists(sqlConnection, dbName))
                                {
                                    CreateDatabase(sqlConnection, dbName);
                                }

                                while (sqliteReader.Read())
                                {
                                    using (SqlCommand sqlCommand = new SqlCommand("INSERT INTO sql_server_table (caseid, name, memo, owner, date) VALUES (@param1, @param2, @param3, @param4, @param5)", sqlConnection))
                                    {
                                        sqlCommand.Parameters.AddWithValue("@param1", sqliteReader["caseid"]);
                                        sqlCommand.Parameters.AddWithValue("@param2", sqliteReader["name"]);
                                        sqlCommand.Parameters.AddWithValue("@param3", sqliteReader["memo"]);
                                        sqlCommand.Parameters.AddWithValue("@param4", sqliteReader["owner"]);
                                        sqlCommand.Parameters.AddWithValue("@param5", sqliteReader["date"]);


                                        sqlCommand.ExecuteNonQuery();
                                    }
                                }
                            }
                        }
                    }
                }

                Console.WriteLine("Data transfer completed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }

            // Helper function to check if a database exists
            bool DatabaseExists(SqlConnection connection, string databaseName)
            {
                string query = $"SELECT 1 FROM sys.databases WHERE name = '{databaseName}'";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    return cmd.ExecuteScalar() != null;
                }
            }

            // Helper function to create a database
            void CreateDatabase(SqlConnection connection, string databaseName)
            {
                string createDatabaseQuery = $"CREATE DATABASE [{databaseName}]";
                using (SqlCommand createDbCommand = new SqlCommand(createDatabaseQuery, connection))
                {
                    createDbCommand.ExecuteNonQuery();
                }
            }


        }
    }
}