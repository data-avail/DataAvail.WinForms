using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace DataAvail.WinUtils
{
    //http://www.vcskicks.com/csharp_animated_gif2.php
    public class AnimatedImage
    {
        private Image gifImage;
        private FrameDimension dimension;
        private int frameCount;
        private int currentFrame = -1;
        private bool reverse;
        private int step = 1;
        private System.Timers.Timer _timer = new System.Timers.Timer() { AutoReset = true };
        private bool _timerStarted = false;

        public AnimatedImage(Image Image)
        {
            gifImage = Image; //initialize
            dimension = new FrameDimension(gifImage.FrameDimensionsList[0]); //gets the GUID
            frameCount = gifImage.GetFrameCount(dimension); //total frames in the animation
            _timer.Elapsed += new System.Timers.ElapsedEventHandler(_timer_Elapsed);
        }

        public bool ReverseAtEnd //whether the gif should play backwards when it reaches the end
        {
            get { return reverse; }
            set { reverse = value; }
        }

        public Image GetNextFrame()
        {

            currentFrame += step;

            //if the animation reaches a boundary...
            if (currentFrame >= frameCount || currentFrame < 1)
            {
                if (reverse)
                {
                    step *= -1; //...reverse the count
                    currentFrame += step; //apply it
                }
                else
                    currentFrame = 0; //...or start over
            }
            return GetFrame(currentFrame);
        }

        public Image GetFrame(int index)
        {
            gifImage.SelectActiveFrame(dimension, index); //find the frame
            return (Image)gifImage.Clone(); //return a copy of it
        }

        public void StartAnimation(int Interval)
        {
            if (_timerStarted)
                throw new Exception("Timer already started");

            _timerStarted = true;

            _timer.Interval = Interval;

            _timer.Start();
        }

        public void StopAnimation()
        {
            if (!_timerStarted)
                throw new Exception("Timer not started yet");


            _timer.Stop();

            _timerStarted = false;
        }

        void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            GetNextFrame();

            gifImage.SelectActiveFrame(dimension, currentFrame);
        }

    }
}
