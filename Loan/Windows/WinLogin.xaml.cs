using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using DAL.Class;
using Loan.Class;

namespace Loan.Windows
{
    /// <summary>
    /// Interaction logic for WinLogin.xaml
    /// </summary>
    public partial class WinLogin
    {
        private PersianDateTime _getDate;
        public bool OkLogin;
        public WinLogin()
        {
            InitializeComponent();
            OkLogin = false;
        }

        private int _counter;
        
        private readonly DispatcherTimer _dispatcherTimer = new DispatcherTimer();

        #region Event
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TxtUserName.Focus();
            DUserLogin.SetSecurityAccess();
            if (!(DUserLogin.SecurityAccess(ref _counter) >= 5))
            {
                _getDate = DUserLogin.Date();
                BtnLogin.IsEnabled = false;
                _dispatcherTimer.Tick += dispatcherTimer_Tick;
                _dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
                _dispatcherTimer.Start();
            }

            var adminRegistered = DUserLogin.ChekAdminRegistered();
            if (adminRegistered != null && (bool)adminRegistered) return;
            Hide();
            var winAddAdmin = new WinAddAdmin();
            winAddAdmin.ShowDialog();
            var chekAdminRegistered = DUserLogin.ChekAdminRegistered();
            if (chekAdminRegistered != null && (bool)chekAdminRegistered)
            {
                Show();
            }
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (DUserLogin.SecurityAccess(ref _counter) >= 5)
            {
                LblNotifiction.Content = string.Empty;
                if (TxtUserName.Text.Trim() == string.Empty)
                    LblNotifiction.Content = "نام کاربری را وارد کنید";

                else if (TxtPassword.Password.Trim() == string.Empty)
                    LblNotifiction.Content = "کلمه عبور را وارد کنید";
                else
                {
                    var userLogin = new DUserLogin
                    {
                        DUserName = TxtUserName.Text,
                        DUserPassword = TxtPassword.Password
                    };
                    try
                    {
                        if (userLogin.Login())
                        {
                            DUserLogin.SaveCounter(0);
                            var mainWindow = new MainWindow();
                            OkLogin = true;
                            mainWindow.Show();
                            Close();
                        }
                        else
                        {
                            Utility.MyMessageBox("خطا", "نام کاربری یا کلمه عبور صحیح نمی باشد");
                            DUserLogin.SaveCounter(++_counter);
                            if (_counter != 5) return;
                            BtnLogin.IsEnabled = false;
                            userLogin.StartSecurityTimeAccess();
                            _getDate = DUserLogin.Date();
                            _dispatcherTimer.Tick += dispatcherTimer_Tick;
                            _dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
                            _dispatcherTimer.Start();
                            DUserLogin.SaveCounter(0);
                        }
                    }
                    catch (Exception exception)
                    {
                        Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در ورود کاربر\n" + exception.Message);
                    }
                }
            }
            else
            {
                Utility.MyMessageBox("خطا", "برای امتحان مجدد 5 دقیقه منتظر بمانید");
            }
        }

        private void BtnOsk_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("osk.exe");
            TxtUserName.Focus();
        }

        private void LblForgetPassword_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var winForgetPassword = new WinForgetPassword();
            winForgetPassword.ShowDialog();
        }

        private void TxtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ImgShowPassword.Visibility = TxtPassword.Password.Length > 0 ? Visibility.Visible : Visibility.Hidden;
        }

        private void ImgShowPassword_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            TxtVisiblePassword.Visibility = Visibility.Visible;
            TxtPassword.Visibility = Visibility.Hidden;
            TxtVisiblePassword.Text = TxtPassword.Password;
        }

        private void ImgShowPassword_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            TxtVisiblePassword.Visibility = Visibility.Hidden;
            TxtPassword.Visibility = Visibility.Visible;
            TxtPassword.Focus();
        }

        private void ImgShowPassword_MouseLeave(object sender, MouseEventArgs e)
        {
            TxtVisiblePassword.Visibility = Visibility.Hidden;
            TxtPassword.Visibility = Visibility.Visible;
            TxtPassword.Focus();
        }

        #endregion

        #region FixedEvent

        private void DragMove(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!OkLogin)
            {
                Application.Current.Shutdown();
            }
        }

        private void Minimize(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        #endregion

        #region Method

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            var currenTime = PersianDateTime.Now;
            var result = _getDate - currenTime;
            var elapsedTime = $"{result.Minutes:00}:{result.Seconds:00}";

            if (_getDate < currenTime)
            {
                _dispatcherTimer.Stop();
                LblNotifiction.Content = string.Empty;
                BtnLogin.IsEnabled = true;
                TxtUserName.Focus();
            }
            else
            {
                _dispatcherTimer.Start();
                LblNotifiction.Content = "لطفا بعداز " + elapsedTime + " مجددا اقدام کنید";
                BtnLogin.IsEnabled = false;
            }
        }

        #endregion
    }
}
