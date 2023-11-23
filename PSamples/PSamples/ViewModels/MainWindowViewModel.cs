using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using PSamples.Views;
using System.Diagnostics;
using System.Security.RightsManagement;

namespace PSamples.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private IRegionManager _regionManager;
        private IDialogService _dialogService;
        private string _title = "PSamples";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }//変更があったらView側に通知する                   Thông báo cho View side nếu có thay đổi                       
        }

        private bool _buttonEnabled = false;
        public bool ButtonEnabled
        {
            get { return _buttonEnabled; }
            set { SetProperty(ref _buttonEnabled, value); }//変更があったらView側に通知する
        }


        public MainWindowViewModel(IRegionManager regionManager, IDialogService dialogService)//IRegionManager：リージョンマネージャで画面遷移をする。IDialogService：ポップアップ用
        {
            _regionManager = regionManager;//Prismが自動で取得してくれる。
            _dialogService = dialogService;//Prismが自動で取得してくれる。
            SystemDateUpdateButton = new DelegateCommand(SystemDateUpdateButtonExecute);//①引数にアクションを登録しておく          Đăng ký hành động như một đối số

            ShowViewAButton = new DelegateCommand(ShowViewAButtonExecute).ObservesCanExecute(() => ButtonEnabled);//ObservesCanExecute(() => ButtonEnabled)の記載でボタン押せる押せないはButtonEnabled次第と設定する
            ShowViewPButton = new DelegateCommand(ShowViewPButtonExecute);
            ShowViewBButton = new DelegateCommand(ShowViewBButtonExecute);
            ShowViewCButton = new DelegateCommand(ShowViewCButtonExecute);

        }

        //テキストのバインディング例             Ví dụ ràng buộc văn bản
        private string _systemDateLabel = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        public string SystemDateLabel
        {
            get { return _systemDateLabel; }
            set { SetProperty(ref _systemDateLabel, value); }
        }

        //クリックイベント　バインディング例     Ví dụ ràng buộc sự kiện nhấp chuột
        public DelegateCommand SystemDateUpdateButton { get; }
        //①のアクション         Hành động ①
        private void SystemDateUpdateButtonExecute()
        {
            ButtonEnabled = ButtonEnabled == true ? false : true;
            SystemDateLabel = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        }


        //クリックイベント　バインディング例     Ví dụ ràng buộc sự kiện nhấp chuột
        public DelegateCommand ShowViewAButton { get; }
        private void ShowViewAButtonExecute()
        {
            _regionManager.RequestNavigate("ContentRegion", nameof(ViewA));
        }


        public DelegateCommand ShowViewPButton { get; }
        private void ShowViewPButtonExecute()
        {
            var p = new NavigationParameters();//パラメータを渡す用
            p.Add(nameof(ViewAViewModel.MyLabel), SystemDateLabel);
            _regionManager.RequestNavigate("ContentRegion", nameof(ViewA), p);
        }



        public DelegateCommand ShowViewBButton { get; }
        private void ShowViewBButtonExecute()
        {
            var p = new DialogParameters();
            p.Add(nameof(ViewBViewModel.ViewBTextBox), SystemDateLabel);
            _dialogService.ShowDialog(nameof(ViewB), p, ViewBClose);//第二引数にパラメータを渡せる　ViewBViewModelの②に渡る
        }

        public DelegateCommand ShowViewCButton { get; }

        private void ShowViewCButtonExecute()
        {
            _regionManager.RequestNavigate("ContentRegion", nameof(ViewC));
        }



        private void ViewBClose(IDialogResult dialogResult)
        {
            if (dialogResult.Result == ButtonResult.OK)//OKボタンを押したときだけ
            {
                SystemDateLabel = dialogResult.Parameters.GetValue<string>(nameof(ViewBViewModel.ViewBTextBox));
            }
        }

    }
}
