﻿using System;
using System.Threading.Tasks;

namespace Tollminder.Core.Services
{
    public interface ITextToSpeechService
    {
        Task Speak(string text, bool disableMusic = true);
		bool IsEnabled { get; set; }
    }
}