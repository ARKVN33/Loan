using System;
using System.Windows;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Windows.Forms;
using Loan.Class;
using Stimulsoft.Report;
using Stimulsoft.Report.Dictionary;
using Application = System.Windows.Application;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;

namespace Loan.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly string _loanLogoPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            @"ARKVN\Image\LoanFund\Picture\");

        private const string ConnetionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Database=dbLoan;Integrated Security=True;Connect Timeout=30";

        private bool _okClose;

        public MainWindow()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(new CultureInfo("fa-Ir"));
            InitializeComponent();
        }

        private void BtnAddPersonnel_Click(object sender, RoutedEventArgs e)
        {
            var winPersonnel = new WinPersonnel();
            winPersonnel.ShowDialog();
        }

        private void BtnAddChargeMonthly_Click(object sender, RoutedEventArgs e)
        {
            var winPersonnelSearch = new WinPersonnelSearch
            {
                BtnAddChargeMonthly = { Visibility = Visibility.Visible }
            };
            winPersonnelSearch.ShowDialog();
        }

        private void BtnPayChargeMonthly_Click(object sender, RoutedEventArgs e)
        {
            var winPersonnelSearch = new WinPersonnelSearch
            {
                LblTitle = { Content = "انتخاب عضو برای پرداخت شارژ ماهانه" },
                BtnPayChargeMonthly = { Visibility = Visibility.Visible }
            };
            winPersonnelSearch.TxtSearch.Focus();
            winPersonnelSearch.ShowDialog();
        }

        private void BtnPay_Click(object sender, RoutedEventArgs e)
        {
            var winPersonnelSearch = new WinPersonnelSearch
            {
                LblTitle = { Content = "انتخاب عضو برای پرداخت-برداشت پول" },
                BtnPay = { Visibility = Visibility.Visible }
            };
            winPersonnelSearch.TxtSearch.Focus();
            winPersonnelSearch.ShowDialog();
        }

        private void BtnAddLoan_Click(object sender, RoutedEventArgs e)
        {
            var winPersonnelSearch = new WinPersonnelSearch
            {
                LblTitle = { Content = "انتخاب شخص برای ثبت وام" },
                BtnAddLoan = { Visibility = Visibility.Visible }
            };
            winPersonnelSearch.TxtSearch.Focus();
            winPersonnelSearch.ShowDialog();
        }

        private void BtnAddInstallment_Click(object sender, RoutedEventArgs e)
        {
            var winPersonnelAndLoanSearch = new WinPersonnelAndLoanSearch
            {
                LblTitle = { Content = "انتخاب شخص برای واریز اقساط" },
                BtnPayInstallment = { Visibility = Visibility.Visible }
            };
            winPersonnelAndLoanSearch.TxtSearch.Focus();
            winPersonnelAndLoanSearch.ShowDialog();
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            _okClose = true;
            BackUp_Click(null, null);
            Application.Current.Shutdown();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            _okClose = true;
            BackUp_Click(null, null);
            e.Cancel = false;
            Application.Current.Shutdown();
        }

        private void BtnLoanFund_Click(object sender, RoutedEventArgs e)
        {
            var winLoanFund = new WinLoanFund();
            winLoanFund.ShowDialog();
        }

        private void BtnBankAccount_Click(object sender, RoutedEventArgs e)
        {
            var winBankAccount = new WinBankAccount();
            winBankAccount.ShowDialog();
        }

        private void BtnFeeIncome_Click(object sender, RoutedEventArgs e)
        {
            var winFeeIncome = new WinFeeIncome();
            winFeeIncome.ShowDialog();
        }

        private void BtnLoanFundInfo_Click(object sender, RoutedEventArgs e)
        {
            var winLoanFundInfo = new WinLoanFundInfo();
            winLoanFundInfo.ShowDialog();
        }

        private void BackUp_Click(object sender, RoutedEventArgs e)
        {
            var fileName = PersianDateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");

            if (_okClose == false)
            {
                var savefd = new SaveFileDialog
                {
                    Filter = "Backup File (*.Bak)|*.Bak",
                    FileName = fileName
                };
                var result = savefd.ShowDialog();
                if (result != true) return;
                var directoryName = Path.GetDirectoryName(savefd.FileName) + "\\" + fileName;
                Directory.CreateDirectory(directoryName);
                var winWait = new WinWait
                {
                    DirectoryName = directoryName,
                    FileName = fileName,
                    OkBackUp = true,
                    OkRestore = false,
                    CloseOk = false

                };

                winWait.ShowDialog();
            }
            else
            {
                var directoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    @"ARKVN\BackUp\" + fileName);


                Directory.CreateDirectory(directoryPath);
                var winWait = new WinWait
                {
                    DirectoryName = directoryPath,
                    FileName = fileName,
                    OkBackUp = true,
                    OkRestore = false,
                    CloseOk = true
                };

                winWait.ShowDialog();
            }

        }

        private void Restore_Click(object sender, RoutedEventArgs e)
        {
            string extractPath = null;
            try
            {
                var openFileDialog = new Microsoft.Win32.OpenFileDialog
                {
                    Filter = @"Zip Files (*.Zip) |*.Zip",
                    Title = @"انتخاب مسیر فایل پشتیبان"
                };

                if (openFileDialog.ShowDialog() != true) return;

                var fileName = Path.GetFileName(openFileDialog.FileName);
                var fullPath = Path.GetFullPath(openFileDialog.FileName);
                extractPath = Path.ChangeExtension(fullPath, null);
                Directory.CreateDirectory(extractPath);
                ZipFile.ExtractToDirectory(fullPath, extractPath);
                var winWait = new WinWait
                {
                    FileName = fileName,
                    WExtractPath = extractPath,
                    OkBackUp = false,
                    OkRestore = true
                };

                winWait.ShowDialog();
            }
            catch (Exception exception)
            {
                Utility.MyMessageBox("خطا در بازنشانی اطلاعات", exception.Message);
                if (Directory.Exists(extractPath))
                {
                    try
                    {
                        Directory.Delete(extractPath, true);
                    }
                    catch (Exception exception1)
                    {
                        Utility.MyMessageBox("خطا در حذف فایل ایجاد شده", exception1.Message);
                    }
                }
            }
        }

        private void BtnPerRep_Click(object sender, RoutedEventArgs e)
        {
            var report = new StiReport();
            report.Load("Report//AllPer.mrt");

            report.Dictionary.Databases.Clear();
            report.Dictionary.Databases.Add(new StiSqlDatabase("RepAllPer", ConnetionString));
            report.Dictionary.Variables.Add(new StiVariable("ShamsiDate", PersianDateTime.Now.Date.ToString("yyyy/MM/dd")));
            report.Dictionary.Variables.Add(new StiVariable("TimeNow", PersianDateTime.Now.TimeOfDay.ToHHMMSS()));
            report.Dictionary.Variables.Add(new StiVariable("programPath", _loanLogoPath));
            report.ShowWithWpf();
        }

        private void BtnPerSomeRep_Click(object sender, RoutedEventArgs e)
        {
            var report = new StiReport();
            report.Load("Report//AllPerSD.mrt");
            report.Dictionary.Databases.Clear();
            report.Dictionary.Databases.Add(new StiSqlDatabase("RepAllPer", ConnetionString));
            report.Dictionary.Variables.Add(new StiVariable("ShamsiDate", PersianDateTime.Now.Date.ToString("yyyy/MM/dd")));
            report.Dictionary.Variables.Add(new StiVariable("TimeNow", PersianDateTime.Now.TimeOfDay.ToHHMMSS()));
            report.Dictionary.Variables.Add(new StiVariable("programPath", _loanLogoPath));
            report.ShowWithWpf();
        }

        private void BtnPerFullRep_Click(object sender, RoutedEventArgs e)
        {
            var report = new StiReport();
            report.Load("Report//AllPerFD.mrt");
            report.Dictionary.Databases.Clear();
            report.Dictionary.Databases.Add(new StiSqlDatabase("RepAllPer", ConnetionString));
            report.Dictionary.Variables.Add(new StiVariable("ShamsiDate", PersianDateTime.Now.Date.ToString("yyyy/MM/dd")));
            report.Dictionary.Variables.Add(new StiVariable("TimeNow", PersianDateTime.Now.TimeOfDay.ToHHMMSS()));
            report.Dictionary.Variables.Add(new StiVariable("programPath", _loanLogoPath));
            report.ShowWithWpf();
        }

        private void RibbonButton_Click_2(object sender, RoutedEventArgs e)
        {
            var winPersonnelAndLoanSearch = new WinPersonnelAndLoanSearch
            {
                LblTitle = { Content = "چاپ دفترچه اقساط" },
                BtnPayInstallment = { Visibility = Visibility.Hidden },
                BtnRepIns = { Visibility = Visibility.Visible }
            };
            winPersonnelAndLoanSearch.TxtSearch.Focus();
            winPersonnelAndLoanSearch.ShowDialog();
        }

        private void BtnMemPerRep_Click(object sender, RoutedEventArgs e)
        {
            var report = new StiReport();
            report.Load("Report//AllMemPer.mrt");
            report.Dictionary.Databases.Clear();
            report.Dictionary.Databases.Add(new StiSqlDatabase("RepAllPer", ConnetionString));
            report.Dictionary.Variables.Add(new StiVariable("ShamsiDate", PersianDateTime.Now.Date.ToString("yyyy/MM/dd")));
            report.Dictionary.Variables.Add(new StiVariable("TimeNow", PersianDateTime.Now.TimeOfDay.ToHHMMSS()));
            report.Dictionary.Variables.Add(new StiVariable("programPath", _loanLogoPath));
            report.ShowWithWpf();
        }

        private void BtnMemPerSomeRep_Click(object sender, RoutedEventArgs e)
        {
            var report = new StiReport();
            report.Load("Report//AllMemPerSD.mrt");
            report.Dictionary.Databases.Clear();
            report.Dictionary.Databases.Add(new StiSqlDatabase("RepAllPer", ConnetionString));
            report.Dictionary.Variables.Add(new StiVariable("ShamsiDate", PersianDateTime.Now.Date.ToString("yyyy/MM/dd")));
            report.Dictionary.Variables.Add(new StiVariable("TimeNow", PersianDateTime.Now.TimeOfDay.ToHHMMSS()));
            report.Dictionary.Variables.Add(new StiVariable("programPath", _loanLogoPath));
            report.ShowWithWpf();
        }

        private void BtnMemPerFullRep_Click(object sender, RoutedEventArgs e)
        {
            var report = new StiReport();
            report.Load("Report//AllMemPerFD.mrt");
            report.Dictionary.Databases.Clear();
            report.Dictionary.Databases.Add(new StiSqlDatabase("RepAllPer", ConnetionString));
            report.Dictionary.Variables.Add(new StiVariable("ShamsiDate", PersianDateTime.Now.Date.ToString("yyyy/MM/dd")));
            report.Dictionary.Variables.Add(new StiVariable("TimeNow", PersianDateTime.Now.TimeOfDay.ToHHMMSS()));
            report.Dictionary.Variables.Add(new StiVariable("programPath", _loanLogoPath));
            report.ShowWithWpf();
        }

        private void BtnNoMemPerRep_Click(object sender, RoutedEventArgs e)
        {
            var report = new StiReport();
            report.Load("Report//AllNoMemPer.mrt");
            report.Dictionary.Databases.Clear();
            report.Dictionary.Databases.Add(new StiSqlDatabase("RepAllPer", ConnetionString));
            report.Dictionary.Variables.Add(new StiVariable("ShamsiDate", PersianDateTime.Now.Date.ToString("yyyy/MM/dd")));
            report.Dictionary.Variables.Add(new StiVariable("TimeNow", PersianDateTime.Now.TimeOfDay.ToHHMMSS()));
            report.Dictionary.Variables.Add(new StiVariable("programPath", _loanLogoPath));
            report.ShowWithWpf();
        }

        private void BtnNoMemPerSomeRep_Click(object sender, RoutedEventArgs e)
        {
            var report = new StiReport();
            report.Load("Report//AllNoMemPerSD.mrt");
            report.Dictionary.Databases.Clear();
            report.Dictionary.Databases.Add(new StiSqlDatabase("RepAllPer", ConnetionString));
            report.Dictionary.Variables.Add(new StiVariable("ShamsiDate", PersianDateTime.Now.Date.ToString("yyyy/MM/dd")));
            report.Dictionary.Variables.Add(new StiVariable("TimeNow", PersianDateTime.Now.TimeOfDay.ToHHMMSS()));
            report.Dictionary.Variables.Add(new StiVariable("programPath", _loanLogoPath));
            report.ShowWithWpf();
        }

        private void BtnNoMemPerFullRep_Click(object sender, RoutedEventArgs e)
        {
            var report = new StiReport();
            report.Load("Report//AllNoMemPerFD.mrt");
            report.Dictionary.Databases.Clear();
            report.Dictionary.Databases.Add(new StiSqlDatabase("RepAllPer", ConnetionString));
            report.Dictionary.Variables.Add(new StiVariable("ShamsiDate", PersianDateTime.Now.Date.ToString("yyyy/MM/dd")));
            report.Dictionary.Variables.Add(new StiVariable("TimeNow", PersianDateTime.Now.TimeOfDay.ToHHMMSS()));
            report.Dictionary.Variables.Add(new StiVariable("programPath", _loanLogoPath));
            report.ShowWithWpf();
        }

        private void BtnAllLoan_Click(object sender, RoutedEventArgs e)
        {
            var report = new StiReport();
            report.Load("Report//AllLoan.mrt");
            report.Dictionary.Databases.Clear();
            report.Dictionary.Databases.Add(new StiSqlDatabase("RepLoan", ConnetionString));
            report.Dictionary.Variables.Add(new StiVariable("ShamsiDate", PersianDateTime.Now.Date.ToString("yyyy/MM/dd")));
            report.Dictionary.Variables.Add(new StiVariable("TimeNow", PersianDateTime.Now.TimeOfDay.ToHHMMSS()));
            report.Dictionary.Variables.Add(new StiVariable("programPath", _loanLogoPath));
            report.ShowWithWpf();
        }

        private void BtnAllLoanSomeRep_Click(object sender, RoutedEventArgs e)
        {
            var report = new StiReport();
            report.Load("Report//AllLoanSD.mrt");
            report.Dictionary.Databases.Clear();
            report.Dictionary.Databases.Add(new StiSqlDatabase("RepLoan", ConnetionString));
            report.Dictionary.Variables.Add(new StiVariable("ShamsiDate", PersianDateTime.Now.Date.ToString("yyyy/MM/dd")));
            report.Dictionary.Variables.Add(new StiVariable("TimeNow", PersianDateTime.Now.TimeOfDay.ToHHMMSS()));
            report.Dictionary.Variables.Add(new StiVariable("programPath", _loanLogoPath));
            report.ShowWithWpf();
        }

        private void BtnPayOffLoan_Click(object sender, RoutedEventArgs e)
        {
            var report = new StiReport();
            report.Load("Report//AllPOLoan.mrt");
            report.Dictionary.Databases.Clear();
            report.Dictionary.Databases.Add(new StiSqlDatabase("RepLoan", ConnetionString));
            report.Dictionary.Variables.Add(new StiVariable("ShamsiDate", PersianDateTime.Now.Date.ToString("yyyy/MM/dd")));
            report.Dictionary.Variables.Add(new StiVariable("TimeNow", PersianDateTime.Now.TimeOfDay.ToHHMMSS()));
            report.Dictionary.Variables.Add(new StiVariable("programPath", _loanLogoPath));
            report.ShowWithWpf();
        }

        private void BtnPayOffLoanSomeRep_Click(object sender, RoutedEventArgs e)
        {
            var report = new StiReport();
            report.Load("Report//AllPOLoanSD.mrt");
            report.Dictionary.Databases.Clear();
            report.Dictionary.Databases.Add(new StiSqlDatabase("RepLoan", ConnetionString));
            report.Dictionary.Variables.Add(new StiVariable("ShamsiDate", PersianDateTime.Now.Date.ToString("yyyy/MM/dd")));
            report.Dictionary.Variables.Add(new StiVariable("TimeNow", PersianDateTime.Now.TimeOfDay.ToHHMMSS()));
            report.Dictionary.Variables.Add(new StiVariable("programPath", _loanLogoPath));
            report.ShowWithWpf();
        }

        private void BtnNoPayOffLoan_Click(object sender, RoutedEventArgs e)
        {
            var report = new StiReport();
            report.Load("Report//AllNPOLoan.mrt");
            report.Dictionary.Databases.Clear();
            report.Dictionary.Databases.Add(new StiSqlDatabase("RepLoan", ConnetionString));
            report.Dictionary.Variables.Add(new StiVariable("ShamsiDate", PersianDateTime.Now.Date.ToString("yyyy/MM/dd")));
            report.Dictionary.Variables.Add(new StiVariable("TimeNow", PersianDateTime.Now.TimeOfDay.ToHHMMSS()));
            report.Dictionary.Variables.Add(new StiVariable("programPath", _loanLogoPath));
            report.ShowWithWpf();
        }

        private void BtnNoPayOffLoanSomeRep_Click(object sender, RoutedEventArgs e)
        {
            var report = new StiReport();
            report.Load("Report//AllNPOLoanSD.mrt");
            report.Dictionary.Databases.Clear();
            report.Dictionary.Databases.Add(new StiSqlDatabase("RepLoan", ConnetionString));
            report.Dictionary.Variables.Add(new StiVariable("ShamsiDate", PersianDateTime.Now.Date.ToString("yyyy/MM/dd")));
            report.Dictionary.Variables.Add(new StiVariable("TimeNow", PersianDateTime.Now.TimeOfDay.ToHHMMSS()));
            report.Dictionary.Variables.Add(new StiVariable("programPath", _loanLogoPath));
            report.ShowWithWpf();
        }

        private void ChangePassword_Click(object sender, RoutedEventArgs e)
        {
            var winChangePassword = new WinChangePassword();
            winChangePassword.ShowDialog();
        }

        private void BtnDelayedLoan_Click(object sender, RoutedEventArgs e)
        {
            var report = new StiReport();
            report.Load("Report//AllDelayedLoan.mrt");
            report.Dictionary.Databases.Clear();
            report.Dictionary.Databases.Add(new StiSqlDatabase("RepLoan", ConnetionString));
            report.Dictionary.Variables.Add(new StiVariable("ShamsiDate", PersianDateTime.Now.Date.ToString("yyyy/MM/dd")));
            report.Dictionary.Variables.Add(new StiVariable("TimeNow", PersianDateTime.Now.TimeOfDay.ToHHMMSS()));
            report.Dictionary.Variables.Add(new StiVariable("programPath", _loanLogoPath));
            report.ShowWithWpf();
        }

        private void BtnDelayedLoanSomeRep_Click(object sender, RoutedEventArgs e)
        {
            var report = new StiReport();
            report.Load("Report//AllDelayedLoanSD.mrt");
            report.Dictionary.Databases.Clear();
            report.Dictionary.Databases.Add(new StiSqlDatabase("RepLoan", ConnetionString));
            report.Dictionary.Variables.Add(new StiVariable("ShamsiDate", PersianDateTime.Now.Date.ToString("yyyy/MM/dd")));
            report.Dictionary.Variables.Add(new StiVariable("TimeNow", PersianDateTime.Now.TimeOfDay.ToHHMMSS()));
            report.Dictionary.Variables.Add(new StiVariable("programPath", _loanLogoPath));
            report.ShowWithWpf();
        }

        private void BtnDelayLoan_Click(object sender, RoutedEventArgs e)
        {
            var report = new StiReport();
            report.Load("Report//AllDelayLoan.mrt");
            report.Dictionary.Databases.Clear();
            report.Dictionary.Databases.Add(new StiSqlDatabase("RepLoan", ConnetionString));
            report.Dictionary.Variables.Add(new StiVariable("ShamsiDate", PersianDateTime.Now.Date.ToString("yyyy/MM/dd")));
            report.Dictionary.Variables.Add(new StiVariable("TimeNow", PersianDateTime.Now.TimeOfDay.ToHHMMSS()));
            report.Dictionary.Variables.Add(new StiVariable("programPath", _loanLogoPath));
            report.ShowWithWpf();
        }

        private void BtnDelayLoanSomeRep_Click(object sender, RoutedEventArgs e)
        {
            var report = new StiReport();
            report.Load("Report//AllDelayLoanSD.mrt");
            report.Dictionary.Databases.Clear();
            report.Dictionary.Databases.Add(new StiSqlDatabase("RepLoan", ConnetionString));
            report.Dictionary.Variables.Add(new StiVariable("ShamsiDate", PersianDateTime.Now.Date.ToString("yyyy/MM/dd")));
            report.Dictionary.Variables.Add(new StiVariable("TimeNow", PersianDateTime.Now.TimeOfDay.ToHHMMSS()));
            report.Dictionary.Variables.Add(new StiVariable("programPath", _loanLogoPath));
            report.ShowWithWpf();
        }

        private void BtnPerAcc_Click(object sender, RoutedEventArgs e)
        {
            var winPersonnelSearch = new WinPersonnelSearch
            {
                LblTitle = { Content = "چاپ دفترچه حساب" },
                BtnAccRep = { Visibility = Visibility.Visible }

            };
            winPersonnelSearch.TxtSearch.Focus();
            winPersonnelSearch.ShowDialog();
        }

        private void BtnLoanFeeInc_Click(object sender, RoutedEventArgs e)
        {
            var report = new StiReport();
            report.Load("Report//LoanFeeInc.mrt");
            report.Dictionary.Databases.Clear();
            report.Dictionary.Databases.Add(new StiSqlDatabase("RepLoan", ConnetionString));
            report.Dictionary.Variables.Add(new StiVariable("ShamsiDate", PersianDateTime.Now.Date.ToString("yyyy/MM/dd")));
            report.Dictionary.Variables.Add(new StiVariable("TimeNow", PersianDateTime.Now.TimeOfDay.ToHHMMSS()));
            report.Dictionary.Variables.Add(new StiVariable("programPath", _loanLogoPath));
            report.ShowWithWpf();
        }

        private void BtnPerLoanRep_Click(object sender, RoutedEventArgs e)
        {
            var winPerLoanRep = new WinPerLoanRep();
            winPerLoanRep.ShowDialog();
        }

        private void BtnInfoRep_Click(object sender, RoutedEventArgs e)
        {
            var winInfoSearch = new WinInfoSearch
            {
                LblTitle = { Content = "چاپ گزارش وام های ضمانت شده" },
                BtnInfoRep = { Visibility = Visibility.Visible },
                BtnSelect = { Visibility = Visibility.Hidden }
            };
            winInfoSearch.TxtInfoSearch.Focus();
            winInfoSearch.ShowDialog();
        }

        private void AutoBackUp_Click(object sender, RoutedEventArgs e)
        {
            Utility.MyMessageBox("مواردی که باید بدانید", @"پشتیبان های خودکار شامل حداکثر 30 عدد فایل پشتیبان است که به ازای هربار بسته شدن نرم افزار ایجاد می شوند.
این پشتیبان ها را باید در مکانی ذخیره کنید و سپس با استفاده از گزینه -بازنشانی فایل پشتیبان- اقدام به بازنشانی فایل پشتیبان کنید.","AboutUs.png");

            if (!Utility.Ok) return;
            var dialog = new FolderBrowserDialog
                { Description = @"انتخاب مسیر ذخیره سازی فایل های پشتیبان" };
            var result = dialog.ShowDialog();
            if (result != System.Windows.Forms.DialogResult.OK) return;


            var directoryName = Path.GetFullPath(dialog.SelectedPath);


            var winWait = new WinWait
            {
                DirectoryName = directoryName,
                OkAutoBackUp = true
            };
            winWait.ShowDialog();
        }
    }
}
