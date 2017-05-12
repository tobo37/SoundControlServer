using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;

namespace SoundControllServer
{
    public class IconObject
    {
        public static String iconString { get; set; }
        //static int processId { get; set; }

        public void fillIconByName(string name, int processIdEingang=1)
        {
            //name = namewandler(name);
            using (MemoryStream ms = new MemoryStream())
            {
                try
                {
                    var bImage = Icon.ExtractAssociatedIcon(name).ToBitmap();
                    bImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    iconString = Convert.ToBase64String(ms.GetBuffer()); //Get Base64
                }
                catch { iconString = null; }
            }

            //processId = processIdEingang;
        }

        private string namewandler(string name)
        {
            switch (name)
            {
                case "chrome":
                    name = "Google Chrome";
                    break;
                default:
                    break;

            }

            return name;
        }
        private string getPathbyProcessName(string name)
        {
            Process[] processes = Process.GetProcessesByName(name);

            if (processes.Length > 0)
            {
                return processes[0].MainModule.FileName;
            }
            else
            {
                return string.Empty;
            }
        }
        public string getIconString()
        {
            return iconString;
        }

        private string wandleHarddicsinLetter(string pfad)
        {
            string[] pfadarray = pfad.Split('\\');
            pfad = "C:\\";
            for(int i = 3; i < pfadarray.Length; i++)
            {
                pfad += "\\"+pfadarray[i];
            }
            return pfad;
        }
    }
}
