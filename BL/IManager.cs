using WordsTest.Model;

namespace WordTest.Manager
{
    public interface IManager
    {
        string Progress { get; }

        int Completed { get; }

        int OriginalLength { get; }

        TestItem Get();

        CheckResult Check(string input);
    }
}
