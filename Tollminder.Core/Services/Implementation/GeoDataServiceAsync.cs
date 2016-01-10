using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MvvmCross.Plugins.Sqlite;
using SQLite.Net.Async;
using Tollminder.Core.Helpers;
using Tollminder.Core.Models;


namespace Tollminder.Core.Services.Implementation
{
	public class GeoDataServiceAsync : IGeoDataServiceAsync
	{
		private readonly SQLiteAsyncConnection connection;

		public GeoDataServiceAsync (IMvxSqliteConnectionFactory factory)
		{
			this.connection = factory.GetAsyncConnection("tollminder.db");
			connection.CreateTableAsync<GeoLocation> ();
		}
	
		#region IGeoDataService implementation

		public async Task<ParallelQuery<GeoLocation>> FindNearGeoLocationsAsync (GeoLocation center)
		{
			var lcations = await connection.Table<GeoLocation> ().ToListAsync ();
			var nearLocations = await center.GetLocationsFromRadiusAsync (lcations);
			return nearLocations;
		}

		public Task UpdateAsync (GeoLocation geoLocation)
		{
			return connection.UpdateAsync (geoLocation);
		}

		public Task InsertAsync (GeoLocation geoLocation)
		{
			return connection.InsertAsync (geoLocation);
		}

		public Task DeleteAsync (GeoLocation geoLocation)
		{
			return connection.DeleteAsync<GeoLocation> (geoLocation);
		}

		public Task<int> CountAsync {
			get {
				return connection.Table<GeoLocation> ().CountAsync ();
			}
		}

		#endregion
	}
}

