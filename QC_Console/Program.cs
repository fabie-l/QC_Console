using System.Data.SQLite;
using System.Security.Cryptography.X509Certificates;

namespace QC_Console
{
    internal class Program
    {

        static void Main(string[] args)
        {
            SQLiteConnection connection = CSQLite.OpenConnection(Directory.GetCurrentDirectory() + "\\copy\\data.qda");

            Cases cases = new Cases(902000001, "Raj");

            for (int i = 0; i < 100; i++)
            {
                cases.Generate(connection);
            }

            CSQLite.CloseConnection(connection);
        }
    }
}