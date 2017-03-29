using System;
using MvvmCross.Core.ViewModels;
using Tollminder.Core.Services.Api;
using Tollminder.Core.Services.Settings;
using Cirrious.CrossCore;
using Chance.MvvmCross.Plugins.UserInteraction;

namespace Tollminder.Core.ViewModels.Payments
{
    public class CreditCardsViewModel : BaseViewModel
    {
        readonly IServerApiService serverApiService;
        readonly IStoredSettingsService storedSettingsService;

        public MvxObservableCollection<CreditCardAuthorizeDotNetViewModel> CrediCards { get; set; }
        public MvxCommand CloseCreditCardsCommand { get; set; }

        public CreditCardsViewModel(IServerApiService serverApiService, IStoredSettingsService storedSettingsService)
        {
            this.serverApiService = serverApiService;
            this.storedSettingsService = storedSettingsService;

            CloseCreditCardsCommand = new MvxCommand(() => Close(this));
        }

        public async override void Start()
        {
            base.Start();

            try
            {
                var getCreditCard = await serverApiService.NewsAsync();
                CrediCards.AddRange(getCreditCard.Select(cards => new CreditCardAuthorizeDotNetViewModel(cards, serverApiService, storedSettingsService)));
            }
            catch (Exception ex)
            {
                await Mvx.Resolve<IUserInteraction>().AlertAsync(ex.Message, "Error");
            }
        }
    }
}
