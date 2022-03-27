using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Amazon.Models;
using Amazon.Services.Data;
using FreshMvvm;
using Xamarin.Forms;

namespace Amazon.ViewModels
{
    public class SearchViewModel : FreshBasePageModel
    {
        IDataServices ctx;
        public SearchViewModel(IDataServices service)
        {
            ctx = service;

            #region Commands
            SearchWithText = new Command(onSearchWithText);
            SelectedItem = new Command<ItemModel>(onSelectedItem);
            ReFresh = new Command(OnRefresh);
            NavBack = new Command(onNavBack); 
            #endregion
        }

        #region Observable Collection
        string _TxtSearch;
        public string TxtSearch
        {
            get { return _TxtSearch; }
            set
            {
                _TxtSearch = value;
                RaisePropertyChanged("TxtSearch");
            }
        }

        bool _IsRefresh;
        public bool IsRefresh
        {
            get
            {
                return _IsRefresh;
            }
            set
            {
                _IsRefresh = value;
                RaisePropertyChanged("IsRefresh");
            }
        }

        List<ItemImagesModel> _lstImages;
        public List<ItemImagesModel> lstImages
        {
            get { return _lstImages; }
            set
            {
                _lstImages = value;
                RaisePropertyChanged("lstImages");
            }
        }

        ObservableCollection<ItemModel> _lstItems;
        public ObservableCollection<ItemModel> lstItems
        {
            get { return _lstItems; }
            set
            {
                _lstItems = value;
                RaisePropertyChanged("lstItems");
            }
        }

        ObservableCollection<ItemModel> _lstFilterItems;
        public ObservableCollection<ItemModel> lstFilterItems
        {
            get { return _lstFilterItems; }
            set
            {
                _lstFilterItems = value;
                RaisePropertyChanged("lstFilterItems");
            }
        }
        #endregion

        #region Commands
        public Command SearchWithText { get; set; }
        public Command ReFresh { get; set; } 
        public Command NavBack { get; set; }
        public Command SelectedItem { get; set; }
        #endregion

        public async override void Init(object initData)
        {
            lstItems = await ctx.GetAllItemsData();

            lstFilterItems = new ObservableCollection<ItemModel>(lstItems);

            base.Init(initData);
        }

        #region Functions
        void onSearchWithText()
        {
            if (string.IsNullOrEmpty(TxtSearch))
            {
                lstFilterItems = new ObservableCollection<ItemModel>(lstItems);
                return;
            }
            lstFilterItems = new ObservableCollection<ItemModel>(lstItems.Where(a => a.ItemName.Contains(TxtSearch)));
        }

        void OnRefresh()
        {
            IsRefresh = false;
            lstFilterItems = new ObservableCollection<ItemModel>(lstItems);
            TxtSearch = "";
        } 
        void onNavBack()
        {
            CoreMethods.PopPageModel();
        }

        void onSelectedItem(ItemModel itemModel)
        {
            CoreMethods.PushPageModel<ItemDetailsViewModel>(itemModel.ItemId);
        }
        #endregion

    }
}
