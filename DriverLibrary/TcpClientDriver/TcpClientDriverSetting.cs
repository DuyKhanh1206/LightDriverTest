using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLibrary
{
    public class TcpClientDriverSetting
    {
        /// <summary>IPアドレス</summary>
        public string IpAdress { get; set; }
        /// <summary>ポート番号</summary>
        public int PortNum { get; set; }
        /// <summary>オープンタイムアウト</summary>
        public int OpenTimeOut { get; set; }
        /// <summary>読取タイムアウト</summary>
        public int ReadTimeOut { get; set; }
        /// <summary>書込タイムアウト</summary>
        public int WriteTimeOut { get; set; }
    }
}
