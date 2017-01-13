using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using MvvmCross.Platform;
using Tollminder.Core.Helpers;
using Tollminder.Core.Models;

namespace Tollminder.Core.Services.Implementation
{
    public class ServerApiService : HttpClientService, IServerApiService 
	{   
        const string BaseApiUrl = "http://54.152.103.212/api/";
        private string authToken = Mvx.Resolve<IStoredSettingsService>().AuthToken;

        public Task<IList<TollRoad>> RefreshTollRoads (long lastSyncDateTime, CancellationToken token)
		{
            Task<IList<TollRoad>> result = null;
            try
            {
                result =  GetAsync<IList<TollRoad>>($"{BaseApiUrl}sync/{0}", token, "LM9NJSUN3GDQU8BFPPCUPpCRtLnd89NZXLSUUR9DBjjSR32EBQxCbHX963ycqcjv");
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return result;
		}

        public Task<Profile> SignIn(string phone, string password)
        {
            var parameters = new Dictionary<string, object>
            {
                ["phone"] = phone,
                ["password"] = password
            };

            return SendAsync<Dictionary<string, object>, Profile>(parameters, $"{BaseApiUrl}user/signin");
        }

        public Task<string> DownloadPayHistory(string userId, DateTime dateFrom, DateTime dateTo)
        {
            var parameters = new
            {
                user = userId,
                range = new
                {
                    from = dateFrom.ToString("O"),
                    to = dateTo.ToString("O")
                }
            };

            return SendAsync<object, string>(parameters, $"{BaseApiUrl}file/paymentHistoryPdf");
        }

        public Task<List<PayHistory>> GetPayHistory(string userId, DateTime dateFrom, DateTime dateTo)
        {
            var parameters = new
            {
                user = userId,
                from = dateFrom.ToString("O"),
                to = dateTo.ToString("O")
            };

            return SendAsync<object, List<PayHistory>>(parameters, $"{BaseApiUrl}trip/paymentHistory");
        }

        public Task<Profile> GetProfile(string userId, string authToken = null)
        {
            var user = GetAsync<Profile>($"{BaseApiUrl}user/{userId}", authToken);
            return user;
        }

        public Task<System.Net.HttpStatusCode> SaveProfile(string userId, Profile profile, string authToken)
        {
            return SendAsync<Profile>(profile, $"{BaseApiUrl}user/{userId}", new CancellationTokenSource(), authToken);
        }
    }
}