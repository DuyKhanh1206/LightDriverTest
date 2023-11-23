using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightLibrary
{
    public class LightPowerBaseTcpSetting : LightPowerBaseSetting
    {
        // <summary>TCP設定</summary>
        public DriverLibrary.TcpClientDriverSetting TcpSetting { get; set; }

        /// <summary>コンストラクタ</summary>
        /// khởi tạo class TcpSetting với các thuộc tính có ở TcpClientDriverSetting : IpAdress, PortNum, OpenTimeOut, ReadTimeOut, WriteTimeOut
        public LightPowerBaseTcpSetting()
        {
            TcpSetting = new DriverLibrary.TcpClientDriverSetting();
        }

    }
}
