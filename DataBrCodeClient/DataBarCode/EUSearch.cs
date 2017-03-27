using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;
using GridSample;

namespace DataBarCode
{
    public partial class EUSearch : Form
    {
        public DataTable _tblEU;
        public List<CommonType.SelectEU> SelectList;

        public EUSearch()
        {
            InitializeComponent();
            
            this.KeyPreview = true;

            SelectList = new List<CommonType.SelectEU>();
            //dataGridEu.

            CreateColumn("УЕ", "УЕ", 180, 0);
            CreateColumn("Марка", "Марка", 150, 1);
            CreateColumn("Размер", "Размер", 150, 2);
            CreateColumn("Label", "Label", 180, 3);
            CreateColumn("Вес", "Вес", 120, 4);
            CreateColumn("S", "Select", 40, 5);

            InitTable();
            dataGridEu.GridLineStyle = DataGridLineStyle.Solid;
            dataGridEu.SelectionBackColor = Color.DeepSkyBlue;
        }

        public List<CommonType.SelectEU> GetSelectedEU()
        {
            return SelectList;
        }

        public void InitTable()
        {

            _tblEU = new DataTable("EU");

            DataColumn colCheckBox = new DataColumn("Select", typeof(String));
            colCheckBox.DefaultValue = "-";
            _tblEU.Columns.Add(colCheckBox);

            DataColumn colSource = new DataColumn("УЕ", typeof(String));
            _tblEU.Columns.Add(colSource);
            // colSource.Caption = "Партия";
            DataColumn colDate = new DataColumn("Марка", typeof(String));
            _tblEU.Columns.Add(colDate);
            // colDate.Caption = "Марка";

            DataColumn colNomer = new DataColumn("Размер", typeof(String));
            _tblEU.Columns.Add(colNomer);

            DataColumn colN = new DataColumn("Label", typeof(String));
            _tblEU.Columns.Add(colN);

            DataColumn colV = new DataColumn("Вес", typeof(String));
            colV.DefaultValue = "-";
            _tblEU.Columns.Add(colV);

            dataGridEu.DataSource = _tblEU;
        }

        public void CreateColumn(string HeaderText, string MappingName, int Width, int Pos)
        {
            ColumnStyle GridSelectCoulmn;
            GridSelectCoulmn = new ColumnStyle(Pos);
            GridSelectCoulmn.NullText = "-";
            GridSelectCoulmn.HeaderText = HeaderText;
            GridSelectCoulmn.MappingName = MappingName;
            GridSelectCoulmn.Width = Width;
            GridSelectCoulmn.CheckCellEven += new CheckCellEventHandler(myStyle_isEven);
            dataGridTableStyleMain.GridColumnStyles.Add(GridSelectCoulmn);

        }

        public void myStyle_isEven(object sender, DataGridEnableEventArgs e)
        {
            try
            {
                //Внедрим новое условие проверки
                string YE = (string)dataGridEu[e.Row, 0];
                //Для этой НУ найдем в таблице соответсвие и выделим как стоит
                e.PaintColor = false;
                e.MeetsCriteria = false;
                e.ColorRows = new SolidBrush(Color.DeepSkyBlue);

                for (int i = 0; i < _tblEU.Rows.Count; i++)
                {

                    if (YE == _tblEU.Rows[i]["УЕ"].ToString())
                    {
                        //Если это нужная
                        if (_tblEU.Rows[i]["Select"].ToString() == "+")
                        {
                            e.PaintColor = true;
                            break;
                        }
                    }

                }

            }

            catch (Exception)
            {
                e.PaintColor = false;
            }
        }

        private void Search()
        {
            string searchText = textBoxScan.Text;
            //Для поиска
            if (searchText.Length < 3)
            {
                MessageBox.Show("Введите более 3-х символов");
                return;
            }

            _tblEU.Clear();
            //Тут запилим поиск
            //select EU.RELMUCH_LABEL, EU.RPRT_NOM,  EU.MARKA_NAME, EU.RELMUCH_THICKNESS, EU.RELMUCH_WIDTH  FROM EU WHERE EU.RPRT_NOM  LIKE '%325%'LIMIT 100
         
            using (SQLiteConnection connection = new SQLiteConnection())
            {

                //Тут делаем таблицу и выводим инфу



                ;//(SQLiteConnection)factory.CreateConnection();
                connection.ConnectionString = "Data Source = " + SqLiteDB.pathDBFull_EU;
                SQLiteCommand command = new SQLiteCommand(connection);
                SQLiteCommand insert = new SQLiteCommand("select EU.RELMUCH_LABEL, EU.RPRT_NOM,  EU.MARKA_NAME, EU.RELMUCH_THICKNESS, EU.RELMUCH_WIDTH, EU.RELMUCH_VES  FROM EU WHERE EU.RPRT_NOM  LIKE '%" + searchText + "%' LIMIT 100;", connection);
                connection.Open();
                SQLiteDataReader reader = insert.ExecuteReader();
                while (reader.Read())
                {
                    DataRow row1 = _tblEU.NewRow();
                    //Запроск К БД
                    row1["Select"] = "-";
                    row1["Label"] = SqlLiteQuery.getReaderByName(reader, "RELMUCH_LABEL");
                    row1["УЕ"] = SqlLiteQuery.getReaderByName(reader, "RPRT_NOM");
                    row1["Марка"] = SqlLiteQuery.getReaderByName(reader, "MARKA_NAME");
                    row1["Размер"] = SqlLiteQuery.getReaderByName(reader, "RELMUCH_THICKNESS") + "х" + SqlLiteQuery.getReaderByName(reader, "RELMUCH_WIDTH");
                    row1["Вес"] = SqlLiteQuery.getReaderByName(reader, "RELMUCH_VES");

                    _tblEU.Rows.Add(row1);
                    //listEU.Add(EU);
                }
                reader.Close();
                connection.Close();

                command.Dispose();
                insert.Dispose();
                reader.Dispose();



            }


            dataGridEu.BeginInvoke(new Action(() =>
            {
                dataGridEu.DataSource = _tblEU;
            }));

            if (_tblEU.Rows.Count > 0)
            {
                //dataGridEu.BeginInvoke(new Action(() =>
                //{
                //    dataGridEu.Select(0);
                //    dataGridEu[dataGridEu.CurrentRowIndex, 5] = "+";
                //}));

            }
        }
        private void buttonSearch_Click(object sender, EventArgs e)
        {
            Search();
        }

        private void EUSearch_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
            else if (e.KeyCode == Keys.F3)
            {
                Search();
            }
            else if (e.KeyCode == Keys.F9)
            {
                this.DialogResult = DialogResult.OK;
                CreateSelectedList();
                this.Close();
            }

        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            CreateSelectedList();
            this.Close();
        }

        private void CreateSelectedList()
        {//Подготавливаем список выбраных ЕУ
            for (int i = 0; i < dataGridEu.VisibleRowCount; i++)
            {
                if (dataGridEu[i, 5].ToString() == "+")
                {
                    string SelectLabel = dataGridEu[i, 3].ToString();
                    string SelectYE = dataGridEu[i, 0].ToString();
                    string SelectMarka = dataGridEu[i, 1].ToString();
                    string SelectRazmer = dataGridEu[i, 2].ToString();
                    //
                    Double SelectWeight = 0;
                    try
                    {
                        SelectWeight = Double.Parse(dataGridEu[i, 4].ToString());
                    }

                    catch (Exception) { }

                    CommonType.SelectEU EU = new CommonType.SelectEU(SelectLabel, SelectYE, SelectMarka, SelectRazmer, SelectWeight);
                    SelectList.Add(EU);
                }
            }
        }

        private void dataGridEu_CurrentCellChanged(object sender, EventArgs e)
        {
            try
            {
                //dataGridEu.Select(dataGridEu.CurrentRowIndex);
                //Установим нужные значения
                if (dataGridEu[dataGridEu.CurrentRowIndex, 5].ToString() == "-")
                    dataGridEu[dataGridEu.CurrentRowIndex, 5] = "+";
                else
                    dataGridEu[dataGridEu.CurrentRowIndex, 5] = "-";
            }
            catch (Exception ex)
            {

            }
        }
    }
}