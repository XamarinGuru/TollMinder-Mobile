#r @"packages/FAKE.4.4.2/tools/FakeLib.dll"
#load "build-helpers.fsx"
open Fake
open System
open System.IO
open System.Linq
open BuildHelpers
open Fake.XamarinHelper
open HockeyAppHelper

//Environment variables
let version = environVarOrDefault "VERSION" "1"
let build = environVarOrDefault "BUILD_NUMBER" "1"

Target "clean" (fun _ ->
    let dirs = !! "./**/bin/"
                  ++ "./**/obj/"
    CleanDirs dirs
)


Target "core-build" (fun () ->
    RestorePackages "TollMinder.sln"
    MSBuild "TollMinder.Core/bin/Release" "Build" [ ("Configuration", "Release"); ("Platform", "Any CPU") ] [ "TollMinder.Core/TollMinder.Core.csproj" ] |> ignore
)

Target "ios-build" (fun () ->
    RestorePackages "TollMinder.sln"
    MSBuild "TollMinder.Touch/bin/Release" "Build" [ ("Configuration", "Release"); ("Platform", "iPhoneSimulator") ] [ "TollMinder.Touch/TollMinder.Touch.csproj" ] |> ignore
)

Target "ios-adhoc" (fun () ->
    RestorePackages "TollMinder.sln"

    iOSBuild (fun defaults ->
        {defaults with
            ProjectPath = "TollMinder.Touch/TollMinder.Touch.csproj"
            OutputPath = "TollMinder.Touch/bin/iPhone/Adhoc/"
            Target = "Build"
            Configuration = "Release"
            Platform = "iPhone"
            BuildIpa = true
        }) 

    let appPath = Directory.EnumerateFiles(Path.Combine("TollMinder.Touch", "bin", "iPhone"), "*.ipa").First()
    TeamCityHelper.PublishArtifact appPath
)

Target "android-build" (fun () ->
    RestorePackages "TollMinder.sln"
    MSBuild "TollMinder.Droid/bin/Release" "Build" [ ("Configuration", "Release") ] [ "TollMinder.Droid/TollMinder.Droid.csproj" ] |> ignore
)

Target "android-package" (fun () ->
    AndroidPackage (fun defaults ->
        {defaults with
            ProjectPath = "TollMinder.Droid/TollMinder.Droid.csproj"
            Configuration = "Release"
            OutputPath = "TollMinder.Droid/bin/Release"
        })
    |> AndroidSignAndAlign (fun defaults ->
        {defaults with
            KeystorePath = "Tollminder.keystore"
            KeystorePassword = getBuildParamOrDefault "pass" ""
            KeystoreAlias = "tollminder"
            ZipalignPath = "/Users/nickolasshpotenko/Library/Developer/Xamarin/android-sdk-macosx/build-tools/23.0.2/zipalign"
        })
    |> fun file -> TeamCityHelper.PublishArtifact file.FullName
)

Target "ios-hockey" (fun () ->
    HockeyApp(fun parametrs ->
        {parametrs with
            ApiToken = getBuildParam "hkey"
            File = Directory.EnumerateFiles(Path.Combine("TollMinder.Touch", "bin", "iPhone"), "*.ipa").First()
         }) |> ignore    
)

Target "android-hockey" (fun () ->
    HockeyApp(fun parametrs ->
        {parametrs with
            ApiToken = getBuildParam "hkey"
            File = Directory.EnumerateFiles(Path.Combine("TollMinder.Droid", "bin", "Release"), "*Aligned.apk").First()
         }) |> ignore   
)


Target "android-version"(fun () -> 
    let path = Directory.EnumerateFiles(Path.Combine("TollMinder.Droid", "Properties"), "AndroidManifest.xml").First()
    let ns = Seq.singleton(("android", "http://schemas.android.com/apk/res/android"))
    XmlPokeNS path ns "manifest/@android:versionName" (version + "." + build)
    XmlPokeNS path ns "manifest/@android:versionCode" build
)

Target "ios-version"(fun () -> 
    let path = Directory.EnumerateFiles(Path.Combine("TollMinder.Touch"), "Info.plist").First()
    let file = fileInfo path
    log ("Path to project: " + file.FullName + " Version number: " + version + "." + build)
    Exec "/usr/libexec/PlistBuddy" ("-c 'Set :CFBundleShortVersionString " + version + "' " + file.FullName)
    Exec "/usr/libexec/PlistBuddy" ("-c 'Set :CFBundleVersion " + build + "' " + file.FullName)
)

"core-build"      
    ==> "android-package"

"core-build"   
    ==> "ios-adhoc"

"android-version"
    ==> "android-package"

"android-package"
    ==> "android-hockey"

"ios-adhoc"
    ==> "ios-hockey"

"ios-version"
    ==> "ios-adhoc"
     
RunTargetOrListTargets()
