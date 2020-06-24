using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Arash;
using DAL;
using DAL.Class;
using Loan.Class;
using Stimulsoft.Report;
using Stimulsoft.Report.Dictionary;
using Clipboard = System.Windows.Clipboard;

// ReSharper disable PossibleInvalidOperationException

namespace Loan.Windows
{
    /// <summary>
    /// Interaction logic for WinAccounOperation.xaml
    /// </summary>
    public partial class WinAccountOperation
    {
        private List<spSelectAccountInfo_Result> _accountData;
        private bool _canEditDelete;

        public WinAccountOperation()
        {
            InitializeComponent();
            _accountData = new List<spSelectAccountInfo_Result>();
        }

        #region Properties

        public int PersonnelId { get; set; }

        public int? InfoId { get; set; }

        public string PersonnelMembershipDate { get; set; }

        #endregion

        #region Event

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _accountData = await DAccount.GetData(PersonnelId);
            }
            catch (Exception exception)
            {
                Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در دریافت اطلاعات حساب پس انداز\n" + exception.Message);
                return;
            }
            DgdAccount.ItemsSource = _accountData;

            BtnNew_Click(null, null);

            TxtCanReceive.Text = ((long) await DAccount.CanReceive(PersonnelId, InfoId)).ToString("N0",
                CultureInfo.InvariantCulture);

            LblPerCurrentBalance.Content =
                    _accountData.Count == 0
                        ? "0"
                        : long.Parse(_accountData[_accountData.Count - 1].AccountCurrentBalance.ToString())
                            .ToString("N0", CultureInfo.InvariantCulture);

            LblPerAllPay.Content =
                _accountData.Where(
                    t =>
                        t.Account_TransactionType_Id == 1 || t.Account_TransactionType_Id == 2 ||
                        t.Account_TransactionType_Id == 3 || t.Account_TransactionType_Id == 4 ||
                        t.Account_TransactionType_Id == 5 || t.Account_TransactionType_Id == 9 ||
                        t.Account_TransactionType_Id == 10)
                    .Sum(t => (long)t.AccountAmount)
                    .ToString("N0", CultureInfo.InvariantCulture);

            LblPerAllReceive.Content =
                _accountData.Where(t => t.Account_TransactionType_Id == 6)
                    .Sum(t => (long)t.AccountAmount)
                    .ToString("N0", CultureInfo.InvariantCulture);

            LblPerAllPayNum.Content =
                _accountData.Count(
                    t =>
                        t.Account_TransactionType_Id == 1 || t.Account_TransactionType_Id == 2 ||
                        t.Account_TransactionType_Id == 3).ToString();

            LblPerAllReceiveNum.Content = _accountData.Count(t => t.Account_TransactionType_Id == 6).ToString();
        }

        private async void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            #region AddPay
            if (!CheckEmpty() || !CheckPersonnelMembershipDate()) return;
            if (TabItemPay.IsSelected)
            {
                try
                {
                    var addAccount = new DAccount
                    {
                        DPersonnelId = PersonnelId,
                        DPaymentTypeId = Convert.ToByte(CboPayType.SelectedIndex),
                        DTransactionTypeId = 2,
                        DAmount = long.Parse(Regex.Replace(TxtPay.Text, "[\\W]", "")),
                        DReceiptNumber = TxtPayReceiptNum.Text.Trim() == string.Empty ? null : TxtPayReceiptNum.Text,
                        DCurrentBalance =
                            _accountData.Count == 0
                                ? long.Parse(Regex.Replace(TxtPay.Text, "[\\W]", ""))
                                : (long)_accountData[_accountData.Count - 1].AccountCurrentBalance +
                                  long.Parse(Regex.Replace(TxtPay.Text, "[\\W]", "")),

                        DPaymentDate = Utility.CurrectDate(TxtPayDate.Text),
                        DDescription = TxtPayDescription.Text.Trim() == string.Empty
                            ? null
                            : TxtPayDescription.Text
                    };
                    await addAccount.Add();

                    Utility.Message("پیام", "اطلاعات واریز با موفقیت ثبت گردید", "Correct.png");
                }
                catch (Exception exception)
                {
                    Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در ثبت اطلاعات واریز\n" + exception.Message);
                }
            }
            #endregion

            #region AddReceive
            else if (TabItemReceive.IsSelected)
            {
                if (!CheckCanRecAtDate()) return;
                try
                {
                    var addAccount = new DAccount
                    {
                        DPersonnelId = PersonnelId,
                        DPaymentTypeId = Convert.ToByte(CboReceiveType.SelectedIndex),
                        DTransactionTypeId = 6,
                        DAmount = long.Parse(Regex.Replace(TxtReceive.Text, "[\\W]", "")),
                        DReceiptNumber = TxtReceiveReceiptNum.Text.Trim() == string.Empty ? null : TxtReceiveReceiptNum.Text,
                        DCurrentBalance =
                            (long)_accountData[_accountData.Count - 1].AccountCurrentBalance -
                            long.Parse(Regex.Replace(TxtReceive.Text, "[\\W]", "")),
                        DPaymentDate = Utility.CurrectDate(TxtReceiveDate.Text),
                        DDescription = TxtReceiveDescription.Text.Trim() == string.Empty
                            ? null
                            : TxtReceiveDescription.Text
                    };
                    await addAccount.Add();

                    Utility.Message("پیام", "اطلاعات برداشت با موفقیت ثبت گردید", "Correct.png");
                }
                catch (Exception exception)
                {
                    Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در ثبت اطلاعات برداشت\n" + exception.Message);
                }
            }

            #endregion

            Window_Loaded(null, null);
        }

        private async void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _accountData = await DAccount.GetData(PersonnelId);
            }
            catch (Exception exception)
            {
                Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در دریافت اطلاعات حساب پس انداز\n" + exception.Message);
                return;
            }
            if (!CheckSelect()) return;
            var selectAccountItem = _accountData[DgdAccount.SelectedIndex];
            if (!CheckEmpty() || !CheckPersonnelMembershipDate()|| !CheckCanEdit(selectAccountItem.Id,selectAccountItem.Account_TransactionType_Id, selectAccountItem.AccountAmount,selectAccountItem.AccountPaymentDate)) return;

            #region EditPay

            if (TabItemPay.IsSelected)
            {
                try
                {
                    var editAccount = new DAccount
                    {
                        DId = selectAccountItem.Id,
                        DPersonnelId = selectAccountItem.Account_Personnel_Id,
                        DPaymentTypeId = Convert.ToByte(CboPayType.SelectedIndex),
                        DTransactionTypeId = selectAccountItem.Account_TransactionType_Id,
                        DAmount = long.Parse(Regex.Replace(TxtPay.Text, "[\\W]", "")),
                        DReceiptNumber = TxtPayReceiptNum.Text.Trim() == string.Empty ? null : TxtPayReceiptNum.Text,
                        DCurrentBalance = 0,

                        DPaymentDate = Utility.CurrectDate(TxtPayDate.Text),
                        DDescription = TxtPayDescription.Text.Trim() == string.Empty ? null : TxtPayDescription.Text
                    };
                    await Task.Run(()=> editAccount.Edit());

                    Utility.Message("پیام", "ویرایش با موفقیت انجام شد", "Correct.png");
                }
                catch (Exception exception)
                {
                    Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در ثبت ویرایش\n" + exception.Message);
                }
            }
            #endregion

            #region AddReceive

            else if (TabItemReceive.IsSelected)
            {
                try
                {
                    var addAccount = new DAccount
                    {
                        DId = selectAccountItem.Id,
                        DPersonnelId = PersonnelId,
                        DPaymentTypeId = Convert.ToByte(CboReceiveType.SelectedIndex),
                        DTransactionTypeId = 6,
                        DAmount = long.Parse(Regex.Replace(TxtReceive.Text, "[\\W]", "")),
                        DReceiptNumber = TxtReceiveReceiptNum.Text.Trim() == string.Empty
                            ? null
                            : TxtReceiveReceiptNum.Text,
                        DCurrentBalance = 0,
                        DPaymentDate = Utility.CurrectDate(TxtReceiveDate.Text),
                        DDescription = TxtReceiveDescription.Text.Trim() == string.Empty
                            ? null
                            : TxtReceiveDescription.Text
                    };
                    await Task.Run(() => addAccount.Edit());

                    Utility.Message("پیام", "ویرایش با موفقیت انجام شد", "Correct.png");
                }
                catch (Exception exception)
                {
                    Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در ثبت ویرایش\n" + exception.Message);
                }
            }

            #endregion

            Window_Loaded(null, null);
        }

        private async void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckSelect()) return;
            var selectAccountItem = _accountData[DgdAccount.SelectedIndex];
            if (!CheckCanDelete(selectAccountItem.Id, selectAccountItem.Account_TransactionType_Id, selectAccountItem.AccountAmount)) return;
            Utility.MyMessageBox("هشدار", "آیا از حذف تراکنش اطمینان دارید؟", "Warning.png", false);
            if (!Utility.YesNo) return;
            try
            {
                var deleteAccount = new DAccount
                {
                    DId = selectAccountItem.Id,
                    DPersonnelId = (int)selectAccountItem.Account_Personnel_Id
                };
                await Task.Run(() => deleteAccount.Delete());
                Utility.Message("پیام", "اطلاعات تراکنش مورد نظر با موفقیت حذف گردید", "Correct.png");
            }
            catch (Exception exception)
            {
                Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در حذف اطلاعات\n" + exception.Message);
                return;
            }
            Window_Loaded(null, null);
        }

        private void BtnNew_Click(object sender, RoutedEventArgs e)
        {
            TabItemPay.IsSelected = true;
            DgdAccount.SelectedIndex = -1;

            TxtPay.Focus();
            TxtPay.Text = "0";
            TxtPayDate.Text = PersianDate.Today.ToString();
            TxtPayReceiptNum.Text = string.Empty;
            TxtPayDescription.Text = string.Empty;
            CboPayType.SelectedIndex = 1;

            TxtPay.IsEnabled = true;
            TxtPayDate.IsEnabled = true;
            TxtPayReceiptNum.IsEnabled = true;
            TxtPayDescription.IsEnabled = true;
            CboPayType.IsEnabled = true;


            BtnAdd.IsEnabled = true;
        }

        private void BtnAccRep_Click(object sender, RoutedEventArgs e)
        {
            var report = new StiReport();
            report.Load("Report//PerAccount.mrt");
            report.Dictionary.Databases.Clear();
            report.Dictionary.Databases.Add(new StiSqlDatabase("RepLoan", @"Data Source=(LocalDB)\MSSQLLocalDB;Database=dbLoan;Integrated Security=True;Connect Timeout=30"));
            report.Dictionary.Variables.Add(new StiVariable("ShamsiDate", PersianDateTime.Now.Date.ToString("yyyy/MM/dd")));
            report.Dictionary.Variables.Add(new StiVariable("TimeNow", PersianDateTime.Now.TimeOfDay.ToHHMMSS()));
            report.Dictionary.Variables.Add(new StiVariable("programPath", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                @"ARKVN\Image\LoanFund\Picture\")));
            report.Dictionary.Variables.Add(new StiVariable("PerId", PersonnelId));
            report.ShowWithWpf();
        }

        private void DgdAccount_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DgdAccount.SelectedIndex == -1) return;
            BtnAdd.IsEnabled = false;
            _canEditDelete = true;
            var selectAccountItem = _accountData[DgdAccount.SelectedIndex];
            if (selectAccountItem.Account_TransactionType_Id != 6)
            {
                TabItemPay.IsSelected = true;
                TxtPay.Text = selectAccountItem.AccountAmount.ToString();
                TxtPayDate.Text = selectAccountItem.AccountPaymentDate;
                TxtPayReceiptNum.Text = selectAccountItem.AccountReceiptNumber;
                TxtPayDescription.Text = selectAccountItem.AccountDescription;
                CboPayType.SelectedIndex = Convert.ToInt32(selectAccountItem.Account_PaymentType_Id);
                if (selectAccountItem.Account_TransactionType_Id == 3 ||
                    selectAccountItem.Account_TransactionType_Id == 5)
                {
                    TxtPay.IsEnabled = false;
                    TxtPayDate.IsEnabled = false;
                    TxtPayReceiptNum.IsEnabled = false;
                    TxtPayDescription.IsEnabled = false;
                    CboPayType.IsEnabled = false;
                }
                else if (selectAccountItem.Account_TransactionType_Id == 11)
                {
                    var index = _accountData.FindIndex(x => x.Id == selectAccountItem.Id);
                    if (index - 2 >= 0 && _accountData[DgdAccount.SelectedIndex - 1].AccountDescription ==
                        "برداشت از وام برای حق عضویت")
                    {
                        TxtPay.IsEnabled = false;
                        TxtPayDate.IsEnabled = false;
                        TxtPayReceiptNum.IsEnabled = false;
                        TxtPayDescription.IsEnabled = false;
                        CboPayType.IsEnabled = false;
                        _canEditDelete = false;
                    }
                    else
                    {
                        TxtPay.IsEnabled = true;
                        TxtPayDate.IsEnabled = true;
                        TxtPayReceiptNum.IsEnabled = true;
                        TxtPayDescription.IsEnabled = true;
                        CboPayType.IsEnabled = true;
                    }
                }
                else
                {
                    TxtPay.IsEnabled = true;
                    TxtPayDate.IsEnabled = true;
                    TxtPayReceiptNum.IsEnabled = true;
                    TxtPayDescription.IsEnabled = true;
                    CboPayType.IsEnabled = true;
                }

            }
            else
            {
                TabItemReceive.IsSelected = true;
                TxtReceive.Text = selectAccountItem.AccountAmount.ToString();
                TxtReceiveDate.Text = selectAccountItem.AccountPaymentDate;
                TxtReceiveReceiptNum.Text = selectAccountItem.AccountReceiptNumber;
                TxtReceiveDescription.Text = selectAccountItem.AccountDescription;
                CboReceiveType.SelectedIndex = Convert.ToInt32(selectAccountItem.Account_PaymentType_Id);

                if (selectAccountItem.AccountDescription == "برداشت از وام برای حق عضویت")
                {
                    TxtReceive.IsEnabled = false;
                    TxtReceiveDate.IsEnabled = false;
                    TxtReceiveReceiptNum.IsEnabled = false;
                    TxtReceiveDescription.IsEnabled = false;
                    CboReceiveType.IsEnabled = false;
                    _canEditDelete = false;
                }
                else
                {
                    TxtReceive.IsEnabled = true;
                    TxtReceiveDate.IsEnabled = true;
                    TxtReceiveReceiptNum.IsEnabled = true;
                    TxtReceiveDescription.IsEnabled = true;
                    CboReceiveType.IsEnabled = true;
                }
            }

        }

        private void TabItemPay_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            TxtPay.Focus();
            TxtPay.Text = "0";
            TxtPayDate.Text = PersianDate.Today.ToString();
            TxtPayReceiptNum.Text = string.Empty;
            TxtPayDescription.Text = string.Empty;
            CboPayType.SelectedIndex = 1;

            TxtPay.IsEnabled = true;
            TxtPayDate.IsEnabled = true;
            TxtPayReceiptNum.IsEnabled = true;
            TxtPayDescription.IsEnabled = true;
            CboPayType.IsEnabled = true;

            TxtReceive.Focus();
            TxtReceive.Text = "0";
            TxtReceiveDate.Text = PersianDate.Today.ToString();
            TxtReceiveReceiptNum.Text = string.Empty;
            TxtReceiveDescription.Text = string.Empty;
            CboReceiveType.SelectedIndex = 1;

            TxtReceive.IsEnabled = true;
            TxtReceiveDate.IsEnabled = true;
            TxtReceiveReceiptNum.IsEnabled = true;
            TxtReceiveDescription.IsEnabled = true;
            CboReceiveType.IsEnabled = true;
        }

        private void TabItemReceive_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            TxtReceive.Focus();
            TxtReceive.Text = "0";
            TxtReceiveDate.Text = PersianDate.Today.ToString();
            TxtReceiveReceiptNum.Text = string.Empty;
            TxtReceiveDescription.Text = string.Empty;
            CboReceiveType.SelectedIndex = 1;

            TxtReceive.IsEnabled = true;
            TxtReceiveDate.IsEnabled = true;
            TxtReceiveReceiptNum.IsEnabled = true;
            TxtReceiveDescription.IsEnabled = true;
            CboReceiveType.IsEnabled = true;

            TxtPay.Focus();
            TxtPay.Text = "0";
            TxtPayDate.Text = PersianDate.Today.ToString();
            TxtPayReceiptNum.Text = string.Empty;
            TxtPayDescription.Text = string.Empty;
            CboPayType.SelectedIndex = 1;

            TxtPay.IsEnabled = true;
            TxtPayDate.IsEnabled = true;
            TxtPayReceiptNum.IsEnabled = true;
            TxtPayDescription.IsEnabled = true;
            CboPayType.IsEnabled = true;
        }

        private void TxtPay_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TabItemReceive.IsSelected) return;

            if (TxtPay.Text.Trim() == string.Empty) TxtPay.Text = "0";
            else
            {
                decimal number;
                if (!decimal.TryParse(TxtPay.Text, out number)) return;
                TxtPay.Text = $"{number:N0}";
                TxtPay.SelectionStart = TxtPay.Text.Length;
            }
        }

        private void TxtReceive_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!TabItemReceive.IsSelected) return;

            if (TxtReceive.Text.Trim() == string.Empty) TxtReceive.Text = "0";
            else
            {
                decimal number;
                if (!decimal.TryParse(TxtReceive.Text, out number)) return;
                TxtReceive.Text = $"{number:N0}";
                TxtReceive.SelectionStart = TxtReceive.Text.Length;
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

        private void NumberInput(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void DateInput(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex(@"[^0-9^\/]");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void DisablePaste(object sender, ExecutedRoutedEventArgs e)
        {
            var regex = new Regex("[^0-9]+");
            e.Handled = e.Command == ApplicationCommands.Paste && regex.IsMatch(Clipboard.GetText());
        }

        private void DisablePasteDate(object sender, ExecutedRoutedEventArgs e)
        {
            var regex = new Regex(@"[^0-9^\/]+");
            e.Handled = e.Command == ApplicationCommands.Paste && regex.IsMatch(Clipboard.GetText());
        }

        #endregion

        #region Method

        private bool CheckEmpty()
        {

            if (TabItemPay.IsSelected)
            {
                if (TxtPay.Text.Trim() == string.Empty || long.Parse(Regex.Replace(TxtPay.Text, "[\\W]", "")) == 0)
                {
                    Utility.Message("خطا", "لطفا مبلغ واریزی را وارد کنید", "Stop.png");
                    return false;
                }
                if (TxtPayDate.Text.Trim() == string.Empty)
                {
                    Utility.Message("خطا", "لطفا تاریخ پرداخت را وارد کنید", "Stop.png");
                    return false;
                }
                if (!Utility.CheckDate(TxtPayDate.Text))
                {
                    Utility.Message("خطا", "لطفا یک تاریخ صحیح برای پرداخت وارد کنید", "Stop.png");
                    return false;
                }
                if (CboPayType.SelectedIndex == 0)
                {
                    Utility.Message("خطا", "لطفا نوع پرداخت را مشخص کنید", "Stop.png");
                    return false;
                }
            }
            else if (TabItemReceive.IsSelected)
            {
                if (TxtReceive.Text.Trim() == string.Empty || long.Parse(Regex.Replace(TxtReceive.Text, "[\\W]", "")) == 0)
                {
                    Utility.Message("خطا", "لطفا مبلغ برداشت شده را وارد کنید", "Stop.png");
                    return false;
                }
                if (BtnAdd.IsEnabled && Convert.ToInt64(Regex.Replace(TxtCanReceive.Text, "[\\W]", "")) <
                    Convert.ToInt64(Regex.Replace(TxtReceive.Text, "[\\W]", "")))
                {
                    Utility.Message("خطا", "مبلغ وارد شده جهت برداشت بیشتر از مبلغ قابل برداشت است", "Stop.png");
                    return false;
                }
                if (TxtReceiveDate.Text.Trim() == string.Empty)
                {
                    Utility.Message("خطا", "لطفا تاریخ برداشت را وارد کنید", "Stop.png");
                    return false;
                }
                if (!Utility.CheckDate(TxtReceiveDate.Text))
                {
                    Utility.Message("خطا", "لطفا یک تاریخ صحیح برای برداشت وارد کنید", "Stop.png");
                    return false;
                }
                if (CboReceiveType.SelectedIndex == 0)
                {
                    Utility.Message("خطا", "لطفا نوع برداشت را مشخص کنید", "Stop.png");
                    return false;
                }
            }
            return true;
        }

        private bool CheckPersonnelMembershipDate()
        {
            if (TabItemPay.IsSelected &&
                string.CompareOrdinal(PersonnelMembershipDate, Utility.CurrectDate(TxtPayDate.Text)) <= 0 ||
                TabItemReceive.IsSelected &&
                string.CompareOrdinal(PersonnelMembershipDate, Utility.CurrectDate(TxtReceiveDate.Text)) <= 0)
                return true;
            Utility.Message("خطا", "تاریخ وارد شده قبل از عضویت شخص می باشد", "Stop.png");
            return false;
        }

        private bool CheckCanRecAtDate()
        {
            var allDate = _accountData.Where(
                t => string.CompareOrdinal(t.AccountPaymentDate, Utility.CurrectDate(TxtReceiveDate.Text)) <= 0).ToList();
            if (allDate.Count == 0)
            {
                Utility.Message("خطا", "برداشت در ابتدای حساب ممکن نیست!",
                    "Stop.png");
                return false;
            }
            var lastDate = allDate[allDate.Count - 1];
            var firstOrDefault = allDate.FirstOrDefault(x => x.Account_TransactionType_Id == 11);
            var memFee = firstOrDefault != null ? firstOrDefault.AccountAmount : 0;

            if (lastDate != null && lastDate.AccountCurrentBalance - memFee < long.Parse(Regex.Replace(TxtReceive.Text, "[\\W]", "")))
            {
                Utility.Message("خطا", "برداشت در تاریخ وارد شده به دلیل کافی نبودن مانده حساب میسر نمی باشد",
                    "Stop.png");
                return false;
            }

            allDate = _accountData.Where(
                t => string.CompareOrdinal(t.AccountPaymentDate, Utility.CurrectDate(TxtReceiveDate.Text)) > 0).ToList();

            if (allDate.Count > 0)
            {
                var remaining = allDate[0].AccountCurrentBalance -
                                long.Parse(Regex.Replace(TxtReceive.Text, "[\\W]", ""));
                if (remaining < 0)
                {
                    Utility.Message("خطا", "برداشت در تاریخ وارد شده به دلیل منفی کردن تراکنش های بعدی میسر نمی باشد",
                        "Stop.png");
                    return false;
                }
                foreach (var x in allDate)
                {
                    if (x.Id == allDate[0].Id) continue;
                    if (remaining >= 0)
                    {
                        if (x.Account_TransactionType_Id == 6)
                        {
                            remaining -= x.AccountAmount;
                        }
                        else
                        {
                            remaining += x.AccountAmount;
                        }
                    }
                    else
                    {
                        Utility.Message("خطا",
                            "برداشت در تاریخ وارد شده به دلیل منفی کردن تراکنش های بعدی میسر نمی باشد",
                            "Stop.png");
                        return false;
                    }
                }
            }
            return true;
        }

        private bool CheckSelect()
        {
            if (DgdAccount.SelectedIndex == -1)
            {
                Utility.Message("اخطار", "تراکنشی را انتخاب کنید", "Warning.png");
                return false;
            }
            return true;
        }

        private bool CheckCanDelete(int id, byte? transactionType , long? accountAmount)
        {
            if (transactionType == 5 || _canEditDelete == false)
            {
                Utility.Message("خطا", "شما مجاز به حذف تراکنش های مربوط به وام نیستید\nبرای حذف به قسمت وام ها رجوع کنید",
                    "Stop.png");
                return false;
            }
            if (transactionType == 3)
            {
                Utility.Message("خطا", "شما مجاز به حذف شارژ ماهانه نیستید\nبرای حذف به قسمت پرداخت شارژ ماهانه رجوع کنید",
                    "Stop.png");
                return false;
            }
            if (transactionType != 11 && transactionType != 6 && accountAmount >
                Convert.ToInt64(Regex.Replace(TxtCanReceive.Text, "[\\W]", "")))
            {
                Utility.Message("خطا", "مبلغ تراکنش مورد نظر برای حذف از مبلغ قابل برداشت بیشتر است", "Stop.png");
                return false;
            }

            if (transactionType == 6) return true;

            var index = _accountData.FindIndex(x => x.Id == id);
            var lastIndex = _accountData.Count - 1;
            if (lastIndex <= index) return true;

            var remaining = _accountData[index + 1].AccountCurrentBalance - long.Parse(Regex.Replace(TxtPay.Text, "[\\W]", ""));

            if (remaining < 0)
            {
                Utility.Message("خطا",
                    "حذف تراکنش در تاریخ موردنظر به دلیل منفی کردن تراکنش های بعدی میسر نمی باشد",
                    "Stop.png");
                return false;
            }

            if (lastIndex <= index + 1) return true;
            for (var i = index + 2 ; i <= lastIndex; i++)
            {
                if (_accountData[i].Account_TransactionType_Id == 6)
                {
                    remaining -= _accountData[i].AccountAmount;
                }
                else
                {
                    remaining += _accountData[i].AccountAmount;
                }
                if (remaining >= 0) continue;
                Utility.Message("خطا",
                    "حذف تراکنش در تاریخ موردنظر به دلیل منفی کردن تراکنش های بعدی میسر نمی باشد",
                    "Stop.png");
                return false;
            }

            return true;
        }

        private bool CheckCanEdit(int id, byte? transactionType, long? accountAmount, string accountDate)
        {
            if (transactionType == 5 || _canEditDelete == false)
            {
                Utility.Message("خطا", "شما مجاز به ویرایش تراکنش های مربوط به وام نیستید\nبرای ویرایش به قسمت وام ها رجوع کنید",
                    "Stop.png");
                return false;
            }

            if (transactionType == 3)
            {
                Utility.Message("خطا", "شما مجاز به ویرایش شارژ ماهانه نیستید\nبرای حذف به قسمت پرداخت شارژ ماهانه رجوع کنید",
                    "Stop.png");
                return false;
            }

            long? amountChange;
            long? amount;
            string date;
            if (TabItemPay.IsSelected)
            {
                amount = Convert.ToInt64(Regex.Replace(TxtPay.Text, "[\\W]", ""));
                amountChange = amount - accountAmount;
                date = Utility.CurrectDate(TxtPayDate.Text);
            }
            else
            {
                amount = Convert.ToInt64(Regex.Replace(TxtReceive.Text, "[\\W]", ""));
                amountChange = accountAmount - amount;
                date = Utility.CurrectDate(TxtReceiveDate.Text);
            }

            if (transactionType != 11 && amountChange < 0)
            {
                var canReceive = Convert.ToInt64(Regex.Replace(TxtCanReceive.Text, "[\\W]", ""));
                if (-amountChange > canReceive )
                {
                    Utility.Message("خطا", "مبلغ تراکنش مورد نظر برای ویرایش از مبلغ قابل برداشت بیشتر است",
                        "Stop.png");
                    return false;
                }
            }

            if (string.CompareOrdinal(accountDate, Utility.CurrectDate(TxtReceiveDate.Text)) != 0 || amountChange < 0)
            {
                var index = _accountData.FindIndex(x => x.Id == id);
                var allData = _accountData;
                allData[index].AccountPaymentDate = date;
                allData[index].AccountAmount = amount;
                var newAccountData = allData.OrderBy(x => x.AccountPaymentDate).ThenBy(x => x.Id);
                long? remaining = 0;
                foreach (var x in newAccountData)
                {
                    if (x.Account_TransactionType_Id == 6)
                    {
                        remaining -= x.AccountAmount;
                    }
                    else
                    {
                        remaining += x.AccountAmount;
                    }
                    if (remaining >= 0) continue;
                    Utility.Message("خطا",
                        "ویرایش تراکنش موردنظر به دلیل منفی کردن تراکنش های بعدی میسر نمی باشد",
                        "Stop.png");
                    return false;
                }
            }
            return true;
        }

        #endregion
    }
}