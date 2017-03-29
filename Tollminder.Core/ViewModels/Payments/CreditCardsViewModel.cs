using System;
using MvvmCross.Core.ViewModels;
using Tollminder.Core.Services.Api;
using Tollminder.Core.Services.Settings;
using System.Linq;
using Tollminder.Core.ViewModels.UserProfile;
using System.Diagnostics;

namespace Tollminder.Core.ViewModels.Payments
{
    public class CreditCardsViewModel : BaseViewModel
    {
        readonly IServerApiService serverApiService;
        readonly IStoredSettingsService storedSettingsService;

        public MvxObservableCollection<CreditCardAuthorizeDotNetViewModel> CrediCards { get; set; }
        public MvxCommand CloseCreditCardsCommand { get; set; }
        public MvxCommand AddCreditCardsCommand { get; set; }

        public CreditCardsViewModel(IServerApiService serverApiService, IStoredSettingsService storedSettingsService)
        {
            this.serverApiService = serverApiService;
            this.storedSettingsService = storedSettingsService;

            CloseCreditCardsCommand = new MvxCommand(() => ShowViewModel<ProfileViewModel>());
            AddCreditCardsCommand = new MvxCommand(() => ShowViewModel<AddCreditCardViewModel>());
        }

        public async override void Start()
        {
            base.Start();

            try
            {
                var getCreditCard = await serverApiService.GetCreditCardsAsync();
                CrediCards.AddRange(getCreditCard?.Select(cards => new CreditCardAuthorizeDotNetViewModel(cards, serverApiService, storedSettingsService)));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, ex.StackTrace);
            }
        }
    }
}
