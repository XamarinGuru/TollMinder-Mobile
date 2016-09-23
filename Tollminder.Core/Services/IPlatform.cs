using System;

namespace Tollminder.Core.Services
{
    public interface IPlatform
    {
		bool IsAppInForeground { get; }

		void PauseMusic();
		void PlayMusic();
    }
}

