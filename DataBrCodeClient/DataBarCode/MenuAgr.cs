using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DataBarCode
{
    public partial class MenuAgr : Form
    {
        public ListScanOperation ScanOperation;
        public Intermec.DataCollection.BarcodeReader bcr;
        WarehousePost _WareHousePost = null;
        QueueTask _queueT = null;

        public MenuAgr(Intermec.DataCollection.BarcodeReader _bcr, ListScanOperation _ScanOperation)
        {
            InitializeComponent();
            bcr = _bcr;
            ScanOperation = _ScanOperation;

            //Выводим инфушку
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
            this.KeyPreview = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //Выводим инфушку
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

        private void MenuAgr_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }



        private void buttonapr_Click(object sender, EventArgs e)
        {

            if (_WareHousePost == null)
            {
                _WareHousePost = new WarehousePost(bcr, "MX0462.4620001", ScanOperation, 3);
                _WareHousePost.Show();
            }

            else
            {
                if (_WareHousePost.FormActive) { }
                else
                {
                    _WareHousePost.Close();
                    // UIEU.Dispose();
                    _WareHousePost = new WarehousePost(bcr, "MX0462.4620001", ScanOperation, 3);
                    _WareHousePost.Show();
                }
            }

        }

        private void buttonpga_Click(object sender, EventArgs e)
        {
            //Для ПГА логика другая!!!!!!

           // QueueTask _queueT = null;
            if (_queueT == null)
            {
                _queueT = new QueueTask(bcr, ListScanOperation.TaskPGA);
                _queueT.Show();
            }

            else
            {
                if (_queueT.FormActive) { }
                else
                {
                    _queueT.Close();
                    // UIEU.Dispose();
                    _queueT = new QueueTask(bcr, ListScanOperation.TaskPGA);
                    _queueT.Show();
                }
            }
        }

        private void buttontgp_Click(object sender, EventArgs e)
        {
            if (_WareHousePost == null)
            {
                _WareHousePost = new WarehousePost(bcr, "MX0441.4410001", ScanOperation, 3);
                _WareHousePost.Show();
            }

            else
            {
                if (_WareHousePost.FormActive) { }
                else
                {
                    _WareHousePost.Close();
                    // UIEU.Dispose();
                    _WareHousePost = new WarehousePost(bcr, "MX0441.4410001", ScanOperation, 3);
                    _WareHousePost.Show();
                }
            }
        }

        private void buttonT19_Click(object sender, EventArgs e)
        {
            if (_WareHousePost == null)
            {
                _WareHousePost = new WarehousePost(bcr, "MX0263.2630001", ScanOperation, 3);
                _WareHousePost.Show();
            }

            else
            {
                if (_WareHousePost.FormActive) { }
                else
                {
                    _WareHousePost.Close();
                    // UIEU.Dispose();
                    _WareHousePost = new WarehousePost(bcr, "MX0263.2630001", ScanOperation, 3);
                    _WareHousePost.Show();
                }
            }
        }

        private void buttonT50_Click(object sender, EventArgs e)
        {
            if (_WareHousePost == null)
            {
                _WareHousePost = new WarehousePost(bcr, "MX0324.3240001", ScanOperation, 3);
                _WareHousePost.Show();
            }

            else
            {
                if (_WareHousePost.FormActive) { }
                else
                {
                    _WareHousePost.Close();
                    // UIEU.Dispose();
                    _WareHousePost = new WarehousePost(bcr, "MX0324.3240001", ScanOperation, 3);
                    _WareHousePost.Show();
                }
            }
        }

        private void buttonT21_Click(object sender, EventArgs e)
        {
            if (_WareHousePost == null)
            {
                _WareHousePost = new WarehousePost(bcr, "MX0362.3620001", ScanOperation, 3);
                _WareHousePost.Show();
            }

            else
            {
                if (_WareHousePost.FormActive) { }
                else
                {
                    _WareHousePost.Close();
                    // UIEU.Dispose();
                    _WareHousePost = new WarehousePost(bcr, "MX0362.3620001", ScanOperation, 3);
                    _WareHousePost.Show();
                }
            }
        }
    }
}