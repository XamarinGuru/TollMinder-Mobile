using System;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using Tollminder.Core.Helpers;
using Tollminder.Core.Models;
using Tollminder.Core.Services;
using Tollminder.Core.Services.Implementation;

namespace Tollminder.Core.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        const string Login = "380000000000";
        const string Password = "123456789";
        private DataBaseService _dataBaseService;

        string _loginString;
        public string LoginString
        {
            get { return _loginString; }
            set { SetProperty(ref _loginString, value); }
        }

        IStoredSettingsService _storedSettingsService;
        public IStoredSettingsService StoredSettingsService
        {
            get { return _storedSettingsService ?? (_storedSettingsService = Mvx.Resolve<IStoredSettingsService>()); }
        }

        IFacebookLoginService _facebookLoginService;
        public IFacebookLoginService FacebookLoginService
        {
            get { return _facebookLoginService ?? (_facebookLoginService = Mvx.Resolve<IFacebookLoginService>()); }
        }

        IGPlusLoginService _gPlusLoginService;
        public IGPlusLoginService GPlusLoginService
        {
            get { return _gPlusLoginService ?? (_gPlusLoginService = Mvx.Resolve<IGPlusLoginService>()); }
        }

        string _passwordString;
        public string PasswordString
        {
            get { return _passwordString; }
            set { SetProperty(ref _passwordString, value); }
        }

        public SocialData EmailLoginData
        {
            get
            {
                return new SocialData()
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
            _dataBaseService = new DataBaseService();
        }

        public override void Start()
        {
            base.Start();

            FacebookLoginService.Initialize();
            GPlusLoginService.Initialize();
        }

        protected override void SetValidators()
        {
            Validators.Add(new Validator("Login", "Field can't' be empty", () => string.IsNullOrEmpty(LoginString)));
            //Validators.Add(new Validator("Login", "E-mail is not correct", () => !LoginString.ValidateRegExpression(RegularExpressionHelper.EmailRegexpr)));
            Validators.Add(new Validator("Password", "Field can't' be empty", () => string.IsNullOrEmpty(PasswordString)));
        }

        MvxCommand _emailLoginCommand;
        public ICommand EmailLoginCommand
        {
            get
            {
                return _emailLoginCommand ?? (_emailLoginCommand = new MvxCommand(async () => await ServerCommandWrapper(async () => await LoginTask(EmailLoginData))));
            }
        }

        MvxCommand _facebookLoginCommand;
        public ICommand FacebookLoginCommand
        {
            get
            {
                return _facebookLoginCommand ?? (_facebookLoginCommand = new MvxCommand(async () => await ServerCommandWrapper(async () => await LoginTask(await FacebookLoginService.GetPersonData()))));
            }
        }

        MvxCommand _gPlusLoginCommand;
        public ICommand GPlusLoginCommand
        {
            get
            {
                return _gPlusLoginCommand ?? (_gPlusLoginCommand = new MvxCommand(async () => await ServerCommandWrapper(async () => await LoginTask(await GPlusLoginService.GetPersonData()))));
            }
        }

        async Task LoginTask(SocialData data)
        {
            if (data == null)
            {
                Mvx.Trace("Received empty data for login");
                return;
            }

            var success = false;
            var result = default(Profile);
            var _serverApiService = Mvx.Resolve<IServerApiService>();
            
            switch (data.Source)
            {
                case AuthorizationType.Email:
                    if (Validate())
                    {
                        result = await _serverApiService.SignIn(LoginString, PasswordString);
                        success = result != null;
                    }
                    break;
                case AuthorizationType.Facebook:
                    result = await _serverApiService.SocialSignIn(data.Email, data.Source.ToString());
                    success = result != null;
                    break;
                case AuthorizationType.GPlus:
                    result = await _serverApiService.SocialSignIn(data.Email, data.Source.ToString());
                    //Profile
                    //{
                    //    FirstName = data.FirstName,
                    //    LastName = data.LastName,
                    //    Email = data.Email,
                    //    Source = data.Source.ToString()
                    //};
                    success = result != null;
                    break;
            }

            if (success)
            {
                //_dataBaseService.SetUser(result);
                StoredSettingsService.Profile = result;
                StoredSettingsService.IsAuthorized = true;
                StoredSettingsService.ProfileId = result.Id;
                StoredSettingsService.AuthToken = result.Token;
                Close(this);
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

        public override void OnDestroy()
        {
            base.OnDestroy();
            FacebookLoginService.ReleaseResources();
            GPlusLoginService.ReleaseResources();
        }
    }
}
