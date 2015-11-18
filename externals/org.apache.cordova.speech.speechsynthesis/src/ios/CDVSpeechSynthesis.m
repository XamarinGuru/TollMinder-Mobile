// http://docs.phonegap.com/en/3.5.0/guide_platforms_ios_plugin.md.html#iOS%20Plugins
#import "CDVSpeechSynthesis.h"


@implementation CDVSpeechSynthesis

@synthesize status = CDVCommandStatus_OK;
@synthesize state = STOPED;

- (void) continueSpeakingSyntheticSpeaking{
    [self.speechSynthesizer continueSpeaking];
}


- (void) stopSyntheticSpeaking{
    [self.speechSynthesizer stopSpeakingAtBoundary:AVSpeechBoundaryImmediate];
}


# pragma mark CDVPlugin


- (void) startup:(CDVInvokedUrlCommand*) command {
    [self.commandDelegate runInBackground:^{
        if(!self.speechSynthesizer) {
            self.state = INITIALIZING;
            self.speechSynthesizer = [[AVSpeechSynthesizer alloc] init];
            self.speechSynthesizer.delegate = self;
            self.state = STARTED;
        }
        
        self.delegateCallbackId = command.callbackId;
        
        CDVPluginResult* pluginResult = [CDVPluginResult resultWithStatus:self.status messageAsInt:self.state];
    
        [pluginResult setKeepCallbackAsBool:YES];
        [self.commandDelegate sendPluginResult:pluginResult callbackId:command.callbackId];
    }];
}


- (void) shutdown:(CDVInvokedUrlCommand*) command {
    [self.commandDelegate runInBackground:^{
        if(self.speechSynthesizer) {
            self.speechSynthesizer = nil;
        }
        CDVPluginResult* pluginResult = [CDVPluginResult resultWithStatus:self.status messageAsString:@""];
        [self.commandDelegate sendPluginResult:pluginResult callbackId:command.callbackId];
    }];
    
}



- (void) speak:(CDVInvokedUrlCommand*) command {
    [self.commandDelegate runInBackground:^{
        if ([self isReady]) {
            NSError* error;
            AVSpeechUtterance* utterance = [self parseUtterance:command returningError:&error];
            [self.speechSynthesizer speakUtterance:utterance];
            // The sendPluginResult method is thread-safe.
            CDVPluginResult* pluginResult = [CDVPluginResult resultWithStatus:CDVCommandStatus_OK messageAsInt:STARTED];
            [pluginResult setKeepCallbackAsBool:YES];
            [self.commandDelegate sendPluginResult:pluginResult callbackId:command.callbackId];
        }
    }];
}

- (void) cancel:(CDVInvokedUrlCommand*) command {
    [self.commandDelegate runInBackground:^{
        NSString* payload = nil;
        CDVPluginResult* pluginResult = [CDVPluginResult resultWithStatus:self.status messageAsString:payload];
        [self.speechSynthesizer stopSpeakingAtBoundary:AVSpeechBoundaryImmediate];
        [self.commandDelegate sendPluginResult:pluginResult callbackId:command.callbackId];
    }];
}


- (void) pause:(CDVInvokedUrlCommand*) command {
    [self.commandDelegate runInBackground:^{
        NSString* payload = nil;
        CDVPluginResult* pluginResult = [CDVPluginResult resultWithStatus:self.status messageAsString:payload];
        [self.speechSynthesizer pauseSpeakingAtBoundary:AVSpeechBoundaryImmediate];
        [self.commandDelegate sendPluginResult:pluginResult callbackId:command.callbackId];
    }];
}


- (void) resume:(CDVInvokedUrlCommand*) command {
    [self.commandDelegate runInBackground:^{
        NSString* payload = nil;
        CDVPluginResult* pluginResult = [CDVPluginResult resultWithStatus:self.status messageAsString:payload];
        [self.speechSynthesizer continueSpeaking];
        [self.commandDelegate sendPluginResult:pluginResult callbackId:command.callbackId];
    }];
}


- (void) stop:(CDVInvokedUrlCommand*) command {
    [self.commandDelegate runInBackground:^{
        NSString* payload = nil;
        CDVPluginResult* pluginResult = [CDVPluginResult resultWithStatus:self.status messageAsString:payload];
        [self.speechSynthesizer stopSpeakingAtBoundary:AVSpeechBoundaryImmediate];
        [self.commandDelegate sendPluginResult:pluginResult callbackId:command.callbackId];
    }];
}

- (void) isLanguageAvailable:(CDVInvokedUrlCommand*) command {
    [self.commandDelegate runInBackground:^{
        NSString* payload = nil;
        // Some blocking logic...
        CDVPluginResult* pluginResult = [CDVPluginResult resultWithStatus:CDVCommandStatus_OK messageAsString:payload];
        
        [self.commandDelegate sendPluginResult:pluginResult callbackId:command.callbackId];
    }];
}

#pragma mark Delegets
-(void)speechSynthesizer:(AVSpeechSynthesizer *)synthesizer didFinishSpeechUtterance:(AVSpeechUtterance *)utterance {
    [self.commandDelegate runInBackground:^{

        NSMutableDictionary* dict = [[NSMutableDictionary alloc]init];
        [dict setObject:@"end" forKey:@"fireEndEvent"];
        
        CDVPluginResult* pluginResult = [CDVPluginResult resultWithStatus:CDVCommandStatus_OK messageAsDictionary:dict];
        [pluginResult setKeepCallbackAsBool:YES];
        [self.commandDelegate sendPluginResult:pluginResult callbackId:self.delegateCallbackId];
    }];
    NSLog(@"Playback finished");
}

-(void)speechSynthesizer:(AVSpeechSynthesizer *)synthesizer didCancelSpeechUtterance:(AVSpeechUtterance *)utterance {
    [self.commandDelegate runInBackground:^{
        
        NSMutableDictionary* dict = [[NSMutableDictionary alloc]init];
        [dict setObject:@"cancel" forKey:@"fireEndEvent"];
        
        CDVPluginResult* pluginResult = [CDVPluginResult resultWithStatus:CDVCommandStatus_OK messageAsDictionary:dict];
        [pluginResult setKeepCallbackAsBool:YES];
        [self.commandDelegate sendPluginResult:pluginResult callbackId:self.delegateCallbackId];
    }];
    NSLog(@"Playback canceled");
}

-(void)speechSynthesizer:(AVSpeechSynthesizer *)synthesizer didPauseSpeechUtterance:(AVSpeechUtterance *)utterance {
    [self.commandDelegate runInBackground:^{
        
        NSMutableDictionary* dict = [[NSMutableDictionary alloc]init];
        [dict setObject:@"pause" forKey:@"fireEndEvent"];
        
        CDVPluginResult* pluginResult = [CDVPluginResult resultWithStatus:CDVCommandStatus_OK messageAsDictionary:dict];
        [pluginResult setKeepCallbackAsBool:YES];
        [self.commandDelegate sendPluginResult:pluginResult callbackId:self.delegateCallbackId];
    }];
    NSLog(@"Playback paused");
}

-(void)speechSynthesizer:(AVSpeechSynthesizer *)synthesizer didContinueSpeechUtterance:(AVSpeechUtterance *)utterance {
    [self.commandDelegate runInBackground:^{
        
        NSMutableDictionary* dict = [[NSMutableDictionary alloc]init];
        [dict setObject:@"continue" forKey:@"fireEndEvent"];
        
        CDVPluginResult* pluginResult = [CDVPluginResult resultWithStatus:CDVCommandStatus_OK messageAsDictionary:dict];
        [pluginResult setKeepCallbackAsBool:YES];
        [self.commandDelegate sendPluginResult:pluginResult callbackId:self.delegateCallbackId];
    }];
    NSLog(@"Playback continued");
}

-(void)speechSynthesizer:(AVSpeechSynthesizer *)synthesizer didStartSpeechUtterance:(AVSpeechUtterance *)utterance {
    [self.commandDelegate runInBackground:^{
        
        NSMutableDictionary* dict = [[NSMutableDictionary alloc]init];
        [dict setObject:@"start" forKey:@"fireEndEvent"];
        
        CDVPluginResult* pluginResult = [CDVPluginResult resultWithStatus:CDVCommandStatus_OK messageAsDictionary:dict];
        [pluginResult setKeepCallbackAsBool:YES];
        [self.commandDelegate sendPluginResult:pluginResult callbackId:self.delegateCallbackId];
    }];
    NSLog(@"Playback started");
}

#pragma mark Parsing

- (AVSpeechUtterance*) parseUtterance:(CDVInvokedUrlCommand*) command returningError:(out NSError **)error {
    
    AVSpeechUtterance* utterance;
    
    NSDictionary* dict = command.arguments[0];
    
    NSString* lang = [dict objectForKey:@"lang"];
    if (lang == nil) {
        *error = [self parseErrorWithDescription:@"'lang' is missing, cannot parse SpeechSynthesisUtterance."];
        return nil;
    }
    
    NSString* text = [dict objectForKey:@"text"];
    if (text == nil) {
        *error = [self parseErrorWithDescription:@"'text' is missing, cannot parse SpeechSynthesisUtterance."];
        return nil;
    }
    
    NSNumber *pitch = [dict objectForKey:@"pitch"];
    if (pitch == nil) {
        pitch = [NSNumber numberWithFloat:1.0f];
    }
    

    NSNumber *volume = [dict objectForKey:@"volume"];
    if (volume == nil) {
        volume = [NSNumber numberWithFloat:0.5f];
    }
    
    NSNumber *rate = [dict objectForKey:@"rate"];
    if (rate == nil) {
        rate = [NSNumber numberWithFloat:1.0f];
    }
    
    AVSpeechSynthesisVoice *voice;
    
    if (lang==nil){
        voice = [AVSpeechSynthesisVoice voiceWithLanguage: [AVSpeechSynthesisVoice currentLanguageCode]];
        
    } else {
        voice = [AVSpeechSynthesisVoice voiceWithLanguage: lang];
        
    }
    
    utterance = [AVSpeechUtterance speechUtteranceWithString:text];
    utterance.voice = voice;
    utterance.rate = rate.floatValue;
    utterance.volume = volume.floatValue;
    utterance.pitchMultiplier = pitch.floatValue;
    
    return utterance;
}

#pragma mark Utilities

- (NSError*) parseErrorWithDescription:(NSString*) description {
    return [self errorWithCode:CDV_SPEECH_SYNTHESIS_INPUT_PARSE_ERROR andDescription:description];
}

- (NSError*) errorWithCode: (int)code andDescription:(NSString*) description {
    
    NSMutableDictionary* details;
    if (description != nil) {
        details = [NSMutableDictionary dictionary];
        [details setValue:description forKey:NSLocalizedDescriptionKey];
    }
    
    return [[NSError alloc] initWithDomain:@"CDVSpeechSynthesis" code:code userInfo:details];
}

- (bool) isReady {
    return (self.state == STARTED) ? true : false;
}

@end
