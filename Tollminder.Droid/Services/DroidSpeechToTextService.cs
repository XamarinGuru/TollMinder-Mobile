using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Speech;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid.Platform;
using MvvmCross.Plugins.Messenger;
using Tollminder.Core.Helpers;
using Tollminder.Core.Models;
using Tollminder.Core.Services;
using Tollminder.Droid.Views;

namespace Tollminder.Droid.Services
{
    //TODO: Google Speech Recognition timeout http://stackoverflow.com/questions/38150312/google-speech-recognition-timeout
    public class DroidSpeechToTextService : Java.Lang.Object, ISpeechToTextService, IRecognitionListener
    {
        TaskCompletionSource<bool> _recognitionTask;

        SpeechRecognizer _speechRecognizer;

        Handler _handler;
        AlertDialog _dialog;
        bool _firstInit = true;
        bool _dialogWasManuallyAnswered;
        bool _isMusicRunning;

        IPlatform _platform;
        IPlatform Platform
        {
            get
            {
                return _platform ?? (_platform = Mvx.Resolve<IPlatform>());
            }
        }

        ITextFromSpeechMappingService _mappingService;
        ITextFromSpeechMappingService MappingService
        {
            get
            {
                return _mappingService ?? (_mappingService = Mvx.Resolve<ITextFromSpeechMappingService>());
            }
        }

        ITextToSpeechService _textToSpeechService;
        ITextToSpeechService TextToSpeechService
        {
            get
            {
                return _textToSpeechService ?? (_textToSpeechService = Mvx.Resolve<ITextToSpeechService>());
            }
        }

        IPlatform _platformService;
        IPlatform PlatformService
        {
            get
            {
                return _platformService ?? (_platformService = Mvx.Resolve<IPlatform>());
            }
        }

        string _question;
        public string Question
        {
            get
            {
                return _question;
            }
            set
            {
                _question = value;
            }
        }

        Intent _voiceIntent;
        public Intent VoiceIntent
        {
            get
            {
                if (_voiceIntent == null)
                {
                    _voiceIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
                    _voiceIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);
                    _voiceIntent.PutExtra(RecognizerIntent.ExtraCallingPackage, "com.tollminder");
                    _voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, new Java.Lang.Long(10000));
                    _voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, new Java.Lang.Long(10000));
                    _voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, new Java.Lang.Long(25000));
                    _voiceIntent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);

                    _voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, "en-US");
                }

                return _voiceIntent;
            }
        }

        public Task<bool> AskQuestion(string question)
        {
            _recognitionTask = new TaskCompletionSource<bool>();

            AskQuestionMethod(question);

            return _recognitionTask.Task;
        }

        void AskQuestionMethod(string question)
        {
            _isMusicRunning = Platform.IsMusicRunning;

            if (_isMusicRunning)
                Platform.PauseMusic();

            if (Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity == null)
            {
                var otherActivity = new Intent(Application.Context, typeof(HomeView));
                otherActivity.AddFlags(ActivityFlags.NewTask);
                Application.Context.StartActivity(otherActivity);

                EnsureActivityLoaded().Wait();
            }

            _handler = new Handler(Application.Context.MainLooper);

            TextToSpeechService.Speak(question, false).Wait();

            Question = question;

            StartSpeechRecognition();
        }

        void StartSpeechRecognition()
        {
            _handler.Post(() =>
           {
               ShowDialog();
               StartRecognizer();
           });
        }

        void StartRecognizer()
        {
            if (_speechRecognizer != null)
                StopRecognizer();
            
            _speechRecognizer = SpeechRecognizer.CreateSpeechRecognizer(Application.Context);

            PlatformService.SetAudioEnabled(_firstInit);

            if (_firstInit)
            {
                TextToSpeechService.Speak("Please, answer yes or no after the tone", false).Wait();
                _firstInit = false;
            }

            _speechRecognizer.SetRecognitionListener(this);
            _speechRecognizer.StartListening(VoiceIntent);
        }

        void StopRecognizer()
        {
            _speechRecognizer?.StopListening();
            _speechRecognizer?.Cancel();
            _speechRecognizer?.Destroy();
            _speechRecognizer = null;
        }

        void ShowDialog()
        {
            if (_platform.IsAppInForeground && _dialog == null)
            {
                try
                {
                    _dialogWasManuallyAnswered = false;
                    _dialog = new AlertDialog.Builder(Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity)
                            .SetTitle(Question)
                            .SetMessage("Please, answer yes or no after the tone")
                            .SetCancelable(false)
                            .SetPositiveButton("Yes", (sender, e) =>
                {
                    _dialogWasManuallyAnswered = true;
                    _speechRecognizer?.StopListening();
                    _recognitionTask.TrySetResult(true);
                })
                            .SetNegativeButton("No", (sender, e) =>
                {
                    _dialogWasManuallyAnswered = true;
                    _speechRecognizer?.StopListening();
                    _recognitionTask.TrySetResult(false);
                })
                            .Show();
                }
                catch (Exception e)
                {
                    Mvx.Trace(e.Message + e.StackTrace);
                }
            }
        }

        void DisposeDialog()
        {
            if (_dialog?.IsShowing ?? false)
                _dialog?.Cancel();
            _dialog = null;
        }

        Task<bool> EnsureActivityLoaded()
        {
            var _ensureTask = new TaskCompletionSource<bool>();
            Mvx.Resolve<IMvxMessenger>().SubscribeOnThreadPoolThread<SpechRecognitionActivityLoadedMessage>(x =>
            {
                Console.WriteLine("Received SpechRecognitionActivityLoadedMessage");
                _ensureTask.SetResult(true);
            });
            return _ensureTask.Task;
        }

        public void OnBeginningOfSpeech()
        {
            Console.WriteLine("OnBeginningOfSpeech");
        }

        public void OnBufferReceived(byte[] buffer)
        {
            Console.WriteLine("OnBufferReceived");
        }

        public void OnEndOfSpeech()
        {
            Console.WriteLine("OnEndOfSpeech");
        }

        public void OnError([GeneratedEnum] SpeechRecognizerError error)
        {
            Core.Helpers.Log.LogMessage("SpeechRecognizerError = " + error);

            if (!_dialogWasManuallyAnswered && (error == SpeechRecognizerError.NoMatch || error == SpeechRecognizerError.SpeechTimeout))
                StartSpeechRecognition();
        }

        public void OnEvent(int eventType, Bundle @params)
        {
            Console.WriteLine("OnEvent");
        }

        public void OnPartialResults(Bundle partialResults)
        {
            Console.WriteLine("OnPartialResults");
        }

        public void OnReadyForSpeech(Bundle @params)
        {
            Console.WriteLine("OnReadyForSpeech");
            PlatformService.SetAudioEnabled(true);
        }

        public void OnResults(Bundle results)
        {
            Console.WriteLine("OnResults");
            if (!_dialogWasManuallyAnswered)
            {
                var res = results.GetStringArrayList(SpeechRecognizer.ResultsRecognition);
                Log.LogMessage($"Speech recognition results = {string.Join(", ", res)}");
                var answer = MappingService.DetectAnswer(res);

                _handler.Post(() =>
                {
                    if (answer != AnswerType.Unknown)
                        DisposeDialog();

                    StopRecognizer();
                });

                _firstInit = true;

                if (answer != AnswerType.Unknown)
                {
                    TextToSpeechService.Speak($"Your answer is {answer.ToString()}", false).Wait();
                    if (_isMusicRunning)
                        Platform.PlayMusic();
                    _recognitionTask.TrySetResult(answer == AnswerType.Positive);
                }
                else
                {
                    AskQuestionMethod(Question);
                }
            }
        }

        public void OnRmsChanged(float rmsdB)
        {
            Console.WriteLine("OnRmsChanged");
        }
    }
}

