using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using MvvmCross.Platform;
using Tollminder.Core.Models;
using Tollminder.Core.Models.DriverData;
using Tollminder.Core.Models.PaymentData;

namespace Tollminder.Core.Services.Implementation
{
    public class ServerApiService : HttpClientService, IServerApiService
    {
        const string BaseApiUrl = "https://tollminder.com/api/";
        private string authToken = "LM9NJSUN3GDQU8BFPPCUPpCRtLnd89NZXLSUUR9DBjjSR32EBQxCbHX963ycqcjv";
        readonly IStoredSettingsService storedSettingsService;

        public ServerApiService(IStoredSettingsService storedSettingsService)
        {
            this.storedSettingsService = storedSettingsService;
        }

        public Task<IList<TollRoad>> RefreshTollRoadsAsync(long lastSyncDateTime, CancellationToken token)
        {
            Task<IList<TollRoad>> result = null;
            try
            {
                result = GetAsync<IList<TollRoad>>($"{BaseApiUrl}sync/{0}", token, storedSettingsService.AuthToken);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return result;
        }

        public Task<Profile> SignInAsync(string phone, string password)
        {
            var parameters = new Dictionary<string, object>
            {
                ["phone"] = phone,
                ["password"] = password
            };

            return SendAsync<Dictionary<string, object>, Profile>(parameters, $"{BaseApiUrl}user/signin");
        }

        /// <summary>
        /// Sign in using social networks.
        /// </summary>
        /// <returns>Profile</returns>
        /// <param name="email">Email - user email from social network.</param>
        /// <param name="source">Source - what kind of social network user choose.</param>
        public Task<Profile> FacebookSignInAsync(string facebookId, string source)
        {
            var parameters = new Dictionary<string, object>
            {
                ["facebookId"] = facebookId,
                ["source"] = source
            };

            return CheckProfileAsync<Dictionary<string, object>>(parameters, $"{BaseApiUrl}user/oauth");
        }

        public Task<Profile> SignUpAsync(Profile profile)
        {
            return CheckProfileAsync<Profile>(profile, $"{BaseApiUrl}user/signup");
        }

        public Task<Profile> GooglePlusSignInAsync(string email, string source)
        {
            var parameters = new Dictionary<string, object>
            {
                ["email"] = email,
                ["source"] = source
            };

            return CheckProfileAsync<Dictionary<string, object>>(parameters, $"{BaseApiUrl}user/oauth");
        }

        public Task<string> DownloadPayHistoryAsync(DateTime dateFrom, DateTime dateTo)
        {
            var parameters = new
            {
                user = storedSettingsService.ProfileId,
                range = new
                {
                    from = dateFrom.ToString("O"),
                    to = dateTo.ToString("O")
                }
            };

            return SendAsync<object, string>(parameters, $"{BaseApiUrl}file/paymentHistoryPdf");
        }

        public Task<List<PayHistory>> GetPayHistoryAsync(DateTime dateFrom, DateTime dateTo)
        {
            var parameters = new
            {
                user = storedSettingsService.ProfileId,
                from = dateFrom.ToString("O"),
                to = dateTo.ToString("O")
            };

            return SendAsync<object, List<PayHistory>>(parameters, $"{BaseApiUrl}trip/paymentHistory");
        }

        public Task<Profile> GetProfileAsync(string userId, string authToken = null)
        {
            return GetAsync<Profile>($"{BaseApiUrl}user/{userId}", authToken);
        }

        public Task<Profile> SaveProfileAsync(string userId, Profile profile, string authToken)
        {
            return SendAsync<Profile>(profile, $"{BaseApiUrl}user/{userId}", new CancellationTokenSource(), authToken);
        }

        public Task<string> GetValidAuthorizeTokenAsync()
        {
            return GetAsync<string>($"{BaseApiUrl}user/{storedSettingsService.ProfileId}/token", storedSettingsService.AuthToken);
        }

        public Task<Vehicle> SaveVehicleAsync(Vehicle vehicle)
        {
            throw new NotImplementedException();
        }

        public Task<Vehicle> GetVehiclesAsync()
        {
            throw new NotImplementedException();
        }
    }
}