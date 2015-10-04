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
        private readonly int _originalLength;
        private int _completed;

        public Manager(IEnumerable<TestItem> items, int treshold)
        {
            _items = new List<TestItem>(items);
            _originalLength = _items.Count;
            _random = new Random();
            _treshold = treshold;
            _completed = 0;
        }

        public TestItem Get()
        {
            var index = 0;
            if (_currentItem != null)
            {
                _items.Add(_currentItem);
                _currentItem = null;
            }
            if (_items.Count > 1)
            {
                index = _random.Next(0, _items.Count);
            }

            _currentItem = _items[index];
            _items.Remove(_currentItem);

            return _currentItem;
        }

        public string Progress => $"{_originalLength}/{_completed}";

        public CheckResult Check(string input)
        {
            CheckResult result;
            if (string.IsNullOrWhiteSpace(input))
            {
                result = CheckResult.Incorrect;
            }
            else
            {
                var distance = LevenshteinDistance.Compute(_currentItem.Translation, input);
                var longer = input.Length;
                if (_currentItem.Translation.Length > longer)
                {
                    longer = _currentItem.Translation.Length;
                }

                var per = Math.Floor(100 - (distance/(float) longer*100));

                result = per >= _treshold ? CheckResult.Correct : CheckResult.Incorrect;
            }

            if (result == CheckResult.Correct)
            {
                _completed++;
                _currentItem = null;
            }

            if (result == CheckResult.Correct && _items.Count == 0)
            {
                result = CheckResult.Done;
            }

            return result;
        }
    }
}
