using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace DataBarCode
{
    static class CLog
    {
        public static string FileName { get; set; }
        public static Int32 DayToSave { get; set; }
        public static void InitCLog()
        {
            try
            {
                //Создадим папку для логов
                Directory.CreateDirectory("Log");
            }
            catch (Exception ex)
            {

            }
           
            CleanLog();

        }

        private static void ActualFile()
        {
            string _file = "Log/";
            _file += DateTime.Now.ToString("yyyy_MM_dd");
            _file += ".txt";
            CLog.FileName = _file;
        }
        private static void CleanLog()
        {
            //Тут будет чистка логов
            foreach (var elem in Directory.GetFiles("Log"))
            {
                Debug.WriteLine(elem);
                bool del = false;
                if (File.GetCreationTime(elem) < DateTime.Now.AddDays(-DayToSave))
                {
                    del = true;
                }
                if (del)
                {
                    File.Delete(elem);
                }
            }

        }

        public static int CleanScreenShoot()
        {
            int counterDel = 0;
            //Тут будет чистка логов
            foreach (var elem in Directory.GetFiles("Log"))
            {
                Debug.WriteLine(elem);
                bool del = false;
                if (elem.IndexOf(".bmp") != -1)
                {
                    del = true;
                }
                if (del)
                {
                    File.Delete(elem);
                    counterDel++;
                }
            }
            return counterDel;

        }

        public static void WriteInfo(string Module, string Message)
        {
            ///Определяем дату и время
            ///
            string dtM = DateTime.Now.ToString();
            string WriteMessage = dtM + ";" + "INFO;" + Module + ";" + Message + ";";
            WriteFile(WriteMessage);

        }


        public static void WriteBuffer(string Message)
        {
            ///Определяем дату и время
            ///
            string dtM = DateTime.Now.ToString();
            string WriteMessage = dtM + ";" + "BUFFER;" + Message + ";";
            WriteFile(WriteMessage);

        }

        public static void WriteException(string Module, string Func, string Message)
        {
            ///Определяем дату и время
            ///
            string dtM = DateTime.Now.ToString();
            string WriteMessage = dtM + ";" + "EXCEPTION;" + Module + "Func:" + Func + ";" + Message + ";";
            WriteFile(WriteMessage);

        }

        private static void WriteFile(string Out)
        {
            //Пишем в файл
            //Проверяем актуальность файла
            CLog.ActualFile();

            try
            {
                StreamWriter writer = new StreamWriter(FileName, true, Encoding.UTF8);
                writer.WriteLine(Out);
                writer.Close();
            }

            catch (Exception ex)
            {

            }

        }
    }
}
