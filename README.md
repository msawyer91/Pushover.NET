# Pushover.NET
API for the Pushover (iOS/Android) mobile client (migrated from CodePlex/Recreated February 2023)

This project used to live in CodePlex. When viewing the Pushover page at pushover.net, I saw the link to CodePlex no longer worked (it was shut down last year). Since I still had the original source, I wanted it to live on!

# Description
Pushover.NET is an open source library written in C# that allows you to send notifications to Apple iOS and Android mobile devices running the Pushover client. Pushover is supported on Apple iPhone, iPad and iPod Touch running iOS 4+ and is optimized for retina displays, and is also supported on Android phones and tablets running Android 2.0 or later, although it is optimized for Android 4.0 and later.

To send Pushover notifications to your own device(s), you must set up a free account at http://pushover.net. By setting up a Pushover account, you will be provided with a user key. You may also set up one or more applications, each of which receives its own application key.

Once you have a user and application key, all that's left for you to do is install Pushover on your iOS or Android device. If you have iOS devices, you can obtain Pushover from the App Store. Pushover is designed for both iPhone/iPod Touch and iPad, meaning you only have purchase it once and it runs optimized relative to the device; it is also designed for Android phones and tablets as a single purchase. At the time of this writing, Pushover is $4.99 (USD) in both the App Store and Google Play. If you intent to use Pushover on both iOS and Android devices, you must purchase it twice--once from the App Store and once from Google Play.

# Pushover Messages

Pushover messages consist of three parts at a minimum, but they're highly customizable with additional parameters, including custom titles, sound effects and priorities.

All Pushover messages require, at a minimum, a user key, an application key and a message. You may send additional information such as a custom title, sound effect and priority as well. By default, Pushover will display the application name as the title (based on the application key specified), but you can override this by passing a custom title.

Pushover provides 21 different sound effects, as well as a silent option. If you don't pass a sound effect argument, the user's default sound is played.

Priority allows you to specify whether a message is normal, low or high priority, or an emergency. Emergency notifications don't get delivered faster, but they do require user acknowledgement on the device, and emergency notifications override the "quiet hours" of the Pushover client (so use them sparingly).

Pushover even allows device targeting--if you have several devices linked to the same Pushover account, and therefore user key, you can specify a specific device so the alert is sent to that device only. By default Pushover sends alerts to all your devices.

# Using Pushover.NET

To send a Pushover notification from your application, you need to compile the Pushover.NET project and then set a project reference in your own project to Pushover.dll.

Next, add a using statement to your C# source files where you intend to call the Pushover.NET library as follows:  using DojoNorthSoftware.Pushover;

Once this is done, add the following code to send a Pushover notification. There are many overloads of SendNotification you can use; the example below is the simplest one. If the notification is sent successfully, the method returns true. If it fails, false is returned. An Exception object is also provided--if SendNotification returns false, you can examine the Exception object for more details on what went wrong.

````csharp
Exception except;
bool notify = Pushover.SendNotification("LJNNaBNdqGKaVQeT38V8Y58EjMqA4d", "YOUR_USER_KEY_HERE", "This is my first Pushover notification using Microsoft Visual Studio!", out except);
````

# Test Harness

The Visual Studio solution (imported/tested in VS 2022 Community) includes a test harness Windows Forms app. This allows you to fully test the Pushover.NET solution, including sending messages, including titles, URLs, changing sounds, priority, etc.

I created a default application key, ah4qn2853qakpyh5r4p5yd6dw41knu, which is prepopulated. I just created this on "Twosday" Tuesday 2/22/22, as the old one I had featured on CodePlex is so heavily used it gets maxed out of its 10,000 free monthly messages. I ask that when you use this key, please don't be greedy. Try it out a couple times, then create your own applications. You can create many applications in your own Pushover account, EACH of which come with 10,000 free messages per month. So use mine just for a quick smoke test, then set up your own.

If you get an error code 429, that means the 10,000 free monthly messages have been exceeded.
