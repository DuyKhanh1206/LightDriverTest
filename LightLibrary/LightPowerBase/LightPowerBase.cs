using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace LightLibrary
{
    /// <summary>照明電源毎に作成するクラス（基本になる抽象クラス）</summary>
    public abstract class LightPowerBase
    {
        #region ■Event■


        //___________________________________________  Phần trả lại dữ liệu rất Quan  trong_____________________________________

        //Sự kiện trả về kết quả của yêu cầu
        /// <summary>リクエストの結果を返すイベント</summary>
        public event EventHandler<RequestReceiveEventArgs> EventRequestResult;
        protected virtual void OnRequestRequested(RequestReceiveEventArgs e)
        {
            EventRequestResult?.Invoke(this, e);
        }




        public class RequestReceiveEventArgs : EventArgs
        {
            public bool ResultReceiveJudge { get; private set; }
            public string ResultReceiveOrigin { get; private set; }
            public RequestReceiveEventArgs(bool resultReceiveJudge, string resultReceiveOrigin)
            {
                ResultReceiveJudge = resultReceiveJudge;
                ResultReceiveOrigin = resultReceiveOrigin;
            }
        }


        //Sự kiện được trả về khi việc Chấm dứt hoàn tất

        /// <summary>Terminate完了した時に返すイベント</summary>
        public event EventHandler EventTerminateComplete;
        protected virtual void OnTerminateCompleted(EventArgs e)
        {
            EventTerminateComplete?.Invoke(this, e);
        }
        #endregion

        #region ■メンバ変数■
        /// <summary>スレッド周期（ミリ秒）</summary>
        private const int THREAD_PERIOD = 100;
        /// <summary>polling/Request周期（ミリ秒）</summary>
        protected const int POLLING_PERIOD = 3000;
        /// <summary>LightPowerBase設定</summary>
        public LightPowerBaseTcpSetting LightPowerBaseSetting { get; protected set; }
        /// <summary>最後のpolling/Request時間</summary>
        protected DateTime _lastPollingRequestTime;
        /// <summary>スレッドのフラグ</summary>
        private bool _bThreadFlag;
        /// <summary>スレッド</summary>
        private Thread _threadMain;

        private int _myPowerSupplyNum;

        /// <summary>フローデータ（リスト）</summary>
        protected List<FlowData> _lstFlowData;
        protected class FlowData
        {
            public FlowStatus FlowStatus { get; private set; }
            public List<string> lstMessage { get; private set; }

            public FlowData(FlowStatus flowStatus, List<string> lstMsg = default(List<string>))
            {
                FlowStatus = flowStatus;
                lstMessage = lstMsg;
            }
        }
        #endregion

        /// <summary>コンストラクタ</summary>
        public LightPowerBase()
        {
            //メンバ変数の初期化         Khởi tạo các biến thành viên add list FlowData =  Initialize. sau khi Load từng Light và chạy thread thì bắt đầu cài đặt.

            LightPowerBaseSetting = new LightPowerBaseTcpSetting();
            _lastPollingRequestTime = DateTime.Now;
            _bThreadFlag = false;
            _threadMain = null;
            _lstFlowData = new List<FlowData>();
            _lstFlowData.Add(new FlowData(FlowStatus.Initialize));

            //_myPowerSupplyNum = aaa;
        }




        #region ■Public■
        /// <summary>設定の読み込み</summary>
        public abstract bool Load(string sPath);
        /// <summary>外部からの完了受付用</summary>
        public abstract bool Terminate();

        /// <summary>照明値を変更する</summary>
        public abstract bool LightValueChange(List<LightPowerBaseTcpSetting.LightCh> lstLightCh, int SetValue);




        /// <summary>スレッドフロー開始</summary>
        public bool StartFlowThread()
        {
            bool bRes = true;
            //スレッド開始
            bRes &= StartThread();
            return bRes;
        }

        /// <summary>外部からのリクエスト受付用</summary>        Để chấp nhận yêu cầu từ bên ngoài
        public bool Request(List<string> RequestMessage)
        {
            bool bRes = true;
            //リストは2を上限で考える。             Xem xét danh sách có giới hạn trên là 2.
            if (_lstFlowData.Count >= 2)
                bRes = false;
            else
                _lstFlowData.Insert(0, new FlowData(FlowStatus.Request, RequestMessage));//先頭に追加   -------------- thêm vào đầu với flowstatus.request và list string gửi đi

            return bRes;
        }

       

        #endregion

        #region ■スレッド■
        /// <summary>スレッドの開始</summary>
        private bool StartThread()
        {
            if (true == _bThreadFlag)
                return false;
            _threadMain = new Thread(ThreadMain);
            _threadMain.Name = LightPowerBaseSetting.LightPowerName + "_Thread";
            _bThreadFlag = true;
            _threadMain.Start();
            return true;
        }

        /// <summary>スレッドの終了</summary>
        protected bool StopThread()
        {
            DateTime TimeOutTime = DateTime.Now.AddMilliseconds(3000);
            do
            {
                if (_lstFlowData[0].FlowStatus == FlowStatus.Finished)
                    break;
                Thread.Sleep(50);
            } while (TimeOutTime >= DateTime.Now);

            if (_threadMain == null)
                return true;
            _bThreadFlag = false;
            do
            {
                _threadMain.Join(100);
            } while (_threadMain.IsAlive);
            return true;
        }

        /// <summary>スレッド本体</summary>
        private void ThreadMain()
        {
            while (_bThreadFlag)
            {
                if (LightManager.getInstance().timingCount == _myPowerSupplyNum) {

                    //スレッドの周期を決めるために使用
                    DateTime StartTime = DateTime.Now;

                    //Common.CommonMethod.DebugConsoleWriteLine(this, "●●●" + _lstFlowData.Count.ToString());

                    if (_lstFlowData.Count > 0)
                    {
                        //QUEUE : firt in firt out 
                        //先頭データを1つピックアップ
                        FlowData switchData = _lstFlowData[0];
                        _lstFlowData.RemoveAt(0);

                        switch (switchData.FlowStatus)
                        {
                            case FlowStatus.Initialize:
                                {
                                    _lstFlowData.Add(new FlowData(FlowInitialize()));
                                    break;
                                }

                            case FlowStatus.Open: { _lstFlowData.Add(new FlowData(FlowOpen())); break; }
                            case FlowStatus.Polling: { _lstFlowData.Add(new FlowData(FlowPolling())); break; }
                            case FlowStatus.Close: { _lstFlowData.Add(new FlowData(FlowClose())); break; }
                            case FlowStatus.Error: { _lstFlowData.Add(new FlowData(FlowError())); Thread.Sleep(500); break; }//少し休憩

                            case FlowStatus.Request: { FlowRequest(switchData); break; }//リクエストの時は、Addしない。


                            case FlowStatus.Terminate: { _lstFlowData.Add(new FlowData(FlowTerminate(switchData))); break; }
                            case FlowStatus.Finished: { break; }//何もしない。

                            default: { break; }
                        }
                    }

                    //ここでスレッド間隔を作る。
                    Thread.Sleep(5);//最低でも5msは休む
                    DateTime EndTime = DateTime.Now;
                    TimeSpan TimePeriod = EndTime - StartTime;

                    //Common.CommonMethod.DebugConsoleWriteLine(this, "■スレッド間隔１■" + (int)TimePeriod.TotalMilliseconds);
                    if ((int)TimePeriod.TotalMilliseconds < THREAD_PERIOD)
                    {
                        Thread.Sleep(THREAD_PERIOD - (int)TimePeriod.TotalMilliseconds);
                    }
                    else
                    {
                        //Common.CommonMethod.DebugConsoleWriteLine(this, "■スレッド時間超過■");
                    }
                    EndTime = DateTime.Now;
                    TimePeriod = EndTime - StartTime;
                    //Common.CommonMethod.DebugConsoleWriteLine(this, "■スレッド間隔２■" + (int)TimePeriod.TotalMilliseconds);
                }

                //timingCount++;///over powersupply is 0

            }
        }
        #endregion

        #region ■Flow■
        /// <summary>フローステータスのenum</summary>
        public enum FlowStatus
        {
            Initialize,
            Open,
            Polling,
            Close,
            Error,
            /// <summary>外部からのリクエスト</summary>
            Request,
            Terminate,
            None,
            /// <summary>アプリ終了時のステータス</summary>
            Finished,
        }

        /// <summary>各種設定を初期化する（抽象メソッド、継承先で実装する）</summary>
        protected abstract FlowStatus FlowInitialize();

        /// <summary>オープンする</summary>
        protected abstract FlowStatus FlowOpen();

        /// <summary>ポーリングする</summary>
        protected abstract FlowStatus FlowPolling();

        /// <summary>リクエストする</summary>
        protected abstract void FlowRequest(FlowData flowData);

        /// <summary>クローズする/summary>
        protected abstract FlowStatus FlowClose();

        /// <summary>エラー発生時</summary>
        protected abstract FlowStatus FlowError();

        /// <summary>フローを終了する</summary>
        protected abstract FlowStatus FlowTerminate(FlowData TerminateCommand);
        #endregion
    }
}
