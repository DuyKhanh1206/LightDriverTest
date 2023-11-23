using Prism.Ioc;
using PSamples.ViewModels;
using PSamples.Views;
using System.Windows;

namespace PSamples
{
    /// <summary>
    /// Interaction logic for App.xaml          Logic tương tác cho App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ViewA>();//Đăng ký các loại trang (pages) trong ứng dụng. Điều này giúp ứng dụng biết được cách điều hướng (navigate) giữa các trang.
            containerRegistry.RegisterForNavigation<ViewC>();
            containerRegistry.RegisterForNavigation<ViewB>();
            containerRegistry.RegisterDialog<ViewB, ViewBViewModel>();      //đăng ký một loại trang (page) như một dialog trong ứng dụng. Dialog thường là các cửa sổ hoặc hộp thoại 

            containerRegistry.RegisterSingleton<MainWindowViewModel>();//シングルトン画面の設定          chỉ có một phiên bản duy nhất của lớp này được tạo ra và sử dụng trong toàn bộ ứng dụng.
        }
    }
}

