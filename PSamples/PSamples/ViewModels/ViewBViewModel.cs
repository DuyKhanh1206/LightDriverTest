using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using PSamples.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace PSamples.ViewModels
{
    public class ViewBViewModel : BindableBase, IDialogAware//ポップアップにする場合には「IDialogAware」をまとっておく    
                                                            //Nếu bạn muốn biến nó thành cửa sổ bật lên, hãy sử dụng "IDialogAware"
    {
        public ViewBViewModel()
        {
            OKButton = new DelegateCommand(OKButtonExecute);
        }

        public string Title => throw new NotImplementedException();

        private string _viewBTextBox = "XXX";
        public string ViewBTextBox
        {
            get { return _viewBTextBox; }
            set { SetProperty(ref _viewBTextBox, value); }//変更があったらView側に通知する
        }

        public event Action<IDialogResult> RequestClose;

        //OKButton
        //クリックイベント　バインディング例
        public DelegateCommand OKButton { get; }
        private void OKButtonExecute()
        {
            var p = new DialogParameters();
            p.Add(nameof(ViewBTextBox), ViewBTextBox);
            RequestClose?.Invoke(new DialogResult(ButtonResult.OK, p));
        }

        public bool CanCloseDialog()//画面が閉じれるかどうか。
        {
            return true;
        }

        public void OnDialogClosed()//画面が閉じるときの処理
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)//画面が開くときの処理②
        {
            ViewBTextBox = parameters.GetValue<string>(nameof(ViewBTextBox));
        }
    }
}
