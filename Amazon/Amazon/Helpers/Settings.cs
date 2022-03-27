using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amazon.Helpers
{
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        #region Setting Constants

        private const string SettingsKey = "settings_key";

        private const string CartListKey = "CartList_key";

        private const string User = "User_key";

        private const string Lang = "Lang_key";

        private const string CurrentTheme = "CurrentTheme_key";

        private const string RelatedItemkey1 = "RelatedItem_key1";

        private const string RelatedItemkey2 = "RelatedItem_key2";

        private const string LstAllItemsKey = "LstAllItems_Key";

        private const string LstFavouriteItemsKey = "LstFavouriteItems_Key";

        private static readonly string SettingsDefault = string.Empty;

        #endregion
        public static string LstFavouriteItems
        {
            get
            {
                return AppSettings.GetValueOrDefault(LstFavouriteItemsKey, "");
            }
            set
            {
                AppSettings.AddOrUpdateValue(LstFavouriteItemsKey, value);
            }
        }
        public static string LstAllItems
        {
            get
            {
                return AppSettings.GetValueOrDefault(LstAllItemsKey, "");
            }
            set
            {
                AppSettings.AddOrUpdateValue(LstAllItemsKey, value);
            }
        }
        public static string RelatedItem2
        {
            get
            {
                return AppSettings.GetValueOrDefault(RelatedItemkey2, "");
            }
            set
            {
                AppSettings.AddOrUpdateValue(RelatedItemkey2, value);
            }
        }

        public static string RelatedItem1
        {
            get
            {
                return AppSettings.GetValueOrDefault(RelatedItemkey1, "");
            }
            set
            {
                AppSettings.AddOrUpdateValue(RelatedItemkey1, value);
            }
        }

        public static string Theme
        {
            get
            {
                return AppSettings.GetValueOrDefault(CurrentTheme, "");
            }
            set
            {
                AppSettings.AddOrUpdateValue(CurrentTheme, value);
            }
        }

        public static string Language
        {
            get
            {
                return AppSettings.GetValueOrDefault(Lang, "");
            }
            set
            {
                AppSettings.AddOrUpdateValue(Lang, value);
            }
        }
        public static string GeneralSettings
        {
            get
            {
                return AppSettings.GetValueOrDefault(SettingsKey, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(SettingsKey, value);
            }
        }

        public static string CartList
        {
            get
            {
                return AppSettings.GetValueOrDefault(CartListKey, "");
            }
            set
            {
                AppSettings.AddOrUpdateValue(CartListKey, value);
            }
        }

        public static string UserInfo
        {
            get
            {
                return AppSettings.GetValueOrDefault(User, "");
            }
            set
            {
                AppSettings.AddOrUpdateValue(User, value);
            }
        }

    }
}
