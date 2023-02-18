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
        #region Public Methods - Async
        public static async Task<PushoverAsyncResult> SendNotificationAsync(String appToken, String userKey, String title, String message, Priority pri, PushoverSound sound, String device,
            String supplementaryUrl, String urlTitle, String fileName, int emergencyRetry, int emergencyExpire)
        {
            // PushoverAsyncResult keeps track of our async function call results.
            PushoverAsyncResult result = new PushoverAsyncResult();

            // Ensure the message is 512 characters or less, and convert the PushoverSound and Priority enums to their string representations.
            // The Pushover API expects strings.
            String prunedMessage = PruneLongMessage(message);
            String soundName = GetPushoverSound(sound);
            int priority = GetPushoverPriority(pri);

            using (HttpClient client = new HttpClient())
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                MultipartFormDataContent content = new MultipartFormDataContent();
                content.Add(new StringContent(appToken), "\"token\"");
                content.Add(new StringContent(userKey), "\"user\"");
                content.Add(new StringContent(prunedMessage), "\"message\"");
                content.Add(new StringContent(priority.ToString()), "\"priority\"");
                if (sound != PushoverSound.DeviceDefault)
                    content.Add(new StringContent(soundName), "\"sound\"");

                // Include the title, device, url and url_title values in the NameValueCollection if valid data was specified.
                // Do not include if they are null or empty.
                if (!String.IsNullOrEmpty(title))
                    content.Add(new StringContent(title), "\"title\"");
                if (!String.IsNullOrEmpty(device))
                    content.Add(new StringContent(device), "\"device\"");
                if (!String.IsNullOrEmpty(supplementaryUrl))
                    content.Add(new StringContent(supplementaryUrl), "\"url\"");
                if (!String.IsNullOrEmpty(urlTitle))
                    content.Add(new StringContent(urlTitle), "\"url_title\"");

                // Include the attachment, if one was requested.
                if (!String.IsNullOrEmpty(fileName))
                {
                    StreamContent sc = new StreamContent(GetAttachmentData(fileName));
                    sc.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
                    content.Add(sc, "\"attachment\"", "image.jpeg");
                }

                // Make sure the emergency retry and expiration values are within valid ranges. If invalid values are specified, we correct
                // them to their defaults.
                if (emergencyRetry < 30 || emergencyRetry > 3600)
                    emergencyRetry = defaultEmergencyRetry;
                if (emergencyExpire < 60 || emergencyExpire > 86400)
                    emergencyExpire = defaultEmergencyExpire;

                // Emergency priority requires the retry and expire parameters. These should not be specified for other messages.
                if (pri == Priority.Emergency)
                {
                    content.Add(new StringContent(emergencyRetry.ToString()), "\"retry\"");
                    content.Add(new StringContent(emergencyExpire.ToString()), "\"expire\"");
                }

                // It is invalid to specify a supplementary URL title but not a URL. Set an argument exception and return false.
                if (String.IsNullOrEmpty(supplementaryUrl) && !string.IsNullOrEmpty(urlTitle))
                {
                    result.ExceptionObj = new ArgumentException("If a URL title is specified, a URL must also be specified.");
                    result.Success = false;
                }

                // Clean up any headers we don't need
                foreach (var param in content)
                {
                    param.Headers.ContentType = null;
                }

                // Try executing the async call.
                HttpResponseMessage responseMessage;
                try
                {
                    responseMessage = await client.PostAsync(pushoverUrl, content);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        result.ResponseMessage = responseMessage;
                        result.Success = true;
                        result.IsComplete = true;
                        return result;
                    }
                }
                catch (Exception ex)
                {
                    // Something went wrong. Capture the error details and return false.
                    result.ExceptionObj = ex;
                    result.Success = false;
                    result.IsComplete = true;
                    return result;
                }
                // The call completed but we didn't get success - return false.
                result.ResponseMessage = responseMessage;
                result.Success = false;
                result.IsComplete = true;
                return result;
            }
        }
        #endregion
    }
}