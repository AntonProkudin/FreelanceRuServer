using ServiceClient.Views.Pages;

namespace ServiceClient
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new LoginPage();
        }
    }
}
