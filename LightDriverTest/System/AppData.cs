using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightDriverTest
{
    class AppData
    {
        #region ■Singleton■
        private static AppData _singleton = new AppData();

        public static AppData getInstance()
        {
            return _singleton;
        }
        #endregion

        /// <summary>テスト用に仮</summary>
        public string LightSettingPath { get; set; } = @"C:\Users\k-nguyen\Documents\Visual Studio 2015\Projects\yuasa_sample\LightDriverTest\bin\Debug\Setting\Light\";

    }
}
