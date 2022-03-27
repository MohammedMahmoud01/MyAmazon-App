using System;
using System.Collections.Generic;
using System.Text;
using FreshMvvm;
using Xamarin.Forms;
using Amazon.Helpers;
using System.Collections.ObjectModel;
using Amazon.Models;
using Amazon.Services.Data;
using System.Linq;
using Newtonsoft.Json;

namespace Amazon.ViewModels
{
    public class MainViewModel : FreshBasePageModel
    {
        IDataServices ctx;
        public MainViewModel(IDataServices services)
        {
            ctx = services;

            #region Commands
            SelectedItem = new Command<ItemModel>(onSelectedItem);

            ShowItemsList = new Command<string>(onShowItemsList);

            #region Navigation
            NavToCart = new Command(onNavToCart);

            NavToFavourite = new Command(onNavToFavourite);

            NavToLogin = new Command(onNavToLogin);

            NavToRegister = new Command(onNavToRegister);

            NavToSettings = new Command(onNavToSettings);

            SearchBar = new Command(onNavSearchBar);
            #endregion

            #region Add To Fav. & Cart
            AddToCart = new Command<ItemModel>(onAddToCart);
            AddToCart2 = new Command<ItemDiscountModel>(onAddToCart2);

            AddToFavourite = new Command<ItemModel>(onAddToFavourite);
            AddToFavourite2 = new Command<ItemDiscountModel>(onAddToFavourite2); 
            #endregion

            SignOut = new Command(onSignOut);
            #endregion
        }


        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Settings.UserInfo) || Settings.UserInfo == "null")
            {
                IsLogged = true;

                IsSignOut = false;

                onCalcCartNum();
            }
            else
            {
                IsLogged = false;

                IsSignOut = true;

                userLogin = JsonConvert.DeserializeObject<UserModel>(Settings.UserInfo);

                onCalcCartNum();
            }


            base.ViewIsAppearing(sender, e);
        }

        void onCalcCartNum()
        {
            cartItems = JsonConvert.DeserializeObject<ObservableCollection<ItemModel>>(Settings.CartList);
            if(cartItems == null)
                CartNum = 0;
            else
                CartNum = cartItems.Count;
        }

        public async override void Init(object initData)
        {
            Banners = GetBanners();

            lstSlider = await ctx.GetSlidersData();


            if (initData == null)
            {
                homeModel = await ctx.GetHomePageData(null);

                homeModel2 = await ctx.GetHomePageData2(null);
            }
            else
            {
                homeModel = await ctx.GetHomePageData(1);

                homeModel2 = await ctx.GetHomePageData2(1);
            }
         
            base.Init(initData);


            lstCategories = await ctx.GetCategoryData();
        }
        

        #region Observable Collections
        ObservableCollection<ItemDiscountModel> _lstItemsDiscount;
        public ObservableCollection<ItemDiscountModel> lstItemsDiscount
        {
            get { return _lstItemsDiscount; }
            set
            {
                _lstItemsDiscount = value;
                RaisePropertyChanged("lstItemsDiscount");
            }
        }

        //New , Popular and Discounts Products
        HomePageModel _homeModel;
        public HomePageModel homeModel
        {
            get { return _homeModel; }
            set
            {
                _homeModel = value;
                RaisePropertyChanged("homeModel");
            }
        }

        //Apple ,Xiaomi and Samsung Products
        CatItemModel _homeModel2;
        public CatItemModel homeModel2
        {
            get { return _homeModel2; }
            set
            {
                _homeModel2 = value;
                RaisePropertyChanged("homeModel2");
            }
        }

        ObservableCollection<SliderModel> _lstSlider;
        public ObservableCollection<SliderModel> lstSlider
        {
            get
            {
                return _lstSlider;
            }
            set
            {
                _lstSlider = value;
                RaisePropertyChanged("lstSlider");
            }
        }

        ObservableCollection<CategoryModel> _lstCategories;
        public ObservableCollection<CategoryModel> lstCategories
        {
            get
            {
                return _lstCategories;
            }
            set
            {
                _lstCategories = value;
                RaisePropertyChanged("lstCategories");
            }
        }

        ObservableCollection<ItemModel> _lstAllItems;
        public ObservableCollection<ItemModel> lstAllItems
        {
            get { return _lstAllItems; }
            set
            {
                _lstAllItems = value;
                RaisePropertyChanged("lstAllItems");
            }
        }


        ObservableCollection<Banner> banners;
        public ObservableCollection<Banner> Banners
        {
            get { return banners; }
            set
            {
                banners = value;
                RaisePropertyChanged("Banners");
            }
        }

        ObservableCollection<ItemModel> _cartItems;
        public ObservableCollection<ItemModel> cartItems
        {
            get { return _cartItems; }
            set
            {
                _cartItems = value;
                RaisePropertyChanged("cartItems");
            }
        }

        ObservableCollection<ItemModel> _FavouriteItems;
        public ObservableCollection<ItemModel> FavouriteItems
        {
            get { return _FavouriteItems; }
            set
            {
                _FavouriteItems = value;
                RaisePropertyChanged("FavouriteItems");
            }
        }

        ObservableCollection<ItemModel> _RelatedItems1;
        public ObservableCollection<ItemModel> RelatedItems1
        {
            get
            {
                return _RelatedItems1;
            }
            set
            {
                _RelatedItems1 = value;
                RaisePropertyChanged("RelatedItems1");
            }
        }

        ObservableCollection<ItemModel> _RelatedItems2;
        public ObservableCollection<ItemModel> RelatedItems2
        {
            get
            {
                return _RelatedItems2;
            }
            set
            {
                _RelatedItems2 = value;
                RaisePropertyChanged("RelatedItems2");
            }
        }

        bool _IsLogged;
        public bool IsLogged
        {
            get
            {
                return _IsLogged;
            }
            set
            {
                _IsLogged = value;
                RaisePropertyChanged("IsLogged");
            }
        }

        bool _IsSignOut;
        public bool IsSignOut
        {
            get
            {
                return _IsSignOut;
            }
            set
            {
                _IsSignOut = value;
                RaisePropertyChanged("IsSignOut");
            }
        }

        int _CartNum;
        public int CartNum
        {
            get
            {
                return _CartNum;
            }
            set
            {
                _CartNum = value;
                RaisePropertyChanged("CartNum");
            }
        }

        UserModel _userLogin;
        public UserModel userLogin
        {
            get { return _userLogin; }
            set
            {
                _userLogin = value;
                RaisePropertyChanged("userLogin");
            }
        }
        #endregion

        #region Commands

        #region Item Detials Command
        public Command SelectedItem { get; set; } 
        #endregion

        #region Items List Page Command
        public Command ShowItemsList { get; set; }
        #endregion

        #region Navigation
        public Command NavToCart { get; set; }
        public Command NavToFavourite { get; set; }
        public Command NavToLogin { get; set; }
        public Command NavToRegister { get; set; }
        public Command NavToSettings { get; set; }
        public Command SearchBar { get; set; }
        #endregion

        #region Add To Cart & Favourite
        public Command AddToCart { get; set; }
        public Command AddToCart2 { get; set; }

        public Command AddToFavourite { get; set; }
        public Command AddToFavourite2 { get; set; } 
        #endregion

        public Command SignOut { get; set; }

        #endregion

        #region Navigation Functions
        void onNavToCart()
        {
             CoreMethods.PushPageModel<CartViewModel>();
        }

        void onNavToFavourite()
        {
             CoreMethods.PushPageModel<FavouriteViewModel>();
        }

        void onNavSearchBar()
        {
            CoreMethods.PushPageModel<SearchViewModel>();
        } 

        void onNavToLogin()
        {
            CoreMethods.PushPageModel<LoginViewModel>();
        }

        void onNavToRegister()
        {
            CoreMethods.PushPageModel<RegisterViewModel>();
        }

        void onNavToSettings()
        {
            SettingsModel model = new SettingsModel();

            FavouriteItems = JsonConvert.DeserializeObject<ObservableCollection<ItemModel>>(Settings.LstFavouriteItems);
            if (FavouriteItems == null)
                model.FavouriteItemsCount = 0;
            else
                model.FavouriteItemsCount = FavouriteItems.Count;

            cartItems = JsonConvert.DeserializeObject<ObservableCollection<ItemModel>>(Settings.CartList);
            if (cartItems == null)
                model.CartItemsCount = 0;
            else
                model.CartItemsCount = cartItems.Count;

            CoreMethods.PushPageModel<SettingsViewModel>(model);
        }
        #endregion

        #region Show and Hidden
        void onSignOut()
        {
            Settings.UserInfo = String.Empty;
            IsLogged = true;
            IsSignOut = false;
        }
        #endregion

        #region ItemsListPage Function

        void onShowItemsList(string filter)
        {
            CoreMethods.PushPageModel<ItemsListViewModel>(filter);
        }
        #endregion

        #region ItemDetailsPage Functions
        void onSelectedItem(ItemModel itemModel)
        {
            CoreMethods.PushPageModel<ItemDetailsViewModel>(itemModel.ItemId);
        }

        #endregion

        #region Add To Cart
        async void onAddToCart(ItemModel item)
        {
            if (cartItems == null)
            {
                cartItems = new ObservableCollection<ItemModel>();

                RelatedItems1 = null;
                RelatedItems2 = null;

                item.Quantity = 1;
                cartItems.Add(item);

                if(Settings.Language == "en")
                    await CoreMethods.DisplayAlert("Success", item.ItemName + " Added to Cart Successfully", "Close");
                else
                    await CoreMethods.DisplayAlert("نجاح", item.ItemName + " تم أضافته الى عربة التسوق", "اغلاق");

                CreateItems(item);

                CartNum++;
            }
            else if (cartItems.Where(x => x.ItemId == item.ItemId).Count() > 0)
            {
                if (Settings.Language == "en")
                    await CoreMethods.DisplayAlert("Warning", "This Item is Already exist in Cart", "Close");
                else
                    await CoreMethods.DisplayAlert("تحذير", "هذا المنتج موجود فعلا فى عربتك", "اغلاق");
            }

            else
            {
                item.Quantity = 1;
                cartItems.Add(item);
                if (Settings.Language == "en")
                    await CoreMethods.DisplayAlert("Success", item.ItemName + " Added to Cart Successfully", "Close");
                else
                    await CoreMethods.DisplayAlert("نجاح", item.ItemName + " تم أضافته الى عربة التسوق", "اغلاق");

                CreateItems(item);
                CartNum++;
            }
            Settings.CartList = JsonConvert.SerializeObject(cartItems);

        }

        async void onAddToCart2(ItemDiscountModel model)
        {
            if(lstAllItems == null)
            {
                lstAllItems = JsonConvert.DeserializeObject<ObservableCollection<ItemModel>>(Settings.LstAllItems);

                if (lstAllItems == null)
                {
                    lstAllItems = new ObservableCollection<ItemModel>();
                    lstAllItems = await ctx.GetAllItemsData();
                }
            }
            ItemModel item = lstAllItems.FirstOrDefault(x => x.ItemId == model.ItemId);
            onAddToCart(item);
        }

        async void CreateItems(ItemModel itemModel)
        {
            if (lstAllItems == null)
            {
                lstAllItems = new ObservableCollection<ItemModel>();
                lstAllItems = await ctx.GetAllItemsData();
            }
            else
                lstAllItems = await ctx.GetAllItemsData();

            decimal startPrice = itemModel.SalesPrice - 2500;

            decimal endPrice = itemModel.SalesPrice + 6000;


            RelatedItems1 = new ObservableCollection<ItemModel>();
    
            var items = lstAllItems.Where(x => x.SalesPrice > startPrice && x.SalesPrice < endPrice).ToList();

            foreach (var item in items)
                RelatedItems1.Add(item);

            Settings.RelatedItem1 = JsonConvert.SerializeObject(RelatedItems1);
           

            if (itemModel.SubCategoryId == 1)
            {
                RelatedItems2 = new ObservableCollection<ItemModel>();

                var Samsung = lstAllItems.Where(x => x.SubCategoryId == itemModel.SubCategoryId).ToList();

                foreach (var item in Samsung)
                    RelatedItems2.Add(item);

                Settings.RelatedItem2 = JsonConvert.SerializeObject(RelatedItems2);
            }

            if (itemModel.SubCategoryId == 2)
            {
                RelatedItems2 = new ObservableCollection<ItemModel>();

                var Apple = lstAllItems.Where(x => x.SubCategoryId == itemModel.SubCategoryId).ToList();

                foreach (var item in Apple)
                    RelatedItems2.Add(item);

                Settings.RelatedItem2 = JsonConvert.SerializeObject(RelatedItems2);
            }

            if (itemModel.SubCategoryId == 3)
            {
                RelatedItems2 = new ObservableCollection<ItemModel>();

                var Xiaomi = lstAllItems.Where(x => x.SubCategoryId == itemModel.SubCategoryId).ToList();

                foreach (var item in Xiaomi)
                    RelatedItems2.Add(item);

                Settings.RelatedItem2 = JsonConvert.SerializeObject(RelatedItems2);
            }
        }
        #endregion

        #region Add To Favourites
        async void onAddToFavourite(ItemModel item)
        {
            FavouriteItems = JsonConvert.DeserializeObject<ObservableCollection<ItemModel>>(Settings.LstFavouriteItems);

            if (FavouriteItems == null)
            {
                FavouriteItems = new ObservableCollection<ItemModel>();
                item.Quantity = 1;
                FavouriteItems.Add(item);
                if (Settings.Language == "en")
                    await CoreMethods.DisplayAlert("Success", item.ItemName + " Added to Favourite Successfully", "Close");
                else
                    await CoreMethods.DisplayAlert("نجاح", item.ItemName + " تم أضافته الى المفضلة", "اغلاق");
            }
            else if (FavouriteItems.Where(x => x.ItemId == item.ItemId).Count() > 0)
            {
                if (Settings.Language == "en")
                    await CoreMethods.DisplayAlert("Warning", "This Item is Already exist in Favourite", "Close");
                else
                    await CoreMethods.DisplayAlert("تحذير", "هذا المنتج موجود فعلا فى المفضلة", "اغلاق");
            }
            else
            {
                item.Quantity = 1;

                FavouriteItems.Add(item);

                if (Settings.Language == "en")
                    await CoreMethods.DisplayAlert("Success", item.ItemName + " Added to Favourite Successfully", "Close");
                else
                    await CoreMethods.DisplayAlert("نجاح", item.ItemName + " تم أضافته الى المفضلة", "اغلاق");
            }
            Settings.LstFavouriteItems = JsonConvert.SerializeObject(FavouriteItems);
        }
        async void onAddToFavourite2(ItemDiscountModel itemDiscountModel)
        {
            if (lstAllItems == null)
            {
                lstAllItems = JsonConvert.DeserializeObject<ObservableCollection<ItemModel>>(Settings.LstAllItems);

                if (lstAllItems == null)
                {
                    lstAllItems = new ObservableCollection<ItemModel>();
                    lstAllItems = await ctx.GetAllItemsData();
                }
            }
            ItemModel item = lstAllItems.FirstOrDefault(x => x.ItemId == itemDiscountModel.ItemId);
            onAddToFavourite(item);
        } 
        #endregion

        #region Dummy Data
        private ObservableCollection<Banner> GetBanners()
        {
            return new ObservableCollection<Banner>
            {
            new Banner { Heading = "SUMMER COLLECTION", Message = "Comming Soon", Caption = "BEST DISCOUNT THIS SEASON", Image = "classic.png" },
            new Banner { Heading = "Men'S CLOTHINGS", Message = "Comming Soon", Caption = "GET 50% OFF ON EVERY ITEM", Image = "leatherBag.png" },
            new Banner { Heading = "ELEGANT COLLECTION", Message = "Comming Soon", Caption = "UNIQUE COMBINATIONS OF ITEMS", Image = "elegantCol.png" },
            };
        } 
        #endregion
    }
}
