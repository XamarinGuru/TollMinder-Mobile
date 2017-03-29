using System;
using System.Threading.Tasks;
using Tollminder.Core.Models.PaymentData;

namespace Tollminder.Core.Services.Api
{
    public interface IPaymentProcessing
    {
        Task TripCompletedAsync(TripCompleted tripRequest);
        Task AddCreditCardAsync(AddCreditCard crediCard);
        Task<string> PayForTripAsync(PayForTrip tripRequest);
    }
}
