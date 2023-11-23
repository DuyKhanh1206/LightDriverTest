using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightLibrary
{
    //■▲■T型で、「LEIMAC_IWDV_600M2_24Setting」なのか「LEIMAC_IDGB」なのかを決める。
    class LightPowerBaseTcp<T> : LightPowerBase where T : ModelSettingBase
    {
        /// <summary>LightManager設定XSD</summary>
        private string _XSD = Properties.Resources.LightSingle;
        /// <summary>IWDV_600M2_24設定</summary>
        private T _modelSetting;

      
        /// <summary>TCP/IP用のドライバ</summary>
        protected DriverLibrary.TcpClientDriver _tcpClientDriver;
        /// <summary>LightPowerBaseTcp設定</summary>
        protected LightPowerBaseTcpSetting _lightPowerBaseTcpSetting;

        /// <summary>コンストラクタ</summary>
        public LightPowerBaseTcp()
        {
            //メンバ変数の初期化
            _tcpClientDriver = new DriverLibrary.TcpClientDriver();
            //_lightPowerBaseTcpSetting = new LightPowerBaseTcpSetting();

            //メンバ変数の初期化
            //_LEIMAC_IWDV_600M2_24Setting = new T();
        }

        override public bool Load(string sPath)

        {
            bool bRes = true;
            //XML読み込み
            string Errormsg;
            LightPowerBaseTcpSetting ReadData;
            //------------ tên class LightPowerBaseTcpSetting phỉ trùng với tên trong file và cùng tên với _XSD khi check định dạng----------------
            //------------ nên phải để tạm là LightPowerBaseTcpSetting và đổi tên Class thật để chạy cho gọn chương trình-------------------------
            if (CommonLibrary.XmlData.Load<LightPowerBaseTcpSetting>(sPath, _XSD, out ReadData, out Errormsg) == false)
            {
                bRes = false;
                CommonLibrary.CommonMethod.DebugConsoleWriteLine(this, Errormsg);
            }
            else
            {
                //LEIMAC_IWDV_600M2_24Setting a= new LEIMAC_IWDV_600M2_24Setting();
                //
                // gán dữ liệu đọc được để dùng ở lớp này các thông tin như : teenCh , số CH
                _lightPowerBaseTcpSetting = ReadData;

                //_setting.LightCh

                //LightPowerBaseSetting = _setting.
                //_lightPowerBaseTcpSetting = _setting.LightPowerName
                ////var a = _setting.LightPowerName;
                ////LEIMAC_IWDV_600M2_24Setting aaa = ReadData as LEIMAC_IWDV_600M2_24Setting;
                ////if (aaa != null)
                ////{
                ////    aaa.LightPowerName;
                ////}
                

                //_setting.TcpSetting
            }
            return bRes;
        }

        override public bool Terminate() // buộc dừng và tắt toàn bộ đèn do con người tác động tắt
        {
            bool bRes = true;

            if (bRes == true)
            {
                _lstFlowData.Insert(0, new FlowData(FlowStatus.Terminate, new List<string>() { "W12010000020000", "W12ACK " }));//先頭に追加 thêm vào đầu
            }
            if (bRes == true)
            {
                if (StopThread() == true)// Viêc stopThread và truyền lệnh có kịp hay không
                {
                    //画面に通達     thông báo trên màn hình
                    OnTerminateCompleted(new EventArgs());
                }
                else
                {
                    bRes = false;
                }
            }
            return bRes;
        }

        override public bool LightValueChange(List<LightPowerBaseTcpSetting.LightCh> lstLightCh, int SetValue)
        {
            //int iRealValue = (int)(SetValue * (999.0 / 255.0));

            string RequesMsg = "W12";
            string OKResultMsg = "W12ACK ";
            for (int i = 1; i <= 2; i++)
            {
                if (lstLightCh.FindIndex(x => x.LightChNumber == i) != -1)
                {
                    RequesMsg += i.ToString("00") + SetValue.ToString("0000");
                }
            }
            //Request(RequesMsg);

            List<string> lstRequestMsg = new List<string>();
            lstRequestMsg.Add(RequesMsg);   /*lệnh để gửi đi điều khiển đèn*/
            lstRequestMsg.Add(OKResultMsg); /*list[1] dùng để đưa ra kết quả so sánh với kết quả nhận được từ power supply*/

            return Request(lstRequestMsg);
        }

        override protected void OnRequestRequested(RequestReceiveEventArgs e)
        {
            base.OnRequestRequested(e);
        }

        override protected void OnTerminateCompleted(EventArgs e)
        {
            base.OnTerminateCompleted(e);
        }

        override protected FlowStatus FlowInitialize()
        {
            CommonLibrary.CommonMethod.DebugConsoleWriteLine(this, "■■■Initialize■■■");

            //設定クラスの初期化（Load）Khởi tạo lớp cấu hình
            for (int i = 0; i < 3; i++)
            {
                return _tcpClientDriver.SettingLoad(_lightPowerBaseTcpSetting.TcpSetting) ? FlowStatus.Open : FlowStatus.Error;                
            }
            return FlowStatus.Terminate;  

        }
        override protected FlowStatus FlowOpen()
        {
            CommonLibrary.CommonMethod.DebugConsoleWriteLine(this, "■■■Open■■■");

            return _tcpClientDriver.Open() ? FlowStatus.Polling : FlowStatus.Error;
        }
        override protected FlowStatus FlowPolling()
        {
            bool bRes = true;

            DateTime NowTime = DateTime.Now;
            if ((int)(NowTime - _lastPollingRequestTime).TotalMilliseconds >= POLLING_PERIOD) // ít nhất sau 3000 mili second mới polling lại tính từ thời điểm cuối cùng 
            {
                _lastPollingRequestTime = NowTime;

                CommonLibrary.CommonMethod.DebugConsoleWriteLine(this, "■■■Polling■■■");

                string sResult = string.Empty;
                for (int i = 0; i < 3; i++)//リトライ
                {
                    if (_tcpClientDriver.Request("R080000", out sResult) == true) // lệnh kiểm tra trạng thái
                    {
                        bRes = true;
                        break;
                    }
                    bRes = false;
                }

                if (bRes == true)
                {
                    if (sResult != "R08010000020000030002040002")
                        bRes = false;
                }
            }

            return bRes ? FlowStatus.Polling : FlowStatus.Error;
        }
        override protected void FlowRequest(FlowData flowData)
        {
            CommonLibrary.CommonMethod.DebugConsoleWriteLine(this, "■■■Request■■■");

            bool bRes = true;
            string sResult = string.Empty;
            for (int i = 0; i < 3; i++)//リトライ thử lại 3 lần
            {
                if (_tcpClientDriver.Request(flowData.lstMessage[0], out sResult) == true)
                {
                    bRes = true;
                    break;
                }
                bRes = false;
            }
            if (bRes == true)
            {
                _lastPollingRequestTime = DateTime.Now;
                bool resultReceiveJudge = true;
                if (flowData.lstMessage.Count >= 2)
                {
                    resultReceiveJudge = flowData.lstMessage[1] == sResult;
                }
                OnRequestRequested(new RequestReceiveEventArgs(resultReceiveJudge, sResult));
            }
            else
            {
                _lstFlowData.Insert(0, new FlowData(FlowStatus.Error));
            }
        }
        override protected FlowStatus FlowClose()
        {
            CommonLibrary.CommonMethod.DebugConsoleWriteLine(this, "■■■Close■■■");

            bool bRes = true;

            //一旦閉じる
            bRes &= _tcpClientDriver.Close();
            _lstFlowData.Clear();
            return bRes ? FlowStatus.Terminate : FlowStatus.Error;
        }
        override protected FlowStatus FlowError()
        {
            CommonLibrary.CommonMethod.DebugConsoleWriteLine(this, "■■■Error■■■");
            //ログ生成等

            //一旦閉じる
            _tcpClientDriver.Close();
            _lstFlowData.Clear();

            //再度初期化からやりなおし。
            return FlowStatus.Initialize;
        }
        override protected FlowStatus FlowTerminate(FlowData TerminateCommand)
        {
            CommonLibrary.CommonMethod.DebugConsoleWriteLine(this, "■■■Terminate■■■");

            string sResult = string.Empty;
            for (int i = 0; i < 3; i++)//リトライ
            {
                if (_tcpClientDriver.Request(TerminateCommand.lstMessage[0], out sResult) == true)
                {
                    break;
                }
            }

            _tcpClientDriver.Close();
            _lstFlowData.Clear();

            return FlowStatus.Finished;
        }
    }
}
