using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tollminder.Core.Models.PaymentData;

namespace Tollminder.Core.Services.Api
{
    public interface IPaymentProcessing
    {
        Task TripCompletedAsync(TripCompleted tripRequest);
        Task<NotPayedTrip> GetNotPayedTripsAsync();
        Task<string> PayForTripAsync(PayForTrip tripRequest);

        Task<List<PayHistory>> GetPayHistoryAsync(DateTime dateFrom, DateTime dateTo);
        Task<string> DownloadPayHistoryAsync(DateTime dateFrom, DateTime dateTo);

        Task AddCreditCardAsync(AddCreditCard crediCard);
        Task<List<CreditCardAuthorizeDotNet>> GetCreditCardsAsync();
        Task RemoveCreditCardAsync(string paymentProfileId);
    }
}
