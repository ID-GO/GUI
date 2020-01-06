using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GUI.Model;
using GUI.ViewModel;

namespace GUI.View
{
    /// <summary>
    ///     Interaction logic for SecondScreen.xaml
    /// </summary>
    public partial class SecondScreen : Window
    {
        public SecondScreen()
        {
            InitializeComponent();
            DataContext = new SecondScreeenViewModel();
        }


        //private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    DragMove();
        //}


        private void ListViewMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var index = ListViewMenu.SelectedIndex;
            MoveCursorMenu(index);

            switch (index)
            {
                case 0:
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(new SecondScreen1());
                    break;
                case 1:
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(SecondScreen2.InstanceSecondScreen2);
                    break;
                default:
                    break;
            }
            
        }

        private void MoveCursorMenu(int index)
        {
            //TrainsitionigContentSlide.OnApplyTemplate();
            GridCursor.Margin = new Thickness(0, 100 + 60 * index, 0, 0);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //var transparancyConverter = new TransparancyConverter(this);
            //transparancyConverter.MakeTransparent();
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            ExportToExcel.Model.Dt.Rows[1][1] = "WWWWWWWWWWWWWWWWWW";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(ExportToExcel.Model.Dt.Rows[1][1].ToString());
        }
    }
}