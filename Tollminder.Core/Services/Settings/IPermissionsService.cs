using System;
using System.Threading.Tasks;

namespace Tollminder.Core.Services.Settings
{
    public interface IPermissionsService
    {
        Task<bool> CheckPermissionsAccesGrantedAsync();
    }
}

