using Amazon.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Amazon.Helpers
{
    class ItemSelector : DataTemplateSelector
    {
        public DataTemplate TemplateCart1 { get; set; }
        public DataTemplate TemplateCart2 { get; set; }
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            //ItemModel model = item as ItemModel;
            //if (model != null)
            //    return TemplateCart1;
            //else
            //    return TemplateCart2;
            return TemplateCart2;
        }
    }
}
