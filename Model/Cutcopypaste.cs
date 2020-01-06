using System;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using GUI.ViewModel;
using Clipboard = System.Windows.Clipboard;
using DataGrid = System.Windows.Controls.DataGrid;
using MessageBox = System.Windows.MessageBox;
using TextDataFormat = System.Windows.TextDataFormat;

namespace GUI.Model
{
    internal class Cutcopypaste
    {

        public async void DatagridPaste(object sender)
        {
            var dgg = sender as DataGrid;
            var rawData = "";
            var reportTask = Task.Factory.StartNew(
                () => { rawData = GetClipboardRawData().TrimEnd('\r', '\n'); }
                , CancellationToken.None
                , TaskCreationOptions.AttachedToParent
                , TaskScheduler.FromCurrentSynchronizationContext()
            );

            await reportTask;


            if (string.IsNullOrEmpty(rawData)) throw new Exception("The clipboard is empty");
            var width = GetWidth1(rawData);
            var columnStart = dgg?.CurrentColumn.DisplayIndex;
            var rowStart = dgg?.Items.IndexOf(dgg.CurrentItem);
            var data = rawData.Split((char) Keys.Tab, '\n');
            var colPtr = columnStart;
            var rowPtr = rowStart;
            await Task.Run(() => NewMethod(0, data, rowPtr, colPtr, columnStart, width));
        }


        private static void NewMethod(int dataPtr, string[] data, int? rowPtr, int? colPtr, int? columnStart, int width)
        {
            try
            {
                while (dataPtr < data.Length)
                {
                    if (rowPtr != null)
                    {
                        ExportToExcel.Model.Dt.Rows[(int) rowPtr].SetField((int) colPtr, data[dataPtr].Trim());
                        if (colPtr == columnStart + width - 1)
                        {
                            colPtr = columnStart;
                            rowPtr += 1;
                        }
                        else
                        {
                            colPtr += 1;
                        }
                    }

                    dataPtr += 1;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }


            ExportToExcel.Model.Dt.AcceptChanges();
        }

        public void DatagridCut(object sender)
        {
            var dgg = sender as DataGrid;
            try
            {
                if (dgg == null) return;
                dgg.ClipboardCopyMode = DataGridClipboardCopyMode.ExcludeHeader;
                ApplicationCommands.Copy.Execute(null, null);
                DatagridCellBlank(dgg);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private string GetClipboardRawData()
        {
            return Clipboard.GetText(TextDataFormat.Text);
        }

        private int GetWidth1(string raw)
        {
            try
            {
                var lg = raw.IndexOf('\n');
                var ln = raw.Substring(0, lg);
                return ln.Count(c => c == (char) Keys.Tab) + 1;
            }
            catch (Exception)
            {
                // ignored
            }

            return 0;
        }

        public void DatagridDelete(object sender)
        {
            var dgg = sender as DataGrid;
            try
            {
                DatagridCellBlank(dgg);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DatagridCellBlank(DataGrid dgg)
        {
            var rownumber = 0;
            var colNumber = 0;
            FieldInfo fieldInfo;
            if (dgg != null)
                foreach (var variable in dgg.SelectedCells)
                    try
                    {
                        fieldInfo = ((DataRowView) variable.Item).Row.GetType()
                            .GetField("_rowID", BindingFlags.NonPublic | BindingFlags.Instance);
                        rownumber = Convert.ToInt32(fieldInfo?.GetValue(((DataRowView) variable.Item).Row));
                        colNumber = variable.Column.DisplayIndex;
                        ExportToExcel.Model.Dt.Rows[rownumber - 1][colNumber] = "";
                    }
                    catch (Exception)
                    {
                        // ignored
                    }

            ExportToExcel.Model.Dt.AcceptChanges();
            dgg.CurrentCell = new DataGridCellInfo(dgg.Items[rownumber - 1], dgg.Columns[colNumber]);
            dgg.SelectedCells.Add(dgg.CurrentCell);
        }
    }
}