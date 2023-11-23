using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using PSamples.Services;
using PSamples.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace PSamples.ViewModels
{
    public class ViewAViewModel : BindableBase, INavigationAware//データ受け取る用
    {

        private IDialogService _dialogService;//Prismが勝手にやってくれる
        private IMessageService _messageService;//Prismが勝手にやってくれないので自分で作る③のコンストラクタ

        //③
        public ViewAViewModel(IDialogService dialogService) : this(dialogService, new MessageService())//実実行用
        {
        }

        public ViewAViewModel(IDialogService dialogService, IMessageService messageService)//テスト実行用
        {
            _dialogService = dialogService;
            _messageService = messageService;
            OKButton = new DelegateCommand(OKButtonExecute);
            OKButton2 = new DelegateCommand(OKButton2Execute);
        }

        //テキストのバインディング例     Ví dụ ràng buộc văn bản
        private string _myLabel = string.Empty;
        public string MyLabel
        {
            get { return _myLabel; }
            set { SetProperty(ref _myLabel, value); }
        }

        public DelegateCommand OKButton { get; }

        public DelegateCommand OKButton2 { get; }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            MyLabel = navigationContext.Parameters.GetValue<string>(nameof(MyLabel));
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)//インスタンスを使いまわすかどうか。毎回新しくする場合はfalse
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)//Navigationが離れるときにコールされる
        {
        }

        private void OKButtonExecute()//ViewModelでMessageBox.Showを読んではいけない。自分で画面を作ってコールする。
        {
            //MessageBox.Show("Saveします");
            var p = new DialogParameters();
            p.Add(nameof(ViewBViewModel.ViewBTextBox), "Saveします");
            _dialogService.ShowDialog(nameof(ViewB), p, null);
        }

        private void OKButton2Execute()//MessageBox.Showを使うやり方。
        {
            //MessageBox.Show("Saveします");
            if (_messageService.Question("Saveしますか？") == MessageBoxResult.OK)
            {
                _messageService.ShowDialog("Saveしました");
            }
        }
    }
}