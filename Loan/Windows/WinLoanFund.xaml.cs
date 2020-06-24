using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using DAL;
using DAL.Class;
using Loan.Class;
using Clipboard = System.Windows.Clipboard;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace Loan.Windows
{
    /// <summary>
    /// Interaction logic for WinLoanFund.xaml
    /// </summary>
    public partial class WinLoanFund
    {
        private OpenFileDialog _openFileDialog;
        private bool _selectedPicture;
        private string _fileName;
        private string _filePath;
        private readonly string _currentDirectory;
        private List<tblLoanFund> _loanFundData;
        private InputLanguage _currentInputLanguage;
        private readonly string _directoryPath;

        public WinLoanFund()
        {
            InitializeComponent();
            _currentDirectory = Directory.GetCurrentDirectory();
            _directoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "ARKVN");
        }

        private void BtnChoosePicture_Click(object sender, RoutedEventArgs e)
        {
            _selectedPicture = false;
            _openFileDialog = new OpenFileDialog
            {
                Filter = @"Image Files (*.jpg;*.png;*.bmp) |*.jpg;*.png;*.bmp",
                Title = "انتخاب عکس"
            };
            try
            {
                if (_openFileDialog.ShowDialog() != true) return;
                
                ImgPersonnelImage.Source = Utility.DisplayImage(_openFileDialog.FileName);

                //استخراج نام کامل عکس
                _fileName = Path.GetFileName(_openFileDialog.FileName);
                _filePath = Path.GetFullPath(_openFileDialog.FileName);
                _selectedPicture = true;
            }
            catch (Exception)
            {
                ImgPersonnelImage.Source = new BitmapImage(new Uri(_currentDirectory + @"\Icon\Eye.png"));
                Utility.Message("اخطار", "عکس یافت نشد", "Stop.png");
            }
        }

        #region FixedEvent

        private void NumberInput(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

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

        private void DisablePaste(object sender, ExecutedRoutedEventArgs e)
        {
            var regex = new Regex("[^0-9]+");
            e.Handled = e.Command == ApplicationCommands.Paste && regex.IsMatch(Clipboard.GetText());
        }

        #endregion

        private string SelectePicture()
        {
            //Sakhtan masir zakhireh aks personnel
            if (!Directory.Exists($@"{_directoryPath}\Image\LoanFund\Picture"))
                Directory.CreateDirectory($@"{_directoryPath}\Image\LoanFund\Picture");
            try
            {
                //ایجاد نام تصادفی به منظور عدم ایجاد خطا در مواردی که عکس هایی با نام یکسان وجود دارند
                var randomFileName = Path.GetRandomFileName();

                //کپی عکس از مسیر اصلی در پوشه دیتا
                File.Copy(_filePath,
                    _directoryPath + @"\Image\LoanFund\Picture\" + randomFileName + _fileName);

                //استخراج مسیر و نام کامل عکس مورد نظر
                return randomFileName + _fileName;
            }
            catch (Exception)
            {
                ImgPersonnelImage.Source =
                    new BitmapImage(new Uri(_currentDirectory + @"\Icon\Eye.png"));
                Utility.Message("خطا", "عکس یافت نشد", "Warning.png");
                return null;
            }
        }

        private async void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!Utility.CheckEmail(TxtEmail.Text)) return;
            if (long.Parse(TxtWage.Text) <= 0 || long.Parse(TxtWage.Text) >= 100)
            {
                Utility.Message("پیام", "درصد کارمزد باید بین 0 تا 100 باشد", "Stop.png");
                return;
            }
            if (_loanFundData.Count == 0)
            {
                #region AddloanFund

                try
                {
                    var addloanFund = new DLoanFund
                    {
                        DName = TxtName.Text.Trim() == string.Empty ? null : TxtName.Text,
                        DTell1 = TxtTell1.Text.Trim() == string.Empty ? null : TxtTell1.Text,
                        DTell2 = TxtTell2.Text.Trim() == string.Empty ? null : TxtTell2.Text,
                        DFax = TxtFax.Text.Trim() == string.Empty ? null : TxtFax.Text,
                        DEmail = TxtEmail.Text.Trim() == string.Empty ? null : TxtEmail.Text,
                        DPostalCode = TxtPostalCode.Text.Trim() == string.Empty ? null : TxtPostalCode.Text,
                        DAddress = TxtAddress.Text.Trim() == string.Empty ? null : TxtAddress.Text,
                        DInitialBalance = long.Parse(Regex.Replace(TxtInitialBalance.Text, "[\\W]", "")),
                        DPenalty = TxtPenalty.Text.Trim() == string.Empty ? null : (long?)long.Parse(TxtPenalty.Text),
                        DWagePercent = TxtWage.Text.Trim() == string.Empty ? null : TxtWage.Text,
                        DLogo = _selectedPicture ? SelectePicture() : null
                    };
                    await Task.Run(() => addloanFund.Add());
                }
                catch (Exception exception)
                {
                    Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در ثبت اطلاعات صندوق\n" + exception.Message);
                    return;
                }

                #endregion
            }
            else
            {
                #region EditloanFund

                try
                {
                    var editloanFund = new DLoanFund
                    {
                        DName = TxtName.Text.Trim() == string.Empty ? null : TxtName.Text,
                        DTell1 = TxtTell1.Text.Trim() == string.Empty ? null : TxtTell1.Text,
                        DTell2 = TxtTell2.Text.Trim() == string.Empty ? null : TxtTell2.Text,
                        DFax = TxtFax.Text.Trim() == string.Empty ? null : TxtFax.Text,
                        DEmail = TxtEmail.Text.Trim() == string.Empty ? null : TxtEmail.Text,
                        DPostalCode = TxtPostalCode.Text.Trim() == string.Empty ? null : TxtPostalCode.Text,
                        DAddress = TxtAddress.Text.Trim() == string.Empty ? null : TxtAddress.Text,
                        DInitialBalance = long.Parse(Regex.Replace(TxtInitialBalance.Text, "[\\W]", "")),
                        DPenalty = TxtPenalty.Text.Trim() == string.Empty ? null : (long?)long.Parse(TxtPenalty.Text),
                        DWagePercent = TxtWage.Text.Trim() == string.Empty ? null : TxtWage.Text,
                        DLogo = _selectedPicture ? SelectePicture() : _loanFundData[0].LoanFundLogo

                    };
                    await Task.Run(() => editloanFund.Edit());
                }
                catch (Exception exception)
                {
                    Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در ثبت اطلاعات شخصی\n" + exception.Message);
                    return;
                }

                #endregion
            }

            Window_Loaded(null, null);
            Utility.Message("پیام", "اطلاعات با موفقیت ثبت گردید", "Correct.png");
        }

        private void EnglishInput(object sender, KeyEventArgs e)
        {
            var language = new CultureInfo("en-US");
            InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(language);
        }

        private void TxtInitialBalance_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (TxtInitialBalance.Text.Trim() == string.Empty)
            {
                TxtInitialBalance.Text = "0";
            }
            else
            {
                decimal number;
                if (!decimal.TryParse(TxtInitialBalance.Text, out number)) return;
                TxtInitialBalance.Text = $"{number:N0}";
                TxtInitialBalance.SelectionStart = TxtInitialBalance.Text.Length;
            }
        }

        private void TxtWage_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (TxtWage.Text.Trim() == string.Empty)
            {
                TxtWage.Text = "2";
            }
        }

        private void TxtPenalty_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (TxtPenalty.Text.Trim() == string.Empty)
            {
                TxtPenalty.Text = "0";
            }
            else
            {
                decimal number;
                if (!decimal.TryParse(TxtPenalty.Text, out number)) return;
                TxtPenalty.Text = $"{number:N0}";
                TxtPenalty.SelectionStart = TxtPenalty.Text.Length;
            }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TxtInitialBalance.Text = "0";
            TxtPenalty.Text = "0";
            TxtWage.Text = "2";
            try
            {
                _loanFundData = await DLoanFund.GetData();
            }
            catch (Exception exception)
            {
                Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در دریافت اطلاعات\n" + exception.Message);
                return;
            }
            if (_loanFundData.Count == 0) return;
            TxtName.Text = _loanFundData[0].LoanFundName;
            TxtTell1.Text = _loanFundData[0].LoanFundTell1;
            TxtTell2.Text = _loanFundData[0].LoanFundTell2;
            TxtFax.Text = _loanFundData[0].LoanFundFax;
            TxtEmail.Text = _loanFundData[0].LoanFundEmail;
            TxtPostalCode.Text = _loanFundData[0].LoanFundPostalCode;
            TxtAddress.Text = _loanFundData[0].LoanFundAddress;
            TxtInitialBalance.Text = _loanFundData[0].LoanFundInitialBalance.ToString();
            TxtPenalty.Text = _loanFundData[0].LoanFundPenalty.ToString();
            TxtWage.Text = _loanFundData[0].LoanFundWagePercent;

            if (string.IsNullOrEmpty(_loanFundData[0].LoanFundLogo))
            {
                ImgPersonnelImage.Source =
                    new BitmapImage(new Uri(_currentDirectory + @"\Icon\Eye.png"));
            }
            else
            {
                try
                {
                    ImgPersonnelImage.Source =
                        Utility.DisplayImage($@"{_directoryPath}\Image\LoanFund\Picture\" +
                                             _loanFundData[0].LoanFundLogo);
                }
                catch (Exception)
                {
                    ImgPersonnelImage.Source =
                        new BitmapImage(new Uri(_currentDirectory + @"\Icon\Eye.png"));
                }
            }
        }

        private void TxtEmail_GotFocus(object sender, RoutedEventArgs e)
        {
            _currentInputLanguage = InputLanguage.CurrentInputLanguage;
        }

        private void TxtEmail_LostFocus(object sender, RoutedEventArgs e)
        {
            InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(_currentInputLanguage.Culture);
        }
    }
}
