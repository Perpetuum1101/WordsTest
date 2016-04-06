using System;
using SQLite.Net.Attributes;

namespace WordTest.Repository.Tabels
{
    public class TestItem
    {
        [PrimaryKey]
        [AutoIncrement]
        public int? Id { get; set; }

        public string Word { get; set; }

        public string Translation { get; set; }

        public int? TestId { get; set; }

        public DateTime Sync { get; set; }

        public bool Deleted { get; set; }
    }
}
