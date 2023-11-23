using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LightDriverTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            LightLibrary.LightManager.getInstance().SetEvent("表照明電源", A_EventRequestResult, A_EventTerminateComplete);
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            LightLibrary.LightManager.getInstance().ReleaseEvent("表照明電源", A_EventRequestResult, A_EventTerminateComplete);
        }
        #region ■登録イベント■
        private void A_EventRequestResult(object sender, LightLibrary.LightPowerBase.RequestReceiveEventArgs e)
        {
            Action act = new Action(() =>
            {
                txtReceive.Text = "ジャッジ：" + e.ResultReceiveJudge.ToString() + " 生データ：「" + e.ResultReceiveOrigin + "」"; 
            });

            if (InvokeRequired)
                Invoke(act);
            else
                act.Invoke();
        }

        private void A_EventTerminateComplete(object sender, EventArgs e)
        {
            Action act = new Action(() =>
            {
                txtReceive.Text = "完了";
            });

            if (InvokeRequired)
                Invoke(act);
            else
                act.Invoke();
        }
        #endregion

        private void btnRequest_Click_1(object sender, EventArgs e)
        {

            if (LightLibrary.LightManager.getInstance().RequestLightPower(new List<string> { txtWrite.Text }, "表照明電源") == false)
                MessageBox.Show("リクエスト失敗");

            //■▲■T型も継承クラスでOK
            //var a = new LightLibrary.LightPowerBaseTcp<LightLibrary.LEIMAC_IWDV_600M2_24Setting>();
        }



        
        private void btnLightTerminate_Click(object sender, EventArgs e)
        {
            if( LightLibrary.LightManager.getInstance().AllLightTerminate() ==false)
                MessageBox.Show("完了失敗");
        }

        private void btnLightOn_Click(object sender, EventArgs e)
        {
            if( LightLibrary.LightManager.getInstance().LigthValueChange("表照明電源", "表", (int)nudLightValuePercent.Value) ==false)
                MessageBox.Show("照明値変更失敗");
        }
    }
}
