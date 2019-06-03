using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;

using System.Windows.Forms;
using System.Drawing;


namespace ChurchPlayer
{
    class Controller : Form
    {
        private ListBox lb;
        private string working_directory;
        private VideoPlayer player;
        private bool hasPlayed;
        private int windowID;
        private List<VideoPlayer> windowList = new List<VideoPlayer>();


        public Controller()
        {
            windowID = 0;
            player = new VideoPlayer(windowID);
            windowList.Add(player);
            this.Size = new Size(400, 400);
            this.Text = "Church Video Player";
            this.IsMdiContainer = true;
            Panel panel = new Panel();
            panel.Size = new Size(this.Width, this.Height);
            panel.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
            
            hasPlayed = false;

            lb = new ListBox();
            lb.Size = new Size(380, 325);
            lb.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
            
            // Read media files in to the ListBox
            working_directory = Directory.GetCurrentDirectory() + "\\media";
            DirectoryInfo d = new DirectoryInfo(@working_directory);
            FileInfo[] Files = d.GetFiles();

            lb.BeginUpdate();

            foreach(FileInfo file in Files)
            {
                lb.Items.Add(file.Name);
            }

            lb.EndUpdate();

            // Now we need a spawn button
            Button play = new Button();
            play.Location = new Point(10, 330);
            play.Text = "Play";
            play.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
            play.Click += new EventHandler(play_clicked);

            Button stop = new Button();
            stop.Location = new Point(85, 330);
            stop.Text = "Stop";
            stop.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
            stop.Click += new EventHandler(stop_clicked);

            Button pause = new Button();
            pause.Location = new Point(160, 330);
            pause.Text = "Pause";
            pause.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
            pause.Click += new EventHandler(pause_clicked);

            Button close = new Button();
            close.Location = new Point(235, 330);
            close.Text = "Close";
            close.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
            close.Click += new EventHandler(close_clicked);

            panel.Controls.Add(lb);
            panel.Controls.Add(play);
            panel.Controls.Add(stop);
            panel.Controls.Add(pause);
            panel.Controls.Add(close);
            this.Controls.Add(panel);
        }

        private void PanelOnResize(object obj, EventArgs e)
        {
            ((Panel) obj).Invalidate();
        }
        private void PanelOnPaint(object obj, PaintEventArgs pea)
        {
            Panel panel = (Panel) obj;
            Graphics gfx = pea.Graphics;

            panel.Width = this.Width;
            panel.Height = this.Height;

        }
        private void close_clicked(object sender, EventArgs e)
        {
            player.CloseWindow();
        }

        private void pause_clicked(object sender, EventArgs e)
        {
            player.PauseVideo();
        }

        private void stop_clicked(object sender, EventArgs e)
        {
            player.Stop();
        }

        private void play_clicked(object sender, EventArgs e)
        {
            // Console.WriteLine(lb.SelectedIndices[0].ToString());
            // Console.WriteLine(lb.SelectedItems[0].ToString());
            if (hasPlayed)
            {
                windowID++;
                // player.CloseWindow();
                player = new VideoPlayer(windowID);
                windowList.Add(player);
                player.PlayMedia(working_directory + "\\" + lb.SelectedItems[0].ToString());
            }
            else
            {
                hasPlayed = true;
                player.PlayMedia(working_directory + "\\" + lb.SelectedItems[0].ToString());
            }   
        }
        [STAThread]
        static void Main(string[] args)
        {          
            Application.EnableVisualStyles();
            Application.Run(new Controller());
        }
    }
}