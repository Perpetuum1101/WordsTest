using System;
using System.Collections.Generic;
using System.Linq;
using WordsTest.Model;

namespace WordTes.UI.Services
{
    public static class Validator
    {
        public const int MinimumItemsCount = 3;

        public static string Validate(IList<TestItem> items, string action)
        {
            if (items == null)
            {
                throw new ArgumentNullException();
            }

            if (items.Count() < MinimumItemsCount)
            {
                return string.Format(
                    "At least {0} items required to {1} the test!",
                    MinimumItemsCount,
                    action);
            }

            return null;
        }

        public static IList<TestItem> Correct(IList<TestItem> items)
        {
            var itemsToRemove =
                items.Where(
                    item =>
                        string.IsNullOrWhiteSpace(item.Word) ||
                        string.IsNullOrWhiteSpace(item.Translation)).ToList();

            var result = items.ToList();
            result.RemoveAll(x => itemsToRemove.Contains(x));

            return result;
        }
    }
}
