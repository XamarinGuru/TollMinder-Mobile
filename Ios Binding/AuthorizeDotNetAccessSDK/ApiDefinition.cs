using System;

using UIKit;
using Foundation;
using ObjCRuntime;
using CoreGraphics;

namespace AuthorizeDotNetAccessSDK
{
    // @interface AcceptSDKErrorResponse : NSObject
    [BaseType(typeof(NSObject))]
    interface AcceptSDKErrorResponse
    {
    }

    // @interface AcceptSDKHandler : NSObject
    [BaseType(typeof(NSObject))]
    interface AcceptSDKHandler
    {
        // -(void)getTokenWithRequest:(AcceptSDKRequest * _Nonnull)inRequest successHandler:(void (^ _Nonnull)(AcceptSDKTokenResponse * _Nonnull))successHandler failureHandler:(void (^ _Nonnull)(AcceptSDKErrorResponse * _Nonnull))failureHandler;
        [Export("getTokenWithRequest:successHandler:failureHandler:")]
        void GetTokenWithRequest(AcceptSDKRequest inRequest, Action<AcceptSDKTokenResponse> successHandler, Action<AcceptSDKErrorResponse> failureHandler);
    }

    // @interface AcceptSDKRequest : NSObject
    [BaseType(typeof(NSObject))]
    interface AcceptSDKRequest
    {
    }

    // @interface AcceptSDKSettings : NSObject
    [BaseType(typeof(NSObject))]
    interface AcceptSDKSettings
    {
    }

    // @interface AcceptSDKTokenResponse : NSObject
    [BaseType(typeof(NSObject))]
    interface AcceptSDKTokenResponse
    {
    }

    // @interface AuthorizeNetAccept_Swift_158 (NSMutableURLRequest)
    [Category]
    [BaseType(typeof(NSMutableUrlRequest))]
    interface NSMutableURLRequest_AuthorizeNetAccept_Swift_158
    {
    }

    //[Static]
    ////[Verify(typeof(ConstantsInterfaceAssociation))]
    //partial interface Constants
    //{
    //    // extern double AuthorizeNetAcceptVersionNumber;
    //    [Field("AuthorizeNetAcceptVersionNumber", "__Internal")]
    //    double AuthorizeNetAcceptVersionNumber { get; }

    //    // extern const unsigned char [] AuthorizeNetAcceptVersionString;
    //    [Field("AuthorizeNetAcceptVersionString", "__Internal")]
    //    byte[] AuthorizeNetAcceptVersionString { get; }
    //}
}
