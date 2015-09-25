﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using TestModel;
using UI;
using UI.Annotations;

namespace WordTes.UI
{

    

    public class TestSetupPageModel : INotifyPropertyChanged
    {
        private ICommand _addCommand;
        private ICommand _removeCommand;
        private ICommand _startTestCommand;
        private readonly bool _canExecute;

        public TestSetupPageModel()
        {
            Items = new ObservableCollection<TestItem>
            {
                new TestItem()
            };

            _canExecute = true;
        }

        public ObservableCollection<TestItem> Items { get; set; }

        public ICommand AddCommand => _addCommand ?? (_addCommand = new CommandHandler(Add, _canExecute));
        public ICommand RemoveCommand => _removeCommand ?? (_removeCommand = new CommandHandler(Remove, _canExecute));
        public ICommand StartTestCommand => _startTestCommand ?? (_startTestCommand = new CommandHandler(StartTest, _canExecute));

        public void Add()
        {
            Items.Add(new TestItem());
        }

        public void Remove(TestItem item)
        {
            Items.Remove(item);
        }

        public void StartTest()
        {
            App.NavigationService.Navigate<Test>(Items);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
