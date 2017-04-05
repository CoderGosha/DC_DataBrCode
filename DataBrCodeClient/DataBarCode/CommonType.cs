using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace CommonType
{
    public interface ICommonTypeEU
    {
         
    }

    public class SelectEU : ICommonTypeEU
    {
        public SelectEU(String _Label, String _YE, String _Marka, String _Razmer, Double _Weight)
        {
            Label = _Label;
            YE = _YE;
            Marka = _Marka;
            Razmer = _Razmer;
            Weight = _Weight;
        }

        public String Label {get; set;}
        public String YE {get; set;}
        public String Marka {get; set;}
        public String Razmer {get; set;}
        public Double Weight {get; set;}
    }


}
