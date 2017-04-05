using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace DataBarCode
{
    static class ValidateList
    {

        public static bool CheckEUByList(List<String> EUInTAble, string EUScan)
        {//Проверяем есть ли данная ЕУ в списке
            if (EUInTAble == null) 
                return false;

            if (EUInTAble.IndexOf(EUScan) == -1) return false;
            else return true;
           
        }

        public static bool CheckEUByListType(List<WebReference.Relmuch> EUInTAble, string EUScan)
        {//Проверяем есть ли данная ЕУ в списке
            if (EUInTAble == null)
                return false;

            bool find = false;
            foreach (var elem in EUInTAble)
            {
                if (elem.LABEL == EUScan)
                {
                    find = true;
                    break;
                }
            }
            return find;

        }
    }
}
