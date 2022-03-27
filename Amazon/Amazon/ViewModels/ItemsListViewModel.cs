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
    public class ItemsListViewModel : FreshBasePageModel
    {
        IDataServices ctx;
        public ItemsListViewModel(IDataServices service)
        {
            ctx = service;

            SelectedItem = new Command<ItemModel>(onSelectedItem);
        }
        public override async void Init(object initData)
        {
            string filter = initData as string;

            if(filter == "Xiaomi" || filter == "Apple" || filter == "Samsung")
            {
                CatItemModel model = await ctx.GetCatItemsData(1);

                if (filter == "Xiaomi")
                    lstItems = model.lstXiaomi;

                else if (filter == "Apple")
                    lstItems = model.lstApple;

                else 
                    lstItems = model.lstSamsung;
            }
            else
            {
                ItemsListModel model = await ctx.GetItemsListData(1);

                if (filter == "Popular")
                    lstItems = model.lstPopularItems;

                else
                    lstItems = model.lstNewItems;
            }       

            base.Init(initData);
        }

        #region Observable Collection
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

        #endregion

        public Command SelectedItem { get; set; }

        void onSelectedItem(ItemModel itemModel)
        {
            CoreMethods.PushPageModel<ItemDetailsViewModel>(itemModel.ItemId);
        }
    }
}
