using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Tollminder.Core.Models;
using System.Threading;
using Tollminder.Core.Models.DriverData;
using Tollminder.Core.Models.PaymentData;

namespace Tollminder.Core.Services
{
    public interface IServerApiService
    {
        Task<IList<TollRoad>> RefreshTollRoads(long lastSyncDateTime, CancellationToken token);
        // Authorization processing
        Task<Profile> SignIn(string phone, string password);
        Task<Profile> SignUp(Profile profile);
        Task<string> GetValidAuthorizeToken();

        /// <summary>
        /// Sign in using social networks.
        /// </summary>
        /// <returns>Profile</returns>
        /// <param name="email">Email - user email from social network.</param>
        /// <param name="source">Source - what kind of social network user choose.</param>
        Task<Profile> GooglePlusSignIn(string email, string source);
        Task<Profile> FacebookSignIn(string facebookId, string source);

        // Profile processing
        Task<Profile> SaveProfile(string userId, Profile profile, string authToken);
        Task<Profile> GetProfile(string userId, string authToken = null);

        // Payhistory processing
        Task<List<PayHistory>> GetPayHistory(DateTime dateFrom, DateTime dateTo);
        Task<string> DownloadPayHistory(DateTime dateFrom, DateTime dateTo);

        // Vehicle processing
        Task<Vehicle> SaveVehicle(Vehicle vehicle);
        Task<Vehicle> GetVehicles();
    }
}