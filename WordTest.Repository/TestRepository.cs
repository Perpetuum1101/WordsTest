using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SQLite.Net;
using SQLite.Net.Platform.WinRT;
using WordTest.Repository.Tabels;
using TestItem = WordsTest.Model.TestItem;

namespace WordTest.Repository
{
    public class TestRepository : ITestRepository
    {
        private readonly string _path;

        public TestRepository()
        {
            _path = Path.Combine(
                Windows.Storage.ApplicationData.Current.LocalFolder.Path,
                "db.WordTest");

            CreateTable<Test>();
            CreateTable<Tabels.TestItem>();
        }

        #region Public API

        public IList<string> GetTestsList()
        {
            using (var conn = new SQLiteConnection(new SQLitePlatformWinRT(), _path))
            {
                var result = conn.Table<Test>().Select(t => t.Name);

                return result.ToList();
            }
        }

        public IList<TestItem> GeTestItems(string testName)
        {
            var items = GetTestItems(testName);

            var result = items.Select(item => new TestItem
            {
                Translation = item.Translation,
                Word = item.Word
            }).ToList();

            return result;
        }

        public async Task SaveAsync(string testName, IList<TestItem> testItems)
        {
            await Task.Run(() => SaveTest(testName, testItems));
        }

        public void SaveTest(string testName, IList<TestItem> testItems)
        {
           
            if (string.IsNullOrWhiteSpace(testName))
            {
                throw new ArgumentNullException(nameof(testName));
            }

            if (testItems == null)
            {
                throw new ArgumentNullException(nameof(testItems));
            }

            using (var conn = new SQLiteConnection(new SQLitePlatformWinRT(), _path))
            {
                var oldTest = conn.Table<Test>().FirstOrDefault(t => t.Name == testName);
                int testId;

                if (oldTest?.Id != null)
                {
                    testId = oldTest.Id.Value;
                    DeleteTestItems(testId, conn);
                }
                else
                {
                    var test = new Test
                    {
                        Name = testName
                    };

                    conn.Insert(test);
                    var testTable = conn.Table<Test>().FirstOrDefault(t => t.Name == testName);
                    if (testTable?.Id == null)
                    {
                        throw new Exception("Test was not saved");
                    }

                    testId = testTable.Id.Value;
                }

                foreach (var item in testItems)
                {
                    var testItem = new Tabels.TestItem
                    {
                        Word = item.Word,
                        Translation = item.Translation,
                        TestId = testId
                    };

                   conn.Insert(testItem);
                }
               
            }
        }

        public void DeleteTest(string testName)
        {
            using (var conn = new SQLiteConnection(new SQLitePlatformWinRT(), _path))
            {
                var testId = GetTestId(testName, conn);
                if (!testId.HasValue)
                {
                    throw new ArgumentNullException(nameof(testId));
                }

                DeleteTestItems(testId.Value, conn);
                conn.Delete<Test>(testId);
            }
        }

        #endregion

        #region Internal Methods

        private void CreateTable<T>()
        {
            using (var conn = new SQLiteConnection(new SQLitePlatformWinRT(), _path))
            {
                var info = conn.GetTableInfo(nameof(T));
                if (info == null || info.Count == 0)
                {
                    conn.CreateTable<T>();
                }
            }
        }

        private IEnumerable<Tabels.TestItem> GetTestItems(string testName)
        {
            using (var conn = new SQLiteConnection(new SQLitePlatformWinRT(), _path))
            {
                var testId = GetTestId(testName, conn);
                if (!testId.HasValue)
                {
                    throw new ArgumentNullException(nameof(testId));
                }

                return GetTestItems(testId.Value, conn);
            }
        }

        private int? GetTestId(string testName, SQLiteConnection conn)
        {
            if (testName == null)
            {
                throw new ArgumentNullException(nameof(testName));
            }

            var id = conn.Table<Test>()
                    .Where(t => t.Name == testName)
                    .Select(t => t.Id)
                    .FirstOrDefault();

            return id;
        }

        private IEnumerable<Tabels.TestItem> GetTestItems(int testId, SQLiteConnection conn)
        {
            var items = conn.Table<Tabels.TestItem>().Where(ti => ti.TestId == testId).ToList();

            return items;
        }

        private void DeleteTestItems(int testId, SQLiteConnection conn)
        {
            var items = GetTestItems(testId, conn);

            foreach (var item in items)
            {
                conn.Delete<Tabels.TestItem>(item.Id);
            }
        }

        #endregion
    }
}
