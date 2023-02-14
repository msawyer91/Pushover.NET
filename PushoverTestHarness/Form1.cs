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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using DojoNorthSoftware.Pushover;

namespace PushoverTestHarness
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBoxPriority.SelectedIndex = 0;
            comboBoxSound.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (ValidateFields())
            {
                Exception except;
                bool result = Pushover.SendNotification(textBoxAppKey.Text, textBoxUserKey.Text, textBoxTitle.Text, textBoxMessage.Text, ResolvePriorityFromString(),
                    ResolveSoundFromString(), String.Empty, textBoxUrl.Text, textBoxUrlTitle.Text, (int)numericUpDownRetry.Value, (int)numericUpDownExpire.Value, out except);
                if (result)
                {
                    MessageBox.Show("Your message was sent to Pushover successfully. Please check your device(s) to confirm receipt.", "Success", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    if (except == null)
                    {
                        MessageBox.Show("Your message was not sent successfully. Unfortunately, Pushover did not return an error with details.", "Severe",
                            MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    }
                    else
                    {
                        MessageBox.Show("Your message was not sent successfully. An error was detected with the message: " + except.Message, "Severe",
                            MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool ValidateFields()
        {
            if (String.IsNullOrEmpty(textBoxAppKey.Text))
            {
                MessageBox.Show("Application Key is a mandatory field.", "Data Missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            if (String.IsNullOrEmpty(textBoxUserKey.Text))
            {
                MessageBox.Show("User Key is a mandatory field.", "Data Missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            if (String.IsNullOrEmpty(textBoxMessage.Text))
            {
                MessageBox.Show("Message Body is a mandatory field.", "Data Missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            if (String.IsNullOrEmpty(textBoxUrl.Text) && !String.IsNullOrEmpty(textBoxUrlTitle.Text))
            {
                MessageBox.Show("URL is mandatory if a URL title is specified.", "Data Missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            
            return true;
        }

        private PushoverSound ResolveSoundFromString()
        {
            switch (comboBoxSound.SelectedItem.ToString())
            {
                case "(Device default)":
                    {
                        return PushoverSound.DeviceDefault;
                    }
                case "Persistent (long)":
                    {
                        return PushoverSound.Persistent;
                    }
                case "Pushover (default)":
                default:
                    {
                        return PushoverSound.Pushover;
                    }
            }
        }

        private Priority ResolvePriorityFromString()
        {
            switch (comboBoxPriority.SelectedItem.ToString())
            {
                case "Emergency":
                    {
                        return Priority.Emergency;
                    }
                case "High":
                    {
                        return Priority.High;
                    }
                case "Low":
                    {
                        return Priority.Low;
                    }
                case "Normal":
                default:
                    {
                        return Priority.Normal;
                    }
            }
        }
    }
}
