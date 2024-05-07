using LinqToDB;
using LinqToDB.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Linq2DbIsNullSqlite
{
    internal class Program
    {
        static string connectionString = "FullUri=file::memory:?cache=shared";
        static async Task Main(string[] args)
        {
            await ConnectionFactory.CreateTables(connectionString);

            //LeftJoin
            using (var dc = new DataConnection(new DataOptions().UseSQLite(connectionString)))
            {
                var query = dc.GetTable<TableA>()
                    .LeftJoin(dc.GetTable<TableB>(),
                        (JOIN, B) => JOIN.C2A == B.C1B,
                        (JOIN, B) => new { JOIN, B })
                    .Where(i => i.B.C1B != 0);

                var results = await query.ToListAsync();
                await Console.Out.WriteLineAsync($"Manual join gives {results.Count} results");
            }

            //LoadWith
            using (var dc = new DataConnection(new DataOptions().UseSQLite(connectionString)))
            {
                var query = dc.GetTable<TableA>()
                    .LoadWith(i => i.TableBJoin)
                    .Where(i => i.TableBJoin.C1B != 0);


                var results = await query.ToListAsync();
                await Console.Out.WriteLineAsync($"LoadWith gives {results.Count} results");
            }

            //I would expect the same results here, but the where clauses differ
            //WHERE [B_1].[C1B] <> 0
            //WHERE ([a_TableBJoin].[C1B] <> 0 OR[a_TableBJoin].[C1B] IS NULL)
            Console.ReadLine();
        }
    }
}