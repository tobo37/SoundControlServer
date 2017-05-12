// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
// This source file was altered for use in AudioSwitcher.
/*
  LICENSE
  -------
  Copyright (C) 2007 Ray Molenkamp

  This source code is provided 'as-is', without any express or implied
  warranty.  In no event will the authors be held liable for any damages
  arising from the use of this source code or the software it produces.

  Permission is granted to anyone to use this source code for any purpose,
  including commercial applications, and to alter it and redistribute it
  freely, subject to the following restrictions:

  1. The origin of this source code must not be misrepresented; you must not
     claim that you wrote the original source code.  If you use this source code
     in a product, an acknowledgment in the product documentation would be
     appreciated but is not required.
  2. Altered source versions must be plainly marked as such, and must not be
     misrepresented as being the original source code.
  3. This notice may not be removed or altered from any source distribution.
*/
using System;
using System.ComponentModel.Composition;
using System.Runtime.InteropServices;
using System.Threading;
using AudioSwitcher.Audio.Interop;
using AudioSwitcher.Interop;

namespace AudioSwitcher.Audio
{
    [Export(typeof(AudioDeviceManager))]
    internal class AudioDeviceManager
    {
        public void SetDefaultAudioDevice(AudioDevice device)
        {
            if (device == null)
                throw new ArgumentNullException("device");

            SetDefaultAudioDevice(device, AudioDeviceRole.Multimedia);
            SetDefaultAudioDevice(device, AudioDeviceRole.Communications);
            SetDefaultAudioDevice(device, AudioDeviceRole.Console);
        }

        public void SetDefaultAudioDevice(AudioDevice device, AudioDeviceRole role)
        {
            if (device == null)
                throw new ArgumentNullException("device");

            // BADNESS: The following code uses undocumented interfaces provided by the Audio SDK. This is completely
            // unsupported, and should be used for amusement purposes only. This is *extremely likely* to be broken 
            // in future updates and/or versions of Windows. If Larry Osterman was dead, he would be rolling over 
            // in his grave if he knew you were using this for nefarious purposes.
            var config = new PolicyConfig();

            int hr;
            IPolicyConfig2 config2 = config as IPolicyConfig2;
            if (config2 != null)
            {   // Windows 7 -> Windows 8.1
                hr = config2.SetDefaultEndpoint(device.Id, role);
            }
            else
            {   // Windows 10+
                hr = ((IPolicyConfig3)config).SetDefaultEndpoint(device.Id, role);
            }

            if (hr != HResult.OK)
                throw Marshal.GetExceptionForHR(hr);
        }
#region kompatiblität
        public void SetDefaultAudioDevice(SoundControllServer.AudioDevice device)
        {
            if (device == null)
                throw new ArgumentNullException("device");

            SetDefaultAudioDevice(device, AudioDeviceRole.Multimedia);
            SetDefaultAudioDevice(device, AudioDeviceRole.Communications);
            SetDefaultAudioDevice(device, AudioDeviceRole.Console);
        }
        public void SetDefaultAudioDevice(SoundControllServer.AudioDevice device, AudioDeviceRole role)
        {
            if (device == null)
                throw new ArgumentNullException("device");

            // BADNESS: The following code uses undocumented interfaces provided by the Audio SDK. This is completely
            // unsupported, and should be used for amusement purposes only. This is *extremely likely* to be broken 
            // in future updates and/or versions of Windows. If Larry Osterman was dead, he would be rolling over 
            // in his grave if he knew you were using this for nefarious purposes.
            var config = new PolicyConfig();

            int hr;
            IPolicyConfig2 config2 = config as IPolicyConfig2;
            if (config2 != null)
            {   // Windows 7 -> Windows 8.1
                hr = config2.SetDefaultEndpoint(device.Id, role);
            }
            else
            {   // Windows 10+
                hr = ((IPolicyConfig3)config).SetDefaultEndpoint(device.Id, role);
            }

            if (hr != HResult.OK)
                throw Marshal.GetExceptionForHR(hr);
        }
        public void SetDefaultAudioDevice(string _ID)
        {
            if (_ID == null)
                throw new ArgumentNullException("device");

            SetDefaultAudioDevice(_ID, AudioDeviceRole.Multimedia);
            SetDefaultAudioDevice(_ID, AudioDeviceRole.Communications);
            SetDefaultAudioDevice(_ID, AudioDeviceRole.Console);
        }
        public void SetDefaultAudioDevice(string _ID, AudioDeviceRole role)
        {
            if (_ID == null)
                throw new ArgumentNullException("device");

            // BADNESS: The following code uses undocumented interfaces provided by the Audio SDK. This is completely
            // unsupported, and should be used for amusement purposes only. This is *extremely likely* to be broken 
            // in future updates and/or versions of Windows. If Larry Osterman was dead, he would be rolling over 
            // in his grave if he knew you were using this for nefarious purposes.
            var config = new PolicyConfig();

            int hr;
            IPolicyConfig2 config2 = config as IPolicyConfig2;
            if (config2 != null)
            {   // Windows 7 -> Windows 8.1
                hr = config2.SetDefaultEndpoint(_ID, role);
            }
            else
            {   // Windows 10+
                hr = ((IPolicyConfig3)config).SetDefaultEndpoint(_ID, role);
            }

            if (hr != HResult.OK)
                throw Marshal.GetExceptionForHR(hr);
        }

        #endregion

    }
}
