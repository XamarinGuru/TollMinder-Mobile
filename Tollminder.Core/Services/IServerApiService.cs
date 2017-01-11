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
        Task<User> SignIn(string phone, string password);
        Task<User> GetUser(string id);
        Task<List<PayHistory>> GetPayHistory(string userId, DateTime dateFrom, DateTime dateTo);
        Task<string> DownloadPayHistory(string userId, DateTime dateFrom, DateTime dateTo);
	}
}