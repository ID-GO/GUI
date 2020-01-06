using System.Windows.Controls;
using System.Windows.Input;
using GUI.Model;
using GUI.ViewModel;

namespace GUI.View
{
  

    /// <summary>
    ///     Interaction logic for SecondScreen1.xaml
    /// </summary>
    public partial class SecondScreen1
    {
     

        public DtDv Model { get; set; }

        public SecondScreen1()
        {
            InitializeComponent();
            Model = new DtDv();
          
            DataContext = Model;
            var exportToExcel = new ExportToExcel(Model);
            new DatagridRowAddModel().DatagridRowAdd();
        }

     

    }
}