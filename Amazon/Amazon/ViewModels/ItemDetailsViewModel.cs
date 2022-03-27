using Amazon.Helpers;
using Amazon.Models;
using Amazon.Services.Data;
using FreshMvvm;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace Amazon.ViewModels
{
    public class ItemDetailsViewModel : FreshBasePageModel
    {
        IDataServices ctx;
        public ItemDetailsViewModel(IDataServices service)
        {
            ctx = service;

            #region Commands
            AddToCart = new Command<ItemModel>(onAddToCart);
            AddToFavourite = new Command<ItemModel>(onAddToFavourite);      
            #endregion
        }

        #region Observable Collections
        ObservableCollection<ItemImagesModel> _lstImages;
        public ObservableCollection<ItemImagesModel> lstImages
        {
            get { return _lstImages; }
            set
            {
                _lstImages = value;
                RaisePropertyChanged("lstImages");
            }
        }

        ItemModel _ItemDetials;
        public ItemModel ItemDetials
        {
            get { return _ItemDetials; }
            set
            {
                _ItemDetials = value;
                RaisePropertyChanged("ItemDetials");
            }
        }

        ItemDetailsModel _itemDetailsModel;
        public ItemDetailsModel itemDetailsModel
        {
            get { return _itemDetailsModel; }
            set
            {
                _itemDetailsModel = value;
                RaisePropertyChanged("itemDetailsModel");
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


        bool _RelatedVisible;
        public bool RelatedVisible
        {
            get
            {
                return _RelatedVisible;
            }
            set
            {
                _RelatedVisible = value;
                RaisePropertyChanged("RelatedVisible");
            }
        }

        bool _CatVisible;
        public bool CatVisible
        {
            get
            {
                return _CatVisible;
            }
            set
            {
                _CatVisible = value;
                RaisePropertyChanged("CatVisible");
            }
        }
        #endregion

        #region Commands
        public Command AddToCart { get; set; }
        public Command AddToFavourite { get; set; }
        #endregion

        public async override void Init(object initData)
        {
            int ItemId = Convert.ToInt32(initData);

            base.Init(initData);

            itemDetailsModel = await ctx.GetItemDetailsData(ItemId);

            itemDetailsModel.itemModel.ItemName = (Settings.Language == "ar") ? itemDetailsModel.itemModel.ItemNameAr : itemDetailsModel.itemModel.ItemName;

            ItemDetials = itemDetailsModel.itemModel;

            lstImages = itemDetailsModel.lstImages;
             
        }


        protected override void ViewIsAppearing(object sender, EventArgs e)
        {     
            base.ViewIsAppearing(sender, e);

            if (Settings.CartList != "null")
            {
                if (!string.IsNullOrEmpty(Settings.RelatedItem1))
                {
                    RelatedVisible = true;
                    CatVisible = true;
                    RelatedItems1 = JsonConvert.DeserializeObject<ObservableCollection<ItemModel>>(Settings.RelatedItem1);
                }
                if (!string.IsNullOrEmpty(Settings.RelatedItem2))
                    RelatedItems2 = JsonConvert.DeserializeObject<ObservableCollection<ItemModel>>(Settings.RelatedItem2);
                else
                {
                    RelatedVisible = false;
                    CatVisible = false;
                }
            }
            else
            {
                RelatedVisible = false;
                CatVisible = false;
            }
        }

        #region Functions

        #region Add To Cart
        async void onAddToCart(ItemModel item)
        {

            cartItems = JsonConvert.DeserializeObject<ObservableCollection<ItemModel>>(Settings.CartList);

            if (cartItems == null)
            {
                cartItems = new ObservableCollection<ItemModel>();

                RelatedItems1 = null;
                RelatedItems2 = null;

                item.Quantity = 1;
                cartItems.Add(item);

                if (Settings.Language == "en")
                    await CoreMethods.DisplayAlert("Success", item.ItemName + " Added to Cart Successfully", "Close");
                else
                    await CoreMethods.DisplayAlert("نجاح", item.ItemName + " تم أضافته الى عربة التسوق", "اغلاق");

                CreateItems(item);
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
            }
            Settings.CartList = JsonConvert.SerializeObject(cartItems);

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

            decimal startPrice = itemModel.SalesPrice - 2000;

            decimal endPrice = itemModel.SalesPrice + 5000;

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

        #region Add To Favourite
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
        #endregion
        #endregion

    }
}
