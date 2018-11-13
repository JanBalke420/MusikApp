using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusikApp
{
    class AnimationTimer : Timer
    {
        List<Animation> animations = new List<Animation>();

        override protected void OnTick(EventArgs e)
        {
            foreach(Animation anim in animations)
            {
                anim.iterate();
            }
        }

        public void AddAnimation(Animation anim)
        {
            this.animations.Add(anim);
        }
    }
}
