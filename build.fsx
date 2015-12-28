#r @"packages/FAKE.4.4.2/tools/FakeLib.dll"
#load "build-helpers.fsx"
open Fake
open System
open System.IO
open System.Linq
open BuildHelpers
open Fake.XamarinHelper

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
    RestorePackages "TollMinder_iOS.sln"

    iOSBuild (fun defaults ->
        {defaults with
            ProjectPath = "TollMinder_iOS.sln"
            Configuration = "Debug|iPhoneSimulator"
            Target = "Build"
        })
)

Target "ios-adhoc" (fun () ->
    RestorePackages "TollMinder_iOS.sln"

    iOSBuild (fun defaults ->
        {defaults with
            ProjectPath = "TollMinder_iOS.sln"
            Configuration = "Ad-Hoc|iPhone"
            Target = "Build"
        })

    let appPath = Directory.EnumerateFiles(Path.Combine("TollMinder.Touch", "bin", "iPhone", "Ad-Hoc"), "*.ipa").First()

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
            KeystorePath = "Toll\ Minder.keystore"
            KeystorePassword = getBuildParamOrDefault "pass" ""
            KeystoreAlias = "TollMinder"
        })
    |> fun file -> TeamCityHelper.PublishArtifact file.FullName
)

Target "android-uitests" (fun () ->
    AndroidPackage (fun defaults ->
        {defaults with
            ProjectPath = "TollMinder.Droid/TollMinder.Droid.csproj"
            Configuration = "Release"
            OutputPath = "TollMinder.Droid/bin/Release"
        }) |> ignore

    let appPath = Directory.EnumerateFiles(Path.Combine("TollMinder.Droid", "bin", "Release"), "*.apk", SearchOption.AllDirectories).First()

    RunUITests appPath
)

Target "android-package-testfairy" (fun () ->
    let appPath = Directory.EnumerateFiles(Path.Combine("TollMinder.Droid", "bin", "Release"), "*Aligned.apk", SearchOption.AllDirectories).First()
    Exec "testfairy-upload.sh" appPath
)

Target "android-package-hockeyapp" (fun () ->
    let appPath = Directory.EnumerateFiles(Path.Combine("TollMinder.Droid", "bin", "Release"), "*Aligned.apk", SearchOption.AllDirectories).First()
    Exec "hockeyapp-upload.sh" appPath
)

//"clean"
//    ==> "core-build"
//    ==> "android-build"
//    ==> "ios-build"
//    ==> "android-package" 

"core-build"    
    ==> "android-build"

"core-build"
    ==> "ios-build"

"android-build"
    ==> "android-package"

//"android-package"
//    ==> "android-package-testfairy"

//"android-package"
//   ==> "android-package-hockeyapp"

RunTarget()
