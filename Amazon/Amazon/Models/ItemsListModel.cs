using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Amazon.Models
{
    public class ItemsListModel
    {
        public ObservableCollection<ItemModel> lstPopularItems { get; set; }
        public ObservableCollection<ItemModel> lstNewItems { get; set; }
    }
}
