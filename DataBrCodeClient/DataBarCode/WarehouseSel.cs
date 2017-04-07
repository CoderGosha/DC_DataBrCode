using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Intermec.DataCollection;

namespace DataBarCode
{
    public partial class WarehouseSel : Form
    {
        public ListScanOperation ScanOperation;

        public string LoginUser;
        DataTable _TblAgr = null;
        DataTable _TblSelectWareHouse = null;
        public Intermec.DataCollection.BarcodeReader bcr;
        WarehousePost _WareHousePost = null;

        public WarehouseSel(Intermec.DataCollection.BarcodeReader _bcr, string _LoginUser, string _Date, string _Sm, ListScanOperation _ScanOperation)
        {
            InitializeComponent();
            bcr = _bcr;

            ScanOperation = _ScanOperation;

            this.LoginUser = _LoginUser;
            this.Text = _LoginUser + " Смена:" + _Sm;

            try
            {
                _TblAgr = SqlLiteQuery.GetWareHouse();
            }

            catch (Exception exe) 
            {
                CLog.WriteException("WarehouseSel.cs", "WarehouseSel", exe.Message);
            }
            InitUi();
            InitScaner();




            labelStatus.Text = "";

            //Тут правим лейбл
            string StatusBD = "БД: " + SqLiteDB.UpdateDateTime + ". Операции: " + BufferToBD.CountBuffer;

            labelBD.BeginInvoke(new Action(() =>
            {
                labelBD.Text = StatusBD;
            }));

            this.KeyPreview = true;

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
                    bcr.BarcodeRead += new BarcodeReadEventHandler(bcr_BarcodeRead);
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
                CLog.WriteException("WarehouseSel.cs", "InitScaner", exp.Message);
            }

        }

        public void InitUi()
        {
            if (_TblAgr != null)
            {
                comboBoxAgr.DataSource = _TblAgr;
                comboBoxAgr.DisplayMember = "AGR_NAZ";
                comboBoxAgr.ValueMember = "AGR_KOD";
                // comboBoxAgr.SelectedIndex = -1;

            }


        }

        void bcr_BarcodeRead(object sender, BarcodeReadEventArgs bre)
        {

            try
            {
                string EU = bre.strDataBuffer;

                if (EU.IndexOf("MX") == 0)
                {//Это место хранения 
                    //bcr.BarcodeRead -= new BarcodeReadEventHandler(bcr_BarcodeRead);

                    switch (ScanOperation)
                    {
                        case ListScanOperation.MXSet:
                            {
                                //Если операция размещение то нужно что то сделать...!!!!

                                if (_WareHousePost == null)
                                {
                                    _WareHousePost = new WarehousePost(bcr, EU, ScanOperation, 5);
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
                                        _WareHousePost.Show();
                                    }
                                }

                                break;
                            }

                        case ListScanOperation.EUTaskMove:
                            {
                                //Если операция Перемещение то нужно что то сделать...!!!!

                                if (_WareHousePost == null)
                                {
                                    _WareHousePost = new WarehousePost(bcr, EU, ScanOperation, 5);
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
                                        _WareHousePost.Show();
                                    }
                                }

                                break;
                            }

                    }
                  

                    //Возвращаем обработку события этого делать тут не стоит
                    //bcr.BarcodeRead += new BarcodeReadEventHandler(bcr_BarcodeRead);

                }
                else
                {

                }

            }
            catch (Exception exp)
            {
                CLog.WriteException("WarehouseSel.cs", "bcr_BarcodeRead", exp.Message);
                //MessageBox.Show(exp.Message);
            }

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }




        private void buttonNext_Click(object sender, EventArgs e)
        {
            //comboBoxWareHouse.Items.Clear();
            if (comboBoxWareHouse.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите место размещения");
                return;

            }
            try
            {
                string SelectedPlace = comboBoxWareHouse.SelectedValue.ToString();


                switch (ScanOperation)
                {
                    case ListScanOperation.MXSet:
                        {
                            //Если операция размещение то нужно что то сделать...!!!!

                            if (_WareHousePost == null)
                            {
                                _WareHousePost = new WarehousePost(bcr, SelectedPlace, ScanOperation, 3);
                                _WareHousePost.Show();
                            }

                            else
                            {
                                if (_WareHousePost.FormActive) { }
                                else
                                {
                                    _WareHousePost.Close();
                                    // UIEU.Dispose();
                                    _WareHousePost = new WarehousePost(bcr, SelectedPlace, ScanOperation, 3);
                                    _WareHousePost.Show();
                                }
                            }

                            break;
                        }

                    case ListScanOperation.EUTaskMove:
                        {
                            //Если операция Перемещение то нужно что то сделать...!!!!

                            if (_WareHousePost == null)
                            {
                                _WareHousePost = new WarehousePost(bcr, SelectedPlace, ScanOperation, 3);
                                _WareHousePost.Show();
                            }

                            else
                            {
                                if (_WareHousePost.FormActive) { }
                                else
                                {
                                    _WareHousePost.Close();
                                    // UIEU.Dispose();
                                    _WareHousePost = new WarehousePost(bcr, SelectedPlace, ScanOperation, 3);
                                    _WareHousePost.Show();
                                }
                            }

                            break;
                        }

                }
            }

            catch (Exception ex)
            {
                labelStatus.BeginInvoke(new Action(() =>
                {
                    labelStatus.Text = "Ex: " + ex.Message;
                }));

            }
        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBoxAgr_SelectedIndexChanged(object sender, EventArgs e)
        {////Подгуржаем места хранения...

        }

        private void comboBoxAgr_SelectedValueChanged(object sender, EventArgs e)
        {

            if ((comboBoxAgr.SelectedIndex != -1) && (comboBoxAgr.Items.Count > 0))
            {
                labelStatus.BeginInvoke(new Action(() =>
                {
                    labelStatus.Text = "";
                }));

                //comboBoxWareHouse.BeginUpdate();
                string SelectedAgr = "";
                SelectedAgr = comboBoxAgr.SelectedValue.ToString();
                try
                {

                    comboBoxWareHouse.Enabled = true;
                    

                    if (SelectedAgr != null)
                    {

                        _TblSelectWareHouse = SqlLiteQuery.GetPlace(SelectedAgr, true);

                        comboBoxWareHouse.DataSource = _TblSelectWareHouse;
                        comboBoxWareHouse.DisplayMember = "TEHUZ_NAZ";
                        
                        comboBoxWareHouse.ValueMember = "TEHUZ_LABEL";
                    }



                }



                catch (Exception ex)
                {

                    labelStatus.BeginInvoke(new Action(() =>
                    {
                        labelStatus.Text = ex.Message + SelectedAgr;
                    }));
                    CLog.WriteException("WarehouseSel.cs", "comboBoxAgr_SelectedIndexChanged", ex.Message);
                }
                finally
                {
                    comboBoxWareHouse.EndUpdate();
                }
            }
            else
            {
                try
                {
                   
                }
                catch (Exception) { }

            }
        }

        private void comboBoxWareHouse_SelectedValueChanged(object sender, EventArgs e)
        {
          
        }

        private void WarehouseSel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }

            else if (e.KeyCode == Keys.D1)
            {
                if (_WareHousePost == null)
                {
                    _WareHousePost = new WarehousePost(bcr, "MX0262.0005511", ListScanOperation.MXSet, 3);
                    _WareHousePost.Show();
                }

                else
                {
                    _WareHousePost.Close();
                    // UIEU.Dispose();
                    _WareHousePost = new WarehousePost(bcr, "MX0262.0005511", ListScanOperation.MXSet, 3);
                    _WareHousePost.Show();
                }
            }

            else if (e.KeyCode == Keys.D2)
            {
                if (_WareHousePost == null)
                {
                    _WareHousePost = new WarehousePost(bcr, "MX0262.0005512", ListScanOperation.MXSet, 3);
                    _WareHousePost.Show();
                }

                else
                {
                    _WareHousePost.Close();
                    // UIEU.Dispose();
                    _WareHousePost = new WarehousePost(bcr, "MX0262.0005512", ListScanOperation.MXSet, 3);
                    _WareHousePost.Show();
                }
            }

            else if (e.KeyCode == Keys.D3)
            {
                if (_WareHousePost == null)
                {
                    _WareHousePost = new WarehousePost(bcr, "MX0262.0005510", ListScanOperation.MXSet, 3);
                    _WareHousePost.Show();
                }

                else
                {
                    _WareHousePost.Close();
                    // UIEU.Dispose();
                    _WareHousePost = new WarehousePost(bcr, "MX0262.0005510", ListScanOperation.MXSet, 3);
                    _WareHousePost.Show();
                }
            }
        }

        private void WarehouseSel_Closed(object sender, EventArgs e)
        {
            bcr.BarcodeRead -= new BarcodeReadEventHandler(bcr_BarcodeRead);
        }

        private void timerDich_Tick(object sender, EventArgs e)
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

      
    }
}
