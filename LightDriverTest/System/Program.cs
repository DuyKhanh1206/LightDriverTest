using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LightDriverTest
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            bool bRes = true;
            // Load data và 
            bRes &= LightLibrary.LightManager.getInstance().Load(AppData.getInstance().LightSettingPath); 

            if (bRes == true)
            {
                Application.Run(new Form1());
            }
            else
            {
                MessageBox.Show("起動に失敗しました。");      //Không thể bắt đầu
            }
        }

        /// <summary>エラーイベントを登録する</summary>
        static void ErrorEventRegist()
        {
            try
            {
                // エラーイベント登録        Đăng ký sự kiện lỗi
                Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
                Thread.GetDomain().UnhandledException += new UnhandledExceptionEventHandler(Program_UnhandledException);
            }
            catch (Exception)
            {
                MessageBox.Show("アプリケーション異常が発生しました");    //Nếu không đăng ký sự kiện thông báo lỗi thành công thì thông báo -> Đã xảy ra lỗi ứng dụng
            }
        }

        // 2 sựu kiện thông báo lỗi
        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.Message, "ThreadException による例外通知です。");
        }

        static void Program_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;
            if (ex != null)
            {
                MessageBox.Show(ex.Message, "UnhandledException による例外通知です。");
            }
        }
    }
}
