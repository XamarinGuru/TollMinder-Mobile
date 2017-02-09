using System;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmValidation;
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

        public void Init(string name)
        {
            profileData = storedSettingsService.Profile;
            if(name != null)
                Mvx.Resolve<IProgressDialogManager>().CloseAndShowMessage("Welcome " + name, "Please continue registration.");
        }

        public override void Start()
        {
            base.Start();
        }

        protected override void SetValidators()
        {
            Validator.AddRequiredRule(() => PhoneNumber, "Field can't' be empty.");
            Validator.AddRequiredRule(() => SmsCode, "Field can't' be empty.");
            //Validators.Add(new Validator("SmsCode", "Field can't' be empty.", () => string.IsNullOrEmpty(SmsCode)));
            //Validators.Add(new Validator("PhoneNumber", "Field can't' be empty.", () => string.IsNullOrEmpty(PhoneNumber)));
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

        async Task ComparePhoneCode()
        {
            //if (!Validate())
            //{
            //    Mvx.Resolve<IProgressDialogManager>().ShowMessage("Error", "Field can't' be empty.");
            //    return;
            //}

            //if (SmsCode == profileData.PhoneCode)
            if(SmsCode == "1111")
            {
                profileData.PhoneValidate = true;
                var result = await serverApiService.SaveProfile(profileData.Id, profileData, profileData.Token);
                if (CheckHttpStatuseCode(result.StatusCode))
                {
                    storedSettingsService.Profile = profileData;
                    storedSettingsService.IsAuthorized = true;
                    storedSettingsService.ProfileId = profileData.Id;
                    storedSettingsService.AuthToken = profileData.Token;
                    Close(this);
                    ShowViewModel<HomeViewModel>();
                }
            }
            else
                Mvx.Resolve<IProgressDialogManager>().ShowMessage("Error", "Wrong SMS code! Please try again.");
        }

        async Task Registration()
        {
            //if (!Validate())
            //{
            //    Mvx.Resolve<IProgressDialogManager>().ShowMessage("Error", "Field can't' be empty.");
            //    return;
            //}
            
            //Mvx.Resolve<IProgressDialogManager>().ShowProgressDialog("Registration", "Please wait!");

            profileData.Phone = PhoneNumber;

            var result = await serverApiService.SignUp(profileData);
            if (CheckHttpStatuseCode(result.StatusCode))
            {
                Mvx.Resolve<IProgressDialogManager>().CloseProgressDialog();
                SmsCode = "1111";
                IsSmsValidationHidden = true;
                profileData = result;
            }
        }

        bool CheckHttpStatuseCode(System.Net.HttpStatusCode statusCode)
        {
            switch (statusCode)
            {
                case System.Net.HttpStatusCode.Found:
                    Mvx.Resolve<IProgressDialogManager>().ShowMessage("Error", "User with this phone number already registrated. Please, enter diferent number or try again.");
                    break;
                case System.Net.HttpStatusCode.OK:
                    return true;
                case System.Net.HttpStatusCode.NotFound:
                case System.Net.HttpStatusCode.BadRequest:
                case System.Net.HttpStatusCode.Unauthorized:
                    Mvx.Resolve<IProgressDialogManager>().ShowMessage("Error", "Server error!");
                    return false;
            }
            return false;
        }
    }
}