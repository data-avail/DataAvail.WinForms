using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace DataAvail.WinUtils
{
    public partial class SplashScreen : Form
    {
        public SplashScreen()
        {
            InitializeComponent();
        }

        private SplashScreen(string ImgPath)
        {
            InitializeComponent();
       
            if (!string.IsNullOrEmpty(ImgPath))
            {
                try
                {
                    this.BackgroundImage = Image.FromFile(ImgPath);
                }
                catch (System.IO.FileNotFoundException)
                { }
            }

            this.Opacity = .0;
            _timer.Interval = TIMER_INTERVAL;
            
        }

        static Thread _ssThread;

        static SplashScreen _ssForm;

        private double _opacityIncrement = .05;
        private const int TIMER_INTERVAL = 100;
        private System.Windows.Forms.Timer _timer = new System.Windows.Forms.Timer();

        protected override void OnLoad(EventArgs e)
        {
            _timer.Tick += new EventHandler(_timer_Tick);
            _timer.Start();

            base.OnLoad(e);
        }

        void _timer_Tick(object sender, EventArgs e)
        {
            if (this.Opacity < 1)
                this.Opacity += _opacityIncrement;
            else
                _timer.Stop();

        }


        static public void ShowSplashScreen(string ImagePath)
        {
            if (_ssForm != null)
                throw new InvalidOperationException("Splash screen already shown");

            _ssForm = new SplashScreen(ImagePath);

            _ssThread = new Thread(new ThreadStart(ShowForm));
            _ssThread.IsBackground = true;
            _ssThread.SetApartmentState(ApartmentState.STA);
            _ssThread.Start();
        }

        static public void HideSplashScreen()
        {
            if (_ssForm == null)
                throw new InvalidOperationException("Splash screen not shown shown");

            _ssForm.BeginInvoke(new MethodInvoker(_ssForm.Close));

            _ssThread = null;  

            _ssForm = null;
        }

        static private void ShowForm()
        {
            _ssForm.ShowDialog();
        }
    }
}
