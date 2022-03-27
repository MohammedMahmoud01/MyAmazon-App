using Akavache;
using Amazon.Helpers;
using Amazon.Services.Data;
using Amazon.ViewModels;
using FreshMvvm;
using System;
using System.Globalization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Amazon
{
    public partial class App : Application
    {
        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NTc3NTc0QDMxMzkyZTM0MmUzMEFyUGVZem0rTHp6TTE4RWl6MnZKTlVzblpETE0ydkNzRXN0YmdjM1JzWTQ9");
            LoadDefaults();

            BlobCache.ApplicationName = "MyAmazonCache";

            InitializeComponent();

            if(!string.IsNullOrEmpty(Settings.Theme))
                ChangeTheme(Convert.ToInt32(Settings.Theme));

        

            var page = FreshMvvm.FreshPageModelResolver.ResolvePageModel<MainViewModel>();

            MainPage = new FreshMvvm.FreshNavigationContainer(page);
        }

        public static void ChangeTheme(int option)
        {
            App.Current.Resources.MergedDictionaries.Clear();
            if (option == 1)
                App.Current.Resources.MergedDictionaries.Add(new Themes.Light());
            else
                App.Current.Resources.MergedDictionaries.Add(new Themes.Dark());

            App.Current.Resources.MergedDictionaries.Add(new Themes.GeneralStyle());
        }

        void LoadDefaults()
        {
            FreshIOC.Container.Register<IGenericRepository, GenericRepository>();
            
            FreshIOC.Container.Register<IDataServices, DataServices>();

            if(!string.IsNullOrEmpty(Settings.Language))
            {
                CultureInfo cal = new CultureInfo(Settings.Language);
                Amazon.Resources.AppResources.Culture = cal;
            }
            else
            {
                Settings.Language = "en";
                CultureInfo cal = new CultureInfo(Settings.Language);
                Amazon.Resources.AppResources.Culture = cal;
            }
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
