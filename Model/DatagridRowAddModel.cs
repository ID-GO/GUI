using System.Data;
using System.Threading.Tasks;
using GUI.ViewModel;

namespace GUI.Model
{
    public class DatagridRowAddModel
    {
        public static long TotalRows = 100000;
        public void DatagridRowAdd()
        {

            ExportToExcel.Model.Dt = new DataTable();
            ExportToExcel.Model.Dt.Columns.Add("Dosage", typeof(string));
            ExportToExcel.Model.Dt.Columns.Add("Drug", typeof(string));
            ExportToExcel.Model.Dt.Columns.Add("Patient", typeof(string));
            ExportToExcel.Model.Dt.Columns.Add("Date", typeof(string));

            Parallel.For((long) 0, TotalRows, i =>
            {
                lock (ExportToExcel.Model.Dt.Rows)
                {
                    ExportToExcel.Model.Dt.Rows.Add();
                }
            });
        }

        public string Cleardatagrid()
        {
            ExportToExcel.Model.Dt.Columns.Clear();
            ExportToExcel.Model.Dt.Columns.Add("Dosage", typeof(string));
            ExportToExcel.Model.Dt.Columns.Add("Drug", typeof(string));
            ExportToExcel.Model.Dt.Columns.Add("Patient", typeof(string));
            ExportToExcel.Model.Dt.Columns.Add("Date", typeof(string));
            var cleardata = Searchboxtext(string.Empty);
            return cleardata;
        }


        public string Searchboxtext(object obj)
        {
            var mm = obj.ToString();
            if (mm != "")
            {
                ExportToExcel.Model.Dt.DefaultView.RowFilter = "(Dosage like '" + mm + "*')";
            }
            else
            {
                ExportToExcel.Model.Dt.DefaultView.RowFilter = string.Empty;
                return "Cleartextinsearchbox";
            }

            return "";
        }
    }
}