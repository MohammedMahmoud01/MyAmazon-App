using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Amazon.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemDetailsPage : Controls.CustomPage
    {
        public ItemDetailsPage()
        {
            InitializeComponent();
        }
    }
}