﻿using AVFoundation;
using System.Threading.Tasks;
using Tollminder.Core.Services.SpeechRecognition;
using System;
using System.Diagnostics;

namespace Tollminder.Touch.Services
{
    public class TouchTextToSpeechService : ITextToSpeechService
    {
        private readonly AVSpeechSynthesizer _speechSynthesizer;

        public bool IsEnabled { get; set; }

        TaskCompletionSource<bool> _speakTask;

        public TouchTextToSpeechService()
        {
            _speechSynthesizer = new AVSpeechSynthesizer();
        }

        #region ITextToSpeechService implementation

        public Task<bool> SpeakAsync(string text, bool disableMusic = false)
        {
            _speakTask = new TaskCompletionSource<bool>();
            if (IsEnabled)
            {
                var speechUtterance = new AVSpeechUtterance(text)
                {
                    Rate = AVSpeechUtterance.MaximumSpeechRate / 2,
                    Voice = AVSpeechSynthesisVoice.FromLanguage("en-US"),
                    Volume = 1.0f,
                    PitchMultiplier = 1.0f
                    //PreUtteranceDelay = 0.1
                };

                _speechSynthesizer.DidFinishSpeechUtterance += DidFinishSpeechUtterance;

                if (_speechSynthesizer.Speaking)
                {
                    _speechSynthesizer.StopSpeaking(AVSpeechBoundary.Immediate);
                }

                _speechSynthesizer.SpeakUtterance(speechUtterance);
            }
            return _speakTask.Task;
        }

        private void DidFinishSpeechUtterance(object sender, AVSpeechSynthesizerUteranceEventArgs e)
        {
            try
            {
                _speakTask.SetResult(true);
                _speechSynthesizer.DidFinishSpeechUtterance -= DidFinishSpeechUtterance;
            }
            catch (InvalidOperationException ex)
            {
                Debug.WriteLine(ex.Message, ex.StackTrace);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, ex.StackTrace);
            }
        }

        #endregion
    }
}

