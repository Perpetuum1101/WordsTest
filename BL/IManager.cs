using WordsTest.Model;

namespace WordTest.Manager
{
    public interface IManager
    {
        string Progress { get; }

        TestItem Get();

        CheckResult Check(string input);
    }
}
