using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AVFoundation;
using Foundation;
using MvvmCross.Platform;
using OpenEars;
using Tollminder.Core.Models;
using UIKit;
using Chance.MvvmCross.Plugins.UserInteraction;
using MvvmCross.Platform.Core;
using Tollminder.Core.Services.SpeechRecognition;

namespace Tollminder.Touch.Services.SpeechServices
{
    public class TouchSpeechToTextService : ISpeechToTextService
    {
        readonly ITextFromSpeechMappingService MappingService;
        readonly ITextToSpeechService TextToSpeechService;

        OEEventsObserver observer;
        OEPocketsphinxController pocketSphinxController;
        OEFliteController fliteController;
        AVAudioPlayer _audioPlayer;
        AnswerType _answer;
        Core.Utils.Timer _timer;
        bool isTimeStarted;
        int repeatTimeoutSeconts = 5;
        TaskCompletionSource<bool> _alertRecognitionTCS;
        TaskCompletionSource<bool> _voiceRecognitionTCS;
        CancellationTokenSource cancellationToken;

        UIAlertView _questionAlert;

        string pathToLanguageModel;
        string pathToDictionary;
        string pathToAcousticModel;
        string firstVoiceToUse;
        string secondVoiceToUse;


        public string Question { get; set; }

        readonly IUserInteraction userInteractionService;

        public TouchSpeechToTextService(IUserInteraction userInteractionService)
        {
            this.userInteractionService = userInteractionService;
            MappingService = Mvx.Resolve<ITextFromSpeechMappingService>();
            TextToSpeechService = Mvx.Resolve<ITextToSpeechService>();

            observer = new OEEventsObserver();
            observer.WeakDelegate = new MyOpenEarsEventsObserverDelegate(this);
            pocketSphinxController = new OEPocketsphinxController();
            fliteController = new OEFliteController();

            firstVoiceToUse = "cmu_us_slt";
            secondVoiceToUse = "cmu_us_rms";
            pathToLanguageModel = NSBundle.MainBundle.ResourcePath + System.IO.Path.DirectorySeparatorChar + "OpenEars1.languagemodel";
            pathToDictionary = NSBundle.MainBundle.ResourcePath + System.IO.Path.DirectorySeparatorChar + "OpenEars1.dic";
            pathToAcousticModel = NSBundle.MainBundle.ResourcePath + System.IO.Path.DirectorySeparatorChar + "AcousticModelEnglish.bundle";
        }

        public async Task<bool> AskQuestionAsync(string question)
        {
            bool result = false;
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    cancellationToken = new CancellationTokenSource(TimeSpan.FromSeconds(repeatTimeoutSeconts));
                    cancellationToken.CancelAfter(TimeSpan.FromSeconds(repeatTimeoutSeconts));
                    result = await AskOneTimeQuestionAsync(question);
                    break;
                }
                catch (TaskCanceledException ex)
                {
                    Debug.WriteLine("TaskCanceledException");
                    //cancellationToken.Dispose();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
            return result;

        }

        public async Task<bool> AskOneTimeQuestionAsync(string question)
        {
            var result = false;
            _alertRecognitionTCS = new TaskCompletionSource<bool>();
            try
            {
                UIApplication.SharedApplication.InvokeOnMainThread(() =>
                {
                    _questionAlert = new UIAlertView(question, "Please, answer after the signal", null, "NO", "Yes");
                    _questionAlert.Clicked += (sender, _buttonArgs) =>
                    {
                        try
                        {
                            _alertRecognitionTCS.TrySetResult(_buttonArgs.ButtonIndex != _questionAlert.CancelButtonIndex);
                            _questionAlert.Show();
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.Message);
                        }
                    };
                    _questionAlert.Show();
                });

                _voiceRecognitionTCS = new TaskCompletionSource<bool>();
                result = await await Task.WhenAny(new[] { _alertRecognitionTCS.Task, AskQuestionMethodAsync(question) });

                userInteractionService.DisposeIfDisposable();
            }
            catch (TaskCanceledException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                StopListening();
            }
            return result;
        }

        async Task<bool> AskQuestionMethodAsync(string question)
        {
            bool result = false;


            await TextToSpeechService.SpeakAsync(question, false).ConfigureAwait(false);
            await TextToSpeechService.SpeakAsync("Please, answer after the signal", false).ConfigureAwait(false);
            UIApplication.SharedApplication.InvokeOnMainThread(() =>
           {
               AVAudioSession.SharedInstance().SetCategory(AVAudioSessionCategory.Playback);
               AVAudioSession.SharedInstance().SetActive(true);

               _audioPlayer = AVAudioPlayer.FromUrl(NSUrl.FromFilename(Path.Combine("Sounds", "tap.aif")));
               _audioPlayer.PrepareToPlay();
               _audioPlayer.Play();
           });
            StartListening();
            var intRes = Task.WaitAny(new[] { _voiceRecognitionTCS.Task }, TimeSpan.FromSeconds(10));
            if (intRes == -1)
                throw new TaskCanceledException();
            result = await _voiceRecognitionTCS.Task;

            return result;
        }

        public async void CheckResult(IList<string> matches)
        {
            _answer = MappingService.DetectAnswer(matches);

            if (_answer != AnswerType.Unknown)
            {
                await TextToSpeechService.SpeakAsync($"Your answer is {_answer}", false).ConfigureAwait(false);
                _voiceRecognitionTCS.TrySetResult(_answer == AnswerType.Positive);
            }
            else
                _voiceRecognitionTCS.TrySetCanceled();
        }

        public void StartListening()
        {
            pocketSphinxController.StartListeningWithLanguageModelAtPathdictionaryAtPathlanguageModelIsJSGF(
                pathToLanguageModel,
                pathToDictionary,
                pathToAcousticModel,
                false
            );
        }

        public void StopListening()
        {
            pocketSphinxController.StopListening();
            UIApplication.SharedApplication.InvokeOnMainThread(() =>
            {
                _questionAlert?.DismissWithClickedButtonIndex(0, true);
            });
        }

        public void SuspendRecognition()
        {
            pocketSphinxController.SuspendRecognition();
        }

        public void ResumeRecognition()
        {
            pocketSphinxController.ResumeRecognition();
        }
    }
}

