using Amazon.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Amazon.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomPage : ContentPage
    {
        public CustomPage()
        {
            InitializeComponent();
            if (Settings.Language == "ar")
                this.FlowDirection = FlowDirection.RightToLeft;
            else
                this.FlowDirection = FlowDirection.LeftToRight;
        }
    }
}