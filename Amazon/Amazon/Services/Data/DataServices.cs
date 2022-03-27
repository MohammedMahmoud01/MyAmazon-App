using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Akavache;
using Amazon.Helpers;
using Amazon.Models;
using System.Reactive.Linq;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace Amazon.Services.Data
{
    public interface IDataServices
    {
        #region Home Page Data
        Task<HomePageModel> GetHomePageData(int? Lang);
        Task<CatItemModel> GetHomePageData2(int? Lang);
        Task<ObservableCollection<SliderModel>> GetSlidersData();
        Task<ObservableCollection<CategoryModel>> GetCategoryData();
        #endregion

        #region Items List Page Data
        Task<CatItemModel> GetCatItemsData(int? Lang);
        Task<ItemsListModel> GetItemsListData(int? Lang); 
        #endregion

        #region Item Details Data
        Task<ItemDetailsModel> GetItemDetailsData(int itemId); 
        #endregion

        #region Post Data
        Task<UserModel> SignIn(UserModel model);
        Task<bool> Register(UserModel model);
        #endregion

        Task<ObservableCollection<ItemModel>> GetAllItemsData();
    }
    public class DataServices : IDataServices
    {
        #region Akavache Parameters
        string HomePageKey = "HomePage_Key";

        string HomePageKey2 = "HomePage_Key2";

        string SliderKey = "Slider_Key";

        string CatItemDataKey = "CatItemData_Key";

        string ItemsListKey = "ItemsListData_Key";

        string CategoryListKey = "CategoryList_Key";
        #endregion

        #region Constructor
        GenericRepository oRep;
        public DataServices(GenericRepository repository)
        {
            oRep = repository;
        } 
        #endregion

        #region Home Page Data
        public async Task<HomePageModel> GetHomePageData(int? Lang)
        {
            
            var HomeModelCash = new HomePageModel();
            try
            {
                HomeModelCash = await BlobCache.LocalMachine.GetObject<HomePageModel>(HomePageKey);
            }
            catch
            {
                HomeModelCash = null;
            }
            if (HomeModelCash != null && Lang == 1)
            {
                var HomeModelCash2 = HomePageDataHelper(HomeModelCash);

                return HomeModelCash2;
            }
            if (HomeModelCash != null)
            {
                return HomeModelCash;
            }
            else
            {
                HomeModelCash = await oRep.GetAsync<HomePageModel>(Utility.ServerUrl + "/api/homePageApi");

                await BlobCache.LocalMachine.InsertObject(HomePageKey, HomeModelCash, DateTimeOffset.Now.AddHours(3));

                var HomeModelCash2 = HomePageDataHelper(HomeModelCash);

                return HomeModelCash2;
            }

        }
        private HomePageModel HomePageDataHelper(HomePageModel HomeModelCash)
        {
            foreach (var popularItem in HomeModelCash.lstPopularItems)
                popularItem.ItemName = (Helpers.Settings.Language == "ar") ? popularItem.ItemNameAr : popularItem.ItemName;

            foreach (var newItem in HomeModelCash.lstNewItems)
                newItem.ItemName = (Helpers.Settings.Language == "ar") ? newItem.ItemNameAr : newItem.ItemName;

            foreach (var discountItem in HomeModelCash.lstItemsDiscount)
                discountItem.ItemName = (Helpers.Settings.Language == "ar") ? discountItem.ItemNameAr : discountItem.ItemName;


            return HomeModelCash;
        }

        public async Task<CatItemModel> GetHomePageData2(int? Lang)
        {
            var HomeModelCash = new CatItemModel();
            try
            {
                HomeModelCash = await BlobCache.LocalMachine.GetObject<CatItemModel>(HomePageKey2);
            }
            catch
            {
                HomeModelCash = null;
            }
            if (HomeModelCash != null && Lang == 1)
            {
                var HomeModelCash2 = CatItemsDataHelper(HomeModelCash);

                return HomeModelCash2;
            }
            if (HomeModelCash != null)
            {
                return HomeModelCash;
            }
            else
            {
                HomeModelCash = await oRep.GetAsync<CatItemModel>(Utility.ServerUrl + "/api/homePageApi2");

                await BlobCache.LocalMachine.InsertObject(HomePageKey2, HomeModelCash, DateTimeOffset.Now.AddHours(3));

                var HomeModelCash2 = CatItemsDataHelper(HomeModelCash);

                return HomeModelCash2;
            }
        }
        private CatItemModel CatItemsDataHelper(CatItemModel HomeModelCash)
        {
            foreach (var samsung in HomeModelCash.lstSamsung)
                samsung.ItemName = (Helpers.Settings.Language == "ar") ? samsung.ItemNameAr : samsung.ItemName;

            foreach (var apple in HomeModelCash.lstApple)
                apple.ItemName = (Helpers.Settings.Language == "ar") ? apple.ItemNameAr : apple.ItemName;

            foreach (var xiaomi in HomeModelCash.lstXiaomi)
                xiaomi.ItemName = (Helpers.Settings.Language == "ar") ? xiaomi.ItemNameAr : xiaomi.ItemName;


            return HomeModelCash;
        }

        public async Task<ObservableCollection<SliderModel>> GetSlidersData()
        {
            ObservableCollection<SliderModel> SliderCash = new ObservableCollection<SliderModel>();
            try
            {
                SliderCash = await BlobCache.LocalMachine.GetObject<ObservableCollection<SliderModel>>(SliderKey);
            }
            catch
            {
                SliderCash = null;
            }
            if (SliderCash != null)
            {
                return SliderCash;
            }
            else
            {
                SliderCash = new ObservableCollection<SliderModel>();

                SliderCash = await oRep.GetAsync<ObservableCollection<SliderModel>>(Utility.ServerUrl + "/api/SliderApi");

                await BlobCache.LocalMachine.InsertObject(SliderKey, SliderCash, DateTimeOffset.Now.AddHours(3));

                return SliderCash;
            }


        }

        public async Task<ObservableCollection<CategoryModel>> GetCategoryData()
        {
            ObservableCollection<CategoryModel> catModel = new ObservableCollection<CategoryModel>();

            try
            {
                catModel = await BlobCache.LocalMachine.GetObject<ObservableCollection<CategoryModel>>(CategoryListKey);
            }
            catch
            {
                catModel = null;
            }
            if (catModel != null)
            {
                return catModel;
            }
            else
            {
                catModel = await oRep.GetAsync<ObservableCollection<CategoryModel>>(Utility.ServerUrl + "/api/CatApi");

                await BlobCache.LocalMachine.InsertObject(CategoryListKey, catModel, DateTimeOffset.Now.AddHours(3));

                return catModel;
            }       
        }

        #endregion

        #region Items List Data

        public async Task<CatItemModel> GetCatItemsData(int? Lang)
        {
            var CatItemsModelCash = new CatItemModel();
            try
            {
                CatItemsModelCash = await BlobCache.LocalMachine.GetObject<CatItemModel>(CatItemDataKey);
            }
            catch
            {
                CatItemsModelCash = null;
            }
            if (CatItemsModelCash != null && Lang == 1)
            {
                var CatItemsModelCash2 = CatItemsDataHelper(CatItemsModelCash);

                return CatItemsModelCash2;
            }
            if (CatItemsModelCash != null)
            {
                return CatItemsModelCash;
            }
            else
            {
                CatItemsModelCash = await oRep.GetAsync<CatItemModel>(Utility.ServerUrl + "/api/ItemsCatapi");

                await BlobCache.LocalMachine.InsertObject(CatItemDataKey, CatItemsModelCash, DateTimeOffset.Now.AddHours(3));

                var CatItemsModelCash2 = CatItemsDataHelper(CatItemsModelCash);

                return CatItemsModelCash2;
            }
        }

        public async Task<ItemsListModel> GetItemsListData(int? Lang)
        {
            var ItemsListCash = new ItemsListModel();
            try
            {
                ItemsListCash = await BlobCache.LocalMachine.GetObject<ItemsListModel>(ItemsListKey);
            }
            catch
            {
                ItemsListCash = null;
            }
            if (ItemsListCash != null && Lang == 1)
            {
                var ItemsListCash2 = ItemsListDataHelper(ItemsListCash);

                return ItemsListCash2;
            }
            if (ItemsListCash != null)
            {
                return ItemsListCash;
            }
            else
            {
                ItemsListCash = await oRep.GetAsync<ItemsListModel>(Utility.ServerUrl + "/api/ItemsListApi");

                await BlobCache.LocalMachine.InsertObject(ItemsListKey, ItemsListCash, DateTimeOffset.Now.AddHours(3));

                var ItemsListCash2 = ItemsListDataHelper(ItemsListCash);

                return ItemsListCash2;
            }
        }
        private ItemsListModel ItemsListDataHelper(ItemsListModel ItemsListCash)
        {
            foreach (var NewItem in ItemsListCash.lstNewItems)
                NewItem.ItemName = (Settings.Language == "ar") ? NewItem.ItemNameAr : NewItem.ItemName;

            foreach (var Popular in ItemsListCash.lstPopularItems)
                Popular.ItemName = (Settings.Language == "ar") ? Popular.ItemNameAr : Popular.ItemName;

            return ItemsListCash;
        }
        #endregion


        #region Item Details Data
        public async Task<ItemDetailsModel> GetItemDetailsData(int itemId)
        {
            ItemDetailsModel model = await oRep.GetAsync<ItemDetailsModel>(Utility.ServerUrl + "/api/ItemApi/" + itemId);

            return model;
        }
        #endregion

        #region Post Data
        public async Task<UserModel> SignIn(UserModel model)
        {
            UserModel user = await oRep.PostAsync(Utility.ServerUrl + "/api/LoginApi", model);

            if (user.Email != null)
            {
                Settings.UserInfo = JsonConvert.SerializeObject(user);
                return user;
            }
            else
            {
                return new UserModel();
            }
        }

        public async Task<bool> Register(UserModel model)
        {
            var json = await oRep.PostDataAsync(Utility.ServerUrl + "/api/SignUpApi", model);

            string result = json.ToString();
            if (result == "Done")
                return true;
            else
                return false;

        }
        #endregion

        public async Task<ObservableCollection<ItemModel>> GetAllItemsData()
        {
            ObservableCollection<ItemModel> model;
            if (!string.IsNullOrEmpty(Settings.LstAllItems))
            {
                model = JsonConvert.DeserializeObject<ObservableCollection<ItemModel>>(Settings.LstAllItems);

                foreach (var item in model)
                    item.ItemName = (Settings.Language == "ar") ? item.ItemNameAr : item.ItemName;

                return model;
            }
            else
            {
                model = await oRep.GetAsync<ObservableCollection<ItemModel>>(Utility.ServerUrl + "/api/AllItemsApi");

                foreach (var item in model)
                    item.ItemName = (Settings.Language == "ar") ? item.ItemNameAr : item.ItemName;

                Settings.LstAllItems = JsonConvert.SerializeObject(model);

                return model;
            }

        }
    }
}
