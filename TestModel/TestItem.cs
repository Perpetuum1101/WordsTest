using System;

namespace TestModel
{
    public class TestItem : IEquatable<TestItem>
    {
        public string Word { get; set; }

        public string Translation { get; set; }

        public bool Equals(TestItem other)
        {
            if (other == null)
            {
                return false;
            }

            return Word == other.Word && Translation == other.Translation;
        }
    }
}
