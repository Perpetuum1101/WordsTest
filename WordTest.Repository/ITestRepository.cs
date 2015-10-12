using System.Collections.Generic;
using WordsTest.Model;

namespace WordTest.Repository
{
    public interface ITestRepository
    {
        IList<string> GetTestsList();

        IList<TestItem> GeTestItems(string test);

        void SaveTest(string testName, IList<TestItem> items);

        void DeleteTest(string testName);
    }
}
