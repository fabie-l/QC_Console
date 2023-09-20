using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QC_Console
{
    public class Cases
    {
        private long _id;
        private readonly string _owner;
        private const string tableName = "cases";
        private readonly string db = Directory.GetCurrentDirectory() + "\\copy\\data.qda";

        public Cases (long id, string owner)
        {
            _id = id;
            _owner = owner;
        }

        private string QuoteValue(object value)
        {
            if (value == null)
            {
                return "NULL";
            }
            else if (value is string || value is DateTime)
            {
                return $"'{value}'";
            }
            else
            {
                return value?.ToString() ?? "NULL";
            }
        }

        public void Generate (SQLiteConnection connection)
        {
            Dictionary<string, object> _case = new Dictionary<string, object>()
            {
                {"caseid", _id},
                {"name", "Case " + _id},
                {"memo", "This is a test."},
                {"owner", _owner},
                {"date", DateTime.Now.ToString("yyyy-MM-d HH:mm:ss")}
            };

            // Create the INSERT query dynamically based on the table and column names
            string columns = string.Join(", ", _case.Keys);
            string values = string.Join(", ", _case.Values.Select(value => QuoteValue(value)));

            string insertQuery = $"INSERT INTO {tableName} ({columns}) VALUES ({values})";

            try
            {
                CSQLite.InsertData(connection, insertQuery);
                _id += 10000;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            
        }

        

    }
}
