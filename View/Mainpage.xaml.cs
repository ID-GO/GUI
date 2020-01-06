using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using GUI.ViewModel;

namespace GUI.View
{
    /// <summary>
    /// Interaction logic for Mainpage.xaml
    /// </summary>
    public partial class Mainpage : Window
    {
        public Mainpage()
        {
            InitializeComponent();
            this.DataContext = new Dateandtime();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //var transparancyConverter = new TransparancyConverter(this);
            //transparancyConverter.MakeTransparent();
        }

     
    }
}

