using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightLibrary
{
    public class LightPowerBaseSetting
    {
        /// <summary>照明電源名称</summary>
        public string LightPowerName { get; set; }

        /// <summary>コンストラクタ</summary>
        public LightPowerBaseSetting()
        {
            lstLightCh = new List<LightCh>();
            TcpSetting = new DriverLibrary.TcpClientDriverSetting();
        }


        /// <summary>照明Ch毎の設定</summary>
        public List<LightCh> lstLightCh { get; set; }
        public DriverLibrary.TcpClientDriverSetting TcpSetting { get; set; }
        public class LightCh
        {
            /// <summary>照明Ch番号</summary>        số cổng     eg.ChNumber = 2
            public int LightChNumber { get; set; }      // Ch number 01 , 02 ,
            /// <summary>照明単体の名前</summary>    
            public string LightChName { get; set; }         // light top, down
        }


        //----------Có thể gộp Class k cần thiết vào 1 class như phần  đã " // " bên trên.--------------
        //---------------Gộp hết thông tin lại được-------------
        // -------------- có tách thì chỉ tách phần kết nối TCP-------------------
        //-------------- phần class LightPowerBaseSetting  và LightPowerBaseTcpSetting gộp lại load vẫn bao gồm TCPIP và CH-----------


        //Class LightPowerBaseTcpSetting

        //public DriverLibrary.TcpClientDriverSetting TcpSetting { get; set; }

        ///// <summary>コンストラクタ</summary>
        ///// khởi tạo class TcpSetting với các thuộc tính có ở TcpClientDriverSetting : IpAdress, PortNum, OpenTimeOut, ReadTimeOut, WriteTimeOut

        //public LightPowerBaseTcpSetting()
        //{
        //    TcpSetting = new DriverLibrary.TcpClientDriverSetting();
        //}


       


    }
}
