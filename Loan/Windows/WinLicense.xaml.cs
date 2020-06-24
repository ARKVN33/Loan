using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using DAL.Class;
using Loan.Class;

namespace Loan.Windows
{
    /// <summary>
    /// Interaction logic for WinLicense.xaml
    /// </summary>
    public partial class WinLicense
    {
        private string _serialNum;
        public WinLicense()
        {
            InitializeComponent();
        }

        #region Event

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _serialNum = await Task.Run(() => SerialNumber.GetHardwareSerial());
            TxtSerialNum.Text = _serialNum;
            if (string.IsNullOrEmpty(_serialNum))
            {
                Utility.MyMessageBox("خطا","خطا در ایجاد شماره سریال");
                BtnRegistration.IsEnabled = false;
            }
        }

        private async void BtnRegistration_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TxtLicensekey.Text))
            {
                Utility.MyMessageBox("خطا","لطفا کد فعال سازی را وارد کنید");
            }
            else
            {
                var salt = new SaltyPasswordHashing
                {
                    MaxHashSize = 40,
                    SaltSize = 0
                };

                var licensekey = salt.ComputeHash(_serialNum);

                if (salt.VerifyHash(_serialNum, licensekey) == salt.VerifyHash(_serialNum, TxtLicensekey.Text))
                {
                    var licenseData = await DLicense.GetData();
                    var dLicense = new DLicense
                    {
                        DAppVersion = licenseData[0].AppVersion,
                        DAppLicense = TxtLicensekey.Text
                    };
                    await Task.Run(() => dLicense.Edit());
                    Process.Start(Application.ResourceAssembly.Location);
                    Application.Current.Shutdown();
                }
                else
                {
                    Utility.MyMessageBox("خطا", "کد فعال سازی وارد شده معتبر نمی باشد");
                }
            }
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
        #endregion
    }
}
