using System;
namespace Tollminder.Core.Services
{
    public interface ICheckerAppFirstLaunch
    {
        bool IsAppAlreadyLaunchedOnce();
    }
}
