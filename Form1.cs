using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace MusikApp
{
    public partial class Form1 : Form
    {
        System.Windows.Forms.Timer playbackPositionTimer = new System.Windows.Forms.Timer();
        AnimationTimer animTimer = new AnimationTimer();

        MusicLibrary musicLibrary = new MusicLibrary();
        List<Track> historyPlayList = new List<Track>();
        List<Track> currentPlayList = new List<Track>();

        List<ControlPanel> playlistView = new List<ControlPanel>(); 
        bool randomPlay = false;
        Track currentTrack = null;
        int currentPlaybackPosition = 0;
        bool paused = true;

        Image playImage = Image.FromFile("icons/ic_play_arrow_white_48dp/web/ic_play_arrow_white_48dp_1x.png");
        Image pauseImage = Image.FromFile("icons/ic_pause_white_48dp/web/ic_pause_white_48dp_1x.png");

        Image top_shadow_image = Image.FromFile("icons/dropshadow/drop_shadow_top.png");
        Image bottom_shadow_image = Image.FromFile("icons/dropshadow/drop_shadow_bottom.png");
        Image left_shadow_image = Image.FromFile("icons/dropshadow/drop_shadow_left.png");
        Image right_shadow_image = Image.FromFile("icons/dropshadow/drop_shadow_right.png");

        GlobalKeyboardHook gHook;

        AppConnection app = new AppConnection();
        System.Windows.Forms.Timer AppTimer = new System.Windows.Forms.Timer();

        List<string> albums = new List<string>();
        List<CoverArt> albumCovers = new List<CoverArt>();
        int coverSize = 200;
        int minCoverOffset = 50;
        int realCoverOffset = 50;

        int globalAnimTime = 150;

        NAudio.Wave.BlockAlignReductionStream stream = null;
        NAudio.Wave.DirectSoundOut output = null;
        public Form1()
        {
            gHook = new GlobalKeyboardHook();
            gHook.KeyDown += new KeyEventHandler(this.gHook_KeyDown);
            foreach(Keys key in Enum.GetValues(typeof(Keys)))
            {
                gHook.HookedKeys.Add(key);
            }
            gHook.hook();

            AppTimer.Interval = 100;
            AppTimer.Enabled = true;
            AppTimer.Tick += new EventHandler(this.NetworkTick);
            Console.WriteLine("main thread: " + Thread.CurrentThread.ManagedThreadId);

            playbackPositionTimer.Interval = 100;
            playbackPositionTimer.Enabled = true;
            playbackPositionTimer.Tick += new EventHandler(this.TimeLineTick);

            animTimer.Interval = 10;
            animTimer.Enabled = true;

            InitializeComponent();

            MainSidePanel.BackColor = Color.FromArgb(50,100,200);
            MainSidePanel.Height = this.Height - BottomControlPanel.Height - 39;
            MainSidePanel.Location = new Point(-250,0);

            AddMusicButton.neutralColor = Color.FromArgb(50, 100, 200);
            AddMusicButton.mouseOverColor = Color.FromArgb(70, 120, 200);
            AddMusicButton.mouseDownColor = Color.FromArgb(10, 60, 200);
            AddMusicButton.BackColor = AddMusicButton.neutralColor;
            AddMusicButton.Enabled = false;

            SidePanelFoldButton.neutralColor = Color.FromArgb(50, 100, 200);
            SidePanelFoldButton.mouseOverColor = Color.FromArgb(70, 120, 200);
            SidePanelFoldButton.mouseDownColor = Color.FromArgb(10, 60, 200);
            SidePanelFoldButton.BackColor = AddMusicButton.neutralColor;
            SidePanelFoldButton.neutralImage = Image.FromFile("icons/ic_arrow_forward_white_48dp/web/ic_arrow_forward_white_48dp_1x.png");
            SidePanelFoldButton.mouseOverImage = Image.FromFile("icons/ic_arrow_forward_white_48dp/web/ic_arrow_forward_white_48dp_1x.png");
            SidePanelFoldButton.mouseDownImage = Image.FromFile("icons/ic_arrow_forward_white_48dp/web/ic_arrow_forward_white_48dp_1x.png");
            SidePanelFoldButton.clickedImage = Image.FromFile("icons/ic_arrow_back_white_48dp/web/ic_arrow_back_white_48dp_1x.png");
            SidePanelFoldButton.Image = SidePanelFoldButton.neutralImage;

            MusikPanel.Width = this.Width - 68;
            MusikPanel.Height = BottomControlPanel.Location.Y;

            MusikScrollBar.Location = new Point(this.Width-33,0);
            MusikScrollBar.Height = MusikPanel.Height;

            BottomControlPanel.Width = this.Width -16;
            BottomControlPanel.Location = new Point(0, this.Height- BottomControlPanel.Height - 39);
            BottomControlPanel.BackColor = Color.FromArgb(50, 100, 200);

            MusikControls.Location = new Point(BottomControlPanel.Width/2-MusikControls.Width/2, 0);

            trackLabel.Text = "";
            artistLabel.Text = "";
            albumLabel.Text = "";

            skipBackButton.neutralColor = Color.Transparent;
            skipBackButton.mouseOverColor = Color.Transparent;
            skipBackButton.mouseDownColor = Color.Transparent;
            skipBackButton.BackColor = skipBackButton.neutralColor;
            skipBackButton.neutralImage = Image.FromFile("icons/ic_skip_previous_white_48dp/web/ic_skip_previous_white_48dp_1x.png");
            skipBackButton.mouseOverImage = Image.FromFile("icons/ic_skip_previous_white_48dp/web/ic_skip_previous_white_48dp_1x.png");
            skipBackButton.mouseDownImage = Image.FromFile("icons/ic_skip_previous_white_48dp/web/ic_skip_previous_white_48dp_1x.png");
            skipBackButton.clickedImage = Image.FromFile("icons/ic_skip_previous_white_48dp/web/ic_skip_previous_white_48dp_1x.png");
            skipBackButton.Image = Image.FromFile("icons/ic_skip_previous_white_48dp/web/ic_skip_previous_white_48dp_1x.png");

            skipForwardButton.neutralColor = Color.Transparent;
            skipForwardButton.mouseOverColor = Color.Transparent;
            skipForwardButton.mouseDownColor = Color.Transparent;
            skipForwardButton.BackColor = skipBackButton.neutralColor;
            skipForwardButton.neutralImage = Image.FromFile("icons/ic_skip_next_white_48dp/web/ic_skip_next_white_48dp_1x.png");
            skipForwardButton.mouseOverImage = Image.FromFile("icons/ic_skip_next_white_48dp/web/ic_skip_next_white_48dp_1x.png");
            skipForwardButton.mouseDownImage = Image.FromFile("icons/ic_skip_next_white_48dp/web/ic_skip_next_white_48dp_1x.png");
            skipForwardButton.clickedImage = Image.FromFile("icons/ic_skip_next_white_48dp/web/ic_skip_next_white_48dp_1x.png");
            skipForwardButton.Image = Image.FromFile("icons/ic_skip_next_white_48dp/web/ic_skip_next_white_48dp_1x.png");

            PlayPauseButton.neutralColor = Color.Transparent;
            PlayPauseButton.mouseOverColor = Color.Transparent;
            PlayPauseButton.mouseDownColor = Color.Transparent;
            PlayPauseButton.BackColor = skipBackButton.neutralColor;
            PlayPauseButton.mouseOverImage = playImage;
            PlayPauseButton.neutralImage = playImage;
            PlayPauseButton.Image = PlayPauseButton.neutralImage;
            PlayPauseButton.SizeMode = PictureBoxSizeMode.CenterImage;

            TimeLineBack.BackColor = Color.FromArgb(70, 120, 200);
            TimeLineBack.Width = BottomControlPanel.Width - 45;

            TimeLine.Width = 0;

            /*
            randomButton.neutralImage = Image.FromFile("icons/ic_shuffle_white_48dp/web/ic_shuffle_white_48dp_1x.png");
            randomButton.mouseOverImage = Image.FromFile("icons/ic_shuffle_white_48dp/web/ic_shuffle_white_48dp_1x.png");
            randomButton.mouseDownImage = Image.FromFile("icons/ic_shuffle_white_48dp/web/ic_shuffle_white_48dp_1x.png");
            randomButton.clickedImage = Image.FromFile("icons/ic_shuffle_white_48dp/web/ic_shuffle_white_48dp_1x.png");
            randomButton.Image = randomButton.neutralImage;
            */

            Animation MusikControlSlideLeft = new Animation(MusikControls, "musikControlSlideLeft", Animation.MOVE, 0, 0, globalAnimTime, animTimer);
            MusikControlSlideLeft.SetTransMode(Animation.ABSOLUTE);
            MusikControls.AddAnimation(MusikControlSlideLeft);

            Animation MusikControlSlideRight = new Animation(MusikControls, "musikControlSlideRight", Animation.MOVE, (BottomControlPanel.Width / 2 - MusikControls.Width / 2), 0, globalAnimTime, animTimer);
            MusikControlSlideRight.SetTransMode(Animation.ABSOLUTE);
            MusikControls.AddAnimation(MusikControlSlideRight);


            Animation SidePanelFoldOut = new Animation(MainSidePanel, "sidePanelFoldOut", Animation.MOVE, 0, 0, globalAnimTime, animTimer);
            SidePanelFoldOut.SetTransMode(Animation.ABSOLUTE);
            MainSidePanel.AddAnimation(SidePanelFoldOut);

            Animation SidePanelFoldIn = new Animation(MainSidePanel, "sidePanelFoldIn", Animation.MOVE, -250, 0, globalAnimTime, animTimer);
            SidePanelFoldIn.SetTransMode(Animation.ABSOLUTE);
            MainSidePanel.AddAnimation(SidePanelFoldIn);
        }

        private void NetworkTick(object sender, EventArgs e)
        {
            string width = "" + TimeLine.Width;
            //app.SendCommand(width);
            
            if (app.command.Equals("playpause"))
            {
                PlayPauseButton_Click(sender, e);
                app.command = "";
            }

            if (app.command.Equals("prev"))
            {
                skipBackButton_Click(sender, e);
                app.command = "";
            }

            if (app.command.Equals("next"))
            {
                skipForwardButton_Click(sender, e);
                app.command = "";
            }
                
        }

        private void TimeLineTick(object sender, EventArgs e)
        {
            if(stream != null)
            {
                TimeSpan current = stream.CurrentTime;
                TimeSpan total = stream.TotalTime;
                if (current >= total)
                {
                    skipForward();
                }
                double width = (double)TimeLineBack.Width / total.TotalMilliseconds * current.TotalMilliseconds;
                TimeLine.Width = (int)width;
            }
        }

        private void MainSidePanel_Click(object sender, EventArgs e)
        {
            if (!MainSidePanel.foldedOut)
            {
                MainSidePanel.BackColor = Color.FromArgb(50, 100, 200);
                foreach (Control con in MainSidePanel.Controls)
                {
                    con.BackColor = Color.FromArgb(50, 100, 200);
                    con.Enabled = true;
                }
                SidePanelFoldButton.Image = SidePanelFoldButton.clickedImage;
                MainSidePanel.GetAnimation("sidePanelFoldOut").start();
                MusikControls.GetAnimation("musikControlSlideLeft").start();
                MainSidePanel.foldedOut = true;
            }
        }

        private void MainSidePanel_MouseEnter(object sender, EventArgs e)
        {
            if (!MainSidePanel.foldedOut)
            {
                MainSidePanel.BackColor = Color.FromArgb(70, 120, 200);
                foreach(Control con in MainSidePanel.Controls)
                {
                    con.BackColor = Color.FromArgb(70, 120, 200);
                }
            }
        }

        private void MainSidePanel_MouseLeave(object sender, EventArgs e)
        {
            if (!MainSidePanel.foldedOut)
            {
                MainSidePanel.BackColor = Color.FromArgb(50, 100, 200);
                foreach (Control con in MainSidePanel.Controls)
                {
                    con.BackColor = Color.FromArgb(50, 100, 200);
                }
            }
        }

        private void SidePanelFoldButton_Click(object sender, EventArgs e)
        {
            if (MainSidePanel.foldedOut)
            {
                foreach (Control con in MainSidePanel.Controls)
                {
                    if(con.Name != "SidePanelFoldButton")
                    {
                        con.Enabled = false;
                    }
                }
                MainSidePanel.GetAnimation("sidePanelFoldIn").start();
                MusikControls.GetAnimation("musikControlSlideRight").start();
                MainSidePanel.foldedOut = false;
                SidePanelFoldButton.Image = SidePanelFoldButton.neutralImage;
            }
            else
            {
                MainSidePanel.BackColor = Color.FromArgb(50, 100, 200);
                foreach (Control con in MainSidePanel.Controls)
                {
                    if (con.Name != "SidePanelFoldButton")
                    {
                        con.BackColor = Color.FromArgb(50, 100, 200);
                        con.Enabled = true;
                    }
                }
                MainSidePanel.GetAnimation("sidePanelFoldOut").start();
                MusikControls.GetAnimation("musikControlSlideLeft").start();
                MainSidePanel.foldedOut = true;
                SidePanelFoldButton.Image = SidePanelFoldButton.clickedImage;
            }
        }

        public void SidePanelFoldIn()
        {
            if (MainSidePanel.foldedOut)
            {
                foreach (Control con in MainSidePanel.Controls)
                {
                    if (con.Name != "SidePanelFoldButton")
                    {
                        con.Enabled = false;
                    }
                }
                MainSidePanel.GetAnimation("sidePanelFoldIn").start();
                MusikControls.GetAnimation("musikControlSlideRight").start();
                MainSidePanel.foldedOut = false;
                SidePanelFoldButton.Image = SidePanelFoldButton.neutralImage;
            }
        }

        private void CoverClick(object sender, EventArgs e)
        {
            SidePanelFoldIn();
            CoverArt clickedCover = (CoverArt)sender;
            Console.WriteLine(clickedCover.album + " cover clicked");
            if (clickedCover.selected)
            {
                foreach (CoverArt cover in albumCovers)
                {
                    if (cover.Location.Y > clickedCover.Location.Y+20)
                    {
                        cover.GetAnimation("coverMoveUp").start();
                        cover.movedDown = false;
                    }
                }
                clickedCover.GetAnimation("coverZoomOut").start();
                clickedCover.albumPanel.GetAnimation("albumFoldIn").start();
                clickedCover.selected = false;
            }
            else
            {
                foreach(CoverArt cover in albumCovers)
                {
                    if (cover.selected)
                    {
                        cover.Location = new Point(cover.Location.X +10, cover.Location.Y + 10);
                        cover.Size = new Size(cover.Width - 20, cover.Height - 20);
                        cover.albumPanel.Size = new Size(cover.albumPanel.Width, 0);
                        cover.selected = false;
                    }
                    if(cover.Location.Y > clickedCover.Location.Y+20 && !cover.movedDown)
                    {
                        cover.GetAnimation("coverMoveDown").start();
                        cover.movedDown = true;
                    }
                }
                if (clickedCover.movedDown)
                {
                    foreach (CoverArt cover in albumCovers)
                    {
                        if ((cover.Location.Y == clickedCover.Location.Y || cover.movedDown) && cover != clickedCover)
                        {
                            cover.GetAnimation("coverMoveUp").start();
                            cover.movedDown = false;
                        }
                    }
                    clickedCover.Location = new Point(clickedCover.Location.X, clickedCover.Location.Y-315);
                    clickedCover.movedDown = false;
                }
                clickedCover.GetAnimation("coverZoomIn").start();
                clickedCover.albumPanel.GetAnimation("albumFoldOut").start();
                clickedCover.selected = true;
            }
        }

        private void CoverDoubleClick(object sender, EventArgs e)
        {
            SidePanelFoldIn();
            CoverArt clickedCover = (CoverArt)sender;
            currentPlayList.Clear();
            if (randomPlay)
            {
                int maxRandomTracks = musicLibrary.GetTracksByAlbum(clickedCover.album).Count;
                Random rand = new Random();
                for (int i = 0; i < maxRandomTracks; i++)
                {
                    int randomIndex = rand.Next(0, musicLibrary.GetTracksByAlbum(clickedCover.album).Count);

                    foreach (Track trck in currentPlayList)
                    {
                        if (randomIndex == musicLibrary.GetTracksByAlbum(clickedCover.album).IndexOf(trck))
                        {
                            Console.WriteLine("already there...");
                            randomIndex = rand.Next(0, musicLibrary.GetTracksByAlbum(clickedCover.album).Count);
                        }
                    }
                    currentPlayList.Add(musicLibrary.GetTracksByAlbum(clickedCover.album)[randomIndex]);
                }
            }
            else
            {
                foreach(Track trck in musicLibrary.GetTracksByAlbum(clickedCover.album))
                {
                    currentPlayList.Add(trck);
                }
            }
            currentTrack = currentPlayList[0];
            currentPlayList.RemoveAt(0);
            DisposeWave();
            NAudio.Wave.WaveStream pcm = NAudio.Wave.WaveFormatConversionStream.CreatePcmStream(new NAudio.Wave.Mp3FileReader(currentTrack.GetPath()));
            stream = new NAudio.Wave.BlockAlignReductionStream(pcm);
            output = new NAudio.Wave.DirectSoundOut();
            output.Init(stream);
            PlayCurrentTrack();
        }

        private void AlbumPanelClick(object sender, EventArgs e)
        {
            SidePanelFoldIn();
        }

        private void TrackButtonDoubleClick(object sender, EventArgs e)
        {
            SidePanelFoldIn();
            TrackButton clickedTrack = (TrackButton)sender;
            Track track = musicLibrary.GetTrackByID(clickedTrack.trackID);
            string album = track.GetAlbum();
            currentPlayList.Clear();
            if (randomPlay)
            {
                int maxRandomTracks = musicLibrary.GetTracksByAlbum(album).Count;
                Random rand = new Random();
                for (int i = 0; i < maxRandomTracks; i++)
                {
                    int randomIndex = rand.Next(0, musicLibrary.GetTracksByAlbum(album).Count);

                    foreach (Track trck in currentPlayList)
                    {
                        if (randomIndex == musicLibrary.GetTracksByAlbum(album).IndexOf(trck))
                        {
                            Console.WriteLine("already there...");
                            randomIndex = rand.Next(0, musicLibrary.GetTracksByAlbum(album).Count);
                        }
                    }
                    currentPlayList.Add(musicLibrary.GetTracksByAlbum(album)[randomIndex]);
                }
            }
            else
            {
                foreach (Track trck in musicLibrary.GetTracksByAlbum(album))
                {
                    currentPlayList.Add(trck);
                }
            }
            currentTrack = track;
            currentPlayList.Remove(track);
            DisposeWave();
            NAudio.Wave.WaveStream pcm = NAudio.Wave.WaveFormatConversionStream.CreatePcmStream(new NAudio.Wave.Mp3FileReader(currentTrack.GetPath()));
            stream = new NAudio.Wave.BlockAlignReductionStream(pcm);
            output = new NAudio.Wave.DirectSoundOut();
            output.Init(stream);
            PlayCurrentTrack();
        }

        private void TrackButtonClick(object sender, EventArgs e)
        {
            SidePanelFoldIn();
        }

        public void BigCoverClick(object sender, EventArgs e)
        {
            SidePanelFoldIn();
        }

        public void PlayCurrentTrack()
        {
            PlayPauseButton.neutralImage = null;
            PlayPauseButton.mouseOverImage = pauseImage;
            PlayPauseButton.Image = PlayPauseButton.mouseOverImage;
            TagLib.File file = TagLib.File.Create(currentTrack.GetPath());

            if (file.Tag.Pictures.Length != 0)
            {
                MemoryStream ms = new MemoryStream(file.Tag.Pictures[0].Data.Data);
                PlayPauseButton.BackgroundImage = Image.FromStream(ms);
            }
            else
            {
                PlayPauseButton.BackgroundImage = Image.FromFile(@"icons\no_cover_art.jpg");
            }

            //MemoryStream ms = new MemoryStream(file.Tag.Pictures[0].Data.Data);
            //PlayPauseButton.BackgroundImage = Image.FromStream(ms);
            trackLabel.Text = currentTrack.GetTitle();
            artistLabel.Text = currentTrack.GetArtists()[0];
            albumLabel.Text = currentTrack.GetAlbum();
            paused = false;
            output.Play();
            //app.SendCommand(currentTrack.GetTitle());
        }

        private void AddAlbumPanel(CoverArt cover)
        {
            if (cover.albumPanel == null)
            {
                cover.albumPanel = new AlbumPanel();
                MusikPanel.Controls.Add(cover.albumPanel);
                cover.albumPanel.Click += new System.EventHandler(this.AlbumPanelClick);
                List<Track> tracksByAlbum = musicLibrary.GetTracksByAlbum(cover.album);
                //Console.WriteLine(tracksByAlbum.Count);

                PictureBox bigCover = new PictureBox();
                bigCover.Image = cover.Image;
                cover.albumPanel.Controls.Add(bigCover);
                bigCover.Size = new Size(245, 245);
                bigCover.Location = new Point(20, 30);
                bigCover.SizeMode = PictureBoxSizeMode.Zoom;
                bigCover.Click += new System.EventHandler(this.BigCoverClick);

                int i = 0;
                foreach (Track trck in tracksByAlbum)
                {
                    TrackButton trackButton = new TrackButton();
                    cover.albumPanel.trackButtons.Add(trackButton);
                    //Console.WriteLine("trackButton added...");
                    cover.albumPanel.Controls.Add(trackButton);

                    trackButton.trackID = trck.GetID();
                    string trackNumberStr = "" + trck.GetNumber();
                    if (trck.GetNumber() < 10)
                    {
                        trackNumberStr = "0" + trackNumberStr;
                    }
                    trackButton.Text = trackNumberStr + " - " + trck.GetTitle();
                    trackButton.TextAlign = ContentAlignment.MiddleLeft;
                    trackButton.Size = new Size(250, 20);
                    trackButton.Location = new Point(305 + (int)(i / 10) * 290, 30 + tracksByAlbum.IndexOf(trck) * (20 + 5) - (int)(i / 10) * 250);

                    trackButton.neutralColor = Color.Transparent;
                    trackButton.mouseOverColor = Color.FromArgb(70, 120, 200);
                    trackButton.mouseDownColor = Color.FromArgb(10, 60, 200);
                    trackButton.selectedColor = Color.FromArgb(10, 60, 200);

                    trackButton.neutralColorText = Color.FromArgb(0, 0, 0);
                    trackButton.mouseOverColorText = Color.FromArgb(0, 0, 0);
                    trackButton.mouseDownColorText = Color.FromArgb(255, 255, 255);
                    trackButton.selectedColorText = Color.FromArgb(255, 255, 255);

                    trackButton.BackColor = trackButton.neutralColor;
                    trackButton.ForeColor = trackButton.neutralColorText;
                    trackButton.DoubleClick += new System.EventHandler(this.TrackButtonDoubleClick);
                    trackButton.Click += new System.EventHandler(this.TrackButtonClick);
                    i++;
                }

                Animation coverZoomIn = new Animation(cover, "coverZoomIn", Animation.SCALE, 20, 20, globalAnimTime, animTimer);
                coverZoomIn.SetTransMode(Animation.RELATIVE);
                coverZoomIn.SetPivot(Animation.CenterCenter);
                cover.AddAnimation(coverZoomIn);

                Animation coverZoomOut = new Animation(cover, "coverZoomOut", Animation.SCALE, -20, -20, globalAnimTime, animTimer);
                coverZoomOut.SetTransMode(Animation.RELATIVE);
                coverZoomOut.SetPivot(Animation.CenterCenter);
                cover.AddAnimation(coverZoomOut);

                Animation coverMoveDown = new Animation(cover, "coverMoveDown", Animation.MOVE, 0, 315, globalAnimTime, animTimer);
                coverMoveDown.SetTransMode(Animation.RELATIVE);
                cover.AddAnimation(coverMoveDown);

                Animation coverMoveUp = new Animation(cover, "coverMoveUp", Animation.MOVE, 0, -315, globalAnimTime, animTimer);
                coverMoveUp.SetTransMode(Animation.RELATIVE);
                cover.AddAnimation(coverMoveUp);

                Animation albumFoldOut = new Animation(cover.albumPanel, "albumFoldOut", Animation.SCALE, 0, 305, globalAnimTime, animTimer);
                albumFoldOut.SetTransMode(Animation.RELATIVE);
                cover.albumPanel.AddAnimation(albumFoldOut);

                Animation albumFoldIn = new Animation(cover.albumPanel, "albumFoldIn", Animation.SCALE, 0, -305, globalAnimTime, animTimer);
                albumFoldIn.SetTransMode(Animation.RELATIVE);
                cover.albumPanel.AddAnimation(albumFoldIn);

                cover.Click += new System.EventHandler(this.CoverClick);
                cover.DoubleClick += new System.EventHandler(this.CoverDoubleClick);

                cover.albumPanel.BackColor = Color.FromArgb(255, 255, 255);
                cover.albumPanel.Size = new Size(MusikPanel.Width, 0);
                cover.albumPanel.Location = new Point(0, cover.Location.Y + cover.Height + 10);

                //PictureBox top_shadow = new PictureBox();
                cover.albumPanel.top_shadow.Image = top_shadow_image;
                cover.albumPanel.top_shadow.SizeMode = PictureBoxSizeMode.StretchImage;
                cover.albumPanel.Controls.Add(cover.albumPanel.top_shadow);
                cover.albumPanel.top_shadow.Size = new Size(cover.albumPanel.Width, 10);
                cover.albumPanel.top_shadow.Location = new Point(0, 0);

                //PictureBox bottom_shadow = new PictureBox();
                cover.albumPanel.bottom_shadow.Image = bottom_shadow_image;
                cover.albumPanel.bottom_shadow.SizeMode = PictureBoxSizeMode.StretchImage;
                cover.albumPanel.Controls.Add(cover.albumPanel.bottom_shadow);
                cover.albumPanel.bottom_shadow.Size = new Size(cover.albumPanel.Width, 10);
                cover.albumPanel.bottom_shadow.Location = new Point(0, 295);
            }
        }

        private void AddMusicButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog addFilesDialog = new OpenFileDialog();
            addFilesDialog.Filter = "Audio Files|*.mp3";
            addFilesDialog.Title = "Select one or more Audio Files.";
            addFilesDialog.Multiselect = true;
            if (addFilesDialog.ShowDialog() == DialogResult.OK)
            {
                for (int f = 0; f < addFilesDialog.FileNames.GetLength(0); f++)
                {
                    string path = addFilesDialog.FileNames[f];
                    string filename = addFilesDialog.SafeFileNames[f];

                    TagLib.File file = TagLib.File.Create(path);
                    string title = file.Tag.Title;
                    string album = file.Tag.Album;
                    List<string> albumArtists = new List<string>();
                    foreach (string aArtist in file.Tag.AlbumArtists)
                    {
                        albumArtists.Add(aArtist);
                    }
                    List<string> artists = new List<string>();
                    foreach (string artist in file.Tag.Performers)
                    {
                        artists.Add(artist);
                    }
                    List<string> genres = new List<string>();
                    foreach (string genre in file.Tag.Genres)
                    {
                        genres.Add(genre);
                    }
                    TimeSpan length = file.Properties.Duration;
                    int year = (int)file.Tag.Year;
                    int number = (int)file.Tag.Track;
                    int numTracks = (int)file.Tag.TrackCount;
                    int bpm = (int)file.Tag.BeatsPerMinute;
                    int sampleRate = file.Properties.AudioSampleRate;
                    //file.Properties.AudioBitrate;

                    musicLibrary.addNewTrack(new Track(path, filename, title, album, albumArtists, artists, genres, length, year, number, numTracks, bpm, sampleRate));
                    foreach (Track track in musicLibrary.GetAll())
                    {
                        if (!albums.Contains(track.GetAlbum().ToLower()))
                        {
                            albums.Add(track.GetAlbum().ToLower());
                            CoverArt cover = new CoverArt();
                            cover.album = track.GetAlbum().ToLower();
                            cover.artists = track.GetArtists();
                            Console.WriteLine(file.Tag.Pictures.Length);
                            if (file.Tag.Pictures.Length != 0)
                            {
                                MemoryStream ms = new MemoryStream(file.Tag.Pictures[0].Data.Data);
                                cover.Image = Image.FromStream(ms);
                            } else
                            {
                                cover.Image = Image.FromFile(@"icons\no_cover_art.jpg");
                            }
                            cover.SizeMode = PictureBoxSizeMode.Zoom;
                            albumCovers.Add(cover);

                            MusikPanel.Controls.Add(cover);

                            int n = (MusikPanel.Width - minCoverOffset) / (coverSize + minCoverOffset);
                            Console.WriteLine("max covers per row: " + n);
                            int row = (albumCovers.IndexOf(cover) / n);
                            int x = 50 + (coverSize + 50) * (albumCovers.IndexOf(cover)) - row * (n * (coverSize + 50));
                            int y = 50 + row * (coverSize + 50);
                            cover.Location = new Point(x, y);
                            cover.Size = new Size(coverSize, coverSize);
                        }
                    }
                }
                foreach(CoverArt cover in albumCovers)
                {
                    AddAlbumPanel(cover);
                }
            }
            if (MainSidePanel.foldedOut)
            {
                foreach (Control con in MainSidePanel.Controls)
                {
                    if (con.Name != "SidePanelFoldButton")
                    {
                        con.Enabled = false;
                    }
                }
                MainSidePanel.GetAnimation("sidePanelFoldIn").start();
                MusikControls.GetAnimation("musikControlSlideRight").start();
                MainSidePanel.foldedOut = false;
                SidePanelFoldButton.Image = SidePanelFoldButton.neutralImage;
            }
        }

        public void UpdatePlaylistView(bool forward)
        {
            int maxTracks = 5;
            Panel hidePanel = new Panel();
            MainSidePanel.Controls.Add(hidePanel);
            hidePanel.Location = new Point(0, BottomControlPanel.Location.Y - (maxTracks+1) * 64);
            hidePanel.Size = new Size(MainSidePanel.Width, 64);
            hidePanel.BackColor = Color.Red;
            if (playlistView.Count == 0)
            {
                if (currentPlayList.Count < maxTracks)
                {
                    maxTracks = currentPlayList.Count;
                }
                for (int i = 0; i < maxTracks; i++)
                {
                    ControlPanel panel = new ControlPanel();
                    MainSidePanel.Controls.Add(panel);
                    panel.SendToBack();
                    panel.Location = new Point(0, BottomControlPanel.Location.Y - 64 * (i + 1));
                    panel.Size = new Size(MainSidePanel.Width, 64);
                    panel.BackColor = Color.Transparent;
                    PictureBox cover = new PictureBox();
                    panel.Controls.Add(cover);
                    cover.Size = new Size(64, 64);
                    cover.Location = new Point(0, 0);
                    cover.SizeMode = PictureBoxSizeMode.Zoom;
                    Label trackname = new Label();
                    Label artistname = new Label();
                    Label albumname = new Label();
                    panel.Controls.Add(trackname);
                    panel.Controls.Add(artistname);
                    panel.Controls.Add(albumname);
                    trackname.BackColor = Color.Transparent;
                    artistname.BackColor = Color.Transparent;
                    albumname.BackColor = Color.Transparent;
                    trackname.ForeColor = Color.White;
                    artistname.ForeColor = Color.White;
                    albumname.ForeColor = Color.White;
                    trackname.Location = new Point(75, 5);
                    artistname.Location = new Point(75, 25);
                    albumname.Location = new Point(75, 45);

                    TagLib.File file = TagLib.File.Create(currentPlayList[i].GetPath());
                    MemoryStream ms = new MemoryStream(file.Tag.Pictures[0].Data.Data);

                    cover.Image = Image.FromStream(ms);
                    trackname.Text = currentPlayList[i].GetTitle();
                    artistname.Text = currentPlayList[i].GetArtists()[0];
                    albumname.Text = currentPlayList[i].GetAlbum();

                    Animation moveDown = new MusikApp.Animation(panel, "moveDown", Animation.MOVE, 0, 64, globalAnimTime, animTimer);
                    moveDown.SetTransMode(Animation.RELATIVE);
                    panel.AddAnimation(moveDown);

                    Animation moveUp = new MusikApp.Animation(panel, "moveUp", Animation.MOVE, 0, -64, globalAnimTime, animTimer);
                    moveUp.SetTransMode(Animation.RELATIVE);
                    panel.AddAnimation(moveUp);

                    playlistView.Add(panel);
                }
            }
            else
            {
                if (currentPlayList.Count >= maxTracks)
                {
                    if (forward)
                    {
                        int i = maxTracks - 1;
                        ControlPanel panel = new ControlPanel();
                        MainSidePanel.Controls.Add(panel);
                        panel.SendToBack();
                        panel.Location = new Point(0, BottomControlPanel.Location.Y - 64 * (i + 2));
                        panel.Size = new Size(MainSidePanel.Width, 64);
                        panel.BackColor = Color.Transparent;
                        PictureBox cover = new PictureBox();
                        panel.Controls.Add(cover);
                        cover.Size = new Size(64, 64);
                        cover.Location = new Point(0, 0);
                        cover.SizeMode = PictureBoxSizeMode.Zoom;
                        Label trackname = new Label();
                        Label artistname = new Label();
                        Label albumname = new Label();
                        panel.Controls.Add(trackname);
                        panel.Controls.Add(artistname);
                        panel.Controls.Add(albumname);
                        trackname.BackColor = Color.Transparent;
                        artistname.BackColor = Color.Transparent;
                        albumname.BackColor = Color.Transparent;
                        trackname.ForeColor = Color.White;
                        artistname.ForeColor = Color.White;
                        albumname.ForeColor = Color.White;
                        trackname.Location = new Point(75, 5);
                        artistname.Location = new Point(75, 25);
                        albumname.Location = new Point(75, 45);

                        TagLib.File file = TagLib.File.Create(currentPlayList[i].GetPath());
                        MemoryStream ms = new MemoryStream(file.Tag.Pictures[0].Data.Data);

                        cover.Image = Image.FromStream(ms);
                        trackname.Text = currentPlayList[i].GetTitle();
                        artistname.Text = currentPlayList[i].GetArtists()[0];
                        albumname.Text = currentPlayList[i].GetAlbum();

                        Animation moveDown = new MusikApp.Animation(panel, "moveDown", Animation.MOVE, 0, 64, globalAnimTime, animTimer);
                        moveDown.SetTransMode(Animation.RELATIVE);
                        panel.AddAnimation(moveDown);

                        Animation moveUp = new MusikApp.Animation(panel, "moveUp", Animation.MOVE, 0, -64, globalAnimTime, animTimer);
                        moveUp.SetTransMode(Animation.RELATIVE);
                        panel.AddAnimation(moveUp);

                        playlistView.Add(panel);

                        foreach (ControlPanel pnl in playlistView)
                        {
                            pnl.GetAnimation("moveDown").start();
                        }
                        playlistView.RemoveAt(0);
                    }
                    else
                    {
                        ControlPanel panel = new ControlPanel();
                        MainSidePanel.Controls.Add(panel);
                        panel.SendToBack();
                        panel.Location = new Point(0, BottomControlPanel.Location.Y);
                        panel.Size = new Size(MainSidePanel.Width, 64);
                        panel.BackColor = Color.Transparent;
                        PictureBox cover = new PictureBox();
                        panel.Controls.Add(cover);
                        cover.Size = new Size(64, 64);
                        cover.Location = new Point(0, 0);
                        cover.SizeMode = PictureBoxSizeMode.Zoom;
                        Label trackname = new Label();
                        Label artistname = new Label();
                        Label albumname = new Label();
                        panel.Controls.Add(trackname);
                        panel.Controls.Add(artistname);
                        panel.Controls.Add(albumname);
                        trackname.BackColor = Color.Transparent;
                        artistname.BackColor = Color.Transparent;
                        albumname.BackColor = Color.Transparent;
                        trackname.ForeColor = Color.White;
                        artistname.ForeColor = Color.White;
                        albumname.ForeColor = Color.White;
                        trackname.Location = new Point(75, 5);
                        artistname.Location = new Point(75, 25);
                        albumname.Location = new Point(75, 45);

                        TagLib.File file = TagLib.File.Create(currentTrack.GetPath());
                        MemoryStream ms = new MemoryStream(file.Tag.Pictures[0].Data.Data);

                        cover.Image = Image.FromStream(ms);
                        trackname.Text = currentTrack.GetTitle();
                        artistname.Text = currentTrack.GetArtists()[0];
                        albumname.Text = currentTrack.GetAlbum();

                        Animation moveDown = new MusikApp.Animation(panel, "moveDown", Animation.MOVE, 0, 64, globalAnimTime, animTimer);
                        moveDown.SetTransMode(Animation.RELATIVE);
                        panel.AddAnimation(moveDown);

                        Animation moveUp = new MusikApp.Animation(panel, "moveUp", Animation.MOVE, 0, -64, globalAnimTime, animTimer);
                        moveUp.SetTransMode(Animation.RELATIVE);
                        panel.AddAnimation(moveUp);

                        playlistView.Insert(0, panel);

                        foreach (ControlPanel pnl in playlistView)
                        {
                            pnl.GetAnimation("moveUp").start();
                        }
                        playlistView.RemoveAt(playlistView.Count - 1);
                    }
                }
                else
                {
                    if (forward)
                    {
                        foreach (ControlPanel pnl in playlistView)
                        {
                            pnl.GetAnimation("moveDown").start();
                        }
                    }
                    else
                    {
                        foreach (ControlPanel pnl in playlistView)
                        {
                            pnl.GetAnimation("moveUp").start();
                        }
                    }
                }
            }
        }

        public void DisposeWave()
        {
            if(output != null)
            {
                if(output.PlaybackState == NAudio.Wave.PlaybackState.Playing)
                {
                    output.Stop();
                }
                output.Dispose();
                output = null;
            }
            if(stream != null)
            {
                stream.Dispose();
                stream = null;
            }
        }

        public void gHook_KeyDown(object sender, KeyEventArgs e)
        {
            Console.WriteLine(e.KeyCode);
            if (e.KeyCode == Keys.MediaPlayPause)
            {
                if (paused)
                {
                    if (musicLibrary.GetAll().Count != 0)
                    {
                        foreach (CoverArt cover in albumCovers)
                        {
                            if (cover.selected)
                            {
                                currentPlayList.Clear();
                                if (randomPlay)
                                {
                                    int maxRandomTracks = musicLibrary.GetTracksByAlbum(cover.album).Count;
                                    Random rand = new Random();
                                    for (int i = 0; i < maxRandomTracks; i++)
                                    {
                                        int randomIndex = rand.Next(0, musicLibrary.GetTracksByAlbum(cover.album).Count);

                                        foreach (Track trck in currentPlayList)
                                        {
                                            if (randomIndex == musicLibrary.GetTracksByAlbum(cover.album).IndexOf(trck))
                                            {
                                                Console.WriteLine("already there...");
                                                randomIndex = rand.Next(0, musicLibrary.GetTracksByAlbum(cover.album).Count);
                                            }
                                        }
                                        currentPlayList.Add(musicLibrary.GetTracksByAlbum(cover.album)[randomIndex]);
                                    }
                                }
                                else
                                {
                                    foreach (Track trck in musicLibrary.GetTracksByAlbum(cover.album))
                                    {
                                        currentPlayList.Add(trck);
                                    }
                                }
                                currentTrack = currentPlayList[0];
                                currentPlayList.RemoveAt(0);
                                DisposeWave();
                                NAudio.Wave.WaveStream pcm = NAudio.Wave.WaveFormatConversionStream.CreatePcmStream(new NAudio.Wave.Mp3FileReader(currentTrack.GetPath()));
                                stream = new NAudio.Wave.BlockAlignReductionStream(pcm);
                                output = new NAudio.Wave.DirectSoundOut();
                                output.Init(stream);
                                PlayCurrentTrack();
                            }
                        }
                        if (currentPlayList.Count == 0)
                        {
                            int maxRandomTracks = musicLibrary.GetAll().Count;
                            Random rand = new Random();
                            for (int i = 0; i < maxRandomTracks; i++)
                            {
                                int randomIndex = rand.Next(0, musicLibrary.GetAll().Count);

                                foreach (Track trck in currentPlayList)
                                {
                                    if (randomIndex == musicLibrary.GetAll().IndexOf(trck))
                                    {
                                        Console.WriteLine("already there...");
                                        randomIndex = rand.Next(0, musicLibrary.GetAll().Count);
                                    }
                                }

                                currentPlayList.Add(musicLibrary.GetAll()[randomIndex]);
                            }
                            currentTrack = currentPlayList[0];
                            currentPlayList.RemoveAt(0);
                            DisposeWave();
                            NAudio.Wave.WaveStream pcm = NAudio.Wave.WaveFormatConversionStream.CreatePcmStream(new NAudio.Wave.Mp3FileReader(currentTrack.GetPath()));
                            stream = new NAudio.Wave.BlockAlignReductionStream(pcm);
                            output = new NAudio.Wave.DirectSoundOut();
                            output.Init(stream);
                            PlayCurrentTrack();
                        }
                        else
                        {
                            PlayCurrentTrack();
                        }
                    }
                }
                else
                {
                    output.Pause();
                    PlayPauseButton.mouseOverImage = playImage;
                    PlayPauseButton.Image = PlayPauseButton.mouseOverImage;
                    paused = true;
                }
            }
            if (e.KeyCode == Keys.MediaNextTrack)
            {
                skipForward();
            }
            if (e.KeyCode == Keys.MediaPreviousTrack)
            {
                skipBack();
            }
        }

        private void PlayPauseButton_Click(object sender, EventArgs e)
        {
            if (paused)
            {
                if (musicLibrary.GetAll().Count != 0)
                {
                    foreach(CoverArt cover in albumCovers)
                    {
                        if (cover.selected)
                        {
                            currentPlayList.Clear();
                            if (randomPlay)
                            {
                                int maxRandomTracks = musicLibrary.GetTracksByAlbum(cover.album).Count;
                                Random rand = new Random();
                                for (int i = 0; i < maxRandomTracks; i++)
                                {
                                    int randomIndex = rand.Next(0, musicLibrary.GetTracksByAlbum(cover.album).Count);

                                    foreach (Track trck in currentPlayList)
                                    {
                                        if (randomIndex == musicLibrary.GetTracksByAlbum(cover.album).IndexOf(trck))
                                        {
                                            //Console.WriteLine("already there...");
                                            randomIndex = rand.Next(0, musicLibrary.GetTracksByAlbum(cover.album).Count);
                                        }
                                    }
                                    currentPlayList.Add(musicLibrary.GetTracksByAlbum(cover.album)[randomIndex]);
                                }
                            }
                            else
                            {
                                foreach (Track trck in musicLibrary.GetTracksByAlbum(cover.album))
                                {
                                    currentPlayList.Add(trck);
                                }
                            }
                            currentTrack = currentPlayList[0];
                            currentPlayList.RemoveAt(0);
                            DisposeWave();
                            NAudio.Wave.WaveStream pcm = NAudio.Wave.WaveFormatConversionStream.CreatePcmStream(new NAudio.Wave.Mp3FileReader(currentTrack.GetPath()));
                            stream = new NAudio.Wave.BlockAlignReductionStream(pcm);
                            output = new NAudio.Wave.DirectSoundOut();
                            output.Init(stream);
                            PlayCurrentTrack();
                        }
                    }
                    if (currentPlayList.Count == 0)
                    {
                        int maxRandomTracks = musicLibrary.GetAll().Count;
                        Random rand = new Random();
                        for (int i = 0; i < maxRandomTracks; i++)
                        {
                            int randomIndex = rand.Next(0, musicLibrary.GetAll().Count);

                            foreach (Track trck in currentPlayList)
                            {
                                if (randomIndex == musicLibrary.GetAll().IndexOf(trck))
                                {
                                    //Console.WriteLine("already there...");
                                    randomIndex = rand.Next(0, musicLibrary.GetAll().Count);
                                }
                            }

                            currentPlayList.Add(musicLibrary.GetAll()[randomIndex]);
                        }
                        currentTrack = currentPlayList[0];
                        currentPlayList.RemoveAt(0);
                        DisposeWave();
                        NAudio.Wave.WaveStream pcm = NAudio.Wave.WaveFormatConversionStream.CreatePcmStream(new NAudio.Wave.Mp3FileReader(currentTrack.GetPath()));
                        stream = new NAudio.Wave.BlockAlignReductionStream(pcm);
                        output = new NAudio.Wave.DirectSoundOut();
                        output.Init(stream);
                        PlayCurrentTrack();
                    }
                    else
                    {
                        PlayCurrentTrack();
                    }
                }
            }
            else
            {
                output.Pause();
                PlayPauseButton.mouseOverImage = playImage;
                PlayPauseButton.Image = PlayPauseButton.mouseOverImage;
                paused = true;
            }
        }

        public void skipForward()
        {
            currentPlaybackPosition = 0;
            if (currentPlayList.Count != 0)
            {
                historyPlayList.Add(currentTrack);
                currentTrack = currentPlayList[0];
                currentPlayList.RemoveAt(0);
                TagLib.File file = TagLib.File.Create(currentTrack.GetPath());

                if (file.Tag.Pictures.Length != 0)
                {
                    MemoryStream ms = new MemoryStream(file.Tag.Pictures[0].Data.Data);
                    PlayPauseButton.BackgroundImage = Image.FromStream(ms);
                }
                else
                {
                    PlayPauseButton.BackgroundImage = Image.FromFile(@"icons\no_cover_art.jpg");
                }

                //MemoryStream ms = new MemoryStream(file.Tag.Pictures[0].Data.Data);
                //PlayPauseButton.BackgroundImage = Image.FromStream(ms);
                trackLabel.Text = currentTrack.GetTitle();
                artistLabel.Text = currentTrack.GetArtists()[0];
                albumLabel.Text = currentTrack.GetAlbum();
                if (!paused)
                {
                    DisposeWave();
                    NAudio.Wave.WaveStream pcm = NAudio.Wave.WaveFormatConversionStream.CreatePcmStream(new NAudio.Wave.Mp3FileReader(currentTrack.GetPath()));
                    stream = new NAudio.Wave.BlockAlignReductionStream(pcm);
                    output = new NAudio.Wave.DirectSoundOut();
                    output.Init(stream);
                    PlayCurrentTrack();
                }
                else
                {
                    DisposeWave();
                    NAudio.Wave.WaveStream pcm = NAudio.Wave.WaveFormatConversionStream.CreatePcmStream(new NAudio.Wave.Mp3FileReader(currentTrack.GetPath()));
                    stream = new NAudio.Wave.BlockAlignReductionStream(pcm);
                    output = new NAudio.Wave.DirectSoundOut();
                    output.Init(stream);
                    TimeLine.Width = 0;
                }
            }
            else
            {
                DisposeWave();
                PlayPauseButton.BackgroundImage = null;
                PlayPauseButton.Image = playImage;
                PlayPauseButton.mouseOverImage = playImage;
                trackLabel.Text = "";
                artistLabel.Text = "";
                albumLabel.Text = "";
                paused = true;
            }
        }

        public void skipBack()
        {
            currentPlaybackPosition = 0;
            if (historyPlayList.Count != 0)
            {
                currentPlayList.Insert(0, currentTrack);
                currentTrack = historyPlayList[historyPlayList.Count - 1];
                historyPlayList.RemoveAt(historyPlayList.Count - 1);
                TagLib.File file = TagLib.File.Create(currentTrack.GetPath());

                if (file.Tag.Pictures.Length != 0)
                {
                    MemoryStream ms = new MemoryStream(file.Tag.Pictures[0].Data.Data);
                    PlayPauseButton.BackgroundImage = Image.FromStream(ms);
                }
                else
                {
                    PlayPauseButton.BackgroundImage = Image.FromFile(@"icons\no_cover_art.jpg");
                }

                //MemoryStream ms = new MemoryStream(file.Tag.Pictures[0].Data.Data);
                //PlayPauseButton.BackgroundImage = Image.FromStream(ms);
                //PlayPauseButton.BackgroundImage = Image.FromFile("icons/ic_skip_previous_white_48dp/web/ic_skip_previous_white_48dp_1x.png");
                trackLabel.Text = currentTrack.GetTitle();
                artistLabel.Text = currentTrack.GetArtists()[0];
                albumLabel.Text = currentTrack.GetAlbum();
                if (!paused)
                {
                    DisposeWave();
                    NAudio.Wave.WaveStream pcm = NAudio.Wave.WaveFormatConversionStream.CreatePcmStream(new NAudio.Wave.Mp3FileReader(currentTrack.GetPath()));
                    stream = new NAudio.Wave.BlockAlignReductionStream(pcm);
                    output = new NAudio.Wave.DirectSoundOut();
                    output.Init(stream);
                    PlayCurrentTrack();
                }
                else
                {
                    DisposeWave();
                    NAudio.Wave.WaveStream pcm = NAudio.Wave.WaveFormatConversionStream.CreatePcmStream(new NAudio.Wave.Mp3FileReader(currentTrack.GetPath()));
                    stream = new NAudio.Wave.BlockAlignReductionStream(pcm);
                    output = new NAudio.Wave.DirectSoundOut();
                    output.Init(stream);
                    TimeLine.Width = 0;
                }
            }
            else
            {
                DisposeWave();
                PlayPauseButton.BackgroundImage = null;
                PlayPauseButton.Image = playImage;
                PlayPauseButton.mouseOverImage = playImage;
                trackLabel.Text = "";
                artistLabel.Text = "";
                albumLabel.Text = "";
                paused = true;
            }
        }

        private void skipForwardButton_Click(object sender, EventArgs e)
        {
            skipForward();
        }

        private void skipBackButton_Click(object sender, EventArgs e)
        {
            skipBack();
        }

        private void TimeLineBack_MouseEnter(object sender, EventArgs e)
        {
            currentMovingTimeLabel.Visible = true;
        }

        private void TimeLineBack_MouseLeave(object sender, EventArgs e)
        {
            currentMovingTimeLabel.Visible = false;
        }

        private void TimeLineBack_MouseMove(object sender, MouseEventArgs e)
        {
            int x = TimeLineBack.Location.X + e.X - currentMovingTimeLabel.Width / 2;
            int y = currentMovingTimeLabel.Location.Y;
            if(stream != null)
            {
                TimeSpan total = stream.TotalTime;
                double currentSeconds = ((double)e.X / (double)TimeLineBack.Width) * total.TotalSeconds; 
                TimeSpan current = new TimeSpan(0, 0, 0, (int)currentSeconds);
                currentMovingTimeLabel.Text = current.ToString().Substring(4);
                if (e.Button == MouseButtons.Left)
                {
                    stream.CurrentTime = current;
                }
            }
            else
            {
                currentMovingTimeLabel.Text = "";
            }
            currentMovingTimeLabel.Location = new Point(x,y);
        }

        private void TimeLine_MouseEnter(object sender, EventArgs e)
        {
            currentMovingTimeLabel.Visible = true;
        }

        private void TimeLine_MouseLeave(object sender, EventArgs e)
        {
            currentMovingTimeLabel.Visible = false;
        }

        private void TimeLine_MouseMove(object sender, MouseEventArgs e)
        {
            int x = TimeLineBack.Location.X + e.X - currentMovingTimeLabel.Width / 2;
            int y = currentMovingTimeLabel.Location.Y;
            if (stream != null)
            {
                TimeSpan total = stream.TotalTime;
                double currentSeconds = ((double)e.X / (double)TimeLineBack.Width) * total.TotalSeconds;
                TimeSpan current = new TimeSpan(0, 0, 0, (int)currentSeconds);
                currentMovingTimeLabel.Text = current.ToString().Substring(4);
                if(e.Button == MouseButtons.Left)
                {
                    stream.CurrentTime = current;
                }
            }
            else
            {
                currentMovingTimeLabel.Text = "";
            }
            currentMovingTimeLabel.Location = new Point(x, y);
        }

        private void TimeLineBack_MouseClick(object sender, MouseEventArgs e)
        {
            if (stream != null)
            {
                TimeSpan total = stream.TotalTime;
                double currentSeconds = ((double)e.X / (double)TimeLineBack.Width) * total.TotalSeconds;
                TimeSpan current = new TimeSpan(0, 0, 0, (int)currentSeconds);
                stream.CurrentTime = current;
            }
        }

        private void TimeLine_MouseClick(object sender, MouseEventArgs e)
        {
            if (stream != null)
            {
                TimeSpan total = stream.TotalTime;
                double currentSeconds = ((double)e.X / (double)TimeLineBack.Width) * total.TotalSeconds;
                TimeSpan current = new TimeSpan(0, 0, 0, (int)currentSeconds);
                stream.CurrentTime = current;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            gHook.unhook();
            //app.readThread.Abort();
            //Console.WriteLine(app.readThread.ThreadState);
            if(musicLibrary != null)
            {
                Console.WriteLine("saving music library...");
                BinaryFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream("MyFile.bin", FileMode.Create, FileAccess.Write, FileShare.None);
                formatter.Serialize(stream, musicLibrary);
                stream.Close();
                Console.WriteLine("music library saved!");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                Console.WriteLine("reading music library...");
                BinaryFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream("MyFile.bin", FileMode.Open, FileAccess.Read, FileShare.Read);
                musicLibrary = (MusicLibrary) formatter.Deserialize(stream);
                stream.Close();
                Console.WriteLine("music library read!");
                foreach (Track track in musicLibrary.GetAll())
                {
                    if (!albums.Contains(track.GetAlbum().ToLower()))
                    {
                        albums.Add(track.GetAlbum().ToLower());
                        CoverArt cover = new CoverArt();
                        cover.album = track.GetAlbum().ToLower();
                        cover.artists = track.GetArtists();
                        TagLib.File file = TagLib.File.Create(track.GetPath());

                        if (file.Tag.Pictures.Length != 0)
                        {
                            MemoryStream ms = new MemoryStream(file.Tag.Pictures[0].Data.Data);
                            cover.Image = Image.FromStream(ms);
                        }
                        else
                        {
                            cover.Image = Image.FromFile(@"icons\no_cover_art.jpg");
                        }

                        //MemoryStream ms = new MemoryStream(file.Tag.Pictures[0].Data.Data);
                        //cover.Image = Image.FromStream(ms);
                        cover.SizeMode = PictureBoxSizeMode.Zoom;
                        albumCovers.Add(cover);

                        MusikPanel.Controls.Add(cover);

                        int n = (MusikPanel.Width - minCoverOffset) / (coverSize + minCoverOffset);
                        int row = (albumCovers.IndexOf(cover) / n);
                        int x = 50 + (coverSize + 50) * (albumCovers.IndexOf(cover)) - row * (n * (coverSize + 50));
                        int y = 50 + row * (coverSize + 50);
                        cover.Location = new Point(x, y);
                        cover.Size = new Size(coverSize, coverSize);

                        AddAlbumPanel(cover);
                    }
                }
            } catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void MusikPanel_Click(object sender, EventArgs e)
        {
            foreach (CoverArt cover in albumCovers)
            {
                if (cover.movedDown)
                {
                    cover.GetAnimation("coverMoveUp").start();
                    cover.movedDown = false;
                }
                if (cover.selected)
                {
                    cover.GetAnimation("coverZoomOut").start();
                    cover.albumPanel.GetAnimation("albumFoldIn").start();
                    cover.selected = false;
                }
            }
            if (MainSidePanel.foldedOut)
            {
                foreach (Control con in MainSidePanel.Controls)
                {
                    if (con.Name != "SidePanelFoldButton")
                    {
                        con.Enabled = false;
                    }
                }
                MainSidePanel.GetAnimation("sidePanelFoldIn").start();
                MusikControls.GetAnimation("musikControlSlideRight").start();
                MainSidePanel.foldedOut = false;
                SidePanelFoldButton.Image = SidePanelFoldButton.neutralImage;
            }
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            Console.WriteLine("size changed...");
            MainSidePanel.Height = this.Height - BottomControlPanel.Height - 39;
            BottomControlPanel.Width = this.Width - 16;
            BottomControlPanel.Location = new Point(0, this.Height - BottomControlPanel.Height - 39);
            if (MusikControls.Location.X != 0)
            {
                MusikControls.Location = new Point(BottomControlPanel.Width / 2 - MusikControls.Width / 2, 0);
            }
            MusikControls.GetAnimation("musikControlSlideRight").SetX(BottomControlPanel.Width / 2 - MusikControls.Width / 2);
            TimeLineBack.Width = BottomControlPanel.Width - 45;
            MusikPanel.Width = this.Width - 68;
            MusikPanel.Height = BottomControlPanel.Location.Y;
            MusikScrollBar.Location = new Point(this.Width - 33, 0);
            MusikScrollBar.Height = MusikPanel.Height;
        }

        public void SortAlbumCoverByName()
        {

        }

        public void RearrangeCovers()
        {
            int n = (MusikPanel.Width - minCoverOffset) / (coverSize + minCoverOffset);
            Console.WriteLine("max covers per row: " + n);
            foreach (CoverArt cover in albumCovers)
            {
                int row = (albumCovers.IndexOf(cover) / n);
                int x = 50 + (coverSize + 50) * (albumCovers.IndexOf(cover)) - row * (n * (coverSize + 50));
                int y = 50 + row * (coverSize + 50);
                cover.Location = new Point(x, y);
                cover.Size = new Size(coverSize, coverSize);

                if (cover.selected)
                {
                    cover.albumPanel.Height = 0;
                    cover.selected = false;
                }

                cover.albumPanel.Width = MusikPanel.Width;
                cover.albumPanel.Location = new Point(0, cover.Location.Y + cover.Height + 10);
                cover.albumPanel.top_shadow.Size = new Size(cover.albumPanel.Width, 10);
                cover.albumPanel.bottom_shadow.Size = new Size(cover.albumPanel.Width, 10);

                cover.movedDown = false;
            }
            double rows = (double)albumCovers.Count / (double)n;
            if ((int)rows < rows)
            {
                rows = (int)rows + 1;
            }
            MusikPanel.Height = 50 + ((int)rows * (coverSize + 50));
            MusikScrollBar.Visible = true;
            if (MusikPanel.Height < BottomControlPanel.Location.Y)
            {
                MusikPanel.Height = BottomControlPanel.Location.Y;
                MusikScrollBar.Visible = false;
            }
            MusikScrollBar.Maximum = MusikPanel.Height - BottomControlPanel.Location.Y;
        }

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            RearrangeCovers();
        }

        private void MusikScrollBar_ValueChanged(object sender, EventArgs e)
        {
            MusikPanel.Location = new Point(MusikPanel.Location.X, -MusikScrollBar.Value);
        }
    }
}
