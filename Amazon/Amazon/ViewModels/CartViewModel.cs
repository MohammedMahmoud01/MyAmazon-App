using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Amazon.Helpers;
using Amazon.Models;
using Amazon.Services.Data;
using FreshMvvm;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace Amazon.ViewModels
{
    public class CartViewModel : FreshBasePageModel
    {
        IDataServices ctx;
        public CartViewModel(IDataServices service)
        {
            ctx = service;

            #region Commands
            Remove = new Command<ItemModel>(RemoveFromCart);

            AddFavourite = new Command<ItemModel>(onAddFavourite);

            AddQty = new Command<ItemModel>(onAddQty);

            MinusQty = new Command<ItemModel>(onMinusQty);

            lstCartItems = new ObservableCollection<ItemModel>();

            BuyNow = new Command(onBuyNow);

            AddToCart = new Command<ItemModel>(onAddToCart);    
            #endregion
        }

        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            lstCartItems = JsonConvert.DeserializeObject<ObservableCollection<ItemModel>>(Settings.CartList);

            if(lstCartItems == null)
            {
                NoCartVisible = true;
                CartVisible = false;
                lstCartItems = new ObservableCollection<ItemModel>();
            }
            else if(lstCartItems.Count <= 0)
            {
                  NoCartVisible = true;
                  CartVisible = false;               
            }
            else
            {
                NoCartVisible = false;
                CartVisible = true;

                onCalcTotalPrice();

                RelatedItems1 = JsonConvert.DeserializeObject<ObservableCollection<ItemModel>>(Settings.RelatedItem1);

                RelatedItems2 = JsonConvert.DeserializeObject<ObservableCollection<ItemModel>>(Settings.RelatedItem2);
            }

            base.ViewIsAppearing(sender, e);
        }

        

        #region Observable Collection

        ObservableCollection<ItemModel> _lstFavouriteItems;
        public ObservableCollection<ItemModel> lstFavouriteItems
        {
            get
            {
                return _lstFavouriteItems;
            }
            set
            {
                _lstFavouriteItems = value;
                RaisePropertyChanged("lstFavouriteItems");
            }
        }

        ObservableCollection<ItemModel> _lstCartItems;
        public ObservableCollection<ItemModel> lstCartItems
        {
            get
            {
                return _lstCartItems;
            }
            set
            {
                _lstCartItems = value;
                RaisePropertyChanged("lstCartItems");
            }
        }

        bool _NoCartVisible;
        public bool NoCartVisible
        {
            get
            {
                return _NoCartVisible;
            }
            set
            {
                _NoCartVisible = value;
                RaisePropertyChanged("NoCartVisible");
            }
        }

        bool _CartVisible;
        public bool CartVisible
        {
            get
            {
                return _CartVisible;
            }
            set
            {
                _CartVisible = value;
                RaisePropertyChanged("CartVisible");
            }
        }

        decimal _TotalPrice;
        public decimal TotalPrice
        {
            get
            {
                return _TotalPrice;
            }
            set
            {
                _TotalPrice = value;
                RaisePropertyChanged("TotalPrice");
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

        #endregion

        #region Commands
        public Command Remove { get; set; }
        public Command AddFavourite { get; set; }
        public Command AddQty { get; set; }
        public Command MinusQty { get; set; }
        public Command BuyNow { get; set; }
        public Command AddToCart { get; set; }
        #endregion

        #region Functions
        void onCalcTotalPrice()
        {
            decimal price = 0;

            foreach (var item in lstCartItems)
            {
                price += item.SalesPrice * item.Quantity;
            }
            TotalPrice = price;
        }

        void RemoveFromCart(ItemModel model)
        {
            lstCartItems.Remove(model);

            Settings.CartList = JsonConvert.SerializeObject(lstCartItems);

            if(lstCartItems.Count >0)
                onCalcTotalPrice();

            if (lstCartItems.Count <= 0)
            {
                Settings.RelatedItem1 = "";
                Settings.RelatedItem2 = "";
                NoCartVisible = true;
                CartVisible = false;
            }

        }

        async void onAddFavourite(ItemModel model)
        {
            lstFavouriteItems = JsonConvert.DeserializeObject<ObservableCollection<ItemModel>>(Settings.LstFavouriteItems);

            if(lstFavouriteItems == null )
            {
                lstFavouriteItems = new ObservableCollection<ItemModel>();
                lstFavouriteItems.Add(model);
                
            }
            else
            {
                lstFavouriteItems.Add(model);
            }
            Settings.LstFavouriteItems = JsonConvert.SerializeObject(lstFavouriteItems);
            await CoreMethods.DisplayAlert("Success", model.ItemName + " Added to Favourite Successfully", "Close");
        }

        void onAddQty(ItemModel model)
        {
            foreach(var item in lstCartItems)
            {
                if (item == model)
                    if(item.Quantity < 20)
                    {
                        item.Quantity++;
                        Settings.CartList = JsonConvert.SerializeObject(lstCartItems);
                        break;
                    }
              
            }
            CoreMethods.RemoveFromNavigation<CartViewModel>();

            CoreMethods.PushPageModel<CartViewModel>();
        }

        void onMinusQty(ItemModel model)
        {
            foreach (var item in lstCartItems)
            {
                if (item == model)
                    if (item.Quantity > 1)
                    {
                        item.Quantity--;
                        Settings.CartList = JsonConvert.SerializeObject(lstCartItems);
                        break;
                    }
            }
            CoreMethods.RemoveFromNavigation<CartViewModel>();

            CoreMethods.PushPageModel<CartViewModel>();
        }

        async void onBuyNow()
        {
            ItemModel model;
            var LstAllItems = new ObservableCollection<ItemModel>();

            LstAllItems = JsonConvert.DeserializeObject<ObservableCollection<ItemModel>>(Settings.LstAllItems);

            if (LstAllItems == null)
            {
                await CoreMethods.DisplayAlert("Warning", "SomeThing Error Happened", "Cancel");
                return;
            }

            foreach(var item in lstCartItems)
            {
                model = LstAllItems.FirstOrDefault(x=>x.ItemId == item.ItemId);

                if(model.Quantity < item.Quantity)
                {
                    await CoreMethods.DisplayAlert("Warning", "We Don'T Have Enough Quantity For " + model.ItemName + " You Can Order Maximum " + model.Quantity, "Cancel");
                    await CoreMethods.DisplayAlert("Note", "Operation Failed", "Cancel");
                    return;
                }
                else if(model.Quantity == 0)
                {
                    await CoreMethods.DisplayAlert("Note", "Product " + model.ItemName + " Is Already Sold", "Cancel");
                    return;
                }
            }           

            await CoreMethods.DisplayAlert("Success", "Operation Success", "Cancel");

            lstCartItems = null;

            Settings.CartList = "null";

            await CoreMethods.PopToRoot(true);

        }

        async void onAddToCart(ItemModel item)
        {

            ItemModel model = lstCartItems.FirstOrDefault(x => x.ItemId == item.ItemId);

            if (model == null)
            {
                RelatedItems1 = null;
                RelatedItems2 = null;
                item.Quantity = 1;
                lstCartItems.Add(item);
                Settings.CartList = JsonConvert.SerializeObject(lstCartItems);
                await CoreMethods.DisplayAlert("Success", item.ItemName + " Added to Cart Successfully", "Close");
                onCalcTotalPrice();
                CreateItems(item);
            }
            else           
                onAddQty(model);
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
    }
}
