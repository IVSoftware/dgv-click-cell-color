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
            Recordset.ListChanged += onRecordsetChanged;
            dataGridView.CellClick += onCellClick;
            for (int i = 0; i < 3; i++)
            {
                Recordset.Add(new Record());
            }
            unselectCurrentCell();
        }

        BindingList<Record> Recordset = new BindingList<Record>();

        private void onCellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if(dataGridView.Columns[nameof(Color)].Index == e.ColumnIndex &&  e.RowIndex != -1)
            {
                var record = Recordset[e.RowIndex];
                switch (record.Color.Name) 
                {
                    case nameof(Color.LightGreen):
                        record.Color = Color.LightSalmon;
                        break;
                    case nameof(Color.LightSalmon):
                        record.Color = Color.LightBlue;
                        break;
                    case nameof(Color.LightBlue):
                        record.Color = Color.LightGreen;
                        break;
                }
                unselectCurrentCell();
            }
        }

        private void onRecordsetChanged(object? sender, ListChangedEventArgs e)
        {
            DataGridViewCell cell;
            switch (e.ListChangedType)
            {
                case ListChangedType.ItemAdded:
                case ListChangedType.ItemChanged:
                    cell = dataGridView[dataGridView.Columns[nameof(Color)].Index, e.NewIndex];
                    cell.Style.BackColor = Recordset[e.NewIndex].Color;
                    break;
            }
        }

        void unselectCurrentCell()
        {
            BeginInvoke((MethodInvoker)delegate { dataGridView.CurrentCell = null; });
        }
    }



    class Record : INotifyPropertyChanged
    {
        static int _id = 0;

        /// <summary>
        /// Make a bindable property e.g. "Color"
        /// </summary>
        public Color Color
        {
            get => _color;
            set
            {
                if (!Equals(_color, value))
                {
                    _color = value;
                    OnPropertyChanged();
                }
            }
        }
        Color _color = Color.LightGreen;

        public string Name { get; set; } = $"Record {_id++}";

        private void OnPropertyChanged([CallerMemberName]string caller = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}