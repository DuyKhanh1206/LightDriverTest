using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary
{
    /// <summary>Xmlデータの読取・保存を行うクラス</summary>
    static public class XmlData
    {
        /// <summary>セーブ</summary>
        // Lưu SaveData vaofg ddwuowngf dẫn gồm các thông tin kết nối IP/Port/TimeOut
        public static bool Save<ClassType>(string sPath, ClassType SaveData, out string ErrorMessage)
        {
            bool bRes = true;
            bool bBackupFail = false;
            string sBackupFilePath = string.Empty;
            ErrorMessage = string.Empty;

            //１）既存ファイルがあった場合、バックアップコピーを実行       Nếu có tệp hiện có, hãy thực hiện sao lưu
            if (bRes == true)
            {
                if (System.IO.File.Exists(sPath) == true)
                {
                    try
                    {
                        sBackupFilePath = sPath + DateTime.Now.ToString("_yyyyMMdd_HHmmss");
                        System.IO.File.Copy(sPath, sBackupFilePath, true);
                    }
                    catch
                    {
                        ErrorMessage = "ファイルのバックアップに失敗しました。";
                        bRes = false;
                    }
                }
            }

            //２）xmlを書き込み、失敗したらバックアップファイルに戻す。        Viết xml và nếu thất bại, hãy quay lại tệp sao lưu.
            if (bRes == true)
            {
                try
                {
                    StreamWriter sw = new StreamWriter(sPath, false, new UTF8Encoding(false));
                    System.Xml.Serialization.XmlSerializer xs =
                        new System.Xml.Serialization.XmlSerializer(typeof(ClassType));
                    //シリアル化して書き込む       tuần tự hóa và viết 
                    xs.Serialize(sw, SaveData);
                    sw.Flush();//Close前にFlush       giải phóng bộ nhớ
                    sw.Close();
                    bBackupFail = false;
                }
                catch (Exception ex)
                {
                    bBackupFail = true;
                    bRes = false;
                }
                finally
                {
                    //catchに入っていたらファイルコピーしてバックアップファイルを元ファイルに上書き。        Nếu nó bị bắt, hãy sao chép tệp và ghi đè tệp gốc bằng tệp sao lưu.
                    if (true == bBackupFail)
                    {
                        ErrorMessage = "ファイルの書き込みに失敗しました。";
                        try
                        {
                            File.Copy(sBackupFilePath, sPath, true);
                        }
                        catch
                        {
                        }
                    }
                }
            }

            //３）全て上手くいったらバックアップファイルを削除      Nếu mọi việc suôn sẻ, hãy xóa tập tin sao lưu
            if (bRes == true)
            {
                try
                {
                    if (System.IO.File.Exists(sBackupFilePath))
                        File.Delete(sBackupFilePath);
                }
                catch
                {
                    //ここな何もしない
                }
            }
            return bRes;
        }

        /// <summary>ロード</summary>
        // ClassType : eg LightManagerSetting / LightPowerBaseSetting / LightPowerBaseTcpSetting
        public static bool Load<ClassType>(string sPath, string sXsdString, out ClassType ReadData, out string ErrorMessage)
        {
            bool bRes = true;
            ReadData = default(ClassType);
            ErrorMessage = string.Empty;

            //１）ファイル有無チェック          kiểm tra file có tồn tại không
            if (true == bRes)
            {
                if (false == System.IO.File.Exists(sPath))
                {
                    ErrorMessage = "ユーザ設定ファイルが見つかりません。";
                    bRes = false;
                }
            }

            //２）整合性チェック：参考：スキーマ作成 Visual StudioでXMLファイルを開いて、メニューバーで、 [XML] > [スキーマの作成]で作成する。
            // kiểm tra tính nhất quán 
            if (true == bRes)
            {
                if (CommonLibrary.XSDUtils.XmlValidate(sPath, sXsdString) == false)
                {
                    string sFileName = Path.GetFileName(sPath);// lấy ra tên của file trong đường dẫn.
                    ErrorMessage = sFileName + "の整合性チェックに失敗しました。\n" + sFileName + "の内容を確認して下さい。";
                    bRes = false;
                }
            }

            //３）ロード
            if (true == bRes)
            {
                try
                {
                    StreamReader sr = new StreamReader(sPath, new UTF8Encoding(false));
                    System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(typeof(ClassType));
                    //読み込んで逆シリアル化する
                    object obj = xs.Deserialize(sr);
                    sr.Close();

                    ReadData = (ClassType)obj;
                }
                catch (Exception ex)
                {
                    string sFileName = Path.GetFileName(sPath);
                    ErrorMessage = sFileName + "の読み込みに失敗しました。";
                    bRes = false;
                }
            }
            return bRes;
        }
    }
}
