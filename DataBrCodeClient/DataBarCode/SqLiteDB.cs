using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using System.IO;
using Ionic.Zip;
using System.Data;
using System.Net;
using System.Threading;

namespace DataBarCode
{
    static class SqLiteDB
    {

        static public string pathDB_EU = "EU.db3";
        static public string pathBD = "DataBrCode\\SQLite";
        static public string UpdateDateTime;
        static public DateTime dUpdateDateTime;
        static public string pathDBFull_EU = "";
        static public string pathDBFull_OldEU = "";
        static public bool RunUpdateBd = false;
        public static void InitSqLiteDB()
        {

            UpdateDateTime = "Нет обновления";
        }
        public static void UpdateDataBaseEU()
        {
            RunUpdateBd = true;
            Settings set;
            set = new Settings("DataBrCode.xml");
            WebReference.WebSDataBrCode BrServer = new WebReference.WebSDataBrCode();
            BrServer.SoapVersion = System.Web.Services.Protocols.SoapProtocolVersion.Soap12;
            BrServer.Url = set.AdressAppServer;
            BrServer.BrHeaderValue = CBrHeader.GetHeader();
            BrServer.Timeout = 1000 * 180;
            BrServer.Credentials = new NetworkCredential(CBrHeader.Login, CBrHeader.Password);
            Byte[] result = BrServer.DataBase_Clone_SQLiteZipFile();

            try
            {
                //Создадим директорию если ее нет
                Directory.CreateDirectory(pathBD);
            }

            catch (Exception) { }

            string FileName = pathBD + "\\" + DateTime.Now.ToString("yyMMdd-HHmmss") + ".zip";
            FileStream f = File.Create(FileName);
            f.Write(result, 0, result.Length);
            f.Close();

            //Раззипуем файл
            Unzip(FileName, pathBD);

            //Скопируем БД под новым именем
            string tmp_pathDBFull_EU = pathBD + "\\" + DateTime.Now.ToString("yyMMdd-HHmmss") + ".db3";
            System.IO.File.Copy(pathBD + "\\" + pathDB_EU, tmp_pathDBFull_EU);
            
            pathDBFull_OldEU = pathDBFull_EU;
            pathDBFull_EU = tmp_pathDBFull_EU;

            UpdateDateTime = DateTime.Now.ToString("dd.MM.yy HH:mm");
            dUpdateDateTime = DateTime.Now;
            RunUpdateBd = false;


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
                // MessageBox.Show(exz.ToString());
            }
        }

        public static void CleanOldSqlBd()
        {
            if (!RunUpdateBd)
            {//Если мы не обновляемся то->


                var List = Directory.GetFiles(pathBD);
                //Удаляем все файлы из...
                foreach (var elem in List)
                {
                    if (elem == pathDBFull_EU)
                    {//Пропуск удаления

                    }
                    else if (elem == pathDBFull_OldEU)
                    {//Пропуск удаления старого релиза

                    }
                    else if (elem == pathBD + "\\" + "EU.db3")
                    {//Пропуск удаления

                    }
                    else
                    {
                        //System.IO.File.Delete();
                        CLog.WriteInfo("CleanOldSqlBd", "Del File: " + elem);
                        try
                        {
                            System.IO.File.Delete(elem);
                        }
                        catch (Exception) { }
                    }

                }


            }

        }


        public static void AutomaticUpdateTime()
        {
            int MinuteUpdate = 3;
            if ((!RunUpdateBd) && (BufferToBD.CountBufferI <= 0))
            {//Если БД сейчас не обновляется....
                    if (dUpdateDateTime < DateTime.Now.AddMinutes(-MinuteUpdate))
                    {//Обновляем..
                    //Если менее минуты то обновляем БД
                        CLog.WriteInfo("AutomaticUpdateTime", "Start Automatic Update BD");
                        SqLiteDB.UpdateDataBaseEU();
                        Thread.Sleep(100);
                        CLog.WriteInfo("AutomaticUpdateTime", "Start Clean old BD");
                        SqLiteDB.CleanOldSqlBd();
                        CLog.WriteInfo("AutomaticUpdateTime", "Update Complite: " + pathDBFull_EU);
                    }
            }

        }
    }

    static class SqlLiteQuery
    {
      //  private static string baseName = SqLiteDB.pathDBFull_EU;

        public static string GetNameMX(string LabelMx)
        {
            string R = "Нет данных";
            using (SQLiteConnection connection = new SQLiteConnection())
            {

                ;//(SQLiteConnection)factory.CreateConnection();
                connection.ConnectionString = "Data Source = " + SqLiteDB.pathDBFull_EU;
                SQLiteCommand command = new SQLiteCommand(connection);
                SQLiteCommand insert = new SQLiteCommand("select  (select AGR.AGR_NAZ from AGR WHERE AGR.AGR_KOD = TEHUZ.AGR_KOD) AGR_NAZ, TEHUZ.TEHUZ_NAZ from TEHUZ  WHERE TEHUZ.TEHUZ_LABEL = '" + LabelMx + "';", connection);
                connection.Open();
                SQLiteDataReader reader = insert.ExecuteReader();

                while (reader.Read())
                {
                    R = _getReaderByName(reader, "AGR_NAZ") + "/" + _getReaderByName(reader, "TEHUZ_NAZ");
                }
                reader.Close();
                connection.Close();

                command.Dispose();
                insert.Dispose();
                reader.Dispose();
            }

            return R;

        }

        public static DataTable GetAgr()
        {
            DataTable R = null;
            using (SQLiteConnection connection = new SQLiteConnection())
            {

                ;//(SQLiteConnection)factory.CreateConnection();
                connection.ConnectionString = "Data Source = " + SqLiteDB.pathDBFull_EU;
                SQLiteCommand command = new SQLiteCommand(connection);
                SQLiteCommand insert = new SQLiteCommand("select * FROM agr;", connection);
                connection.Open();
                SQLiteDataReader reader = insert.ExecuteReader();
                R = new DataTable();

                R.Load(reader);
                reader.Close();

                connection.Close();

                command.Dispose();
                insert.Dispose();
                reader.Dispose();


            }

            return R;

        }
        public static DataTable GetWareHouse()
        {
            DataTable R = null;
            using (SQLiteConnection connection = new SQLiteConnection())
            {

                ;//(SQLiteConnection)factory.CreateConnection();
                connection.ConnectionString = "Data Source = " + SqLiteDB.pathDBFull_EU;
                SQLiteCommand command = new SQLiteCommand(connection);
                SQLiteCommand insert = new SQLiteCommand("select * FROM agr WHERE agr.AGR_KOD = '262' OR agr.AGR_KOD = '364' OR agr.AGR_KOD = '461';", connection);
                connection.Open();
                SQLiteDataReader reader = insert.ExecuteReader();
                R = new DataTable();

                R.Load(reader);
                reader.Close();

                connection.Close();

                command.Dispose();
                insert.Dispose();
                reader.Dispose();


            }

            return R;

        }


        public static DataTable GetPlace(string AGR, bool Shot)
        {
            DataTable R = null;
            using (SQLiteConnection connection = new SQLiteConnection())
            {

                ;//(SQLiteConnection)factory.CreateConnection();
                connection.ConnectionString = "Data Source = " + SqLiteDB.pathDBFull_EU;
                SQLiteCommand command = new SQLiteCommand(connection);

                SQLiteCommand insert = new SQLiteCommand("select TEHUZ.TEHUZ_NAZ, TEHUZ.TEHUZ_LABEL FROM  TEHUZ WHERE TEHUZ.TEHUZ_LABEL NOT LIKE 'B%' AND TEHUZ.AGR_KOD = '" + AGR + "';", connection);
                connection.Open();
                SQLiteDataReader reader = insert.ExecuteReader();

                if (Shot)
                {
                    R = new DataTable();

                    DataColumn colSource = new DataColumn("TEHUZ_NAZ", typeof(String));
                    colSource.DefaultValue = "-";
                    R.Columns.Add(colSource);
                    // colSource.Caption = "Партия";
                    DataColumn colDate = new DataColumn("TEHUZ_LABEL", typeof(String));
                    colDate.DefaultValue = "-";
                    R.Columns.Add(colDate);
                    while (reader.Read())
                    {
                        DataRow row1 = R.NewRow();
                        string s = _getReaderByName(reader, "TEHUZ_NAZ");
                        if (s.Length > 30)
                            s = s.Substring(0, 30);
                        row1["TEHUZ_NAZ"] = s;
                        row1["TEHUZ_LABEL"] = _getReaderByName(reader, "TEHUZ_LABEL");
                        R.Rows.Add(row1);
                    }
                }
                else
                {
                    R = new DataTable();
                    R.Load(reader);
                }

                reader.Close();
                connection.Close();

                command.Dispose();
                insert.Dispose();
                reader.Dispose();

            }

            return R;

        }

        public static DataTable GetMarkaUnic()
        {
            DataTable R = null;
            using (SQLiteConnection connection = new SQLiteConnection())
            {

                ;//(SQLiteConnection)factory.CreateConnection();
                connection.ConnectionString = "Data Source = " + SqLiteDB.pathDBFull_EU;
                SQLiteCommand command = new SQLiteCommand(connection);
                SQLiteCommand insert = new SQLiteCommand("select distinct(EU.MARKA_NAME) from EU WHERE EU.MARKA_NAME <> '' AND EU.SIGN='1' ORDER BY EU.MARKA_NAME", connection);
                connection.Open();
                SQLiteDataReader reader = insert.ExecuteReader();
                R = new DataTable();

                R.Load(reader);
                reader.Close();

                connection.Close();

                command.Dispose();
                insert.Dispose();
                reader.Dispose();
                //Добавим в табличку пустую запись
                DataRow row1 = R.NewRow();
                row1["MARKA_NAME"] = "-";
                R.Rows.InsertAt(row1, 0);

            }

            return R;

        }

        public static DataTable GetStandartUnic()
        {
            DataTable R = null;
            using (SQLiteConnection connection = new SQLiteConnection())
            {

                ;//(SQLiteConnection)factory.CreateConnection();
                connection.ConnectionString = "Data Source = " + SqLiteDB.pathDBFull_EU;
                SQLiteCommand command = new SQLiteCommand(connection);
                SQLiteCommand insert = new SQLiteCommand("select distinct(EU.MARKA_GOST) from EU WHERE EU.MARKA_GOST <> '' AND EU.SIGN='1' ORDER BY EU.MARKA_GOST", connection);
                connection.Open();
                SQLiteDataReader reader = insert.ExecuteReader();
                R = new DataTable();

                R.Load(reader);
                reader.Close();

                connection.Close();

                command.Dispose();
                insert.Dispose();
                reader.Dispose();
                //Добавим в табличку пустую запись
                DataRow row1 = R.NewRow();
                row1["MARKA_GOST"] = "-";
                R.Rows.InsertAt(row1, 0);

            }

            return R;

        }

        public static string GetNameMXByKod(string MXKOD)
        {
            string R = "Нет данных";
            using (SQLiteConnection connection = new SQLiteConnection())
            {

                ;//(SQLiteConnection)factory.CreateConnection();
                connection.ConnectionString = "Data Source = " + SqLiteDB.pathDBFull_EU;
                SQLiteCommand command = new SQLiteCommand(connection);
                SQLiteCommand insert = new SQLiteCommand("select  (select AGR.AGR_NAZ from AGR WHERE AGR.AGR_KOD = TEHUZ.AGR_KOD) AGR_NAZ, TEHUZ.TEHUZ_NAZ from TEHUZ  WHERE TEHUZ.TEHUZ_KOD = '" + MXKOD + "';", connection);
                connection.Open();
                SQLiteDataReader reader = insert.ExecuteReader();

                while (reader.Read())
                {
                    R = _getReaderByName(reader, "AGR_NAZ") + "/" + _getReaderByName(reader, "TEHUZ_NAZ");
                }
                reader.Close();
                connection.Close();

                command.Dispose();
                insert.Dispose();
                reader.Dispose();
            }

            return R;

        }

        public static string GetNameMXByKod(string MXKOD, bool AgrName)
        {
            string R = "Нет данных";
            using (SQLiteConnection connection = new SQLiteConnection())
            {

                ;//(SQLiteConnection)factory.CreateConnection();
                connection.ConnectionString = "Data Source = " + SqLiteDB.pathDBFull_EU;
                SQLiteCommand command = new SQLiteCommand(connection);
                SQLiteCommand insert = new SQLiteCommand("select  (select AGR.AGR_NAZ from AGR WHERE AGR.AGR_KOD = TEHUZ.AGR_KOD) AGR_NAZ, TEHUZ.TEHUZ_NAZ from TEHUZ  WHERE TEHUZ.TEHUZ_KOD = '" + MXKOD + "';", connection);
                connection.Open();
                SQLiteDataReader reader = insert.ExecuteReader();

                while (reader.Read())
                {
                    if (AgrName)
                        R = _getReaderByName(reader, "AGR_NAZ") + "/" + _getReaderByName(reader, "TEHUZ_NAZ");
                    else
                        R = _getReaderByName(reader, "TEHUZ_NAZ");
                }
                reader.Close();
                connection.Close();

                command.Dispose();
                insert.Dispose();
                reader.Dispose();
            }

            return R;

        }

        public static string GetNameAgrByMX(string MXKOD)
        {
            string R = "Нет данных";
            using (SQLiteConnection connection = new SQLiteConnection())
            {

                ;//(SQLiteConnection)factory.CreateConnection();
                connection.ConnectionString = "Data Source = " + SqLiteDB.pathDBFull_EU;
                SQLiteCommand command = new SQLiteCommand(connection);
                SQLiteCommand insert = new SQLiteCommand("select  (select AGR.AGR_NAZ from AGR WHERE AGR.AGR_KOD = TEHUZ.AGR_KOD) AGR_NAZ, TEHUZ.TEHUZ_NAZ from TEHUZ  WHERE TEHUZ.TEHUZ_KOD = '" + MXKOD + "';", connection);
                connection.Open();
                SQLiteDataReader reader = insert.ExecuteReader();

                while (reader.Read())
                {
                    R = _getReaderByName(reader, "AGR_NAZ");
                }
                reader.Close();
                connection.Close();

                command.Dispose();
                insert.Dispose();
                reader.Dispose();
            }

            return R;

        }

        private static string _getReaderByName(SQLiteDataReader rd, string NameF)
        {
            string tmp = "Нет данных";
            tmp = rd.GetValue(rd.GetOrdinal(NameF)).ToString();
            return tmp;

        }

        public static string getReaderByName(SQLiteDataReader rd, string NameF)
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



        public static string GetRZDNForLabel(string label)
        {
            string R = "Нет данных";
            using (SQLiteConnection connection = new SQLiteConnection())
            {

                ;//(SQLiteConnection)factory.CreateConnection();
                connection.ConnectionString = "Data Source = " + SqLiteDB.pathDBFull_EU;
                SQLiteCommand command = new SQLiteCommand(connection);
                SQLiteCommand insert = new SQLiteCommand("select TaskList.RZDN_PRM from TaskList WHERE TaskList.DOC_BC = '" + label + "';", connection);
                connection.Open();
                SQLiteDataReader reader = insert.ExecuteReader();

                while (reader.Read())
                {
                    R = _getReaderByName(reader, "RZDN_PRM");
                }
                reader.Close();
                connection.Close();

                command.Dispose();
                insert.Dispose();
                reader.Dispose();
            }

            return R;

        }

        public static DataTable GetScales()
        {
            DataTable R = null;
            using (SQLiteConnection connection = new SQLiteConnection())
            {

                ;//(SQLiteConnection)factory.CreateConnection();
                connection.ConnectionString = "Data Source = " + SqLiteDB.pathDBFull_EU;
                SQLiteCommand command = new SQLiteCommand(connection);
                SQLiteCommand insert = new SQLiteCommand("select * FROM VESY;", connection);
                connection.Open();
                SQLiteDataReader reader = insert.ExecuteReader();
                R = new DataTable();
                R.Load(reader);
                reader.Close();
                connection.Close();
                command.Dispose();
                insert.Dispose();
                reader.Dispose();

            }

            return R;

        }

        public static string GetMarkabyRZDN(string RZDN)
        {
            string R = "Нет данных";
            using (SQLiteConnection connection = new SQLiteConnection())
            {

                ;//(SQLiteConnection)factory.CreateConnection();
                connection.ConnectionString = "Data Source = " + SqLiteDB.pathDBFull_EU;
                SQLiteCommand command = new SQLiteCommand(connection);
                SQLiteCommand insert = new SQLiteCommand("select * from TASKLIST WHERE TASKLIST.RZDN_PRM = '" + RZDN + "';", connection);
                connection.Open();
                SQLiteDataReader reader = insert.ExecuteReader();

                while (reader.Read())
                {
                    R = _getReaderByName(reader, "TASK_MARKA");
                }
                reader.Close();
                connection.Close();

                command.Dispose();
                insert.Dispose();
                reader.Dispose();
            }

            return R;

        }

        public static List<string> GetMarkabyRZDNList(string RZDN)
        {
            List<string> R = null;
            using (SQLiteConnection connection = new SQLiteConnection())
            {
               
                ;//(SQLiteConnection)factory.CreateConnection();
                connection.ConnectionString = "Data Source = " + SqLiteDB.pathDBFull_EU;
                SQLiteCommand command = new SQLiteCommand(connection);
                SQLiteCommand insert = new SQLiteCommand("select * from RZDN_MARKA WHERE RZDN_MARKA.RZDN_PRM = '" + RZDN + "';", connection);
                connection.Open();
                SQLiteDataReader reader = insert.ExecuteReader();
                while (reader.Read())
                {
                    if (R == null)
                        R = new List<string>();

                    R.Add(_getReaderByName(reader, "MARKA").ToUpper());
                }
                reader.Close();
                connection.Close();

                command.Dispose();
                insert.Dispose();
                reader.Dispose();
            }

            return R;

        }

        public static string GetDetalisbyRZDN(string RZDN)
        {
            string R = "Нет данных";


            //А теперь проверим Марку
            //"Марка: " + _getReaderByName(reader, "TASK_MARKA") +
            using (SQLiteConnection connection = new SQLiteConnection())
            {

                ;//(SQLiteConnection)factory.CreateConnection();
                connection.ConnectionString = "Data Source = " + SqLiteDB.pathDBFull_EU;
                SQLiteCommand command = new SQLiteCommand(connection);
                SQLiteCommand insert = new SQLiteCommand("select * from RZDN_MARKA WHERE RZDN_MARKA.RZDN_PRM = '" + RZDN + "';", connection);
                connection.Open();
                SQLiteDataReader reader = insert.ExecuteReader();
                R = "";
                while (reader.Read())
                {
                    R += _getReaderByName(reader, "MARKA") + ", ";
                }
                reader.Close();
                connection.Close();

                command.Dispose();
                insert.Dispose();
                reader.Dispose();
            }
            string Razmer = "";
            using (SQLiteConnection connection = new SQLiteConnection())
            {

                ;//(SQLiteConnection)factory.CreateConnection();
                connection.ConnectionString = "Data Source = " + SqLiteDB.pathDBFull_EU;
                SQLiteCommand command = new SQLiteCommand(connection);
                SQLiteCommand insert = new SQLiteCommand("select * from TASKLIST WHERE TASKLIST.RZDN_PRM = '" + RZDN + "';", connection);
                connection.Open();
                SQLiteDataReader reader = insert.ExecuteReader();

                while (reader.Read())
                {
                    Razmer = "Размер: " + _getReaderByName(reader, "TASK_THICKNESS") + "x" + _getReaderByName(reader, "TASK_WIDTH");
                }
                reader.Close();
                connection.Close();

                command.Dispose();
                insert.Dispose();
                reader.Dispose();
            }

            R += Razmer; 
            return R;

        }

        public static Double GetWEIGHTByRZDN(string RZDN)
        {
            string R = "Нет данных";
            Double RScan = 0;
            Double RD = 0;
            using (SQLiteConnection connection = new SQLiteConnection())
            {
                //TASK_WEIGHT

                ;//(SQLiteConnection)factory.CreateConnection();
                connection.ConnectionString = "Data Source = " + SqLiteDB.pathDBFull_EU;
                SQLiteCommand command = new SQLiteCommand(connection);
                SQLiteCommand insert = new SQLiteCommand("select * from TASKLIST WHERE TASKLIST.RZDN_PRM = '" + RZDN + "';", connection);
                connection.Open();
                SQLiteDataReader reader = insert.ExecuteReader();

                while (reader.Read())
                {
                    R = _getReaderByName(reader, "TASK_WEIGHT");
                    try
                    {
                        RScan += Double.Parse(_getReaderByName(reader, "SUM_RELMUCH_VES"));
                    }

                    catch (Exception) { }
                }
                reader.Close();
                connection.Close();

                command.Dispose();
                insert.Dispose();
                reader.Dispose();
            }

            try
            {
                RD = Double.Parse(R);
                // RD = RD / 1000;
                RD = RD - RScan;
            }

            catch (Exception)
            {
                RD = 0;
            }
            return RD;

        }

    }

}
