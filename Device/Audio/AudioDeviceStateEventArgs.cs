﻿// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
using System;

namespace AudioSwitcher.Audio
{
    internal class AudioDeviceStateEventArgs : AudioDeviceEventArgs
    {
        private readonly AudioDeviceState _newState;

        public AudioDeviceStateEventArgs(AudioDevice device, AudioDeviceState newState)
            : base(device)
        {
            _newState = newState;
        }

        public AudioDeviceState NewState
        {
            get { return _newState; }
        }
    }
}
