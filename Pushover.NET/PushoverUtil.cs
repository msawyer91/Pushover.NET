/*
 * This software and source code is licensed under the BSD 2-Clause license. Please see http://opensource.org/licenses/BSD-2-Clause
 * 
 * Copyright (c) 2013-2023 Heathens Haven Hashers, LLC
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
using System.Net.Http;
using System.Text;
using System.Web;
using System.IO;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace DojoNorthSoftware.Pushover
{
    public static partial class Pushover
    {
        #region Constants
        /// <summary>
        /// Constants - changing these is NOT recommended. The Pushover URL should definitely not be changed. If you want to adjust the default retry
        /// and expiration, the retry range is 30-3600 and the expire interval is 60-86400. Using values outside these ranges will result in the
        /// Pushover API returning an error.
        /// </summary>
        private const String pushoverUrl = "https://api.pushover.net/1/messages.json";
        private const int defaultEmergencyRetry = 120;
        private const int defaultEmergencyExpire = 3600;
        private const int maxAttachmentSize = 2621440;
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

        private static Stream GetAttachmentData(String fileName)
        {
            FileStream fs = new FileStream(fileName, FileMode.Open);
            if ((int)fs.Length > maxAttachmentSize)
            {
                throw new ArgumentOutOfRangeException("Attachment size " + fs.Length.ToString() + " exceeds Pushover limit of file size " +
                    maxAttachmentSize.ToString() + "; please select a smaller file.");
            }
            return fs;
        }
        #endregion

        public class PushoverAsyncResult
        {
            Exception ex;
            HttpResponseMessage message;
            bool success;
            bool isComplete;

            public PushoverAsyncResult()
            {
                ex = null;
                message = null;
                success = false;
                isComplete = false;
            }

            public Exception ExceptionObj
            {
                get { return ex; }
                set { ex = value; }
            }

            public HttpResponseMessage ResponseMessage
            {
                get { return message; }
                set { message = value; }
            }

            public bool Success
            {
                get { return success; }
                set { success = value; }
            }

            public bool IsComplete
            {
                get { return isComplete; }
                set { isComplete = value; }
            }
        }
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
        Lowest = -2,
        Low = -1,
        Normal = 0,
        High = 1,
        Emergency = 2
    }
    #endregion
}