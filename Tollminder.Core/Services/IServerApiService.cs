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
        Task<Profile> SignUp(Profile profile);
        /// <summary>
        /// Sign in using social networks.
        /// </summary>
        /// <returns>Profile</returns>
        /// <param name="email">Email - user email from social network.</param>
        /// <param name="source">Source - what kind of social network user choose.</param>
        Task<Profile> GooglePlusSignIn(string email, string source);
        Task<Profile> FacebookSignIn(string facebookId, string source);
        Task<Profile> GetProfile(string userId, string authToken = null);
        Task<List<PayHistory>> GetPayHistory(string userId, DateTime dateFrom, DateTime dateTo);
        Task<string> DownloadPayHistory(string userId, DateTime dateFrom, DateTime dateTo);
        Task<System.Net.HttpStatusCode> SaveProfile(string userId, Profile profile, string authToken);
        Task<string> GetValidAuthorizeToken(string userId, string authToken);
	}
}