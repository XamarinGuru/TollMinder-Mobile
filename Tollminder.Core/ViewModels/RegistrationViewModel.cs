using System;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using Tollminder.Core.Models;
using Tollminder.Core.Services;

namespace Tollminder.Core.ViewModels
{
    public class RegistrationViewModel: BaseViewModel
    {
        private Profile profileData;
        private string phoneCode;
        IStoredSettingsService storedSettingsService;
        IServerApiService serverApiService;

        public RegistrationViewModel()
        {
            storedSettingsService = Mvx.Resolve<IStoredSettingsService>();
            serverApiService = Mvx.Resolve<IServerApiService>();

            backToLoginViewCommand = new MvxCommand(() => { ShowViewModel<LoginViewModel>(); });
            registrationCommand = new MvxCommand(async () => await ServerCommandWrapper(async () => await Registration()));
            validateCommand = new MvxCommand(() => ComparePhoneCode());
        }

        public void Init(string name, Profile profile)
        {
            profileData = profile;
            if(name != null)
                Mvx.Resolve<IProgressDialogManager>().CloseAndShowMessage("Welcome " + name, "Please continue registration.");
        }

        public override void Start()
        {
            base.Start();
        }

        protected override void SetValidators()
        {
            Validators.Add(new Validator("SmsCode", "Field can't' be empty.", () => string.IsNullOrEmpty(SmsCode)));
            Validators.Add(new Validator("PhoneNumber", "Field can't' be empty.", () => string.IsNullOrEmpty(PhoneNumber)));
        }

        MvxCommand backToLoginViewCommand;
        public ICommand BackToLoginViewCommand { get { return backToLoginViewCommand; } }
        MvxCommand registrationCommand;
        public ICommand RegistrationCommand { get { return registrationCommand; } }
        MvxCommand validateCommand;
        public ICommand ValidateCommand { get { return validateCommand; } }

        string phoneNumber;
        public string PhoneNumber
        {
            get { return phoneNumber; }
            set { SetProperty(ref phoneNumber, value); }
        }

        string smsCode;
        public string SmsCode
        {
            get { return smsCode; }
            set { SetProperty(ref smsCode, value); }
        }

        bool isSmsValidationHidden;
        public bool IsSmsValidationHidden
        {
            get { return isSmsValidationHidden; }
            set
            {
                SetProperty(ref isSmsValidationHidden, value);
                RaisePropertyChanged(() => IsSmsValidationHidden);
            }
        }

        void ComparePhoneCode()
        {
            if(SmsCode == profileData.PhoneCode ? true : false)
            {
                storedSettingsService.Profile = profileData;
                storedSettingsService.IsAuthorized = true;
                storedSettingsService.ProfileId = profileData.Id;
                storedSettingsService.AuthToken = profileData.Token;
                Close(this);
                ShowViewModel<HomeViewModel>();
            }
            else
                Mvx.Resolve<IProgressDialogManager>().ShowProgressDialog("Error", "Wrong SMS code! Please try again.");
        }

        async Task Registration()
        {
            Mvx.Resolve<IProgressDialogManager>().ShowProgressDialog("Registration", "Please wait!");

            profileData.Phone = PhoneNumber;

            var result = await serverApiService.SignUp(profileData);
            if (CheckHttpStatuseCode(result.StatusCode))
            {
                Mvx.Resolve<IProgressDialogManager>().CloseProgressDialog();
                IsSmsValidationHidden = true;
                profileData = result;
            }
        }

        bool CheckHttpStatuseCode(System.Net.HttpStatusCode statusCode)
        {
            switch (statusCode)
            {
                case System.Net.HttpStatusCode.Found:
                    Mvx.Resolve<IProgressDialogManager>().CloseAndShowMessage("Error", "User with this phone number already registrated. Please, enter diferent number or try login.");
                    break;
                case System.Net.HttpStatusCode.OK:
                    return true;
                case System.Net.HttpStatusCode.BadRequest:
                    Mvx.Resolve<IProgressDialogManager>().CloseAndShowMessage("Error", "Server error!");
                    return false;
            }
            return false;
        }
    }
}
