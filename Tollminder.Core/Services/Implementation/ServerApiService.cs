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

        public Task<User> SignIn(string phone, string password)
        {
            var parameters = new Dictionary<string, object>
            {
                ["phone"] = phone,
                ["password"] = password
            };

            var user = SendAsync<Dictionary<string, object>, User>(parameters, $"{BaseApiUrl}user/signin");
            return user;
        }

        public Task<IList<PayHistory>> GetPayHistory(string userId, DateTime dateFrom, DateTime dateTo)
        {
            var parameters = new
            {
                _user = userId,
                from = dateFrom.ToString("yyyy-MM-dd HH':'mm':'ss"),
                to = dateTo.ToString("yyyy-MM-dd HH':'mm':'ss")
            };

            return SendAsync<object, IList<PayHistory>>(parameters, $"{BaseApiUrl}/trip/paymentHistory");
        }
        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1);

        public static long ToUnixTime(DateTime dateTime)
        {
            return (dateTime - UnixEpoch).Ticks / TimeSpan.TicksPerMillisecond;
        }

        public Task<User> GetUser(string id)
        {
            var user = GetAsync<User>($"{BaseApiUrl}user/{id}");
            return user;
        }
	}
}

