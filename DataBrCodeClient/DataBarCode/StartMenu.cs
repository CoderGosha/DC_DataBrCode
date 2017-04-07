using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Intermec.DataCollection;
using System.Threading;
using System.Net;
using System.Diagnostics;


namespace DataBarCode
{
    public enum ListScanOperation { MXView, MXSet, EUShip, EUTaskMove, EuInAgr, TaskPGA, InventoryTask, EuInAgrTESA };

    public partial class StartMenu : Form
    {
        String LoginUser;
        String _Date;
        String _Sm;
        public Intermec.DataCollection.BarcodeReader bcr;

        Settings set;

        EU_Action UIEU = null;
        WarehousePost _WareHousePost = null;

        Thread InitThUpdate;
        bool CheckVersion = false;


        Thread UpdateLocalBd;
        bool UpdateLocalBdBool = false;
        // DataTable TblAgr = null;
        // DataTable TblWarehouse = null;

        //  public delegate void GetDataDelegate();

        public StartMenu(string _LoginUser, string _Passwords, string _Date, string _Sm)
        {
            InitializeComponent();
            ButtonEnable(false);

            set = new Settings("DataBrCode.xml");
            this.LoginUser = _LoginUser;
            this._Date = _Date;
            this._Sm = _Sm;
            this.Text = _LoginUser + " Смена:" + _Sm;

            CBrHeader.Login = _LoginUser;
            //CBrHeader.Domen = _Domen;
            CBrHeader.Password = _Passwords;


            CLog.WriteInfo("StartMenu.cs", "Enter: " + _LoginUser);

            if (!set.Emulator)
                InitScaner();

            // GetDataDelegate _Get = InitDataTabel;
            //  _Get.BeginInvoke(null, null);

            //IAsyncResult ar = InitDataTabel;
            // //BinaryOp bo = DelegateThread;
            //// AsyncCallGetDataEU(GetDataDelegate);
            Thread InitTh = new Thread(InitDataTabel);
            InitTh.Start();

            BufferToBD.InitReadBuffer();

            //  this.isInputKey = true;
            this.KeyPreview = true;

            InitThUpdate = new Thread(InitCheckVersion);
            InitThUpdate.Start();



        }


        public void ButtonEnable(bool enable)
        {
            buttonScales.BeginInvoke(new Action(() =>
            {
                buttonScales.Enabled = enable;
            }));


            buttonWarehouse.BeginInvoke(new Action(() =>
            {
                buttonWarehouse.Enabled = enable;
            }));


            buttonUpdate.BeginInvoke(new Action(() =>
            {
                buttonUpdate.Enabled = enable;
            }));


            buttonPlace.BeginInvoke(new Action(() =>
            {
                buttonPlace.Enabled = enable;
            }));

            buttonOutCGP.BeginInvoke(new Action(() =>
            {
                buttonOutCGP.Enabled = enable;
            }));

            buttonAgr.BeginInvoke(new Action(() =>
            {
                buttonAgr.Enabled = enable;
            }));


        }

        public void InitDataTabel()
        {
            try
            {
                labelStatus.BeginInvoke(new Action(() =>
                {
                    labelStatus.Text = "Синхронизация времени с сервером;";
                }));

                CBrHeader.Init();

                //  Thread.Sleep(5000);
                //Загружаем справичники
                WebReference.WebSDataBrCode BrServer = new WebReference.WebSDataBrCode();
                BrServer.SoapVersion = System.Web.Services.Protocols.SoapProtocolVersion.Soap12;
                BrServer.BrHeaderValue = CBrHeader.GetHeader();
                BrServer.Credentials = new NetworkCredential(CBrHeader.Login, CBrHeader.Password);


                BrServer.Url = set.AdressAppServer;
                DateTime NowServer = BrServer.GetServerDataTime();

                CLog.WriteInfo("StartMenu.cs", "Time Update");
                //Установим на ТСД
                OpenNETCF.WindowsCE.DateTimeHelper.SystemTime = NowServer;
                labelStatus.BeginInvoke(new Action(() =>
                {
                    labelStatus.Text = "Синхронизация данных с БД";
                }));

                SqLiteDB.InitSqLiteDB();
                SqLiteDB.UpdateDataBaseEU();
                CLog.WriteInfo("StartMenu.cs", "Data Base Update");
                //Тут правим лейбл
                string StatusBD = StatusBar.getSatus();

                labelBD.BeginInvoke(new Action(() =>
                {
                    labelBD.Text = StatusBD;
                }));

                labelStatus.BeginInvoke(new Action(() =>
                {
                    labelStatus.Text = "Синхронизация завершена";
                }));

                ButtonEnable(true);
                //OpenNETCF.WindowsCE.Notification.Led vib = new OpenNETCF.WindowsCE.Notification.Led();
                //vib.SetLedStatus(1, OpenNETCF.WindowsCE.Notification.Led.LedState.On);
               
                //Тут включить бы вибрацию.
                Sound.PlaySoundWarningVolumeHIGH();
                Thread.Sleep(100);
                Sound.PlaySoundWarningVolumeHIGH();
                Thread.Sleep(100);
                Sound.PlaySoundWarningVolumeHIGH();

                //  vib.SetLedStatus(1, OpenNETCF.WindowsCE.Notification.Led.LedState.Off);
               

                CLog.WriteInfo("StartMenu.cs", "Start Ok");


                UpdateLocalBd = new Thread(ThreadUpdateBd);
                UpdateLocalBdBool = true;
                UpdateLocalBd.Start();

            }

            catch (System.Net.WebException exp)
            {
                labelStatus.BeginInvoke(new Action(() =>
                {
                    labelStatus.Text = "Нет связи с сервером: " + set.AdressAppServer;
                }));
                CLog.WriteException("StartMenu.cs", "InitDataTabel_WEB", exp.Message);
            }

            catch (System.Web.Services.Protocols.SoapException exp)
            {
                labelStatus.BeginInvoke(new Action(() =>
                {
                    labelStatus.Text = exp.Message;
                }));
                CLog.WriteException("StartMenu.cs", "InitDataTabel_SOAP", exp.Message);
            }
            catch (Exception ex)
            {

                labelStatus.BeginInvoke(new Action(() =>
                {
                    labelStatus.Text = ex.Message;
                }));
                CLog.WriteException("StartMenu.cs", "InitDataTabel", ex.Message);
            }
        }

        public void StartManualUpdateBD()
        {
            if (!SqLiteDB.RunUpdateBd)
            {
                Thread UpdateManualLocalBd = new Thread(UpdateBd);
                UpdateManualLocalBd.Start();
            }
        }

        public void UpdateBd()
        {
            ButtonEnable(false);
            CLog.WriteInfo("UpdateBd", "Start Manual Update BD");
            if (BufferToBD.ModeNetTerminalB)
            {//Если терминал онлайн
                try
                {

                    SqLiteDB.UpdateDataBaseEU();
                }

                catch (System.Net.WebException)
                {//На случай если во время выполнения сломается связть 
                    SqLiteDB.RunUpdateBd = false;
                    BufferToBD.ModeNetTerminalB = false;

                }

                catch (System.Net.Sockets.SocketException)
                {//На случай если во время выполнения сломается связть 
                    SqLiteDB.RunUpdateBd = false;
                    BufferToBD.ModeNetTerminalB = false;

                }
                catch (Exception exe)
                {
                    //Если случилось исключение то уже выключаем режим обновления
                    SqLiteDB.RunUpdateBd = false;
                    CLog.WriteException("StartMenu.cs", "ThreadUpdateBd", exe.Message);
                }
            }
            ButtonEnable(true);

        }
        public void ThreadUpdateBd()
        {
            CLog.WriteInfo("ThreadUpdateBd", "Strat Automatic Update BD");
            while (UpdateLocalBdBool)
            {
                if (BufferToBD.ModeNetTerminalB)
                {//Если терминал онлайн
                    try
                    {
                        SqLiteDB.AutomaticUpdateTime();
                    }

                    catch (System.Net.WebException)
                    {//На случай если во время выполнения сломается связть 
                        SqLiteDB.RunUpdateBd = false;
                        BufferToBD.ModeNetTerminalB = false;

                    }


                    catch (System.Net.Sockets.SocketException)
                    {//На случай если во время выполнения сломается связть 
                        SqLiteDB.RunUpdateBd = false;
                        BufferToBD.ModeNetTerminalB = false;

                    }
                    catch (Exception exe)
                    {
                        //Если случилось исключение то уже выключаем режим обновления
                        SqLiteDB.RunUpdateBd = false;
                        CLog.WriteException("StartMenu.cs", "ThreadUpdateBd", exe.Message);
                    }
                }

                else
                {//Если связи нет производим тест..

                    BufferToBD.ModeNetTerminalB = BufferToBD.testConnect(set.AdressAppServer);
                    if (!BufferToBD.ModeNetTerminalB)
                        Thread.Sleep(1000);

                }
                //Ждем 10 сек
                Thread.Sleep(10000);
            }

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

                bcr.symbology.Code128.Enable = true;
                //set Interleaved 2 of 5
                bcr.symbology.Interleaved2Of5.Enable = false;
                //set PDF417
                bcr.symbology.Pdf417.Enable = false;

            }

            catch (Exception exp)
            {
                //MessageBox.Show(exp.Message);
                CLog.WriteException("StartMenu.cs", "InitScaner", exp.Message);
            }

        }

        public void DisposeScaner()
        {
            try
            {
                if (bcr != null)
                {
                    bcr.Dispose();
                    bcr = null;
                }

            }
            catch (Exception exp)
            {
                // MessageBox.Show(exp.Message);
                CLog.WriteException("StartMenu.cs", "DisposeScaner", exp.Message);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (!set.Emulator)
                DisposeScaner();
            CLog.WriteInfo("StartMenu.cs", "Close UI Form");
            // this.Close();
            Application.Exit();
        }

        void bcr_BarcodeRead(object sender, BarcodeReadEventArgs bre)
        {
            if (_WareHousePost != null)
            {//не обрабатываем ничего
                if (_WareHousePost.FormActive)
                    return;
            }

            try
            {
                string EU = bre.strDataBuffer;

                ///Тут Алгоритм разбора что мы все-таки считали
                ///Для начала считаем по-умолчанию что считываем мы только ЕУ и пишем алгоритм
                ///Открытия формы


                //Это открытие фрормы ЭкшенЕУ, тут так же пилим для размещения форму

                if (EU.IndexOf("MX") == 0)
                {//Это место хранения 
                    // bcr.BarcodeRead -= new BarcodeReadEventHandler(bcr_BarcodeRead);



                    //Возвращаем обработку события этого делать тут не стоит
                    //bcr.BarcodeRead += new BarcodeReadEventHandler(bcr_BarcodeRead);

                    if (_WareHousePost == null)
                    {
                        _WareHousePost = new WarehousePost(bcr, EU, ListScanOperation.MXSet, 5);
                        _WareHousePost.Show();
                    }

                    else
                    {
                        _WareHousePost.Close();
                        // UIEU.Dispose();
                        _WareHousePost = new WarehousePost(bcr, EU, ListScanOperation.MXSet, 5);
                        _WareHousePost.Show();
                    }
                }
                else if (EU.IndexOf("S") == 0)
                {//Это вагонная карта, обработчик будет потом 

                }

                else
                {

                    if (UIEU == null)
                    {
                        UIEU = new EU_Action(bcr, EU);
                        UIEU.Show();
                    }

                    else
                    {
                        UIEU.Close();
                        UIEU.Dispose();
                        UIEU = new EU_Action(bcr, EU);
                        UIEU.Show();
                    }
                }

                textBoxScan.BeginInvoke(new Action(() =>
                {
                    textBoxScan.Text = EU;
                }));

            }
            catch (Exception exp)
            {
                CLog.WriteException("StartMenu.cs", "bcr_BarcodeRead", exp.Message);
                //MessageBox.Show(exp.Message);
            }

        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            //if (!set.Emulator)
            //    DisposeScaner();

            //BufferToBD.StopReadBuffer();

            //CLog.WriteInfo("StartMenu.cs", "Close UI Form");
            //this.Close();
            if (BufferToBD.CountBufferI > 0)
            {
                //Если в буфере остались данные то спросить пользователя?!?
                if (DialogResult.OK == MessageBox.Show("Остались незавершенные операции. Вы действительно хотите выйти?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                {//Выходим
                    //
                    CLog.WriteInfo("StartMenu.cs", "User.ОК Buffer: " + BufferToBD.CountBuffer);
                    this.Close();
                }
            }
            else
            {
                this.Close();
            }

        }

        private void buttonScales_Click(object sender, EventArgs e)
        {
            //По кнопочке просто открываем форму весовую, если туда передадим параметр весов, то запуск от сканера
            bcr.BarcodeRead -= new BarcodeReadEventHandler(bcr_BarcodeRead);

            DataScales scales = new DataScales(bcr);
            DialogResult DL = scales.ShowDialog();

            //Возвращаем обработку события
            bcr.BarcodeRead += new BarcodeReadEventHandler(bcr_BarcodeRead);

        }

        private void buttonWarehouse_Click(object sender, EventArgs e)
        {
            bcr.BarcodeRead -= new BarcodeReadEventHandler(bcr_BarcodeRead);

            WarehouseSel _WareHouseSel = new WarehouseSel(bcr, LoginUser, _Date, _Sm, ListScanOperation.MXSet);
            DialogResult DL = _WareHouseSel.ShowDialog();

            //Возвращаем обработку события
            bcr.BarcodeRead += new BarcodeReadEventHandler(bcr_BarcodeRead);
        }

        private void buttonFix_Click(object sender, EventArgs e)
        {

        }

        private void InitCheckVersion()
        {

            try
            {
                Thread.Sleep(2000);
                CheckVersion = false;
                WebReference.WebSDataBrCode BrServer = new WebReference.WebSDataBrCode();
                BrServer.SoapVersion = System.Web.Services.Protocols.SoapProtocolVersion.Soap12;
                BrServer.BrHeaderValue = CBrHeader.GetHeader();
                BrServer.Credentials = new NetworkCredential(CBrHeader.Login, CBrHeader.Password);
                BrServer.Url = set.AdressAppServer;
                Double CStableVer = BrServer.GET_ACTUAL_VERSION_CLIENT();
                Double ClientVer = Double.Parse(CBrHeader.ClientVersion.Replace('.', ','));

                if (ClientVer < CStableVer)
                {//Необходимо обновить ПО
                    if (MessageBox.Show("Необходимо обновить ПО, запустить обновление?", "Обновление", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) == DialogResult.OK)
                    {
                        ProcessStartInfo processStartInfo = new ProcessStartInfo();
                        processStartInfo.FileName = @"DataBrCode\DataBrUpdater.exe";
                        processStartInfo.WorkingDirectory = @"DataBrCode\";
                        processStartInfo.Arguments = "/AutoUpdate";

                        try
                        {
                            Process.Start(processStartInfo);
                            try
                            {//Выключаем потом обновления локальной БД
                                UpdateLocalBdBool = false;
                                UpdateLocalBd.Abort();
                            }
                            catch { }

                            if (!set.Emulator)
                                DisposeScaner();

                            BufferToBD.StopReadBuffer();
                            Application.Exit();
                        }
                        catch (Exception f)
                        {
                            MessageBox.Show(f.ToString());
                        }
                    }
                }
                else if (ClientVer == CStableVer)
                {
                    labelBD.BeginInvoke(new Action(() =>
                    {
                        labelBD.Text = "Актуальная версия";
                    }));
                    Thread.Sleep(5000);
                }
                else
                {
                    labelBD.BeginInvoke(new Action(() =>
                    {
                        labelBD.Text = "Используется версия для разработчиков";
                        labelBD.ForeColor = Color.Yellow;
                    }));
                    Thread.Sleep(5000);
                }

                CheckVersion = true;
            }

            catch (Exception) { }
            timerUpdateRelise.Enabled = true;

        }

        private void timerUpdateDich_Tick(object sender, EventArgs e)
        {
            //Выводим инфушку
            try
            {
                //тут проверим один раз версию клиента
                if (!CheckVersion)
                {

                }

                else
                {
                    string StatusBD = StatusBar.getSatus();

                    labelBD.BeginInvoke(new Action(() =>
                    {
                        labelBD.Text = StatusBD;
                        labelBD.ForeColor = StatusBar.GetColorLabel();
                    }));
                }
            }

            catch (Exception) { CheckVersion = true; }

        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            StartManualUpdateBD();
        }

        private void buttonPlace_Click(object sender, EventArgs e)
        {
            bcr.BarcodeRead -= new BarcodeReadEventHandler(bcr_BarcodeRead);

            //WarehouseSel _WareHouseSel = new WarehouseSel(bcr, LoginUser, _Date, _Sm);
            //DialogResult DL = _WareHouseSel.ShowDialog();
            MenuWereHouse _MPlace = new MenuWereHouse(bcr);
            // {
            _MPlace.ShowDialog();
            // }
            //Возвращаем обработку события
            bcr.BarcodeRead += new BarcodeReadEventHandler(bcr_BarcodeRead);
        }

        private void buttonOutCGP_Click(object sender, EventArgs e)
        {
            bcr.BarcodeRead -= new BarcodeReadEventHandler(bcr_BarcodeRead);
            using (ScanWareHouse _scan = new ScanWareHouse(bcr, ListScanOperation.EUShip))
            {
                _scan.ShowDialog();
            }

            bcr.BarcodeRead += new BarcodeReadEventHandler(bcr_BarcodeRead);
        }





        private void StartMenu_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Escape)
            {
                // this.Close();
            }
            else if (e.KeyCode == Keys.F4)
            {
                if (BufferToBD.CountBufferI > 0)
                {
                    //Если в буфере остались данные то спросить пользователя?!?
                    if (DialogResult.OK == MessageBox.Show("Остались незавершенные операции. Вы действительно хотите выйти?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                    {//Выходим
                        //
                        CLog.WriteInfo("StartMenu.cs", "User.ОК Buffer: " + BufferToBD.CountBuffer);
                        this.Close();
                    }
                }
                else
                {
                    this.Close();
                }
            }

            else if (e.KeyCode == Keys.D1)
            {
                //По кнопочке просто открываем форму весовую, если туда передадим параметр весов, то запуск от сканера
                bcr.BarcodeRead -= new BarcodeReadEventHandler(bcr_BarcodeRead);

                DataScales scales = new DataScales(bcr);
                DialogResult DL = scales.ShowDialog();

                //Возвращаем обработку события
                bcr.BarcodeRead += new BarcodeReadEventHandler(bcr_BarcodeRead);
            }
            else if (e.KeyCode == Keys.D2)
            {

                bcr.BarcodeRead -= new BarcodeReadEventHandler(bcr_BarcodeRead);

                WarehouseSel _WareHouseSel = new WarehouseSel(bcr, LoginUser, _Date, _Sm, ListScanOperation.MXSet);
                DialogResult DL = _WareHouseSel.ShowDialog();

                //Возвращаем обработку события
                bcr.BarcodeRead += new BarcodeReadEventHandler(bcr_BarcodeRead);
            }
            else if (e.KeyCode == Keys.D3)
            {
                bcr.BarcodeRead -= new BarcodeReadEventHandler(bcr_BarcodeRead);
                using (ScanWareHouse _scan = new ScanWareHouse(bcr, ListScanOperation.EUShip))
                {
                    _scan.ShowDialog();
                }

                bcr.BarcodeRead += new BarcodeReadEventHandler(bcr_BarcodeRead);

            }
            else if (e.KeyCode == Keys.D4)
            {
                bcr.BarcodeRead -= new BarcodeReadEventHandler(bcr_BarcodeRead);

                //WarehouseSel _WareHouseSel = new WarehouseSel(bcr, LoginUser, _Date, _Sm);
                //DialogResult DL = _WareHouseSel.ShowDialog();
                MenuWereHouse _MPlace = new MenuWereHouse(bcr);
                // {
                _MPlace.ShowDialog();
                // }
                //Возвращаем обработку события
                bcr.BarcodeRead += new BarcodeReadEventHandler(bcr_BarcodeRead);

            }
            else if (e.KeyCode == Keys.D5)
            {


            }
            else if (e.KeyCode == Keys.F5)
            {///Обновляем БД
                StartManualUpdateBD();

            }

            else if (e.KeyCode == Keys.F13)
            {///Отправка логов
                LogUpload _lupload = new LogUpload();
                _lupload.ShowDialog();
            }

            else if (e.KeyCode == Keys.F14)
            {
                using (ServiceFunc service = new ServiceFunc())
                {
                    service.ShowDialog();
                }

            }

            else if (e.KeyCode == Keys.F16)
            {
                bool rezult = ScreenShot.MakeShot("LogUpdate");
                if (rezult)
                    MessageBox.Show("Снимок успешно сохранен", "ScreenShot", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
                else
                    MessageBox.Show("Ошибка сохранения", "ScreenShot", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);

            }


        }



        private void buttonAgr_Click_1(object sender, EventArgs e)
        {
            bcr.BarcodeRead -= new BarcodeReadEventHandler(bcr_BarcodeRead);
            using (MenuAgr _agr = new MenuAgr(bcr, ListScanOperation.EuInAgr))
            {
                _agr.ShowDialog();
            }

            bcr.BarcodeRead += new BarcodeReadEventHandler(bcr_BarcodeRead);
        }

        private void timerUpdateRelise_Tick(object sender, EventArgs e)
        {
            //try
            //{
            //    if (InitThUpdate != null)
            //        InitThUpdate.Start();
            //    else
            //    {
            //        InitThUpdate = new Thread(InitCheckVersion);
            //        InitThUpdate.Start();
            //    }
            //}

            //catch (Exception exe)
            //{

            //    CLog.WriteException("StartMenu.cs", "timerUpdateRelise_Tick", exe.ToString());
            //}
        }

        private void StartMenu_Closed(object sender, EventArgs e)
        {
            try
            {//Выключаем потом обновления локальной БД
                UpdateLocalBdBool = false;
                UpdateLocalBd.Abort();
            }
            catch { }

            if (!set.Emulator)
                DisposeScaner();

            BufferToBD.StopReadBuffer();

            CLog.WriteInfo("StartMenu.cs", "Close UI Form");

        }





    }
}