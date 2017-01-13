using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Tollminder.Core.Models;
using System.Threading;

namespace Tollminder.Core.Services
{
	public interface IServerApiService
	{
        Task<IList<TollRoad>> RefreshTollRoads (long lastSyncDateTime, CancellationToken token);
        Task<Profile> SignIn(string phone, string password);
        Task<Profile> GetProfile(string userId, string authToken = null);
        Task<List<PayHistory>> GetPayHistory(string userId, DateTime dateFrom, DateTime dateTo);
        Task<string> DownloadPayHistory(string userId, DateTime dateFrom, DateTime dateTo);
        Task<System.Net.HttpStatusCode> SaveProfile(string userId, Profile profile, string authToken);
        Task<string> GetValidAuthorizeToken(string userId, string authToken);
	}
}