using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;
using Intermec.DataCollection;

namespace DataBarCode
{
    public partial class InventTaskMX : Form
    {
        public ListScanOperation ScanOperation;
        public DataTable _tblTask;
        public Intermec.DataCollection.BarcodeReader bcr;
        public bool FormActive = true;
        private string SelectRZDN = "";
        private string SelectMX_LABEL = "";
        public WarehousePost _WareHousePost = null;

        public InventTaskMX(Intermec.DataCollection.BarcodeReader _bcr, ListScanOperation _ScanOperation, string RZDN, string DateRZDN, string MoreRZDN)
        {
            InitializeComponent();
            labelData.Text = DateRZDN;
            labelDetalIN.Text = MoreRZDN;
            SelectRZDN = RZDN;

            bcr = _bcr;
            ScanOperation = _ScanOperation;
            //Тут правим лейбл
            string StatusBD = "БД: " + SqLiteDB.UpdateDateTime + ". Операции: " + BufferToBD.CountBuffer;

            labelBD.BeginInvoke(new Action(() =>
            {
                labelBD.Text = StatusBD;
            }));
            this.KeyPreview = true;

            LoadRZDN();

            InitScaner();
        }

        public void InitScaner()
        {
            try
            {
                if (bcr == null)
                {
                    bcr = new BarcodeReader();
                    //set BarcodeRead event
                }
                bcr.BarcodeRead += new BarcodeReadEventHandler(bcr_BarcodeReadInventTaskMX);
                //sends the BarcodeRead event after each successful read
                bcr.ThreadedRead(true);

                bcr.symbology.Code128.Enable = false;
                //set Interleaved 2 of 5
                bcr.symbology.Interleaved2Of5.Enable = false;
                //set PDF417
                bcr.symbology.Pdf417.Enable = false;


            }

            catch (Exception exp)
            {
                //MessageBox.Show(exp.Message);
                CLog.WriteException("InventTaskMX.cs", "InitScaner", exp.Message);
            }

        }

        void bcr_BarcodeReadInventTaskMX(object sender, BarcodeReadEventArgs bre)
        {

            try
            {
                string EU = bre.strDataBuffer;
                if (EU.IndexOf("MX") == 0)
                {//Это место хранения 
                    //bcr.BarcodeRead -= new BarcodeReadEventHandler(bcr_BarcodeRead);
                    //А тперь проверим на соответсвие списку по ведомости
                    bool GoScan = false;
                    if (_tblTask != null)
                    {
                        for (int ii= 0; ii <_tblTask.Rows.Count; ii++) {
                            if (_tblTask.Rows[ii]["MX_LABEL"].ToString().ToUpper() == EU)
                                GoScan = true;
                        }
                    }

                    if (GoScan)
                    {

                        switch (ScanOperation)
                        {
                            case ListScanOperation.InventoryTask:
                                {
                                    //Если операция Перемещение то нужно что то сделать...!!!!

                                    if (_WareHousePost == null)
                                    {
                                        _WareHousePost = new WarehousePost(bcr, EU, ScanOperation, 5);
                                        _WareHousePost.SETRZDN = SelectRZDN;
                                        _WareHousePost.Show();
                                    }

                                    else
                                    {
                                        if (_WareHousePost.FormActive) { }
                                        else
                                        {
                                            _WareHousePost.Close();
                                            // UIEU.Dispose();
                                            _WareHousePost = new WarehousePost(bcr, EU, ScanOperation, 5);
                                            _WareHousePost.SETRZDN = SelectRZDN;
                                            _WareHousePost.Show();
                                        }
                                    }

                                    break;
                                }

                        }
                    }
                }
                else
                {

                }
            }

            catch (Exception exp)
            {
                CLog.WriteException("InventTaskMX.cs", "bcr_BarcodeReadInventTaskMX", exp.Message);
                //MessageBox.Show(exp.Message);
            }

        }
        private void LoadRZDN()
        {//Загрузка заданий
        

            using (SQLiteConnection connection = new SQLiteConnection())
            {

                //Тут делаем таблицу и выводим инфу

                _tblTask = new DataTable("TASK");

                ;//(SQLiteConnection)factory.CreateConnection();
                connection.ConnectionString = "Data Source = " + SqLiteDB.pathDBFull_EU;
                SQLiteCommand command = new SQLiteCommand(connection);
                // SQLiteCommand insert = new SQLiteCommand("select * from TASKLIST WHERE ROWNUM <> -1 ORDER BY TASKLIST.ROWNUM  LIMIT 100;", connection);
                //
                SQLiteCommand insert = new SQLiteCommand("select  AGR.AGR_NAZ 'AGR' , TEHUZ.TEHUZ_NAZ 'MX', TEHUZ.TEHUZ_LABEL 'MX_LABEL', INVENTORY.RESCODE  from INVENTORY, TEHUZ, AGR WHERE INVENTORY.RZDN_PRM= '" + SelectRZDN + "' AND TEHUZ.TEHUZ_KOD = INVENTORY.TEHUZ_KOD AND TEHUZ.AGR_KOD = AGR.AGR_KOD", connection);
                connection.Open();
                SQLiteDataReader reader = insert.ExecuteReader();
                _tblTask.Load(reader);
                reader.Close();
                connection.Close();

                command.Dispose();
                insert.Dispose();
                reader.Dispose();

            }


            dataGridTask.BeginInvoke(new Action(() =>
            {
                dataGridTask.DataSource = _tblTask;
            }));

            if (_tblTask.Rows.Count > 0)
            {

                buttonNext.BeginInvoke(new Action(() =>
                {
                    buttonNext.Enabled = true;
                }));


                dataGridTask.BeginInvoke(new Action(() =>
                {
                    dataGridTask.Select(0);

                    try
                    {
                        SelectMX_LABEL = dataGridTask[0, 2].ToString();
                    }

                    catch (Exception ex)
                    {
                        // MessageBox.Show(ex.Message);
                    }

                }));

                //SelectLabel = dataGridEu[dataGridEu.CurrentRowIndex, 3].ToString();
                //SelectYE = dataGridEu[dataGridEu.CurrentRowIndex, 0].ToString();
                //SelectMarka = dataGridEu[dataGridEu.CurrentRowIndex, 1].ToString();
                //SelectRazmer = dataGridEu[dataGridEu.CurrentRowIndex, 2].ToString();
            }


        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                string StatusBD = StatusBar.getSatus();

                labelBD.BeginInvoke(new Action(() =>
                {
                    labelBD.Text = StatusBD;
                    labelBD.ForeColor = StatusBar.GetColorLabel();
                }));
            }

            catch (Exception) { }
        }

        private void OperationNext()
        {
            //Тут пилим запуск формы2

            //Если операция Перемещение то нужно что то сделать...!!!!

            if (_WareHousePost == null)
            {
                _WareHousePost = new WarehousePost(bcr, SelectMX_LABEL, ScanOperation, 3);
                _WareHousePost.SETRZDN = SelectRZDN;
                _WareHousePost.Show();
            }

            else
            {
                if (_WareHousePost.FormActive) { }
                else
                {
                    _WareHousePost.Close();
                    // UIEU.Dispose();
                    _WareHousePost = new WarehousePost(bcr, SelectMX_LABEL, ScanOperation, 3);
                    _WareHousePost.SETRZDN = SelectRZDN;
                    _WareHousePost.Show();
                }
            }


        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            OperationNext();
        }

        private void InventTaskMX_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                FormActive = false;
                this.Close();
            }

            else if (e.KeyCode == Keys.F1)
            {
                ///Интерфейс для теста


            }

            else if (e.KeyCode == Keys.F9)
            {
                OperationNext();
            }
        }

        private void dataGridTask_CurrentCellChanged(object sender, EventArgs e)
        {
            try
            {
                dataGridTask.Select(dataGridTask.CurrentRowIndex);
                //Установим нужные значения
                SelectMX_LABEL = dataGridTask[dataGridTask.CurrentRowIndex, 2].ToString();
            }
            catch (Exception ex)
            {

            }
        }

        private void InventTaskMX_Closed(object sender, EventArgs e)
        {
            bcr.BarcodeRead -= new BarcodeReadEventHandler(bcr_BarcodeReadInventTaskMX);
        }
    }
}