using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using OpenNETCF.Net.NetworkInformation;
using System.Net;

namespace DataBarCode
{
    static class CBrHeader
    {
        static public string Login { get; set; }
        static public string Domen { get; set; }
        static public string Password { get; set; }
        static public string DnsAdress { get; set; }
        static public string MacAdress { get; set; }
        static public string ClientVersion { get; set; }
        static public void Init()
        {///Тут мы проиницилизируем все переменные
            MacAdress = "00:00:00:00";
            DnsAdress = "--";
            try
            {
                //Тут определим ДНС и МАК
                INetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
                foreach (var iter in interfaces)
                {
                    MacAdress = iter.GetPhysicalAddress().ToString();
                }
                DnsAdress = Dns.GetHostName();
            }

            catch (Exception) {}
            ClientVersion = "0.198";

        }
        static public WebReference.BrHeader GetHeader()
        {
            WebReference.BrHeader _BrHeader = new WebReference.BrHeader();
            _BrHeader.Login = Login;
            _BrHeader.Password = Password;
            _BrHeader.DnsAdress = DnsAdress;
            _BrHeader.MacAdress = MacAdress;
            _BrHeader.ClientVersion = ClientVersion;
            return _BrHeader;
        }
    }
}
