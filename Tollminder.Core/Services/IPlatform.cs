using System;

namespace Tollminder.Core.Services
{
    public interface IPlatform
    {
		bool IsAppInForeground { get; }
		bool IsMusicRunning { get; }

		void PauseMusic();
		void PlayMusic();

        void SetAudioEnabled(bool flag);
    }
}

