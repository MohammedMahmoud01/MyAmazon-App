using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using Amazon.Helpers;
using Amazon.Models;
using FreshMvvm;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace Amazon.ViewModels
{
    public class SettingsViewModel : FreshBasePageModel
    {
        public SettingsViewModel()
        {
            ChangeTheme = new Command<string>(onChangeTheme);
            TranslateEn = new Command(onTranslateEn);
            TranslateAr = new Command(onTranslateAr);
            NavToAboutUs = new Command(onNavToAboutUs);
        }

        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Settings.UserInfo) || Settings.UserInfo == "null")
                IsSigned = false;
            else
                IsSigned = true;
            base.ViewIsAppearing(sender, e);
        }

        public override void Init(object initData)
        {
            SettingsModel model = initData as SettingsModel;

            CartCount = model.CartItemsCount.ToString();

            FavouriteCount = model.FavouriteItemsCount.ToString();

            base.Init(initData);
        }

        #region Properties

        bool _IsSigned;
        public bool IsSigned
        {
            get
            {
                return _IsSigned;
            }
            set
            {
                _IsSigned = value;
                RaisePropertyChanged("IsSigned");
            }
        }

        bool _isRadioLight;
        public bool isRadioLight
        {
            get
            {
                return _isRadioLight;
            }
            set
            {
                _isRadioLight = value;
                RaisePropertyChanged("isRadioLight");
            }
        }

        bool _isRadioDark;
        public bool isRadioDark
        {
            get
            {
                return _isRadioDark;
            }
            set
            {
                _isRadioDark = value;
                RaisePropertyChanged("isRadioDark");
            }
        }

        string _CartCount;
        public string CartCount
        {
            get
            {
                return _CartCount;
            }
            set
            {
                _CartCount = value;
                RaisePropertyChanged("CartCount");
            }
        }

        string _FavouriteCount;
        public string FavouriteCount
        {
            get
            {
                return _FavouriteCount;
            }
            set
            {
                _FavouriteCount = value;
                RaisePropertyChanged("FavouriteCount");
            }
        }
        #endregion

        #region Commands
        public Command ChangeTheme { get; set; }
        public Command TranslateEn { get; set; }
        public Command TranslateAr { get; set; }
        public Command NavToAboutUs { get; set; }
        #endregion

        #region Functions
        void onChangeTheme(string par)
        {
            if (par == "1")
            {
                Settings.Theme = Convert.ToString(1);
                App.ChangeTheme(1);
                isRadioLight = true;
                isRadioDark = false;
            }
            else
            {
                Settings.Theme = Convert.ToString(2);
                App.ChangeTheme(2);
                isRadioDark = true;
                isRadioLight = false;
            }
        }

        async void onTranslateEn()
        {
            if (Settings.Language == "en")
                await CoreMethods.DisplayAlert("Note", "You Are Already In English Mode", "Cancel");
            else
            {
                CultureInfo cal = new CultureInfo("en");
                Amazon.Resources.AppResources.Culture = cal;
                Helpers.Settings.Language = "en";

                await CoreMethods.PushPageModel<MainViewModel>("1");
            }
           
        }

        async void onTranslateAr()
        {
            if (Settings.Language == "ar")
                await CoreMethods.DisplayAlert("ملاحظة", "أنت بالفعل على وضع اللغة العربية", "اغلاق");
            else
            {
                CultureInfo cal = new CultureInfo("ar");
                Amazon.Resources.AppResources.Culture = cal;
                Helpers.Settings.Language = "ar";

                await CoreMethods.PushPageModel<MainViewModel>("1");
            }
        }

        void onNavToAboutUs()
        {
            CoreMethods.PushPageModel<AboutUsViewModel>();
        }
        #endregion

    }
}
