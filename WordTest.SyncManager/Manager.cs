using System.Collections.Generic;
using System.Linq;
using WordsTest.Model;

namespace WordTest.SyncManager
{
    public class Manager
    {
        public void SyncTestItemsIList(ref IList<TestItem> source, ref IList<TestItem> destination)
        {
            foreach (var item in source)
            {
                var dest = destination.SingleOrDefault(d => d.Id == item.Id);
                if (dest == null)
                {
                    destination.Add(item);
                }
                else
                {
                    if (dest.Sync < item.Sync)
                    {
                         destination[destination.IndexOf(dest)] = item.Clone();
                    }
                }
            }

            foreach (var item in destination)
            {
                var sour = source.SingleOrDefault(s => s.Id == item.Id);
                if (sour == null)
                {
                    source.Add(item);
                }
                else
                {
                    if (sour.Sync < item.Sync)
                    {
                        source[source.IndexOf(sour)] = item.Clone();
                    }
                }
            }
        }
    }
}
