/*
 * This software and source code is licensed under the BSD 2-Clause license. Please see http://opensource.org/licenses/BSD-2-Clause
 * 
 * Copyright (c) 2013 Dojo North Software, LLC
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
 *
 * - Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
 * - Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer
 *   in the documentation and/or other materials provided with the distribution.
 * - THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING,
 *   BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT
 *   SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
 *   DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
 *   INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE
 *   OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 *
 */

using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace DojoNorthSoftware.Pushover
{
    public static class Pushover
    {
        /// <summary>
        /// Constants - changing these is NOT recommended. The Pushover URL should definitely not be changed. If you want to adjust the default retry
        /// and expiration, the retry range is 30-3600 and the expire interval is 60-86400. Using values outside these ranges will result in the
        /// Pushover API returning an error.
        /// </summary>
        private const String pushoverUrl = "https://api.pushover.net/1/messages.json";
        private const int defaultEmergencyRetry = 120;
        private const int defaultEmergencyExpire = 3600;

        #region Public Methods
        /// <summary>
        /// Sends a Pushover notification to mobile devices.
        /// </summary>
        /// <param name="appToken">The application token assigned by Pushover. This parameter is mandatory for all overloads.</param>
        /// <param name="userKey">The user's Pushover API key. This parameter is mandatory for all overloads.</param>
        /// <param name="title">Title of the alert, if you want to display something other than the app's title.</param>
        /// <param name="message">The message body. Messages are limited to 512 characters; longer messages will be trimmed.</param>
        /// <param name="pri">The Pushover priority level. Values are normal, high, low and emergency.</param>
        /// <param name="sound">The Pushover sound that is played to the user.</param>
        /// <param name="device">To send the message to a specific named device, specify the device name. Otherwise send String.Empty.</param>
        /// <param name="supplementaryUrl">A supplementary URL to show with your message.</param>
        /// <param name="urlTitle">An optional title to show with your supplemental URL. If specified, a valid URL must be supplied for supplementalUrl.</param>
        /// <param name="emergencyRetry">If sending with Priority.Emergency, a value in the range of 30-3600 seconds should be specified
        /// as the retry interval if the user doesn't acknowledge the alert. Default value is 120.</param>
        /// <param name="emergencyExpire">If sending with Priority.Emergency, a value in the range of 60-86400 seconds should be specified as the expiration time limit
        /// if the user doesn't acknowledge the alert. Default value is 3600 (10 minutes).</param>
        /// <param name="except">If an error sending the notification occurs, this parameter allows you to capture the reason for the failure.</param>
        /// <returns>true if the notification was sent successfully; false if the notification failed. Examine the "except" output parameter if notification fails.</returns>
        public static bool SendNotification(String appToken, String userKey, String title, String message, Priority pri, PushoverSound sound, String device,
            String supplementaryUrl, String urlTitle, int emergencyRetry, int emergencyExpire, out Exception except)
        {
            // The Exception object is used only if an exception occurs. However, it must be initialized or the code won't compile.
            // Initializing to null is perfectly OK.
            except = null;

            // Ensure the message is 512 characters or less, and convert the PushoverSound and Priority enums to their string representations.
            // The Pushover API expects strings.
            String prunedMessage = PruneLongMessage(message);
            String soundName = GetPushoverSound(sound);
            int priority = GetPushoverPriority(pri);

            // A NameValueCollection is used to load up the parameters expected by Pushover. The API specifies that token, user and message are mandatory.
            // We will also specify priority and, if a sound other than "device default" (the user's device setting) is specified, we include that as well.
            NameValueCollection parameters;
            if (sound == PushoverSound.DeviceDefault)
            {
                parameters = new NameValueCollection { { "token", appToken }, { "user", userKey }, { "message", prunedMessage },
                    { "priority", priority.ToString() } };
            }
            else
            {
                parameters = new NameValueCollection { { "token", appToken }, { "user", userKey }, { "message", prunedMessage },
                    { "priority", priority.ToString() }, {"sound", soundName } };
            }

            // Include the title, device, url and url_title values in the NameValueCollection if valid data was specified.
            // Do not include if they are null or empty.
            if (!String.IsNullOrEmpty(title))
                parameters.Add("title", title);
            if (!String.IsNullOrEmpty(device))
                parameters.Add("device", device);
            if (!String.IsNullOrEmpty(supplementaryUrl))
                parameters.Add("url", supplementaryUrl);
            if (!String.IsNullOrEmpty(urlTitle))
                parameters.Add("url_title", urlTitle);
            
            // Make sure the emergency retry and expiration values are within valid ranges. If invalid values are specified, we correct
            // them to their defaults.
            if (emergencyRetry < 30 || emergencyRetry > 3600)
                emergencyRetry = defaultEmergencyRetry;
            if (emergencyExpire < 60 || emergencyExpire > 86400)
                emergencyExpire = defaultEmergencyExpire;
            
            // Emergency priority requires the retry and expire parameters. These should not be specified for other messages.
            if (pri == Priority.Emergency)
            {
                parameters.Add("retry", emergencyRetry.ToString());
                parameters.Add("expire", emergencyExpire.ToString());
            }

            // It is invalid to specify a supplementary URL title but not a URL. Set an argument exception and return false.
            if (String.IsNullOrEmpty(supplementaryUrl) && !string.IsNullOrEmpty(urlTitle))
            {
                except = new ArgumentException("If a URL title is specified, a URL must also be specified.");
                return false;
            }

            // Try creating the System.Net.WebClient object and making the JSON call to Pushover. We use a "using" statement
            // to ensure the WebClient is disposed after the call.
            try
            {
                using (System.Net.WebClient client = new System.Net.WebClient())
                {
                    client.UploadValues(pushoverUrl, parameters);
                }
            }
            catch (Exception ex)
            {
                // Something went wrong. Capture the error details and return false.
                except = ex;
                return false;
            }
            // The call completed successfully - return true.
            return true;
        }

        /// <summary>
        /// Sends a Pushover notification to mobile devices.
        /// </summary>
        /// <param name="appToken">The application token assigned by Pushover. This parameter is mandatory for all overloads.</param>
        /// <param name="userKey">The user's Pushover API key. This parameter is mandatory for all overloads.</param>
        /// <param name="title">Title of the alert, if you want to display something other than the app's title.</param>
        /// <param name="message">The message body. Messages are limited to 512 characters; longer messages will be trimmed.</param>
        /// <param name="pri">The Pushover priority level. Values are normal, high, low and emergency.</param>
        /// <param name="sound">The Pushover sound that is played to the user.</param>
        /// <param name="device">To send the message to a specific named device, specify the device name. Otherwise send String.Empty.</param>
        /// <param name="supplementaryUrl">A supplementary URL to show with your message.</param>
        /// <param name="urlTitle">An optional title to show with your supplemental URL. If specified, a valid URL must be supplied for supplementalUrl.</param>
        /// <param name="except">If an error sending the notification occurs, this parameter allows you to capture the reason for the failure.</param>
        /// <returns>true if the notification was sent successfully; false if the notification failed. Examine the "except" output parameter if notification fails.</returns>
        public static bool SendNotification(String appToken, String userKey, String title, String message, Priority pri, PushoverSound sound, String device,
            String supplementaryUrl, String urlTitle, out Exception except)
        {
            return SendNotification(appToken, userKey, title, message, pri, sound, device, supplementaryUrl, urlTitle, defaultEmergencyRetry,
                defaultEmergencyExpire, out except);
        }

        /// <summary>
        /// Sends a Pushover notification to mobile devices.
        /// </summary>
        /// <param name="appToken">The application token assigned by Pushover. This parameter is mandatory for all overloads.</param>
        /// <param name="userKey">The user's Pushover API key. This parameter is mandatory for all overloads.</param>
        /// <param name="title">Title of the alert, if you want to display something other than the app's title.</param>
        /// <param name="message">The message body. Messages are limited to 512 characters; longer messages will be trimmed.</param>
        /// <param name="pri">The Pushover priority level. Values are normal, high, low and emergency.</param>
        /// <param name="sound">The Pushover sound that is played to the user.</param>
        /// <param name="device">To send the message to a specific named device, specify the device name. Otherwise send String.Empty.</param>
        /// <param name="emergencyRetry">If sending with Priority.Emergency, a value in the range of 30-3600 seconds should be specified
        /// as the retry interval if the user doesn't acknowledge the alert. Default value is 120.</param>
        /// <param name="emergencyExpire">If sending with Priority.Emergency, a value in the range of 60-86400 seconds should be specified as the expiration time limit
        /// if the user doesn't acknowledge the alert. Default value is 3600 (10 minutes).</param>
        /// <param name="except">If an error sending the notification occurs, this parameter allows you to capture the reason for the failure.</param>
        /// <returns>true if the notification was sent successfully; false if the notification failed. Examine the "except" output parameter if notification fails.</returns>
        public static bool SendNotification(String appToken, String userKey, String title, String message, Priority pri, PushoverSound sound, String device,
            int emergencyRetry, int emergencyExpire, out Exception except)
        {
            return SendNotification(appToken, userKey, title, message, pri, sound, device, String.Empty, String.Empty, emergencyRetry,
                emergencyExpire, out except);
        }

        /// <summary>
        /// Sends a Pushover notification to mobile devices.
        /// </summary>
        /// <param name="appToken">The application token assigned by Pushover. This parameter is mandatory for all overloads.</param>
        /// <param name="userKey">The user's Pushover API key. This parameter is mandatory for all overloads.</param>
        /// <param name="title">Title of the alert, if you want to display something other than the app's title.</param>
        /// <param name="message">The message body. Messages are limited to 512 characters; longer messages will be trimmed.</param>
        /// <param name="pri">The Pushover priority level. Values are normal, high, low and emergency.</param>
        /// <param name="sound">The Pushover sound that is played to the user.</param>
        /// <param name="device">To send the message to a specific named device, specify the device name. Otherwise send String.Empty.</param>
        /// <param name="supplementaryUrl">A supplementary URL to show with your message.</param>
        /// <param name="emergencyRetry">If sending with Priority.Emergency, a value in the range of 30-3600 seconds should be specified
        /// as the retry interval if the user doesn't acknowledge the alert. Default value is 120.</param>
        /// <param name="emergencyExpire">If sending with Priority.Emergency, a value in the range of 60-86400 seconds should be specified as the expiration time limit
        /// if the user doesn't acknowledge the alert. Default value is 3600 (10 minutes).</param>
        /// <param name="except">If an error sending the notification occurs, this parameter allows you to capture the reason for the failure.</param>
        /// <returns>true if the notification was sent successfully; false if the notification failed. Examine the "except" output parameter if notification fails.</returns>
        public static bool SendNotification(String appToken, String userKey, String title, String message, Priority pri, PushoverSound sound, String device,
            String supplementaryUrl, int emergencyRetry, int emergencyExpire, out Exception except)
        {
            return SendNotification(appToken, userKey, title, message, pri, sound, device, supplementaryUrl, String.Empty, emergencyRetry,
                emergencyExpire, out except);
        }

        /// <summary>
        /// Sends a Pushover notification to mobile devices.
        /// </summary>
        /// <param name="appToken">The application token assigned by Pushover. This parameter is mandatory for all overloads.</param>
        /// <param name="userKey">The user's Pushover API key. This parameter is mandatory for all overloads.</param>
        /// <param name="title">Title of the alert, if you want to display something other than the app's title.</param>
        /// <param name="message">The message body. Messages are limited to 512 characters; longer messages will be trimmed.</param>
        /// <param name="pri">The Pushover priority level. Values are normal, high, low and emergency.</param>
        /// <param name="sound">The Pushover sound that is played to the user.</param>
        /// <param name="device">To send the message to a specific named device, specify the device name. Otherwise send String.Empty.</param>
        /// <param name="supplementaryUrl">A supplementary URL to show with your message.</param>
        /// <param name="except">If an error sending the notification occurs, this parameter allows you to capture the reason for the failure.</param>
        /// <returns>true if the notification was sent successfully; false if the notification failed. Examine the "except" output parameter if notification fails.</returns>
        public static bool SendNotification(String appToken, String userKey, String title, String message, Priority pri, PushoverSound sound, String device,
            String supplementaryUrl, out Exception except)
        {
            return SendNotification(appToken, userKey, title, message, pri, sound, device, supplementaryUrl, String.Empty, defaultEmergencyRetry,
                defaultEmergencyExpire, out except);
        }

        /// <summary>
        /// Sends a Pushover notification to mobile devices.
        /// </summary>
        /// <param name="appToken">The application token assigned by Pushover. This parameter is mandatory for all overloads.</param>
        /// <param name="userKey">The user's Pushover API key. This parameter is mandatory for all overloads.</param>
        /// <param name="title">Title of the alert, if you want to display something other than the app's title.</param>
        /// <param name="message">The message body. Messages are limited to 512 characters; longer messages will be trimmed.</param>
        /// <param name="pri">The Pushover priority level. Values are normal, high, low and emergency.</param>
        /// <param name="sound">The Pushover sound that is played to the user.</param>
        /// <param name="device">To send the message to a specific named device, specify the device name. Otherwise send String.Empty.</param>
        /// <param name="except">If an error sending the notification occurs, this parameter allows you to capture the reason for the failure.</param>
        /// <returns>true if the notification was sent successfully; false if the notification failed. Examine the "except" output parameter if notification fails.</returns>
        public static bool SendNotification(String appToken, String userKey, String title, String message, Priority pri, PushoverSound sound, String device,
            out Exception except)
        {
            return SendNotification(appToken, userKey, title, message, pri, sound, device, String.Empty, String.Empty, defaultEmergencyRetry,
                defaultEmergencyExpire, out except);
        }

        /// <summary>
        /// Sends a Pushover notification to mobile devices.
        /// </summary>
        /// <param name="appToken">The application token assigned by Pushover. This parameter is mandatory for all overloads.</param>
        /// <param name="userKey">The user's Pushover API key. This parameter is mandatory for all overloads.</param>
        /// <param name="title">Title of the alert, if you want to display something other than the app's title.</param>
        /// <param name="message">The message body. Messages are limited to 512 characters; longer messages will be trimmed.</param>
        /// <param name="pri">The Pushover priority level. Values are normal, high, low and emergency.</param>
        /// <param name="sound">The Pushover sound that is played to the user.</param>
        /// <param name="except">If an error sending the notification occurs, this parameter allows you to capture the reason for the failure.</param>
        /// <returns>true if the notification was sent successfully; false if the notification failed. Examine the "except" output parameter if notification fails.</returns>
        public static bool SendNotification(String appToken, String userKey, String title, String message, Priority pri, PushoverSound sound, out Exception except)
        {
            return SendNotification(appToken, userKey, title, message, pri, sound, String.Empty, String.Empty, String.Empty, defaultEmergencyRetry,
                defaultEmergencyExpire, out except);
        }

        /// <summary>
        /// Sends a Pushover notification to mobile devices.
        /// </summary>
        /// <param name="appToken">The application token assigned by Pushover. This parameter is mandatory for all overloads.</param>
        /// <param name="userKey">The user's Pushover API key. This parameter is mandatory for all overloads.</param>
        /// <param name="title">Title of the alert, if you want to display something other than the app's title.</param>
        /// <param name="message">The message body. Messages are limited to 512 characters; longer messages will be trimmed.</param>
        /// <param name="pri">The Pushover priority level. Values are normal, high, low and emergency.</param>
        /// <param name="except">If an error sending the notification occurs, this parameter allows you to capture the reason for the failure.</param>
        /// <returns>true if the notification was sent successfully; false if the notification failed. Examine the "except" output parameter if notification fails.</returns>
        public static bool SendNotification(String appToken, String userKey, String title, String message, Priority pri, out Exception except)
        {
            return SendNotification(appToken, userKey, title, message, pri, PushoverSound.DeviceDefault, String.Empty, String.Empty,
                String.Empty, defaultEmergencyRetry, defaultEmergencyExpire, out except);
        }

        /// <summary>
        /// Sends a Pushover notification to mobile devices.
        /// </summary>
        /// <param name="appToken">The application token assigned by Pushover. This parameter is mandatory for all overloads.</param>
        /// <param name="userKey">The user's Pushover API key. This parameter is mandatory for all overloads.</param>
        /// <param name="title">Title of the alert, if you want to display something other than the app's title.</param>
        /// <param name="message">The message body. Messages are limited to 512 characters; longer messages will be trimmed.</param>
        /// <param name="sound">The Pushover sound that is played to the user.</param>
        /// <param name="except">If an error sending the notification occurs, this parameter allows you to capture the reason for the failure.</param>
        /// <returns>true if the notification was sent successfully; false if the notification failed. Examine the "except" output parameter if notification fails.</returns>
        public static bool SendNotification(String appToken, String userKey, String title, String message, PushoverSound sound, out Exception except)
        {
            return SendNotification(appToken, userKey, title, message, Priority.Normal, sound, String.Empty, String.Empty, String.Empty, defaultEmergencyRetry,
                defaultEmergencyExpire, out except);
        }

        /// <summary>
        /// Sends a Pushover notification to mobile devices.
        /// </summary>
        /// <param name="appToken">The application token assigned by Pushover. This parameter is mandatory for all overloads.</param>
        /// <param name="userKey">The user's Pushover API key. This parameter is mandatory for all overloads.</param>
        /// <param name="message">The message body. Messages are limited to 512 characters; longer messages will be trimmed.</param>
        /// <param name="pri">The Pushover priority level. Values are normal, high, low and emergency.</param>
        /// <param name="sound">The Pushover sound that is played to the user.</param>
        /// <param name="except">If an error sending the notification occurs, this parameter allows you to capture the reason for the failure.</param>
        /// <returns>true if the notification was sent successfully; false if the notification failed. Examine the "except" output parameter if notification fails.</returns>
        public static bool SendNotification(String appToken, String userKey, String message, Priority pri, PushoverSound sound, out Exception except)
        {
            return SendNotification(appToken, userKey, String.Empty, message, pri, sound, String.Empty, String.Empty, String.Empty, defaultEmergencyRetry,
                defaultEmergencyExpire, out except);
        }

        /// <summary>
        /// Sends a Pushover notification to mobile devices.
        /// </summary>
        /// <param name="appToken">The application token assigned by Pushover. This parameter is mandatory for all overloads.</param>
        /// <param name="userKey">The user's Pushover API key. This parameter is mandatory for all overloads.</param>
        /// <param name="message">The message body. Messages are limited to 512 characters; longer messages will be trimmed.</param>
        /// <param name="pri">The Pushover priority level. Values are normal, high, low and emergency.</param>
        /// <param name="except">If an error sending the notification occurs, this parameter allows you to capture the reason for the failure.</param>
        /// <returns>true if the notification was sent successfully; false if the notification failed. Examine the "except" output parameter if notification fails.</returns>
        public static bool SendNotification(String appToken, String userKey, String message, Priority pri, out Exception except)
        {
            return SendNotification(appToken, userKey, String.Empty, message, pri, PushoverSound.DeviceDefault, String.Empty, String.Empty, String.Empty,
                defaultEmergencyRetry, defaultEmergencyExpire, out except);
        }

        /// <summary>
        /// Sends a Pushover notification to mobile devices.
        /// </summary>
        /// <param name="appToken">The application token assigned by Pushover. This parameter is mandatory for all overloads.</param>
        /// <param name="userKey">The user's Pushover API key. This parameter is mandatory for all overloads.</param>
        /// <param name="message">The message body. Messages are limited to 512 characters; longer messages will be trimmed.</param>
        /// <param name="sound">The Pushover sound that is played to the user.</param>
        /// <param name="except">If an error sending the notification occurs, this parameter allows you to capture the reason for the failure.</param>
        /// <returns>true if the notification was sent successfully; false if the notification failed. Examine the "except" output parameter if notification fails.</returns>
        public static bool SendNotification(String appToken, String userKey, String message, PushoverSound sound, out Exception except)
        {
            return SendNotification(appToken, userKey, String.Empty, message, Priority.Normal, sound, String.Empty, String.Empty, String.Empty,
                defaultEmergencyRetry, defaultEmergencyExpire, out except);
        }

        /// <summary>
        /// Sends a Pushover notification to mobile devices.
        /// </summary>
        /// <param name="appToken">The application token assigned by Pushover. This parameter is mandatory for all overloads.</param>
        /// <param name="userKey">The user's Pushover API key. This parameter is mandatory for all overloads.</param>
        /// <param name="title">Title of the alert, if you want to display something other than the app's title.</param>
        /// <param name="message">The message body. Messages are limited to 512 characters; longer messages will be trimmed.</param>
        /// <param name="except">If an error sending the notification occurs, this parameter allows you to capture the reason for the failure.</param>
        /// <returns>true if the notification was sent successfully; false if the notification failed. Examine the "except" output parameter if notification fails.</returns>
        public static bool SendNotification(String appToken, String userKey, String title, String message, out Exception except)
        {
            return SendNotification(appToken, userKey, title, message, Priority.Normal, PushoverSound.DeviceDefault, String.Empty, String.Empty,
                String.Empty, defaultEmergencyRetry, defaultEmergencyExpire, out except);
        }

        /// <summary>
        /// Sends a Pushover notification to mobile devices.
        /// </summary>
        /// <param name="appToken">The application token assigned by Pushover. This parameter is mandatory for all overloads.</param>
        /// <param name="userKey">The user's Pushover API key. This parameter is mandatory for all overloads.</param>
        /// <param name="message">The message body. Messages are limited to 512 characters; longer messages will be trimmed.</param>
        /// <param name="except">If an error sending the notification occurs, this parameter allows you to capture the reason for the failure.</param>
        /// <returns>true if the notification was sent successfully; false if the notification failed. Examine the "except" output parameter if notification fails.</returns>
        public static bool SendNotification(String appToken, String userKey, String message, out Exception except)
        {
            return SendNotification(appToken, userKey, String.Empty, message, Priority.Normal, PushoverSound.DeviceDefault, String.Empty, String.Empty,
                String.Empty, defaultEmergencyRetry, defaultEmergencyExpire, out except);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Prunes messages longer than 512 characters to 512 characters, the longest supported by Pushover.
        /// </summary>
        /// <param name="message">Message to prune to 512 characters.</param>
        /// <returns>The pruned message. The whole message is returned if it is 512 characters or shorter.</returns>
        private static String PruneLongMessage(String message)
        {
            if (message.Length > 512)
            {
                return message.Substring(0, 512);
            }
            return message;
        }

        /// <summary>
        /// Gets the lowercase string required by the Pushover API for the sound name.
        /// </summary>
        /// <param name="sound">PushoverSound enum.</param>
        /// <returns>String representing the sound name expected by the Pushover API.</returns>
        private static String GetPushoverSound(PushoverSound sound)
        {
            switch (sound)
            {
                case PushoverSound.Alien:
                    {
                        return "alien";
                    }
                case PushoverSound.Bike:
                    {
                        return "bike";
                    }
                case PushoverSound.Bugle:
                    {
                        return "bugle";
                    }
                case PushoverSound.CashRegister:
                    {
                        return "cashregister";
                    }
                case PushoverSound.Classical:
                    {
                        return "classical";
                    }
                case PushoverSound.Climb:
                    {
                        return "climb";
                    }
                case PushoverSound.Cosmic:
                    {
                        return "cosmic";
                    }
                case PushoverSound.Echo:
                    {
                        return "echo";
                    }
                case PushoverSound.Falling:
                    {
                        return "falling";
                    }
                case PushoverSound.Gamelan:
                    {
                        return "gamelan";
                    }
                case PushoverSound.Incoming:
                    {
                        return "incoming";
                    }
                case PushoverSound.Intermission:
                    {
                        return "intermission";
                    }
                case PushoverSound.Magic:
                    {
                        return "magic";
                    }
                case PushoverSound.Mechanical:
                    {
                        return "mechanical";
                    }
                case PushoverSound.None:
                    {
                        return "none";
                    }
                case PushoverSound.Persistent:
                    {
                        return "persistent";
                    }
                case PushoverSound.PianoBar:
                    {
                        return "pianobar";
                    }
                case PushoverSound.Siren:
                    {
                        return "siren";
                    }
                case PushoverSound.SpaceAlarm:
                    {
                        return "spacealarm";
                    }
                case PushoverSound.TugBoat:
                    {
                        return "tugboat";
                    }
                case PushoverSound.UpDown:
                    {
                        return "updown";
                    }
                case PushoverSound.DeviceDefault:
                    {
                        return String.Empty;
                    }
                case PushoverSound.Pushover:
                default:
                    {
                        return "pushover";
                    }
            }
        }

        /// <summary>
        /// Gets the integer value of the Pushover priority.
        /// </summary>
        /// <param name="pri">Priority enum.</param>
        /// <returns>0 = normal, 1 = high, 2 = emergency, -1 = low</returns>
        private static int GetPushoverPriority(Priority pri)
        {
            switch (pri)
            {
                case Priority.Emergency:
                    {
                        return 2;
                    }
                case Priority.High:
                    {
                        return 1;
                    }
                case Priority.Low:
                    {
                        return -1;
                    }
                case Priority.Normal:
                default:
                    {
                        return 0;
                    }
            }
        }
        #endregion
    }

    #region Enums
    /// <summary>
    /// Provides a simplified way to work with the Pushover sound names, reducing the likelihood of creating a typo
    /// in a sound name in your code.
    /// </summary>
    public enum PushoverSound
    {
        Pushover = 0,
        Bike = 1,
        Bugle = 2,
        CashRegister = 3,
        Classical = 4,
        Cosmic = 5,
        Falling = 6,
        Gamelan = 7,
        Incoming = 8,
        Intermission = 9,
        Magic = 10,
        Mechanical = 11,
        PianoBar = 12,
        Siren = 13,
        SpaceAlarm = 14,
        TugBoat = 15,
        Alien = 16,
        Climb = 17,
        Persistent = 18,
        Echo = 19,
        UpDown = 20,
        None = 21,
        DeviceDefault = 22
    }

    /// <summary>
    /// Provides a simplified way to work with the Pushover priority levels.
    /// </summary>
    public enum Priority
    {
        Normal = 0,
        Low = 1,
        High = 2,
        Emergency = 3
    }
#endregion
}
