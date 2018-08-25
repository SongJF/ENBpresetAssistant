using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace ENBpresetAssistant.Components
{
    class LoadingWaitingHandler
    {
        private BaseShowDialog dialog;
        private int seconds = 12;
        //如果在seconds秒之后还未调用End(),则自己调用停止缓冲动画
        protected DispatcherTimer timer;
        protected Thread UiThread;

        //要在那个窗口上显示缓冲动画
        private Window TargetWindow;
        private Double PreLeft;//距离屏幕左侧的距离
        private Double PreTop;//距离屏幕上侧的距离
        public LoadingWaitingHandler(Window targetWindow, int afterClose = 12)
        {
            TargetWindow = targetWindow;

            PreLeft = TargetWindow.Left + TargetWindow.ActualWidth / 2;
            PreTop = TargetWindow.Top + TargetWindow.ActualHeight / 2;
            seconds = afterClose;
        }
        public void Start()
        {
            UiThread = new Thread(new ThreadStart(ThreadStartingPoint));
            UiThread.SetApartmentState(ApartmentState.STA);
            UiThread.IsBackground = true;
            UiThread.Start();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(seconds);
            timer.Tick += (ss, ee) =>
            {
                End();
                timer.Stop();
            };
            timer.Start();
        }
        private void ThreadStartingPoint()
        {
            LoadingOneRing LoadingUC = new LoadingOneRing();
            LoadingUC.Width = 28;
            LoadingUC.Height = 28;
            LoadingUC.Foreground = new SolidColorBrush()
            {
                Color = Color.FromArgb(0x8F, 0xFF, 0xFF, 0xFF)
            };
            dialog = new BaseShowDialog(LoadingUC);
            dialog.Left = PreLeft - dialog.Width / 2;
            dialog.Top = PreTop - dialog.Height / 2;
            dialog.Topmost = true;
            dialog.Show();
            Dispatcher.Run();
        }
        public void End()
        {
            dialog.Dispatcher.Invoke(async () =>
            {
                try
                {
                    timer.Stop();
                    await Task.Delay(150);
                    dialog.Close();
                }
                finally { }
            });
        }

    }
}
