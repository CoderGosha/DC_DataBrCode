using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppDataBrCode
{
    public class Common
    {
    }
    public class Relmuch
    {
        public Relmuch()
        {

        }

        public Relmuch(string Label, int CodeAutomatic)
        {
            LABEL = Label;
            CODEAUTOMATIC = CodeAutomatic;
        }
        public string LABEL { get; set; }
        public int CODEAUTOMATIC { get; set; }
        public bool MANUAL
        {
            get
            {
                if (CODEAUTOMATIC < 5)
                    return true;
                else return false;
            }
        }
        public override string ToString()
        {
            return LABEL;
        }

    }

    public class MXPlace
    {
        public MXPlace()
        {
        }

        public MXPlace(string Label, int CodeAutomatic)
        {
            LABEL = Label;
            CODEAUTOMATIC = CodeAutomatic;
        }
        public string LABEL { get; set; }
        public int CODEAUTOMATIC { get; set; }
        public bool MANUAL
        {
            get
            {
                if (CODEAUTOMATIC < 5)
                    return true;
                else return false;
            }
        }
        public override string ToString()
        {
            return LABEL;
        }
    }
}