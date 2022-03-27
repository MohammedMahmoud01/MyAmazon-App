using Amazon.Helpers;
using Amazon.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Newtonsoft.Json;
using FreshMvvm;
using Amazon.Services.Data;

namespace Amazon.ViewModels
{
    class LoginViewModel : FreshBasePageModel
    {
        IDataServices ctx;
        public LoginViewModel(IDataServices services)
        {
            ctx = services;

            Login = new Command(NavToHomePage);
            BtnBack = new Command(NavBack);
            userLogin = new UserModel();
            NavToRegister = new Command(onNavToRegister);
        }

        #region Commands
        public Command Login { get; set; }
        public Command BtnBack { get; set; }
        public Command NavToRegister { get; set; }
        #endregion

        #region Models
        UserModel _userLogin;
        public UserModel userLogin
        {
            get { return _userLogin; }
            set
            {
                _userLogin = value;
                RaisePropertyChanged("userLogin");
            }
        } 
        #endregion


        #region Functions
        private async void NavBack()
        {
            await CoreMethods.PopPageModel();
        }

        private async void NavToHomePage()
        {
            if (string.IsNullOrEmpty(userLogin.Email) || string.IsNullOrEmpty(userLogin.Password))
            {
                if(Settings.Language =="en")
                    await CoreMethods.DisplayAlert("Warning", "Please Enter Your Email and Password Correctly", "Close");
                else
                    await CoreMethods.DisplayAlert("تحذير", "يجب ادخال البريد الالكترونى وكلمة السر بطريقة صحيحة", "اغلاق");
                return;
            }
            else if (!IsValidEmail(userLogin.Email))
            {
                if (Settings.Language == "en")
                    await CoreMethods.DisplayAlert("Warning", "Email is not Valid", "Close");
                else
                    await CoreMethods.DisplayAlert("تحذير", "البريد الالكترونى غير صحيح", "اغلاق");
                return;
            }
            else if (userLogin.Password.Length < 8)
            {
                if (Settings.Language == "en")
                    await CoreMethods.DisplayAlert("Warning", "Password Must be More Than 8 Characters", "Close");
                else
                    await CoreMethods.DisplayAlert("تحذير", "كلمة المرور يجب ان تكون اكبر من 8 حروف", "اغلاق");
                return;
            }
            else if (!userLogin.Password.Any(char.IsUpper) || !userLogin.Password.Any(char.IsLower))
            {
                if (Settings.Language == "en")
                    await CoreMethods.DisplayAlert("Warning", "Password Must have Capital and Small Characters", "Close");
                else
                    await CoreMethods.DisplayAlert("تحذير", "كلمة المرور يجب ان تحتوى على حروف انجليزية Capital و Small", "اغلاق");
                return;
            }
            else if (userLogin.Password.Contains(" "))
            {
                if(Settings.Language == "en")
                    await CoreMethods.DisplayAlert("Warning", "Password White Space is not Valid", "Close");
                else
                    await CoreMethods.DisplayAlert("تحذير", "كلمة المرور يجب ان لا تحتوى على مسافات", "اغلاق");
                return;
            }
            else if (!HaveSpecialChar())
            {
                if (Settings.Language == "en")
                    await CoreMethods.DisplayAlert("Warning", "Password Must have Special Character", "Close");
                else
                    await CoreMethods.DisplayAlert("تحذير", "كلمة المرور يجب ان تحتوى رموز", "اغلاق");
                return;
            }
            else
            {
                userLogin.FirstName = "x";
                userLogin.LastName = "x";

                var SignedUser =  await ctx.SignIn(userLogin);

                if (SignedUser.Email != null)
                {
                    if(Settings.Language == "en")
                        await CoreMethods.DisplayAlert("Success", "You Loged In Successfully", "Close");
                    else
                        await CoreMethods.DisplayAlert("نجاح", "تم تسجيل الدخول بنجاح", "اغلاق");

                    await CoreMethods.PopPageModel();
                }
                else
                {
                    if (Settings.Language == "en")
                        await CoreMethods.DisplayAlert("Warning", "Log In Failed Try Again", "Close");
                    else
                        await CoreMethods.DisplayAlert("تحذير", "فشل تسجيل الدخول", "اغلاق");
                    return;
                }

            }
        }

        void onNavToRegister()
        {
            CoreMethods.PushPageModel<RegisterViewModel>();
        }

        #region Helper Functions
        bool HaveSpecialChar()
        {
            string specialCh = @"%!@#$%^&*()?/>.<,:;'\|}]{[_~`+=-" + "\"";
            char[] charc = specialCh.ToCharArray();
            foreach (char ch in charc)
            {
                if (userLogin.Password.Contains(ch))
                    return true;
            }
            return false;
        }
        bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false; // suggested by @TK-421
            }
            else if (!trimmedEmail.Contains("."))
            {
                return false; // suggested by @TK-421
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }  
        #endregion
        #endregion

    }
}
