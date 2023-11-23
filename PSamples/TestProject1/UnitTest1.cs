using Moq;
using Prism.Services.Dialogs;
using PSamples.Services;
using PSamples.ViewModels;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Controls;

namespace TestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var mock = new Mock<IDialogService>();
            var vm = new ViewAViewModel(mock.Object);


            mock.Setup(x => x.ShowDialog(
                It.IsAny<string>(),
                It.IsAny<IDialogParameters>(),
                It.IsAny<Action<IDialogResult>>()
                )).Callback<
                    string,
                    IDialogParameters,
                    Action<IDialogResult>
                    >
                    ((viewName, p, result) =>
                    {
                    Assert.AreEqual("ViewB", viewName);
                    });

            vm.OKButton.Execute();//OKボタンが押される。
        }

        [TestMethod]
        public void ボタン２のテスト()
        {
            var dialogService = new Mock<IDialogService>();
            var messageService = new Mock<IMessageService>();
            var vm =new ViewAViewModel(dialogService.Object, messageService.Object);

            messageService.Setup(x => x.Question("Saveしますか？")).Returns(System.Windows.MessageBoxResult.OK);


            messageService.Setup(x => x.ShowDialog(
                It.IsAny<string>()
                )).Callback<string>(message =>
                    {
                        Assert.AreEqual("Saveしました", message);
                    });

            vm.OKButton2.Execute();//OKボタンが押される。
            messageService.VerifyAll();
        }


    }
}