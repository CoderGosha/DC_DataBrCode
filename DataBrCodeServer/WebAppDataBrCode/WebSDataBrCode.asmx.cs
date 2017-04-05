using DataBarCode;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OracleClient;
using System.Data.SQLite;
using System.Data.SqlClient;
using System.Data.SQLite.EF6;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using Ionic.Zip;
using Ionic.Zlib;
using System.Net;
using System.Text;
using System.Web.Services.Protocols;
using System.Security.Principal;
using System.Configuration;
using System.Threading;
using System.Security.Cryptography;

namespace WebAppDataBrCode
{
    /// <summary>
    /// Сводное описание для WebSDataBrCode
    /// </summary>
    [WebService(Namespace = "WebSDataBrCode")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Чтобы разрешить вызывать веб-службу из скрипта с помощью ASP.NET AJAX, раскомментируйте следующую строку. 
    // [System.Web.Script.Services.ScriptService]

    public class WebSDataBrCode : System.Web.Services.WebService
    {
        static DateTime CleanDataBase;
        static int CleanMinutesDataBase = 10;
        static bool RunCleanDataBase = false;
        public WebSDataBrCode()
        {
            if (CleanDataBase < DateTime.Now.AddMinutes(-CleanMinutesDataBase))
            {
                if (!RunCleanDataBase)
                {
                    RunCleanDataBase = true;
                    //AddAllLog("WebSDataBrCode", "Starting WebService");
                    //Стартанем поток для чистки ЕУ
                    Thread cln = new Thread(CleanOldSqlBd);
                    cln.Start();
                    CleanDataBase = DateTime.Now;
                    RunCleanDataBase = false;
                }
            }
        }

        WindowsIdentity winId = (WindowsIdentity)HttpContext.Current.User.Identity;
        WindowsImpersonationContext ctx = null;

        public class TInfo
        {
            public string Login { get; set; }
            public string Password { get; set; }
            public string MacAdress { get; set; }
            public string DnsAdress { get; set; }
            public string ClientVersion { get; set; }
            public TInfo()
            {
                Login = "-";
                Password = "-";
                MacAdress = "-";
                DnsAdress = "unknow";
                ClientVersion = "v0.0";
            }

        }
        public class BrHeader : SoapHeader
        {
            public string Login { get; set; }
            public string Password { get; set; }
            public string MacAdress { get; set; }
            public string DnsAdress { get; set; }
            public string ClientVersion { get; set; }
        }

        public BrHeader brHeader;
        // Receive all SOAP headers other than the MyHeader SOAP header.
        public SoapUnknownHeader[] unknownHeaders;



        public void AddAllLog(string _service, string _message)
        {
            SLogWeb.Add(new LogElem(_service, _message));
            CLog.WriteInfo(_service, _message);
        }

        public void AddAllLog(TInfo tInfo, string _service, string _message)
        {
            var request = this.Context.Request;
            string IP = request.UserHostAddress;
            SLogWeb.Add(new LogElem(tInfo.Login, tInfo.Password, tInfo.ClientVersion, tInfo.MacAdress, IP, tInfo.DnsAdress, _service, _message));
            CLog.WriteInfo(new DataBarCode.LogElem(tInfo.Login, tInfo.Password, tInfo.ClientVersion, tInfo.MacAdress, IP, tInfo.DnsAdress, _service, _message));
        }

        public void AddLogFile(TInfo tInfo, string _service, string _message)
        {
            var request = this.Context.Request;
            string IP = request.UserHostAddress;
            // SLogWeb.Add(new LogElem(tInfo.Login, tInfo.Password, tInfo.ClientVersion, tInfo.MacAdress, IP, tInfo.DnsAdress, _service, _message));
            CLog.WriteInfo(new DataBarCode.LogElem(tInfo.Login, tInfo.Password, tInfo.ClientVersion, tInfo.MacAdress, IP, tInfo.DnsAdress, _service, _message));
        }

        public void InitUser()
        {
            try
            {
                ctx = winId.Impersonate();
            }
            catch (Exception) { }
        }

        public void EndUser()
        {
            if (ctx != null)
                ctx.Undo();
        }
        //public string DataBaseTNS = "TESTDCA";
        // public string DataBaseTNS = "SSM-CGP1";
        // COracle orclCGP = null;
        public string GetDataBaseTNS
        {
            get
            {
                if (this.Server.MachineName == "IVPAKHOLKOV-PC")
                    return "TESTDCA";
                else return "SSM-CGP1";
            }

        }

        [SoapHeader("brHeader")]
        //Receive any SOAP headers other than MyHeader.
        [SoapHeader("unknownHeaders")]
        [WebMethod(Description = "Получение времени с сервера")]
        public DateTime GetServerDataTime()
        {
            InitUser();
            TInfo t = Identific(brHeader);
            AddAllLog(t, "GetServerDataTime", "ТСД синхронизирует время с сервером");
            using (COracle orclCGP = new COracle())
            {
                orclCGP.SaveLogTSD("Терминал синхронизирует время", t.DnsAdress, t.Login, "1999");
            }
            EndUser();
            return DateTime.UtcNow;
        }

        [SoapHeader("brHeader")]
        [SoapHeader("unknownHeaders")]
        //--------------Работа с весами
        [WebMethod(Description = "Фиксация веса для ЕУ")]
        public string FixedWeight(String label, String UnitScales, String UnitWeight)
        {
            string result = null;
            InitUser();
            TInfo t = Identific(brHeader);
            AddAllLog(t, "FixedWeight", "Фиксирование веса для: " + label + " UnitScales: " + UnitScales + " UnitWeight: " + UnitWeight);
            using (COracle orclCGP = new COracle())
            {
                UnitWeight = UnitWeight.Replace(" ", ""); //Удаляем пробелы
                UnitWeight = UnitWeight.Replace(".", ","); //Меняем разделитель
                result = orclCGP.EU_FixedWeigth(label, UnitScales, UnitWeight);
            }
            EndUser();
            return result;
        }

        [SoapHeader("brHeader")]
        //Receive any SOAP headers other than MyHeader.
        [SoapHeader("unknownHeaders")]
        [WebMethod(Description = "Тестирование связи с сервером")]
        public bool Test_Connect(string message)
        {

            InitUser();
            TInfo t = Identific(brHeader);
            AddAllLog(t, "Test_Connect", "Подключился терминал. " + message);

            using (COracle orclCGP = new COracle())
            {
                orclCGP.SaveLogTSD(message, t.DnsAdress, t.Login, "1999");
            }

            EndUser();
            return true;
        }

        [SoapHeader("brHeader")]
        //Receive any SOAP headers other than MyHeader.
        [SoapHeader("unknownHeaders")]
        [WebMethod(Description = "Проверка авторизации пользователя")]
        public bool Test_Login()
        {
            TInfo t = Identific(brHeader);

            if (this.Server.MachineName == "IVPAKHOLKOV-PC")
            {
                AddAllLog(t, "Login", "Пропуск авторизации");
                return true;
            }

            try
            {
                ctx = winId.Impersonate();
                AddAllLog(t, "Login", "Доступ разрешен");
            }
            catch (Exception)
            {
                AddAllLog(t, "Login", "Доступ запрешен, и мы не разрешаем");
                return false;
            }


            EndUser();
            return true;
        }

        [SoapHeader("brHeader")]
        //Receive any SOAP headers other than MyHeader.
        [SoapHeader("unknownHeaders")]
        [WebMethod(Description = "Проверка административного доступа")]
        public bool Test_Login_Admin()
        {
            
            TInfo t = Identific(brHeader);

            if (this.Server.MachineName == "IVPAKHOLKOV-PC")
            {
                AddAllLog(t, "Login_Admin", "Пропуск авторизации");
                return true;
            }

            try
            {
                ctx = winId.Impersonate();
                using (COracle orclCGP = new COracle())
                {
                    orclCGP.AdminTSDCheck();
                }
                AddAllLog(t, "Login_Admin", "Доступ разрешен");
            }
            catch (Exception)
            {
                AddAllLog(t, "Login_Admin", "Доступ запрешен, и мы не разрешаем");
                return false;
            }


            EndUser();
            return true;
        }

        //------------Размещение на складе
        [SoapHeader("brHeader")]
        //Receive any SOAP headers other than MyHeader.
        [SoapHeader("unknownHeaders")]
        [WebMethod(Description = "Размещение ЕУ на МХ")]
        public DataTable POST_EU_LIST_Warehouse(List<string> listEU, string Place, DateTime? TimeOperation = null)
        {
            DataTable Result = null;
            InitUser();

            TInfo t = Identific(brHeader);
            if ((TimeOperation == DateTime.MinValue) || (TimeOperation == null))
                AddAllLog(t, "POST_EU_LIST_Warehouse", "Операция размещения списком на: " + Place + "; Count EU: " + listEU.Count.ToString());
            else
                AddAllLog(t, "POST_EU_LIST_Warehouse", "Операция размещения списком на: " + Place + "; Count EU: " + listEU.Count.ToString() + "; Время: " + TimeOperation.ToString());
            using (COracle orclCGP = new COracle())
            {
                Result = orclCGP.POST_EU_LIST_Warehouse(listEU, Place, TimeOperation);
            }
            EndUser();
            return Result;
        }

        [SoapHeader("brHeader")]
        //Receive any SOAP headers other than MyHeader.
        [SoapHeader("unknownHeaders")]
        [WebMethod(Description = "Создание задания на перемещение ЕУ списком")]
        public DataTable POST_EU_TASK_MOVE(List<string> listEU, string Place, DateTime? TimeOperation = null)
        {
            DataTable Result = null;
            InitUser();
            TInfo t = Identific(brHeader);
            if ((TimeOperation == DateTime.MinValue) || (TimeOperation == null))
                AddAllLog(t, "POST_EU_TASK_MOVE", "Создания задания " + " в MX: " + Place);
            else
                AddAllLog(t, "POST_EU_TASK_MOVE", "Создания задания " + " в MX: " + Place + "; Время: " + TimeOperation.ToString());
            using (COracle orclCGP = new COracle())
            {
                Result = orclCGP.POST_EU_TASK_MOVE(listEU, Place, TimeOperation);
            }

            EndUser();
            return Result;
        }

        [SoapHeader("brHeader")]
        //Receive any SOAP headers other than MyHeader.
        [SoapHeader("unknownHeaders")]
        [WebMethod(Description = "Задание ЕУ в Агрегат списком")]
        public DataTable POST_EU_IN_AGR(List<string> listEU, string Agr, DateTime? TimeOperation = null)
        {
            DataTable Result = null;
            InitUser();
            TInfo t = Identific(brHeader);

            if ((TimeOperation == DateTime.MinValue) || (TimeOperation == null))
                AddAllLog(t, "POST_EU_IN_AGR", "ЕУ в агрегат: " + Agr);
            else
                AddAllLog(t, "POST_EU_IN_AGR", "ЕУ в агрегат: " + Agr + "; Время: " + TimeOperation.ToString());

            using (COracle orclCGP = new COracle())
            {
                Result = orclCGP.POST_EU_IN_AGR(listEU, Agr, TimeOperation);
            }

            EndUser();
            return Result;
        }

        [SoapHeader("brHeader")]
        //Receive any SOAP headers other than MyHeader.
        [SoapHeader("unknownHeaders")]
        [WebMethod(Description = "Отгрузка ЕУ списком")]
        public DataTable POST_EU_LIST_SHIP(List<string> listEU, string RZDN, DateTime? TimeOperation = null)
        {
            DataTable Result = null;
            InitUser();

            TInfo t = Identific(brHeader);
            if ((TimeOperation == DateTime.MinValue) || (TimeOperation == null))
                AddAllLog(t, "POST_EU_LIST_SHIP", "Отгружаем задание: " + RZDN);
            else
                AddAllLog(t, "POST_EU_LIST_SHIP", "Отгружаем задание: " + RZDN + "; Время: " + TimeOperation.ToString());
            using (COracle orclCGP = new COracle())
            {
                Result = orclCGP.POST_EU_LIST_SHIP(listEU, RZDN, TimeOperation);
            }

            EndUser();
            return Result;
        }

        [SoapHeader("brHeader")]
        //Receive any SOAP headers other than MyHeader.
        [SoapHeader("unknownHeaders")]
        [WebMethod(Description = "ЕУ по задание в агрегат списком")]
        public DataTable POST_EU_LIST_RZDN_AGR(List<string> listEU, string RZDN, DateTime? TimeOperation = null)
        {
            DataTable Result = null;
            InitUser();

            TInfo t = Identific(brHeader);
            if ((TimeOperation == DateTime.MinValue) || (TimeOperation == null))
                AddAllLog(t, "POST_EU_LIST_RZDN_AGR", "Формируем задание из ЕУ, RZDN: " + RZDN);
            else
                AddAllLog(t, "POST_EU_LIST_RZDN_AGR", "Формируем задание из ЕУ, RZDN: " + RZDN + "; Время: " + TimeOperation.ToString());
            using (COracle orclCGP = new COracle())
            {
                Result = orclCGP.POST_EU_LIST_RZDN_AGR(listEU, RZDN, TimeOperation);
            }

            EndUser();
            return Result;
        }

        [SoapHeader("brHeader")]
        //Receive any SOAP headers other than MyHeader.
        [SoapHeader("unknownHeaders")]
        [WebMethod(Description = "Инвентаризация ЕУ под задание на МХ")]
        public DataTable POST_EU_LIST_INVERT_MX(List<string> listEU, string RZDN, string MX_LABEL, DateTime? TimeOperation = null)
        {
            DataTable Result = null;
            InitUser();

            TInfo t = Identific(brHeader);
            if ((TimeOperation == DateTime.MinValue) || (TimeOperation == null))
                AddAllLog(t, "POST_EU_LIST_INVERT_MX", "Инвентаризация MX:" + MX_LABEL + ", RZDN: " + RZDN);
            else
                AddAllLog(t, "POST_EU_LIST_INVERT_MX", "Инвентаризация MX:" + MX_LABEL + ", RZDN: " + RZDN + "; Время: " + TimeOperation.ToString());
            using (COracle orclCGP = new COracle())
            {
                Result = orclCGP.POST_EU_LIST_INVERT_MX(listEU, RZDN, MX_LABEL, TimeOperation);
            }

            EndUser();
            return Result;
        }


        //------------Размещение на складе
        [SoapHeader("brHeader")]
        //Receive any SOAP headers other than MyHeader.
        [SoapHeader("unknownHeaders")]
        [WebMethod(Description = "Размещение ЕУ на МХ")]
        public DataTable POST_EU_LIST_WAREHOUSE_TYPE(List<Relmuch> listEU, MXPlace Place, DateTime? TimeOperation = null)
        {
            DataTable Result = null;
            InitUser();
            TInfo t = Identific(brHeader);

            int CountEU = listEU.Count;
            int CountEuManual = 0;
            listEU.ForEach(delegate(Relmuch n) { if (n.MANUAL) CountEuManual++; });
            string message = String.Format("Размещения на: {0}-{1}  ЕУ: {2}-{3};", Place.LABEL, Place.CODEAUTOMATIC, CountEU.ToString(), CountEuManual.ToString());
            
            if ((TimeOperation == DateTime.MinValue) || (TimeOperation == null))
                AddAllLog(t, "POST_EU_LIST_Warehouse", message);
            else
                AddAllLog(t, "POST_EU_LIST_Warehouse", message + "; Время: " + TimeOperation.ToString());
            using (COracle orclCGP = new COracle())
            {
                Result = orclCGP.POST_EU_LIST_Warehouse(listEU, Place, TimeOperation);
            }
            EndUser();
            return Result;
        }


        /// <summary>
        /// Изменим логику, и добавим код автомтизации поперации
        /// </summary>

        [SoapHeader("brHeader")]
        //Receive any SOAP headers other than MyHeader.
        [SoapHeader("unknownHeaders")]
        [WebMethod(Description = "Задание ЕУ в агрегат списком")]
        public DataTable POST_EU_IN_AGR_TYPE(List<Relmuch> listEU, MXPlace Agr, DateTime? TimeOperation = null)
        {//3 and 5
            DataTable Result = null;
            InitUser();
            TInfo t = Identific(brHeader);

            int CountEU = listEU.Count;
            int CountEuManual = 0;
            listEU.ForEach(delegate(Relmuch n) { if (n.MANUAL) CountEuManual++; });
            string message = String.Format("ЕУ в агрегат: {0}-{1} ЕУ: {2}-{3};", Agr.LABEL, Agr.CODEAUTOMATIC, CountEU.ToString(), CountEuManual.ToString());
            
            if ((TimeOperation == DateTime.MinValue) || (TimeOperation == null))
                AddAllLog(t, "POST_EU_IN_AGR", message);
            else
                AddAllLog(t, "POST_EU_IN_AGR", message + ";Время: " + TimeOperation.ToString());

            using (COracle orclCGP = new COracle())
            {
                Result = orclCGP.POST_EU_IN_AGR(listEU, Agr, TimeOperation);
            }

            EndUser();
            return Result;
        }

        [SoapHeader("brHeader")]
        //Receive any SOAP headers other than MyHeader.
        [SoapHeader("unknownHeaders")]
        [WebMethod(Description = "Отгрузка ЕУ списком")]
        public DataTable POST_EU_LIST_SHIP_TYPE(List<Relmuch> listEU, string RZDN, DateTime? TimeOperation = null)
        {
            DataTable Result = null;
            InitUser();
            TInfo t = Identific(brHeader);

            int CountEU = listEU.Count;
            int CountEuManual = 0;
            listEU.ForEach(delegate(Relmuch n) { if (n.MANUAL) CountEuManual++; });
            string message = String.Format("Отгружаем задание: {0} ЕУ: {1}-{2};", RZDN, CountEU.ToString(), CountEuManual.ToString());
            

            if ((TimeOperation == DateTime.MinValue) || (TimeOperation == null))
                AddAllLog(t, "POST_EU_LIST_SHIP", message);
            else
                AddAllLog(t, "POST_EU_LIST_SHIP", message + "; Время: " + TimeOperation.ToString());
            using (COracle orclCGP = new COracle())
            {
                Result = orclCGP.POST_EU_LIST_SHIP(listEU, RZDN, TimeOperation);
            }

            EndUser();
            return Result;
        }

        [SoapHeader("brHeader")]
        //Receive any SOAP headers other than MyHeader.
        [SoapHeader("unknownHeaders")]
        [WebMethod(Description = "ЕУ по задание в агрегат списком")]
        public DataTable POST_EU_LIST_RZDN_AGR_TYPE(List<Relmuch> listEU, string RZDN, DateTime? TimeOperation = null)
        {
            DataTable Result = null;
            InitUser();
            TInfo t = Identific(brHeader);

            int CountEU = listEU.Count;
            int CountEuManual = 0;
            listEU.ForEach(delegate(Relmuch n) { if (n.MANUAL) CountEuManual++; });
            string message = String.Format("Формируем задание из ЕУ, RZDN: {0} ЕУ: {1}-{2};", RZDN, CountEU.ToString(), CountEuManual.ToString());
            
            if ((TimeOperation == DateTime.MinValue) || (TimeOperation == null))
                AddAllLog(t, "POST_EU_LIST_RZDN_AGR", message);
            else
                AddAllLog(t, "POST_EU_LIST_RZDN_AGR", message + "; Время: " + TimeOperation.ToString());
            using (COracle orclCGP = new COracle())
            {
                Result = orclCGP.POST_EU_LIST_RZDN_AGR(listEU, RZDN, TimeOperation);
            }

            EndUser();
            return Result;
        }

        [SoapHeader("brHeader")]
        //Receive any SOAP headers other than MyHeader.
        [SoapHeader("unknownHeaders")]
        [WebMethod(Description = "Инвентаризация ЕУ под задание на МХ")]
        public DataTable POST_EU_LIST_INVERT_MX_TYPE(List<Relmuch> listEU, string RZDN, MXPlace MX_LABEL, DateTime? TimeOperation = null)
        {
            DataTable Result = null;
            InitUser();
            TInfo t = Identific(brHeader);

            int CountEU = listEU.Count;
            int CountEuManual = 0;
            listEU.ForEach(delegate(Relmuch n) { if (n.MANUAL) CountEuManual++; });
            string message = String.Format("Инвентаризация MX: {0}-{1} ЕУ: {2}-{3}; RZDN: {4}", MX_LABEL.LABEL, MX_LABEL.CODEAUTOMATIC, CountEU.ToString(), CountEuManual.ToString(), RZDN);
            
            if ((TimeOperation == DateTime.MinValue) || (TimeOperation == null))
                AddAllLog(t, "POST_EU_LIST_INVERT_MX", message);
            else
                AddAllLog(t, "POST_EU_LIST_INVERT_MX", message + "; Время: " + TimeOperation.ToString());
            using (COracle orclCGP = new COracle())
            {
                Result = orclCGP.POST_EU_LIST_INVERT_MX(listEU, RZDN, MX_LABEL, TimeOperation);
            }

            EndUser();
            return Result;
        }

        [WebMethod(Description = "Получение логов web service")]
        public DataTable GetLogTSD()
        {
            InitUser();

            //using (COracle orclCGP = new COracle())
            //{
            //    orclCGP.SaveLogTSD("Тест авторизации", "Test", "Test", "1999");
            //}

            EndUser();
            //   SLogWeb.Add( new LogElem("Получаем ЛОГ"));
            return SLogWeb.GetDataTable();
        }

        [WebMethod(Description = "Получение онлайн терминалов")]
        public DataTable GetLogTSDOnline()
        {
            InitUser();
            EndUser();
            //   SLogWeb.Add( new LogElem("Получаем ЛОГ"));
            return SLogWeb.GetDataTableTSDOnline();
        }

        [SoapHeader("brHeader")]
        //Receive any SOAP headers other than MyHeader.
        [SoapHeader("unknownHeaders")]
        [WebMethod(Description = "Клонирование БД в SQLite")]
        public Byte[] DataBase_Clone_SQLiteZipFile()
        {
            InitUser();

            string fileName;

            TInfo t = Identific(brHeader);
            DateTime Dt = DateTime.Now;
            AddLogFile(t, "DataBase_Clone", "Старт запроса всех ЕУ");
            int CountEU = 0;
            //Сделаем заглушку для тестовой БД
            if (ConfigurationManager.AppSettings["MODE_TEST"] == "true")
            {
                AddAllLog(t, "DataBase_Clone", "Тестовый режим");
                fileName = "C:\\TSD_Release\\Sqlite\\EU.db3";
            }
            else
            {
                using (COracle orclCGP = new COracle())
                {
                    OracleDataReader R = orclCGP.DataBase_Clone_2();

                    fileName = InitTableSQLite(t);
                    CountEU = InsertEU(fileName, R);
                    R.Close();

                    OracleDataReader R2 = orclCGP.GET_AGR_READER();

                    InsertAGR(fileName, R2);
                    R2.Close();

                    OracleDataReader R3 = orclCGP.GET_WarehouseReader();

                    InsertTEHUZ(fileName, R3);
                    R3.Close();

                    OracleDataReader R4 = orclCGP.GET_ListTaskDispath();

                    InsertList(fileName, R4);
                    R4.Close();

                    OracleDataReader R4_1 = orclCGP.GET_ListTaskPGA();

                    InsertList(fileName, R4_1);
                    R4_1.Close();


                    OracleDataReader R4_2 = orclCGP.GET_ListTaskIntrovert();

                    InsertList(fileName, R4_2);
                    R4_2.Close();


                    OracleDataReader R5 = orclCGP.GET_ListTaskEU();

                    InsertListTaskEU(fileName, R5);
                    R5.Close();



                    OracleDataReader R6 = orclCGP.GetScalesList();
                    InsertVesy(fileName, R6);
                    R6.Close();

                    OracleDataReader R7 = orclCGP.GET_ListIntrovertMX();
                    InsertInventory(fileName, R7);
                    R7.Close();

                    OracleDataReader R8 = orclCGP.GetListCampMarka();
                    InsertRZDNMARKA(fileName, R8);
                    R8.Close();
                }
            }

            //string path = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();
            //  string baseNameDirectory = path + "\\" + "SQLITE_TSD\\" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm") + "_" + t.DnsAdress;
            string ZipFile = fileName + ".Zip";
            Compression(ZipFile, fileName);

            var DtEnd = DateTime.Now - Dt;
            AddAllLog(t, "DataBase_Clone", "Время выборки: " + DtEnd.ToString() + " CountEU: " + CountEU.ToString());

            EndUser();

            return File.ReadAllBytes(ZipFile); ;
        }

        public static string GetSha256FromString(string strData)
        {
            var message = Encoding.ASCII.GetBytes(strData);
            SHA256Managed hashString = new SHA256Managed();
            string hex = "";

            var hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex;
        }

        public string InitTableSQLite(TInfo t)
        {//
            string path = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();
            
           // string baseNameDirectory = path + "\\" + "SQLITE_TSD\\" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + "_" + t.DnsAdress;
            string baseNameDirectory = path + "\\" + "SQLITE_TSD\\" + t.DnsAdress + "-" + GetSha256FromString(t.DnsAdress + DateTime.Now.ToString() + DateTime.Now.Millisecond.ToString());
            Directory.CreateDirectory(path + "\\" + "SQLITE_TSD");
            Directory.CreateDirectory(baseNameDirectory);

            string baseName = baseNameDirectory + "/EU.db3";

            SQLiteConnection.CreateFile(baseName);

            //SQLiteFactory factory = (SQLiteFactory)DbProviderFactories.GetFactory("System.Data.SQLite");
            System.Data.SQLite.EF6.SQLiteProviderFactory factory = (System.Data.SQLite.EF6.SQLiteProviderFactory)DbProviderFactories.GetFactory("System.Data.SQLite");

            using (SQLiteConnection connection = (SQLiteConnection)factory.CreateConnection())
            {
                connection.ConnectionString = "Data Source = " + baseName;
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {

                    command.CommandText = @"CREATE TABLE [EU] (
                                [RELMUCH_LABEL] char(20) NOT NULL,
                                 [SIGN] char(5),
                                [RELMUCH_PRM] char(20),
                                [RELMUCH_VES] char(20),
                                [RELMUCH_FVES] char(20),
                                [RELMUCH_WIDTH] char(20),
                                [RELMUCH_THICKNESS] char(20),
                                [RPRT_NOM] char(20),
                                [RPRTTYP_NAME] char(20),
                                [RPRT_PLVNOM] char(20),
                                [MARKA_NAME] char(20),
                                [MARKA_GOST] char(20),
                                [TEHUZ_KOD] intager,
                                [INTRV_TMBEG] char(20),
                                [INTRV_TMEND] char(20),
                                [ORDER_SAP] char(20),
                                [RELMUCH_PROFILE] char(25)
                                );";
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
                connection.Close();

            }

            using (SQLiteConnection connection = (SQLiteConnection)factory.CreateConnection())
            {
                connection.ConnectionString = "Data Source = " + baseName;
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = @"CREATE TABLE [TEHUZ] (
                                [AGR_KOD] intager,
                                [TEHUZ_LABEL] char(20),
                                [TEHUZ_KOD] intager,
                                [TEHUZ_NAZ] char(50)
                                );";
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
                connection.Close();

            }

            using (SQLiteConnection connection = (SQLiteConnection)factory.CreateConnection())
            {
                connection.ConnectionString = "Data Source = " + baseName;
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    /*
                    AGR_KOD	140
                    AGR_NAZ	ÖÃÏ



                     * */
                    command.CommandText = @"CREATE TABLE [AGR] (
                                [AGR_KOD] char(20),
                                [AGR_NAZ] char(20));";
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
                connection.Close();

            }


            using (SQLiteConnection connection = (SQLiteConnection)factory.CreateConnection())
            {
                connection.ConnectionString = "Data Source = " + baseName;
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    /*
                    RZDN_PRM	130823
                    RZDN_DATE	21.11.2016 5:58:35
                    ZDN_NUM	10600
                    ZDN_PRM	1
                    DOC_BC	S0001500000106002016
                    TS_NUM	Âàãîííàÿ êàðòà
                    RZDN_STATUS	Îòãðóçêà íå çàâåðøåíà
                    OPER_MASTER_NAME	ÀÑÓÏ
                     * 
                     * TASK_THICKNESS, -- Толщина         
            TASK_WIDTH, -- Ширина
            TASK_MARKA, -- Марка
            TASK_STANDART_MARKA,-- Стандарт
             RZDN_DATE  


                     * */
                    command.CommandText = @"CREATE TABLE [TaskList] (
                                [ROWNUM] intager,
                                [RZDN_PRM] char(20),
                                [RZDN_DATE] char(20),
                                [ZDN_NUM] char(20),
                                [ZDN_PRM] char(20),
                                [DOC_BC] char(20),
                                [TS_NUM] char(200),
                                [RZDN_STATUS] char(20),
                                [TASK_THICKNESS] char(20),
                                [TASK_WIDTH] char(20),
                                [TASK_MARKA] char(20),
                                [TASK_STANDART_MARKA] char(20),
                                [OPER_TYPE_ID] char(20),
                                [OPER_MASTER_NAME] char(20),
                                [TASK_WEIGHT] char(20),
                                [SUM_RELMUCH_VES] char(20));";
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
                connection.Close();

            }

            using (SQLiteConnection connection = (SQLiteConnection)factory.CreateConnection())
            {
                connection.ConnectionString = "Data Source = " + baseName;
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    /*
NUM_PP	1
RZDN_PRM	130823
RELMUCH_PRM	258552982
åðøåíà

                     * */
                    command.CommandText = @"CREATE TABLE [TaskListEU] (
                                [NUM_PP] char(20),
                                [RZDN_PRM] char(20),
                                [RELMUCH_PRM] char(20));";
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
                connection.Close();

            }

            //Креетим весы
            using (SQLiteConnection connection = (SQLiteConnection)factory.CreateConnection())
            {
                connection.ConnectionString = "Data Source = " + baseName;
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    /*
                    VESY_NOM	777
                    VESY_NAME	Scales-Test

                     * */
                    command.CommandText = @"CREATE TABLE [VESY] (
                                [VESY_NOM] char(20),
                                [VESY_NAME] char(60));";
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
                connection.Close();

            }

            //Креетим весы
            using (SQLiteConnection connection = (SQLiteConnection)factory.CreateConnection())
            {
                connection.ConnectionString = "Data Source = " + baseName;
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    /*
                    RZDN_PRM	122606
                    TEHUZ_KOD	482
                    RESCODE	0
                     * */
                    command.CommandText = @"CREATE TABLE [INVENTORY] (
                                [RZDN_PRM] char(20),
                                [TEHUZ_KOD] char(50),
                                [RESCODE] char(20));";
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
                connection.Close();

            }

            //С хороним задание и марки стали к заданиям
            using (SQLiteConnection connection = (SQLiteConnection)factory.CreateConnection())
            {
                connection.ConnectionString = "Data Source = " + baseName;
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = @"CREATE TABLE [RZDN_MARKA] (
                                [RZDN_PRM] char(20),
                                [MARKA] char(20));";
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
                connection.Close();

            }

            return baseName;
        }

        public string _getOracleByName(OracleDataReader rd, string NameF)
        {
            string tmp = "-1";
            try
            {
                tmp = rd.GetValue(rd.GetOrdinal(NameF)).ToString();
            }

            catch (Exception)
            {

            }
            return tmp;

        }

        public void Compression(string FileZip, string FileSQLite)
        {
            try
            {
                File.Delete(FileZip);
            }
            catch (Exception) { }

            using (var zipFile = new ZipFile(FileZip))
            {
                zipFile.CompressionLevel = CompressionLevel.BestSpeed;
                zipFile.AddFile(FileSQLite, "\\");
                zipFile.Save();

            }
        }

        public int InsertEU(String baseName, OracleDataReader reader)
        {//Запишем событие в строенную БД
            DateTime dtEv = DateTime.Now;

            int CountEU = 0;

            SQLiteProviderFactory factory = (SQLiteProviderFactory)DbProviderFactories.GetFactory("System.Data.SQLite");

            using (SQLiteConnection connection = (SQLiteConnection)factory.CreateConnection())
            {

                connection.ConnectionString = "Data Source = " + baseName;


                SQLiteCommand command = new SQLiteCommand(connection);
                connection.Open();
                SQLiteTransaction liteTransaction = connection.BeginTransaction();

                while (reader.Read())
                {
                    using (SQLiteCommand insert = new SQLiteCommand("insert into EU(RELMUCH_LABEL, SIGN, RELMUCH_PRM, RELMUCH_VES, RELMUCH_FVES, RELMUCH_WIDTH, RELMUCH_THICKNESS, RPRT_NOM, RPRTTYP_NAME,RPRT_PLVNOM, MARKA_NAME,  MARKA_GOST, TEHUZ_KOD,INTRV_TMBEG, INTRV_TMEND, ORDER_SAP, RELMUCH_PROFILE) values(@RELMUCH_LABEL, @SIGN, @RELMUCH_PRM, @RELMUCH_VES, @RELMUCH_FVES,@RELMUCH_WIDTH,@RELMUCH_THICKNESS,@RPRT_NOM,@RPRTTYP_NAME, @RPRT_PLVNOM, @MARKA_NAME, @MARKA_GOST, @TEHUZ_KOD, @INTRV_TMBEG, @INTRV_TMEND, @ORDER_SAP, @RELMUCH_PROFILE)", connection))
                    {

                        insert.Parameters.AddWithValue("@RELMUCH_LABEL", _getOracleByName(reader, "RELMUCH_LABEL"));
                        insert.Parameters.AddWithValue("@SIGN", _getOracleByName(reader, "SIGN"));
                        insert.Parameters.AddWithValue("@RELMUCH_PRM", _getOracleByName(reader, "RELMUCH_PRM"));
                        insert.Parameters.AddWithValue("@RELMUCH_VES", _getOracleByName(reader, "RELMUCH_VES"));
                        insert.Parameters.AddWithValue("@RELMUCH_FVES", _getOracleByName(reader, "RELMUCH_FVES"));
                        insert.Parameters.AddWithValue("@RELMUCH_WIDTH", _getOracleByName(reader, "RELMUCH_WIDTH"));
                        insert.Parameters.AddWithValue("@RELMUCH_THICKNESS", _getOracleByName(reader, "RELMUCH_THICKNESS"));
                        insert.Parameters.AddWithValue("@RPRT_NOM", _getOracleByName(reader, "RPRT_NOM") + "-" + _getOracleByName(reader, "RELMUCH_ELMNOM"));
                        insert.Parameters.AddWithValue("@RPRTTYP_NAME", _getOracleByName(reader, "RPRTTYP_NAME"));
                        insert.Parameters.AddWithValue("@RPRT_PLVNOM", _getOracleByName(reader, "RPRT_PLVNOM"));
                        insert.Parameters.AddWithValue("@MARKA_NAME", _getOracleByName(reader, "MARKA_NAME").ToUpper());
                        insert.Parameters.AddWithValue("@MARKA_GOST", _getOracleByName(reader, "MARKA_GOST").ToUpper());
                        insert.Parameters.AddWithValue("@TEHUZ_KOD", _getOracleByName(reader, "TEHUZ_KOD"));
                        //  insert.Parameters.AddWithValue("@TEHUZ_LABEL", _getOracleByName(reader, "TEHUZ_LABEL"));
                        //  insert.Parameters.AddWithValue("@FACT_PLACE_NAME", _getOracleByName(reader, "FACT_PLACE_NAME"));
                        insert.Parameters.AddWithValue("@INTRV_TMBEG", _getOracleByName(reader, "INTRV_TMBEG"));
                        insert.Parameters.AddWithValue("@INTRV_TMEND", _getOracleByName(reader, "INTRV_TMEND"));
                        insert.Parameters.AddWithValue("@ORDER_SAP", _getOracleByName(reader, "ORDER_SAP"));
                        insert.Parameters.AddWithValue("@RELMUCH_PROFILE", _getOracleByName(reader, "RELMUCH_PROFILE"));
                        insert.Transaction = liteTransaction;
                        insert.ExecuteNonQuery();

                        CountEU++;
                    }

                }

                liteTransaction.Commit();

                try
                {



                }
                catch (Exception ex)
                {

                }
                finally
                {

                    connection.Close();
                }
            }

            return CountEU;

        }

        public string InsertAGR(String baseName, OracleDataReader reader)
        {//Запишем событие в строенную БД
            DateTime dtEv = DateTime.Now;
            SQLiteProviderFactory factory = (SQLiteProviderFactory)DbProviderFactories.GetFactory("System.Data.SQLite");

            using (SQLiteConnection connection = (SQLiteConnection)factory.CreateConnection())
            {

                connection.ConnectionString = "Data Source = " + baseName;


                SQLiteCommand command = new SQLiteCommand(connection);
                connection.Open();
                SQLiteTransaction liteTransaction = connection.BeginTransaction();

                while (reader.Read())
                {
                    using (SQLiteCommand insert = new SQLiteCommand("insert into AGR(AGR_KOD, AGR_NAZ ) values(@AGR_KOD, @AGR_NAZ)", connection))
                    {
                        /*
                         *                     command.CommandText = @"CREATE TABLE [TEHUZ] (
                                [AGR_KOD] char(20),
                                [TEHUZ_LABEL] char(20),
                                [TEHUZ_KOD] char(20),
                                [TEHUZ_KOD_MES] char(20),
                                [TEHUZ_NAZ] char(20),
                                [TEHUZ_NAME] char(20));";
                         * 
                         *                      * */
                        /*   command.CommandText = @"CREATE TABLE [AGR] (
                                       [AGR_KOD] char(20),
                                       [AGR_NAZ] char(20));";
                                * */
                        insert.Parameters.AddWithValue("@AGR_KOD", _getOracleByName(reader, "AGR_KOD"));
                        insert.Parameters.AddWithValue("@AGR_NAZ", _getOracleByName(reader, "AGR_NAZ"));
                        insert.Transaction = liteTransaction;
                        insert.ExecuteNonQuery();
                    }

                }

                liteTransaction.Commit();

                try
                {



                }
                catch (Exception ex)
                {

                }
                finally
                {

                    connection.Close();
                }
            }
            return baseName;

        }

        public string InsertVesy(String baseName, OracleDataReader reader)
        {//Запишем событие в строенную БД
            DateTime dtEv = DateTime.Now;

            SQLiteProviderFactory factory = (SQLiteProviderFactory)DbProviderFactories.GetFactory("System.Data.SQLite");

            using (SQLiteConnection connection = (SQLiteConnection)factory.CreateConnection())
            {

                connection.ConnectionString = "Data Source = " + baseName;


                SQLiteCommand command = new SQLiteCommand(connection);
                connection.Open();
                SQLiteTransaction liteTransaction = connection.BeginTransaction();

                while (reader.Read())
                {
                    using (SQLiteCommand insert = new SQLiteCommand("insert into VESY(VESY_NOM, VESY_NAME ) values(@VESY_NOM, @VESY_NAME)", connection))
                    {
                        insert.Parameters.AddWithValue("@VESY_NOM", _getOracleByName(reader, "VESY_NOM"));
                        insert.Parameters.AddWithValue("@VESY_NAME", _getOracleByName(reader, "VESY_NAME"));
                        insert.Transaction = liteTransaction;
                        insert.ExecuteNonQuery();
                    }

                }

                liteTransaction.Commit();

                try
                {



                }
                catch (Exception ex)
                {

                }
                finally
                {

                    connection.Close();
                }
            }
            return baseName;

        }

        public string InsertRZDNMARKA(String baseName, OracleDataReader reader)
        {//Запишем событие в строенную БД
            DateTime dtEv = DateTime.Now;

            SQLiteProviderFactory factory = (SQLiteProviderFactory)DbProviderFactories.GetFactory("System.Data.SQLite");

            using (SQLiteConnection connection = (SQLiteConnection)factory.CreateConnection())
            {

                connection.ConnectionString = "Data Source = " + baseName;


                SQLiteCommand command = new SQLiteCommand(connection);
                connection.Open();
                SQLiteTransaction liteTransaction = connection.BeginTransaction();

                while (reader.Read())
                {
                    using (SQLiteCommand insert = new SQLiteCommand("insert into RZDN_MARKA(RZDN_PRM, MARKA) values(@RZDN_PRM, @MARKA)", connection))
                    {
                        /*
                         *  @"CREATE TABLE [RZDN_PRM_MARKA] (
                                [RZDN_PRM] char(20),
                                [MARKA] char(20));";
                         * */
                        insert.Parameters.AddWithValue("@RZDN_PRM", _getOracleByName(reader, "RZDN_PRM"));
                        insert.Parameters.AddWithValue("@MARKA", _getOracleByName(reader, "MARKA"));
                        insert.Transaction = liteTransaction;
                        insert.ExecuteNonQuery();
                    }

                }

                liteTransaction.Commit();

                try
                {



                }
                catch (Exception ex)
                {

                }
                finally
                {

                    connection.Close();
                }
            }
            return baseName;

        }

        public string InsertTEHUZ(String baseName, OracleDataReader reader)
        {//Запишем событие в строенную БД
            DateTime dtEv = DateTime.Now;
            SQLiteProviderFactory factory = (SQLiteProviderFactory)DbProviderFactories.GetFactory("System.Data.SQLite");

            using (SQLiteConnection connection = (SQLiteConnection)factory.CreateConnection())
            {

                connection.ConnectionString = "Data Source = " + baseName;


                SQLiteCommand command = new SQLiteCommand(connection);
                connection.Open();
                SQLiteTransaction liteTransaction = connection.BeginTransaction();

                while (reader.Read())
                {
                    using (SQLiteCommand insert = new SQLiteCommand("insert into TEHUZ(AGR_KOD, TEHUZ_LABEL,TEHUZ_KOD, TEHUZ_NAZ ) values(@AGR_KOD, @TEHUZ_LABEL, @TEHUZ_KOD, @TEHUZ_NAZ)", connection))
                    {
                        /*
                         *                     command.CommandText = @"CREATE TABLE [TEHUZ] (
                                [AGR_KOD] char(20),
                                [TEHUZ_LABEL] char(20),
                                [TEHUZ_KOD] char(20),
                                [TEHUZ_KOD_MES] char(20),
                                [TEHUZ_NAZ] char(20),
                                [TEHUZ_NAME] char(20));";
                         * 
                         *                      * */
                        /*   command.CommandText = @"CREATE TABLE [AGR] (
                                       [AGR_KOD] char(20),
                                       [AGR_NAZ] char(20));";
                                * */
                        insert.Parameters.AddWithValue("@AGR_KOD", _getOracleByName(reader, "AGR_KOD"));
                        insert.Parameters.AddWithValue("@TEHUZ_LABEL", _getOracleByName(reader, "TEHUZ_LABEL"));
                        insert.Parameters.AddWithValue("@TEHUZ_KOD", _getOracleByName(reader, "TEHUZ_KOD"));
                        // insert.Parameters.AddWithValue("@TEHUZ_KOD_MES", _getOracleByName(reader, "TEHUZ_KOD_MES"));
                        insert.Parameters.AddWithValue("@TEHUZ_NAZ", _getOracleByName(reader, "TEHUZ_NAZ"));
                        // insert.Parameters.AddWithValue("@TEHUZ_NAME", _getOracleByName(reader, "TEHUZ_NAME"));
                        insert.Transaction = liteTransaction;
                        insert.ExecuteNonQuery();
                    }

                }

                liteTransaction.Commit();

                try
                {



                }
                catch (Exception ex)
                {

                }
                finally
                {

                    connection.Close();
                }
            }
            return baseName;

        }

        public string InsertList(String baseName, OracleDataReader reader)
        {//Запишем событие в строенную БД
            DateTime dtEv = DateTime.Now;
            SQLiteProviderFactory factory = (SQLiteProviderFactory)DbProviderFactories.GetFactory("System.Data.SQLite");

            using (SQLiteConnection connection = (SQLiteConnection)factory.CreateConnection())
            {

                connection.ConnectionString = "Data Source = " + baseName;


                SQLiteCommand command = new SQLiteCommand(connection);
                connection.Open();
                SQLiteTransaction liteTransaction = connection.BeginTransaction();

                while (reader.Read())
                {
                    using (SQLiteCommand insert = new SQLiteCommand("insert into TaskList(ROWNUM, RZDN_PRM, RZDN_DATE,ZDN_NUM, ZDN_PRM , DOC_BC , TS_NUM , RZDN_STATUS, TASK_THICKNESS, TASK_WIDTH,  TASK_MARKA, TASK_STANDART_MARKA, OPER_TYPE_ID, OPER_MASTER_NAME, TASK_WEIGHT, SUM_RELMUCH_VES) values(@ROWNUM, @RZDN_PRM, @RZDN_DATE, @ZDN_NUM, @ZDN_PRM, @DOC_BC, @TS_NUM, @RZDN_STATUS, @TASK_THICKNESS, @TASK_WIDTH, @TASK_MARKA, @TASK_STANDART_MARKA, @OPER_TYPE_ID, @OPER_MASTER_NAME, @TASK_WEIGHT, @SUM_RELMUCH_VES)", connection))
                    {
                        /*
                         *                     command.CommandText = @"CREATE TABLE [] (
                                [RZDN_PRM] char(20),
                                [RZDN_DATE] char(20),
                                [ZDN_NUM] char(20),
                                [ZDN_PRM] char(20),
                                [DOC_BC] char(20),
                                [TS_NUM] char(20),
                                [RZDN_STATUS] char(20),
                         *                               [TASK_THICKNESS] char(20),
                                [TASK_WIDTH] char(20),
                                [TASK_MARKA] char(20),
                                [TASK_STANDART_MARKA] char(20),
                                [OPER_MASTER_NAME] char(20));";
                                * */

                        insert.Parameters.AddWithValue("@ROWNUM", _getOracleByName(reader, "ROWNUM"));
                        insert.Parameters.AddWithValue("@RZDN_PRM", _getOracleByName(reader, "RZDN_PRM"));
                        insert.Parameters.AddWithValue("@RZDN_DATE", _getOracleByName(reader, "RZDN_DATE"));
                        insert.Parameters.AddWithValue("@ZDN_NUM", _getOracleByName(reader, "ZDN_NUM"));
                        insert.Parameters.AddWithValue("@ZDN_PRM", _getOracleByName(reader, "ZDN_PRM"));
                        insert.Parameters.AddWithValue("@DOC_BC", _getOracleByName(reader, "DOC_BC"));
                        insert.Parameters.AddWithValue("@TS_NUM", _getOracleByName(reader, "TS_NUM"));
                        insert.Parameters.AddWithValue("@RZDN_STATUS", _getOracleByName(reader, "RZDN_STATUS"));
                        insert.Parameters.AddWithValue("@TASK_THICKNESS", _getOracleByName(reader, "TASK_THICKNESS"));
                        insert.Parameters.AddWithValue("@TASK_WIDTH", _getOracleByName(reader, "TASK_WIDTH"));
                        insert.Parameters.AddWithValue("@TASK_MARKA", _getOracleByName(reader, "TASK_MARKA"));
                        insert.Parameters.AddWithValue("@TASK_STANDART_MARKA", _getOracleByName(reader, "TASK_STANDART_MARKA"));
                        insert.Parameters.AddWithValue("@OPER_TYPE_ID", _getOracleByName(reader, "OPER_TYPE_ID"));
                        insert.Parameters.AddWithValue("@OPER_MASTER_NAME", _getOracleByName(reader, "OPER_MASTER_NAME"));
                        insert.Parameters.AddWithValue("@TASK_WEIGHT", _getOracleByName(reader, "TASK_WEIGHT")); //TASK_WEIGHT	24,4
                        insert.Parameters.AddWithValue("@SUM_RELMUCH_VES", _getOracleByName(reader, "SUM_RELMUCH_VES")); //TASK_WEIGHT	24,4
                        insert.Transaction = liteTransaction;
                        insert.ExecuteNonQuery();
                    }

                }

                liteTransaction.Commit();

                try
                {



                }
                catch (Exception ex)
                {

                }
                finally
                {

                    connection.Close();
                }
            }
            return baseName;

        }

        public string InsertListTaskEU(String baseName, OracleDataReader reader)
        {//Запишем событие в строенную БД
            DateTime dtEv = DateTime.Now;

            SQLiteProviderFactory factory = (SQLiteProviderFactory)DbProviderFactories.GetFactory("System.Data.SQLite");

            using (SQLiteConnection connection = (SQLiteConnection)factory.CreateConnection())
            {

                connection.ConnectionString = "Data Source = " + baseName;


                SQLiteCommand command = new SQLiteCommand(connection);
                connection.Open();
                SQLiteTransaction liteTransaction = connection.BeginTransaction();

                while (reader.Read())
                {
                    using (SQLiteCommand insert = new SQLiteCommand("insert into TaskListEU(NUM_PP, RZDN_PRM, RELMUCH_PRM) values(@NUM_PP, @RZDN_PRM, @RELMUCH_PRM)", connection))
                    {
                        /*
                         *                     command.CommandText = @"CREATE TABLE [TEHUZ] (
                    /*
NUM_PP	1
RZDN_PRM	130823
RELMUCH_PRM	258552982
                                * */
                        insert.Parameters.AddWithValue("@NUM_PP", _getOracleByName(reader, "NUM_PP"));
                        insert.Parameters.AddWithValue("@RZDN_PRM", _getOracleByName(reader, "RZDN_PRM"));
                        insert.Parameters.AddWithValue("@RELMUCH_PRM", _getOracleByName(reader, "RELMUCH_PRM"));
                        insert.Transaction = liteTransaction;
                        insert.ExecuteNonQuery();
                    }

                }

                liteTransaction.Commit();

                try
                {



                }
                catch (Exception ex)
                {

                }
                finally
                {

                    connection.Close();
                }
            }
            return baseName;

        }

        public string InsertInventory(String baseName, OracleDataReader reader)
        {//Запишем событие в строенную БД
            DateTime dtEv = DateTime.Now;
            SQLiteProviderFactory factory = (SQLiteProviderFactory)DbProviderFactories.GetFactory("System.Data.SQLite");

            using (SQLiteConnection connection = (SQLiteConnection)factory.CreateConnection())
            {

                connection.ConnectionString = "Data Source = " + baseName;


                SQLiteCommand command = new SQLiteCommand(connection);
                connection.Open();
                SQLiteTransaction liteTransaction = connection.BeginTransaction();

                while (reader.Read())
                {
                    using (SQLiteCommand insert = new SQLiteCommand("insert into INVENTORY(RZDN_PRM, TEHUZ_KOD, RESCODE) values(@RZDN_PRM, @TEHUZ_KOD, @RESCODE)", connection))
                    {
                        /*
 [INVENTORY] (
                                [RZDN_PRM] char(20),
                                [TEHUZ_KOD] char(20),
                                [RESCODE] char(20));";
                                * */
                        insert.Parameters.AddWithValue("@RZDN_PRM", _getOracleByName(reader, "RZDN_PRM"));
                        insert.Parameters.AddWithValue("@TEHUZ_KOD", _getOracleByName(reader, "TEHUZ_KOD"));
                        insert.Parameters.AddWithValue("@RESCODE", _getOracleByName(reader, "RESCODE"));
                        insert.Transaction = liteTransaction;
                        insert.ExecuteNonQuery();
                    }

                }

                liteTransaction.Commit();

                try
                {



                }
                catch (Exception ex)
                {

                }
                finally
                {

                    connection.Close();
                }
            }
            return baseName;

        }

        // [WebMethod(Description = "Чистка старых БД")]
        public void CleanOldSqlBd()
        {
            //   AddAllLog("CleanOldSqlBd", "Cleaning SnapShot ORACLE");
            //Очистка старых версий Базы данных
            string path = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();
            string baseNameDirectory = path + "\\" + "SQLITE_TSD\\";
            var List = Directory.GetDirectories(baseNameDirectory);
            int BufferCounterAll = List.Length;
            int BufferOldCount = 10; //Если менее 10 шт то ничего не делаем
            int BufferOldTimeHour = 1; //Удаляем старше стольки то часов
            int BufferCounterDel = 0;

            if (List.Length > BufferOldCount)
            {//Если менее 10 файлов то ничего не делаем.
                //
                foreach (var elem in List)
                {
                    if (BufferCounterAll <= BufferOldCount)
                        break;

                    if (Directory.GetCreationTime(elem) < DateTime.Now.AddHours(-BufferOldTimeHour))
                    {
                        try
                        {
                            Directory.Delete(elem, true);
                        }
                        catch
                        {
                            AddAllLog("CleanOldSqlBd", "NOT DELETE: " + elem);
                        };

                        BufferCounterAll--;
                        BufferCounterDel++;
                    }
                }
            }
            AddAllLog("CleanOldSqlBd", "Cleaning Files: " + BufferCounterDel.ToString() + "; All Files: " + BufferCounterAll.ToString());
            //return List;
        }
        /// <summary>
        /// -----------------Работа с релизами
        /// </summary>
        /// <returns></returns>
        [WebMethod(Description = "Обновление ПО DataBarCode")]
        public Byte[] System_Get_Release()
        {
            AddAllLog("System_Get_Release", "ТСД получает обновление");
            CLog.WriteInfo("System_Get_Release", "Девайс получает релиз");
            return File.ReadAllBytes("C:\\TSD_Release\\Current\\DataBrCode.zip");
        }

        public TInfo Identific(BrHeader _header)
        {
            TInfo t = new TInfo();
            if (_header != null)
            {
                t.Login = _header.Login;
                t.Password = _header.Password;
                t.MacAdress = _header.MacAdress;
                t.DnsAdress = _header.DnsAdress;
                t.ClientVersion = _header.ClientVersion;
            }
            return t;
        }

        [WebMethod]
        [SoapHeader("brHeader")]
        //Receive any SOAP headers other than MyHeader.
        [SoapHeader("unknownHeaders")]
        public string TestLog()
        {

            ctx = winId.Impersonate();

            // Revert impersonation


            AddAllLog("TestLog", "Тест Лог");

            var request = this.Context.Request;

            StringBuilder sb = new StringBuilder();
            sb.Append("IP = ").AppendLine(request.UserHostAddress);
            sb.Append("URL = ").AppendLine(request.Url.OriginalString);
            sb.Append("Header 'Connection' = ").AppendLine(request.Headers["Connection"]);

            DateTime dnsDate = DateTime.Now;
            TimeSpan dnsSpan;

            try
            {
                //        throw new Exception("Закомментируй меня.");
                var entry = Dns.GetHostEntry(request.UserHostAddress);
                dnsSpan = DateTime.Now.Subtract(dnsDate);
                sb.Append("HostName = ").AppendLine(entry.HostName);

            }
            catch (Exception ex)
            {
                dnsSpan = DateTime.Now.Subtract(dnsDate);
                sb.AppendLine(ex.Message);
            }

            sb.Append("dnsSpan = ").AppendLine(dnsSpan.ToString());
            if (ctx != null)
                ctx.Undo();
            return sb.ToString();
        }

        [SoapHeader("brHeader")]
        //Receive any SOAP headers other than MyHeader.
        [SoapHeader("unknownHeaders")]
        [WebMethod(Description = "Сохранение логов с ТСД на сервер в ZIP")]
        public bool SAVE_LOG_TSD_ZIP(Byte[] result)
        {
            try
            {
                InitUser();
                TInfo t = Identific(brHeader);
                AddAllLog(t, "SAVE_LOG_TSD_ZIP", "Получаем логи с ТСД");
                //Процедура работы с логами
                var tmp = CreateFileLog(t, result);
                AddAllLog(t, "SAVE_LOG_TSD_ZIP", "Файлы в директории: " + tmp);

                EndUser();
                return true;
            }
            catch (Exception ex)
            {
                AddAllLog("SAVE_LOG_TSD_ZIP", "Ошибка при получении логов с ТСД" + ex.ToString());
                return false;
            }

        }

        private string CreateFileLog(TInfo t, Byte[] result)
        {   //Создаем директорию на сервере
            string path = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();
            string baseNameDirectory = path + "\\" + "LOG_TSD\\" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm") + "_" + t.DnsAdress;
            Directory.CreateDirectory(path + "\\" + "LOG_TSD");
            Directory.CreateDirectory(baseNameDirectory);
            //Сохраняем бинарный файл

            string FileName = baseNameDirectory + "\\" + t.DnsAdress + "_" + DateTime.Now.ToString("yyMMdd-HHmm") + ".zip";
            FileStream f = File.Create(FileName);
            f.Write(result, 0, result.Length);
            f.Close();

            //Пробуем разархивировать
            Unzip(FileName, baseNameDirectory + "\\");

            return baseNameDirectory;
        }

        public void Unzip(string exFile, string exDir)
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
                AddAllLog("Unzip", "Ошибка при получении логов с ТСД" + exz.ToString());
            }
        }

        [SoapHeader("brHeader")]
        //Receive any SOAP headers other than MyHeader.
        [SoapHeader("unknownHeaders")]
        [WebMethod(Description = "Проверка актуальности версии")]
        public string CHECK_CLIENT_VERSION()
        {
            string SReturn = "Версия клиента не определена";
            try
            {

                InitUser();
                TInfo t = Identific(brHeader);
                AddAllLog(t, "CHECK_CLIENT_VERSION", "Проверяем версию клиента");

                string CurrentClient = ConfigurationManager.AppSettings["CurrentVersionClient"];
                if (t.ClientVersion == "v0.0")
                {
                    SReturn = "Версия клиента не определена";
                }
                else if (CurrentClient == t.ClientVersion)
                {
                    SReturn = "Актуальная версия ПО";
                }
                else
                {
                    SReturn = "Доступна новая версия ПО";
                }

                try
                {
                    Double CStableVer = Double.Parse(CurrentClient.Replace('.', ','));
                    Double ClientVer = Double.Parse(t.ClientVersion.Replace('.', ','));
                    if (ClientVer > CStableVer)
                        SReturn = "Версия для разарботчиков";
                }
                catch (Exception ex) { AddAllLog(t, "CHECK_CLIENT_VERSION", ex.Message); }

                AddAllLog(t, "CHECK_CLIENT_VERSION", SReturn);
                EndUser();
            }
            catch (Exception ex)
            {
                AddAllLog("CHECK_CLIENT_VERSION", "Ошибка при проверке версии ТСД" + ex.ToString());

            }


            return SReturn;

        }

        [SoapHeader("brHeader")]
        //Receive any SOAP headers other than MyHeader.
        [SoapHeader("unknownHeaders")]
        [WebMethod(Description = "BlackListAPP")]
        public List<string> BLACKLISTAPP()
        {
            List<string> LSR = new List<string>();
            try
            {

                InitUser();
                TInfo t = Identific(brHeader);
                string path = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();
                path += "\\BlackListAppTSD.txt";
                using (StreamReader sr = new StreamReader(path))
                {
                    while (!sr.EndOfStream)
                    {
                        LSR.Add(sr.ReadLine());
                    
                    }
                    sr.Close();
                }

                AddAllLog(t, "BLACKLISTAPP", "SEND");
                EndUser();
            }
            catch (Exception ex)
            {
                AddAllLog("BLACKLISTAPP", ex.Message);

            }


            return LSR;

        }


    }
}
