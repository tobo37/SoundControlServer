using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SoundControllServer
{
    public class program
    {        
        public static ContextMenu menu;
        public static MenuItem mnuExit;
        public static NotifyIcon notificationIcon;
        public static Thread thread;
        public static LinkedList<SoundObject> obj = new LinkedList<SoundObject>();
        public static MasterObject masterObj = new MasterObject();
        public static SendObject sendObj = new SendObject();
        public static LinkedList<IconObject> iconList = new LinkedList<IconObject>();
        public static LinkedList<AudioDevice> audioDeviceList = new LinkedList<AudioDevice>();
        public static LinkedList<Speaker> speakersList = new LinkedList<Speaker>();
        public static int serverVersion;
        public static string antwort;
        public static HttpServer httpServer;
        public static void Main(String[] args)
        {
            Environment.SetEnvironmentVariable("TEMP", "<dir>");
            serverVersion = 5;
            refreshData();
            Thread notifyThread = new Thread(
            delegate ()
            {
                menu = new ContextMenu();
                mnuExit = new MenuItem("Exit");
                menu.MenuItems.Add(0, mnuExit);

                notificationIcon = new NotifyIcon()
                {
                    Icon = Icon.ExtractAssociatedIcon("./SoundControllServer.exe"),
                        //Icon = new Icon(new Icon("../../Symbolbild.ico"), 40, 40),
                        ContextMenu = menu,
                    Text = "Menu"
                };
                mnuExit.Click += new EventHandler(mnuExit_Click);

                notificationIcon.Visible = true;
                Application.Run();
            }
        );
            //Thread broadcasttrhead = new Thread(broadcaster);
            //testapp
            
            httpServer = new MyHttpServer(8080);
            thread = new Thread(new ThreadStart(httpServer.listen));
            thread.Start();
            notifyThread.Start();
        }
        public static void SortSoundObject()
        {
            if (sendObj.soundObjList != null)
            {
                var peopleInOrder = sendObj.soundObjList.OrderBy(x => x.name);
                sendObj.soundObjList = new LinkedList<SoundObject>();
                foreach(var eintrag in peopleInOrder)
                {
                    sendObj.soundObjList.AddLast((SoundObject)eintrag);
                }
            }
            
        }



        static void mnuExit_Click(object sender, EventArgs e)
        {
            //hier soll die app komplett geschlossen werden
            thread.Abort();
            Application.Exit();
            notificationIcon.Dispose();
            
            
            //Process.GetCurrentProcess().Kill();
        }

        public static void refreshData()
        {
            obj = AudioManager.FillObject(); //füllt obj
            
            AudioManager.fillAudioDevices();
            checkDaten(); //korrigiert SoundObjekte
            fillMasterObject();
            deviceToSpeaker();
            sendObj.soundObjList = obj;
            SortSoundObject(); //sortiert Soundobj (obj) nach Namen
            sendObj.masterObj = masterObj;
            sendObj.serverVersion = serverVersion;
            sendObj.speakersList = speakersList;
        }

        public static void deviceToSpeaker()
        {
            AudioDevice defaulDevice = AudioUtilities.GetSpeakersDevice();
            speakersList = new LinkedList<Speaker>();
            foreach (AudioDevice device in program.audioDeviceList)
            {
                if (device.State.ToString().Equals("Active"))
                {
                    if (device.EnumeratorName.Equals("HDAUDIO")){
                        Speaker speakertemp = new Speaker();
                        if (device.Id == defaulDevice.Id)
                        {
                            speakertemp.isDefault = true;
                        }
                        else
                        {
                            speakertemp.isDefault = false;
                        }
                        speakertemp.name = device.Description;
                        speakertemp.id = device.Id;
                        speakersList.AddLast(speakertemp);
                    }
                }
            }
        }

        public static void setSendeObjAsAntwort()
        {
            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            antwort = javaScriptSerializer.Serialize(program.sendObj);
        }
        public static void setIconObjAsAntwort(IconObject iconObj)
        {
            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            antwort = javaScriptSerializer.Serialize(iconObj.getIconString());
            Console.WriteLine(antwort);
        }

        public static void postHandler(string context)
        {
            string[] text = context.Split(',');
            int proID;
            float volume;
            bool mute;
            switch (text[0])
            {
                case "mute":
                    proID = int.Parse(text[1]);
                    mute = (bool)AudioManager.GetApplicationMute(proID);
                    AudioManager.SetApplicationMute(proID, !mute);
                    refreshData();
                    setSendeObjAsAntwort();
                    break;
                case "change":
                    proID = int.Parse(text[1]);
                    volume = float.Parse(text[2]);
                    AudioManager.SetApplicationVolume(proID, volume);
                    refreshData();
                    setSendeObjAsAntwort();
                    break;
                case "master":
                    if (text[1].Equals("mute"))
                    {
                        mute = (bool)AudioManager.GetMasterVolumeMute();
                        AudioManager.SetMasterVolumeMute(!mute);
                        refreshData();
                        setSendeObjAsAntwort();
                    }
                    else if(text[1].Equals("change"))
                    {
                        volume = float.Parse(text[2]);
                        AudioManager.SetMasterVolume(volume);
                        refreshData();
                        setSendeObjAsAntwort();
                    }
                    break;
                case "getIcon":
                    refreshData();
                    IconObject icontemp = new IconObject();
                    icontemp.fillIconByName(text[1]);
                    setIconObjAsAntwort(icontemp);
                    break;
                case "changeAudioDevice":
                    SetDefaultAudioDevic(text[1]);
                    refreshData();
                    setSendeObjAsAntwort();
                    break;

                default:
                    break;
            }
            

        }
      
        public static void fillMasterObject()
        {
            masterObj.volume = (int)AudioManager.GetMasterVolume();
            masterObj.isMute = AudioManager.GetMasterVolumeMute();
        }
        public static void SetDefaultAudioDevic(string id)
        {
            AudioSwitcher.Audio.AudioDeviceManager adm = new AudioSwitcher.Audio.AudioDeviceManager();
            adm.SetDefaultAudioDevice(id);
        }
        public static void checkDaten()
            //entfernt doppelte Spotify eintrage
        {
            LinkedList<SoundObject> ent = new LinkedList<SoundObject>();
            foreach (SoundObject eintrag in obj)
            {
                if (!(eintrag.isEintrag()))
                {
                    ent.AddLast(eintrag);
                }
            }
            foreach (SoundObject eintrag in ent)
            {
                if (!(eintrag.isEintrag()))
                {
                    obj.Remove(eintrag);
                }
            }
        }

        public static IconObject getIconByProcessId(int prodID)
        {
            SoundObject so = new SoundObject();
            foreach(SoundObject eintrag in obj)
            {
                if(eintrag.processId == prodID)
                {
                    so = eintrag;
                    break;
                }
            }
            IconObject io = new IconObject();
            io.fillIconByName(so.name,so.processId);
            return io;
        }
        public static void broadcaster()
        {
            var Server = new UdpClient(8888);
            var ResponseData = Encoding.ASCII.GetBytes("SomeResponseData");

            while (true)
            {
                var ClientEp = new IPEndPoint(IPAddress.Any, 0);
                var ClientRequestData = Server.Receive(ref ClientEp);
                var ClientRequest = Encoding.ASCII.GetString(ClientRequestData);

                Console.WriteLine("Recived {0} from {1}, sending response", ClientRequest, ClientEp.Address.ToString());
                Server.Send(ResponseData, ResponseData.Length, ClientEp);
            }
        }
    }
}
