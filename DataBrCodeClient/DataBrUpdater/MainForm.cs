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


namespace DataBrUpdater
{
    public partial class MainForm : Form
    {
        Settings set;

        public MainForm()
        {
            InitializeComponent();

            set = new Settings("DataBrCode.xml");

        }



        private void buttonExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void buttonDevelop_Click(object sender, EventArgs e)
        {

        }

        private void buttonLast_Click(object sender, EventArgs e)
        {//Очистка директории

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