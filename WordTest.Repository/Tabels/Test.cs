using System;
using SQLite.Net.Attributes;

namespace WordTest.Repository.Tabels
{
    public class Test
    {
        [PrimaryKey]
        [AutoIncrement]
        public int? Id { get; set; }

        [Unique]
        public string Name { get; set; }

        public DateTime? TestDate { get; set; }
    }
}
