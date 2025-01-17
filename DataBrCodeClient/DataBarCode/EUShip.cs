﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;
using Intermec.DataCollection;
using System.Threading;
using GridSample;
using System.Net;

namespace DataBarCode
{
    public partial class EUShip : Form
    {
        public string TN;
        public DataTable _tblEU;
        public bool FormActive = true;
        private Intermec.DataCollection.BarcodeReader bcr;
        public List<WebReference.Relmuch> listEU = null;
        Settings set;
        public enum StatusScan { Ok, Fail, Init, Buffer };

        public EUShip(Intermec.DataCollection.BarcodeReader _bcr, string _TN)
        {
            InitializeComponent();

            set = new Settings("DataBrCode.xml");


            TN = _TN;

            ///TODO На тестирование
            ///
            bcr = _bcr;



            labelCaption.Text = TN;
            try
            {
                InitScaner();
            }

            catch (Exception)
            {

            }
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
            //CreateTable();

            //  

            CreateColumn("ФОК", "ФОК", 160, 0);
           // CreateColumn("Марка", "Марка", 120, 1);
            CreateColumn("Размер", "Размер", 140, 1);
            CreateColumn("МХ", "МХ", 400, 2);
            // CreateColumn("Select", "Select", 40, 3);
            try
            {
                GetDataForDoc(TN);
            }

            catch (Exception exp)
            {
                CLog.WriteException("EUShip.cs", "Init", exp.Message);
            }

            SetColorBackGround(StatusScan.Ok);
            listEU = new List<WebReference.Relmuch>();
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
                e.MeetsCriteria = false;

                for (int i = 0; i < _tblEU.Rows.Count; i++)
                {

                    if (YE == _tblEU.Rows[i]["ФОК"].ToString())
                    {
                        //Если это нужная
                        if (_tblEU.Rows[i]["Select"].ToString() == "1")
                        {
                            e.MeetsCriteria = true;
                            break;
                        }
                    }

                }

            }

            catch (Exception)
            {
                e.MeetsCriteria = false;
            }
        }



        private void InitScaner()
        {

            if (bcr == null)
                bcr = new BarcodeReader();
            //set BarcodeRead event
            bcr.BarcodeRead += new BarcodeReadEventHandler(bcr_BarcodeReadEUShip);
            //sends the BarcodeRead event after each successful read
            bcr.ThreadedRead(true);
            bcr.symbology.Code128.Enable = false;
            //set Interleaved 2 of 5
            bcr.symbology.Interleaved2Of5.Enable = false;
            //set PDF417
            bcr.symbology.Pdf417.Enable = false;
            bcr.symbology.QrCode.Enable = false;
        }


        void bcr_BarcodeReadEUShip(object sender, BarcodeReadEventArgs bre)
        {

            try
            {
                string EU = bre.strDataBuffer;

                ///Тут Алгоритм разбора что мы все-таки считали
                ///Для начала считаем по-умолчанию что считываем мы только ЕУ и пишем алгоритм
                ///Открытия формы

                if (EU.IndexOf("MX") == 0)
                {//
                    return;
                }


                ///Надем ЕУ в табилице и сигнализируем и иначе тоже сигназизируем
                ///
                bool find = false;
                for (int i = 0; i < _tblEU.Rows.Count; i++)
                {
                    string l = _tblEU.Rows[i]["Label"].ToString();
                    if (l == EU)
                    {
                        find = true;
                        _tblEU.Rows[i]["Select"] = "1";

                        //Добавляем УЕ в список
                        WebReference.Relmuch EUT = new WebReference.Relmuch();
                        EUT.LABEL = EU;
                        EUT.CODEAUTOMATIC = 5;
                        listEU.Add(EUT);
                        break;
                    }
                }

                int v = OpenNETCF.Media.SystemSound.GetVolume();
                OpenNETCF.Media.SystemSound.SetVolume(100);
                if (find)
                {

                    Thread.Sleep(500);
                    OpenNETCF.Media.SystemSounds.Beep.Play();
                    SetColorBackGround(StatusScan.Ok);
                }
                else
                {
                    Thread.Sleep(500);
                    OpenNETCF.Media.SystemSounds.Beep.Play();
                    Thread.Sleep(100);
                    OpenNETCF.Media.SystemSounds.Beep.Play();
                    SetColorBackGround(StatusScan.Fail);
                }
                OpenNETCF.Media.SystemSound.SetVolume(v);

            }
            catch (Exception exp)
            {
                CLog.WriteException("EUShip.cs", "bcr_BarcodeReadEUShip", exp.Message);
                //MessageBox.Show(exp.Message);
            }

        }


        private void GetDataForDoc(string Doc)
        {

            _tblEU = new DataTable("EU");


            using (SQLiteConnection connection = new SQLiteConnection())
            {

                ;//(SQLiteConnection)factory.CreateConnection();
                connection.ConnectionString = "Data Source = " + SqLiteDB.pathDBFull_EU;
                SQLiteCommand command = new SQLiteCommand(connection);
                //

                ///select EU.RELMUCH_LABEL 'Label', EU.RPRT_NOM 'ФОК', EU.MARKA_NAME 'Марка',  printf('%sx%s', EU.RELMUCH_THICKNESS, EU.RELMUCH_WIDTH) 'Размер' , (
                //select TEHUZ.TEHUZ_NAZ  FROM TEHUZ WHERE TEHUZ.TEHUZ_KOD = EU.TEHUZ_KOD) 'МХ'
                // FROM TaskListEU, EU, TaskList WHERE TaskListEU.RELMUCH_PRM = EU.RELMUCH_PRM AND TaskList.RZDN_PRM = TaskListEU.RZDN_PRM  AND TaskList.DOC_BC =  'S0001500000331052016'
                SQLiteCommand insert = new SQLiteCommand("select EU.RELMUCH_LABEL 'Label', EU.RPRT_NOM 'ФОК', EU.RELMUCH_PROFILE 'Размер' , (select TEHUZ.TEHUZ_NAZ  FROM TEHUZ WHERE TEHUZ.TEHUZ_KOD = EU.TEHUZ_KOD) 'МХ' FROM TaskListEU, EU, TaskList WHERE TaskListEU.RELMUCH_PRM = EU.RELMUCH_PRM AND TaskList.RZDN_PRM = TaskListEU.RZDN_PRM  AND TaskList.DOC_BC = '" + Doc + "' LIMIT 50;", connection);
                connection.Open();
                SQLiteDataReader reader = insert.ExecuteReader();
                _tblEU.Load(reader);
                reader.Close();
                connection.Close();

                command.Dispose();
                insert.Dispose();
                reader.Dispose();

                DataColumn colSelect = new DataColumn("Select", typeof(String));
                colSelect.DefaultValue = "0";
                _tblEU.Columns.Add(colSelect);
            }

            dataGridEu.BeginInvoke(new Action(() =>
            {
                dataGridEu.DataSource = _tblEU;
            }));


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

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }



        private void buttonNext_Click(object sender, EventArgs e)
        {
            SetShip();
        }

        private void SetShip()
        {

            WebReference.WebSDataBrCode BrServer = new WebReference.WebSDataBrCode();
            BrServer.SoapVersion = System.Web.Services.Protocols.SoapProtocolVersion.Soap12;
            BrServer.Url = set.AdressAppServer;
            BrServer.BrHeaderValue = CBrHeader.GetHeader();
            BrServer.Credentials = new NetworkCredential(CBrHeader.Login, CBrHeader.Password);
            //Найдем задание 
            string RZDN = SqlLiteQuery.GetRZDNForLabel(TN);
            //Необходимо перенести список на этап сканирования
            ////Спискок ЕУ запилим

            //listEU = new List<WebReference.Relmuch>();

            //for (int i = 0; i < _tblEU.Rows.Count; i++)
            //{
            //    if (_tblEU.Rows[i]["Select"].ToString() == "1")
            //    {
            //        //Вот тут то и дичка
            //        listEU.Add(_tblEU.Rows[i]["Label"].ToString());
            //    }

            //}


            if (BufferToBD.ModeNetTerminalB)
            {//Если мы в Онлайне
                try
                {
                    DataTable result = BrServer.POST_EU_LIST_SHIP_TYPE(listEU.ToArray(), RZDN, null);

                    dataGridEu.BackColor = Color.LightGray;
                    OpenNETCF.Media.SystemSounds.Beep.Play();

                    ////Далее нужен алгоритм обработки ответа

                    //foreach (DataRow dr in dataGridEu.Rows)
                    //{

                    //}
                    SetColorBackGround(StatusScan.Ok);
                }

                catch (System.Net.WebException ex)
                {
                    dataGridEu.BackColor = Color.LemonChiffon;

                    BufferToBD.ModeNetTerminalB = false;
                    CLog.WriteException("EUSHip.cs", "SetShip", ex.ToString());
                    //Отправляем в буфер
                    BufferOper_POST_EU_LIST_SHIP Oper = new BufferOper_POST_EU_LIST_SHIP(RZDN, listEU.ToArray());
                    BufferToBD.BufferAdd(new BufferOperation(TypeClassBuffer.POST_EU_LIST_SHIP, Oper));
                    OpenNETCF.Media.SystemSounds.Beep.Play();
                    Thread.Sleep(100);
                    OpenNETCF.Media.SystemSounds.Beep.Play();

                    SetColorBackGround(StatusScan.Buffer);
                }

                catch (System.Net.Sockets.SocketException ex)
                {//На случай если во время выполнения сломается связть 

                    dataGridEu.BackColor = Color.LemonChiffon;

                    BufferToBD.ModeNetTerminalB = false;
                    CLog.WriteException("EUSHip.cs", "SetShip", ex.ToString());
                    //Отправляем в буфер
                    BufferOper_POST_EU_LIST_SHIP Oper = new BufferOper_POST_EU_LIST_SHIP(RZDN, listEU.ToArray());
                    BufferToBD.BufferAdd(new BufferOperation(TypeClassBuffer.POST_EU_LIST_SHIP, Oper));
                    OpenNETCF.Media.SystemSounds.Beep.Play();
                    Thread.Sleep(100);
                    OpenNETCF.Media.SystemSounds.Beep.Play();

                    SetColorBackGround(StatusScan.Buffer);
                }
            }
            else
            {//Если мы в Офлайне
                BufferOper_POST_EU_LIST_SHIP Oper = new BufferOper_POST_EU_LIST_SHIP(RZDN, listEU.ToArray());
                BufferToBD.BufferAdd(new BufferOperation(TypeClassBuffer.POST_EU_LIST_SHIP, Oper));

                dataGridEu.BackColor = Color.LemonChiffon;
                OpenNETCF.Media.SystemSounds.Beep.Play();
                Thread.Sleep(100);
                OpenNETCF.Media.SystemSounds.Beep.Play();

                SetColorBackGround(StatusScan.Buffer);

            }
            //Анализируем результат, и подсветку делаем строк
        }
        private bool CheckEUComplite()
        {
            bool BReturn = true;

            for (int i = 0; i < _tblEU.Rows.Count; i++)
            {
                if ((_tblEU.Rows[i]["Select"].ToString() == "0") || (_tblEU.Rows[i]["Select"].ToString() == "-1"))
                {
                    BReturn = false;
                    break;
                }
            }

            return BReturn;
        }


        private void EUShip_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {

                //Проверка на операции в списке
                if (!CheckEUComplite())
                {
                    //Если в буфере остались данные то спросить пользователя?!?
                    if (DialogResult.OK == MessageBox.Show("Остались незавершенные операции. Вы действительно хотите выйти?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                    {//Выходим
                        this.Close();
                    }
                }
                else
                {
                    this.Close();
                }
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
                        //Значит что то выбрали и есть что вставить
                        bool find = false;
                        List<CommonType.SelectEU> SelList = search.GetSelectedEU();
                        if (SelList != null)
                        {
                            foreach (var elem in SelList)
                            {
                                string Label = "0";
                                Label = elem.Label;

                                for (int i = 0; i < _tblEU.Rows.Count; i++)
                                {
                                    string l = _tblEU.Rows[i]["Label"].ToString();
                                    if (l == Label)
                                    {
                                        find = true;
                                        _tblEU.Rows[i]["Select"] = "1";
                                        //Добавляем УЕ в список
                                        WebReference.Relmuch EUT = new WebReference.Relmuch();
                                        EUT.LABEL = elem.Label;
                                        EUT.CODEAUTOMATIC = 3;
                                        listEU.Add(EUT);
                                        
                                        break;
                                    }
                                }
                            }

                        }

                        if (find)
                        {
                            Thread.Sleep(200);
                            OpenNETCF.Media.SystemSounds.Beep.Play();
                            SetColorBackGround(StatusScan.Ok);
                        }
                        else
                        {
                            Thread.Sleep(200);
                            OpenNETCF.Media.SystemSounds.Beep.Play();
                            Thread.Sleep(100);
                            OpenNETCF.Media.SystemSounds.Beep.Play();
                            SetColorBackGround(StatusScan.Fail);
                        }
                    }
                }
            }

            else if (e.KeyCode == Keys.F9)
            {
                SetShip();
            }
        }



        private void buttonEUSearch_Click_1(object sender, EventArgs e)
        {
            EUSearch search = new EUSearch();
            DialogResult DL = search.ShowDialog();
            if (DL == DialogResult.OK)
            {
                if (search._tblEU.Rows.Count > 0)
                {
                    //Значит что то выбрали и есть что вставить
                    bool find = false;
                    List<CommonType.SelectEU> SelList = search.GetSelectedEU();
                    if (SelList != null)
                    {
                        foreach (var elem in SelList)
                        {
                            string Label = "0";
                            Label = elem.Label;

                            for (int i = 0; i < _tblEU.Rows.Count; i++)
                            {
                                string l = _tblEU.Rows[i]["Label"].ToString();
                                if (l == Label)
                                {
                                    find = true;
                                    _tblEU.Rows[i]["Select"] = "1";
                                    //Добавляем УЕ в список
                                    WebReference.Relmuch EUT = new WebReference.Relmuch();
                                    EUT.LABEL = elem.Label;
                                    EUT.CODEAUTOMATIC = 3;
                                    listEU.Add(EUT);
                                    break;
                                }
                            }
                        }
                    }

                    if (find)
                    {
                        Thread.Sleep(200);
                        OpenNETCF.Media.SystemSounds.Beep.Play();
                        SetColorBackGround(StatusScan.Ok);
                    }
                    else
                    {
                        Thread.Sleep(200);
                        OpenNETCF.Media.SystemSounds.Beep.Play();
                        Thread.Sleep(100);
                        OpenNETCF.Media.SystemSounds.Beep.Play();
                        SetColorBackGround(StatusScan.Fail);
                    }
                }
            }
        }



        private void SetColorBackGround(StatusScan _status)
        {

            switch (_status)
            {
                case StatusScan.Init:
                    {//При инициализации выберем фон серый 
                        this.BeginInvoke(new Action(() =>
                        {
                            this.BackColor = Color.Honeydew;
                        }));
                        break;
                    }

                case StatusScan.Buffer:
                    {//При инициализации выберем фон серый 
                        this.BeginInvoke(new Action(() =>
                        {
                            this.BackColor = Color.MediumAquamarine;
                        }));
                        break;
                    }
                case StatusScan.Ok:
                    {//Если все ок то выберем фон зеленый
                        this.BeginInvoke(new Action(() =>
                        {
                            this.BackColor = Color.MediumAquamarine;
                        }));
                        break;
                    }
                case StatusScan.Fail:
                    {
                        this.BeginInvoke(new Action(() =>
                        {
                            this.BackColor = Color.LightCoral;
                        }));
                        break;
                    }
            }

        }

        private void EUShip_Closed(object sender, EventArgs e)
        {
            bcr.BarcodeRead -= new BarcodeReadEventHandler(bcr_BarcodeReadEUShip);
            bcr.symbology.Code128.Enable = true;
            FormActive = false;
        }
    }
}