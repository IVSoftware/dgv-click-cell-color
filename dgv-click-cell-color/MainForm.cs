using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace dgv_click_cell_color
{
    public partial class MainForm : Form
    {
        public MainForm() => InitializeComponent();
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            dataGridView.DataSource = Recordset;
            dataGridView.Columns[nameof(Record.Name)].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView.Columns[nameof(Record.Color)].ReadOnly = true;
            dataGridView.CellMouseDown += onCellMouseDown;
            dataGridView.CellMouseUp += onCellMouseUp;
            for (int i = 0; i < 3; i++)
            {
                Recordset.Add(new Record());
                var cell = dataGridView[dataGridView.Columns[nameof(Color)].Index, i];
                Recordset[i].Color =
                    cell.Style.BackColor =
                    Color.LightGreen;
            }
            unselectCurrentCell();
        }

        BindingList<Record> Recordset = new BindingList<Record>();

        private void onCellMouseDown(object? sender, DataGridViewCellMouseEventArgs e)
        {
            if(dataGridView.Columns[nameof(Color)].Index == e.ColumnIndex &&  e.RowIndex != -1)
            {
                Color newColor;
                var cell = dataGridView[e.ColumnIndex, e.RowIndex];
                switch (cell.Style.BackColor.Name) 
                {
                    case nameof(Color.LightGreen): newColor = Color.LightSalmon; break;
                    case nameof(Color.LightSalmon): newColor = Color.LightBlue; break;
                    case nameof(Color.LightBlue):
                    default:
                        newColor = Color.LightGreen;
                        break;

                }
                Recordset[e.RowIndex].Color = 
                    cell.Style.BackColor = newColor;

                const int OFFSET = 40;
                cell.Style.SelectionBackColor =
                    Color.FromArgb(
                        Math.Min(255, newColor.R + OFFSET),
                        Math.Min(255, newColor.G + OFFSET),
                        Math.Min(255, newColor.B + OFFSET)
                );
                dataGridView.Refresh();
            }
        }

        private void onCellMouseUp(object? sender, DataGridViewCellMouseEventArgs e)
        {
            if (dataGridView.Columns[nameof(Color)].Index == e.ColumnIndex && e.RowIndex != -1)
            {
                var cell = dataGridView[e.ColumnIndex, e.RowIndex];

                cell.Style.SelectionBackColor =
                    cell.Style.BackColor;
                dataGridView.Refresh();
            }
        }

        void unselectCurrentCell()
        {
            BeginInvoke((MethodInvoker)delegate { dataGridView.CurrentCell = null; });
        }
    }

    class DataGridViewEx : DataGridView
    {
        //protected override void SetSelectedCellCore(int columnIndex, int rowIndex, bool selected)
        //{
        //    base
        //        .SetSelectedCellCore(
        //            columnIndex, 
        //            rowIndex, 
        //            selected && !(Columns[nameof(Record.Color)].Index == columnIndex));
        //}
    }

    class Record
    {
        static int _id = 0;
        public Color Color { get; set; }

        public string Name { get; set; } = $"Record {_id++}";
    }
}