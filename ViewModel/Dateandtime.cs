using System;
using System.Windows;
using System.Windows.Input;
using GUI.Model;
using GUI.View;

namespace GUI.ViewModel
{
    public class Dateandtime : Basepropertychangedevent
    {
        private ICommand _command;

        private ICommand _leftButtonDownCommand;
        private DateTime _startDate = DateTime.Now;

        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                _startDate = value;
                OnPropertyChanged(nameof(StartDate));
            }
        }

        public ICommand LeftMouseButtonDown => _leftButtonDownCommand ?? (_leftButtonDownCommand = new RelayCommand(
                                                   x => { DoStuff(); }));

        public ICommand Command
        {
            get
            {
                return _command ?? (_command = new RelayCommand(
                           x => { DoStuff1(); }));
            }
        }

        private void DoStuff1()
        {
            MessageBox.Show("Responding to button click event...");
        }

        private void DoStuff()
        {
            var dd = new SecondScreen();

            dd.ShowDialog();
        }
    }
}