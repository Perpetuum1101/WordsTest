using System.Collections.Generic;
using WordsTest.Model;

namespace WordTes.UI.Models
{
    public class TestSetupModel
    {
        public TestSetupModel()
        {
            Items = new List<TestItem>();
            CorrectnessRate = 75;
            TestName = "New Test";
        }
        
        public string TestName { get; set; }

        public int CorrectnessRate { get; set; }

        public IList<TestItem> Items { get; set; }
    }
}
