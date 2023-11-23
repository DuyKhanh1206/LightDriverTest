using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightLibrary
{
    abstract class ModelSettingBase
    {
        protected abstract string PollingCommand { get; set; }

        protected abstract string PollingReciveOKCommand { get; set; }

        //protected abstract string GetValue();
        //override　←must



        //protected virtual string GetValue2()
        //{
        //    return "aaaaa";
        //}
        //override　←


    }
}
