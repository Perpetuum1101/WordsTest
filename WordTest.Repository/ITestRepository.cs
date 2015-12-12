using System.Collections.Generic;
using System.Threading.Tasks;
using WordsTest.Model;

namespace WordTest.Repository
{
    public interface ITestRepository
    {
        IList<string> GetTestsList();

        IList<TestItem> GeTestItems(string test);

        Task SaveAsync(string testName, IList<TestItem> testItems);

        void SaveTest(string testName, IList<TestItem> items);

        void DeleteTest(string testName);
    }
}
