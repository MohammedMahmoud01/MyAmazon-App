using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Amazon.Helpers;
using Amazon.Models;
using FreshMvvm;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace Amazon.ViewModels
{
    public class FavouriteViewModel : FreshBasePageModel
    {
        public FavouriteViewModel()
        {
            #region Commands
            AddToCart = new Command<ItemModel>(onAddToCart);
            Remove = new Command<ItemModel>(RemoveFromFavourite); 
            #endregion
        }

        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            lstFavouriteItems = JsonConvert.DeserializeObject<ObservableCollection<ItemModel>>(Settings.LstFavouriteItems);

            if (lstFavouriteItems == null)
            {
                NoFavouriteVisible = true;
                FavouriteVisible = false;
                lstFavouriteItems = new ObservableCollection<ItemModel>();
            }
            else if (lstFavouriteItems.Count <= 0)
            {
                NoFavouriteVisible = true;
                FavouriteVisible = false;
            }
            else
            {
                NoFavouriteVisible = false;
                FavouriteVisible = true;

            }

            base.ViewIsAppearing(sender, e);
        }

        #region Observable Collections
        bool _NoFavouriteVisible;
        public bool NoFavouriteVisible
        {
            get
            {
                return _NoFavouriteVisible;
            }
            set
            {
                _NoFavouriteVisible = value;
                RaisePropertyChanged("NoFavouriteVisible");
            }
        }

        bool _FavouriteVisible;
        public bool FavouriteVisible
        {
            get
            {
                return _FavouriteVisible;
            }
            set
            {
                _FavouriteVisible = value;
                RaisePropertyChanged("FavouriteVisible");
            }
        }

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
        #endregion

        #region Commands
        public Command Remove { get; set; }
        public Command AddToCart { get; set; }
        #endregion

        #region Functions
        async void onAddToCart(ItemModel item)
        {

            cartItems = JsonConvert.DeserializeObject<ObservableCollection<ItemModel>>(Settings.CartList);

            if (cartItems == null)
            {
                cartItems = new ObservableCollection<ItemModel>();
                item.Quantity = 1;

                cartItems.Add(item);

                lstFavouriteItems.Remove(item);

                await CoreMethods.DisplayAlert("Success", item.ItemName + " Added to Cart Successfully", "Close");
            }
            else if (cartItems.Where(x => x.ItemId == item.ItemId).Count() > 0)
            {
                lstFavouriteItems.Remove(item);

                await CoreMethods.DisplayAlert("Warning", "This Item is Already exist in Cart", "Close");
            }
            else
            {
                item.Quantity = 1;

                cartItems.Add(item);

                lstFavouriteItems.Remove(item);

                await CoreMethods.DisplayAlert("Success", item.ItemName + " Added to Cart Successfully", "Close");
            }

            Settings.CartList = JsonConvert.SerializeObject(cartItems);

            Settings.LstFavouriteItems = JsonConvert.SerializeObject(lstFavouriteItems);

            if (lstFavouriteItems.Count <= 0)
            {
                NoFavouriteVisible = true;
                FavouriteVisible = false;
            }
        }

        async void RemoveFromFavourite(ItemModel model)
        {
            lstFavouriteItems.Remove(model);

            Settings.LstFavouriteItems = JsonConvert.SerializeObject(lstFavouriteItems);

            await CoreMethods.DisplayAlert("Success", model.ItemName + " Removed From Favourite Successfully", "Close");

            if (lstFavouriteItems.Count <= 0)
            {
                NoFavouriteVisible = true;
                FavouriteVisible = false;
            }

        } 
        #endregion
    }
}
