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
    public partial class ToolBar : ContentView
    {
        public ToolBar()
        {
            InitializeComponent();
        }
        public event EventHandler<string> MenuClick;

        #region Title Property
        public static readonly BindableProperty TitleProperty = BindableProperty.Create("Title", typeof(string), typeof(ContentView), "", propertyChanged: OnEventNameChanged);

        public string Title
        {
            get { return (string)base.GetValue(TitleProperty); }
            set
            {
                base.SetValue(TitleProperty, value);
            }
        }
       
        static void OnEventNameChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue == null)
                return;
            var tolbal = bindable as ToolBar;
            tolbal.lblTitle.Text = newValue.ToString();
        }
        #endregion

        

        #region HasBackButton Property
        public static readonly BindableProperty HasBackButtonProperty = BindableProperty.Create("HasBackButton", typeof(bool), typeof(ContentView), true, propertyChanged: OnBackButtonChanged);

        public bool HasBackButton
        {
            get { return (bool)base.GetValue(HasBackButtonProperty); }
            set
            {
                base.SetValue(HasBackButtonProperty, value);
            }
        }

        static void OnBackButtonChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue == null)
                return;
            var tolbal = bindable as ToolBar;
            var fontSource = new FontImageSource();
            fontSource.FontFamily = "SolidIcon";
            if ((bool)newValue)
                fontSource.Glyph = FontAwesome.FontAwesomeIcons.ArrowLeft;
            else
                fontSource.Glyph = FontAwesome.FontAwesomeIcons.Bars;

            tolbal.imgBack.Source = fontSource;
        }

        #endregion

        #region Has Image Bar Property
        public bool HasImgBar
        {
            get { return (bool)base.GetValue(HasImgBarProperty); }
            set
            {
                base.SetValue(HasImgBarProperty, value);
            }
        }

        public static readonly BindableProperty HasImgBarProperty = BindableProperty.Create("HasImgBar", typeof(bool), typeof(ContentView), true, propertyChanged: onHasImgBar);

        static void onHasImgBar(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue == null)
                return;
            var tolbal = bindable as ToolBar;
            if ((bool)newValue)
                tolbal.imgBar.Source = "icon.png";
            else
                tolbal.imgBar.Source = "";
        }

        #endregion



        #region Cart Bar
        public bool HasCartBar
        {
            get { return (bool)base.GetValue(HasCartProperty); }
            set
            {
                base.SetValue(HasCartProperty, value);
            }
        }

        public static readonly BindableProperty HasCartProperty = BindableProperty.Create("HasCartBar", typeof(bool), typeof(ContentView), true, propertyChanged: onCartBar);


        static void onCartBar(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue == null)
                return;
            var tolbal = bindable as ToolBar;
            if ((bool)newValue)
                tolbal.stackCart.IsVisible = true;
            else
                tolbal.stackCart.IsVisible = false;
        }
        #endregion

        #region Cart Title

        public static readonly BindableProperty CartTitleProperty = BindableProperty.Create("CartTitle", typeof(string), typeof(ContentView), "", propertyChanged: OnCartNameChanged);

        public string CartTitle
        {
            get { return (string)base.GetValue(CartTitleProperty); }
            set
            {
                base.SetValue(CartTitleProperty, value);
            }
        }
        static void OnCartNameChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue == null)
                return;
            var tolbal = bindable as ToolBar;
            tolbal.lblCartNum.Text = newValue.ToString();
        }

        #endregion
        private async void imgBackOrMenu(object sender, EventArgs e)
        {
            if (HasBackButton)
                await Navigation.PopAsync();
            else
                MenuClick?.Invoke(this, "ali");
        }

    }
}