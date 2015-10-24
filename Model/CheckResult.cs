
namespace WordsTest.Model
{
    public enum CheckState
    {
        Correct,
        Incorrect,
        Done
    }

    public class CheckResult
    {
        public CheckState State { get; set; }

        public int Correctness { get; set; }

        public string CorrectAnswer { get; set; }
    }
}
