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
            return await AskForPermissionAsync(Permission.Location) &&
                   await AskForPermissionAsync(Permission.Microphone);
        }

        protected async Task<bool> AskForPermissionAsync(Permission permission)
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(permission);
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(permission))
                    {
                        // TODO: Ask for Permission
                    }
                    var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { permission });
                    status = results[permission];
                }
                if (status == PermissionStatus.Granted)
                {
                    return true;
                }
                else if (status != PermissionStatus.Unknown)
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Log.LogMessage(e.Message + e.StackTrace);
            }
            return false;
        }
    }
}

