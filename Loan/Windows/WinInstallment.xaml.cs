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
// ReSharper disable PossibleInvalidOperationException

namespace Loan.Windows
{
    /// <summary>
    /// Interaction logic for WinInstallment.xaml
    /// </summary>
    public partial class WinInstallment
    {
        private List<tblInstallment> _installmentData;
        private List<spSelectAccountInfo_Result> _accountData;
        private List<tblGuarantor> _loanGuarantorData;

        public WinInstallment()
        {
            InitializeComponent();
            _installmentData = new List<tblInstallment>();
            _accountData = new List<spSelectAccountInfo_Result>();
            _loanGuarantorData = new List<tblGuarantor>();
        }

        #region Properties

        public int PersonnelId { get; set; }

        public int LoanId { get; set; }

        public string LoanDate { get; set; }


        #endregion

        #region Event

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BtnNew_Click(null, null);
            try
            {
                _installmentData = await DInstallment.GetData(LoanId);
                _loanGuarantorData = await DGuarantor.GetData(LoanId);
            }
            catch (Exception exception)
            {
                Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در دریافت اطلاعات\n" + exception.Message);
                Close();
                return;
            }

            DgdInstallment.ItemsSource = _installmentData;

            LblPerInsNum.Content = _installmentData.Count.ToString();

            LblPerInsNumPay.Content = _installmentData.Count(t => t.Installment_PaymentType_Id != null).ToString();

            LblPerInsNumRemaining.Content =
                (Convert.ToInt32(LblPerInsNum.Content) - Convert.ToInt32(LblPerInsNumPay.Content)).ToString();

            LblPerInsDelayed.Content =
                _installmentData.Count(
                    t =>
                        t.Installment_PaymentType_Id == null &&
                        PersianDateTime.Now.Date > PersianDateTime.Parse(t.InstallmentDueDate)).ToString();

            LblPerInsAmount.Content = _installmentData.Count > 1
                ? _installmentData[1].InstallmentDueAmount.GetValueOrDefault().ToString("N0", CultureInfo.InvariantCulture)
                : _installmentData[0].InstallmentDueAmount.GetValueOrDefault().ToString("N0", CultureInfo.InvariantCulture);

            var count = _installmentData.Count(t => t.Installment_PaymentType_Id != null);

            LblPerInsAmountPay.Content = count != 0
                ? _installmentData[count - 1].InstallmentTotalPaid.GetValueOrDefault()
                    .ToString("N0", CultureInfo.InvariantCulture)
                : "0";


            LblPerInsAmountRemaining.Content = count != 0
                ? _installmentData[count - 1].InstallmentRemaining.GetValueOrDefault()
                    .ToString("N0", CultureInfo.InvariantCulture)
                : _installmentData.Sum(t => t.InstallmentDueAmount).GetValueOrDefault()
                    .ToString("N0", CultureInfo.InvariantCulture);
        }

        private async void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckEmpty() || !CheckLoanDate() || !CheckLastPay()) return;

            var count = _installmentData.Count(t => t.Installment_PaymentType_Id != null);
            //tedad aghsate pardakht shodeh
            var count1 = count;

            var selectInstallmentItem = _installmentData[count]; //entekhabe ghest jari

            var amount = long.Parse(Regex.Replace(TxtPayIns.Text, "[\\W]", "")); //pul varizi
            var amount1 = amount;

            var dueAmount = selectInstallmentItem.InstallmentDueAmount; //mablagh harghest

            if (amount >= dueAmount)
            {
                var repeatNum = 0;

                #region MoreCheck

                while (amount1 > 0)
                {
                    if (_installmentData.Count > count1)
                    {
                        amount1 = amount1 - _installmentData[count1].InstallmentDueAmount.GetValueOrDefault();
                        count1++;
                        repeatNum++;
                    }
                    else if (amount1 > 0)
                    {
                        Utility.MyMessageBox("هشدار",
                            " مبلغ واریزی، بیشتر از مبلغ مورد نیاز برای پرداخت کل باقی مانده ی وام است.\n آیا مایل به واریز باقی مانده مبلغ واریزی در حساب پس انداز هستید؟",
                            "Warning.png", false);
                        if (Utility.YesNo)
                            break;
                        return;
                    }
                }

                if (amount1 < 0)
                {
                    repeatNum--;
                    Utility.MyMessageBox("هشدار",
                        $"مبلغ واریزی، بیشتر از مبلغ مورد نیاز برای پرداخت {repeatNum} عدد قسط است\n آیا مایل به واریز باقی مانده مبلغ واریزی در حساب پس انداز هستید؟",
                        "Warning.png", false);
                    if (!Utility.YesNo)
                    {
                        return;
                    }
                }

                #endregion

                var remaining = count != 0
                    ? _installmentData[count - 1].InstallmentRemaining
                    : _installmentData.Sum(t => t.InstallmentDueAmount); //mande bedehi

                var payInsReceiptNum = TxtPayInsReceiptNum.Text.Trim() == string.Empty ? null : TxtPayInsReceiptNum.Text;
                var payInsDate = Utility.CurrectDate(TxtPayInsDate.Text);
                var payInsDescription = TxtPayInsDescription.Text.Trim() == string.Empty
                    ? null
                    : TxtPayInsDescription.Text;

                for (var i = 0; i < repeatNum; i++)
                {
                    #region addInstallment

                    try
                    {
                        var addInstallment = new DInstallment
                        {
                            DId = selectInstallmentItem.Id,
                            DPaymentTypeId = (byte?)CboPayInsType.SelectedIndex,
                            DAmount = dueAmount,
                            DReceiptNumber = payInsReceiptNum,
                            DTotalPaid =
                                count == 0
                                    ? dueAmount
                                    : _installmentData[count - 1].InstallmentTotalPaid + dueAmount,
                            DRemaining = remaining - dueAmount,
                            DPaymentDate = payInsDate,
                            DDelayDay =
                                Convert.ToInt16(
                                    (PersianDateTime.Parse(payInsDate) -
                                     PersianDateTime.Parse(selectInstallmentItem.InstallmentDueDate)).Days),
                            DDescription = payInsDescription
                        };
                        await Task.Run(() => addInstallment.AddPayment());



                        remaining -= dueAmount;
                        amount -= dueAmount.GetValueOrDefault();
                        count++;
                        if (_installmentData.Count > count)
                        {
                            selectInstallmentItem = _installmentData[count];
                        }
                        dueAmount = selectInstallmentItem.InstallmentDueAmount;
                    }

                    catch (Exception exception)
                    {
                        Utility.MyMessageBox("خطا در بانک اطلاعاتی",
                            $"خطا در ثبت اطلاعات اقساط در حساب پس انداز\n مبلغ {amount} ریال در اقساط ثبت نشد\n {exception.Message}");
                        return;
                    }
                    try
                    {
                        _installmentData = await DInstallment.GetData(LoanId);
                    }
                    catch (Exception exception)
                    {
                        Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در دریافت اطلاعات\n" + exception.Message);
                        return;
                    }
                    #endregion
                }
                if (_installmentData.Count == count)
                {
                    #region EditLoan // sabte tasviye vam

                    try
                    {
                        var editLoan = new DLoan
                        {
                            DId = LoanId,
                            DPayOff = true
                        };
                        await Task.Run(() => editLoan.EditPayOff());
                        Utility.MyMessageBox("پیام","اقساط این وام به طور کامل پرداخت شد","Correct.png");
                    }

                    catch (Exception exception)
                    {
                        Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در ثبت تسویه حساب وام\n" + exception.Message);
                    }

                    #endregion

                    foreach (var t in _loanGuarantorData)
                    {
                        #region EditGuarantor // taghir vaziat masdodi zamen ha

                        if (t.GuarantorBlock == true)
                        {
                            try
                            {
                                var editGuarantor = new DGuarantor
                                {
                                    DId = t.id,
                                    DBlock = false
                                };
                                await Task.Run(() => editGuarantor.EditGuarantorBlock());
                            }

                            catch (Exception exception)
                            {
                                Utility.MyMessageBox("خطا در بانک اطلاعاتی",
                                    "خطا در بازگرداندن ضامن به حالت غیر مسدود برای این وام" + exception.Message);
                            }
                        }

                        #endregion
                    }
                }
                if (amount > 0)
                {
                    #region addAccount //sabte baghimandeh dar hesabe karbari

                    try
                    {
                        _accountData = await DAccount.GetData(PersonnelId);
                        var addAccount = new DAccount
                        {
                            DPersonnelId = PersonnelId,
                            DPaymentTypeId = (byte)CboPayInsType.SelectedIndex,
                            DTransactionTypeId = 10,
                            DAmount = amount,
                            DReceiptNumber = payInsReceiptNum,
                            DCurrentBalance =
                                _accountData.Count == 0
                                    ? amount
                                    : (long)_accountData[_accountData.Count - 1].AccountCurrentBalance + amount,
                            DPaymentDate = payInsDate,
                            DDescription = payInsDescription
                        };
                        await addAccount.Add();
                        Utility.MyMessageBox("پیام","باقی مانده پول در حساب کاربر ثبت شد", "Correct.png");
                    }
                    catch (Exception exception)
                    {
                        Utility.MyMessageBox("خطا در بانک اطلاعاتی",
                            $"خطا در ثبت اطلاعات باقی مانده قسط در حساب پس انداز\n مبلغ {amount} ریال در حساب پس انداز ثبت نشد\n {exception.Message}");
                    }

                    #endregion
                }
                Utility.Message("پیام", "اطلاعات با موفقیت ثبت گردید", "Correct.png");
                Window_Loaded(null, null);
            }
            else
            {
                Utility.Message("پیام", "مبلغ وارد شده از مبلغ یک قسط کمتر است", "Stop.png");
            }

        }

        private async void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckSelect() || !CheckEmpty() || !CheckLoanDate() || !CheckPaymentsForEdit()) return;
            var selectInstallmentItem = _installmentData[DgdInstallment.SelectedIndex];
            var payInsDate = Utility.CurrectDate(TxtPayInsDate.Text);
            try
            {
                var editInstallment = new DInstallment
                {
                    DId = selectInstallmentItem.Id,
                    DPaymentTypeId = (byte?) CboPayInsType.SelectedIndex,
                    DReceiptNumber = TxtPayInsReceiptNum.Text.Trim() == string.Empty ? null : TxtPayInsReceiptNum.Text,
                    DPaymentDate = payInsDate,
                    DDelayDay =
                        Convert.ToInt16(
                            (PersianDateTime.Parse(payInsDate) -
                             PersianDateTime.Parse(selectInstallmentItem.InstallmentDueDate)).Days),
                    DDescription = TxtPayInsDescription.Text.Trim() == string.Empty
                        ? null
                        : TxtPayInsDescription.Text
                };
                await Task.Run(() => editInstallment.Edit());
            }

            catch (Exception exception)
            {
                Utility.MyMessageBox("خطا در بانک اطلاعاتی",
                    $"خطا در ویرایش اطلاعات اقساط در حساب پس انداز\n {exception.Message}");
            }
            Window_Loaded(null, null);
            Utility.Message("پیام", "اطلاعات قسط با موفقیت ویرایش گردید", "Correct.png");
        }

        private async void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckSelect()) return;
            var count = _installmentData.Count(t => t.Installment_PaymentType_Id != null);

            if (!CheckCanDelete(count)) return;
            var selectInstallmentItem = _installmentData[DgdInstallment.SelectedIndex];

            Utility.MyMessageBox("هشدار", @"آیا از حذف قسط اطمینان دارید؟
توجه داشته باشید که تنها مجاز به حذف آخرین قسط پرداختی هستید و در صورتی که قسط وام مورد نظر، هنگام ثبت باقی مانده داشته است، این باقی مانده از حساب شخص حذف نخواهد شد",
                "Warning.png", false);

            if (!Utility.YesNo) return;

            if (_installmentData.Count == count)
            {
                #region EditLoan // hazfe tasviye vam

                try
                {
                    var editLoan = new DLoan
                    {
                        DId = LoanId,
                        DPayOff = false
                    };
                    await Task.Run(() => editLoan.EditPayOff());
                    Utility.MyMessageBox("پیام", "این وام به حالت درگردش درآمد","Correct.png");
                }

                catch (Exception exception)
                {
                    Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در حذف تسویه حساب وام" + "\n" + exception.Message);
                    return;
                }

                #endregion

                foreach (var t in _loanGuarantorData)
                {
                    #region EditGuarantor // taghir vaziat masdodi zamen ha

                    if (t.GuarantorBlock == false && t.Guarantor_BlockType_Id != 1)
                    {
                        try
                        {
                            var editGuarantor = new DGuarantor
                            {
                                DId = t.id,
                                DBlock = true
                            };
                            await Task.Run(() => editGuarantor.EditGuarantorBlock());
                        }

                        catch (Exception exception)
                        {
                            Utility.MyMessageBox("خطا در بانک اطلاعاتی",
                                "خطا در بازگرداندن ضامن به حالت مسدود برای این وام" + "\n" + exception.Message);
                            return;
                        }
                    }

                    #endregion
                }
            }


            #region addInstallment

            try
            {
                var addInstallment = new DInstallment
                {
                    DId = selectInstallmentItem.Id,
                    DPaymentTypeId = null,
                    DAmount = null,
                    DReceiptNumber = null,
                    DTotalPaid = null,
                    DRemaining = null,
                    DPaymentDate = null,
                    DDelayDay = null,
                    DDescription = null
                };
                await Task.Run(() => addInstallment.AddPayment());
            }

            catch (Exception exception)
            {
                Utility.MyMessageBox("خطا در بانک اطلاعاتی",$"خطا در حذف اطلاعات اقساط در حساب پس انداز\n {exception.Message}");
                return;
            }

            #endregion


            Utility.Message("پیام", "اطلاعات قسط مورد نظر با موفقیت حذف گردید", "Correct.png");

            Window_Loaded(null, null);
        }

        private void BtnNew_Click(object sender, RoutedEventArgs e)
        {
            TxtPayIns.Text = "0";
            TxtPayInsDate.Text = PersianDate.Today.ToString();
            TxtPayInsReceiptNum.Text = string.Empty;
            TxtPayInsDescription.Text = string.Empty;
            CboPayInsType.SelectedIndex = 1;
            DgdInstallment.SelectedIndex = -1;
            BtnAdd.IsEnabled = true;
            TxtPayIns.IsEnabled = true;
            TxtPayInsDate.IsEnabled = true;
            TxtPayInsReceiptNum.IsEnabled = true;
            TxtPayInsDescription.IsEnabled = true;
            CboPayInsType.IsEnabled = true;

        }

        private void DgdInstallment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DgdInstallment.SelectedIndex == -1) return;
            var selectItem = _installmentData[DgdInstallment.SelectedIndex];
            if (selectItem.Installment_PaymentType_Id == null)
            {
                TxtPayIns.Text = string.Empty;
                TxtPayInsDate.Text = string.Empty;
                TxtPayInsReceiptNum.Text = string.Empty;
                TxtPayInsDescription.Text = string.Empty;
                CboPayInsType.SelectedIndex = 0;
                TxtPayIns.IsEnabled = false;
                TxtPayInsDate.IsEnabled = false;
                TxtPayInsReceiptNum.IsEnabled = false;
                TxtPayInsDescription.IsEnabled = false;
                CboPayInsType.IsEnabled = false;
                return;
            }
            BtnAdd.IsEnabled = false;
            TxtPayIns.Text = selectItem.InstallmentAmount.ToString();
            TxtPayInsDate.Text = selectItem.InstallmentPaymentDate;
            TxtPayInsReceiptNum.Text = selectItem.InstallmentReceiptNumber;
            TxtPayInsDescription.Text = selectItem.InstallmentDescription;
            CboPayInsType.SelectedIndex = Convert.ToInt32(selectItem.Installment_PaymentType_Id);

            TxtPayIns.IsEnabled = false;
            TxtPayInsDate.IsEnabled = true;
            TxtPayInsReceiptNum.IsEnabled = true;
            TxtPayInsDescription.IsEnabled = true;
            CboPayInsType.IsEnabled = true;
        }

        private void TxtPayIns_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TxtPayIns.Text.Trim() == string.Empty) TxtPayIns.Text = "0";
            else
            {
                decimal number;
                if (!decimal.TryParse(TxtPayIns.Text, out number)) return;
                TxtPayIns.Text = $"{number:N0}";
                TxtPayIns.SelectionStart = TxtPayIns.Text.Length;
            }
        }

        private void BtnInsRep_Click(object sender, RoutedEventArgs e)
        {
            var report = new StiReport();
            report.Load("Report//PerLoanIns.mrt");
            report.Dictionary.Databases.Clear();
            report.Dictionary.Databases.Add(new StiSqlDatabase("RepLoan", @"Data Source=(LocalDB)\MSSQLLocalDB;Database=dbLoan;Integrated Security=True;Connect Timeout=30"));
            report.Dictionary.Variables.Add(new StiVariable("ShamsiDate", PersianDateTime.Now.Date.ToString("yyyy/MM/dd")));
            report.Dictionary.Variables.Add(new StiVariable("TimeNow", PersianDateTime.Now.TimeOfDay.ToHHMMSS()));
            report.Dictionary.Variables.Add(new StiVariable("programPath", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                @"ARKVN\Image\LoanFund\Picture\")));
            report.Dictionary.Variables.Add(new StiVariable("LoanId", LoanId));
            report.Dictionary.Variables.Add(new StiVariable("PerId", PersonnelId));
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
            if (BtnAdd.IsEnabled == false &&
                _installmentData[DgdInstallment.SelectedIndex].Installment_PaymentType_Id == null)
            {
                Utility.Message("خطا", "تنها مجاز به ویرایش اقساط پرداخت شده هستید", "Stop.png");
                return false;
            }

            if (_installmentData.Where(t => t.InstallmentRemaining != null).Count(t => t.InstallmentRemaining == 0) == 1)
            {
                Utility.Message("خطا", "این وام تسویه شده است", "Stop.png");
                return false;
            }
            if (long.Parse(Regex.Replace(TxtPayIns.Text, "[\\W]", "")) == 0)
            {
                Utility.Message("خطا", "لطفا مبلغ را وارد کنید", "Stop.png");
                return false;
            }
            if (TxtPayInsDate.Text.Trim() == string.Empty)
            {
                Utility.Message("خطا", "لطفا تاریخ پرداخت وام را وارد کنید", "Stop.png");
                return false;
            }
            if (!Utility.CheckDate(TxtPayInsDate.Text))
            {
                Utility.Message("خطا", "لطفا یک تاریخ معتبر برای تاریخ پرداخت وام وارد کنید", "Stop.png");
                return false;
            }
            if (CboPayInsType.SelectedIndex == 0)
            {
                Utility.Message("خطا", "لطفا نوع پرداخت را مشخص کنید", "Stop.png");
                return false;
            }
            return true;
        }

        private bool CheckLoanDate()
        {
            //tedad aghsate pardakht shodeh

            if (string.CompareOrdinal(LoanDate,Utility.CurrectDate(TxtPayInsDate.Text)) > 0)
            {
                Utility.Message("خطا", "تاریخ قسط وارد شده قبل از تاریخ پرداخت وام می باشد", "Stop.png");
                return false;
            }
            return true;
        }

        private bool CheckSelect()
        {
            if (DgdInstallment.SelectedIndex == -1)
            {
                Utility.Message("اخطار", "قسط پرداخت شده موردنظر را انتخاب کنید.", "Warning.png");
                return false;
            }
            return true;
        }

        private bool CheckCanDelete(int count)
        {
            if (DgdInstallment.SelectedIndex != count - 1)
            {
                Utility.Message("خطا", "شما تنها مجاز به حذف آخرین قسط پرداخت شده هستید","Stop.png");
                return false;
            }

            return true;
        }

        private bool CheckLastPay()
        {
            var count = _installmentData.Count(t => t.Installment_PaymentType_Id != null);
            if (count > 0)
            {
                var lastInsDate = _installmentData[count - 1].InstallmentPaymentDate;
                if (string.CompareOrdinal(Utility.CurrectDate(TxtPayInsDate.Text), lastInsDate) < 0)
                {
                    Utility.Message("خطا", "تاریخ قسط وارد شده قبل از تاریخ آخرین قسط پرداختی  می باشد", "Stop.png");
                    return false;
                }
            }

            return true;
        }

        private bool CheckPaymentsForEdit()
        {
            var selectItem = _installmentData[DgdInstallment.SelectedIndex + 1];
            if (_installmentData[DgdInstallment.SelectedIndex].Installment_PaymentType_Id == null)
            {
                Utility.Message("خطا", "تنها مجاز به ویرایش اقساط پرداخت شده هستید", "Stop.png");
                return false;
            }

            if (DgdInstallment.SelectedIndex + 1 < _installmentData.Count &&
                _installmentData[DgdInstallment.SelectedIndex + 1].Installment_PaymentType_Id != null &&
                string.CompareOrdinal(selectItem.InstallmentPaymentDate, Utility.CurrectDate(TxtPayInsDate.Text)) < 0)
            {
                Utility.Message("خطا", "تاریخ قسط وارد شده بعد از تاریخ قسط بعدی می باشد", "Stop.png");
                return false;
            }
            if (DgdInstallment.SelectedIndex - 1 >= 0 &&
                string.CompareOrdinal(Utility.CurrectDate(TxtPayInsDate.Text), _installmentData[DgdInstallment.SelectedIndex - 1].InstallmentPaymentDate) < 0)
            {
                Utility.Message("خطا", "تاریخ قسط وارد شده قبل از تاریخ قسط قبلی می باشد", "Stop.png");
                return false;
            }
            return true;
        }

        #endregion
    }
}