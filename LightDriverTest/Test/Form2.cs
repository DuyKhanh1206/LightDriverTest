using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LightDriverTest
{
    public partial class Form2 : Form
    {
        //private LightLibrary.LightManagerSetting aaaa;
        //private LightLibrary.IWDV_600M2_24Setting cccc;

        public Form2()
        {
            InitializeComponent();
            //aaaa = new LightManagerSetting();
            //var bbbb = new LightManagerSetting.LightClassSetting();
            //bbbb.LightClassName = "class1";
            //bbbb.LightClassSettingPath = "path1";
            //aaaa.lstLightClassSetting.Add(bbbb);
            //aaaa.lstLightClassSetting.Add(bbbb);



            //string Errormsg;
            //if (Common.XmlData.Save<LightManagerSetting>(@"C:\Users\n-yuasa\Documents\Visual Studio 2015\Projects\LightDriverTest\LightDriverTest\bin\Debug\Setting\Light\LightManagerSetting", aaaa, out Errormsg) == false)
            //{
            //    Common.CommonMethod.DebugConsoleWriteLine(this, Errormsg);
            //}


            //cccc = new LightLibrary.IWDV_600M2_24Setting();
            //cccc.sample = "stringaaaaa";
            //var dddd = new LightLibrary.LightPowerBaseSetting();
            //dddd.LightPowerName = "表照明電源";

            //LightLibrary.LightPowerBaseSetting.LightCh gggg = new LightLibrary.LightPowerBaseSetting.LightCh();
            //gggg.LightChNumber = 1;
            //gggg.LightChName = "表１";
            //dddd.lstLightCh.Add(gggg);

            //LightLibrary.LightPowerBaseSetting.LightCh hhhh = new LightLibrary.LightPowerBaseSetting.LightCh();
            //hhhh.LightChNumber = 2;
            //hhhh.LightChName = "表２";
            //dddd.lstLightCh.Add(hhhh);

            //cccc.XmlUseLightPowerBaseSetting = dddd;

            //var eeee = new LightLibrary.LightPowerBaseTcpSetting();
            //eeee.TcpSetting.IpAdress = "192.168.0.30";
            //eeee.TcpSetting.PortNum = 1000;
            //eeee.TcpSetting.OpenTimeOut = 300;
            //eeee.TcpSetting.ReadTimeOut = 300;
            //eeee.TcpSetting.WriteTimeOut = 300;
            //cccc.XmlUseLightPowerBaseTcpSetting = eeee;

            //string Errormsg;
            //if (CommonLibrary.XmlData.Save<LightLibrary.IWDV_600M2_24Setting>(@"C:\Users\n-yuasa\Documents\Visual Studio 2015\Projects\LightDriverTest\LightDriverTest\bin\Debug\Setting\Light\123456.txt", cccc, out Errormsg) == false)
            //{
            //    CommonLibrary.CommonMethod.DebugConsoleWriteLine(this, Errormsg);
            //}


        }

        private void button1_Click(object sender, EventArgs e)
        {
            var a = new LightLibrary.LightPowerBaseTcpSetting();
            string Errormsg;
            if (CommonLibrary.XmlData.Save<LightLibrary.LightPowerBaseTcpSetting>(@"C:\Users\n-yuasa\Documents\Visual Studio 2015\Projects\LightDriverTest\LightDriverTest\bin\Debug\Setting\Light\123456.txt", a, out Errormsg) == false)
            {
                CommonLibrary.CommonMethod.DebugConsoleWriteLine(this, Errormsg);
            }
        }
    }
}
