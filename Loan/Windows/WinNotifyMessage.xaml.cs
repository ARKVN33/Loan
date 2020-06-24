using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Loan.Windows
{
    /// <summary>
    /// Interaction logic for WinNotifyMessage.xaml
    /// </summary>
    public partial class WinNotifyMessage
    {
        public WinNotifyMessage()
        {
            InitializeComponent();
        }

        private readonly DispatcherTimer _dispatcherTimer = new DispatcherTimer();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _dispatcherTimer.Tick += dispatcherTimer_Tick;
            _dispatcherTimer.Interval = new TimeSpan(0, 0, 10);
            _dispatcherTimer.Start();
        }

        public void Notify(string title, string notifiction, string path)
        {
            LblTitle.Text = title;
            LblNotifiction.Text = notifiction;
            var image = new BitmapImage(new Uri("../Icon/" + path, UriKind.Relative));
            ImgNotifiction.Source = image;
            var desktopWorkingArea = SystemParameters.WorkArea;
            Left = desktopWorkingArea.Right - Width;
            Top = desktopWorkingArea.Bottom - Height;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Closing -= Window_Closing;
            e.Cancel = true;
            var anim = new DoubleAnimation(0, TimeSpan.FromSeconds(.5));
            anim.Completed += (s, _) => Close();
            BeginAnimation(OpacityProperty, anim);
        }

        private void LblClose_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Closing -= Window_Closing;
            var anim = new DoubleAnimation(0, TimeSpan.FromSeconds(.5));
            anim.Completed += (s, _) => Close();
            BeginAnimation(OpacityProperty, anim);
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            _dispatcherTimer.Stop();
            Close();
        }

        public string DisplayedImage => @"Icon / Eye.png";
    }
}
