using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QC_Console
{
    internal class GenerateCases
    {
        private readonly string _db;
        private readonly string _startId;
        private string _owner;
        const int count = 100;
        private int id;



        public GenerateCases(string db, string startId, string owner)
        {
            _db = db;
            _startId = startId;
            _owner = owner;

        }

        int ReadIdFromFile()
        {

            if (File.Exists(_startId))
            {
                string idText = File.ReadAllText(_startId);
                if (int.TryParse(idText, out int id))
                {
                    return id;
                }
            }

            return 1; // Default starting id
        }

        private void WriteIdToFile(int id)
        {
            File.WriteAllText(_startId, id.ToString());
        }

        public void Generate()
        {
            id = ReadIdFromFile();
            // Create a connection to the SQLite database
            using (SQLiteConnection connection = new SQLiteConnection($"Data Source={_db};Version=3;"))
            {
                connection.Open();

                for (int i = 1; i <= count; i++)
                {
                    // Insert data into a table
                    string insertQuery = "INSERT INTO cases (caseid, name, memo, owner, date) " +
                        "VALUES (@Value1, @Value2, @Value3, @Value4, @Value5)";

                    using (SQLiteCommand insertCommand = new SQLiteCommand(insertQuery, connection))
                    {
                        insertCommand.Parameters.AddWithValue("@Value1", id);
                        insertCommand.Parameters.AddWithValue("@Value2", "Case " + id);
                        insertCommand.Parameters.AddWithValue("@Value3", "This is a test.");
                        insertCommand.Parameters.AddWithValue("@Value4", _owner);
                        insertCommand.Parameters.AddWithValue("@Value5", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                        // Execute the INSERT query
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

                    id++; // Increment the id

                    // Write the updated id back to the file
                    WriteIdToFile(id);
                }

                connection.Close();
            }
        }
    }
}
