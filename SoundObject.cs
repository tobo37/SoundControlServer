using System;
using System.IO;
using System.Runtime.InteropServices;

namespace SoundControllServer
{
    public class SoundObject
    {
        public string name { get; set; }
        public int processId { get; set; }
        public string iconPath { get; set; }
        //public Guid groupingParam { get; set; }
        public string sessionIdentifier { get; set; }
        //public string sessionInstanceIdentifier { get; set; }
        public int volume { get; set; }
        public bool isMute { get; set; }
        public string bild { get; set; }


        private void selectIdentifier()
        {
            if (this.processId == 0)
            {
                this.name = "systemsound";
            }
            else
            {
                //{0.0.0.00000000}.{4cb244e9-f7fa-481b-89c0-ada664b8a27d}|\Device\HarddiskVolume2\Users\Gerrit\AppData\Roaming\Spotify\Spotify.exe%b{00000000-0000-0000-0000-000000000000}
                string[] text = this.sessionIdentifier.Split('|');// => \Device\HarddiskVolume2\Users\Gerrit\AppData\Roaming\Spotify\Spotify.exe % b{ 00000000 - 0000 - 0000 - 0000 - 000000000000}
                text = text[1].Split('%');
                string pathnew = Path.GetFileNameWithoutExtension(text[0]);
                if (this.name == "")
                {
                    this.name = pathnew.ToLower();
                }
            }
        }
        public bool isEintrag()
        {
            return (!(this.name.Contains("#")));
        }
    }
}