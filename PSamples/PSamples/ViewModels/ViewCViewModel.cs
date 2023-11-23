using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using PSamples.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace PSamples.ViewModels
{
    public class ViewCViewModel : BindableBase, IConfirmNavigationRequest
    {
        private IMessageService _messageService;
        private MainWindowViewModel _mainWindowViewModel;//書いておくだけでprismがうまい事やってくれる

        public ViewCViewModel(MainWindowViewModel mainWindowViewModel) : this(new MessageService(), mainWindowViewModel)//本番用
        {

        }

        public ViewCViewModel(IMessageService messageService,MainWindowViewModel mainWindowViewModel)//テスト用
        {
            _messageService = messageService;
            _mainWindowViewModel= mainWindowViewModel;
            MyListBox.Add("AAAAAA");
            MyListBox.Add("SSS");
            MyListBox.Add("DDDD");

            Areas.Add(new ComboBoxViewModel(1, "横浜"));
            Areas.Add(new ComboBoxViewModel(2, "神戸"));
            Areas.Add(new ComboBoxViewModel(3, "高松"));

            SelectedArea = Areas[1];

            AreaSelectionChanged = new DelegateCommand<object[]>(AreaSelectionChangedExecute);
        }

        private ObservableCollection<string> _myListBox = new ObservableCollection<string>();
        public ObservableCollection<string> MyListBox//データバインドするリストは基本的にはObservableCollectionを使用する
        {
            get { return _myListBox; }
            set { SetProperty(ref _myListBox, value); }//変更があったらView側に通知する
        }

        private ObservableCollection<ComboBoxViewModel> _areas = new ObservableCollection<ComboBoxViewModel>();
        public ObservableCollection<ComboBoxViewModel> Areas//データバインドするリストは基本的にはObservableCollectionを使用する
                                                            //Danh sách bị ràng buộc dữ liệu về cơ bản sử dụng ObservableCollection
        {
            get { return _areas; }
            set { SetProperty(ref _areas, value); }//変更があったらView側に通知する      Thông báo cho View nếu có thay đổi
        }

        private ComboBoxViewModel _selectedArea;
        public ComboBoxViewModel SelectedArea
        {
            get { return _selectedArea; }
            set { SetProperty(ref _selectedArea, value); }
        }
        
        public DelegateCommand<object[]> AreaSelectionChanged { get; }//イベント発生時にeのAddedItemsをとるためにobject配列を追加


        private void AreaSelectionChangedExecute(object[] items)//イベント発生時にeのAddedItemsをとるためにobject配列を追加
        {
            SelectedAreaLabel = SelectedArea.Value + " : " + SelectedArea.DisplayValue;

            _mainWindowViewModel.Title = SelectedAreaLabel;

        }


        //SelectedAreaLabel
        private string _selectedAreaLabel;
        public string SelectedAreaLabel
        {
            get { return _selectedAreaLabel; }
            set { SetProperty(ref _selectedAreaLabel, value); }
        }



        public void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback)//IConfirmNavigationRequest用！！（確認用） 引数のActionを通知しないと閉じない仕組み！
        {
            if (_messageService.Question("とじますか？") == System.Windows.MessageBoxResult.OK)
            {
                continuationCallback(true);//Collbackをtureで画面が閉じる。
            }



        }

        public bool IsNavigationTarget(NavigationContext navigationContext)//インスタンスを使いまわすかどうか。毎回新しくする場合は、false  Có nên sử dụng lại các phiên bản hay không. Nếu bạn muốn tạo một cái mới mỗi lần
        {
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)//Navigationが離れるときにコールされる     Được gọi khi Điều hướng rời khỏi
        {
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
        }
    }
}
