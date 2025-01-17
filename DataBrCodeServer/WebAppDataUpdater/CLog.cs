﻿using System;
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
        public static string PathDir { get; set; }
        public static void InitCLog()
        {
            String AppData = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();
            try
            {
                //Создадим папку для логов
                Directory.CreateDirectory(AppData + "\\WebServiceLog");
            }
            catch (Exception) { }
            PathDir = AppData + "\\WebServiceLog\\";
            DayToSave = 100;
            CleanLog();
            
        }

        private static void ActualFile()
        {
            string _file = PathDir;
            _file += DateTime.Now.ToString("yyyy_MM_dd");
            _file += ".txt";
            CLog.FileName = _file;
        }
        private static void CleanLog()
        {
            List<string> Elems = new List<string>();
            for (int i=0; i <= DayToSave; i++) {
                   string _file;
                    _file = DateTime.Now.AddDays(-i).ToString("yyyy_MM_dd");
                    _file += ".txt";
                    //Debug.WriteLine(_file);
                    Elems.Add(_file);
            }
            //Тут будет чистка логов
            foreach (var elem in Directory.GetFiles(PathDir))
            {
                Debug.WriteLine(elem);
                bool del = true;
                foreach (var e in Elems) {
                    if (elem.IndexOf(e) != -1)
                    {
                        del = false;
                        break;
                    }
                }

                if (del)
                {
                    //Debug.WriteLine("Delete: " + elem);
                    File.Delete(elem);
                }
            }

        }

        public static void WriteInfo(string Module, string Message)
        {
            InitCLog();
            ///Определяем дату и время
            ///
            string dtM = DateTime.Now.ToString();
            string WriteMessage = dtM + ";" + "INFO;" + Module + ";" + Message + ";";
            WriteFile(WriteMessage);

        }

        public static void WriteException(string Module, string Func, string Message)
        {
            InitCLog();
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
