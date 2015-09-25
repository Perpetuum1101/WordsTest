using TestModel;

namespace WordTest.Manager
{
    public interface IManager
    {
        TestItem Get();

        CheckResult Check(string input);
    }
}
