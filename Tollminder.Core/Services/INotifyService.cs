using System;
using System.Threading.Tasks;

namespace Tollminder.Core.Services
{
	public interface INotifyService
	{
		Task Notify (string message);
	}
}

