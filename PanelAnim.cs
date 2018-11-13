using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace MusikApp
{
    class AlbumPanel : Panel
    {
        private AnimationTimer ticker = null;
        List<Animation> animations = new List<Animation>();

        public bool foldedOut = false;

        public string album = "";
        public List<string> artists = new List<string>();
        public Image cover = null;

        public PictureBox top_shadow = new PictureBox();
        public PictureBox bottom_shadow = new PictureBox();
        //public List<int> trackIDs = new List<int>();
        public List<TrackButton> trackButtons = new List<TrackButton>();

        public void AddAnimation(Animation anim)
        {
            this.animations.Add(anim);
        }

        public void SetAnimTimer(AnimationTimer ticker)
        {
            this.ticker = ticker;
        }

        public Animation GetAnimation(string name)
        {
            foreach (Animation anim in animations)
            {
                if (anim.name == name)
                {
                    return anim;
                }
            }
            return new Animation();
        }

        public void AddTrackControls()
        {
            foreach(TrackButton btn in trackButtons)
            {
                this.Controls.Add(btn);
            }
        }
    }
    class SidePanel : Panel
    {
        private AnimationTimer ticker = null;
        List<Animation> animations = new List<Animation>();

        public bool foldedOut = false;

        public void AddAnimation(Animation anim)
        {
            this.animations.Add(anim);
        }

        public void SetAnimTimer(AnimationTimer ticker)
        {
            this.ticker = ticker;
        }

        public Animation GetAnimation(string name)
        {
            foreach (Animation anim in animations)
            {
                if (anim.name == name)
                {
                    return anim;
                }
            }
            return new Animation();
        }
    }

    class ControlPanel : Panel
    {
        private AnimationTimer ticker = null;
        List<Animation> animations = new List<Animation>();

        public void AddAnimation(Animation anim)
        {
            this.animations.Add(anim);
        }

        public void SetAnimTimer(AnimationTimer ticker)
        {
            this.ticker = ticker;
        }

        public Animation GetAnimation(string name)
        {
            foreach (Animation anim in animations)
            {
                if (anim.name == name)
                {
                    return anim;
                }
            }
            return new Animation();
        }
    }
}
