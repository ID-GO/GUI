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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GUI.View
{
    /// <summary>
    /// Interaction logic for SecondScreen2.xaml
    /// </summary>
    public partial class SecondScreen2 : UserControl
    {

        private static volatile SecondScreen2 _instance;
        private static readonly object SyncRoot = new object();

        public static SecondScreen2 InstanceSecondScreen2
        {
            get 
            {
                if (_instance != null) return _instance;
                lock (SyncRoot) 
                {
                    if (_instance == null) 
                        _instance = new SecondScreen2();
                }

                return _instance;
            }
        }


        public SecondScreen2()
        {
            InitializeComponent();
        }
    }
}
