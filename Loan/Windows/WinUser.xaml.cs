using System;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using DAL.Class;
using Loan.Class;
using Application = System.Windows.Application;
using Clipboard = System.Windows.Clipboard;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using Path = System.IO.Path;

namespace Loan.Windows
{
    /// <summary>
    /// Interaction logic for WinUser.xaml
    /// </summary>
    public partial class WinUser
    {
        private OpenFileDialog _openFileDialog;
        private bool _selectedPicture;
        private string _fileName;
        private string _filePath;
        private InputLanguage _currentInputLanguage;

        //estekhrag masir jari ejra barnameh
        private readonly string _currentDirectory;
        private readonly string _directoryPath;
        public WinUser()
        {
            InitializeComponent();
            _currentDirectory = Directory.GetCurrentDirectory();
            _directoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "ARKVN");
        }
        #region Event
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TxtUserFirstName.Focus();
            try
            {
                CboSecurityQuestion.ItemsSource = await DUser.GetQuestion(); //Ezafe kardan maghadir combobox az db
            }
            catch (Exception exception)
            {
                Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در دریافت اطلاعات\n" + exception.Message);
                return;
            }
            CboSecurityQuestion.SelectedIndex = 0;//Entekhab avalin mored va nemayesh an
        }

        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckEmpty() || !Utility.CheckEmail(TxtUserEmail.Text)) return;

            //Zakhireh moshakhasat Admin dar db
            try
            {
                var user = new DUser
                {
                    DPostTypeId = 1,
                    DSecurityQuestionId = (byte)(CboSecurityQuestion.SelectedIndex + 1),
                    DFirstName = TxtUserFirstName.Text,
                    DLastName = TxtUserLastName.Text,
                    DUserName = TxtUserName.Text,
                    DPassword = TxtUserPassword.Password,
                    DMobile = TxtUserMobile.Text.Trim() == string.Empty ? null : TxtUserMobile.Text,
                    DEmail = TxtUserEmail.Text.Trim() == string.Empty ? null : TxtUserEmail.Text,
                    DAnswer = TxtUserAnswer.Text,
                    DRegistrationDate = PersianDateTime.Now.ToString(),
                    DImage = _selectedPicture ? SelectePicture() : null,
                    DDescription = TxtUserDescription.Text.Trim() == string.Empty ? null : TxtUserDescription.Text

                };

                await Task.Run(() => user.AddAdmin());
            }
            catch (Exception exception)
            {
                Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در ثبت اطلاعات\n" + exception.Message);
                return;
            }
            Utility.MyMessageBox("پیام", "ثبت نام مدیریت با موفقیت انجام شد","Correct.png");
            Hide();
        }

        private void BtnChoosePicture_Click(object sender, RoutedEventArgs e)
        {
            _openFileDialog = new OpenFileDialog
            {
                Filter = @"Image Files (*.jpg;*.png;*.bmp) |*.jpg;*.png;*.bmp",
                Title = "انتخاب عکس"
            };
            try
            {
                if (_openFileDialog.ShowDialog() != true) return;

                _selectedPicture = true;
                ImgAdminPicture.Source = Utility.DisplayImage(_openFileDialog.FileName);
                //استخراج نام کامل عکس
                _fileName = Path.GetFileName(_openFileDialog.FileName);
                _filePath = Path.GetFullPath(_openFileDialog.FileName);
            }
            catch (Exception)
            {
                ImgAdminPicture.Source = new BitmapImage(new Uri(_currentDirectory + @"\Icon\Boss.png"));
                Utility.Message("اخطار", "عکس یافت نشد", "Stop.png");
            }
        }

        private void TxtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ImgShowPassword.Visibility = TxtUserPassword.Password.Length > 0 ? Visibility.Visible : Visibility.Hidden;
        }

        private void ImgShowPassword_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            TxtVisibleUserPassword.Visibility = Visibility.Visible;
            TxtUserPassword.Visibility = Visibility.Hidden;
            TxtVisibleUserPassword.Text = TxtUserPassword.Password;
        }

        private void ImgShowPassword_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            TxtVisibleUserPassword.Visibility = Visibility.Hidden;
            TxtUserPassword.Visibility = Visibility.Visible;
            TxtUserPassword.Focus();
        }

        private void ImgShowPassword_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            TxtVisibleUserPassword.Visibility = Visibility.Hidden;
            TxtUserPassword.Visibility = Visibility.Visible;
            TxtUserPassword.Focus();
        }

        private void TxtConfirmPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ImgShowConfirmPassword.Visibility = TxtConfirmPassword.Password.Length > 0
                ? Visibility.Visible
                : Visibility.Hidden;
        }

        private void ImgShowConfirmPassword_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            TxtVisibleConfirmPassword.Visibility = Visibility.Visible;
            TxtConfirmPassword.Visibility = Visibility.Hidden;
            TxtVisibleConfirmPassword.Text = TxtConfirmPassword.Password;
        }

        private void ImgShowConfirmPassword_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            TxtVisibleConfirmPassword.Visibility = Visibility.Hidden;
            TxtConfirmPassword.Visibility = Visibility.Visible;
            TxtConfirmPassword.Focus();
        }

        private void ImgShowConfirmPassword_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            TxtVisibleConfirmPassword.Visibility = Visibility.Hidden;
            TxtConfirmPassword.Visibility = Visibility.Visible;
            TxtConfirmPassword.Focus();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void TxtUserEmail_GotFocus(object sender, RoutedEventArgs e)
        {
            _currentInputLanguage = InputLanguage.CurrentInputLanguage;
        }

        private void TxtUserEmail_LostFocus(object sender, RoutedEventArgs e)
        {
            InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(_currentInputLanguage.Culture);
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

        private void Minimize(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void NumberInput(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void DisablePaste(object sender, ExecutedRoutedEventArgs e)
        {
            var regex = new Regex("[^0-9]+");
            e.Handled = e.Command == ApplicationCommands.Paste && regex.IsMatch(Clipboard.GetText());
        }

        private void EnglishInput(object sender, KeyEventArgs e)
        {
            var language = new CultureInfo("en-US");
            InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(language);
        }

        private void dataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }
        #endregion

        #region Method

        private bool CheckEmpty()
        {
            if (TxtUserFirstName.Text.Trim() == string.Empty)
            {
                Utility.Message("خطا", "لطفا نام را وارد کنید", "Stop.png");
                return false;
            }

            if (TxtUserLastName.Text.Trim() == string.Empty)
            {
                Utility.Message("خطا", "لطفا نام خانوادگی را وارد کنید", "Stop.png");
                return false;
            }
            if (TxtUserName.Text.Trim() == string.Empty)
            {
                Utility.Message("خطا", "لطفا نام کاربری را وارد کنید", "Stop.png");
                return false;
            }
            if (TxtUserPassword.Password.Trim() == string.Empty)
            {
                Utility.Message("خطا", "لطفا کلمه عبور را وارد کنید", "Stop.png");
                return false;
            }
            if (TxtConfirmPassword.Password.Trim() == string.Empty)
            {
                Utility.Message("خطا", "لطفا کلمه عبور را مجددا وارد کنید", "Stop.png");
                return false;
            }
            if (TxtUserPassword.Password.Trim() != TxtConfirmPassword.Password.Trim())
            {
                Utility.Message("خطا", "کلمه عبور و تکرار کلمه عبور باید برابر باشد", "Stop.png");
                return false;
            }
            if (TxtUserAnswer.Text.Trim() == string.Empty)
            {
                Utility.Message("اخطار",
                    "لطفا پاسخ پرسش امنیتی را وارد کنید\nاین مورد به هنگام فراموشی رمز عبور استفاده میشود در به خاطر سپردن آن کوشا باشید",
                    "Warning.png");
                return false;
            }
            return true;
        }

        private string SelectePicture()
        {
            //Sakhtan masir zakhireh aks personnel
            if (!Directory.Exists($@"{_directoryPath}\Image\User\Picture"))
                Directory.CreateDirectory($@"{_directoryPath}\Image\User\Picture");
            try
            {
                //ایجاد نام تصادفی به منظور عدم ایجاد خطا در مواردی که عکس هایی با نام یکسان وجود دارند
                var randomFileName = Path.GetRandomFileName();

                //کپی عکس از مسیر اصلی در مسیر دیتا
                File.Copy(_filePath,
                    _directoryPath + @"\Image\User\Picture\" + randomFileName + _fileName);

                //استخراج مسیر و نام کامل عکس مورد نظر
                return randomFileName + _fileName;
            }
            catch (Exception)
            {
                ImgAdminPicture.Source =
                    new BitmapImage(new Uri(_currentDirectory + @"\Icon\Boss.png"));
                Utility.Message("خطا", "عکس یافت نشد", "Warning.png");
                return null;
            }
        }

        #endregion
    }
}
