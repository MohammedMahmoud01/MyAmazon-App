using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amazon.Droid
{
    [Activity(Label = "MyAmazon" , MainLauncher = true , Theme = "@style/Theme.Splash")]
    public class Splash : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            System.Threading.Thread.Sleep(50);
            this.StartActivity(typeof(MainActivity));
            // Create your application here
        }
    }
}