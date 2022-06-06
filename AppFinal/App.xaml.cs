using AppFinal.DB.AccessClasses;
using Xamarin.Forms;


namespace AppFinal
{
    
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<UserDbAccess>();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
