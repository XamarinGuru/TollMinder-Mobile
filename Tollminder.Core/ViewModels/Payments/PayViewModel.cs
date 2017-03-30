using MvvmCross.Core.ViewModels;
using Tollminder.Core.ViewModels.UserProfile;
using Tollminder.Core.Services.Api;
using Tollminder.Core.Models.PaymentData;
using System;
using System.Diagnostics;
using Chance.MvvmCross.Plugins.UserInteraction;
using MvvmCross.Platform;
using System.Linq;
using Tollminder.Core.Services.Settings;

namespace Tollminder.Core.ViewModels.Payments
{
    public class PayViewModel : BaseViewModel
    {
        readonly IPaymentProcessing paymentProcessing;

        public MvxCommand BackToMainPageCommand { get; set; }
        public MvxCommand PayCommand { get; set; }
        public MvxObservableCollection<IQueueItem> NotPayedTrips { get; set; }
        public string Amount { get; set; }

        public PayViewModel(IPaymentProcessing paymentProcessing)
        {
            this.paymentProcessing = paymentProcessing;

            BackToMainPageCommand = new MvxCommand(() => ShowViewModel<ProfileViewModel>());
            PayCommand = new MvxCommand(() => AddHeaderViewModel<CreditCardsForPayViewModel>());
        }

        private async void LoadDataAsync()
        {
            try
            {
                NotPayedTrips = new MvxObservableCollection<IQueueItem>();
                var getTrips = await paymentProcessing.GetNotPayedTripsAsync();
                NotPayedTrips.AddRange(getTrips?.Trips);
                Amount = getTrips?.Amount;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, ex.StackTrace);
                await Mvx.Resolve<IUserInteraction>().AlertAsync(ex.Message, "Error");
            }
        }

        public override void Start()
        {
            base.Start();
            LoadDataAsync();
        }

        private void AddHeaderViewModel<T>()
        {
            var firstView = NotPayedTrips.FirstOrDefault();
            var insertedAlready = (firstView?.Priority == ItemPriority.FirstAlways && firstView is T);

            if (insertedAlready)
                return;

            if (firstView?.Priority == ItemPriority.FirstAlways)
                this.NotPayedTrips.Remove(firstView);

            if (typeof(T) == typeof(CreditCardsForPayViewModel))
            {
                try
                {
                    var creditCardList = new CreditCardsForPayViewModel(paymentProcessing, () => CloseViewModel<CreditCardsForPayViewModel>());
                    NotPayedTrips.Insert(0, creditCardList);
                    creditCardList.Start();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message, ex.StackTrace);
                }
            }
        }

        private void CloseViewModel<T>()
        {
            var cardViewModel = this.NotPayedTrips.FirstOrDefault(item => item is T);
            if (cardViewModel != null)
                this.NotPayedTrips.Remove(cardViewModel);
            LoadDataAsync();
        }
    }
}
