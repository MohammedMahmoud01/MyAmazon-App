using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Amazon.Models
{
    public class CatItemModel
    {
        public ObservableCollection<ItemModel> lstSamsung { get; set; }
        public ObservableCollection<ItemModel> lstXiaomi { get; set; }
        public ObservableCollection<ItemModel> lstApple { get; set; }
    }
}
