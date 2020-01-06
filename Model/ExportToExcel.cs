using System;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Aspose.Cells;
using GUI.View;
using GUI.ViewModel;
using MessageBox = System.Windows.Forms.MessageBox;

namespace GUI.Model
{
    public class ExportToExcel
    {
        public static DtDv Model { get; set; }
        public ExportToExcel(DtDv data)
        {
            Model = data;
        }

     


        public async Task Exportdatagrid()
        {


            var workbook = new Workbook();

            var reportTask = Task.Factory.StartNew(
                () =>
                {
                    var worksheet = workbook.Worksheets[0];
                    var exportdatatable = Model.Dt;
                    try
                    {
                        exportdatatable = exportdatatable.Rows.Cast<DataRow>().Where(row =>
                            !row.ItemArray.All(field => field is DBNull || string.IsNullOrWhiteSpace(field as string))).CopyToDataTable();

                        worksheet.Cells.ImportDataTable(exportdatatable, true, "A1");
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }


                }
                , CancellationToken.None
                , TaskCreationOptions.RunContinuationsAsynchronously
                , TaskScheduler.Current
            );

            await reportTask;
            await Task.Run(() => workbook.Save(@"d:\test.xlsx"));


           
          
        }

        public async Task Importdatagrid()
        {
            var importtable = new DataTable();

            var reportTask = Task.Factory.StartNew(
                () =>
                {
                    try
                    {
                        var workbook = new Workbook(@"D:\test.xlsx");
                        var worksheet = workbook.Worksheets[0];
                        var cell = worksheet.Cells.LastCell;
                        importtable = worksheet.Cells.ExportDataTable(0, 0, cell.Row + 1, cell.Column + 1, true);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }
                   
                }
                , CancellationToken.None
                , TaskCreationOptions.RunContinuationsAsynchronously
                , TaskScheduler.Current
            );

            await reportTask;


            Model.Dt = importtable.Copy();

            var mm = DatagridRowAddModel.TotalRows - importtable.Rows.Count;
            //Parallel.For((long)0, mm+4, i =>
            //{
            //    lock (Model.Dt.Rows)
            //    {
            //        Model.Dt.Rows.Add();
            //    }
            //});

        }
    }
}