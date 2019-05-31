using System;
using System.ComponentModel;
using System.IO;
using System.Threading;

using System.Windows.Forms;
using System.Drawing;


namespace ChurchPlayer
{
    class VideoPlayer : Form
    {
        private Vlc.DotNet.Forms.VlcControl vlcControl;
        private Panel panel;
        private Rectangle resolution, resScreen2;
        private Screen[] displayList;
        private int result;
        private int windowID;

        public VideoPlayer(int windowID)
        {
            this.windowID = windowID;

            resolution = Screen.PrimaryScreen.Bounds;
            displayList = Screen.AllScreens;
            // Console.WriteLine(displayList.Length);

            //fullscreen options:

            this.FormBorderStyle = FormBorderStyle.None; 
            // Console.WriteLine(displayList.Length); 

            result = 0;  

            if (displayList.Length > 1)
            {

                // Get the dimensions for the second screen
                result = string.Compare(displayList[0].DeviceName, displayList[1].DeviceName);
                if (result == 1)
                {
                    resScreen2 = displayList[1].Bounds;
                }
                else{
                    resScreen2 = displayList[0].Bounds;
                }
                

                Console.WriteLine("I'm longer");
                this.Size = new Size(resScreen2.Width, resScreen2.Height);
                this.StartPosition = FormStartPosition.Manual;
                this.Location = new Point(resolution.Width, 0);
            }
            else
            {
                Console.WriteLine("Single Monitor");
                this.Size = new Size(resolution.Width, resolution.Height);
                this.Location = new Point(0, 0);    
            }
            
            panel = new Panel();
            panel.Dock = DockStyle.Fill;

            // Console.WriteLine(Directory.GetCurrentDirectory());

            this.Controls.Add(panel);
        }
        public void PlayMedia(string video_path)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            this.vlcControl = new Vlc.DotNet.Forms.VlcControl();
            this.vlcControl.Dock = DockStyle.Fill;
            this.vlcControl.BeginInit();
            this.vlcControl.VlcLibDirectory = new DirectoryInfo(@currentDirectory);
            this.vlcControl.VlcMediaplayerOptions = new string[] {"--directx-device={,display," + displayList[0].DeviceName + "}" };
            // this.vlcControl.VlcMediaplayerOptions = new string[]{};
            this.vlcControl.EndInit();
           
            panel.Controls.Add(this.vlcControl);
            this.vlcControl.SetMedia(new Uri(video_path));
            this.vlcControl.Play();
            this.Show();
        }
        public void CloseWindow()
        {
            this.Close();
        }
        
        public void PauseVideo()
        {
            this.vlcControl.Pause();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (e.CloseReason == CloseReason.WindowsShutDown) return;

            // Confirm user wants to close
            this.vlcControl.Stop();
        }

        public void Stop()
        {
            this.vlcControl.Stop();
        }
    }
}