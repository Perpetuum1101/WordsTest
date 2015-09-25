using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TestModel;
using WordTest.Manager;

namespace WordTest.Tests
{
    
    [TestFixture]
    public class TestManagerTest
    {
        private IList<TestItem> _list;
        private IManager _manager;

        private static string CorrectInputByWord(string word)
        {
            switch (word)
            {
                case "A":
                    return "aax";
                case "B":
                    return "bbx";
                case "C":
                    return "ccx";
            }

            return string.Empty;
        }

        private static string IncorrectInputByWord(string word)
        {
            switch (word)
            {
                case "A":
                    return "axx";
                case "B":
                    return "bxx";
                case "C":
                    return "cxx";
            }

            return string.Empty;
        }

        [SetUp]
        public void SetUp()
        {
            _list = new List<TestItem>
            {
                new TestItem {Word = "A", Translation = "aaa"},
                new TestItem {Word = "B", Translation = "bbb"},
                new TestItem {Word = "C", Translation = "ccc"}
            };

            _manager = new Manager.Manager(_list, 65);
        }

        [TearDown]
        public void TearDown()
        {
            _list = null;
            _manager = null;
        }

        [TestCase(TestName = "Test manager gets random item from collection")]
        public void GetItemFromCollection()
        {
            var originalCount = _list.Count;
            var item = _manager.Get();
            originalCount.ShouldBeEquivalentTo(_list.Count+1);
            _list.FirstOrDefault(l => l.Equals(item)).Should().BeNull();
        }

        [TestCase(CheckResult.Incorrect, TestName = "Manager returns 'Incorrect' state if  user input corectness doesn't exceed treshold")]
        [TestCase(CheckResult.Correct, TestName = "Manager returns 'Correct' state if  user input corectness does exceed treshold")]
        public void CheckUserImput(CheckResult shoulBe)
        {
            var item = _manager.Get();
            var result = _manager.Check(shoulBe == CheckResult.Correct ? CorrectInputByWord(item.Word) : IncorrectInputByWord(item.Word));
            result.Should().Be(shoulBe);
        }

        [TestCase(TestName = "Incorrectly guessed item is returned to the list by the next pull")]
        public void IncorrectItemReturnedByNextPull()
        {
            var item = _manager.Get();
            _manager.Check(IncorrectInputByWord(item.Word));
            var newItem = _manager.Get();
            Assert.True(item.Equals(newItem) || _list.Contains(item));
        }

        [TestCase(TestName = "Last incorrectly guessed item is pulled again in next pull")]
        public void LastIncorrectItem()
        {
            _list.RemoveAt(0);
            _list.RemoveAt(1);

            var item = _manager.Get();
            _manager.Check(IncorrectInputByWord(item.Word));
            var item2 = _manager.Get();
            Assert.True(item.Equals(item2));
        }

        [TestCase(TestName = "If last item guessed correctly Manager returns 'Done' state")]
        public void FinalState()
        {
            _list.RemoveAt(0);
            _list.RemoveAt(1);

            var item = _manager.Get();
            var state = _manager.Check(CorrectInputByWord(item.Word));

            state.ShouldBeEquivalentTo(CheckResult.Done);
        }
    }
}
