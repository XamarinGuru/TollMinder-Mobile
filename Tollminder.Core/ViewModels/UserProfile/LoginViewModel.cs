﻿using System;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using Tollminder.Core.Models;
using Tollminder.Core.Services;
using Tollminder.Core.Services.Implementation;

namespace Tollminder.Core.ViewModels.UserProfile
{
    public class LoginViewModel : BaseViewModel
    {
        const string Login = "380000000000";
        const string Password = "123456789";
        private DataBaseService _dataBaseService;
        IStoredSettingsService _storedSettingsService;
        IFacebookLoginService _facebookLoginService;
        IGPlusLoginService _gPlusLoginService;
        private string userName;

        public LoginViewModel()
        {
            _storedSettingsService = Mvx.Resolve<IStoredSettingsService>();
            _facebookLoginService = Mvx.Resolve<IFacebookLoginService>();
            _gPlusLoginService = Mvx.Resolve<IGPlusLoginService>();
            _emailLoginCommand = new MvxCommand(() => ServerCommandWrapperAsync(() => LoginTaskAsync(EmailLoginData)));
            _facebookLoginCommand = new MvxCommand(() => ServerCommandWrapperAsync(async () => await LoginTaskAsync(await _facebookLoginService.GetPersonDataAsync())));
            _gPlusLoginCommand = new MvxCommand(() => ServerCommandWrapperAsync(async () => await LoginTaskAsync(await _gPlusLoginService.GetPersonDataAsync())));
            _registrationCommand = new MvxCommand(() => { ShowViewModel<RegistrationViewModel>(); });
            _forgotPasswordCommand = new MvxCommand(() => { });
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

            _facebookLoginService.Initialize();
            _gPlusLoginService.Initialize();
        }

        MvxCommand _emailLoginCommand;
        public ICommand EmailLoginCommand { get { return _emailLoginCommand; } }

        MvxCommand _facebookLoginCommand;
        public ICommand FacebookLoginCommand { get { return _facebookLoginCommand; } }

        MvxCommand _gPlusLoginCommand;
        public ICommand GPlusLoginCommand { get { return _gPlusLoginCommand; } }

        MvxCommand _registrationCommand;
        public ICommand RegistrationCommand { get { return _registrationCommand; } }

        MvxCommand _forgotPasswordCommand;
        public ICommand ForgotPasswordCommand { get { return _forgotPasswordCommand; } }

        string _loginString;
        public string LoginString
        {
            get { return _loginString; }
            set { SetProperty(ref _loginString, value); }
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

        protected override void SetValidators()
        {
            Validators.Add(new Validator("Login", "Field can't' be empty.", () => string.IsNullOrEmpty(LoginString)));
            //Validators.Add(new Validator("Login", "E-mail is not correct", () => !LoginString.ValidateRegExpression(RegularExpressionHelper.EmailRegexpr)));
            Validators.Add(new Validator("Password", "Field can't' be empty.", () => string.IsNullOrEmpty(PasswordString)));
        }

        async Task LoginTaskAsync(SocialData data)
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
                        result = await _serverApiService.SignInAsync(LoginString, PasswordString);
                        success = result != null;
                    }
                    break;
                case AuthorizationType.Facebook:
                    result = await _serverApiService.FacebookSignInAsync(data.Id, data.Source.ToString().ToLower());
                    if (result != null)
                    {
                        userName = data.FullName;
                        _storedSettingsService.Profile = GetProfileFromResponse(result, data);
                        SettingsService.SocialRegistartionSource = true;
                        success = CheckHttpStatuseCode(result.StatusCode);
                    }
                    break;
                case AuthorizationType.GPlus:
                    result = await _serverApiService.GooglePlusSignInAsync(data.Email, data.Source.ToString().ToLower());
                    if (result != null)
                    {
                        userName = data.FullName;
                        _storedSettingsService.Profile = GetProfileFromResponse(result, data);
                        success = CheckHttpStatuseCode(result.StatusCode);
                        SettingsService.SocialRegistartionSource = true;
                    }
                    break;
            }

            if (success)
            {
                //_dataBaseService.SetUser(result);
                _storedSettingsService.Profile = result;
                _storedSettingsService.IsAuthorized = true;
                _storedSettingsService.ProfileId = result.Id;
                _storedSettingsService.AuthToken = result.Token;
                Close(this);
                ShowViewModel<HomeViewModel>(new { name = result.FirstName, message = "Welcome back, " });
            }
        }

        private Profile GetProfileFromResponse(Profile profileFromResponse, SocialData data)
        {
            if (profileFromResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                profileFromResponse.FirstName = data.FirstName;
                profileFromResponse.LastName = data.LastName;
                profileFromResponse.Email = data.Email;
                profileFromResponse.FacebookId = data.Id;
                profileFromResponse.Source = data.Source.ToString().ToLower();
            }
            return profileFromResponse;
        }

        bool CheckHttpStatuseCode(System.Net.HttpStatusCode statusCode)
        {
            switch (statusCode)
            {
                case System.Net.HttpStatusCode.Unauthorized:
                case System.Net.HttpStatusCode.NotFound:
                    Close(this);
                    ShowViewModel<RegistrationViewModel>(new { name = userName });
                    break;
                case System.Net.HttpStatusCode.OK:
                    return true;
                case System.Net.HttpStatusCode.BadRequest:
                    return false;
            }
            return false;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            _facebookLoginService.ReleaseResources();
            _gPlusLoginService.ReleaseResources();
        }
    }
}
