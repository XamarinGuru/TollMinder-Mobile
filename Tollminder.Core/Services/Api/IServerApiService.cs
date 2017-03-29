using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Tollminder.Core.Models;
using System.Threading;
using Tollminder.Core.Models.DriverData;
using Tollminder.Core.Models.PaymentData;

namespace Tollminder.Core.Services.Api
{
    public interface IServerApiService
    {
        Task<IList<TollRoad>> RefreshTollRoadsAsync(long lastSyncDateTime, CancellationToken token);

        // Authorization processing
        Task<Profile> SignInAsync(string phone, string password);
        Task<Profile> SignUpAsync(Profile profile);
        Task<string> GetValidAuthorizeTokenAsync();

        /// <summary>
        /// Sign in using social networks.
        /// </summary>
        /// <returns>Profile</returns>
        /// <param name="email">Email - user email from social network.</param>
        /// <param name="source">Source - what kind of social network user choose.</param>
        Task<Profile> GooglePlusSignInAsync(string email, string source);
        Task<Profile> FacebookSignInAsync(string facebookId, string source);

        // Profile processing
        Task<Profile> SaveProfileAsync(string userId, Profile profile, string authToken);
        Task<Profile> GetProfileAsync(string userId, string authToken = null);

        // Payhistory processing
        Task<List<PayHistory>> GetPayHistoryAsync(DateTime dateFrom, DateTime dateTo);
        Task<string> DownloadPayHistoryAsync(DateTime dateFrom, DateTime dateTo);

        // Pay processings
        Task<TripResponse> TripCompletedAsync(TripCompleted tripRequest);
        Task<CreditCardAuthorizeDotNet> AddCreditCardAsync(AddCreditCard crediCard);
        Task<List<CreditCardAuthorizeDotNet>> GetCreditCardsAsync();
        Task<string> PayForTripAsync(PayForTrip tripRequest);
        Task RemoveCreditCardAsync(string userId, string paymentProfileId);

        // Vehicle processing
        Task<Vehicle> SaveVehicleAsync(Vehicle vehicle);
        Task<Vehicle> GetVehiclesAsync();
    }
}