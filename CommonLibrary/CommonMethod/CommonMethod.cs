using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CommonLibrary
{
    static public class CommonMethod
    {
        // kiểm tra IP có đúng định dạng 0.0.0.0?
        static public bool IsIPAddressCorrect(string sTest)
        {
            // IPアドレス指定の場合
            Regex reg = new Regex(@"^(\d|[01]?\d\d|2[0-4]\d|25[0-5])\.(\d|[01]?\d\d|2[0-4]\d|25[0-5])\.(\d|[01]?\d\d|2[0-4]\d|25[0-5])\.(\d|[01]?\d\d|2[0-4]\d|25[0-5])$");
            return reg.IsMatch(sTest);
        }

        // kiểm tra port
        static public bool IsPortCorrect(int iTest)
        {
            if (iTest < 0 || iTest > 65535)
                return false;
            return true;
        }

        /// <summary>Console.WriteLineでどのクラスからを出力するか使い分ける</summary>
        static public void DebugConsoleWriteLine(object Obj, string sMsg)
        {
#if DEBUG
            //出力するClass名を列挙
            string[] OutPutClass = new string[]{ "LightPowerDriver", "TcpClientDriver", "IWDV_600M2_24" };

            if(Array.IndexOf(OutPutClass, Obj.GetType().Name.ToString()) != -1)
            {
                Console.WriteLine("■Type:{0}■{1}", Obj.GetType().Name, sMsg);
            }
            else
            {
                Console.WriteLine("！！！Type:{0}！！！{1}", Obj.GetType().Name, sMsg);
            }
#endif
        }

    }
}
