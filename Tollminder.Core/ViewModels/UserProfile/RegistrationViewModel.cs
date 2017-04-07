using System.Threading.Tasks;
using System.Windows.Input;
using Chance.MvvmCross.Plugins.UserInteraction;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmValidation;
using Tollminder.Core.Models;
using Tollminder.Core.Services.Api;
using Tollminder.Core.Services.Settings;

namespace Tollminder.Core.ViewModels.UserProfile
{
    public class RegistrationViewModel : BaseViewModel
    {
        private string phoneCode;

        readonly IStoredSettingsService storedSettingsService;
        readonly IServerApiService serverApiService;

        public RegistrationViewModel(IStoredSettingsService storedSettingsService, IServerApiService serverApiService)
        {
            this.storedSettingsService = storedSettingsService;
            this.serverApiService = serverApiService;

            backToLoginViewCommand = new MvxCommand(() => { ShowViewModel<LoginViewModel>(); });
            registrationCommand = new MvxCommand(() => ServerCommandWrapperAsync(() => RegistrationAsync()));
            validateCommand = new MvxCommand(() => ComparePhoneCodeAsync());
        }

        public async void Init(string name)
        {
            if (SettingsService.SocialRegistartionSource)
            {
                profile = storedSettingsService.Profile != null ? storedSettingsService.Profile : new Profile();
                IsSocialRegistrationHidden = false;
            }
            else
                profile = new Profile();

            if (name != null)
                await Mvx.Resolve<IUserInteraction>().AlertAsync("Please continue registration.", "Hello, " + name);
        }

        public override void Start()
        {
            base.Start();
        }

        protected override void SetValidators()
        {
            Validator.AddRequiredRule(() => Profile.Phone, "Field can't' be empty.");
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

        Profile profile;
        public Profile Profile
        {
            get { return profile; }
            set { SetProperty(ref profile, value); }
        }

        string smsCode;
        public string SmsCode
        {
            get { return smsCode; }
            set { SetProperty(ref smsCode, value); }
        }

        string confirmPassword;
        public string ConfirmPassword
        {
            get { return confirmPassword; }
            set { SetProperty(ref confirmPassword, value); }
        }

        public string ViewInformation
        {
            get { return !IsSocialRegistrationHidden ? "Please, Enter Your Phone Number." : "Please, Fill Next Fields."; }
        }

        bool isSocialRegistrationHidden = true;
        public bool IsSocialRegistrationHidden
        {
            get { return isSocialRegistrationHidden; }
            set
            {
                SetProperty(ref isSocialRegistrationHidden, value);
                RaisePropertyChanged(() => IsSocialRegistrationHidden);
            }
        }

        async Task ComparePhoneCodeAsync()
        {
            //if (!Validate())
            //{
            //    Mvx.Resolve<IProgressDialogManager>().ShowMessage("Error", "Field can't' be empty.");
            //    return;
            //}

            //if (SmsCode == profileData.PhoneCode)

            if (SmsCode == "1111")
            {
                profile.PhoneValidate = true;
                var result = await serverApiService.SaveProfileAsync(profile.Id, profile, profile.Token);
                if (CheckHttpStatuseCode(result.StatusCode))
                {
                    storedSettingsService.Profile = profile;
                    storedSettingsService.IsAuthorized = true;
                    storedSettingsService.ProfileId = profile.Id;
                    storedSettingsService.AuthToken = profile.Token;
                    Close(this);
                    ShowViewModel<HomeViewModel>(new { name = result.FirstName, message = "Welcome, " });
                }
            }
            else
                await Mvx.Resolve<IUserInteraction>().AlertAsync("Wrong SMS code! Please try again.", "Error");
        }

        async Task RegistrationAsync()
        {
            //if (!Validate())
            //{
            //    Mvx.Resolve<IProgressDialogManager>().ShowMessage("Error", "Field can't' be empty.");
            //    return;
            //}

            if (IsSocialRegistrationHidden)
            {
                if (!CheckFields())
                    return;
            }

            if (CheckField("Phone number", Profile.Phone))
            {
                var result = await serverApiService.SignUpAsync(profile);
                if (CheckHttpStatuseCode(result.StatusCode))
                {
                    SmsCode = "1111";
                    var inputResult = await Mvx.Resolve<IUserInteraction>().InputAsync("Please input code from SMS", "XXXX", null, "Validate", null, SmsCode);
                    SmsCode = inputResult.Text;
                    profile = result;
                }
            }
        }

        bool CheckFields()
        {
            if (string.IsNullOrEmpty(ConfirmPassword) && string.IsNullOrEmpty(Profile.Phone) && string.IsNullOrEmpty(Profile.Password))
            {
                Mvx.Resolve<IUserInteraction>().Alert("Please fill out the required fields.", null, "Error");
                return false;
            }
            if (!CheckField("Password", Profile.Password) || !CheckField("ConfirmPassword", ConfirmPassword))
                return false;
            if (confirmPassword != profile.Password)
            {
                Mvx.Resolve<IUserInteraction>().Alert("Passwords not equal. Please try again.", null, "Error");
                return false;
            }
            return true;
        }

        bool CheckField(string fieldName, string fieldValue)
        {
            if (string.IsNullOrEmpty(fieldValue))
            {
                Mvx.Resolve<IUserInteraction>().Alert(fieldName + " number can't be null.", null, "Error");
                return false;
            }
            return true;
        }

        bool CheckHttpStatuseCode(System.Net.HttpStatusCode statusCode)
        {
            switch (statusCode)
            {
                case System.Net.HttpStatusCode.Found:
                    Mvx.Resolve<IUserInteraction>().Alert("User with this phone number already registrated. Please, enter diferent number and try again.", null, "Error");
                    break;
                case System.Net.HttpStatusCode.OK:
                    return true;
                case System.Net.HttpStatusCode.NotFound:
                case System.Net.HttpStatusCode.BadRequest:
                case System.Net.HttpStatusCode.Unauthorized:
                    Mvx.Resolve<IUserInteraction>().Alert("Server error!", null, "Error");
                    return false;
            }
            return false;
        }
    }
}