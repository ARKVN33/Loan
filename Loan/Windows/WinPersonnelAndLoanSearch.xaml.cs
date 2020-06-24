using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using DAL;
using DAL.Class;
using Loan.Class;
using Stimulsoft.Report;
using Stimulsoft.Report.Dictionary;

namespace Loan.Windows
{
    /// <summary>
    /// Interaction logic for WinPersonnelAndLoanSearch.xaml
    /// </summary>
    public partial class WinPersonnelAndLoanSearch
    {
        private List<spSelectPersonnelInfo_Result> _personnelData;
        private List<spSelectPersonnelInfo_Result> _personnelSearchData;
        private List<spSelectLoanInfo_Result> _loanInfoData;

        public WinPersonnelAndLoanSearch()
        {
            InitializeComponent();
            _personnelData = new List<spSelectPersonnelInfo_Result>();
            _personnelSearchData = new List<spSelectPersonnelInfo_Result>();
            _loanInfoData = new List<spSelectLoanInfo_Result>();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _personnelData = await DPersonnel.GetData();
            }
            catch (Exception exception)
            {
                Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در دریافت اطلاعات\n" + exception.Message);
                Close();
                return;
            }
            _personnelSearchData = _personnelData;
            DgdPersonnel.ItemsSource = _personnelSearchData;
        }

        private void BtnPayInstallment_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckSelect()) return;

            var selectPersonnelItem = _personnelSearchData[DgdPersonnel.SelectedIndex];
            var selectLoanItem = _loanInfoData[DgdLoan.SelectedIndex];

            var winInstallment = new WinInstallment
            {
                LblTitle = { Content = "پرداخت اقساط" },
                LblPerId = { Content = selectPersonnelItem.PersonnelId },
                LblPerFirstName = { Content = selectPersonnelItem.InfoFirstName },
                LblPerLastName = { Content = selectPersonnelItem.InfoLastName },
                LblPerNationalCode = { Content = selectPersonnelItem.InfoNationalCode },
                LblPerLoanAmount =
                {
                    Content = long.Parse(selectLoanItem.LoanAmount.ToString()).ToString("N0", CultureInfo.InvariantCulture)
                },

                LblPerWageAmount =
                {
                    Content = long.Parse(selectLoanItem.WageAmount.ToString()).ToString("N0", CultureInfo.InvariantCulture)
                },

                LblPerLoanDate = { Content = selectLoanItem.LoanDate },

                PersonnelId = selectPersonnelItem.Id,
                LoanId = selectLoanItem.Id,
                LoanDate = selectLoanItem.LoanDate
            };
            winInstallment.ShowDialog();
        }

        private async void DgdPersonnel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DgdPersonnel.SelectedIndex == -1) return;

            var selectItem = _personnelSearchData[DgdPersonnel.SelectedIndex];
            LblPerId.Content = selectItem.PersonnelId;
            LblPerFirstName.Content = selectItem.InfoFirstName;
            LblPerLastName.Content = selectItem.InfoLastName;
            LblPerNationalCode.Content = selectItem.InfoNationalCode;
            try
            {
                _loanInfoData = await DLoan.GetLoanInfoData(selectItem.Id);
            }
            catch (Exception exception)
            {
                Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در دریافت اطلاعات\n" + exception.Message);
                return;
            }
            DgdLoan.ItemsSource = _loanInfoData;
        }

        private async void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            LblPerId.Content = string.Empty;
            LblPerFirstName.Content = string.Empty;
            LblPerLastName.Content = string.Empty;
            LblPerNationalCode.Content = string.Empty;

            var search = TxtSearch.Text;

            _personnelSearchData = await Task.Run(() => _personnelData.FindAll(
                t =>
                    !string.IsNullOrEmpty(t.PersonnelId) && t.PersonnelId.Contains(search) ||
                    !string.IsNullOrEmpty(t.InfoFirstName) && t.InfoFirstName.Contains(search) ||
                    !string.IsNullOrEmpty(t.InfoLastName) && t.InfoLastName.Contains(search) ||
                    !string.IsNullOrEmpty(t.InfoNationalCode) && t.InfoNationalCode.Contains(search) ||
                    !string.IsNullOrEmpty(t.InfoCode) && t.InfoCode.Contains(search) ||
                    !string.IsNullOrEmpty(t.InfoMobile) && t.InfoMobile.Contains(search) ||
                    !string.IsNullOrEmpty(t.InfoTell) && t.InfoTell.Contains(search) ||
                    !string.IsNullOrEmpty(t.InfoPostalCode) && t.InfoPostalCode.Contains(search) ||
                    !string.IsNullOrEmpty(t.InfoAddress) && t.InfoAddress.Contains(search)));
            DgdPersonnel.ItemsSource = _personnelSearchData;
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

        //baraye shomare gozari datagrid
        private void dataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void DgdLoan_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
            e.Row.Foreground = _loanInfoData[e.Row.GetIndex()].LoanPayOff == true ? new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Red);
        }


        #endregion

        #region Method

        private bool CheckSelect()
        {
            if (DgdLoan.SelectedIndex == -1 || LblPerId.Content == null)
            {
                Utility.Message("خطا", "وامی را برای شخص مورد نظر انتخاب کنید", "Stop.png");
                return false;
            }

            return true;
        }

        #endregion

        private void BtnRepIns_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckSelect()) return;

            var report = new StiReport();
            report.Load("Report//PerLoanIns.mrt");
            report.Dictionary.Databases.Clear();
            report.Dictionary.Databases.Add(new StiSqlDatabase("RepLoan", @"Data Source=(LocalDB)\MSSQLLocalDB;Database=dbLoan;Integrated Security=True;Connect Timeout=30"));
            report.Dictionary.Variables.Add(new StiVariable("ShamsiDate", PersianDateTime.Now.Date.ToString("yyyy/MM/dd")));
            report.Dictionary.Variables.Add(new StiVariable("TimeNow", PersianDateTime.Now.TimeOfDay.ToHHMMSS()));
            report.Dictionary.Variables.Add(new StiVariable("programPath", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                @"ARKVN\Image\LoanFund\Picture\")));
            report.Dictionary.Variables.Add(new StiVariable("LoanId", _loanInfoData[DgdLoan.SelectedIndex].Id));
            report.Dictionary.Variables.Add(new StiVariable("PerId", _personnelSearchData[DgdPersonnel.SelectedIndex].Id));
            report.ShowWithWpf();
        }
    }
}
