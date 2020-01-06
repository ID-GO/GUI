using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;
using GUI.Model;

namespace GUI.ViewModel
{
    public class DtDv : Basepropertychangedevent
    {
      
        public DtDv()
        {
            SearchButtonCommand1 = new RelayCommand(Searchboxtext);
           
            MyViewModelSourceList = new List<DataGridCellInfo>();
           
        }

        public ICommand SearchButtonCommand1 { get; set; }
        
        //source property:
        public static ObservableCollection<object> SelectedItems { get; set; } = new ObservableCollection<object>();

        public IList<DataGridCellInfo> MyViewModelSourceList { get; set; }

       private DataTable _datatableproperty;
        public DataTable Dt
        {
            get => _datatableproperty;
            set
            {
                if (_datatableproperty == value) return;
                _datatableproperty = value;

                OnPropertyChanged();
            }
        }


        private void Searchboxtext(object obj)
        {
        }
    }
  
    public class DataGridSelectedItemsBlendBehavior : Behavior<DataGrid>
    {
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItems", typeof(ObservableCollection<object>),
                typeof(DataGridSelectedItemsBlendBehavior),
                new FrameworkPropertyMetadata(null)
                {
                    BindsTwoWayByDefault = true
                });

        public ObservableCollection<object> SelectedItems
        {
            get => (ObservableCollection<object>) GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.SelectionChanged += OnSelectionChanged;
            AssociatedObject.Loaded += AssociatedObject_Loaded;
        }

        private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            if (SelectedItems == null) return;
            var selectedItems = SelectedItems.ToList();
            foreach (var obj in selectedItems)
                if (AssociatedObject.ItemContainerGenerator.ContainerFromItem(obj) is DataGridRow rowContainer)
                    rowContainer.IsSelected = true;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            if (AssociatedObject == null) return;
            AssociatedObject.SelectionChanged -= OnSelectionChanged;
            AssociatedObject.Loaded -= AssociatedObject_Loaded;
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems != null && e.AddedItems.Count > 0 && SelectedItems != null)
                foreach (var obj in e.AddedItems)
                    SelectedItems.Add(obj);

            if (e.RemovedItems == null || e.RemovedItems.Count <= 0 || SelectedItems == null) return;
            {
                foreach (var obj in e.RemovedItems)
                    SelectedItems.Remove(obj);
            }
        }
    }
    public class DataGridHelper
    {
        #region SelectedCells

        public static IList<DataGridCellInfo> GetSelectedCells(DependencyObject obj)
        {
            return (IList<DataGridCellInfo>) obj.GetValue(SelectedCellsProperty);
        }

        public static void SetSelectedCells(DependencyObject obj, IList<DataGridCellInfo> value)
        {
            obj.SetValue(SelectedCellsProperty, value);
        }

        public static readonly DependencyProperty SelectedCellsProperty =
            DependencyProperty.RegisterAttached("SelectedCells", typeof(IList<DataGridCellInfo>),
                typeof(DataGridHelper), new UIPropertyMetadata(null, OnSelectedCellsChanged));

        private static SelectedCellsChangedEventHandler GetSelectionChangedHandler(DependencyObject obj)
        {
            return (SelectedCellsChangedEventHandler) obj.GetValue(SelectionChangedHandlerProperty);
        }

        private static void SetSelectionChangedHandler(DependencyObject obj, SelectedCellsChangedEventHandler value)
        {
            obj.SetValue(SelectionChangedHandlerProperty, value);
        }

        private static readonly DependencyProperty SelectionChangedHandlerProperty =
            DependencyProperty.RegisterAttached("SelectedCellsChangedEventHandler",
                typeof(SelectedCellsChangedEventHandler), typeof(DataGridHelper), new UIPropertyMetadata(null));

        //d is MultiSelector (d as ListBox not supported)
        private static void OnSelectedCellsChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            DataGrid mm = (DataGrid) d;
           
            
            if (GetSelectionChangedHandler(d) != null)
                return;

            if (!(d is DataGrid datagrid)) return;
            foreach (var selected in GetSelectedCells(datagrid))
                datagrid.SelectedCells.Add(selected);

            void Selectionchanged(object sender, SelectedCellsChangedEventArgs e)
            {
                SetSelectedCells(datagrid, datagrid.SelectedCells);
            }

            SetSelectionChangedHandler(datagrid, Selectionchanged);
            datagrid.SelectedCellsChanged += GetSelectionChangedHandler(datagrid);
        }

        public static T FindParent<T>(FrameworkElement element) where T : FrameworkElement
        {
            var parent = LogicalTreeHelper.GetParent(element) as FrameworkElement;
            //parent.FindName
            while (parent != null)
            {
                if (parent is T correctlyTyped)
                    return correctlyTyped;
                return FindParent<T>(parent);
            }

            return null;
        }

        #endregion
    }
}