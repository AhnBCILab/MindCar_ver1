Firebase Unity SDK
==================

The Firebase Unity SDK provides Unity packages for the following Firebase
features on *iOS* and *Android*:

| Feature                            | Unity Package                     |
|:----------------------------------:|:---------------------------------:|
| Firebase Analytics                 | FirebaseAnalytics.unitypackage    |
| Firebase Authentication            | FirebaseAuth.unitypackage         |
| Firebase Realtime Database         | FirebaseDatabase.unitypackage     |
| Firebase Dynamic Links             | FirebaseDynamicLinks.unitypackage |
| Firebase Instance ID               | FirebaseInstanceId.unitypackage   |
| Firebase Invites                   | FirebaseInvites.unitypackage      |
| Firebase Messaging                 | FirebaseMessaging.unitypackage    |
| Firebase Realtime Database         | FirebaseDatabase.unitypackage     |
| Firebase Remote Config             | FirebaseRemoteConfig.unitypackage |
| Firebase Storage                   | FirebaseStorage.unitypackage      |

## AdMob

The AdMob Unity plugin is distributed separately and is available from the
[AdMob Get Started](https://firebase.google.com/docs/admob/unity/start) guide.

## Stub Implementations

Stub (non-functional) implementations are provided for convenience when
building for Windows, OSX and Linux so that you don't need to conditionally
compile code when also targeting the desktop.

## Known Issues

### .NET 4.x

.NET 4.x support is available as an experimental build option in Unity 2017 and
beyond.  Firebase plugins use components of the
[Parse SDK](https://github.com/parse-community/Parse-SDK-dotNET) to provide some
.NET 4.x classes in earlier versions of .NET.  Therefore, when importing
Firebase into a .NET 4.x enabled project you'll see compile errors from
some types in the .NET 4.x framework that are implemented by the Parse SDK.

To resolve the compilation error:

1. Remove or disable the following DLLs for all platforms:
    - `Parse/Plugins/Unity.Compat.dll`
    - `Parse/Plugins/Unity.Tasks.dll`
1. Enable the following DLLs for all platforms:
    - `Parse/Plugins/dotNet45/Unity.Compat.dll`
    - `Parse/Plugins/dotNet45/Unity.Tasks.dll`

If you import another Firebase plugin:

- Select the menu item
  `Assets > Play Services Resolver > Version Handler > Update`
  to re-enable .NET 4.x DLLs and disable .NET 3.x DLLs.

### Unity 2017.2 Networking

Realtime Database and Storage create TLS network connections using the .NET
networking stack.  TLS functionality is broken in Unity 2017.2 when using
.NET 4.6 causing the Realtime Database and Storage plugins to fail.  There is
no workaround for this issue, you have to use a different version of Unity,
for example 2017.1 or 2017.3.


### Unity 4 workarounds

Firebase plugins are not officially supported in Unity 4.  However, we do
make an effort to ensure the plugins can work with some manual setup.

A couple of components do not work in Unity 4:

  - [Version Handler](https://github.com/googlesamples/unity-jar-resolver#unity-plugin-version-management)
    does not work due to no PluginImporter and no clean way to prevent managed
    DLLs from being loaded by Unity 4.
    - This means it's not possible for plugins to automatically enable the most
      recent version of shared components (e.g Firebase, AdMob, Facebook etc.
      may share a common component).
    - DLLs that target a specific .NET version (e.g .NET 4.x) are not disabled.
  - Managed (C#) DLLs cannot be targeted to a specific platform which breaks
    our plugin where we have platform specific C# DLLs for some components.

To use in Unity 4 you will need to:

  - Resolve any dependencies that are shared between multiple plugins.
    For example, Firebase and AdMob use the
    [Play Services Resolver](https://github.com/googlesamples/unity-jar-resolver)
    which contains DLLs that encode the version in their filename under the
    folder `PlayServicesResolver/Editor`.  For each versioned DLL under the
    folder `PlayServicesResolver/Editor` delete the oldest version of each DLL.
  - Remove .NET 4.x DLLs from `Parse/Plugins/dotNet45`.
  - Remove / rename platform specific DLLs.
    Firebase plugins contain iOS specific DLLs under the folder
    `Firebase/Plugins/iOS`.
    - When *not* building for iOS:
      - Change the extension of files under the `Firebase/Plugins/iOS` folder
        from `.dll` to `.dlldisabled`
    - When building for iOS:
      - Change the extension of files under the `Firebase/Plugins/iOS` folder
        from `.dlldisabled` to `.dll`
      - For each file in `Firebase/Plugins/iOS` change the file extension of
        the same name under `Firebase/Plugins` from `.dll` to `.dlldisabled`.
        For example, `Firebase/Plugins/iOS/Firebase.App.dll` and
        `Firebase/Plugins/Firebase.App.dlldisabled`.

Setup
-----

You need to follow the
[SDK setup instructions](https://firebase.google.com/preview/unity).
Each Firebase package requires configuration in the
[Firebase Console](https://firebase.google.com/console).  If you fail to
configure your project your app's initialization will fail.

Support
-------

[Firebase Support](http://firebase.google.com/support/)

Release Notes
-------------
### 4.4.3
  - Overview
    - Bug fixes in Dynamic Links, Invites, Remote Config and Storage.
  - Changes
    - Dynamic Links (iOS): Now fetches the invite ID when using universal links.
    - Dynamic Links (iOS): Fixed crash on failure of dynamic link completion.
    - Dynamic Links (iOS): Fixed an issue where some errors weren't correctly
      reported.
    - Invites (Editor): Fixed SendInvite never completing.
    - Remote Config (iOS): Fixed an issue where some errors weren't correctly
      reported.
    - Storage: Fixed Metadata::content_language returning the wrong data.
    - Storage (iOS): Reference paths formats are now consistent with other
      platforms.
    - Storage (iOS): Fixed an issue where trying to upload to a non-existent
      path would not complete the Task.
    - Storage (iOS): Fixed a crash when a download fails.
    - Editor: Fixed a crash in the editor when using .NET 4.6 with certain
      versions of Unity 2017.
    - General (Android): Fixed an issue when Google Play Services was out of
      date and would hang after returning from the update workflow.

### 4.4.2
  - Overview
    - Updated Firebase iOS dependency version.
  - Changes
    - General (iOS): Updated Firebase iOS Cocoapod dependency version.

### 4.4.1
  - Overview
    - Bug fixes for .Net 4.x, Storage, Realtime Database, and Instance ID on
      iOS.
  - Changes
    - Instance ID (iOS): GetTokenAsync no longer fails without an APNS
      certificate, and no longer forces registering for notifications.
    - Storage: Added support for a progress listener and cancellation
      token to `GetBytesAsync`.
    - Storage: Fixed an issue where the auth token was not refreshed when the
      application is started.
    - Realtime Database: Fixed an issue where the auth token was not refreshed
      when the application is started.
    - General (Android): Fixed a bug with handling transitive dependencies in
      the Android Resolver, where there was a common dependency name from
      different sources.
    - General (Android): Fixed Android Resolver reporting non-existent
      conflicts.
    - General: Fixed 'get_realtimeSinceStartup' Assert in development builds.
    - General: Fixed issues when using types added in .NET 4.x such as Tuple.
      This requires switching to the appropriate Unity.Compat.dll when using
      .NET 4.x (see Known Issues).

### 4.4.0
  - Overview
    - Support for Instance ID, and an Auth fix.
  - Changes
    - Instance ID: Added Instance ID library.
    - Auth: Fixed user metadata property names.

### 4.3.0
  - Overview
    - General threading / callback and other bug fixes and new features in Auth.
  - Changes
    - General: Fixed some invalid calls to Unity APIs from threads.
    - General (Editor): Changed Firebase settings window to work with Unity 4.x
    - General (Editor): Fixed GoogleServices-Info.plist not being read in batch
      mode.
    - Auth: Fixed a bug due to a race condition fetching the authentication
      token which could cause Database and Storage operations to hang.
    - Auth: Added support for accessing user metadata.
    - Remote Config (Android): Fixed a bug where remote config values retrieved
      were misclassified as coming from a default config vs an active config.
    - Database: Fixed hang when Time.timeScale is 0.
    - Storage: Fixed hang when Time.timeScale is 0.

### 4.2.1
  - Overview
    - Bug fixes for Real-Time Database, Storage, API initialization in .NET 4.x,
      and improvements to the iOS and Android Resolver components.
  - Changes
    - General (Android): Fixed Android resolution when a project path contains
      apostrophes.
    - General (iOS): Increased speed of iOS resolver dependency loading.
    - General (Android): Removed legacy resolution method from Android Resolver.
      It is now only possible to use the Gradle or Gradle prebuild resolution
      methods.
    - General (Android): Fixed Android Resolution issues with OpenJDK by
      updating the Gradle wrapper to 4.2.1.
    - General (Android): Android resolution now also uses
      gradle.properties to pass parameters to Gradle in an attempt to workaround
      problems with command line argument parsing on Windows 10.
    - General: Fixed some invalid calls to Unity APIs from threads, when using
      .NET 4.x which is added in Unity 2017.
    - Database: Fixed hang in Real-Time Database when Time.timeScale is 0 in
      Unity 2017.
    - Storage: Fixed hang in Storage when Time.timeScale is 0 in Unity 2017.
    - Storage: Fixed file download in Unity 2017.2.

### 4.2.0
  - Overview
    - Added URL support in Messaging, improved the initialization process on
      Android and fixed bugs in the iOS and Android build systems, Analytics,
      Auth, Database and Messaging.
  - Changes
    - Messaging: Messages sent to users can now contain a link URL.
    - Auth: Added more specific error codes for failed operations.
    - Auth (iOS): Phone Authentication no longer requires push notifications.
      When push notifications aren't available, reCAPTCHA verification is used
      instead.
    - Analytics (iOS): Fixed bug which prevented the user ID and user
      properties being cleared.
    - Database: Fixed issue where user authentication tokens are ignored if
      the application uses the database API before initializing authentication.
    - Messaging (Android): Fixed a bug which prevented the message ID field
      being set.
    - General (iOS): Fixed incorrect processing of framework modulemap files
      which resulted in the wrong link flags being generated when Cocoapod
      project integration is enabled.
    - General (Android): Added support for Google Play services dependency
      resolution when including multiple plugins (e.g AdMob, Google Play Games
      services) that require different versions of Google Play services.
    - General (Android): Fixed Android dependency resolution when local
      project paths contain spaces.
    - General (Android): Fixed race condition in Android Resolver which could
      cause a hang when running auto-resolution.
    - General (Android): Forced Android Gradle resolution process to not use
      the Gradle daemon to improve reliability of the process.
    - General (Android): Added a check for at least JDK 8 when running Android
      dependency resolution.
    - General: Fixed MonoPInvokeCallbackAttribute incorrectly being added to
      the root namespace causing incompatibility with plugins like slua.
  - Known Issues
    - General (Android): Unity (not the Firebase SDK) has a bug that causes
      applications to crash after running the Google Play services update on
      Android 8.0 Oreo devices.

### 4.1.0
  - Overview
    - Bug fixes for the iOS build system, Auth, Messaging, and Remote Config.
  - Changes
    - General (iOS): Fixed spurious errors on initialization of FirebaseApp.
    - General (iOS): Fixed iOS build with Cocoapod Project integration enabled.
      This affected all iOS builds when using Unity 5.5 or below or when using
      Unity Cloud Build.
    - General (iOS): Fixed issue which prevented the use of Unity Cloud Build
      with Unity 5.6 and above.  Unity Cloud Build does not open generated
      Xcode workspaces so we force Cocoapod Project integration in the
      Unity Cloud Build environment.
    - Auth (Android): Now throws an exception if you call GetCredential without
      an Auth instance created.
    - Messaging (Android): Fixed a bug resulting in FirebaseMessages not having
      their MessageType field populated.
    - Messaging (iOS): Fixed a race condition if a message is received before
      Firebase Cloud Messaging is initialized.
    - Messaging (iOS): Fixed a bug detecting whether the notification was opened
      if the app was running in the background.
    - Remote Config: When listing keys, the list now includes keys with defaults
      set, even if they were not present in the fetched config.

### 4.0.3
  - Overview
    - Bug fixes for Database, Dynamic Links, Messaging, iOS SDK compatibility,
      .NET 4.x compatibility.
  - Changes
    - General: Added support for .NET 4.x in the System.Task implementation
      used by the SDK.  The VersionHandler editor plugin is now used to switch
      Task implementations based upon the selected .NET version.
    - General: Fixed root cert installation failure if Firebase is initialized
      after other network operations are performed by an application.
    - General: Improved native shared library name mangling when targeting
      Linux.
    - General (iOS): Fixed an issue which resulted in custom options not being
      applied to FirebaseApp instances.
    - General (iOS): Fixed a bug which caused method implementation look ups
      to fail when other iOS SDKs rename the selectors of swizzled methods.
      This could result in a hang on startup when using some iOS SDKs.
    - Dynamic Links (Android): Fixed task completion if short link
      creation fails.
    - Database: Fixed a bug that caused database connections to fail when
      using the .NET 4.x framework in Unity 2017 on OSX.
    - Database: Fixed a bug where large data updates could be ignored.
    - Messaging (iOS): Fixed message handling when messages they are received
      via the direct channel to the FCM backend (i.e not via APNS).

### 4.0.2
  - Overview
    - Bug fixes for Analytics, Auth, Dynamic Links, and Messaging;
      added support for Android SDK 25.
  - Changes
    - General (Android): Fixed a manifest issue with Android SDK tools and
      support library >= 25.x.
    - General (Android): Fixed an issue which caused Analytics to not be
      enabled in all plugins.
    - General (Android): Fixed native libraries not being included in built
      APKs when using the internal build system in Unity 2017.
    - Analytics (Android): Fix SetCurrentScreen to work from any thread.
    - Auth (iOS): Fixed user being invalidated when linking a credential fails.
    - Dynamic Links: Fixed an issue which caused an app to crash or not receive
      a Dynamic Link if the link is opened when the app is installed and not
      running.
    - Messaging (iOS): Fixed a crash when no notification event is registered.
    - Messaging: Fixed token notification event occasionally being raised twice
      with the same token.

## 4.0.1
  - Overview:
    - Bug fixes for Dynamic links and Invites on iOS, the Google Play
      services updater when using Cloud Messaging and Cloud Messaging on iOS.
  - Changes:
    - Cloud Messaging (Android): Fixed crash when updating Google Play services
      in projects that include the Cloud Messaging functionality.
    - Cloud Messaging (iOS): Fixed an issue where library would crash on start
      up if there was no registration token.
    - Dynamic Links & Invites (iOS): Fixed an issue that resulted in apps not
      receiving a link when opening a link if the app is installed and not
      running.

## 4.0.0
  - Overview
    - Added support for phone number authentication, access to user metadata,
      a standalone dynamic links plugin and bug fixes.
  - Changes
    - Auth: Added support for phone number authentication.
    - Auth: Added the ability to retrieve user metadata.
    - Auth: Moved token notification into a separate token change event.
    - Dynamic Links: Added a standalone Unity plugin separate from Invites.
    - Invites (iOS): Fixed an issue in the analytics SDK's method swizzling
      which resulted in dynamic links / invites not being sent to the
      application.
    - Messaging (Android): Fixed a regression introduced in 3.0.3 which caused
      a crash when opening up a notification when the app is running in the
      background.
    - Messaging (iOS): Fixed interoperation with other users of local
      notifications.
    - General (Android): Fixed crash in some circumstances after resolving
      dependencies by updating Google Play services.
    - General (Editor): Fixed iOS resolver and Jar resolver plugins getting
      disabled when importing multiple Firebase, Google Play Games or AdMob
      plugins into a project.
    - General (iOS): Added support for Cocoapod builds that use Xcode
      workspaces in Unity 5.6 and above.
    - General (iOS): Fixed Cocoapod version pinning which was broken in 3.0.3
      causing the SDK to pull in the most recent Firebase iOS SDK rather than
      the correct version for the current Unity SDK release.

## 3.0.3
  - Overview
    - Bug fixes for Auth.
  - Changes
    - Auth: Fixed a crash caused by a stale memory reference when a
      firebase::auth::Auth object is destroyed and then recreated for the same
      App object.
    - Auth: Fixed potential memory corruption when AuthStateListener is
      destroyed.
    - Auth: Fixed occasional crash in Unity editor when using Auth sign-in
      methods.
## 3.0.2
  - Overview
    - Bug fixes for Auth, Database, Invites, Messaging, Storage, and a general
      fix, plus improved compatibility with Unity 5.6 when using the GoogleVR
      SDK.
  - Changes
    - General (Android): Fixed unhandled exception if FirebaseApp creation
      fails due to an out of date Google Play services.
    - General (Android): Fixed Google Play Services updater crash when clicking
      outside of the dialog on Android 4.x devices.
    - Auth: Fixed user being invalidated when linking a credential fails.
    - Auth: Fixed an occasional crash when events are fired.  This could
      manifest in a crash when signing in.
    - Auth: Deprecated FirebaseUser.RefreshToken.
    - Database: Fixed an issue which caused the application to manually
      refresh the auth token.
    - Messaging: Fixed incorrectly notifying the app of a message when a
      notification is received while the app is in the background and the app
      is then opened by via the app icon rather than the notification.
    - Invites (iOS): Fixed an issue which resulted in the app delegate method
      application:openURL:sourceApplication:annotation: not being called
      when linking the invites library.  This caused the Facebook SDK login
      flow to fail.
    - Storage: Fixed a bug that prevented the construction of Metadata without
      a storage reference.
    - Editor (Android): Fixed referenced Android dependencies in maven
      where the POM references a specific version e.g '[1.2.3]'.
    - Editor (iOS): Improved compatibility with Unity 5.6's Cocoapods support
      required to use the GoogleVR SDK.
    - Editor (Android): Fixed Android dependency resolution when the bundle ID
      is modified.

## 3.0.1
  - Overview
    - Fixed Google Play Services checker on Android and improved Android
      build configuration checks.
  - Changes
    - (Android): Fixed Google Play Services checker on Android.  Previously
      when Google Play Services was out of date,
      FirebaseApp.CheckDependencies() incorrectly returned
      DependencyStatus.Available.
    - Editor (Android): Added check for auto-resolution being enabled in the
      Android Resolver.
      If auto-resolution is disabled by the user or by another plugin
      (e.g Google Play Games), the user is warned about the configuration
      problem and given the opportunity to fix it.
    - (Android) Fixed single architecture builds when using Gradle.
    - (Android) Resolved an issue which caused the READ_PHONE_STATE
      permission to be requested.

## 3.0.0
  - Overview
    - Streamlined editor integration, build support and some bug fixes for
      Auth, Database, Messaging, Invites and Storage.
  - Changes
    - Added link.xml files to allow byte stripping to be enabled.
    - Fixed issues with Android builds when targeting a single ABI.
    - Auth: Fixed race condition when accessing user properties.
    - Auth: Added SetCurrentScreen() method.
    - Database: Resolved issue where large queries resulted in empty results.
    - Database: Fixed an issue which prevented saving boolean values.
    - Mesaging: Fixed issue with initialization on iOS that caused problems
      with other SDKs.
    - Invites: Fixed issue with initialization on iOS that caused problems
      with other SDKs.
    - Storage: Fixed a bug which prevented download URLs from containing
      slashes.
    - Storage: Fixed a bug on iOS which caused networking to fail when the
      full .NET 2.0 is used.
    - Editor: Added process of cleaning stale / moved files when upgrading
      to a newer plugin version.
    - Editor: Automated Cocoapod tool installation and improved Pod tool
      detection when using RVM.  This enables iOS projects to build with
      Unity Cloud Build.
    - Editor: Added support for pods that reference static libraries.
    - Editor: Bundle ID selection dialog for iOS and Android is now displayed
      when the project bundle ID doesn't match the Firebase configuration.
    - Editor: Added experimental support for building with Proguard stripping
      enabled.
    - Editor: Fixed Android package (AAR) synchronization when the project
      bundle ID is modified.
    - Editor: Fixed clean up of stale AAR dependencies when users change
      Android SDK versions.
    - Editor: Android Jar Resolver now remembers - for the editor session -
      which AARs to keep when new AARs are available compared to what is
      included in a project.
    - Editor: Added support for projects that use Google Play Services at
      different versions.
    - Editor: Fixed minor issue with the Firebase window not being repainted as
      Firebase configuration files are added to or removed from a project.
    - Desktop: Added fake - but valid - JWT in the Authentication mock.


## 1.1.2
  - Overview
    - Fix for a major bug causing Auth to hang, as well as other bug fixes.
  - Changes
    - Auth: Fixed a potential deadlock when running callbacks registered via
      Task.ContinueWith()
    - Auth: (Android) Fixed an error in `Firebase.Auth.FirebaseUser.PhotoUrl`.
    - Messaging: (iOS) Removed hard dependency on Xcode 8.
    - Messaging: (Android) Fixed an issue where the application would receive an
      empty message on startup.

## 1.1.1
  - Overview
    - Bug fixes for the editor plugin, Firebase Authentication, Messaging,
      Invites, Real-Time Database and Storage.
  - Changes
    - Fixed an issue in the editor plugin that caused an exception to be
      thrown when the project bundle ID didn't match a bundle ID in the Android
      configuration file (google-services.json).
    - Fixed a bug in the editor plugin that caused a stack overflow when
      multiple iOS configuration files (GoogleServices-Info.plist) are
      present in a project.
    - Auth: (Android) Fixed an issue that caused a Task to never complete
      when signing in while a user is already signed in.
    - Auth: Renamed the Auth.UserProfile.ProtoUri property to
      Auth.UserProfile.ProtoUrl in order to be consistent with the other URL
      properties across the SDK.
    - Messaging / Invites: Fixed an issue with method swizzling that caused
      some of the application's UIApplicationDelegate methods to not be called.
    - Storage: The Storage  plugin was using a Unity API that is only
      present in Unity 5.4. We have modified the component so that it is now
      backwards compatible with previous versions of Unity.
    - Real-Time Database: Fixed an issue that prevented saving floating point
      values.

## 1.1.0
  - Overview
    - Added support for Firebase Storage and bug fixes.
  - Changes
    - Added support for Firebase Storage.
    - Fixed crash in Firebase Analytics when logging arrays of parameters.
    - Fixed crash in Firebase Messaging when receiving messages with empty
      payloads on Android.
    - Fixed random hang when initializing Firebase Messaging on iOS.
    - Fixed topic subscriptions in Firebase Messaging.
    - Fixed an issue that resulted in a missing app icon when using Firebase
      Messaging on Android.
    - Fixed exception in error message construction when FirebaseApp
      initialization fails.
    - Fixed reporting of null events in the Firebase Realtime Database.
    - Fixed unsubscribe for complex queries in the Firebase Realtime Database.
    - Fixed service account authentication in the Firebase Realtime Database.
    - Fixed Firebase.Database.Unity being stripped from iOS builds.
    - Fixed support for building with Firebase plugins in Microsoft
      Visual Studio.
    - Fixed scene transitions causing event routing to break across all
      components.
    - Changed editor plugins for Firebase Authentication and Invites to
      return success for all operations instead of raising exceptions.
    - Changed editor plugin to read JAVA_HOME from the Unity editor
      preferences.
    - Changed editor plugin to scan all google-services.json and
      GoogleService-Info.plist files in the project and select the config file
      matching the project's current bundle ID.
    - Improved the performance of AAR / JAR resolution when the Android config
      is selected and auto-resolution is enabled.
    - Improved error messages in the editor plugin.
  - Known Issues
    - Proguard is not integrated into Android builds. We have distributed
      proguard files that can be manually integrated into Android builds
      within AAR files matching the following pattern in each
      Unity package:
      `Firebase/m2repository/com/google/firebase/firebase-*-unity/*firebase-*.srcaar`
    - Incompatible AARs are not resolved correctly when building for Android.
      This can require manual intervention when using multiple plugins
      (e.g Firebase + AdMob + Google Play Games).  A workaround is documented
      on the
      [AdMob Unity plugin issue tracker](https://github.com/googleads/googleads-mobile-unity/issues/314).

## 1.0.1
  - Overview
    - Bug fixes.
  - Changes
    - Fixed Realtime Database restricted access from the Unity Editor on
      Windows.
    - Fixed load and build errors when iOS support is not installed.
    - Fixed an issue that prevented the creation of multiple FirebaseApp
      instances and customization of the default instance on iOS.
    - Removed all dependencies on Python for Android resource generation on
      Windows.
    - Fixed an issue with pod tool discovery when the Ruby Gem binary directory
      is modified from the default location.
    - Fixed problems when building for Android with the IL2CPP scripting
      backend.
  - Known Issues
    - Proguard is not integrated into Android builds. We have distributed
      proguard files that can be manually integrated into Android builds
      within AAR files matching the following pattern in each
      Unity package:
      `Firebase/m2repository/com/google/firebase/firebase-*-unity/*firebase-*.srcaar`

## 1.0.0
  - Overview
    - First public release with support for Firebase Analytics,
      Authentication, Real-time Database, Invites, Dynamic Links and
      Remote Config.
      See our
      [setup guide](https://firebase.google.com/docs/unity/setup) to
      get started.
  - Known Issues
    - Proguard is not integrated into Android builds.  We have distributed
      proguard files that can be manually integrated into Android builds
      within AAR files matching the following pattern in each
      Unity package:
      `Firebase/m2repository/com/google/firebase/firebase-*-unity/*firebase-*.srcaar`
