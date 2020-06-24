using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DAL;
using DAL.Class;
using Loan.Class;
using Stimulsoft.Report;
using Stimulsoft.Report.Dictionary;

namespace Loan.Windows
{
    /// <summary>
    /// Interaction logic for WinAccount.xaml
    /// </summary>
    public partial class WinPersonnelSearch
    {
        private List<spSelectPersonnelInfo_Result> _personnelData;
        private List<spSelectPersonnelInfo_Result> _personnelSearchData;
        private List<spAllData_Result> _allData;
        private List<tblLoan> _loanData;
        private readonly PersianCalendar _persianCalendar;
        private const string ConnetionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Database=dbLoan;Integrated Security=True;Connect Timeout=30";

        public string RepPicPath { get; set; }
        public string RepMessage { get; set; }
        public WinPersonnelSearch()
        {
            InitializeComponent();
            _personnelData = new List<spSelectPersonnelInfo_Result>();
            _personnelSearchData = new List<spSelectPersonnelInfo_Result>();
            _allData = new List<spAllData_Result>();
            _loanData = new List<tblLoan>();
            _persianCalendar = new PersianCalendar();
        }

        #region Event

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
            if (BtnAddChargeMonthly.Visibility == Visibility.Visible || BtnPayChargeMonthly.Visibility == Visibility.Visible)
            {
                _personnelSearchData = _personnelData.FindAll(x => x.PersonnelMembership == "1");
            }
            DgdPersonnel.ItemsSource = _personnelSearchData;
        }

        private void BtnPay_Click(object sender, RoutedEventArgs e)
        {

            if (!CheckSelect()) return;

            var selectItem = _personnelSearchData[DgdPersonnel.SelectedIndex];

            var winAccounOperation = new WinAccountOperation
            {
                LblTitle = { Content = "واریز - برداشت پول" },
                LblPerId = { Content = selectItem.PersonnelId },
                LblPerFirstName = { Content = selectItem.InfoFirstName },
                LblPerLastName = { Content = selectItem.InfoLastName },
                LblPerNationalCode = { Content = selectItem.InfoNationalCode },

                PersonnelId = selectItem.Id,
                PersonnelMembershipDate = selectItem.PersonnelMembershipDate,
                InfoId = selectItem.Personnel_Info_Id

            };
            winAccounOperation.ShowDialog();
        }

        private void BtnAddChargeMonthly_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckSelect()) return;

            var selectItem = _personnelSearchData[DgdPersonnel.SelectedIndex];

            var winChargeMonthly = new WinChargeMonthly
            {
                LblTitle = { Content = "ثبت اطلاعات شارژ ماهانه" },
                LblPerId = { Content = selectItem.PersonnelId },
                LblPerFirstName = { Content = selectItem.InfoFirstName },
                LblPerLastName = { Content = selectItem.InfoLastName },
                LblPerNationalCode = { Content = selectItem.InfoNationalCode },

                PersonnelId = selectItem.Id,
                PersonnelMembershipDate = selectItem.PersonnelMembershipDate
            };
            winChargeMonthly.ShowDialog();
        }

        private void BtnPayChargeMonthly_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckSelect()) return;

            var selectItem = _personnelSearchData[DgdPersonnel.SelectedIndex];

            var winChMoPay = new WinChMoPay
            {
                LblTitle = { Content = "ثبت اطلاعات پرداخت شارژ ماهانه" },
                LblPerId = { Content = selectItem.PersonnelId },
                LblPerFirstName = { Content = selectItem.InfoFirstName },
                LblPerLastName = { Content = selectItem.InfoLastName },
                LblPerNationalCode = { Content = selectItem.InfoNationalCode },

                PersonnelId = selectItem.Id,
                InfoId = selectItem.Personnel_Info_Id,
                PersonnelMembershipDate = selectItem.PersonnelMembershipDate
            };
            winChMoPay.ShowDialog();
        }

        private async void BtnAddLoan_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckSelect()) return;

            var selectItem = _personnelSearchData[DgdPersonnel.SelectedIndex];
            try
            {
                _loanData = await DLoan.GetLoanData(selectItem.Id);
            }
            catch (Exception exception)
            {
                Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در دریافت اطلاعات\n" + exception.Message);
                return;
            }
            var loanPayOff = _loanData.FindAll(item => item.LoanPayOff == false);
            if (loanPayOff.Count != 0)
            {
                Utility.MyMessageBox("پیام",
                    "این شخص " + loanPayOff.Count +
                    " عدد وام تسویه نشده دارد\nآیا مایل به پرداخت وام دیگری به این شخص هستید؟", "Warning.png", false);
                if (!Utility.YesNo) return;
            } 
            var winLoan = new WinLoan
            {
                LblTitle = {Content = "ثبت اطلاعات وام"},
                LblPerId = {Content = selectItem.PersonnelId},
                LblPerFirstName = {Content = selectItem.InfoFirstName},
                LblPerLastName = {Content = selectItem.InfoLastName},
                LblPerNationalCode = {Content = selectItem.InfoNationalCode},

                PersonnelId = selectItem.Id,
                PersonnelMembershipDate = selectItem.PersonnelMembershipDate,
                PersonnelInfoId = selectItem.Personnel_Info_Id
            };
            winLoan.ShowDialog();
        }

        private void DgdPersonnel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DgdPersonnel.SelectedIndex == -1) return;

            var selectItem = _personnelSearchData[DgdPersonnel.SelectedIndex];
            LblPerId.Content = selectItem.PersonnelId;
            LblPerFirstName.Content = selectItem.InfoFirstName;
            LblPerLastName.Content = selectItem.InfoLastName;
            LblPerNationalCode.Content = selectItem.InfoNationalCode;
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            LblPerId.Content = string.Empty;
            LblPerFirstName.Content = string.Empty;
            LblPerLastName.Content = string.Empty;
            LblPerNationalCode.Content = string.Empty;

            var search = TxtSearch.Text;//todo barresi shavad dar search che etefaghi oftad

            _personnelSearchData = _personnelData;

            if (BtnAddLoan.Visibility != Visibility.Visible)
            {
                _personnelSearchData = _personnelData.FindAll(x => !string.IsNullOrEmpty(x.PersonnelId));
            }
            _personnelSearchData =
                _personnelSearchData.FindAll(
                    t =>
                        !string.IsNullOrEmpty(t.PersonnelId) && t.PersonnelId.Contains(search) ||
                        !string.IsNullOrEmpty(t.InfoFirstName) && t.InfoFirstName.Contains(search) ||
                        !string.IsNullOrEmpty(t.InfoLastName) && t.InfoLastName.Contains(search) ||
                        !string.IsNullOrEmpty(t.InfoNationalCode) && t.InfoNationalCode.Contains(search) ||
                        !string.IsNullOrEmpty(t.InfoCode) && t.InfoCode.Contains(search) ||
                        !string.IsNullOrEmpty(t.InfoMobile) && t.InfoMobile.Contains(search) ||
                        !string.IsNullOrEmpty(t.InfoTell) && t.InfoTell.Contains(search) ||
                        !string.IsNullOrEmpty(t.InfoPostalCode) && t.InfoPostalCode.Contains(search) ||
                        !string.IsNullOrEmpty(t.InfoAddress) && t.InfoAddress.Contains(search));
            DgdPersonnel.ItemsSource = _personnelSearchData;
        }

        private void BtnAccRep_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckSelect()) return;

            var report = new StiReport();
            report.Load("Report//PerAccount.mrt");
            report.Dictionary.Databases.Clear();
            report.Dictionary.Databases.Add(new StiSqlDatabase("RepLoan", ConnetionString));
            report.Dictionary.Variables.Add(new StiVariable("ShamsiDate", PersianDateTime.Now.Date.ToString("yyyy/MM/dd")));
            report.Dictionary.Variables.Add(new StiVariable("TimeNow", PersianDateTime.Now.TimeOfDay.ToHHMMSS()));
            report.Dictionary.Variables.Add(new StiVariable("programPath", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                @"ARKVN\Image\LoanFund\Picture\")));
            report.Dictionary.Variables.Add(new StiVariable("PerId", _personnelSearchData[DgdPersonnel.SelectedIndex].Id));
            report.ShowWithWpf();
        }

        private async void BtnPerRep_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckSelect()) return;
            var selectItem = _personnelSearchData[DgdPersonnel.SelectedIndex];
            var insDelayNum = 0;
            var insDelayDays = 0;
            var chMoDelayNum = 0;
            var chMoDelayMonth = 0;
            try
            {
                _allData = await DAllData.GetAllData();
                var loanData = (await DLoan.GetLoanData(selectItem.Id)).Where(x => x.LoanPayOff == false);
                foreach (var loan in loanData)
                {
                    var installment =
                        (await DInstallment.GetData(loan.Id)).Where(t => t.Installment_PaymentType_Id == null &&
                                                                         PersianDateTime.Now.Date >
                                                                         PersianDateTime.Parse(t.InstallmentDueDate));
                    var tblInstallments = installment as IList<tblInstallment> ?? installment.ToList();
                    insDelayNum += tblInstallments.Count;
                    insDelayDays = tblInstallments.Aggregate(insDelayDays,
                        (current, x) => current + Convert.ToInt16((PersianDateTime.Now.Date -
                                                                   PersianDateTime.Parse(x.InstallmentDueDate)).Days));
                }
                var year = _persianCalendar.GetYear(PersianDateTime.Now.Date.ToDateTime());
                var month = _persianCalendar.GetMonth(PersianDateTime.Now.Date.ToDateTime());
                var chargeMonthlyData = await DChargeMonthly.GetData(selectItem.Id);
                foreach (var x in chargeMonthlyData)
                {
                    var chMoYear =
                        _persianCalendar.GetYear(PersianDateTime.Parse(x.ChargeMonthlyStartDate).ToDateTime());
                    var chMoStartMonth =
                        _persianCalendar.GetMonth(PersianDateTime.Parse(x.ChargeMonthlyStartDate).ToDateTime());
                    var chMoEndMonth =
                        _persianCalendar.GetMonth(PersianDateTime.Parse(x.ChargeMonthlyEndDate).ToDateTime());
                    var chMoPayNum = (await DChMoPay.GetChMoPayData(x.Id)).Count;
                    int chMoMustPay;
                    if (year > chMoYear)
                        chMoMustPay = chMoEndMonth - chMoStartMonth - chMoPayNum + 1;
                    else if (year == chMoYear)
                    {
                        if (month <= chMoStartMonth)
                            chMoMustPay = 0;
                        else
                        {
                            if (month <= chMoEndMonth)
                                chMoMustPay = month - chMoStartMonth - chMoPayNum + 1;
                            else
                                chMoMustPay = chMoEndMonth - chMoStartMonth - chMoPayNum + 1;
                        }
                    }

                    else
                        chMoMustPay = 0;

                    if (chMoMustPay < 0)
                        chMoMustPay = 0;
                    chMoDelayNum += chMoMustPay;

                    var delayMonth = Convert.ToInt16((year - chMoYear) * 12 + month - (chMoStartMonth + chMoPayNum));

                    for (var i = 0; i < chMoMustPay; i++)
                    {
                        chMoDelayMonth += delayMonth - i;
                    }
                }
            }
            catch (Exception exception)
            {
                Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در دریافت اطلاعات\n" + exception.Message);
                return;
            }
            var report = new StiReport();
            report.Load("Report//ReportForPerson.mrt");
            report.Dictionary.Databases.Clear();
            report.Dictionary.Databases.Add(new StiSqlDatabase("RepAllPer", ConnetionString));
            report.Dictionary.Variables.Add(new StiVariable("ShamsiDate", PersianDateTime.Now.Date.ToString("yyyy/MM/dd")));
            report.Dictionary.Variables.Add(new StiVariable("TimeNow", PersianDateTime.Now.TimeOfDay.ToHHMMSS()));
            report.Dictionary.Variables.Add(new StiVariable("programPath", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                @"ARKVN\Image\Personnel\Picture\")));
            report.Dictionary.Variables.Add(new StiVariable("PerId", _personnelSearchData[DgdPersonnel.SelectedIndex].Id));
            report.Dictionary.Variables.Add(new StiVariable("PicPath", RepPicPath));
            report.Dictionary.Variables.Add(new StiVariable("Massage", RepMessage));
            report.Dictionary.Variables.Add(new StiVariable("AllLoanPay", _allData[0].AllLoanPay));
            report.Dictionary.Variables.Add(new StiVariable("AllLoanNum", _allData[0].AllLoanNum));
            report.Dictionary.Variables.Add(new StiVariable("InsDelayNum", insDelayNum));
            report.Dictionary.Variables.Add(new StiVariable("InsDelayDays", insDelayDays));
            report.Dictionary.Variables.Add(new StiVariable("ChMoDelayNum", chMoDelayNum));
            report.Dictionary.Variables.Add(new StiVariable("ChMoDelayMonth", chMoDelayMonth));
            report.ShowWithWpf();
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

        //baraye shomare gozari datagrid
        private void dataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        #endregion

        #region Method

        private bool CheckSelect()
        {
            if (DgdPersonnel.SelectedIndex == -1 || LblPerId.Content == null)
            {
                Utility.Message("خطا", "شخصی را انتخاب کنید", "Stop.png");
                return false;
            }

            return true;
        }


        #endregion


    }
}

//var report = new StiReport();
//report.Load("Report//PerFDloan.mrt");
//report.Dictionary.Variables.Add(new StiVariable("ShamsiDate", PersianDateTime.Now.Date.ToString("yyyy/MM/dd")));
//report.Dictionary.Variables.Add(new StiVariable("TimeNow", PersianDateTime.Now.TimeOfDay.ToHHMMSS()));
//report.Dictionary.Variables.Add(new StiVariable("programPath", Directory.GetCurrentDirectory()));
//report.Dictionary.Variables.Add(new StiVariable("PerId", _personnelSearchData[DgdPersonnel.SelectedIndex].Id));
//report.Dictionary.Variables.Add(new StiVariable("LoanFundIniBal", _allData[0].LoanFundIniBal));
//report.Dictionary.Variables.Add(new StiVariable("BankAccIniBal", _allData[0].BankAccIniBal));
//report.Dictionary.Variables.Add(new StiVariable("Pay", _allData[0].Pay));
//report.Dictionary.Variables.Add(new StiVariable("Receivee", _allData[0].Receivee));
//report.Dictionary.Variables.Add(new StiVariable("MembershipFee", _allData[0].AllMembershipFee));
//report.Dictionary.Variables.Add(new StiVariable("IncomeAmount", _allData[0].IncomeAmount));
//report.Dictionary.Variables.Add(new StiVariable("FeeAmount", _allData[0].FeeAmount));
//report.Dictionary.Variables.Add(new StiVariable("InsTotalAmount", _allData[0].InsTotalAmount));
//report.Dictionary.Variables.Add(new StiVariable("TotalFund", _allData[0].TotalFund));
//report.Dictionary.Variables.Add(new StiVariable("AllLoanPay", _allData[0].AllLoanPay));
//report.Dictionary.Variables.Add(new StiVariable("AllWage", _allData[0].AllWage));
//report.Dictionary.Variables.Add(new StiVariable("InsTotalDueAmount", _allData[0].InsTotalDueAmount));
//report.Dictionary.Variables.Add(new StiVariable("InsRemaining", _allData[0].InsRemaining));
//report.Dictionary.Variables.Add(new StiVariable("LoanIsPayOff", _allData[0].LoanIsPayOff));
//report.Dictionary.Variables.Add(new StiVariable("LoanNotPayOff", _allData[0].LoanNotPayOff));
//report.Dictionary.Variables.Add(new StiVariable("AllLoanNum", _allData[0].AllLoanNum));
//report.ShowWithWpf();
