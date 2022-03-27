using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Amazon.Models
{
    public class HomePageModel
    {
        public ObservableCollection<ItemModel> lstNewItems { get; set; }
        public ObservableCollection<ItemModel> lstPopularItems { get; set; }
        public ObservableCollection<ItemDiscountModel> lstItemsDiscount { get; set; }
          
        public ObservableCollection<ItemImagesModel> lstImages { get; set; }
        public ObservableCollection<ItemModel> lstItems { get; set; }
    }
}
