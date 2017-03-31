﻿using System;
using MvvmCross.Core.ViewModels;
using Tollminder.Core.Services.Api;
using System.Linq;
using Tollminder.Core.ViewModels.UserProfile;
using System.Diagnostics;

namespace Tollminder.Core.ViewModels.Payments
{
    public class CreditCardsViewModel : BaseViewModel
    {
        readonly IPaymentProcessing paymentProcessing;

        public MvxObservableCollection<CreditCardAuthorizeDotNetViewModel> CrediCards { get; set; }
        public MvxCommand CloseCreditCardsCommand { get; set; }
        public MvxCommand AddCreditCardsCommand { get; set; }

        public CreditCardsViewModel(IPaymentProcessing paymentProcessing)
        {
            this.paymentProcessing = paymentProcessing;

            CloseCreditCardsCommand = new MvxCommand(() => ShowViewModel<ProfileViewModel>());
            AddCreditCardsCommand = new MvxCommand(() => ShowViewModel<AddCreditCardViewModel>());
            CrediCards = new MvxObservableCollection<CreditCardAuthorizeDotNetViewModel>();
        }

        protected async void LoadCreditCards()
        {
            try
            {
                var getCreditCard = await paymentProcessing.GetCreditCardsAsync();
                getCreditCard = new System.Collections.Generic.List<Models.PaymentData.CreditCardAuthorizeDotNet>();
                getCreditCard.Add(new Models.PaymentData.CreditCardAuthorizeDotNet()
                {
                    CustomerProfileId = "323",
                    PaymentProfile = new Models.PaymentData.PaymentProfile()
                    {
                        CardNumber = "5463",
                        PaymentProfileId = "3242234"
                    }
                });
                CrediCards.AddRange(getCreditCard?.Select(cards => new CreditCardAuthorizeDotNetViewModel(cards, paymentProcessing)));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, ex.StackTrace);
            }
        }

        public override void Start()
        {
            base.Start();
            LoadCreditCards();
        }
    }
}
