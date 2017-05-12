using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoundControllServer
{
    public class Speaker
    {
        public string name { get; set; }
        public bool isDefault { get; set; }
        public string id { get; set; }
        public void SetSpeaker()
        {
            foreach (AudioDevice device in program.audioDeviceList)
            {
                if (device.State.ToString().Equals("Active"))
                {

                    if (device.Description.Equals(this.name))
                    {
                        AudioSwitcher.Audio.AudioDeviceManager adm = new AudioSwitcher.Audio.AudioDeviceManager();
                        adm.SetDefaultAudioDevice(device.Id);
                    }
                }
            }

        }

    }

}
