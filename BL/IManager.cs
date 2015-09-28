using WordsTest.Model;

namespace WordTest.Manager
{
    public interface IManager
    {
        TestItem Get();

        CheckResult Check(string input);
    }
}
