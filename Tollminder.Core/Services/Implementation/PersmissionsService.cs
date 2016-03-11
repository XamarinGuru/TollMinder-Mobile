using System;
using System.Threading.Tasks;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Tollminder.Core.Helpers;

namespace Tollminder.Core.Services.Implementation
{
	public class PersmissionsService : IPermissionsService
	{
		public async Task<bool> CheckPermissionsAccesGrantedAsync()
		{
			try {
				var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
				if (status != PermissionStatus.Granted)
				{
					var results = await CrossPermissions.Current.RequestPermissionsAsync (new[] { Permission.Location });
					status = results[Permission.Location];
				}
				
				if (status == PermissionStatus.Granted)
				{
					return true;
				}
				else if(status != PermissionStatus.Unknown)
				{
					return false;
				}				
			} catch (Exception ex) {
				Log.LogMessage (ex.Message);
			}
			return false;
		}
	}
}

