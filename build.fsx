#r @"packages/FAKE.3.5.4/tools/FakeLib.dll"
#load "build-helpers.fsx"
open Fake
open System
open System.IO
open System.Linq
open BuildHelpers
open Fake.XamarinHelper

Target "core-build" (fun () ->
    RestorePackages "TollMinder.sln"
    MSBuild "TollMinder.Core/bin/Debug" "Build" [ ("Configuration", "Debug"); ("Platform", "Any CPU") ] [ "TollMinder.Core/TollMinder.Core.csproj" ] |> ignore
)

Target "core-tests" (fun () ->
    MSBuild "Tests/TollMinder.Tests/bin/Debug" "Build" [ ("Configuration", "Debug"); ("Platform", "Any CPU") ] [ "Tests/TollMinder.Tests/TollMinder.Tests.csproj" ] |> ignore
    RunNUnitTests "Tests/TollMinder.Tests/bin/Debug/TollMinder.Tests.dll" "Tests/TollMinder.Tests/bin/Debug/testresults.xml"
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

Target "ios-uitests" (fun () ->
    let appPath = Directory.EnumerateDirectories(Path.Combine("TollMinder.Touch", "bin", "iPhoneSimulator", "Debug"), "*.app").First()

    RunUITests appPath
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
            KeystorePath = "TollMinder.keystore"
            KeystorePassword = "goclientpass" // TODO: don't store this in the build script for a real app!
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

"core-build"
    ==> "android-build"

"core-build"
    ==> "ios-build"

"android-build"
    ==> "android-package"

"android-package"
    ==> "android-package-testfairy"

"android-package"
    ==> "android-package-hockeyapp"

"ios-build"
    ==> "ios-uitests"



RunTarget()