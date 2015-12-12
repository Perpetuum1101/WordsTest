using System;
using System.Collections.Generic;
using WordsTest.Model;

namespace WordTest.Manager
{
    public class Manager : IManager
    {
        private readonly IList<TestItem> _items;
        private readonly Random _random;
        private readonly int _treshold;
        private TestItem _currentItem;

        public Manager(IEnumerable<TestItem> items, int treshold)
        {
            _items = new List<TestItem>(items);
            OriginalLength = _items.Count;
            _random = new Random();
            _treshold = treshold;
            Completed = 0;
        }

        public TestItem Get()
        {
            var index = 0;
           
            if (_items.Count > 1)
            {
                index = _random.Next(0, _items.Count);
            }
            if (_currentItem != null)
            {
                _items.Add(_currentItem);
            }

            _currentItem = _items[index];
            _items.Remove(_currentItem);

            return _currentItem;
        }

        public string Progress => $"{Completed}/{OriginalLength}";

        public int Completed { get; private set; }

        public int OriginalLength { get; }

        public CheckResult Check(string input)
        {
            var result = new CheckResult();

            if (string.IsNullOrWhiteSpace(input))
            {
                result.State = CheckState.Incorrect;
            }
            else
            {
                var distance = LevenshteinDistance.Compute(_currentItem.Translation, input);
                var longer = input.Length;
                if (_currentItem.Translation.Length > longer)
                {
                    longer = _currentItem.Translation.Length;
                }

                result.Correctness = (int)Math.Floor(100 - (distance/(float) longer*100));

                result.State = result.Correctness >= _treshold
                    ? CheckState.Correct
                    : CheckState.Incorrect;
            }

            result.CorrectAnswer = _currentItem.Translation;

            if (result.State == CheckState.Correct)
            {
                Completed++;
                _currentItem = null;
            }

            if (result.State == CheckState.Correct && _items.Count == 0)
            {
                result.State = CheckState.Done;
            }

            return result;
        }
    }
}
