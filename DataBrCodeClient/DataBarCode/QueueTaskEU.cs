using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Intermec.DataCollection;
using System.Data.SQLite;
using System.Threading;
using System.Net;

namespace DataBarCode
{
    public partial class QueueTaskEU : Form
    {
        string RZDN = "";
        public bool FormActive = true;
        public string LabelEU;
        public string details;
        Settings set;
        public DataTable _tblEU;
        public Intermec.DataCollection.BarcodeReader bcr;
        public List<WebReference.Relmuch> listEU = null;
        public string MarkaRZDN = "-";
        public List<string> MarkaRZDNList = null;
        public double ScanWeigth = 0;
        public QueueTaskEU(Intermec.DataCollection.BarcodeReader _bcr, string _RZDN)
        {
            InitializeComponent();

            FormActive = true;
            set = new Settings("DataBrCode.xml");

            //  ScanOperation = _ScanOperation;
            listEU = new List<WebReference.Relmuch>();

            bcr = _bcr;

            this.RZDN = _RZDN;
            InitTable();
            InitScaner();


            //Тут правим лейбл
            string StatusBD = "БД: " + SqLiteDB.UpdateDateTime + ". Операции: " + BufferToBD.CountBuffer;

            labelBD.BeginInvoke(new Action(() =>
            {
                labelBD.Text = StatusBD;
            }));

            this.KeyPreview = true;
          //  labelMX.Text += " " + this.RZDN;

            ScanWeigth = SqlLiteQuery.GetWEIGHTByRZDN(_RZDN);
            labelMX.Text = "Осталось: " + Math.Round(ScanWeigth, 2).ToString() + " т.";

            if (ScanWeigth <= 0)
                labelMX.ForeColor = Color.White;
            else
                labelMX.ForeColor = Color.Tomato;


            details = SqlLiteQuery.GetDetalisbyRZDN(_RZDN);
            MarkaRZDN = SqlLiteQuery.GetMarkabyRZDN(_RZDN);
            labelDetal.Text = details;
            MarkaRZDNList = SqlLiteQuery.GetMarkabyRZDNList(_RZDN);

        }

        public void InitTable()
        {

            _tblEU = new DataTable("EU");



            DataColumn colSource = new DataColumn("УЕ", typeof(String));
            colSource.DefaultValue = "-";
            _tblEU.Columns.Add(colSource);
            // colSource.Caption = "Партия";
            DataColumn colDate = new DataColumn("Марка", typeof(String));
            colDate.DefaultValue = "-";
            _tblEU.Columns.Add(colDate);
            // colDate.Caption = "Марка";

            DataColumn colNomer = new DataColumn("Размер", typeof(String));
            colNomer.DefaultValue = "-";
            // 
            _tblEU.Columns.Add(colNomer);

            DataColumn colN = new DataColumn("Label", typeof(String));
            colN.DefaultValue = "-";
            _tblEU.Columns.Add(colN);

            DataColumn colV = new DataColumn("Вес", typeof(String));
            colV.DefaultValue = "-";
            _tblEU.Columns.Add(colV);

            dataGridEu.DataSource = _tblEU;
        }


        private void InitScaner()
        {

            if (bcr == null)
                bcr = new BarcodeReader();
            //set BarcodeRead event
            bcr.BarcodeRead += new BarcodeReadEventHandler(bcr_BarcodeReadQueueTaskEU);
            //sends the BarcodeRead event after each successful read
            bcr.ThreadedRead(true);
            bcr.symbology.Code128.Enable = false;
            //set Interleaved 2 of 5
            bcr.symbology.Interleaved2Of5.Enable = false;
            //set PDF417
            bcr.symbology.Pdf417.Enable = false;
            bcr.symbology.QrCode.Enable = false;
        }

        public string _getReaderByName(SQLiteDataReader rd, string NameF)
        {
            string tmp = "Нет данных";
            try
            {
                tmp = rd.GetValue(rd.GetOrdinal(NameF)).ToString();
                return tmp;
            }

            catch (Exception)
            {
                return tmp;
            }


        }

        void bcr_BarcodeReadQueueTaskEU(object sender, BarcodeReadEventArgs bre)
        {

            try
            {
                string EU = bre.strDataBuffer;
                LabelEU = EU;
                ///Тут Алгоритм разбора что мы все-таки считали
                ///Для начала считаем по-умолчанию что считываем мы только ЕУ и пишем алгоритм
                ///Открытия формы

                if (EU.IndexOf("MX") == 0)
                {//
                    //labelStatus.BeginInvoke(new Action(() =>
                    //{
                    //    labelStatus.Text = "Считано место хранения: " + EU;
                    //}));
                    return;
                }

                //labelStatus.BeginInvoke(new Action(() =>
                //{
                //    labelStatus.Text = "Label: " + EU;
                //}));

                //MessageBox.Show(EU);

                // GetDataEU(EU);


                //Проверим есть ли данная ЕУ в списке
                if (ValidateList.CheckEUByListType(listEU, LabelEU))
                {
                    //ЕУ уже в списке
                    Sound.PlaySoundWarning();
                    //Vibration.PlayVibration(2000);
                    return;
                }


                dataGridEu.BeginInvoke(new Action(() =>
                {
                    dataGridEu.BackColor = Color.White;
                }));

                //Тут делаем таблицу и выводим инфу
                DataRow row1 = _tblEU.NewRow();
                row1["Label"] = EU;

                Double WEIGHT_EU = 0;
                //WebReference.WebSDataBrCode BrServer = new WebReference.WebSDataBrCode();
                //BrServer.SoapVersion = System.Web.Services.Protocols.SoapProtocolVersion.Soap12;
                //BrServer.Url = set.AdressAppServer;
                //DataTable result = BrServer.EU_GetData(EU);
       
                string MarkaEU = "";
                using (SQLiteConnection connection = new SQLiteConnection())
                {

                    ;//(SQLiteConnection)factory.CreateConnection();
                    connection.ConnectionString = "Data Source = " + SqLiteDB.pathDBFull_EU;
                    SQLiteCommand command = new SQLiteCommand(connection);
                    SQLiteCommand insert = new SQLiteCommand("select * from EU e WHERE e.RELMUCH_LABEL = '" + EU + "';", connection);
                    connection.Open();
                    SQLiteDataReader reader = insert.ExecuteReader();
                    
                    while (reader.Read())
                    {
                        //Запроск К БД
                        MarkaEU = _getReaderByName(reader, "MARKA_NAME");
                        string sWEIGHT_EU = _getReaderByName(reader, "RELMUCH_VES");

                        row1["УЕ"] = _getReaderByName(reader, "RPRT_NOM");
                        row1["Марка"] = MarkaEU;
                        row1["Размер"] = _getReaderByName(reader, "RELMUCH_THICKNESS") + "х" + _getReaderByName(reader, "RELMUCH_WIDTH");
                        row1["Вес"] = sWEIGHT_EU;
                        // row1["S"] = "";
                        /*
                         * CREATE TABLE [EU] (
                                    [RELMUCH_LABEL] char(20) NOT NULL,
                                    [RELMUCH_PRM] char(20),
                                    [RELMUCH_VES] char(20),
                                    [RELMUCH_FVES] char(20),
                                    [RELMUCH_WIDTH] char(20),
                                    [RELMUCH_THICKNESS] char(20),
                                    [RPRT_NOM] char(20),
                                    [RPRTTYP_NAME] char(20),
                                    [RPRT_TOL] char(20),
                                    [RPRT_SHRN] char(20),
                                    [RPRT_PLVNOM] char(20),
                                    [MARKA_NAME] char(20),
                                    [MARKA_GOST] char(20),
                                    [FACT_STORAGE_CODE] char(20),
                                    [TEHUZ_LABEL] char(20),
                                    [FACT_PLACE_NAME] char(20),
                                    [INTRV_TMBEG] char(20))
                         * */
                        try
                        {
                            WEIGHT_EU = Double.Parse(sWEIGHT_EU);
                        }
                        catch (Exception) { }
                    }
                    reader.Close();
                    connection.Close();

                    command.Dispose();
                    insert.Dispose();
                    reader.Dispose();
                }

                //MarkaEU = MarkaEU.ToUpper()
                ////Тут введем проверку на Марку b и потом что то еще
                if (MarkaEU.ToUpper() == MarkaRZDN.ToUpper())
                {
                    ScanWeigth -= WEIGHT_EU;
                    _tblEU.Rows.Add(row1);

                    WebReference.Relmuch EUT = new WebReference.Relmuch();
                    EUT.LABEL = EU;
                    EUT.CODEAUTOMATIC = 5;
                    listEU.Add(EUT);
                }
                else if  (MarkaRZDNList.IndexOf(MarkaEU.ToUpper()) != -1)
                { 
                    //вкошмарим поиск
                    ScanWeigth -= WEIGHT_EU;
                    _tblEU.Rows.Add(row1);

                    WebReference.Relmuch EUT = new WebReference.Relmuch();
                    EUT.LABEL = EU;
                    EUT.CODEAUTOMATIC = 5;
                    listEU.Add(EUT);
                }
                else
                {
                    OpenNETCF.Media.SystemSounds.Beep.Play();
                    Thread.Sleep(100);
                    OpenNETCF.Media.SystemSounds.Beep.Play();
                }

                dataGridEu.BeginInvoke(new Action(() =>
                {
                    dataGridEu.DataSource = _tblEU;
                }));


                labelMX.BeginInvoke(new Action(() =>
                {
                    //Вычитаем и обновляем тонны
                    labelMX.Text = "Осталось: " + Math.Round(ScanWeigth, 2).ToString() + " т.";
                    if (ScanWeigth <= 0)
                        labelMX.ForeColor = Color.White;
                    else
                        labelMX.ForeColor = Color.Tomato;
                }));



            }
            catch (Exception exp)
            {
                CLog.WriteException("WarehousePost.cs", "bcr_BarcodeRead", exp.Message);
                //MessageBox.Show(exp.Message);
            }

        }




        private void dataGridTask_CurrentCellChanged(object sender, EventArgs e)
        {

        }


        private void buttonEUSearch_Click_1(object sender, EventArgs e)
        {
            EUSearch search = new EUSearch();
            DialogResult DL = search.ShowDialog();
            if (DL == DialogResult.OK)
            {
                if (search._tblEU.Rows.Count > 0)
                {
                    //Запросим выбрнаные УЕ
                    List<CommonType.SelectEU> SelList = search.GetSelectedEU();
                    if (SelList != null)
                    {
                        foreach (var elem in SelList)
                        {
                            //Проверим есть ли данная ЕУ в списке
                            if (ValidateList.CheckEUByListType(listEU, elem.Label))
                            {
                                //ЕУ уже в списке
                                Sound.PlaySoundWarning();
                                //return;
                            }
                            else
                            {
                                Double WEIGHT_EU = 0;
                                WEIGHT_EU = elem.Weight;

                                DataRow row1 = _tblEU.NewRow();
                                row1["Label"] = elem.Label;
                                row1["УЕ"] = elem.YE;
                                row1["Вес"] = elem.Weight.ToString();
                                row1["Марка"] = elem.Marka;
                                row1["Размер"] = elem.Razmer;

                                ScanWeigth -= WEIGHT_EU;
                                _tblEU.Rows.InsertAt(row1, 0);

                                WebReference.Relmuch EUT = new WebReference.Relmuch();
                                EUT.LABEL = elem.Label;
                                EUT.CODEAUTOMATIC = 3;
                                listEU.Add(EUT);
                            }
                        }
                    }

                    dataGridEu.BeginInvoke(new Action(() =>
                    {
                        dataGridEu.DataSource = _tblEU;
                    }));
                    OpenNETCF.Media.SystemSounds.Beep.Play();
                }
            }
        }

        private void QueueTaskEU_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }

            else if (e.KeyCode == Keys.F9)
            {

                POST_EU_LIST_RZDN_AGR();

            }


            else if (e.KeyCode == Keys.F3)
            {
                //Поиск ЕУ по филдь фильтрам
                //Запускаем интерефейс поиска ЕУ
                ListMxByRZDN search = new ListMxByRZDN(RZDN, ListScanOperation.TaskPGA);
                DialogResult DL = search.ShowDialog();

            }
            else if (e.KeyCode == Keys.F12)
            {

                //Запускаем интерефейс поиска ЕУ
                EUSearch search = new EUSearch();
                DialogResult DL = search.ShowDialog();
                if (DL == DialogResult.OK)
                {
                    if (search._tblEU.Rows.Count > 0)
                    {

                        //Запросим выбрнаные УЕ
                        List<CommonType.SelectEU> SelList = search.GetSelectedEU();
                        if (SelList != null)
                        {
                            foreach (var elem in SelList)
                            {
                                 //Проверим есть ли данная ЕУ в списке
                                if (ValidateList.CheckEUByListType(listEU, elem.Label))
                                {
                                    //ЕУ уже в списке
                                    Sound.PlaySoundWarning();
                                    //return;
                                }
                                else
                                {
                                    Double WEIGHT_EU = 0;
                                    WEIGHT_EU = elem.Weight;

                                    DataRow row1 = _tblEU.NewRow();
                                    row1["Label"] = elem.Label;
                                    row1["УЕ"] = elem.YE;
                                    row1["Вес"] = elem.Weight.ToString();
                                    row1["Марка"] = elem.Marka;
                                    row1["Размер"] = elem.Razmer;

                                    ScanWeigth -= WEIGHT_EU;
                                    _tblEU.Rows.InsertAt(row1, 0);

                                    WebReference.Relmuch EUT = new WebReference.Relmuch();
                                    EUT.LABEL = elem.Label;
                                    EUT.CODEAUTOMATIC = 3;
                                    listEU.Add(EUT);
                                }
                            }
                        }

                        dataGridEu.BeginInvoke(new Action(() =>
                        {
                            dataGridEu.DataSource = _tblEU;
                        }));
                        OpenNETCF.Media.SystemSounds.Beep.Play();

                        labelMX.BeginInvoke(new Action(() =>
                        {
                            //Вычитаем и обновляем тонны
                            labelMX.Text = "Осталось: " + Math.Round(ScanWeigth, 2).ToString() + " т.";

                            if (ScanWeigth <= 0)
                                labelMX.ForeColor = Color.White;
                            else
                                labelMX.ForeColor = Color.Tomato;
                        }));

                    }
                }
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

        private void QueueTaskEU_Closed(object sender, EventArgs e)
        {
            FormActive = false;
        }

        private void dataGridEu_Paint(object sender, PaintEventArgs e)
        {

        }


        private void POST_EU_LIST_RZDN_AGR()
        {

            WebReference.WebSDataBrCode BrServer = new WebReference.WebSDataBrCode();
            BrServer.SoapVersion = System.Web.Services.Protocols.SoapProtocolVersion.Soap12;
            BrServer.Url = set.AdressAppServer;
            BrServer.BrHeaderValue = CBrHeader.GetHeader();
            BrServer.Credentials = new NetworkCredential(CBrHeader.Login, CBrHeader.Password);
            if (BufferToBD.ModeNetTerminalB)
            {//Если мы в Онлайне
                try
                {
                    DataTable result = BrServer.POST_EU_LIST_RZDN_AGR_TYPE(listEU.ToArray(), this.RZDN, null);

                    dataGridEu.BackColor = Color.MediumAquamarine;
                    OpenNETCF.Media.SystemSounds.Beep.Play();
                }

                catch (System.Net.WebException ex)
                {
                    dataGridEu.BackColor = Color.LemonChiffon;

                    BufferToBD.ModeNetTerminalB = false;
                    CLog.WriteException("QueueTaskEU.cs", "POST_EU_LIST_RZDN_AGR", ex.ToString());
                    //Отправляем в буфер
                    BufferOper_POST_EU_LIST_RZDN_AGR Oper = new BufferOper_POST_EU_LIST_RZDN_AGR(this.RZDN, listEU.ToArray());
                    BufferToBD.BufferAdd(new BufferOperation(TypeClassBuffer.POST_EU_LIST_RZDN_AGR, Oper));
                    OpenNETCF.Media.SystemSounds.Beep.Play();
                    Thread.Sleep(100);
                    OpenNETCF.Media.SystemSounds.Beep.Play();
                }

                catch (System.Net.Sockets.SocketException ex)
                {//На случай если во время выполнения сломается связть 

                    dataGridEu.BackColor = Color.LemonChiffon;

                    BufferToBD.ModeNetTerminalB = false;
                    CLog.WriteException("WarehousePost.cs", "TaskMove", ex.ToString());
                    //Отправляем в буфер
                    BufferOper_POST_EU_LIST_RZDN_AGR Oper = new BufferOper_POST_EU_LIST_RZDN_AGR(this.RZDN, listEU.ToArray());
                    BufferToBD.BufferAdd(new BufferOperation(TypeClassBuffer.POST_EU_LIST_RZDN_AGR, Oper));
                    OpenNETCF.Media.SystemSounds.Beep.Play();
                    Thread.Sleep(100);
                    OpenNETCF.Media.SystemSounds.Beep.Play();

                }
            }
            else
            {//Если мы в Офлайне
                BufferOper_POST_EU_LIST_RZDN_AGR Oper = new BufferOper_POST_EU_LIST_RZDN_AGR(this.RZDN, listEU.ToArray());
                BufferToBD.BufferAdd(new BufferOperation(TypeClassBuffer.POST_EU_LIST_RZDN_AGR, Oper));

                dataGridEu.BackColor = Color.LemonChiffon;
                OpenNETCF.Media.SystemSounds.Beep.Play();
                Thread.Sleep(100);
                OpenNETCF.Media.SystemSounds.Beep.Play();

            }
        }

        private void buttonNext_Click_1(object sender, EventArgs e)
        {
            POST_EU_LIST_RZDN_AGR();
        }

    }
}