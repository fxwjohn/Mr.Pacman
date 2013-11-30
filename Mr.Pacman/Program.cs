using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

namespace Mr.Pacman
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //Thread to show splash window
            Thread thUI = new Thread(new ThreadStart(ShowSplashWindow));
            thUI.Name = "Splash UI";
            thUI.Priority = ThreadPriority.Normal;
            thUI.IsBackground = true;
            thUI.Start();

            //Thread to load time-consuming resources.
            Thread th = new Thread(new ThreadStart(LoadResources));
            th.Name = "Resource Loader";
            th.Priority = ThreadPriority.Highest;
            th.Start();

            th.Join();

            if (SplashForm != null)
            {
                SplashForm.Invoke(new MethodInvoker(delegate { SplashForm.Close(); }));
            }

            thUI.Join();

            Application.Run(new PacmanForm());
        }

        public static StartForm SplashForm
        {
            get;
            set;
        }

        private static void LoadResources()
        {
            for (int i = 0; i < 12; i++)
            {
                if (SplashForm != null)
                {
                    SplashForm.Invoke(new MethodInvoker(delegate
                    {
                        switch (i % 3)
                        {
                            case 0: SplashForm.label_load.Text = "Loading ."; break;
                            case 1: SplashForm.label_load.Text = "Loading .."; break;
                            case 2: SplashForm.label_load.Text = "Loading ..."; break;
                        }
                     }));
                }
                Thread.Sleep(500);
            }
            SplashForm.Invoke(new MethodInvoker(delegate { }));
        }

        private static void ShowSplashWindow()
        {
            SplashForm = new StartForm();
            Application.Run(SplashForm);
        }
    }
}
