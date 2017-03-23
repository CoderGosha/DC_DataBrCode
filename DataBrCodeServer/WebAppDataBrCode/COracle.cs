using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.OracleClient;
using System.Data;
using System.Configuration;

namespace WebAppDataBrCode
{
    public class COracle : IDisposable
    {
        private string TnsName { get; set; }
        private string login { get; set; }
        private string password { get; set; }


        private OracleConnection appConn;
        private string StringConnection;
        private OracleCommand cmd;
        public string oldQuery;

        public bool OracleState;



        public COracle(string _TnsName, string _login, string _password)
        {
            TnsName = _TnsName;
            login = _login;
            password = _password;
            OracleState = false;

        }

        public COracle()
        {
            TnsName = ConfigurationManager.AppSettings["TnsOracle"];
            login = ConfigurationManager.AppSettings["UserOracle"];
            password = ConfigurationManager.AppSettings["PassOracle"];
            OracleState = false;

        }




        ~COracle()
        {
            try
            {
                appConn.Close();
                appConn.Dispose();
            }

            catch (Exception)
            {
            }
        }

        public void Dispose()
        {
            try
            {
                // appConn.Close();
                appConn.Dispose();
            }

            catch (Exception)
            {

            }
        }



        private void Connect()
        {
            StringConnection = "Data Source=" + TnsName + ";User Id=" + login + ";Password=" + password + ";pooling=false;";
            //string StringConnection1 = "Data Source=SSMTEST2;User Id=mts;Password=LfnfWtynh;";
            appConn = new OracleConnection(StringConnection);

            appConn.Open();
            OracleState = true;


        }

        public string _getOracleByName(OracleDataReader rd, string NameF)
        {
            string tmp;
            tmp = rd.GetValue(rd.GetOrdinal(NameF)).ToString();
            return tmp;

        }

        private void Close()
        {
            try
            {
                appConn.Close();
            }

            catch (Exception)
            {
            }
        }

        public void SaveLogTSD(string message, string IP, string User_, string func_nom_)
        {
            /*
             * 
             * agr_kod_     agr.agr_kod%type,
                    StrMes       varchar2, -- Сообщение какое действие предпринято в АРМ
                    IP_          varchar2, -- IP-адрес
                    User_        varchar2, -- Пользователь                     
                    tehuz_kod_   tehuz.tehuz_kod%type default null, --  код ТУ, если есть возможность его передать
                    relmuch_prm_ relmuch.relmuch_prm%type default null, -- идент объекта БД, если есть возможнсть его передать
                    func_nom_    func.func_nom%type default 999) as    -- 999 - АРМ, 1999 - ТСД
             * */
            if (!OracleState)
                this.Connect();

            cmd = appConn.CreateCommand();
            cmd.Parameters.Clear();
            cmd.CommandText = "CGP1.ARM_Log";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("agr_kod_", OracleType.NVarChar).Value = "140";
            cmd.Parameters.Add("StrMes", OracleType.NVarChar).Value = message;
            cmd.Parameters.Add("IP_", OracleType.NVarChar).Value = IP;
            cmd.Parameters.Add("User_", OracleType.NVarChar).Value = User_;
            cmd.Parameters.Add("tehuz_kod_", OracleType.NVarChar).Value = "";
            cmd.Parameters.Add("relmuch_prm_", OracleType.NVarChar).Value = "";
            cmd.Parameters.Add("func_nom_", OracleType.NVarChar).Value = func_nom_;
            //cmd.Parameters.Add("count_posads", OracleType.NVarChar, 200).Direction = ParameterDirection.Output;
            //cmd.Parameters["ErrStr"].Value = operation_kod;
            cmd.ExecuteNonQuery();

            if (OracleState)
                this.Close();
        }


        public DataTable EU_GetData(string lable)
        {
            if (!OracleState)
                this.Connect();

            cmd = appConn.CreateCommand();
            cmd.Parameters.Clear();
            cmd.CommandText = "test_piv.getrelmuch_barcode";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("relmuch_label_", OracleType.NVarChar).Value = lable;
            cmd.Parameters.Add("curs", OracleType.Cursor).Direction = System.Data.ParameterDirection.Output;
            //cmd.Parameters.Add("count_posads", OracleType.NVarChar, 200).Direction = ParameterDirection.Output;
            //cmd.Parameters["ErrStr"].Value = operation_kod;
            OracleDataReader reader = cmd.ExecuteReader();
            DataTable R = new DataTable();
            R.TableName = "EU_GetData";
            R.Load(reader);
            reader.Close();
            if (OracleState)
                this.Close();

            return R;


        }

        public DataTable GET_AGR()
        {
            if (!OracleState)
                this.Connect();

            cmd = appConn.CreateCommand();
            cmd.Parameters.Clear();
            cmd.CommandText = "CGP1_CURSORS.GetListStorage";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("curs", OracleType.Cursor).Direction = System.Data.ParameterDirection.Output;
            OracleDataReader reader = cmd.ExecuteReader();
            DataTable R = new DataTable();
            R.TableName = "GET_AGR";
            R.Load(reader);
            reader.Close();
            if (OracleState)
                this.Close();

            return R;


        }

        public OracleDataReader GET_AGR_READER()
        {
            if (!OracleState)
                this.Connect();

            cmd = appConn.CreateCommand();
            cmd.Parameters.Clear();
            cmd.CommandText = "CGP1_CURSORS.GetListStorage";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("curs", OracleType.Cursor).Direction = System.Data.ParameterDirection.Output;
            OracleDataReader reader = cmd.ExecuteReader();



            return reader;

        }

        public DataTable GET_Warehouse(string agr, bool All)
        {
            if (!OracleState)
                this.Connect();

            cmd = appConn.CreateCommand();
            cmd.Parameters.Clear();
            cmd.CommandText = "CGP1_CURSORS.GetListPlace";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("curs", OracleType.Cursor).Direction = System.Data.ParameterDirection.Output;
            if (!All)
                cmd.Parameters.Add("agr_kod_", OracleType.NVarChar).Value = agr;

            OracleDataReader reader = cmd.ExecuteReader();
            DataTable R = new DataTable();
            R.TableName = "GET_Warehouse";
            R.Load(reader);
            reader.Close();
            if (OracleState)
                this.Close();

            return R;


        }


        public OracleDataReader GET_WarehouseReader()
        {
            if (!OracleState)
                this.Connect();

            cmd = appConn.CreateCommand();
            cmd.Parameters.Clear();
            cmd.CommandText = "CGP1_CURSORS.GetListPlaceTSD";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("curs", OracleType.Cursor).Direction = System.Data.ParameterDirection.Output;
            OracleDataReader reader = cmd.ExecuteReader();
            return reader;


        }


        public OracleDataReader GET_ListTaskDispath()
        {
            if (!OracleState)
                this.Connect();

            cmd = appConn.CreateCommand();
            cmd.Parameters.Clear();
            cmd.CommandText = "CGP1_CURSORS.getlisttaskdispath";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("curs", OracleType.Cursor).Direction = System.Data.ParameterDirection.Output;
            OracleDataReader reader = cmd.ExecuteReader();
            return reader;


        }


        public OracleDataReader GET_ListTaskEU()
        {

            /*
             *   -- Call the procedure
              cgp1_cursors.getrelmuchtasktsd(curs => :curs,
                                             oper_type_id_ => :oper_type_id_,
                                             rzdn_prm_ => :rzdn_prm_);
             * 
             * */
            if (!OracleState)
                this.Connect();

            cmd = appConn.CreateCommand();
            cmd.Parameters.Clear();
            cmd.CommandText = "CGP1_CURSORS.getrelmuchtasktsd";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("curs", OracleType.Cursor).Direction = System.Data.ParameterDirection.Output;
            OracleDataReader reader = cmd.ExecuteReader();
            return reader;


        }

        public OracleDataReader GET_ListTaskPGA()
        {

            /*
cgp1_arm_cursors.get_order(curs => :curs,
                             rprt_plvnom_ => :rprt_plvnom_,
                             rprt_nom_ => :rprt_nom_,
                             agr_kod_ => :agr_kod_,
                             tsd_ => :tsd_);
             * 
             * */
            if (!OracleState)
                this.Connect();

            cmd = appConn.CreateCommand();
            cmd.Parameters.Clear();
            cmd.CommandText = "cgp1_arm_cursors.get_order";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("curs", OracleType.Cursor).Direction = System.Data.ParameterDirection.Output;
            cmd.Parameters.Add("agr_kod_", OracleType.NVarChar).Value = "323";
            cmd.Parameters.Add("tsd_", OracleType.NVarChar).Value = "1";
            OracleDataReader reader = cmd.ExecuteReader();
            return reader;


        }


        public OracleDataReader GET_ListTaskIntrovert()
        {
            if (!OracleState)
                this.Connect();

            cmd = appConn.CreateCommand();
            cmd.Parameters.Clear();
            cmd.CommandText = "CGP1_CURSORS.getlistinventorytsd";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("curs", OracleType.Cursor).Direction = System.Data.ParameterDirection.Output;
            OracleDataReader reader = cmd.ExecuteReader();
            return reader;
        }


        public OracleDataReader GET_ListIntrovertMX()
        {
            if (!OracleState)
                this.Connect();
            /*
             *   cgp1_cursors.getlistpsinventorytsd(curs => :curs,
                                     rzdn_prm_ => :rzdn_prm_);
             * */
            cmd = appConn.CreateCommand();
            cmd.Parameters.Clear();
            cmd.CommandText = "CGP1_CURSORS.getlistpsinventorytsd";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("curs", OracleType.Cursor).Direction = System.Data.ParameterDirection.Output;
            OracleDataReader reader = cmd.ExecuteReader();
            return reader;
        }

        public DataTable POST_EU_LIST_Warehouse(List<string> listEU, string Place, DateTime? TimeOperation = null)
        {
            if (!OracleState)
                this.Connect();

            DataTable _tblEU = new DataTable();
            _tblEU.TableName = "POST_EU_LIST_Warehouse";

            DataColumn colN = new DataColumn("Label", typeof(String));
            _tblEU.Columns.Add(colN);
            DataColumn colSource = new DataColumn("result", typeof(String));
            _tblEU.Columns.Add(colSource);

            DataColumn colCode = new DataColumn("resultCode", typeof(String));
            _tblEU.Columns.Add(colCode);

            foreach (var e in listEU)
            {
                string result = "";
                string resultCode = "";

                try
                {

                    cmd = appConn.CreateCommand();
                    cmd.Parameters.Clear();
                    cmd.CommandText = "CGP1.PutUnit";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add("tehuz_label_", OracleType.NVarChar).Value = Place;
                    cmd.Parameters.Add("relmuch_label_", OracleType.NVarChar).Value = e;
                    // OracleDataReader reader = cmd.ExecuteReader();
                    cmd.Parameters.Add("ResInfo", OracleType.NChar, 100).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("ResCode", OracleType.NChar, 100).Direction = ParameterDirection.Output;

                    if ((TimeOperation != DateTime.MinValue) && (TimeOperation != null))
                        cmd.Parameters.Add("date_event_", OracleType.NVarChar).Value = TimeOperation;

                    cmd.ExecuteNonQuery();

                    result = cmd.Parameters["ResInfo"].Value.ToString();
                    resultCode = cmd.Parameters["ResCode"].Value.ToString();
                }

                catch (Exception ex)
                {
                    result = ex.Message;
                    resultCode = "0";
                }
                DataRow row1 = _tblEU.NewRow();
                row1["Label"] = e;
                row1["result"] = result;
                row1["resultCode"] = resultCode;
                _tblEU.Rows.Add(row1);
                //   cmd->Parameters["relmuch_prm_"]->CollectionType = OracleCollectionType::PLSQLAssociativeArray;
            }




            if (OracleState)
                this.Close();

            return _tblEU;


        }



        public DataTable POST_EU_TASK_MOVE(List<string> listEU, string Place, DateTime? TimeOperation = null)
        {
            if (!OracleState)
                this.Connect();

            DataTable _tblEU = new DataTable();
            _tblEU.TableName = "POST_EU_TASK_MOVE";

            DataColumn colN = new DataColumn("Label", typeof(String));
            _tblEU.Columns.Add(colN);
            DataColumn colSource = new DataColumn("result", typeof(String));
            _tblEU.Columns.Add(colSource);

            DataColumn colCode = new DataColumn("resultCode", typeof(String));
            _tblEU.Columns.Add(colCode);

            foreach (var e in listEU)
            {
                string result = "";
                string resultCode = "";

                try
                {


                    cmd = appConn.CreateCommand();
                    cmd.Parameters.Clear();
                    cmd.CommandText = "CGP1.TaskReplace";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.Add("relmuch_label_", OracleType.NVarChar).Value = e;
                    cmd.Parameters.Add("tehuz_label_", OracleType.NVarChar).Value = Place;

                    if ((TimeOperation != DateTime.MinValue) && (TimeOperation != null))
                        cmd.Parameters.Add("date_event_", OracleType.NVarChar).Value = TimeOperation;

                    cmd.ExecuteNonQuery();

                    result = "";
                    resultCode = "1";

                }

                catch (Exception ex)
                {
                    result = ex.Message;
                    resultCode = "0";

                }
                DataRow row1 = _tblEU.NewRow();
                row1["Label"] = e;
                row1["result"] = result;
                row1["resultCode"] = resultCode;
                _tblEU.Rows.Add(row1);
                //   cmd->Parameters["relmuch_prm_"]->CollectionType = OracleCollectionType::PLSQLAssociativeArray;
            }





            if (OracleState)
                this.Close();

            return _tblEU;

        }

        public void POST_EU_TASK_MOVE_UNIT(string EU, string Place)
        {
            if (!OracleState)
                this.Connect();


            cmd = appConn.CreateCommand();
            cmd.Parameters.Clear();
            cmd.CommandText = "CGP1.TaskReplace";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add("relmuch_label_", OracleType.NVarChar).Value = EU;
            cmd.Parameters.Add("tehuz_label_", OracleType.NVarChar).Value = Place;

            cmd.ExecuteNonQuery();


            if (OracleState)
                this.Close();


        }


        public DataTable POST_EU_IN_AGR(List<string> listEU, string Agr, DateTime? TimeOperation = null)
        {
            if (!OracleState)
                this.Connect();

            DataTable _tblEU = new DataTable();
            _tblEU.TableName = "POST_EU_IN_AGR";

            DataColumn colN = new DataColumn("Label", typeof(String));
            _tblEU.Columns.Add(colN);
            DataColumn colSource = new DataColumn("result", typeof(String));
            _tblEU.Columns.Add(colSource);

            DataColumn colCode = new DataColumn("resultCode", typeof(String));
            _tblEU.Columns.Add(colCode);

            foreach (var e in listEU)
            {
                string result = "";
                string resultCode = "";

                try
                {
                    /*
                     *   cgp1.placeunit(relmuch_label_ => :relmuch_label_,
                 relmuch_prm_ => :relmuch_prm_,
                 tehuz_label_ => :tehuz_label_,
                 tehuz_kod_ => :tehuz_kod_);
                     * */

                    cmd = appConn.CreateCommand();
                    cmd.Parameters.Clear();
                    cmd.CommandText = "CGP1.placeunit";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.Add("relmuch_label_", OracleType.NVarChar).Value = e;
                    cmd.Parameters.Add("tehuz_label_", OracleType.NVarChar).Value = Agr;

                    if ((TimeOperation != DateTime.MinValue) && (TimeOperation != null))
                        cmd.Parameters.Add("date_event_", OracleType.NVarChar).Value = TimeOperation;

                    cmd.ExecuteNonQuery();

                    result = "";
                    resultCode = "1";

                }

                catch (Exception ex)
                {
                    result = ex.Message;
                    resultCode = "0";

                }
                DataRow row1 = _tblEU.NewRow();
                row1["Label"] = e;
                row1["result"] = result;
                row1["resultCode"] = resultCode;
                _tblEU.Rows.Add(row1);
                //   cmd->Parameters["relmuch_prm_"]->CollectionType = OracleCollectionType::PLSQLAssociativeArray;
            }





            if (OracleState)
                this.Close();

            return _tblEU;

        }


        public DataTable POST_EU_LIST_SHIP(List<string> listEU, string rzdn, DateTime? TimeOperation = null)
        {
            if (!OracleState)
                this.Connect();

            DataTable _tblEU = new DataTable();
            _tblEU.TableName = "POST_EU_LIST_SHIP";

            DataColumn colN = new DataColumn("Label", typeof(String));
            _tblEU.Columns.Add(colN);
            DataColumn colSource = new DataColumn("result", typeof(String));
            _tblEU.Columns.Add(colSource);

            DataColumn colCode = new DataColumn("resultCode", typeof(String));
            _tblEU.Columns.Add(colCode);

            foreach (var e in listEU)
            {
                string result = "";
                string resultCode = "";

                try
                {
                    /*
procedure DispatchUnit(rzdn_prm_      rzdn.rzdn_prm%type, -- Идент задания на отгрузку
                       relmuch_label_ relmuch.relmuch_label%type default null,
                       relmuch_prm_   relmuch.relmuch_prm%type default null,
                       sign_master_   pls_integer default 1, -- 1 - ТСД, 2 - АРМ, 3 - ДТ
                       automat_       pls_integer default 3) 
                     * */

                    cmd = appConn.CreateCommand();
                    cmd.Parameters.Clear();
                    cmd.CommandText = "CGP1.DispatchUnit";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.Add("rzdn_prm_", OracleType.NVarChar).Value = rzdn;
                    cmd.Parameters.Add("relmuch_label_", OracleType.NVarChar).Value = e;

                    if ((TimeOperation != DateTime.MinValue) && (TimeOperation != null))
                        cmd.Parameters.Add("date_event_", OracleType.NVarChar).Value = TimeOperation;


                    cmd.ExecuteNonQuery();

                    result = "";
                    resultCode = "1";

                }

                catch (Exception ex)
                {
                    result = ex.Message;
                    resultCode = "0";

                }
                DataRow row1 = _tblEU.NewRow();
                row1["Label"] = e;
                row1["result"] = result;
                row1["resultCode"] = resultCode;
                _tblEU.Rows.Add(row1);
                //   cmd->Parameters["relmuch_prm_"]->CollectionType = OracleCollectionType::PLSQLAssociativeArray;
            }





            if (OracleState)
                this.Close();

            return _tblEU;

        }


        public DataTable POST_EU_LIST_RZDN_AGR(List<string> listEU, string rzdn, DateTime? TimeOperation = null)
        {
            if (!OracleState)
                this.Connect();

            DataTable _tblEU = new DataTable();
            _tblEU.TableName = "POST_EU_LIST_RZDN_AGR";

            DataColumn colN = new DataColumn("Label", typeof(String));
            _tblEU.Columns.Add(colN);
            DataColumn colSource = new DataColumn("result", typeof(String));
            _tblEU.Columns.Add(colSource);

            DataColumn colCode = new DataColumn("resultCode", typeof(String));
            _tblEU.Columns.Add(colCode);

            foreach (var e in listEU)
            {
                string result = "";
                string resultCode = "";

                try
                {
                    cmd = appConn.CreateCommand();
                    cmd.Parameters.Clear();
                    cmd.CommandText = "CGP1.SetUnitsTask_TSD";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.Add("relmuch_label_", OracleType.NVarChar).Value = e;
                    cmd.Parameters.Add("rzdn_prm_", OracleType.NVarChar).Value = rzdn;

                    if ((TimeOperation != DateTime.MinValue) && (TimeOperation != null))
                        cmd.Parameters.Add("date_event_", OracleType.NVarChar).Value = TimeOperation;

                    cmd.ExecuteNonQuery();

                    result = "";
                    resultCode = "1";

                }

                catch (Exception ex)
                {
                    result = ex.Message;
                    resultCode = "0";

                }
                DataRow row1 = _tblEU.NewRow();
                row1["Label"] = e;
                row1["result"] = result;
                row1["resultCode"] = resultCode;
                _tblEU.Rows.Add(row1);
                //   cmd->Parameters["relmuch_prm_"]->CollectionType = OracleCollectionType::PLSQLAssociativeArray;
            }





            if (OracleState)
                this.Close();

            return _tblEU;

        }

        public DataTable POST_EU_LIST_INVERT_MX(List<string> listEU, string rzdn, string MX_LABEL, DateTime? TimeOperation = null)
        {
            if (!OracleState)
                this.Connect();
            /*
             * - Получить данные инвентаризации для ТСД по 1 штуке
  procedure SetInventoryTSD(relmuch_label_ relmuch.relmuch_label%type, -- Список ШК ЕУ
                            tehuz_label_   tehuz.tehuz_label%type, -- Список ШК ТУ
                            rzdn_prm_      rzdn.rzdn_prm%type, -- Идент основной ведомости
                            sign_master_   pls_integer default 1, --Список с ТСД?
                            automat_       pls_integer default 3)
             * */
            DataTable _tblEU = new DataTable();
            _tblEU.TableName = "POST_EU_LIST_INVERT_MX";

            DataColumn colN = new DataColumn("Label", typeof(String));
            _tblEU.Columns.Add(colN);
            DataColumn colSource = new DataColumn("result", typeof(String));
            _tblEU.Columns.Add(colSource);

            DataColumn colCode = new DataColumn("resultCode", typeof(String));
            _tblEU.Columns.Add(colCode);

            foreach (var e in listEU)
            {
                string result = "";
                string resultCode = "";

                try
                {
                    cmd = appConn.CreateCommand();
                    cmd.Parameters.Clear();
                    cmd.CommandText = "CGP1.SetInventoryTSD";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.Add("relmuch_label_", OracleType.NVarChar).Value = e;
                    cmd.Parameters.Add("tehuz_label_", OracleType.NVarChar).Value = MX_LABEL;
                    cmd.Parameters.Add("rzdn_prm_", OracleType.NVarChar).Value = rzdn;

                    if ((TimeOperation != DateTime.MinValue) && (TimeOperation != null))
                        cmd.Parameters.Add("date_event_", OracleType.NVarChar).Value = TimeOperation;

                    cmd.ExecuteNonQuery();

                    result = "";
                    resultCode = "1";

                }

                catch (Exception ex)
                {
                    result = ex.Message;
                    resultCode = "0";

                }
                DataRow row1 = _tblEU.NewRow();
                row1["Label"] = e;
                row1["result"] = result;
                row1["resultCode"] = resultCode;
                _tblEU.Rows.Add(row1);
                //   cmd->Parameters["relmuch_prm_"]->CollectionType = OracleCollectionType::PLSQLAssociativeArray;
            }





            if (OracleState)
                this.Close();

            return _tblEU;

        }

        public DataTable DataBase_Clone()
        {
            if (!OracleState)
                this.Connect();

            cmd = appConn.CreateCommand();
            cmd.Parameters.Clear();
            cmd.CommandText = "CGP1_CURSORS.GetListOnStorageTSD";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("curs", OracleType.Cursor).Direction = System.Data.ParameterDirection.Output;
            OracleDataReader reader = cmd.ExecuteReader();
            DataTable R = new DataTable();
            R.TableName = "DataBase_Clone";
            R.Load(reader);
            reader.Close();
            if (OracleState)
                this.Close();

            return R;


        }

        public OracleDataReader DataBase_Clone_2()
        {
            if (!OracleState)
                this.Connect();

            cmd = appConn.CreateCommand();
            cmd.Parameters.Clear();
            cmd.CommandText = "CGP1_CURSORS.GetListOnStorageTSD";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("curs", OracleType.Cursor).Direction = System.Data.ParameterDirection.Output;
            OracleDataReader reader = cmd.ExecuteReader();

            return reader;


        }

        public OracleDataReader GetScalesList()
        {
            ///  --Курсор для получения списка весов П. И.В. 20140901
            ///procedure Get_Scales_List(Curs      out TCursor);

            if (!OracleState)
                this.Connect();

            cmd = appConn.CreateCommand();
            cmd.Parameters.Clear();
            cmd.CommandText = "web_monitor.Get_Scales_List";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("curs", OracleType.Cursor).Direction = System.Data.ParameterDirection.Output;
            OracleDataReader reader = cmd.ExecuteReader();

            return reader;

        }


        public OracleDataReader GetListCampMarka()
        {
            if (!OracleState)
                this.Connect();

            cmd = appConn.CreateCommand();
            cmd.Parameters.Clear();
            cmd.CommandText = "cgp1_cursors.getlistcampmarkatsd";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("curs", OracleType.Cursor).Direction = System.Data.ParameterDirection.Output;
            OracleDataReader reader = cmd.ExecuteReader();
            return reader;
        }


        public String EU_FixedWeigth(String label, String UnitScales, String UnitWeight)
        {
            try
            {
                if (!OracleState)
                    this.Connect();
                /*
                 *  procedure SetWeight(relmuch_label_ relmuch.relmuch_label%type default null, -- ЕУ
                          relmuch_prm_   relmuch.relmuch_prm%type default null, -- ЕУ     
                          tehuz_label_   tehuz.tehuz_label%type default null, -- ТУ
                          tehuz_kod_     tehuz.tehuz_kod%type default null, -- ТУ   
                          num_scales_    VESYTEHUZ.VESY_NOM%type, --номер весов
                          unit_weight_   number,
                          sign_master    pls_integer default 1, -- 1 - ТСД, 2 - АРМ, 3 - ДТ
                          automat_       pls_integer default 3)
                 * */
                cmd = appConn.CreateCommand();

                cmd.Parameters.Clear();
                cmd.CommandText = "CGP1.SetWeight";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("relmuch_label_", OracleType.NVarChar).Value = label;
                cmd.Parameters.Add("num_scales_", OracleType.NVarChar).Value = UnitScales;
                cmd.Parameters.Add("unit_weight_", OracleType.NVarChar).Value = UnitWeight;
                cmd.ExecuteNonQuery();


                if (OracleState)
                    this.Close();

                return "1";
            }

            catch (Exception ex)
            {
                return "Oracle: " + ex.Message;
            }


        }
    }
}