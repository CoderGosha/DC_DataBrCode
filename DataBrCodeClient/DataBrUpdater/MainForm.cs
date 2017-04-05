using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DataBarCode;
using System.IO;
using Ionic.Zip;
using Terranova.API;
using System.Threading;
using System.Diagnostics;


namespace DataBrUpdater
{
    public partial class MainForm : Form
    {
        Settings set;
        bool AutoUpdate = false;
        Thread InitThUpdate;
        public MainForm(string[] args)
        {
            InitializeComponent();

            set = new Settings("DataBrCode.xml");

            foreach (string Argument in args)
            {
                if (Argument == "/AutoUpdate")
                    AutoUpdate = true;
            }
            if (AutoUpdate)
            {
                buttonClean.Enabled = false;
                buttonExit.Enabled = false;
                buttonRelease.Enabled = false;
                //Стартанем тред...
                labelloading.BeginInvoke(new Action(() =>
                {
                    labelloading.Text = "Режим автообновления";
                }));

                InitThUpdate = new Thread(AutoUpdateProcess);
                InitThUpdate.Start();
            }
        }

        private void AutoUpdateProcess()
        {
            try
            {
                //Обновляем по алгоритму,скачиваем, чистим и распаковываем...
                labelloading.BeginInvoke(new Action(() =>
                {
                    labelloading.Text = "Подключение к серверу";
                }));

                WebReference.WebSDataBrUpdater BrServer = new WebReference.WebSDataBrUpdater();
                BrServer.SoapVersion = System.Web.Services.Protocols.SoapProtocolVersion.Soap12;
                // test.BeginScanEU(EU, AsyncCallGetDataEU, test);
                labelloading.BeginInvoke(new Action(() =>
                {
                    labelloading.Text = "Загрузка файла";
                }));

                BrServer.Url = set.AdressAppServer;
                Byte[] result = BrServer.System_Get_Release();
                labelloading.BeginInvoke(new Action(() =>
                {
                    labelloading.Text = "Сохранение";
                }));

                string FileName = "DataBrCode\\" + DateTime.Now.ToString("yyMMdd-HHmm") + ".zip";
                FileStream f = File.Create(FileName);
                f.Write(result, 0, result.Length);
                f.Close();

                labelloading.BeginInvoke(new Action(() =>
                {
                    labelloading.Text = "Файл сохранен";
                }));

                //Убиваем процессы
                KillProcess();
                //
                //Запускаем чистку..
                CleanOldRelise(FileName);
                //Раззипуем теперь

                Unzip(FileName, "DataBrCode\\" + "Release");

                labelloading.BeginInvoke(new Action(() =>
                {
                    labelloading.Text = "Обновление завершено";
                }));

                if (MessageBox.Show("Обновление успешно завершено!", "Обновление", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) == DialogResult.OK)
                {
                    ProcessStartInfo processStartInfo = new ProcessStartInfo();
                    processStartInfo.FileName = @"DataBrCode\Release\DataBarCode.exe";
                    processStartInfo.WorkingDirectory = @"DataBrCode\Release\";
                    Process.Start(processStartInfo);

                    Application.Exit();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка обновления, запустите в ручном режиме", "Обновление", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            }

            finally
            {
                buttonClean.BeginInvoke(new Action(() =>
                {
                    buttonClean.Enabled = true;
                }));

                buttonRelease.BeginInvoke(new Action(() =>
                {
                    buttonRelease.Enabled = true;
                }));
                buttonExit.BeginInvoke(new Action(() =>
                {
                    buttonExit.Enabled = true;
                }));

            }
        }


        private void buttonExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void buttonDevelop_Click(object sender, EventArgs e)
        {

        }

        private void KillProcess()
        {
            try
            {
                //    ///Пробуем убить процессы
                ProcessInfo[] list = ProcessCE.GetProcesses();

                foreach (ProcessInfo pinfo in list)
                {
                    if (pinfo.FullPath.EndsWith("DataBarCode.exe"))
                        pinfo.Kill();
                }
            }

            catch (Exception ex)
            {

            }
        }

        private void CleanOldRelise(string FilePass)
        {
            foreach (var e1 in System.IO.Directory.GetFiles("DataBrCode\\"))
            {
                try
                {
                    if ((e1.IndexOf("DataBrUpdater.exe") == -1) && (e1.IndexOf("Ionic.Zip.CF.dll") == -1) && (e1.IndexOf(FilePass) == -1))
                        File.Delete(e1);
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + e1);
                    //break;
                }
            }

            foreach (var e2 in System.IO.Directory.GetDirectories("DataBrCode\\"))
            {
                try
                {
                    Directory.Delete(e2, true);
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + e2);
                    // break;
                }
            }

        }


        private void buttonLast_Click(object sender, EventArgs e)
        {//Очистка директории

            KillProcess();

            foreach (var e1 in System.IO.Directory.GetFiles("DataBrCode\\"))
            {
                try
                {
                    if ((e1.IndexOf("DataBrUpdater.exe") == -1) && (e1.IndexOf("Ionic.Zip.CF.dll") == -1))
                        File.Delete(e1);
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + e1);
                    //break;
                }
            }

            foreach (var e2 in System.IO.Directory.GetDirectories("DataBrCode\\"))
            {
                try
                {
                    Directory.Delete(e2, true);
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + e2);
                    // break;
                }
            }

            labelloading.Text = "Очистка завершена";
            buttonRelease.Enabled = true;
        }

        private void buttonRelease_Click(object sender, EventArgs e)
        {
            labelloading.BeginInvoke(new Action(() =>
            {
                labelloading.Text = "Подключение к серверу";
            }));

            WebReference.WebSDataBrUpdater BrServer = new WebReference.WebSDataBrUpdater();
            BrServer.SoapVersion = System.Web.Services.Protocols.SoapProtocolVersion.Soap12;
            // test.BeginScanEU(EU, AsyncCallGetDataEU, test);
            BrServer.Url = set.AdressAppServer;
            BrServer.BeginSystem_Get_Release(AsyncCallGet_Relise, BrServer);

            labelloading.BeginInvoke(new Action(() =>
            {
                labelloading.Text = "Соединение с сервером";
            }));
        }

        void AsyncCallGet_Relise(IAsyncResult res)
        {
            try
            {

                labelloading.BeginInvoke(new Action(() =>
                {
                    labelloading.Text = "Загрузка файла";
                }));

                WebReference.WebSDataBrUpdater BrServer = res.AsyncState as WebReference.WebSDataBrUpdater;
                // Trace.Assert(test != null, "Неверный тип объекта");
                Byte[] result = BrServer.EndSystem_Get_Release(res);

                labelloading.BeginInvoke(new Action(() =>
                {
                    labelloading.Text = "Сохранение";
                }));

                string FileName = "DataBrCode\\" + DateTime.Now.ToString("yyMMdd-HHmm") + ".zip";
                FileStream f = File.Create(FileName);
                f.Write(result, 0, result.Length);
                f.Close();

                labelloading.BeginInvoke(new Action(() =>
                {
                    labelloading.Text = "Файл сохранен";
                }));

                //Раззипуем теперь

                Unzip(FileName, "DataBrCode\\" + "Release");

                labelloading.BeginInvoke(new Action(() =>
                {
                    labelloading.Text = "Обновление завершено";
                }));


            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static void Unzip(string exFile, string exDir)
        {
            try
            {

                Encoding cp866 = Encoding.GetEncoding("cp866");

                using (ZipFile zip = ZipFile.Read(exFile))
                {
                    zip.AlternateEncoding = System.Text.Encoding.UTF8;
                    foreach (ZipEntry e in zip)
                    {
                        e.Extract(exDir, ExtractExistingFileAction.OverwriteSilently);
                    }
                }

            }
            catch (System.Exception exz)
            {
                MessageBox.Show(exz.ToString());
            }
        }
    }
}