using System;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using Tollminder.Core.Helpers;
using Tollminder.Core.Models;
using Tollminder.Core.Services;

namespace Tollminder.Core.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        const string Login = "stweb@live.com";
        const string Password = "1";

        string _loginString;
        public string LoginString
        {
            get { return _loginString; }
            set
            {
                _loginString = value;
                RaisePropertyChanged(() => LoginString);
            }
        }

        IStoredSettingsService _storedSettingsService;
        public IStoredSettingsService StoredSettingsService 
        {
            get { return _storedSettingsService ?? (_storedSettingsService = Mvx.Resolve<IStoredSettingsService>());}
        }

        string _passwordString;
        public string PasswordString
        {
            get { return _passwordString; }
            set
            {
                _passwordString = value;
                RaisePropertyChanged(() => PasswordString);
            }
        }

        public LoginData EmailLoginData
        {
            get
            {
                return new LoginData()
                {
                    Email = LoginString,
                    Password = PasswordString,
                    Source = AuthorizationType.Email
                };
            }
        }

        public override void Init()
        {
            base.Init();
            LoginString = Login;
            PasswordString = Password;
        }

        protected override void SetValidators()
        {
            Validators.Add(new Validator("Login", "Field can't' be empty", () => string.IsNullOrEmpty(LoginString)));
            Validators.Add(new Validator("Login", "E-mail is not correct", () => !LoginString.ValidateRegExpression(RegularExpressionHelper.EmailRegexpr)));
            Validators.Add(new Validator("Password", "Field can't' be empty", () => string.IsNullOrEmpty(PasswordString)));
        }

        MvxCommand<LoginData> _loginCommand;
        public ICommand LoginCommand
        {
            get
            {
                return _loginCommand ?? (_loginCommand = new MvxCommand<LoginData>(async (data) => await ServerCommandWrapper(async () => await LoginTask(data))));
            }
        }

        async Task LoginTask(LoginData data)
        {
            bool success = false;

            switch(data.Source)
            {
                case AuthorizationType.Email:
                    if (Validate())
                    {

                        if (LoginString == Login && PasswordString == Password)
                        {
                            success = true;
                        }
                    }
                    break;
                case AuthorizationType.Facebook:
                case AuthorizationType.GPlus:
                    success = true;
                    break;
            }

            if (success)
            {
                StoredSettingsService.IsAuthorized = true;
                ShowViewModel<HomeViewModel>();
            }
        }

        MvxCommand _registrationCommand;
        public ICommand RegistrationCommand
        {
            get
            {
                return _registrationCommand ?? (_registrationCommand = new MvxCommand(() =>
                {
                    
                }));
            }
        }

        MvxCommand _forgotPasswordCommand;
        public ICommand ForgotPasswordCommand
        {
            get
            {
                return _forgotPasswordCommand ?? (_forgotPasswordCommand = new MvxCommand(() => 
                {
                    
                }));
            }
        }
    }
}
