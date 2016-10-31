using System;

using UIKit;
using Foundation;
using ObjCRuntime;
using CoreGraphics;
using AVFoundation;

namespace OpenEars
{
	[BaseType(typeof(NSObject))]
	interface OEContinuousModel
	{
		[Export("exitListeningLoop")]
		bool ExitListeningLoop { get; set; }

		[Export("inMainRecognitionLoop")]
		bool InMainRecognitionLoop { get; set; }

		[Export("thereIsALanguageModelChangeRequest")]
		bool ThereIsALanguageModelChangeRequest { get; set; }

		[Export("languageModelFileToChangeTo")]
		string LanguageModelFileToChangeTo { get; set; }

		[Export("dictionaryFileToChangeTo")]
		string DictionaryFileToChangeTo { get; set; }

		[Export("secondsOfSilenceToDetect")]
		float SecondsOfSilenceToDetect { get; set; }

		[Export("listeningLoopWithLanguageModelAtPath:dictionaryAtPath:languageModelIsJSGF:")]
		void ListeningLoopWithLanguageModelAtPathdictionaryAtPathlanguageModelIsJSGF(string languageModelPath, string dictionaryPath, bool languageModelIsJSGF);

		[Export("changeLanguageModelToFile:withDictionary:")]
		void ChangeLanguageModelToFilewithDictionary(string languageModelPathAsString, string dictionaryPathAsString);

		[Export("getCurrentRoute")]
		String GetCurrentRoute();

		[Export("setCurrentRouteTo:")]
		void SetCurrentRouteTo(string newRoute);

		[Export("getRecognitionIsInProgress")]
		int GetRecognitionIsInProgress();

		[Export("setRecognitionIsInProgressTo:")]
		void SetRecognitionIsInProgressTo(int recognitionIsInProgress);

		[Export("getRecordData")]
		int GetRecordData();

		[Export("setRecordDataTo:")]
		void SetRecordDataTo(int recordData);

		[Export("getMeteringLevel")]
		float GetMeteringLevel();
	}

	[BaseType(typeof(NSObject))]
	interface OEPocketsphinxController
	{

		[Export("voiceRecognitionThread")]
		NSThread VoiceRecognitionThread { get; set; }

		[Export("continuousModel")]
		OEContinuousModel ContinuousModel { get; set; }

		[Export("eventsObserver")]
		OEEventsObserver OpenEarsEventsObserver { get; set; }

		[Export("secondsOfSilenceToDetect")]
		float SecondsOfSilenceToDetect { get; set; }

		[Export("stopListening")]
		void StopListening();

		[Export("startListeningWithLanguageModelAtPath:dictionaryAtPath:acousticModelAtPath:languageModelIsJSGF:")]
		void StartListeningWithLanguageModelAtPathdictionaryAtPathlanguageModelIsJSGF(string languageModelPath, string dictionaryPath, string acousticModelPath, bool languageModelIsJSGF);

		[Export("suspendRecognition")]
		void SuspendRecognition();

		[Export("resumeRecognition")]
		void ResumeRecognition();

		[Export("changeLanguageModelToFile:withDictionary:")]
		void ChangeLanguageModelToFilewithDictionary(string languageModelPathAsString, string dictionaryPathAsString);

		[Export("startVoiceRecognitionThreadWithLanguageModelAtPath:dictionaryAtPath:languageModelIsJSGF:")]
		void StartVoiceRecognitionThreadWithLanguageModelAtPathdictionaryAtPathlanguageModelIsJSGF(string languageModelPath, string dictionaryPath, bool languageModelIsJSGF);

		[Export("stopVoiceRecognitionThread")]
		void StopVoiceRecognitionThread();

		[Export("waitForVoiceRecognitionThreadToFinish")]
		void WaitForVoiceRecognitionThreadToFinish();

		[Export("startVoiceRecognitionThreadAutoreleasePoolWithArray:")]
		void StartVoiceRecognitionThreadAutoreleasePoolWithArray(NSArray arrayOfLanguageModelItems);

		[Export("suspendRecognitionForFliteSpeech")]
		void SuspendRecognitionForFliteSpeech();

		[Export("resumeRecognitionForFliteSpeech")]
		void ResumeRecognitionForFliteSpeech();

		[Export("setSecondsOfSilence")]
		void SetSecondsOfSilence();

		[Export("pocketsphinxInputLevel")]
		float PocketsphinxInputLevel();

	}

	[BaseType(typeof(NSObject))]
	interface OEFliteController
	{

		[Export("speechInProgress")]
		bool SpeechInProgress { get; set; }

		[Export("audioPlayer")]
		AVAudioPlayer AudioPlayer { get; set; }

		[Export("eventsObserver")]
		OEEventsObserver OpenEarsEventsObserver { get; set; }

		[Export("speechData")]
		NSData SpeechData { get; set; }

		[Export("duration_stretch")]
		float Duration_stretch { get; set; }

		[Export("target_mean")]
		float Target_mean { get; set; }

		[Export("target_stddev")]
		float Target_stddev { get; set; }

		[Export("say:withVoice:")]
		void SaywithVoice(string statement, string voice);

		[Export("fliteOutputLevel")]
		float FliteOutputLevel();

		[Export("interruptionRoutine:")]
		void InterruptionRoutine(AVAudioPlayer player);

		[Export("interruptionOverRoutine:")]
		void InterruptionOverRoutine(AVAudioPlayer player);

		[Export("sendResumeNotificationOnMainThread")]
		void SendResumeNotificationOnMainThread();

		[Export("sendSuspendNotificationOnMainThread")]
		void SendSuspendNotificationOnMainThread();

		[Export("interruptTalking")]
		void InterruptTalking();

	}

	[BaseType(typeof(NSObject))]
	interface OELanguageModelGenerator
	{
		[Export("generateLanguageModelFromArray:withFilesNamed:")]
		NSError GenerateLanguageModelFromArraywithFilesNamed(NSArray languageModelArray, string fileName);
	}


    [BaseType (typeof (NSObject))]
    [Model]
    interface OEAcousticModel
    {
        [Export ("pathToModel:")]
        NSString PathToModel (string acousticModelBundleName);
    }

	//[BaseType(typeof(NSObject))]
	//interface OpenEarsEventsObserver
	//{
	//	[Export("delegate")]
	//	OpenEarsEventsObserverDelegate Delegate { get; set; }
	//}

	[BaseType (typeof (NSObject))]
	[Protocol]
	interface OEEventsObserver
	{
		[Wrap ("WeakDelegate")]
		OEEventsObserverDelegate Delegate { get; set; }

		[Export ("delegate", ArgumentSemantic.Assign), NullAllowed]
		NSObject WeakDelegate { get; set; }
	}

	[BaseType(typeof(NSObject))]
	[Model]
	interface OEEventsObserverDelegate
	{

		[Export("audioSessionInterruptionDidBegin")]
		void AudioSessionInterruptionDidBegin();

		[Export("audioSessionInterruptionDidEnd")]
		void AudioSessionInterruptionDidEnd();

		[Export("audioInputDidBecomeUnavailable")]
		void AudioInputDidBecomeUnavailable();

		[Export("audioInputDidBecomeAvailable")]
		void AudioInputDidBecomeAvailable();

		[Export("audioRouteDidChangeToRoute:")]
		void AudioRouteDidChangeToRoute(string newRoute);

		[Export("pocketsphinxDidStartCalibration")]
		void PocketsphinxDidStartCalibration();

		[Export("pocketsphinxDidCompleteCalibration")]
		void PocketsphinxDidCompleteCalibration();

		[Export("pocketsphinxRecognitionLoopDidStart")]
		void PocketsphinxRecognitionLoopDidStart();

		[Export("pocketsphinxDidStartListening")]
		void PocketsphinxDidStartListening();

		[Export("pocketsphinxDidDetectSpeech")]
		void PocketsphinxDidDetectSpeech();

		[Export("pocketsphinxDidDetectFinishedSpeech")]
		void PocketsphinxDidDetectFinishedSpeech();

		[Export("pocketsphinxDidReceiveHypothesis:recognitionScore:utteranceID:")]
		void PocketsphinxDidReceiveHypothesisrecognitionScoreutteranceID(string hypothesis, string recognitionScore, string utteranceID);

		[Export("pocketsphinxDidStopListening")]
		void PocketsphinxDidStopListening();

		[Export("pocketsphinxDidSuspendRecognition")]
		void PocketsphinxDidSuspendRecognition();

		[Export("pocketsphinxDidResumeRecognition")]
		void PocketsphinxDidResumeRecognition();

		[Export("pocketsphinxDidChangeLanguageModelToFile:andDictionary:")]
		void PocketsphinxDidChangeLanguageModelToFileandDictionary(string newLanguageModelPathAsString, string newDictionaryPathAsString);

		[Export("pocketSphinxContinuousSetupDidFail")]
		void PocketSphinxContinuousSetupDidFail();

		[Export("fliteDidStartSpeaking")]
		void FliteDidStartSpeaking();

		[Export("fliteDidFinishSpeaking")]
		void FliteDidFinishSpeaking();
	}
}

