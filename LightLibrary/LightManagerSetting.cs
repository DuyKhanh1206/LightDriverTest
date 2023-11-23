using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightLibrary
{
    /// <summary>LightManager設定クラス</summary>
    public class LightManagerSetting
    {
        /// <summary>コンストラクタ</summary>
        /// 
        // LightManagerSetting được khai báo thì nó khởi tạo 1 list "lstLightClassSetting"  với các trường dữ liệu là lớp "LightClassSetting" có 3 tham số "Type", "Name", "Path"
        public LightManagerSetting()
        {
            lstLightClassSetting = new List<LightClassSetting>();
        }
        /// <summary>照明クラス（単体）設定のリスト</summary>
        public List<LightClassSetting> lstLightClassSetting { get; set; }
       

        /// <summary>照明クラス（単体）設定</summary>
        public class LightClassSetting
        {
            /// <summary>照明クラスのタイプ（TCPIP等）</summary>
            public string LightClassType { get; set; }
            /// <summary>照明クラスの型式（LEIMAC_IWDV-600M2-24等）</summary>
            public string LightClassModelName { get; set; }
            /// <summary>照明クラス設定パス（相対パス）</summary>
            public string LightClassSettingPath { get; set; }
        }
    }
}
