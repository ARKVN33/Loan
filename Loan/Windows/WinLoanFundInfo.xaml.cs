using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using DAL;
using DAL.Class;
using Loan.Class;
using Stimulsoft.Report;
using Stimulsoft.Report.Dictionary;

namespace Loan.Windows
{
    /// <summary>
    /// Interaction logic for WinLoanFundInfo.xaml
    /// </summary>
    public partial class WinLoanFundInfo
    {
        private List<spAllData_Result> _allData;
        private List<spPeriodAllData_Result> _allPeriodData;
        public WinLoanFundInfo()
        {
            InitializeComponent();
            _allData = new List<spAllData_Result>();
            _allPeriodData = new List<spPeriodAllData_Result>();
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

        private void DateInput(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex(@"[^0-9^\/]");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void DisablePasteDate(object sender, ExecutedRoutedEventArgs e)
        {
            var regex = new Regex(@"[^0-9^\/]+");
            e.Handled = e.Command == ApplicationCommands.Paste && regex.IsMatch(Clipboard.GetText());
        }
        #endregion


        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _allData = await DAllData.GetAllData();
            }
            catch (Exception exception)
            {
                Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در دریافت اطلاعات\n" + exception.Message);
                return;
            }
            LblLoanFundIniBal.Content = Convert.ToInt64(_allData[0].LoanFundIniBal).ToString("N0", CultureInfo.InvariantCulture).PersianNum();
            LblBankAccIniBal.Content = Convert.ToInt64(_allData[0].BankAccIniBal).ToString("N0", CultureInfo.InvariantCulture).PersianNum();
            LblPay.Content = Convert.ToInt64(_allData[0].Pay).ToString("N0", CultureInfo.InvariantCulture).PersianNum();
            LblMembershipFee.Content = Convert.ToInt64(_allData[0].AllMembershipFee).ToString("N0", CultureInfo.InvariantCulture).PersianNum();
            LblReceivee.Content = Convert.ToInt64(_allData[0].Receivee).ToString("N0", CultureInfo.InvariantCulture).PersianNum();
            LblIncomeAmount.Content = Convert.ToInt64(_allData[0].IncomeAmount).ToString("N0", CultureInfo.InvariantCulture).PersianNum();
            LblFeeAmount.Content = Convert.ToInt64(_allData[0].FeeAmount).ToString("N0", CultureInfo.InvariantCulture).PersianNum();
            LblInsTotalAmount.Content = Convert.ToInt64(_allData[0].InsTotalAmount).ToString("N0", CultureInfo.InvariantCulture).PersianNum();
            LblTotalFund.Content = Convert.ToInt64(_allData[0].TotalFund).ToString("N0", CultureInfo.InvariantCulture).PersianNum();
            LblLoanPayType1.Content = Convert.ToInt64(_allData[0].LoanPayType1).ToString("N0", CultureInfo.InvariantCulture).PersianNum();
            LblLoanPayType2.Content = Convert.ToInt64(_allData[0].LoanPayType2).ToString("N0", CultureInfo.InvariantCulture).PersianNum();
            LblLoanPayType3.Content = Convert.ToInt64(_allData[0].LoanPayType3).ToString("N0", CultureInfo.InvariantCulture).PersianNum();
            LblAllLoanPay.Content = Convert.ToInt64(_allData[0].AllLoanPay).ToString("N0", CultureInfo.InvariantCulture).PersianNum();
            LblAllWage.Content = Convert.ToInt64(_allData[0].AllWage).ToString("N0", CultureInfo.InvariantCulture).PersianNum();
            LblInsTotalDueAmount.Content = Convert.ToInt64(_allData[0].InsTotalDueAmount).ToString("N0", CultureInfo.InvariantCulture).PersianNum();
            LblInsRemaining.Content = Convert.ToInt64(_allData[0].InsRemaining).ToString("N0", CultureInfo.InvariantCulture).PersianNum();
            LblPerMemNum.Content = Convert.ToInt32(_allData[0].PerMemNum).ToString("N0", CultureInfo.InvariantCulture).PersianNum();
            LblPerNotMemNum.Content = Convert.ToInt32(_allData[0].PerNotMemNum).ToString("N0", CultureInfo.InvariantCulture).PersianNum();
            LblLoanIsPayOff.Content = Convert.ToInt32(_allData[0].LoanIsPayOff).ToString("N0", CultureInfo.InvariantCulture).PersianNum();
            LblLoanNotPayOff.Content = Convert.ToInt32(_allData[0].LoanNotPayOff).ToString("N0", CultureInfo.InvariantCulture).PersianNum();
            LblAllLoanNum.Content = Convert.ToInt32(_allData[0].AllLoanNum).ToString("N0", CultureInfo.InvariantCulture).PersianNum();
        }

        private async void BtnAllDataRep_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckEmpty()) return;
            var startDate = Utility.CurrectDate(TxtStartDate.Text);
            var endDate = Utility.CurrectDate(TxtEndDate.Text);

            try
            {
                _allPeriodData = await DAllData.GetPeriodAllData(startDate, endDate);
            }
            catch (Exception exception)
            {
                Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در دریافت اطلاعات\n" + exception.Message);
                return;
            }
            var report = new StiReport();
            report.Load("Report//PeriodFDloanFund.mrt");
            report.Dictionary.Databases.Clear();
            report.Dictionary.Databases.Add(new StiSqlDatabase("RepAllPer", @"Data Source=(LocalDB)\MSSQLLocalDB;Database=dbLoan;Integrated Security=True;Connect Timeout=30"));

            report.Dictionary.Variables.Add(new StiVariable("ShamsiDate", PersianDateTime.Now.Date.ToString("yyyy/MM/dd")));
            report.Dictionary.Variables.Add(new StiVariable("TimeNow", PersianDateTime.Now.TimeOfDay.ToHHMMSS()));

            report.Dictionary.Variables.Add(new StiVariable("StartDate", startDate));
            report.Dictionary.Variables.Add(new StiVariable("EndDate", endDate));

            report.Dictionary.Variables.Add(new StiVariable("LoanFundIniBal", _allData[0].LoanFundIniBal));
            report.Dictionary.Variables.Add(new StiVariable("BankAccIniBal", _allData[0].BankAccIniBal));
            report.Dictionary.Variables.Add(new StiVariable("Pay", _allData[0].Pay));
            report.Dictionary.Variables.Add(new StiVariable("Receivee", _allData[0].Receivee));
            report.Dictionary.Variables.Add(new StiVariable("MembershipFee", _allData[0].AllMembershipFee));
            report.Dictionary.Variables.Add(new StiVariable("IncomeAmount", _allData[0].IncomeAmount));
            report.Dictionary.Variables.Add(new StiVariable("FeeAmount", _allData[0].FeeAmount));
            report.Dictionary.Variables.Add(new StiVariable("InsTotalAmount", _allData[0].InsTotalAmount));
            report.Dictionary.Variables.Add(new StiVariable("TotalFund", _allData[0].TotalFund));
            report.Dictionary.Variables.Add(new StiVariable("AllLoanPay", _allData[0].AllLoanPay));
            report.Dictionary.Variables.Add(new StiVariable("AllWage", _allData[0].AllWage));
            report.Dictionary.Variables.Add(new StiVariable("InsTotalDueAmount", _allData[0].InsTotalDueAmount));
            report.Dictionary.Variables.Add(new StiVariable("InsRemaining", _allData[0].InsRemaining));
            report.Dictionary.Variables.Add(new StiVariable("LoanIsPayOff", _allData[0].LoanIsPayOff));
            report.Dictionary.Variables.Add(new StiVariable("LoanNotPayOff", _allData[0].LoanNotPayOff));
            report.Dictionary.Variables.Add(new StiVariable("AllLoanNum", _allData[0].AllLoanNum));

            report.Dictionary.Variables.Add(new StiVariable("PeriodPay", _allPeriodData[0].Pay));
            report.Dictionary.Variables.Add(new StiVariable("PeriodReceive", _allPeriodData[0].Receivee));
            report.Dictionary.Variables.Add(new StiVariable("PeriodMembershipFee", _allPeriodData[0].AllMembershipFee));
            report.Dictionary.Variables.Add(new StiVariable("PeriodIncomeAmount", _allPeriodData[0].IncomeAmount));
            report.Dictionary.Variables.Add(new StiVariable("PeriodFeeAmount", _allPeriodData[0].FeeAmount));
            report.Dictionary.Variables.Add(new StiVariable("PeriodInsTotalAmount", _allPeriodData[0].InsTotalAmount));
            report.Dictionary.Variables.Add(new StiVariable("PeriodTotalFund", _allPeriodData[0].TotalFund));
            report.Dictionary.Variables.Add(new StiVariable("PeriodAllLoanPay", _allPeriodData[0].AllLoanPay));
            report.Dictionary.Variables.Add(new StiVariable("PeriodInsTotalDueAmount", _allPeriodData[0].InsTotalDueAmount));
            report.Dictionary.Variables.Add(new StiVariable("PeriodInsRemaining", _allPeriodData[0].InsRemaining));
            report.Dictionary.Variables.Add(new StiVariable("PeriodLoanIsPayOff", _allPeriodData[0].LoanIsPayOff));
            report.Dictionary.Variables.Add(new StiVariable("PeriodLoanNotPayOff", _allPeriodData[0].LoanNotPayOff));
            report.Dictionary.Variables.Add(new StiVariable("PeriodAllLoanNum", _allPeriodData[0].AllLoanNum));

            report.ShowWithWpf();

        }

        private bool CheckEmpty()
        {
            if (string.IsNullOrEmpty(TxtStartDate.Text.Trim()))
            {
                Utility.Message("خطا", "لطفا تاریخ شروع بازه را وارد کنید", "Stop.png");
                return false;
            }

            if (!Utility.CheckDate(TxtStartDate.Text))
            {
                Utility.Message("خطا", "لطفا یک تاریخ صحیح برای تاریخ شروع بازه انتخاب کنید", "Stop.png");
                return false;
            }

            if (string.IsNullOrEmpty(TxtStartDate.Text.Trim()))
            {
                Utility.Message("خطا", "لطفا تاریخ پایان بازه را وارد کنید", "Stop.png");
                return false;
            }

            if (!Utility.CheckDate(TxtStartDate.Text))
            {
                Utility.Message("خطا", "لطفا یک تاریخ صحیح برای تاریخ پایان بازه انتخاب کنید", "Stop.png");
                return false;
            }
            return true;
        }
    }
}
