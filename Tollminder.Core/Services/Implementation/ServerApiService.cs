using System;
using System.Collections.Generic;
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

        public Task<IList<TollRoad>> RefreshTollRoads (long lastSyncDateTime, CancellationToken token)
		{
            var authToken = Mvx.Resolve<IStoredSettingsService>().AuthToken;
            return GetAsync<IList<TollRoad>>($"{BaseApiUrl}sync/{lastSyncDateTime}", token, authToken);
		}

        public Task<User> SignIn(string phone, string password)
        {
            var parameters = new Dictionary<string, object>
            {
                ["phone"] = phone,
                ["password"] = password
            };

            return SendAsync<Dictionary<string, object>, User>(parameters, $"{BaseApiUrl}user/signin");
        }
	}
}

