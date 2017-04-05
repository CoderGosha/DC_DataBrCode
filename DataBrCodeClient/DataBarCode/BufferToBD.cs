using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Data;
using System.Net;

namespace DataBarCode
{
    public enum TypeClassBuffer { POST_EU_LIST_Warehouse = 1, POST_EU_LIST_TASKMOVE = 2, POST_EU_IN_AGR = 3, POST_EU_LIST_SHIP = 4, POST_EU_LIST_RZDN_AGR = 5, POST_EU_LIST_INVERT_MX = 6, Test }
    static class BufferToBD
    {
        static private int countBuffer = 0;
        static public string CountBuffer
        {
            get
            {
                if (countBuffer < 0)
                    countBuffer = 0;
                return countBuffer.ToString();
            }

        }

        static public int CountBufferI
        {
            get
            {
                if (countBuffer < 0)
                    countBuffer = 0;
                return countBuffer;
            }

        }

        static private bool _ModeNetTerminal = true;
        static public bool ModeNetTerminalB
        {
            get
            {
                return _ModeNetTerminal;
            }
            set
            {
                _ModeNetTerminal = value;
                string M = "";
                if (value)
                    M = "ONLINE";
                else M = "OFFLINE";

                if (_ModeNetTerminal != value)
                    CLog.WriteBuffer("Переходим в режим: " + M);
                _ModeNetTerminal = value;
            }
        }

        static public string ModeNetTerminalS
        {
            get
            {
                if (_ModeNetTerminal)
                    return "ONLINE";
                else return "OFFLINE";
            }
        }

        static private List<BufferOperation> BufferOracle = null;
        static private Thread ReadTh = null;
        static private bool ReadThBool = true;

        static public void InitReadBuffer()
        {
            CLog.WriteBuffer("Включаем буффер");
            ReadThBool = true;
            ReadTh = new Thread(ReadBuffer);
            ReadTh.Start();
        }


        static public void StopReadBuffer()
        {
            //Очистка буфера
            BufferOracle = null;
            countBuffer = 0;
            try
            {
                ReadThBool = false;
                ReadTh.Abort();
            }
            catch (Exception) { }
            finally
            {
                ReadTh = null;
            }
        }

        static public bool testConnect(string AddressServer)
        {
            try
            {
                WebReference.WebSDataBrCode BrServer = new WebReference.WebSDataBrCode();
                BrServer.SoapVersion = System.Web.Services.Protocols.SoapProtocolVersion.Soap12;
                BrServer.BrHeaderValue = CBrHeader.GetHeader();
                BrServer.Credentials = new NetworkCredential(CBrHeader.Login, CBrHeader.Password);
                BrServer.Url = AddressServer;
                string message = "Terminal OnLine; BufferCount: " + CountBuffer;
                bool result = BrServer.Test_Connect(message);
                return result;
            }

            catch (System.Net.WebException)
            {

                return false;
            }


            catch (System.Net.Sockets.SocketException)
            {//На случай если во время выполнения сломается связть 

                return false;

            }

        }

        static private void ReadBuffer()
        {
            Settings set;
            set = new Settings("DataBrCode.xml");

            while (ReadThBool)
            {//А тут занимаеся чтением буффера 

                try
                {
                    if (BufferOracle != null)
                    {
                        if (countBuffer > 0)
                        {
                            if (_ModeNetTerminal)
                            {//Если мы в онлайн то работаем
                                try
                                {
                                    foreach (var elem in BufferOracle)
                                    {
                                        switch (elem.TypeOperation)
                                        {
                                            case TypeClassBuffer.POST_EU_LIST_Warehouse:
                                                {///Операция размещения

                                                    CLog.WriteBuffer("Операция POST_EU_LIST_Warehouse");
                                                    BufferOper_POST_EU_LIST_Warehouse e = (BufferOper_POST_EU_LIST_Warehouse)elem.BufOperatoon;
                                                    e.StartOperation(set.AdressAppServer);
                                                    //Удаление и вычитание
                                                    BufferOracle.Remove(elem);
                                                    countBuffer--;

                                                    break;
                                                }

                                            case TypeClassBuffer.POST_EU_LIST_TASKMOVE:
                                                {///Операция размещения

                                                    CLog.WriteBuffer("Операция POST_EU_LIST_TASKMOVE");
                                                    BufferOper_POST_EU_LIST_TASKMOVE e = (BufferOper_POST_EU_LIST_TASKMOVE)elem.BufOperatoon;
                                                    e.StartOperation(set.AdressAppServer);
                                                    //Удаление и вычитание
                                                    BufferOracle.Remove(elem);
                                                    countBuffer--;

                                                    break;
                                                }

                                            case TypeClassBuffer.POST_EU_IN_AGR:
                                                {///Операция размещения

                                                    CLog.WriteBuffer("Операция POST_EU_IN_AGR");
                                                    BufferOper_POST_EU_IN_AGR e = (BufferOper_POST_EU_IN_AGR)elem.BufOperatoon;
                                                    e.StartOperation(set.AdressAppServer);
                                                    //Удаление и вычитание
                                                    BufferOracle.Remove(elem);
                                                    countBuffer--;

                                                    break;
                                                }

                                            case TypeClassBuffer.POST_EU_LIST_SHIP:
                                                {///Операция размещения

                                                    CLog.WriteBuffer("Операция POST_EU_LIST_SHIP");
                                                    BufferOper_POST_EU_LIST_SHIP e = (BufferOper_POST_EU_LIST_SHIP)elem.BufOperatoon;
                                                    e.StartOperation(set.AdressAppServer);
                                                    //Удаление и вычитание
                                                    BufferOracle.Remove(elem);
                                                    countBuffer--;

                                                    break;
                                                }

                                            case TypeClassBuffer.POST_EU_LIST_RZDN_AGR:
                                                {///Операция размещения

                                                    CLog.WriteBuffer("Операция POST_EU_LIST_RZDN_AGR");
                                                    BufferOper_POST_EU_LIST_RZDN_AGR e = (BufferOper_POST_EU_LIST_RZDN_AGR)elem.BufOperatoon;
                                                    e.StartOperation(set.AdressAppServer);
                                                    //Удаление и вычитание
                                                    BufferOracle.Remove(elem);
                                                    countBuffer--;

                                                    break;
                                                }


                                            case TypeClassBuffer.POST_EU_LIST_INVERT_MX:
                                                {///Операция размещения

                                                    CLog.WriteBuffer("Операция POST_EU_LIST_INVERT_MX");
                                                    BufferOper_POST_EU_LIST_INVERT_MX e = (BufferOper_POST_EU_LIST_INVERT_MX)elem.BufOperatoon;
                                                    e.StartOperation(set.AdressAppServer);
                                                    //Удаление и вычитание
                                                    BufferOracle.Remove(elem);
                                                    countBuffer--;

                                                    break;
                                                }
                                        }
                                        ///Для теста
                                        Thread.Sleep(100);
                                    }
                                }

                                catch (System.Net.WebException)
                                {//На случай если во время выполнения сломается связть 

                                    ModeNetTerminalB = false;

                                }


                                catch (System.Net.Sockets.SocketException)
                                {//На случай если во время выполнения сломается связть 

                                    ModeNetTerminalB = false;

                                }
                            }

                            else
                            {//тут тестируем онлайн мы или нет и устанваливаем статус

                                _ModeNetTerminal = testConnect(set.AdressAppServer);
                                if (!_ModeNetTerminal)
                                    Thread.Sleep(1000);
                            }
                        }

                    }

                    Thread.Sleep(100);
                }
                catch (Exception ex)
                {
                    CLog.WriteException("BufferToBD", "ReadBuffer", ex.Message);
                }
            }


        }

        static public void BufferAdd(BufferOperation bo)
        {
            if (BufferOracle == null)
                BufferOracle = new List<BufferOperation>();

            BufferOracle.Add(bo);
            countBuffer++;
        }
    }
    //Тут запилим наследование 
    public class BufferOperation
    {
        protected DateTime _OperationTime;
        public DateTime OperationTime
        {
            get
            {
                return _OperationTime;
            }
        }
        public TypeClassBuffer TypeOperation { get; set; }
        public IBufferOperationOracle BufOperatoon { get; set; }

        public BufferOperation(TypeClassBuffer TypeOperation, IBufferOperationOracle BufOperatoon)
        {
            this._OperationTime = DateTime.Now;
            this.TypeOperation = TypeOperation;
            this.BufOperatoon = BufOperatoon;
        }

    }
    public interface IBufferOperationOracle
    {
        
    }
    public class BufferOper_POST_EU_LIST_Warehouse : IBufferOperationOracle
    {

        public WebReference.Relmuch[] label;
        public WebReference.MXPlace Place;
        public DateTime TimeOperation;

        public BufferOper_POST_EU_LIST_Warehouse(WebReference.MXPlace Place, WebReference.Relmuch[] label)
        {
            this.label = label;
            this.Place = Place;
            TimeOperation = DateTime.Now;
        }

        public void StartOperation(String AdressAppServer)
        {
            WebReference.WebSDataBrCode BrServer = new WebReference.WebSDataBrCode();
            BrServer.SoapVersion = System.Web.Services.Protocols.SoapProtocolVersion.Soap12;
            BrServer.Url = AdressAppServer;
            BrServer.BrHeaderValue = CBrHeader.GetHeader();
            BrServer.Credentials = new NetworkCredential(CBrHeader.Login, CBrHeader.Password);
            DataTable result = BrServer.POST_EU_LIST_WAREHOUSE_TYPE(label, Place, TimeOperation);

        }

    }

    public class BufferOper_POST_EU_LIST_TASKMOVE : IBufferOperationOracle
    {

        public WebReference.Relmuch[] label;
        public WebReference.MXPlace Place;
        public DateTime TimeOperation;

        public BufferOper_POST_EU_LIST_TASKMOVE(WebReference.MXPlace Place, WebReference.Relmuch[] label)
        {
            this.label = label;
            this.Place = Place;
            TimeOperation = DateTime.Now;
        }

        public void StartOperation(String AdressAppServer)
        {

        }

    }

    public class BufferOper_POST_EU_IN_AGR : IBufferOperationOracle
    {

        public WebReference.Relmuch[] label;
        public WebReference.MXPlace Agr;
        public DateTime TimeOperation;

        public BufferOper_POST_EU_IN_AGR(WebReference.MXPlace Agr, WebReference.Relmuch[] label)
        {
            this.label = label;
            this.Agr = Agr;
            TimeOperation = DateTime.Now;
        }

        public void StartOperation(String AdressAppServer)
        {
            WebReference.WebSDataBrCode BrServer = new WebReference.WebSDataBrCode();
            BrServer.SoapVersion = System.Web.Services.Protocols.SoapProtocolVersion.Soap12;
            BrServer.Url = AdressAppServer;
            BrServer.BrHeaderValue = CBrHeader.GetHeader();
            BrServer.Credentials = new NetworkCredential(CBrHeader.Login, CBrHeader.Password);
            DataTable result = BrServer.POST_EU_IN_AGR_TYPE(label, Agr, TimeOperation);

        }

    }

    public class BufferOper_POST_EU_LIST_SHIP : IBufferOperationOracle
    {

        public WebReference.Relmuch[] label;
        public string Rzdn;
        public DateTime TimeOperation;

        public BufferOper_POST_EU_LIST_SHIP(string RZDN, WebReference.Relmuch[] label)
        {
            this.label = label;
            this.Rzdn = RZDN;
            TimeOperation = DateTime.Now;
        }

        public void StartOperation(String AdressAppServer)
        {
            WebReference.WebSDataBrCode BrServer = new WebReference.WebSDataBrCode();
            BrServer.SoapVersion = System.Web.Services.Protocols.SoapProtocolVersion.Soap12;
            BrServer.Url = AdressAppServer;
            BrServer.BrHeaderValue = CBrHeader.GetHeader();
            BrServer.Credentials = new NetworkCredential(CBrHeader.Login, CBrHeader.Password);
            DataTable result = BrServer.POST_EU_LIST_SHIP_TYPE(label, Rzdn, TimeOperation);

        }

    }

    public class BufferOper_POST_EU_LIST_RZDN_AGR : IBufferOperationOracle
    {

        public WebReference.Relmuch[] label;
        public string Rzdn;
        public DateTime TimeOperation;

        public BufferOper_POST_EU_LIST_RZDN_AGR(string RZDN, WebReference.Relmuch[] label)
        {
            this.label = label;
            this.Rzdn = RZDN;
            TimeOperation = DateTime.Now;
        }

        public void StartOperation(String AdressAppServer)
        {
            WebReference.WebSDataBrCode BrServer = new WebReference.WebSDataBrCode();
            BrServer.SoapVersion = System.Web.Services.Protocols.SoapProtocolVersion.Soap12;
            BrServer.Url = AdressAppServer;
            BrServer.BrHeaderValue = CBrHeader.GetHeader();
            BrServer.Credentials = new NetworkCredential(CBrHeader.Login, CBrHeader.Password);
            DataTable result = BrServer.POST_EU_LIST_RZDN_AGR_TYPE(label, Rzdn, TimeOperation);

        }

    }

    public class BufferOper_POST_EU_LIST_INVERT_MX : IBufferOperationOracle
    {

        public string Rzdn;
        public WebReference.Relmuch[] label;
        public WebReference.MXPlace Place;
        public DateTime TimeOperation;

        public BufferOper_POST_EU_LIST_INVERT_MX(WebReference.MXPlace Place, string RZDN, WebReference.Relmuch[] label)
        {
            this.label = label;
            this.Rzdn = RZDN;
            this.Place = Place;
            TimeOperation = DateTime.Now;
        }

        public void StartOperation(String AdressAppServer)
        {
            WebReference.WebSDataBrCode BrServer = new WebReference.WebSDataBrCode();
            BrServer.SoapVersion = System.Web.Services.Protocols.SoapProtocolVersion.Soap12;
            BrServer.Url = AdressAppServer;
            BrServer.BrHeaderValue = CBrHeader.GetHeader();
            BrServer.Credentials = new NetworkCredential(CBrHeader.Login, CBrHeader.Password);
            DataTable result = BrServer.POST_EU_LIST_INVERT_MX_TYPE(label, Rzdn, Place, TimeOperation);

        }

    }
}
