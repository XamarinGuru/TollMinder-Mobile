﻿using System;
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
			connection.CreateTableAsync<TollRoadWaypoint> ();
		}
	
		#region IGeoDataService implementation

		public async Task<ParallelQuery<TollRoadWaypoint>> FindNearGeoLocationsAsync (GeoLocation center)
		{
			var lcations = await connection.Table<TollRoadWaypoint> ().ToListAsync ();
			var nearLocations = await center.GetLocationsFromRadiusAsync (lcations);
			return nearLocations;
		}

		public async Task<TollRoadWaypoint> FindNearGeoLocationAsync (GeoLocation center)
		{
			var lcations = await connection.Table<TollRoadWaypoint> ().ToListAsync ();
			var nearLocations = await center.GetLocationFromRadiusAsync (lcations);
			return nearLocations;
		}

		public Task UpdateAsync (TollRoadWaypoint geoLocation)
		{
			return connection.UpdateAsync (geoLocation);
		}

		public Task InsertAsync (TollRoadWaypoint geoLocation)
		{
			return connection.InsertAsync (geoLocation);
		}

		public Task DeleteAsync (TollRoadWaypoint geoLocation)
		{
			return connection.DeleteAsync<TollRoadWaypoint> (geoLocation);
		}

		public Task<int> CountAsync {
			get {
				return connection.Table<TollRoadWaypoint> ().CountAsync ();
			}
		}

		#endregion
	}
}

