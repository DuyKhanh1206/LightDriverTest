using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightLibrary
{
    //■■■メモ IWDV-600M"-24：Connection Timeout:10、Standby Timeout:30で検証
    class LEIMAC_IWDV_600M2_24Setting : ModelSettingBase
    {
        public string sample { get; set; }



        ///// <summary>XML用</summary>
        //public LightPowerBaseSetting XmlUseLightPowerBaseSetting { get; set; }
        ///// <summary>XML用</summary>
        //public LightPowerBaseTcpSetting XmlUseLightPowerBaseTcpSetting { get; set; }
        /// <summary>コンストラクタ</summary>
        public LEIMAC_IWDV_600M2_24Setting()
        {
            //XmlUseLightPowerBaseSetting = new LightPowerBaseSetting();
            //XmlUseLightPowerBaseTcpSetting = new LightPowerBaseTcpSetting();


            //■▲■独自コマンドをここで上書きすること。
            //基本コマンドを上書きするだけのクラス
            //sendCommand = "aaaaaa";
            //reciveCommand = "bbbbbbbb";

        }

        protected override string PollingCommand
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        protected override string PollingReciveOKCommand
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

    }



}
