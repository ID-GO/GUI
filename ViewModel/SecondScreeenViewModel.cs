using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GUI.Model;

namespace GUI.ViewModel
{
    public class SecondScreeenViewModel : Basepropertychangedevent
    {
        private bool _controlEnabled = true;

        private string _filterText;

        private Visibility _gridvisibility = Visibility.Collapsed;

        private bool _selectedhandbuger;

        private string _selectedindex;

        private string _selectedItem;


        public SecondScreeenViewModel()
        {
            InitializeViewModelCommand = new RelayCommand(x => DoStuff1());
            ItemSelectedCommand = new RelayCommand(x => DoStuff());
            SelectedItemChangedCommand = new RelayCommand(x => DoStuff());
            SelectedItemListview = new RelayCommand(DoStuff3);
            Cleardata = new RelayCommand(o =>
            {
                ControlEnabled = false;
                Gridvisibility = Visibility.Visible;
                var cleartext = new DatagridRowAddModel().Cleardatagrid();
                if (cleartext == "Cleartextinsearchbox") FilterText = "";
                ControlEnabled = true;
                Gridvisibility = Visibility.Collapsed;
            }, o => true);
            Exportdata = new RelayCommand(x => { Export(); });
            Importdata = new RelayCommand(x => { Import(); });
            MultiThreadingSplitter = new RelayCommand(x => MultiThreadingSplitterMethod());
            Hangburger = new RelayCommand(x => DoStuff());
            ButtonCommand = new RelayCommand(ShowMessage);
            SearchButtonCommand = new RelayCommand(o => new DatagridRowAddModel().Searchboxtext(o), o => true);
            Checkboxclickeffect = new RelayCommand(Checkboxclickbg);
        }

        private async void Export()
        {
            ControlEnabled = false;
            Gridvisibility = Visibility.Visible;

            await new ExportToExcel().Exportdatagrid();
            await Task.Delay(10);
            ControlEnabled = true;
            Gridvisibility = Visibility.Collapsed;
        }

        private async void Import()
        {
            var test = new ExportToExcel();
            ControlEnabled = false;
            Gridvisibility = Visibility.Visible;
             await test.Importdatagrid();
            await Task.Delay(10);
            ControlEnabled = true;
             Gridvisibility = Visibility.Collapsed;
        }

        public Visibility Gridvisibility
        {
            get => _gridvisibility;
            set
            {
                _gridvisibility = value;
                OnPropertyChanged(nameof(Gridvisibility));
            }
        }

        public bool ControlEnabled
        {
            get => _controlEnabled;
            set
            {
                _controlEnabled = value;
                OnPropertyChanged(nameof(ControlEnabled));
            }
        }

        public string FilterText
        {
            get => _filterText;
            set
            {
                _filterText = value;
                new DatagridRowAddModel().Searchboxtext(_filterText);
                OnPropertyChanged(nameof(FilterText));
            }
        }

        public ICommand Cleardata { get; set; }
        public ICommand Exportdata { get; set; }
        public ICommand Importdata { get; set; }
        public ICommand MultiThreadingSplitter { get; set; }

        public string SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                Selectedhandbuger = false;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        public bool Selectedhandbuger
        {
            get => _selectedhandbuger;
            set
            {
                _selectedhandbuger = value;
                OnPropertyChanged();
            }
        }

        public string Selectedindex
        {
            get => _selectedindex;
            set
            {
                _selectedindex = value;
                OnPropertyChanged(nameof(Selectedindex));
            }
        }

        public ICommand SearchButtonCommand { get; set; }
        public ICommand ButtonCommand { get; set; }
        public ICommand InitializeViewModelCommand { get; set; }
        public ICommand ItemSelectedCommand { get; set; }

        public ICommand SelectedItemChangedCommand { get; set; }

        public ICommand SelectedItemListview { get; set; }

        public ICommand Checkboxclickeffect { get; set; }

        public ICommand Hangburger { get; set; }

        private async void MultiThreadingSplitterMethod()
        {
            ControlEnabled = false;
            Gridvisibility = Visibility.Visible;
             await Task.Delay(3000);
            var test = Split(DtDv.SelectedItems.ToList(), 2);
            foreach (var variable in test)
            foreach (DataRowView selectedItem in variable)
            {
                var tmp = selectedItem;

                var fieldInfo = selectedItem.Row.GetType()
                    .GetField("ObjectID", BindingFlags.NonPublic | BindingFlags.Instance);
                var rownumber = Convert.ToInt32(fieldInfo?.GetValue(selectedItem.Row));
                await Task.Factory.StartNew(() => MyMethod(tmp));
                Debug.Print(selectedItem.Row.ItemArray[0].ToString());
                ControlEnabled = true;
                Gridvisibility = Visibility.Collapsed;
                }
        }

        public void MyMethod(DataRowView tmp)
        {
            Thread.Sleep(5000);
            Debug.Print(tmp.Row.ItemArray[0].ToString());
        }


        public void Checkboxclickbg(object obj)
        {
        }


        private void ShowMessage(object obj)
        {
            Selectedhandbuger = false;
        }

        private void DoStuff1()
        {
            Selectedhandbuger = false;
        }

        private void DoStuff()
        {
            Selectedhandbuger = !Selectedhandbuger;
        }

        private void DoStuff3(object obj)
        {
            Selectedhandbuger = false;
        }


        public List<List<T>> Split<T>(List<T> collection, int size)
        {
            var chunks = new List<List<T>>();
            var chunkCount = (int) Math.Ceiling((double) collection.Count / size);
            if (collection.Count % size > 0)
                chunkCount++;
            for (var i = 0; i < size; i++)
                chunks.Add(collection.Skip(i * chunkCount).Take(chunkCount).ToList());
            return chunks;
        }
    }
}