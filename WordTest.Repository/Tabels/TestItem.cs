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
    }
}
