namespace MusikApp
{
    partial class Form1
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.MusikPanel = new System.Windows.Forms.Panel();
            this.MusikScrollBar = new System.Windows.Forms.VScrollBar();
            this.BottomControlPanel = new MusikApp.ControlPanel();
            this.currentMovingTimeLabel = new System.Windows.Forms.Label();
            this.TimeLine = new System.Windows.Forms.PictureBox();
            this.TimeLineBack = new System.Windows.Forms.PictureBox();
            this.MusikControls = new MusikApp.ControlPanel();
            this.albumLabel = new System.Windows.Forms.Label();
            this.artistLabel = new System.Windows.Forms.Label();
            this.trackLabel = new System.Windows.Forms.Label();
            this.PlayPauseButton = new MusikApp.PictureBoxButton();
            this.skipForwardButton = new MusikApp.PictureBoxButton();
            this.skipBackButton = new MusikApp.PictureBoxButton();
            this.MainSidePanel = new MusikApp.SidePanel();
            this.SidePanelFoldButton = new MusikApp.PictureBoxButton();
            this.AddMusicButton = new MusikApp.LabelButton();
            this.BottomControlPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TimeLine)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TimeLineBack)).BeginInit();
            this.MusikControls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PlayPauseButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.skipForwardButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.skipBackButton)).BeginInit();
            this.MainSidePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SidePanelFoldButton)).BeginInit();
            this.SuspendLayout();
            // 
            // MusikPanel
            // 
            this.MusikPanel.BackColor = System.Drawing.Color.WhiteSmoke;
            this.MusikPanel.Location = new System.Drawing.Point(50, 0);
            this.MusikPanel.Name = "MusikPanel";
            this.MusikPanel.Size = new System.Drawing.Size(2492, 1220);
            this.MusikPanel.TabIndex = 2;
            this.MusikPanel.Click += new System.EventHandler(this.MusikPanel_Click);
            // 
            // MusikScrollBar
            // 
            this.MusikScrollBar.Location = new System.Drawing.Point(2527, 0);
            this.MusikScrollBar.Name = "MusikScrollBar";
            this.MusikScrollBar.Size = new System.Drawing.Size(15, 1301);
            this.MusikScrollBar.TabIndex = 3;
            this.MusikScrollBar.ValueChanged += new System.EventHandler(this.MusikScrollBar_ValueChanged);
            // 
            // BottomControlPanel
            // 
            this.BottomControlPanel.BackColor = System.Drawing.Color.DodgerBlue;
            this.BottomControlPanel.Controls.Add(this.currentMovingTimeLabel);
            this.BottomControlPanel.Controls.Add(this.TimeLine);
            this.BottomControlPanel.Controls.Add(this.TimeLineBack);
            this.BottomControlPanel.Controls.Add(this.MusikControls);
            this.BottomControlPanel.Location = new System.Drawing.Point(0, 1301);
            this.BottomControlPanel.Name = "BottomControlPanel";
            this.BottomControlPanel.Size = new System.Drawing.Size(2545, 100);
            this.BottomControlPanel.TabIndex = 1;
            // 
            // currentMovingTimeLabel
            // 
            this.currentMovingTimeLabel.BackColor = System.Drawing.Color.Transparent;
            this.currentMovingTimeLabel.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.currentMovingTimeLabel.ForeColor = System.Drawing.Color.White;
            this.currentMovingTimeLabel.Location = new System.Drawing.Point(339, 65);
            this.currentMovingTimeLabel.Name = "currentMovingTimeLabel";
            this.currentMovingTimeLabel.Size = new System.Drawing.Size(50, 10);
            this.currentMovingTimeLabel.TabIndex = 3;
            this.currentMovingTimeLabel.Text = "0:00";
            this.currentMovingTimeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.currentMovingTimeLabel.Visible = false;
            // 
            // TimeLine
            // 
            this.TimeLine.BackColor = System.Drawing.Color.White;
            this.TimeLine.Location = new System.Drawing.Point(23, 78);
            this.TimeLine.Name = "TimeLine";
            this.TimeLine.Size = new System.Drawing.Size(500, 10);
            this.TimeLine.TabIndex = 2;
            this.TimeLine.TabStop = false;
            this.TimeLine.MouseClick += new System.Windows.Forms.MouseEventHandler(this.TimeLine_MouseClick);
            this.TimeLine.MouseEnter += new System.EventHandler(this.TimeLine_MouseEnter);
            this.TimeLine.MouseLeave += new System.EventHandler(this.TimeLine_MouseLeave);
            this.TimeLine.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TimeLine_MouseMove);
            // 
            // TimeLineBack
            // 
            this.TimeLineBack.BackColor = System.Drawing.Color.LightSkyBlue;
            this.TimeLineBack.Location = new System.Drawing.Point(23, 78);
            this.TimeLineBack.Name = "TimeLineBack";
            this.TimeLineBack.Size = new System.Drawing.Size(2500, 10);
            this.TimeLineBack.TabIndex = 1;
            this.TimeLineBack.TabStop = false;
            this.TimeLineBack.MouseClick += new System.Windows.Forms.MouseEventHandler(this.TimeLineBack_MouseClick);
            this.TimeLineBack.MouseEnter += new System.EventHandler(this.TimeLineBack_MouseEnter);
            this.TimeLineBack.MouseLeave += new System.EventHandler(this.TimeLineBack_MouseLeave);
            this.TimeLineBack.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TimeLineBack_MouseMove);
            // 
            // MusikControls
            // 
            this.MusikControls.BackColor = System.Drawing.Color.Transparent;
            this.MusikControls.Controls.Add(this.albumLabel);
            this.MusikControls.Controls.Add(this.artistLabel);
            this.MusikControls.Controls.Add(this.trackLabel);
            this.MusikControls.Controls.Add(this.PlayPauseButton);
            this.MusikControls.Controls.Add(this.skipForwardButton);
            this.MusikControls.Controls.Add(this.skipBackButton);
            this.MusikControls.Location = new System.Drawing.Point(1222, 0);
            this.MusikControls.Name = "MusikControls";
            this.MusikControls.Size = new System.Drawing.Size(300, 70);
            this.MusikControls.TabIndex = 0;
            // 
            // albumLabel
            // 
            this.albumLabel.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.albumLabel.ForeColor = System.Drawing.Color.White;
            this.albumLabel.Location = new System.Drawing.Point(140, 47);
            this.albumLabel.Name = "albumLabel";
            this.albumLabel.Size = new System.Drawing.Size(90, 13);
            this.albumLabel.TabIndex = 4;
            this.albumLabel.Text = "Albumname";
            this.albumLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // artistLabel
            // 
            this.artistLabel.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.artistLabel.ForeColor = System.Drawing.Color.White;
            this.artistLabel.Location = new System.Drawing.Point(140, 30);
            this.artistLabel.Name = "artistLabel";
            this.artistLabel.Size = new System.Drawing.Size(90, 13);
            this.artistLabel.TabIndex = 3;
            this.artistLabel.Text = "Artistname";
            this.artistLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // trackLabel
            // 
            this.trackLabel.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.trackLabel.ForeColor = System.Drawing.Color.White;
            this.trackLabel.Location = new System.Drawing.Point(140, 10);
            this.trackLabel.Name = "trackLabel";
            this.trackLabel.Size = new System.Drawing.Size(90, 13);
            this.trackLabel.TabIndex = 2;
            this.trackLabel.Text = "Trackname";
            this.trackLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PlayPauseButton
            // 
            this.PlayPauseButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.PlayPauseButton.Location = new System.Drawing.Point(70, 3);
            this.PlayPauseButton.Name = "PlayPauseButton";
            this.PlayPauseButton.Size = new System.Drawing.Size(64, 64);
            this.PlayPauseButton.TabIndex = 1;
            this.PlayPauseButton.TabStop = false;
            this.PlayPauseButton.Click += new System.EventHandler(this.PlayPauseButton_Click);
            // 
            // skipForwardButton
            // 
            this.skipForwardButton.Location = new System.Drawing.Point(240, 10);
            this.skipForwardButton.Name = "skipForwardButton";
            this.skipForwardButton.Size = new System.Drawing.Size(50, 50);
            this.skipForwardButton.TabIndex = 0;
            this.skipForwardButton.TabStop = false;
            this.skipForwardButton.Click += new System.EventHandler(this.skipForwardButton_Click);
            // 
            // skipBackButton
            // 
            this.skipBackButton.Location = new System.Drawing.Point(10, 10);
            this.skipBackButton.Name = "skipBackButton";
            this.skipBackButton.Size = new System.Drawing.Size(50, 50);
            this.skipBackButton.TabIndex = 0;
            this.skipBackButton.TabStop = false;
            this.skipBackButton.Click += new System.EventHandler(this.skipBackButton_Click);
            // 
            // MainSidePanel
            // 
            this.MainSidePanel.BackColor = System.Drawing.Color.DimGray;
            this.MainSidePanel.Controls.Add(this.SidePanelFoldButton);
            this.MainSidePanel.Controls.Add(this.AddMusicButton);
            this.MainSidePanel.Location = new System.Drawing.Point(0, 0);
            this.MainSidePanel.Name = "MainSidePanel";
            this.MainSidePanel.Size = new System.Drawing.Size(300, 1403);
            this.MainSidePanel.TabIndex = 0;
            this.MainSidePanel.Click += new System.EventHandler(this.MainSidePanel_Click);
            this.MainSidePanel.MouseEnter += new System.EventHandler(this.MainSidePanel_MouseEnter);
            this.MainSidePanel.MouseLeave += new System.EventHandler(this.MainSidePanel_MouseLeave);
            // 
            // SidePanelFoldButton
            // 
            this.SidePanelFoldButton.BackColor = System.Drawing.Color.DodgerBlue;
            this.SidePanelFoldButton.Location = new System.Drawing.Point(250, 0);
            this.SidePanelFoldButton.Name = "SidePanelFoldButton";
            this.SidePanelFoldButton.Size = new System.Drawing.Size(50, 50);
            this.SidePanelFoldButton.TabIndex = 2;
            this.SidePanelFoldButton.TabStop = false;
            this.SidePanelFoldButton.Click += new System.EventHandler(this.SidePanelFoldButton_Click);
            // 
            // AddMusicButton
            // 
            this.AddMusicButton.BackColor = System.Drawing.Color.Maroon;
            this.AddMusicButton.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AddMusicButton.ForeColor = System.Drawing.Color.White;
            this.AddMusicButton.Image = ((System.Drawing.Image)(resources.GetObject("AddMusicButton.Image")));
            this.AddMusicButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.AddMusicButton.Location = new System.Drawing.Point(0, 150);
            this.AddMusicButton.Name = "AddMusicButton";
            this.AddMusicButton.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.AddMusicButton.Size = new System.Drawing.Size(300, 50);
            this.AddMusicButton.TabIndex = 1;
            this.AddMusicButton.Text = "           Musik hinzufügen";
            this.AddMusicButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.AddMusicButton.Click += new System.EventHandler(this.AddMusicButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(2544, 1401);
            this.Controls.Add(this.MusikScrollBar);
            this.Controls.Add(this.BottomControlPanel);
            this.Controls.Add(this.MainSidePanel);
            this.Controls.Add(this.MusikPanel);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResizeEnd += new System.EventHandler(this.Form1_ResizeEnd);
            this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
            this.BottomControlPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.TimeLine)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TimeLineBack)).EndInit();
            this.MusikControls.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PlayPauseButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.skipForwardButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.skipBackButton)).EndInit();
            this.MainSidePanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SidePanelFoldButton)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private SidePanel MainSidePanel;
        private LabelButton AddMusicButton;
        private PictureBoxButton SidePanelFoldButton;
        private ControlPanel BottomControlPanel;
        private ControlPanel MusikControls;
        private PictureBoxButton skipBackButton;
        private System.Windows.Forms.Label trackLabel;
        private PictureBoxButton PlayPauseButton;
        private PictureBoxButton skipForwardButton;
        private System.Windows.Forms.Panel MusikPanel;
        private System.Windows.Forms.Label albumLabel;
        private System.Windows.Forms.Label artistLabel;
        private System.Windows.Forms.PictureBox TimeLineBack;
        private System.Windows.Forms.PictureBox TimeLine;
        private System.Windows.Forms.Label currentMovingTimeLabel;
        private System.Windows.Forms.VScrollBar MusikScrollBar;
    }
}

