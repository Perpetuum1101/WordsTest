using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using WordsTest.Model;
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

        [TestCase(CheckState.Incorrect, TestName = "Manager returns 'Incorrect' state if  user input corectness doesn't exceed treshold")]
        [TestCase(CheckState.Correct, TestName = "Manager returns 'Correct' state if  user input corectness does exceed treshold")]
        public void CheckUserImput(CheckState shoulBe)
        {
            var item = _manager.Get();
            var result =
                _manager.Check(shoulBe == CheckState.Correct
                    ? CorrectInputByWord(item.Word)
                    : IncorrectInputByWord(item.Word));
            result.State.Should().Be(shoulBe);
        }

        [TestCase(TestName = "Manager never returns same item two times in a row")]
        public void CheckSameItemReturn()
        {
            var shouldBe = true;
            for (var i = 0; i < 1000; i++)
            {
                var manager = new Manager.Manager(_list, 65);
                var item = manager.Get();
                manager.Check(IncorrectInputByWord(item.Word));
                var item2 = manager.Get();
                shouldBe = !item2.Equals(item);
                if (!shouldBe)
                {
                    break;
                }
            }

            shouldBe.Should().BeTrue();
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

            var manager = new Manager.Manager(_list, 65);
            var item = manager.Get();
            manager.Check(IncorrectInputByWord(item.Word));
            var item2 = manager.Get();

            Assert.True(item.Equals(item2));
        }

        [TestCase(TestName = "If last item guessed correctly Manager returns 'Done' state")]
        public void FinalState()
        {
            _list.RemoveAt(0);
            _list.RemoveAt(1);

            var  manager = new Manager.Manager(_list, 65);
            var item = manager.Get();
            var result = manager.Check(CorrectInputByWord(item.Word));

            result.State.ShouldBeEquivalentTo(CheckState.Done);
        }
    }
}
