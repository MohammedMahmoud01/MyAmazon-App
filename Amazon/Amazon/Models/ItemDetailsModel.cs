using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Amazon.Models
{
    public class ItemDetailsModel
    {
        public ObservableCollection<ItemImagesModel> lstImages { get; set; }
        public ItemDiscountModel itemDiscountModel { get; set; }
        public ItemModel itemModel { get; set; }
    }
}
