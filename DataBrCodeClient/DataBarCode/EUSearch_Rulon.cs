using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;

namespace DataBarCode
{

    public partial class EUSearch_Rulon : Form
    {
        DataTable _TblMarka = null;
        public DataTable _tblEU;

        public EUSearch_Rulon()
        {
            InitializeComponent();

            try
            {
                _TblMarka = SqlLiteQuery.GetMarkaUnic();
            }

            catch (Exception exe)
            {
                CLog.WriteException("EUSearch_Rulon.cs", "EUSearch_Rulon", exe.Message);
            }

            InitUi();

        }

        private void InitUi()
        {
            if (_TblMarka != null)
            {
                comboBoxMarka.DataSource = _TblMarka;
                comboBoxMarka.DisplayMember = "MARKA_NAME";
                comboBoxMarka.ValueMember = "MARKA_NAME";
                // comboBoxAgr.SelectedIndex = -1;
            }

        }

        private void EUSearch_Rulon_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
            else if (e.KeyCode == Keys.F3)
                SearchRun();
        }

        private void SearchRun()
        {
            ///Выполним поиск по необходимым элементам
            ///
            //Определим Биты поиска
            bool SerachTHICKNESS = false;
            if (textBoxTol.Text.Length > 0)
                SerachTHICKNESS = true;

            bool SerachRELMUCH_WIDTH = false;
            if (textBoxScanW.Text.Length > 0)
                SerachRELMUCH_WIDTH = true;

            bool Serach_MARKA_NAME = false;
            if (comboBoxMarka.SelectedValue.ToString() != "-")
                Serach_MARKA_NAME = true;

            _tblEU = new DataTable("EU");

            using (SQLiteConnection connection = new SQLiteConnection())
            {
                string SQL = @"select printf('%sx%s', EU.RELMUCH_THICKNESS, EU.RELMUCH_WIDTH) 'RAZMER', 
                                EU.MARKA_NAME, EU.MARKA_GOST, EU.RELMUCH_VES, TEHUZ.TEHUZ_NAZ from EU, TEHUZ, AGR 
                                WHERE EU.SIGN = '1' AND EU.TEHUZ_KOD = TEHUZ.TEHUZ_KOD AND EU.RPRTTYP_NAME = 'Рулон' 
                                AND (AGR.AGR_KOD = '262' OR AGR.AGR_KOD = '364' OR AGR.AGR_KOD = '461') AND AGR.AGR_KOD = TEHUZ.AGR_KOD



                                ";
                if (SerachTHICKNESS) //Добавляем толщину к поиску
                    SQL += " AND EU.RELMUCH_THICKNESS = '" + textBoxTol.Text.Replace(".", ",") + "'";

                if (SerachRELMUCH_WIDTH) //Добавляем ширину к поиску
                    SQL += " AND EU.RELMUCH_WIDTH = '" + textBoxScanW.Text + "'";

                if (Serach_MARKA_NAME) //Добавляем марка к поиску
                    SQL += " AND EU.MARKA_NAME = '" + comboBoxMarka.SelectedValue.ToString() + "'";


                SQL += " ORDER BY  EU.TEHUZ_KOD LIMIT 100;";

                connection.ConnectionString = "Data Source = " + SqLiteDB.pathDBFull_EU;
                SQLiteCommand command = new SQLiteCommand(connection);
                SQLiteCommand insert = new SQLiteCommand(SQL, connection);
                connection.Open();
                SQLiteDataReader reader = insert.ExecuteReader();
                _tblEU.Load(reader);
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

            if (_tblEU.Rows.Count >= 100)
            {
                MessageBox.Show("Ограничьте параметры поиска. Показаны первые 100 УЕ", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            }
        }
    }
}