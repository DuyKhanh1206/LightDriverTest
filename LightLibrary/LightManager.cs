using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightLibrary
{
    /// <summary>照明を操作するクラス（Singleton）。このクラスにアクセスするだけで照明をコントロールできる。</summary>
    public class LightManager
    {
        #region ■Singleton■
        private static LightManager _singleton = new LightManager();
        public static LightManager getInstance() { return _singleton; }
        #endregion

        #region ■メンバ変数■
        /// <summary>LightManager設定ファイル名</summary>
        private const string LIGHT_MANAGER_SETTING_FILE = "LightManagerSetting.xml";
        /// <summary>LightManager設定XSD</summary>
        private string _lightManagerSettingXSD = Properties.Resources.LightManagerSetting;
        /// <summary>設定フォルダパス</summary>
        private string _settingFolderPath;
        /// <summary>照明電源のリスト</summary>
        private List<LightPowerBase> _lstLightPower;


        public int timingCount;
        #endregion

        /// <summary>コンストラクタ</summary>
        public LightManager()
        {
            _lstLightPower = new List<LightPowerBase>();
        }

        /// <summary>LightManager設定ファイル名の読み込み、各照明クラス設定の読み込み、リストへの登録を行う</summary>
        /// 
        // Đọc tên tệp cài đặt LightManager, đọc từng cài đặt lớp chiếu sáng và đăng ký nó vào danh sách.
        public bool Load(string sSettingFolderPath)
        {
            bool bRes = true;
            //LightManagerSetting ReadData = default(LightManagerSetting); // là 1 Class thông tin gồm 3 trường Type , Name, Path
             LightManagerSetting ReadData = new LightManagerSetting();


            if (bRes == true)//XML読み込み          Đang tải XML
            {
                string Path = System.IO.Path.Combine(sSettingFolderPath, LIGHT_MANAGER_SETTING_FILE);
                string Errormsg;
                // Hàm load bằng Class XMLDATA 
                if (CommonLibrary.XmlData.Load<LightManagerSetting>(Path, _lightManagerSettingXSD, out ReadData, out Errormsg) == false)
                {
                    bRes = false;
                    CommonLibrary.CommonMethod.DebugConsoleWriteLine(this, Errormsg);
                }
            }

            if (bRes == true)//リスト毎にインスタンス              Duyệt trên từng danh sách
            {
                foreach (var x in ReadData.lstLightClassSetting)
                {
                    LightPowerBase lpb = LightPowerInstance(x); // Creat Factory khởi tạo khai báo class xử lý từng đèn( bây giờ muốn nó chạy trên cùng 1 thread thì sao)
                    if (lpb == null)
                    {
                        //インスタンス失敗          Lỗi phiên bản
                        bRes = false;
                        break;
                    }
                    // Loading Các Light và setting / thông qua class LightPowerBase vitual Load(string path) => override Load(string path) (Class: LightPowerBaseTcp)
                    if (lpb.Load(System.IO.Path.Combine(sSettingFolderPath, x.LightClassSettingPath)) == false)
                    {
                        //読み込み失敗            Tải không thành công
                        bRes = false;
                        break;
                    }
                    // chạy Thread
                    if (lpb.StartFlowThread() == false)
                    {
                        //スレッド開始失敗          Lỗi bắt đầu Luồng
                        bRes = false;
                        break;
                    }

                    _lstLightPower.Add(lpb);
                }
               
            }

            if (bRes == true)//メンバ変数に設定パスを入力    Nhập đường dẫn cấu hình vào biến thành viên
            {
                _settingFolderPath = sSettingFolderPath;
            }

            return bRes;
        }
        // khởi tạo Factory
        /// <summary>各照明クラス設定を読み込み、インスタンスする</summary>
        private LightPowerBase LightPowerInstance(LightManagerSetting.LightClassSetting SingleSetting)
        {
            // namespace 取得
            string sNameSpace = System.Reflection.Assembly.GetExecutingAssembly().GetTypes().First(x => x.Name == GetType().Name).Namespace;

            LightPowerBase instancedCalss = null;

            //Type instanceType = null;         //--------khởi tạo có vẻ khác. nên dùng cái cũ và khởi tạo qua LightPowerBase và được kế thừa từ nó.
            switch (SingleSetting.LightClassType)
            {
                case "TCPIP":
                    {
                        switch (SingleSetting.LightClassModelName)
                        {
                            case "LEIMAC_IWDV_600M2_24":
                                instancedCalss = new LightPowerBaseTcp<LEIMAC_IWDV_600M2_24Setting>();
                                break;
                            case "LEIMAC_IWDV-600M2-25":
                                instancedCalss = new LightPowerBaseTcp<LEIMAC_IWDV_600M2_24Setting>();
                                break;
                        }
                        break;
                    }
                case "DIO?"://TCPIP以外
                    {
                        break;
                    }
            }
            return instancedCalss;
        }

        // lấy ra LightPowerBase lpb với LightPowerName truyền vào
        /// <summary>照明電源を取得する</summary>
        private bool GetLightPower(string LightPowerName, out LightPowerBase lpb)
        {
            bool bRes = true;
            lpb = _lstLightPower.Find(x => x.LightPowerBaseSetting.LightPowerName == LightPowerName);
            if (lpb == null)
            {
                bRes = false;
            }
            return bRes;
        }
        // lấy ra tên list LightCh với tên truyền vào LightChName
        /// <summary>照明を取得する</summary>
        private bool GetLight(string LightChName, LightPowerBase lpb, out List<LightPowerBaseTcpSetting.LightCh> lstLightCh)
        {
            bool bRes = true;
            lstLightCh = lpb.LightPowerBaseSetting.lstLightCh.FindAll(x => x.LightChName == LightChName);
            if (lstLightCh.Count <= 0)
            {
                bRes = false;
            }
            return bRes;
        }

        /// <summary>照明電源にリスエストする</summary>         _____ Lệnh quản lý yêu cầu gửi command để chiếu sáng tới lightpowerBase để thực hiện
        public bool RequestLightPower(List<string> lstMsg, string LightPowerName)
        {
            bool bRes = true;
            LightPowerBase lpb;
            if (GetLightPower(LightPowerName, out lpb) == true)
            {
                bRes &= lpb.Request(lstMsg);
            }
            else
            {
                bRes = false;
            }
            return bRes;
        }
        // điều khiển ánh sáng đèn 
        //--------------với data vào là tên power name/ tên cổng, và giá trị setup. tìm ra đối tượng và truyền cho class lightBase thực thi---------------
        /// <summary>照明値を変更する</summary>
        public bool LigthValueChange(string LightPowerName, string LightChName, int SetValue)
        {
            bool bRes = true;
            LightPowerBase lpb;
            List<LightPowerBaseTcpSetting.LightCh> lstLightCh;
            if (GetLightPower(LightPowerName, out lpb) == true)
            {
                if (GetLight(LightChName, lpb, out lstLightCh) == true)
                {
                    bRes &= lpb.LightValueChange(lstLightCh, SetValue);
                }
                else
                {
                    bRes = false;
                }
            }
            else
            {
                bRes = false;
            }
            return bRes;
        }
        // tắt toàn bộ
        /// <summary>全照明を終了する</summary>
        public bool AllLightTerminate()
        {
            bool bRes = true;
            foreach (var x in _lstLightPower)
            {
                bRes &= x.Terminate();
            }
            return bRes;
        }




        

        /// <summary>照明電源のイベントを登録する</summary>    
        public bool SetEvent(string LightPowerName, EventHandler<LightPowerBase.RequestReceiveEventArgs> act1, EventHandler act2)
        {
            bool bRes = true;
            LightPowerBase lpb;
            if (GetLightPower(LightPowerName, out lpb) == true) /*đăng kí sự kiện*/
            {
                lpb.EventRequestResult += act1;
                lpb.EventTerminateComplete += act2;
            }
            else
            {
                bRes = false;
            }
            return bRes;
        }
        /// <summary>照明電源のイベントを解放する</summary>
        public bool ReleaseEvent(string LightPowerName, EventHandler<LightPowerBase.RequestReceiveEventArgs> act1, EventHandler act2)
        {
            bool bRes = true;
            LightPowerBase lpb;
            if (GetLightPower(LightPowerName, out lpb) == true)         /*Giải phóng sự kện*/
            {
                lpb.EventRequestResult -= act1;
                lpb.EventTerminateComplete -= act2;
            }
            else
            {
                bRes = false;
            }
            return bRes;
        }


    }
}
