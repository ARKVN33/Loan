using System.Windows;
using System.Windows.Input;

namespace Loan.Windows
{
    /// <summary>
    /// Interaction logic for WinForgetPassword.xaml
    /// </summary>
    public partial class WinForgetPassword
    {
        public WinForgetPassword()
        {
            InitializeComponent();
        }

        #region Event

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        #endregion

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
