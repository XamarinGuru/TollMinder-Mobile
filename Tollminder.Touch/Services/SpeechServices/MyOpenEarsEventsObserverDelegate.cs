using System;
using OpenEars;

namespace Tollminder.Touch.Services.SpeechServices
{
    public class MyOpenEarsEventsObserverDelegate : OEEventsObserverDelegate
    {
        TouchSpeechToTextService _service;

        public MyOpenEarsEventsObserverDelegate(TouchSpeechToTextService service)
        {
            _service = service;
        }

        public override void PocketsphinxDidReceiveHypothesisrecognitionScoreutteranceID(string hypothesis, string recognitionScore, string utteranceID)
        {
            Console.WriteLine($"Heard: {hypothesis}, score {recognitionScore}, id {utteranceID}");
            _service.CheckResult(hypothesis.ToLower().Split(' '));
            _service.StopListening();
        }

        public override void AudioSessionInterruptionDidBegin()
        {
            Console.WriteLine("AudioSession interruption began.");
        }

        public override void AudioSessionInterruptionDidEnd()
        {
            Console.WriteLine("AudioSession interruption end.");
        }

        public override void AudioInputDidBecomeUnavailable()
        {
            Console.WriteLine("The audio input has become unavailable");
        }

        public override void AudioInputDidBecomeAvailable()
        {
            Console.WriteLine("The audio input is available.");
        }

        public override void AudioRouteDidChangeToRoute(string newRoute)
        {
            Console.WriteLine("Audio route change. The new route is " + newRoute);
        }

        public override void PocketsphinxDidStartCalibration()
        {
            Console.WriteLine("Pocketsphinx calibration has started.");
        }

        public override void PocketsphinxDidCompleteCalibration()
        {
            Console.WriteLine("Pocket calibration is complete");
        }

        public override void PocketsphinxRecognitionLoopDidStart()
        {
            Console.WriteLine("Pocketsphinx is starting up");
        }

        public override void PocketsphinxDidStartListening()
        {
            Console.WriteLine("Pocketsphinx is now listening");
        }

        public override void PocketsphinxDidDetectSpeech()
        {
            Console.WriteLine("Pocketsphinx has detected speech");
        }

        public override void PocketsphinxDidDetectFinishedSpeech()
        {
            Console.WriteLine("Pocketphinx has detected a second of silence, concluding utterance.");
        }

        public override void PocketsphinxDidStopListening()
        {
            Console.WriteLine("Pocketsphinx has stopped listening");
        }

        public override void PocketsphinxDidSuspendRecognition()
        {
            Console.WriteLine("Pocketsphinx has suspended recognition");
        }

        public override void PocketsphinxDidResumeRecognition()
        {
            Console.WriteLine("Pocketsphinx has resumed recognition");
        }

        public override void FliteDidStartSpeaking()
        {
            Console.WriteLine("Flite has started speaking");
        }

        public override void FliteDidFinishSpeaking()
        {
            Console.WriteLine("Flite has finished speaking.");
        }

        public override void PocketSphinxContinuousSetupDidFail()
        {

        }
    }
}
