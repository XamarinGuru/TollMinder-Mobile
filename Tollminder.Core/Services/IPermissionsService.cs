using System;
using System.Threading.Tasks;

namespace Tollminder.Core.Services
{
	public interface IPermissionsService
	{
		Task<bool> CheckPermissionsAccesGrantedAsync();
	}
}

