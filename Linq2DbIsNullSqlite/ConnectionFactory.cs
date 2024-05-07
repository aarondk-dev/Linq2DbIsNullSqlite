using LinqToDB.Data;
using LinqToDB;
using System.Data.SQLite;
using System.Threading.Tasks;

namespace Linq2DbIsNullSqlite
{
    public static class ConnectionFactory
    {
        public static async Task CreateTables(string connectionString)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                var cmdText = @"CREATE TABLE A (
                C1A DECIMAL(14,0) DEFAULT 0 NOT NULL,
                C2A DECIMAL(14,0) DEFAULT 0 NOT NULL
                )";
                var cmd = new SQLiteCommand(cmdText, connection);
                cmd.ExecuteNonQuery();

                cmdText = @"CREATE TABLE B (
                C1B DECIMAL(14,0) DEFAULT 0 NOT NULL,
                C2B DECIMAL(14,0) DEFAULT 0 NOT NULL
                )";
                var cmd2 = new SQLiteCommand(cmdText, connection);
                cmd2.ExecuteNonQuery();
            }

            using (var dc = new DataConnection(new DataOptions().UseSQLite(connectionString)))
            {
                _ = await dc.GetTable<TableA>()
                   .Value(i => i.C1A, 1)
                   .Value(i => i.C2A, 2)
                   .InsertAsync();

                _ = await dc.GetTable<TableA>()
                   .Value(i => i.C1A, 3)
                   .Value(i => i.C2A, 4)
                   .InsertAsync();

                _ = await dc.GetTable<TableB>()
                   .Value(i => i.C1B, 2)
                   .Value(i => i.C2B, 100)
                   .InsertAsync();

                _ = await dc.GetTable<TableB>()
                   .Value(i => i.C1B, 101)
                   .Value(i => i.C2B, 102)
                   .InsertAsync();
            }
        }
    }
}
