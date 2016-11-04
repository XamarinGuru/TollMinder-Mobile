using System;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;
using Tollminder.Core.Helpers;
using Tollminder.Core.Models;

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

        MvxCommand _loginCommand;
        public ICommand LoginCommand
        {
            get
            {
                return _loginCommand ?? (_loginCommand = new MvxCommand(async () => await ServerCommandWrapper(LoginTask)));
            }
        }

        async Task LoginTask()
        {
            if (Validate())
            {
                if (LoginString == Login && PasswordString == Password)
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
