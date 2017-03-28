using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace DataBarCode
{
    public partial class ServiceFunc : Form
    {
        public ServiceFunc()
        {
            InitializeComponent();
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

    }
}