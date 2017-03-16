using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Speech;
using Chance.MvvmCross.Plugins.UserInteraction;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid.Platform;
using Tollminder.Core.Helpers;
using Tollminder.Core.Models;
using Tollminder.Core.Services;
using Tollminder.Core.Utils;
using MvvmCross.Platform.Core;

namespace Tollminder.Droid.Services
{
    //TODO: Google Speech Recognition timeout http://stackoverflow.com/questions/38150312/google-speech-recognition-timeout
    public class DroidSpeechToTextService : Java.Lang.Object, ISpeechToTextService, IRecognitionListener
    {
        readonly IPlatform platformService;
        readonly ITextFromSpeechMappingService mappingService;
        readonly ITextToSpeechService textToSpeechService;

        CancellationTokenSource cancellationToken;
        TaskCompletionSource<bool> _alertRecognitionTCS;
        TaskCompletionSource<bool> _voiceRecognitionTCS;

        SpeechRecognizer _speechRecognizer;
        Handler _handler;
        AlertDialog _dialog;
        bool _firstInit = true;
        bool _dialogWasManuallyAnswered;
        bool _isMusicRunning;
        int repeatTimeoutSeconts = 10;

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

        public DroidSpeechToTextService()
        {
            platformService = Mvx.Resolve<IPlatform>();
            mappingService = Mvx.Resolve<ITextFromSpeechMappingService>();
            textToSpeechService = Mvx.Resolve<ITextToSpeechService>();
        }

        public async Task<bool> AskQuestion(string question)
        {
            bool result = false;
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    cancellationToken = new CancellationTokenSource(TimeSpan.FromSeconds(repeatTimeoutSeconts));
                    cancellationToken.CancelAfter(TimeSpan.FromSeconds(repeatTimeoutSeconts));
                    result = await AskOneTimeQuestion(question);
                    break;
                }
                catch (TaskCanceledException ex)
                {
                    System.Diagnostics.Debug.WriteLine("TaskCanceledException");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
            return result;
        }

        public async Task<bool> AskOneTimeQuestion(string question)
        {
            var result = false;
            _alertRecognitionTCS = new TaskCompletionSource<bool>();
            try
            {
                ShowDialog();

                _voiceRecognitionTCS = new TaskCompletionSource<bool>();
                result = await await Task.WhenAny(new[] { _alertRecognitionTCS.Task, AskQuestionMethod(question) });
            }
            catch (TaskCanceledException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                StopRecognizer();
            }
            return result;
        }

        async Task<bool> AskQuestionMethod(string question)
        {
            bool result = false;
            if (_speechRecognizer != null)
            {
                //StopRecognizer();
                CancelDialog();
            }

            _isMusicRunning = platformService.IsMusicRunning;

            if (_isMusicRunning)
                platformService.PauseMusic();

            _handler = new Handler(Application.Context.MainLooper);

            await textToSpeechService.Speak(question, false);
            await textToSpeechService.Speak("Please, answer yes or no after the tone", false);

            Question = question;
            StartSpeechRecognition();

            var intRes = Task.WaitAny(new[] { _voiceRecognitionTCS.Task }, TimeSpan.FromSeconds(repeatTimeoutSeconts));
            if (intRes == -1)
                throw new TaskCanceledException();
            result = await _voiceRecognitionTCS.Task;

            return result;
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
            _speechRecognizer = SpeechRecognizer.CreateSpeechRecognizer(Application.Context);

            platformService.SetAudioEnabled(_firstInit);

            _speechRecognizer.SetRecognitionListener(this);
            _speechRecognizer.StartListening(VoiceIntent);
        }

        void StopRecognizer()
        {
            Mvx.Resolve<IMvxMainThreadDispatcher>().RequestMainThreadAction(() =>
            {
                _speechRecognizer?.StopListening();
                _speechRecognizer?.Cancel();
                _speechRecognizer?.Destroy();
                _speechRecognizer = null;
                CancelDialog();
            });
        }

        //private void TimerManager(string question)
        //{
        //    StopRecognizer();
        //    if (!isTimeStarted)
        //    {
        //        isTimeStarted = true;
        //        _timer = new Core.Utils.Timer((s) => { AskQuestionMethod(question); }, question, new TimeSpan(0, 0, 0), new TimeSpan(0, 0, 15), true);
        //    }
        //    else
        //    {
        //        _timer.Cancel();
        //        isTimeStarted = false;
        //    }
        //}

        void ShowDialog()
        {
            if (platformService.IsAppInForeground && _dialog == null)
            {
                try
                {
                    _dialogWasManuallyAnswered = false;
                    _dialog = new AlertDialog.Builder(Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity)
                            .SetTitle(Question)
                            .SetMessage("Please, answer yes or no after the tone")
                            .SetCancelable(false)
                            .SetPositiveButton("Yes", (sender, e) => SetDialogAnswer(true))
                            .SetNegativeButton("No", (sender, e) => SetDialogAnswer(false))
                            .Show();
                }
                catch (Exception e)
                {
                    Mvx.Trace(e.Message + e.StackTrace);
                }
            }
        }

        public void OnResults(Bundle results)
        {
            Console.WriteLine("OnResults");
            if (!_dialogWasManuallyAnswered)
            {
                var res = results.GetStringArrayList(SpeechRecognizer.ResultsRecognition);
                Log.LogMessage($"Speech recognition results = {string.Join(", ", res)}");
                var answer = mappingService.DetectAnswer(res);

                //_handler.Post(() =>
                //{
                //    if (answer != AnswerType.Unknown)
                //        CancelDialog();

                //    StopRecognizer();
                //});

                //_firstInit = true;

                if (answer != AnswerType.Unknown)
                {
                    _voiceRecognitionTCS.TrySetResult(answer == AnswerType.Positive);
                    textToSpeechService.Speak($"Your answer is {answer.ToString()}", false).Wait();
                    if (_isMusicRunning)
                        platformService.PlayMusic();
                }
                else
                    _voiceRecognitionTCS.TrySetCanceled();
            }
        }

        void SetDialogAnswer(bool result)
        {
            _dialogWasManuallyAnswered = true;
            _alertRecognitionTCS.TrySetResult(result);
        }

        void CancelDialog()
        {
            if (_dialog?.IsShowing ?? false)
                _dialog?.Cancel();
            _dialog = null;
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
            Log.LogMessage("SpeechRecognizerError = " + error);

            //if (!_dialogWasManuallyAnswered && (error == SpeechRecognizerError.NoMatch || error == SpeechRecognizerError.SpeechTimeout))
            //    StartSpeechRecognition();
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
            platformService.SetAudioEnabled(true);
        }

        public void OnRmsChanged(float rmsdB)
        {
            Console.WriteLine("OnRmsChanged");
        }
    }
}

