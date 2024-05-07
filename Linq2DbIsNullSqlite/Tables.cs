using LinqToDB.Mapping;

namespace Linq2DbIsNullSqlite
{
    [Table(Name = "A")]
    public class TableA
    {
        [PrimaryKey]
        public int C1A { get; set; }

        [Column]
        public int C2A { get; set; }

        [Association(
           ThisKey = "C2A",
           OtherKey = "C1B")]
        public TableB TableBJoin { get; set; }
    }

    [Table(Name = "B")]
    public class TableB
    {
        [PrimaryKey]
        public int C1B { get; set; }

        [Column]
        public int C2B { get; set; }
    }
}
