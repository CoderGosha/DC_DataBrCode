﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;
using Ionic.Zip;

namespace DataBarCode
{
    public partial class ServiceFunc : Form
    {
        Settings set;
        List<string> blacklistApp;
        public ServiceFunc()
        {
            InitializeComponent();

            set = new Settings("DataBrCode.xml");


            labelVersion.Text = "Версия ПО: " + CBrHeader.ClientVersion;

            if (CheckAutostart())
            {
                labelAutoStart.Text = "Автостарт: " + "Ok";
            }
            else
            {
                labelAutoStart.Text = "Автостарт: " + "Fail";
            }

            if (CheckLink())
            {
                labellnk.Text = "Ярлыки: " + "Ok";
            }
            else
            {
                labellnk.Text = "Ярлыки: " + "Fail";
            }
            blacklistApp = new List<string>();
            CheckSecurity();
            LogFilesProgram();
        }

        private void ButtonEnabled(bool enable)
        {
            buttonAppOn.BeginInvoke(new Action(() =>
            {
                buttonAppOn.Enabled = enable;
            }));

            buttonAutostart.BeginInvoke(new Action(() =>
            {
                buttonAutostart.Enabled = enable;
            }));

            buttonLnk.BeginInvoke(new Action(() =>
            {
                buttonLnk.Enabled = enable;
            }));

            buttonAppOFF.BeginInvoke(new Action(() =>
            {
                buttonAppOFF.Enabled = enable;
            }));
        }

        private void CheckSecurity()
        {
            ButtonEnabled(false);
            //Запускаем ассинхронную проверку с сервера
            labelStatus.BeginInvoke(new Action(() =>
            {
                labelStatus.Text = "Проверка безопасности";
            }));

            WebReference.WebSDataBrCode BrServer = new WebReference.WebSDataBrCode();
            BrServer.SoapVersion = System.Web.Services.Protocols.SoapProtocolVersion.Soap12;
            BrServer.Url = set.AdressAppServer;
            BrServer.BrHeaderValue = CBrHeader.GetHeader();
            BrServer.Credentials = new NetworkCredential(CBrHeader.Login, CBrHeader.Password);
            BrServer.BeginTest_Login_Admin(AsyncCallCheckSecurity, BrServer);

        }

        public void AsyncCallCheckSecurity(IAsyncResult res)
        {

            try
            {
                WebReference.WebSDataBrCode BrServer = res.AsyncState as WebReference.WebSDataBrCode;
                bool result = BrServer.EndTest_Login_Admin(res);
                if (result)
                {
                    labelStatus.BeginInvoke(new Action(() =>
                    {
                        labelStatus.Text = "Доступ разрешен";
                    }));
                    CLog.WriteInfo("ServiceFunc.cs", "Authorization ok");
                    ButtonEnabled(true);
                }
                else
                {
                    labelStatus.BeginInvoke(new Action(() =>
                    {
                        labelStatus.Text = "Доступ запрещен";
                    }));
                    CLog.WriteInfo("ServiceFunc.cs", "Authorization Fail");

                }

            }

            catch (Exception ex)
            {
                CLog.WriteException("ServiceFunc.cs", "AsyncCallFixedWeight", ex.Message);
                labelStatus.BeginInvoke(new Action(() =>
                {//
                    labelStatus.Text = "Доступ запрещен";
                }));
            }

            WebReference.WebSDataBrCode BrSer = new WebReference.WebSDataBrCode();
            BrSer.SoapVersion = System.Web.Services.Protocols.SoapProtocolVersion.Soap12;
            BrSer.Url = set.AdressAppServer;
            BrSer.BrHeaderValue = CBrHeader.GetHeader();
            BrSer.Credentials = new NetworkCredential(CBrHeader.Login, CBrHeader.Password);
            BrSer.BeginBLACKLISTAPP(AsyncCallBlackList, BrSer);

        }

        public void AsyncCallBlackList(IAsyncResult res)
        {

            try
            {
                WebReference.WebSDataBrCode BrServer = res.AsyncState as WebReference.WebSDataBrCode;
                blacklistApp = BrServer.EndBLACKLISTAPP(res).ToList();
                labelStatus.BeginInvoke(new Action(() =>
                {
                    labelStatus.Text = "BlackList - ok";
                }));

            }

            catch (Exception ex)
            {
                labelStatus.BeginInvoke(new Action(() =>
                {
                    labelStatus.Text = "BlackList - fail";
                }));
                CLog.WriteException("ServiceFunc.cs", "AsyncCallBlackList", ex.Message);

            }
        }


        private void ServiceFunc_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
            else if (e.KeyCode == Keys.F9)
            {

            }

            else if (e.KeyCode == Keys.E)
            {

            }
        }

        private void CreateAutostart()
        {//Удаляем все что есть в папке с автзапуском

            try
            {
                File.Copy("DataBrCode\\Release\\LnkDataBarCode.lnk", "Windows\\Автозагрузка\\DataBarCode.lnk", true);
            }
            catch (Exception ex)
            {
                CLog.WriteException("ServiceFunc", "CreateAutostart", ex.ToString());
            }

        }

        private void CreateLnk()
        {//Удаляем все что есть в папке с автзапуском

            try
            {
                File.Copy("DataBrCode\\Release\\LnkDataBarCode.lnk", "Windows\\Главное меню\\Программы\\DataBarCode.lnk", true);
            }
            catch (Exception ex)
            {
                CLog.WriteException("ServiceFunc", "CreateAutostart", ex.ToString());
            }

            try
            {
                File.Copy("DataBrCode\\Release\\LnkDataBarUpdater.lnk", "Windows\\Главное меню\\Программы\\DataBarUpdater.lnk", true);
            }
            catch (Exception ex)
            {
                CLog.WriteException("ServiceFunc", "CreateAutostart", ex.ToString());
            }

        }

        private bool CheckAutostart()
        {
            bool find = false;

            foreach (var elem in Directory.GetFiles("Windows\\Автозагрузка"))
            {
                if (elem.IndexOf("DataBarCode.lnk") != -1)
                {
                    find = true;
                }
                CLog.WriteException("ServiceFunc", "CheckAutostart", elem);
            }
            return find;
        }


        private bool CheckLink()
        {
            int i = 0;
            foreach (var elem in Directory.GetFiles("Windows\\Главное меню\\Программы"))
            {
                if (elem.IndexOf("DataBarCode.lnk") != -1)
                {
                    i++;
                }
                else if (elem.IndexOf("DataBarUpdater.lnk") != -1)
                {
                    i++;
                }
                CLog.WriteException("ServiceFunc", "CheckAutostart", elem);
            }

            if (i == 2)
                return true;
            else
                return false;

        }

        private void buttonAutostart_Click(object sender, EventArgs e)
        {
            if (!CheckAutostart())
            {
                CreateAutostart();
            }

            //И еще раз проверим
            if (CheckAutostart())
            {
                labelAutoStart.Text = "Автостарт: " + "Ok";
            }
            else
            {
                labelAutoStart.Text = "Автостарт: " + "Fail";
            }

        }

        private void buttonLnk_Click(object sender, EventArgs e)
        {
            if (!CheckLink())
            {
                CreateLnk();
            }

            if (CheckLink())
            {
                labellnk.Text = "Ярлыки: " + "Ok";
            }
            else
            {
                labellnk.Text = "Ярлыки: " + "Fail";
            }
        }

        private void restoreFiles(){
            string FolderBackup = "TempFilesBackup";

            foreach (var elem in Directory.GetFiles(FolderBackup))
            {
                try
                {
                    //Определим имя файла для копирования
                    File.Copy(elem, "Windows\\Главное меню\\Программы\\" + GetFileName(elem), true);
                   // BackUpFile++;
                }
                catch (Exception ex)
                {
                    CLog.WriteException("ServiceFunc", "restoreFiles", ex.ToString());
                }

            }

            foreach (var elem in Directory.GetDirectories(FolderBackup))
            {
                try
                {
                    Directory.CreateDirectory("Windows\\Главное меню\\Программы\\" + "\\" + GetFileName(elem));

                    foreach (var e in Directory.GetFiles(elem))
                    {
                        try
                        {
                            File.Copy(e, "Windows\\Главное меню\\Программы\\" + "\\" + GetFileName(elem) + "\\" + GetFileName(e), true);
                            //BackUpFile++;
                        }
                        catch (Exception eeee)
                        {

                        }

                    }
                }
                catch (Exception ex)
                {

                }

            }


        }

        private void BackupFile()
        {
            string FolderBackup = "TempFilesBackup";
            int BackUpFile = 0;
            try
            {
                Directory.CreateDirectory(FolderBackup);
            }
            catch (Exception ex)
            {

            }

            foreach (var elem in Directory.GetFiles("Windows\\Главное меню\\Программы\\"))
            {
                try
                {
                    //Определим имя файла для копирования
                    File.Copy(elem, FolderBackup + "\\" + GetFileName(elem), true);
                    //BackUpFile++;
                }
                catch (Exception ex)
                {
                    CLog.WriteException("ServiceFunc", "buttonAppOFF_Click", ex.ToString());
                }

            }

            foreach (var elem in Directory.GetDirectories("Windows\\Главное меню\\Программы\\"))
            {
                try
                {
                    Directory.CreateDirectory(FolderBackup + "\\" + GetFileName(elem));

                    foreach (var e in Directory.GetFiles(elem))
                    {
                        try
                        {
                            File.Copy(e, FolderBackup + "\\" + GetFileName(elem) + "\\" + GetFileName(e), true);
                            BackUpFile++;
                        }
                        catch (Exception eeee)
                        {

                        }

                    }
                }
                catch (Exception ex)
                {

                }

            }

        }

        private void buttonAppOn_Click(object sender, EventArgs e)
        {

            restoreFiles();
            // Unzip("TempFilesBackup\\Links.zip", @"Windows\Главное меню\Программы");

            labelStatus.BeginInvoke(new Action(() =>
            {
                labelStatus.Text = "Restore files";
            }));
        }



        private void buttonAppOFF_Click(object sender, EventArgs e)
        {
            ///Выключаем ярлыки, для уверенности копируем их в куда-то...
            ///
            if (blacklistApp == null)
                return;
            BackupFile();

            int DelFile = DeleteFileList(blacklistApp);

            labelStatus.BeginInvoke(new Action(() =>
            {
                labelStatus.Text = "Delete files: " + DelFile.ToString();
            }));

        }
        private string GetFileName(string pathFile)
        {
            int LIndex = pathFile.LastIndexOf("\\");
            return pathFile.Substring(LIndex + 1);
        }

        private int DeleteFileList(List<String> LDelete)
        {
            int DeleteFile = 0;
            foreach (var elem in LDelete)
            {
                try
                {
                    if (elem.IndexOf(".lnk") == -1)
                        Directory.Delete(elem, true);
                    //Определим имя файла для копирования
                    else
                        File.Delete(elem);
                    DeleteFile++;
                }
                catch (Exception ex)
                {
                    CLog.WriteException("ServiceFunc", "DeleteFileList", ex.ToString());
                }

            }
            return DeleteFile;
        }

        private void LogFilesProgram()
        {
            foreach (var elem in Directory.GetFiles("Windows\\Главное меню\\Программы\\"))
            {
                try
                {
                    CLog.WriteInfo("ServiceFunc", "LogFilesProgram " + elem);
                }
                catch (Exception ex)
                {

                }

            }

            foreach (var elem in Directory.GetDirectories("Windows\\Главное меню\\Программы\\"))
            {
                try
                {
                    CLog.WriteInfo("ServiceFunc", "LogFilesProgram Dir:" + elem);
                    foreach (var e in Directory.GetFiles(elem))
                    {
                        try
                        {
                            CLog.WriteInfo("ServiceFunc", "LogFilesProgram " + e);
                        }
                        catch (Exception eeee)
                        {

                        }

                    }
                }
                catch (Exception ex)
                {

                }

            }
        }

    }
}