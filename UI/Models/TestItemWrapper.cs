using System.ComponentModel;
using System.Runtime.CompilerServices;
using UI.Annotations;
using WordsTest.Model;

namespace WordTes.UI.Models
{
    public class TestItemWrapper : INotifyPropertyChanged
    {
        private TestItem _item;
        private bool _first;
        private bool _last;
        private bool _focus;

        public TestItemWrapper()
        {
            Last = false;
            NotFirst = true;
            Item = new TestItem();
        }

        public TestItem Item
        {
            get { return _item; }
            set
            {
                _item = value;
               OnPropertyChanged(nameof(Item));
            }
        }

        public bool Last
        {
            get { return _last; }
            set
            {
                _last = value;
                OnPropertyChanged(nameof(Last));
            }
        }

        public bool Focus
        {
            get { return _focus; }
            set
            {
                _focus = value;
                OnPropertyChanged(nameof(Focus));
            }
        }

        public bool NotFirst
        {
            get { return _first; }
            set
            {
                _first = value;
                OnPropertyChanged(nameof(NotFirst));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
