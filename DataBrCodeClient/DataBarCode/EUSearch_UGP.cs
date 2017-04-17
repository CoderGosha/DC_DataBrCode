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
    public partial class EUSearch_UGP : Form
    {
        public DataTable _tblEU;

        public EUSearch_UGP()
        {
            InitializeComponent();
        }

        private void EUSearch_UGP_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
            else if (e.KeyCode == Keys.F3)
                SearchRun();
        }

        private void SearchRun()
        {
           ///
            //Определим Биты поиска
            bool Serach_RPRT_NOM = false;
            if (textBoxFOK.Text.Length > 0)
                Serach_RPRT_NOM = true;

            bool Serach_SAP = false;
            if (textBoxSAP.Text.Length > 0)
                Serach_SAP = true;

            _tblEU = new DataTable("EU");

            using (SQLiteConnection connection = new SQLiteConnection())
            {
                string SQL = @"select EU.RPRT_NOM, EU.ORDER_SAP, TEHUZ.TEHUZ_NAZ from EU, TEHUZ, AGR WHERE EU.SIGN = '1' AND EU.TEHUZ_KOD = TEHUZ.TEHUZ_KOD AND EU.RPRTTYP_NAME = 'Пачка'
                                AND (AGR.AGR_KOD = '262' OR AGR.AGR_KOD = '364' OR AGR.AGR_KOD = '461') AND AGR.AGR_KOD = TEHUZ.AGR_KOD AND EU.INTRV_TMEND == ''


                                ";

                if (Serach_RPRT_NOM) //Добавляем ФОК к поиску
                    SQL += " AND EU.RPRT_NOM LIKE '%" + textBoxFOK.Text + "%'";

                if (Serach_SAP) //Добавляем SAP к поиску
                    SQL += " AND EU.ORDER_SAP LIKE '%" + textBoxSAP.Text + "%'";

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