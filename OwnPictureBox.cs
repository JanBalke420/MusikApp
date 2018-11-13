using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace MusikApp
{
    class TrackButton : Label
    {
        List<Animation> animations = new List<Animation>();

        public Color neutralColor = Color.FromArgb(0, 0, 0);
        public Color mouseOverColor = Color.FromArgb(0, 0, 0);
        public Color mouseDownColor = Color.FromArgb(0, 0, 0);
        public Color selectedColor = Color.FromArgb(0, 0, 0);

        public Color neutralColorText = Color.FromArgb(0, 0, 0);
        public Color mouseOverColorText = Color.FromArgb(0, 0, 0);
        public Color mouseDownColorText = Color.FromArgb(0, 0, 0);
        public Color selectedColorText = Color.FromArgb(0, 0, 0);

        bool selected = false;

        private AnimationTimer ticker = null;

        public int trackID = 0;

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
            Console.WriteLine(name + " does not exist...");
            return new Animation();
        }

        override protected void OnMouseEnter(EventArgs e)
        {
            this.BackColor = mouseOverColor;
            this.ForeColor = mouseOverColorText;
        }

        override protected void OnMouseLeave(EventArgs e)
        {
            this.BackColor = neutralColor;
            this.ForeColor = neutralColorText;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            this.BackColor = mouseDownColor;
            this.ForeColor = mouseDownColorText;
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            this.BackColor = mouseOverColor;
            this.ForeColor = mouseOverColorText;
        }
    }
    class CoverArt : PictureBox
    {
        private AnimationTimer ticker = null;
        List<Animation> animations = new List<Animation>();

        public string album = "";
        public AlbumPanel albumPanel = null;
        public List<string> artists = new List<string>();
        public bool selected = false;
        public bool movedDown = false;


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
            Console.WriteLine(name + " does not exist in " + this.album);
            return new Animation();
        }

    }
    class LabelButton : Label
    {
        List<Animation> animations = new List<Animation>();

        public Color neutralColor = Color.FromArgb(0, 0, 0);
        public Color mouseOverColor = Color.FromArgb(0, 0, 0);
        public Color mouseDownColor = Color.FromArgb(0, 0, 0);

        bool clicked = false;

        private AnimationTimer ticker = null;

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
            Console.WriteLine(name + " does not exist...");
            return new Animation();
        }

        override protected void OnMouseEnter(EventArgs e)
        {
            this.BackColor = mouseOverColor;
        }

        override protected void OnMouseLeave(EventArgs e)
        {
            this.BackColor = neutralColor;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            this.BackColor = mouseDownColor;
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            this.BackColor = mouseOverColor;
        }
    }
    class PictureBoxButton : PictureBox
    {
        List<Animation> animations = new List<Animation>();

        public Color neutralColor = Color.FromArgb(0, 0, 0);
        public Color mouseOverColor = Color.FromArgb(0, 0, 0);
        public Color mouseDownColor = Color.FromArgb(0, 0, 0);

        public Image neutralImage = null;
        public Image mouseOverImage = null;
        public Image mouseDownImage = null;
        public Image clickedImage = null;

        bool clicked = false;

        private AnimationTimer ticker = null;

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
            Console.WriteLine(name + " does not exist...");
            return new Animation();
        }

        override protected void OnMouseEnter(EventArgs e)
        {
            this.BackColor = mouseOverColor;
            this.Image = this.mouseOverImage;
        }

        override protected void OnMouseLeave(EventArgs e)
        {
            this.BackColor = neutralColor;
            this.Image = neutralImage;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            this.BackColor = mouseDownColor;
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            this.BackColor = mouseOverColor;
        }
    }
    class PictureBoxAnim : PictureBox
    {
        List<Animation> animations = new List<Animation>();

        Color neutralColor = Color.FromArgb(0,0,0);
        Color mouseOverColor = Color.FromArgb(0, 0, 0);
        Color mouseDownColor = Color.FromArgb(0, 0, 0);

        bool clicked = false;

        private AnimationTimer ticker = null;

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
            foreach(Animation anim in animations)
            {
                if(anim.name == name)
                {
                    return anim;
                }
            }
            Console.WriteLine(name + " does not exist...");
            return new Animation();
        }

        override protected void OnMouseEnter(EventArgs e)
        {
            this.BackColor = mouseOverColor;
        }

        override protected void OnMouseLeave(EventArgs e)
        {
            this.BackColor = neutralColor;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            this.BackColor = mouseDownColor;
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            this.BackColor = neutralColor;
        }
    }
}
