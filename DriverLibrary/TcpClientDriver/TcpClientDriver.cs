using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLibrary
{
    /// <summary>サーバにクライアントとして接続するための基本クラス</summary>
    public class TcpClientDriver
    {
        /// <summary>TCPクライアント</summary>
        private System.Net.Sockets.TcpClient _tcp;
        /// <summary>ネットワークストリーム</summary>
        private System.Net.Sockets.NetworkStream _ns;
        /// <summary>ドライバのセッティング</summary>
        private TcpClientDriverSetting _Setting;

        /// <summary>コンストラクタ</summary>
        public TcpClientDriver()
        {
            _tcp = null;
            _ns = null;
            _Setting = null;
        }

        /// <summary>設定Load</summary>       tải và cài đặt
        public bool SettingLoad(TcpClientDriverSetting Setting)
        {
            bool bRes = true;
            _tcp = new System.Net.Sockets.TcpClient();
            _Setting = new TcpClientDriverSetting();
            _Setting = Setting;
            //bRes &= SettingFileLoad(sPath, _XsdString);   Kiểm tra file tcp setting có đúng định dạng file XML hay không
            return bRes;
        }

        /// <summary>オープンする</summary>
        public bool Open()
        {
            //オープンタイムアウト
            bool bRes = true;
            try
            {
                if (_tcp.ConnectAsync(_Setting.IpAdress, _Setting.PortNum).Wait(_Setting.OpenTimeOut) == false) // chờ tối đa kết nối là bao lâu. nếu quá thì coi như thất bại
                {
                    
                    CommonLibrary.CommonMethod.DebugConsoleWriteLine(this, "■TimeOut■"); // ghi ra thông báo

                    bRes = false;
                }
                else
                {
                    CommonLibrary.CommonMethod.DebugConsoleWriteLine(this, "■ConnectOK■");// ghi ra thông báo
                    //TcpClientを作成し、サーバーと接続する
                    //_tcp = new System.Net.Sockets.TcpClient(_Setting.IpAdress, _Setting.PortNum);

                    //NetworkStreamを取得する
                    _ns = _tcp.GetStream();

                    //読み取り、書き込みのタイムアウトを設定する（デフォルトは、Infinite）
                    _ns.ReadTimeout = _Setting.ReadTimeOut;
                    _ns.WriteTimeout = _Setting.WriteTimeOut;
                }
            }
            catch (Exception ex)
            {
                //相手先電源OFF：HResult = -2147467261
                bRes = false;
            }
            return bRes;
        }

        /// <summary>クローズする</summary>
        public bool Close()
        {
            bool bRes = true;
            try
            {
                //閉じる
                _ns.Close();
                _tcp.Close();
            }
            catch (Exception ex)
            {
                bRes = false;
            }
            _ns = null;
            _tcp = null;
            _Setting = null;

            return bRes;
        }

        /// <summary>サーバにリクエストする（コマンド書き込み→データ読み取り）</summary>
        /// GỬI lệnh và đọc kết quả trả về nếu gửi lệnh thành công
        public bool Request(string sendMsg, out string reciveMsg)
        {
            bool bRes = true;
            reciveMsg = string.Empty;

            if (bRes == true)
            {
                bRes &= Write(sendMsg);
            }

            if (bRes == true)
            {
                bRes &= Read(out reciveMsg);
            }
            return bRes;
        }

        #region ■privateメソッド■
        /// <summary>サーバから受信する（private）</summary>
        private bool Read(out string reciveMsg)
        {
            bool bRes = true;
            reciveMsg = string.Empty;

            //文字列をByte型配列に変換        Chuyển đổi chuỗi thành mảng kiểu Byte
            System.Text.Encoding enc = System.Text.Encoding.UTF8;

            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                byte[] resBytes = new byte[256];
                int resSize = 0;
                do
                {
                    try
                    {
                        //データの一部を受信する       nhận một số dữ liệu
                        resSize = _ns.Read(resBytes, 0, resBytes.Length);
                    }
                    catch (Exception ex)
                    {
                        if (ex.HResult == -2146232800)//■IWDV固有かも？
                        {
                            CommonLibrary.CommonMethod.DebugConsoleWriteLine(this, "タイムアウトしました。");
                        }
                        else
                        {
                            CommonLibrary.CommonMethod.DebugConsoleWriteLine(this, "読み込みエラーが発生しました。" + ex.HResult);
                        }
                        bRes = false;
                        break;
                    }

                    //Readが0を返した時はサーバーが切断したと判断
                    if (resSize == 0)
                    {
                        CommonLibrary.CommonMethod.DebugConsoleWriteLine(this, "サーバが切断しました。");
                        bRes = false;
                        break;
                    }
                    //受信したデータを蓄積する
                    ms.Write(resBytes, 0, resSize);
                    //まだ読み取れるデータがあるか、データの最後が\nでない時は、受信を続ける
                    //} while (_ns.DataAvailable || resBytes[resSize - 1] != '\n');
                } while (_ns.DataAvailable);

                if (bRes == true)
                {
                    //受信したデータを文字列に変換
                    string resMsg = enc.GetString(ms.GetBuffer(), 0, (int)ms.Length);
                    //resMsg = resMsg.TrimEnd('\n');//仕様によって末尾の\nを削除
                    CommonLibrary.CommonMethod.DebugConsoleWriteLine(this, resMsg);

                    reciveMsg = resMsg;
                }
            }
            return bRes;
        }
        /// <summary>サーバへ書き込み（private）</summary>
        private bool Write(string sendMsg)
        {
            bool bRes = true;

            //文字列をByte型配列に変換        Chuyển đổi chuỗi thành mảng kiểu Byte
            System.Text.Encoding enc = System.Text.Encoding.UTF8;
            byte[] sendBytes = enc.GetBytes(sendMsg);//仕様によって「(sendMsg + '\n')」とする。

            try
            {
                _ns.Write(sendBytes, 0, sendBytes.Length);
                CommonLibrary.CommonMethod.DebugConsoleWriteLine(this, sendMsg);
            }
            catch (Exception ex)
            {
                bRes = false;
            }
            return bRes;
        }
        /// <summary>設定ファイルからデータを読み込む（private）</summary>
        //private bool SettingFileLoad(string sPath, string xsdString)
        //{
        //    bool bRes = true;

        //    string Errormsg;
        //    TcpClientDriverSetting ReadData;
        //    if (Common.XmlData.Load<TcpClientDriverSetting>(sPath, xsdString, out ReadData, out Errormsg) == false)
        //    {
        //        bRes = false;
        //        Common.CommonMethod.DebugConsoleWriteLine(this, Errormsg);
        //    }
        //    else
        //    {
        //        _Setting = ReadData;
        //    }
        //    return bRes;
        //}
        /// <summary>設定ファイルへデータを書き込む（private）</summary>
        //private bool SettingFileSave(string sPath)
        //{
        //    bool bRes = true;

        //    string Errormsg;
        //    if (Common.XmlData.Save<TcpClientDriverSetting>(sPath, _Setting, out Errormsg) == false)
        //    {
        //        bRes = false;
        //        Common.CommonMethod.DebugConsoleWriteLine(this, Errormsg);
        //    }
        //    return bRes;
        //}
        #endregion
    }
}
