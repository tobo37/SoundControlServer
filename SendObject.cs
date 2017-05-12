using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoundControllServer
{
    public class SendObject
    {
        public LinkedList<SoundObject> soundObjList { get; set; }
        public MasterObject masterObj { get; set; }
        public int serverVersion { get; set; }
        public LinkedList<Speaker> speakersList { get; set; }
        //public LinkedList<IconObject> iconList { get; set;}
    }
}
