using Newtonsoft.Json;
using Amazon.Helpers;
using Amazon.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using FreshMvvm;
using Xamarin.Forms;
using Amazon.Services.Data;

namespace Amazon.ViewModels
{
    class RegisterViewModel : FreshBasePageModel
    {
        IDataServices ctx;
        public RegisterViewModel(IDataServices service)
        {
            ctx = service;

            Register = new Command(NavToHomePage);
            BtnBack = new Command(NavBack);
            userRegister = new UserModel();          
        }


        #region Commands
        public Command Register { get; set; }
        public Command BtnBack { get; set; } 
        #endregion

        #region Models
        UserModel _userRegister;
        public UserModel userRegister
        {
            get { return _userRegister; }
            set
            {
                _userRegister = value;
                RaisePropertyChanged("userRegister");
            }
        }

        string _confirmPassword;
        public string confirmPassword
        {
            get { return _confirmPassword; }
            set
            {
                _confirmPassword = value;
                RaisePropertyChanged("confirmPassword");
            }
        }

        bool _AgreeTerms;
        public bool AgreeTerms
        {
            get { return _AgreeTerms; }
            set
            {
                _AgreeTerms = value;
                RaisePropertyChanged("AgreeTerms");
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
            if (string.IsNullOrEmpty(userRegister.FirstName) || string.IsNullOrEmpty(userRegister.LastName))
            {
                if (Settings.Language == "en")
                    await CoreMethods.DisplayAlert("Warning", "Please Enter Your First Name and Last Name Correctly", "Close");
                else
                    await CoreMethods.DisplayAlert("تحذير", "يجب ادخال الاسم الاول والاخير بطريقة صحيحة", "Close");
                return;
            }
            else if (userRegister.FirstName.Length < 3 || userRegister.LastName.Length < 3)
            {
                if (Settings.Language == "en")
                    await CoreMethods.DisplayAlert("Warning", "First Name and Last Name Must be more than 3 character", "Close");
                else
                    await CoreMethods.DisplayAlert("تحذير", "الاسم الاول والاخير يجب أن يكون أكثر من 3 حروف", "اغلاق");
                return;
            }
            else if (userRegister.FirstName.Contains(" ") || userRegister.LastName.Contains(" "))
            {
                if (Settings.Language == "en")
                    await CoreMethods.DisplayAlert("Warning", "First Name and Last Name Must Not Contains White Space", "Close");
                else
                    await CoreMethods.DisplayAlert("تحذير", " الاسم الاول والاخير يجب أن لا يحتوى على مسافات", "اغلاق");
                return;
            }
            else if (HaveSpecialChar(userRegister.FirstName))
            {
                if (Settings.Language == "en")
                    await CoreMethods.DisplayAlert("Warning", "First Name Must Not Have Special Character", "Close");
                else
                    await CoreMethods.DisplayAlert("تحذير", " الاسم الاول يجب أن لا يحتوى على رموز", "اغلاق");
                return;
            }
            else if (HaveSpecialChar(userRegister.LastName))
            {
                if (Settings.Language == "en")
                    await CoreMethods.DisplayAlert("Warning", "Last Name Must Not Have Special Character", "Close");
                else
                    await CoreMethods.DisplayAlert("تحذير", " الاسم الاخير يجب أن لا يحتوى على رموز", "اغلاق");
                return;
            }
            else if (string.IsNullOrEmpty(userRegister.Email) || string.IsNullOrEmpty(userRegister.Password))
            {
                if (Settings.Language == "en")
                    await CoreMethods.DisplayAlert("Warning", "Please Enter Your Email and Password Correctly", "Close");
                else
                    await CoreMethods.DisplayAlert("تحذير", "يجب ادخال البريد الالكترونى وكلمة السر بطريقة صحيحة", "اغلاق");
                return;
            }     
            else if (!IsValidEmail(userRegister.Email))
            {
                if (Settings.Language == "en")
                    await CoreMethods.DisplayAlert("Warning", "Email is not Valid", "Close");
                else
                    await CoreMethods.DisplayAlert("تحذير", "البريد الالكترونى غير صحيح", "اغلاق");
                return;
            }
            else if (userRegister.Password.Length < 8)
            {
                if (Settings.Language == "en")
                    await CoreMethods.DisplayAlert("Warning", "Password Must be More Than 8 Characters", "Close");
                else
                    await CoreMethods.DisplayAlert("تحذير", "كلمة المرور يجب ان تكون اكبر من 8 حروف", "اغلاق");
                return;
            }
            else if (!userRegister.Password.Any(char.IsUpper) || !userRegister.Password.Any(char.IsLower))
            {
                if (Settings.Language == "en")
                    await CoreMethods.DisplayAlert("Warning", "Password Must have Capital and Small Characters", "Close");
                else
                    await CoreMethods.DisplayAlert("تحذير", "كلمة المرور يجب ان تحتوى على حروف انجليزية Capital و Small", "اغلاق");
                return;
            }
            else if (userRegister.Password.Contains(" "))
            {
                if (Settings.Language == "en")
                    await CoreMethods.DisplayAlert("Warning", "Password White Space is not Valid", "Close");
                else
                    await CoreMethods.DisplayAlert("تحذير", "كلمة المرور يجب ان لا تحتوى على مسافات", "اغلاق");
                return;
            }
            else if (!HaveSpecialChar(userRegister.Password))
            {
                if (Settings.Language == "en")
                    await CoreMethods.DisplayAlert("Warning", "Password Must have Special Character", "Close");
                else
                    await CoreMethods.DisplayAlert("تحذير", "كلمة المرور يجب ان تحتوى رموز", "اغلاق");
                return;
            }
            else if (userRegister.Password != confirmPassword)
            {
                if (Settings.Language == "en")
                    await CoreMethods.DisplayAlert("Warning", "Password And ConfirmPassword Is Not Compatible", "Close");
                else
                    await CoreMethods.DisplayAlert("تحذير", "كلمة المرور وتأكيد كلمة المرور غير متطابقتان", "اغلاق");
                return;
            }
            else if (!AgreeTerms)
            {
                if (Settings.Language == "en")
                    await CoreMethods.DisplayAlert("Warning", "You Must Agree The Terms", "Close");
                else
                    await CoreMethods.DisplayAlert("تحذير", "يجب أن توافق على الشروط والاحكام", "اغلاق");
                return;
            }
            else
            {
                var result = await ctx.Register(userRegister);
                if (result == true)
                {
                    if (Settings.Language == "en")
                        await CoreMethods.DisplayAlert("Success", "You Sign Up Successfully. Welcome", "Close");
                    else
                        await CoreMethods.DisplayAlert("نجاح", "تم تسجيل مستخدم جديد", "اغلاق");

                    await CoreMethods.PopPageModel();
                }
                else
                {
                    if (Settings.Language == "en")
                        await CoreMethods.DisplayAlert("Warning", "Your Signed Up Failed", "Close");
                    else
                        await CoreMethods.DisplayAlert("تحذير", "هذا المستخدم موجود بالفعل", "اغلاق");
                    return;
                }
            }
        }

        #region Helpers Functions
        bool HaveSpecialChar(string sLetter)
        {
            string specialCh = @"%!@#$%^&*()?/>.<,:;'\|}]{[_~`+=-" + "\"";
            char[] charc = specialCh.ToCharArray();
            foreach (char ch in charc)
            {
                if (sLetter.Contains(ch))
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
