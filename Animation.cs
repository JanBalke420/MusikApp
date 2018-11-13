using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace MusikApp
{
    class KeyFrame
    {
        private int x = 0;
        private int y = 0;
        private int w = 0;
        private int h = 0;
        private int timePoint = 0;

        public KeyFrame(int x, int y, int w, int h, int timePoint)
        {
            this.x = x;
            this.y = y;
            this.w = w;
            this.h = h;
            this.timePoint = timePoint;
        }
    }
    class Animation
    {
        private AnimationTimer animationTicker = null;

        //transformation modes
        public static int RELATIVE = 0;
        public static int ABSOLUTE = 1;

        //transformation
        public static int MOVE = 0;
        public static int SCALE = 1;

        //pivot points
        public static int TopLeft = 0;
        public static int TopCenter = 1;
        public static int TopRight = 2;
        public static int CenterLeft = 3;
        public static int CenterCenter = 4;
        public static int CenterRight = 5;
        public static int BottomLeft = 6;
        public static int BottomCenter = 7;
        public static int BottomRight = 8;

        //smooth options
        public static int LINEAR = 0;
        public static int SMOOTH = 1;
        public static int SMOOTHSTART = 1;
        public static int SMOOTHEND = 1;

        Control parent = null;
        public string name = "";

        private int transformMode = 0;
        private int transform = 0;
        private int smoothOption = 1;

        private int pivotIndex = 0;
        private int pivot_x = 0;
        private int pivot_y = 0;

        private bool customKeys = false;
        private List<KeyFrame> keyFrames = new List<KeyFrame>();

        private int x = 0;
        private int y = 0;
        private int w = 0;
        private int h = 0;

        private List<float> x_keys = new List<float>();
        private List<float> y_keys = new List<float>();
        private List<float> w_keys = new List<float>();
        private List<float> h_keys = new List<float>();

        private int milliseconds = 0;

        private float iterations = 0;
        private int iterationCounter = 0;

        public bool running = false;
        private bool is_empty = false;

        public Animation()
        {
            Console.WriteLine("emptyAnimation");
            this.is_empty = true;
        }
        public Animation(Control parent, string name, int transform, int a, int b, int ms, AnimationTimer ticker)
        {
            animationTicker = ticker;
            animationTicker.AddAnimation(this);
            this.transform = transform;
            this.parent = parent;

            this.iterations = (ms / animationTicker.Interval) +1;

            this.name = name;

            if (this.transform == 0)
            {
                this.x = a;
                this.y = b;
            }
            else if (this.transform == 1)
            {
                this.w = a;
                this.h = b;
            }
        }

        public Animation(Control parent, string name, int transform, List<KeyFrame> keyFrames, int ms, AnimationTimer ticker)
        {
            animationTicker = ticker;
            animationTicker.AddAnimation(this);
            this.transform = transform;
            this.parent = parent;

            this.iterations = (ms / animationTicker.Interval) + 1;

            this.name = name;

        }

        public void start()
        {
            if (this.is_empty)
            {
                Console.WriteLine("you are trying to start an empty animation...");
            }
            else
            {
                if (customKeys)
                {
                    Console.WriteLine("custom keys");
                    //CreateCustomKeys();
                }
                else
                {
                    CreateKeys();
                }
                changePivot();

                x_keys = SmoothKeys(x_keys);
                y_keys = SmoothKeys(y_keys);
                w_keys = SmoothKeys(w_keys);
                h_keys = SmoothKeys(h_keys);

                this.running = true;
                Console.WriteLine(this.name + " animation started");
            }
        }

        public void abort()
        {
            this.running = false;
            x_keys.Clear();
            y_keys.Clear();
            w_keys.Clear();
            h_keys.Clear();
            this.iterationCounter = 0;
            Console.WriteLine(this.name + " animation aborted");
        }

        public void CreateCustomKeys()
        {

        }

        public void CreateKeys()
        {
            if(transform == 0)
            {
                for (int i = 0; i < iterations; i++)
                {
                    this.x_keys.Add(this.parent.Location.X + ((this.x - transformMode*this.parent.Location.X) / (this.iterations-1)) * (i));
                    this.y_keys.Add(this.parent.Location.Y + ((this.y - transformMode * this.parent.Location.Y) / (this.iterations - 1)) * (i));
                    this.w_keys.Add(this.parent.Width);
                    this.h_keys.Add(this.parent.Height);
                }
            }
            else if(transform == 1)
            {
                for (int i = 0; i < iterations; i++)
                {
                    this.x_keys.Add(this.parent.Location.X - (pivot_x / (this.iterations - 1)) * (i));
                    this.y_keys.Add(this.parent.Location.Y - (pivot_y / (this.iterations - 1)) * (i));
                    this.w_keys.Add(this.parent.Width + ((this.w - transformMode * this.parent.Width) / (this.iterations - 1)) * (i));
                    this.h_keys.Add(this.parent.Height + ((this.h - transformMode * this.parent.Height) / (this.iterations - 1)) * (i));
                }
            }
        }

        public void iterate()
        {
            if (parent != null && this.running)
            {
                if(iterationCounter == iterations)
                {
                    this.running = false;
                    x_keys.Clear();
                    y_keys.Clear();
                    w_keys.Clear();
                    h_keys.Clear();
                    this.iterationCounter = 0;
                    Console.WriteLine(this.name + " animation ended");
                }
                else
                {
                    parent.Location = new Point((int)x_keys[iterationCounter], (int)y_keys[iterationCounter]);
                    parent.Width = (int)w_keys[iterationCounter];
                    parent.Height = (int)h_keys[iterationCounter];

                    iterationCounter += 1;
                }
            }
        }

        public void SetTransMode(int mode)
        {
            this.transformMode = mode;
        }

        public void SetPivot(int pivot)
        {
            this.pivotIndex = pivot;
            changePivot();
        }

        private void changePivot()
        {
            switch (this.pivotIndex)
            {
                case 0:
                    pivot_x = 0;
                    pivot_y = 0;
                    break;
                case 1:
                    pivot_x = (this.w - transformMode * this.parent.Width) / 2;
                    pivot_y = 0;
                    break;
                case 2:
                    pivot_x = (this.w - transformMode * this.parent.Width);
                    pivot_y = 0;
                    break;
                case 3:
                    pivot_x = 0;
                    pivot_y = (this.h - transformMode * this.parent.Height) / 2;
                    break;
                case 4:
                    pivot_x = (this.w - transformMode * this.parent.Width) / 2;
                    pivot_y = (this.h - transformMode * this.parent.Height) / 2;
                    break;
                case 5:
                    pivot_x = (this.w - transformMode * this.parent.Width);
                    pivot_y = (this.h - transformMode * this.parent.Height) / 2;
                    break;
                case 6:
                    pivot_x = 0;
                    pivot_y = (this.h - transformMode * this.parent.Height);
                    break;
                case 7:
                    pivot_x = (this.w - transformMode * this.parent.Width) / 2;
                    pivot_y = (this.h - transformMode * this.parent.Height);
                    break;
                case 8:
                    pivot_x = (this.w - transformMode * this.parent.Width);
                    pivot_y = (this.h - transformMode * this.parent.Height);
                    break;
            }
        }

        public void SetSmooth(int opt)
        {
            this.smoothOption = opt;
        }

        public List<float> SmoothKeys(List<float> keys)
        {
            int start = (int)keys[0];
            int end = (int)keys[keys.Count - 1];
            double border_x = 4;
            double border_y = TanH(border_x);
            double min = keys.Min();
            double stretch_x = (border_x*2)/(double)keys.Count;
            float stretch_y = (keys.Max()-keys.Min())/2;
            switch (smoothOption)
            {
                case 0:
                    break;
                case 1:
                    if (keys[0] - keys[1] < 0)
                    {
                        for (int i = 0; i < keys.Count; i++)
                        {
                            double stretched_value = ((double)i * stretch_x) - border_x;
                            double transformed_value = TanH(stretched_value);
                            double destretched_value = (transformed_value + border_y) * stretch_y + min;
                            //Console.WriteLine(keys[i] + " - " + Math.Round(destretched_value, 0, MidpointRounding.AwayFromZero));
                            keys[i] = (float)Math.Round(destretched_value, 0, MidpointRounding.AwayFromZero);
                        }
                        if(keys[keys.Count-1] != end)
                        {
                            keys[keys.Count - 1] = end;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < keys.Count; i++)
                        {
                            double stretched_value = ((double)i * stretch_x) - border_x;
                            double transformed_value = TanH(stretched_value);
                            double destretched_value = (transformed_value + border_y) * stretch_y + min;
                            keys[keys.Count-1-i] = (float)Math.Round(destretched_value, 0, MidpointRounding.AwayFromZero);
                        }
                        if (keys[keys.Count - 1] != end)
                        {
                            keys[keys.Count - 1] = end;
                        }
                    }
                    break;
            }
            return keys;
        }

        public double TanH(double x)
        {
            return (2 / (1 + Math.Pow(Math.E, (-2 * x)))) - 1;
        }

        public int GetLength()
        {
            return this.milliseconds;
        }

        public void SetX(int x)
        {
            this.x = x;
        }

        public void SetY(int y)
        {
            this.y = y;
        }

        public void SetW(int w)
        {
            this.w = w;
        }

        public void SetH(int h)
        {
            this.h = h;
        }
    }
}