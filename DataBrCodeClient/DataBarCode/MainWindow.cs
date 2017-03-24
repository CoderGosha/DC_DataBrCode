using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Threading;


namespace DataBarCode
{
    public partial class MainWindow : Form
    {
        Settings set;
        StartMenu FStart;
        string Date;
        string Smena;
        public MainWindow()
        {
            InitializeComponent();

            DropDownSmena.SelectedIndex = 0;

            set = new Settings("DataBrCode.xml");
            CLog.DayToSave = set.DayToSaveLog;
            CLog.InitCLog();
            CLog.WriteInfo("DataBrCode", "StartProgram");

            textBoxDate.Text = DateTime.Now.ToString("dd.MM.yy");
            this.KeyPreview = true;

            //Заполняем поле ввода 
            SettingsTsd.ReadSettings();
            textBoxLogin.Text = SettingsTsd.USEROLD;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {

            SettingsTsd.USEROLD = textBoxLogin.Text;
            SettingsTsd.SaveSettings();
            btnLogin.Enabled = false;
            //-----------------
            CBrHeader.Login = textBoxLogin.Text;
            CBrHeader.Password = textBoxPassword.Text;
            //    Thread tr = new Thread(CheckLogin);
            //    tr.Start();
            //Сделаем асинхронный вызов
            WebReference.WebSDataBrCode BrServer = new WebReference.WebSDataBrCode();
            BrServer.SoapVersion = System.Web.Services.Protocols.SoapProtocolVersion.Soap12;
            BrServer.BrHeaderValue = CBrHeader.GetHeader();
            BrServer.Credentials = new NetworkCredential(CBrHeader.Login, CBrHeader.Password);
            BrServer.Url = set.AdressAppServer;
            try
            {
                Date = textBoxDate.Text;
                Smena = DropDownSmena.Items[DropDownSmena.SelectedIndex].ToString();

                BrServer.BeginTest_Login(AsyncCallTestLogin, BrServer);

            }

            catch (WebException ex)
            {
                btnLogin.BeginInvoke(new Action(() =>
                {
                    btnLogin.Enabled = true;
                }));

                string mes = ex.ToString();
                CLog.WriteException("MainWindow", "btnLogin_Click", ex.ToString());
                //MessageBox.Show(ex.ToString());
                if (mes.IndexOf("Unable to connect to the remote server") != -1)
                {
                    MessageBox.Show("Нет соединения с сервером: " + set.AdressAppServer);
                    return;
                }
                else
                {
                    MessageBox.Show("Введите правильный логин или пароль");
                    return;
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

                btnLogin.BeginInvoke(new Action(() =>
                {
                    btnLogin.Enabled = true;
                }));

                return;
            }
        }


        public void AsyncCallTestLogin(IAsyncResult res)
        {

            try
            {
                btnLogin.BeginInvoke(new Action(() =>
                {
                    btnLogin.Enabled = true;
                }));

                WebReference.WebSDataBrCode BrServer = res.AsyncState as WebReference.WebSDataBrCode;
                bool result = BrServer.EndTest_Login(res);
                if (result)
                {
                    FStart = new StartMenu(CBrHeader.Login, CBrHeader.Password, Date, Smena);
                    FStart.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Введите правильный логин или пароль");
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("Ошибка подключения к серверу или другая ошибка");
                CLog.WriteException("MainWindows.cs", "AsyncCallTestLogin", ex.ToString());
            }
            finally
            {
                btnLogin.BeginInvoke(new Action(() =>
                {
                    btnLogin.Enabled = true;
                }));
            }
        }


        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            CLog.WriteInfo("DataBrCode", "CloseProgram");
            Application.Exit();
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {

        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            CLog.WriteInfo("DataBrCode", "CloseProgram");
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
            else if (e.KeyCode == Keys.F14)
            {
                WiFiTest test = new WiFiTest();
                test.Show();

            }
        }

        private void buttonKeyBoard_Click(object sender, EventArgs e)
        {//Вызовем экранную клаву


        }


    }
}