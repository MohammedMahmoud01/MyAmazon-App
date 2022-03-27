using System;
using System.Collections.Generic;
using System.Text;

namespace Amazon.Models
{
    public class ItemModel
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string ItemNameAr { get; set; }
        public string ImageName { get; set; }
        public string CategoryName { get; set; }
        public string SubCategoryName { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public decimal SalesPrice { get; set; }
        public decimal PurchasePrice { get; set; }
        public int Quantity { get; set; }
    }
}
