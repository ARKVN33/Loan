using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Loan.Windows
{
    /// <summary>
    /// Interaction logic for WinMessageBox.xaml
    /// </summary>
    public partial class WinMessageBox
    {
        public string Message { get; set; }
        public string MessageTitle { get; set; }
        public string MessageImage { get; set; }

        public bool Ok { get; set; }
        public bool YesNo { get; set; }

        public WinMessageBox()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LblTitle.Content = MessageTitle;
            TxtMessage.Text = Message;
            var image = new BitmapImage(new Uri("../Icon/" + MessageImage, UriKind.Relative));
            ImgMessage.Source = image;
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            Ok = true;
            Close();
        }

        private void BtnYes_Click(object sender, RoutedEventArgs e)
        {
            YesNo = true;
            Close();
        }

        private void BtnNo_Click(object sender, RoutedEventArgs e)
        {
            YesNo = false;
            Close();
        }

        #region FixedEvent

        private void DragMove(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Minimize(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        #endregion
    }
}
