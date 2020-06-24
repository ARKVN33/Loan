using System;
using System.Windows;
using System.Windows.Input;
using DAL.Class;
using Loan.Class;

namespace Loan.Windows
{
    /// <summary>
    /// Interaction logic for WinChangePassword.xaml
    /// </summary>
    public partial class WinChangePassword
    {
        public WinChangePassword()
        {
            InitializeComponent();
        }

        #region Event

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TxtUserName.Focus();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckEmpty()) return;

            var userLogin = new DUserLogin
            {
                DUserName = TxtUserName.Text,
                DUserPassword = TxtPassword.Password
            };
            try
            {
                if (userLogin.Login())
                {

                    var user = new DUser
                    {
                        DUserName = TxtUserName.Text,
                        DPassword = TxtNewPassword.Password
                    };
                    user.ChangePassword();


                    Utility.Message("پیام", "رمز عبور با موفقیت تغییر یافت", "Correct.png");
                }
                else
                {
                    Utility.Message("خطا", "نام کاربری یا کلمه عبور صحیح نمی باشد", "Stop.png");
                }
            }
            catch (Exception exception)
            {
                Utility.MyMessageBox("خطا در بانک اطلاعاتی","خطا در تغییر رمز عبور\n" + exception.Message);
            }

        }

        private void TxtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ImgShowPassword.Visibility = TxtPassword.Password.Length > 0 ? Visibility.Visible : Visibility.Hidden;
        }

        private void TxtNewPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ImgShowNewPassword.Visibility = TxtPassword.Password.Length > 0 ? Visibility.Visible : Visibility.Hidden;
        }

        private void TxtConfirmNewPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ImgShowConfirmNewPassword.Visibility = TxtPassword.Password.Length > 0
                ? Visibility.Visible
                : Visibility.Hidden;
        }

        private void ImgShowPassword_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            TxtVisiblePassword.Visibility = Visibility.Visible;
            TxtPassword.Visibility = Visibility.Hidden;
            TxtVisiblePassword.Text = TxtPassword.Password;
        }

        private void ImgShowNewPassword_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            TxtVisibleNewPassword.Visibility = Visibility.Visible;
            TxtNewPassword.Visibility = Visibility.Hidden;
            TxtVisibleNewPassword.Text = TxtNewPassword.Password;
        }

        private void ImgShowConfirmNewPassword_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            TxtVisibleConfirmNewPassword.Visibility = Visibility.Visible;
            TxtConfirmNewPassword.Visibility = Visibility.Hidden;
            TxtVisibleConfirmNewPassword.Text = TxtConfirmNewPassword.Password;
        }

        private void ImgShowPassword_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            TxtVisiblePassword.Visibility = Visibility.Hidden;
            TxtPassword.Visibility = Visibility.Visible;
            TxtPassword.Focus();
        }

        private void ImgShowNewPassword_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            TxtVisibleNewPassword.Visibility = Visibility.Hidden;
            TxtNewPassword.Visibility = Visibility.Visible;
            TxtNewPassword.Focus();
        }

        private void ImgShowConfirmNewPassword_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            TxtVisibleConfirmNewPassword.Visibility = Visibility.Hidden;
            TxtConfirmNewPassword.Visibility = Visibility.Visible;
            TxtConfirmNewPassword.Focus();
        }

        private void ImgShowPassword_MouseLeave(object sender, MouseEventArgs e)
        {
            TxtVisiblePassword.Visibility = Visibility.Hidden;
            TxtPassword.Visibility = Visibility.Visible;
            TxtPassword.Focus();
        }

        private void ImgShowNewPassword_MouseLeave(object sender, MouseEventArgs e)
        {
            TxtVisibleNewPassword.Visibility = Visibility.Hidden;
            TxtNewPassword.Visibility = Visibility.Visible;
            TxtNewPassword.Focus();
        }

        private void ImgShowConfirmNewPassword_MouseLeave(object sender, MouseEventArgs e)
        {
            TxtVisibleConfirmNewPassword.Visibility = Visibility.Hidden;
            TxtConfirmNewPassword.Visibility = Visibility.Visible;
            TxtConfirmNewPassword.Focus();
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

        #region Method

        private bool CheckEmpty()
        {
            if (TxtUserName.Text.Trim() == string.Empty)
            {
                Utility.Message("خطا", "لطفا نام کاربری را وارد کنید", "Stop.png");
                return false;
            }
            if (TxtPassword.Password.Trim() == string.Empty)
            {
                Utility.Message("خطا", "لطفا کلمه عبور را وارد کنید", "Stop.png");
                return false;
            }
            if (TxtNewPassword.Password.Trim() == string.Empty)
            {
                Utility.Message("خطا", "لطفا کلمه عبور جدید را وارد کنید", "Stop.png");
                return false;
            }
            if (TxtConfirmNewPassword.Password.Trim() == string.Empty)
            {
                Utility.Message("خطا", "لطفا کلمه عبور جدید را مجددا وارد کنید", "Stop.png");
                return false;
            }
            if (TxtNewPassword.Password.Trim() != TxtConfirmNewPassword.Password.Trim())
            {
                Utility.Message("خطا", "کلمه عبور جدید و تکرار کلمه عبور جدید باید برابر باشد", "Stop.png");
                return false;
            }
            return true;
        }

        #endregion
    }
}
