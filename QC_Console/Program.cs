using System.Data.SQLite;
using System.Security.Cryptography.X509Certificates;

namespace QC_Console
{
    internal class Program
    {
        static void Main(string[] args)
        {

            SQLiteConnection connection = CSQLite.OpenConnection(Directory.GetCurrentDirectory() + "\\copy\\data.qda");

            Cases cases = new Cases(100000000000000, "Raj");

            for (int i = 0; i < 500; i++)
            {
                cases.Generate(connection);
            }

            CSQLite.CloseConnection(connection);

        }
    }
}