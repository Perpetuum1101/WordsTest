using System;
using System.Runtime.Serialization;

namespace WordsTest.Model
{
    [DataContract]
    public class TestItem : IEquatable<TestItem>
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Word { get; set; }

        [DataMember]
        public string Translation { get; set; }

        [DataMember]
        public DateTime Sync { get; set; }

        [DataMember]
        public bool Deleted { get; set; }

        public bool Equals(TestItem other)
        {
            if (other == null)
            {
                return false;
            }

            return Word == other.Word && Translation == other.Translation;
        }

        public TestItem Clone()
        {
            return (TestItem) MemberwiseClone();
        }
    }
}
