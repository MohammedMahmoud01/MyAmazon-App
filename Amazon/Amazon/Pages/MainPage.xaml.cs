using Amazon.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Amazon.Pages
{
    public partial class MainPage : Controls.CustomPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void ClkNavDrawer(object sender, string e)
        {
            if(Settings.Language == "en")
            {
                navigationDrawer.Position = Syncfusion.SfNavigationDrawer.XForms.Position.Left;
                navigationDrawer.IsOpen = true;
            }
            else
            {
                navigationDrawer.Position = Syncfusion.SfNavigationDrawer.XForms.Position.Right;
                navigationDrawer.IsOpen = true;
            }
        }
 
    }
}
