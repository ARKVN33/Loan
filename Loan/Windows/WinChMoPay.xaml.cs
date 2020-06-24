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
    /// Interaction logic for WinChMoPay.xaml
    /// </summary>
    public partial class WinChMoPay
    {
        private List<spSelectAccountInfo_Result> _accountData;
        private List<spSelectChMoPayInfo_Result> _chMoPayInfoData;
        private List<tblChargeMonthly> _chargeMonthlyData;
        private readonly PersianCalendar _persianCalendar;

        private long  _canReceive;
        public WinChMoPay()
        {
            InitializeComponent();
            _accountData = new List<spSelectAccountInfo_Result>();
            _chMoPayInfoData = new List<spSelectChMoPayInfo_Result>();
            _chargeMonthlyData = new List<tblChargeMonthly>();
            _persianCalendar = new PersianCalendar();
        }

        #region Properties

        public int PersonnelId { get; set; }

        public int? InfoId;

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
                Close();
                return;
            }

            LblPerCurrentBalance.Content =
                _accountData.Count == 0
                    ? "0"
                    : long.Parse(_accountData[_accountData.Count - 1].AccountCurrentBalance.ToString())
                        .ToString("N0", CultureInfo.InvariantCulture);

            try
            {
                _chMoPayInfoData = await DChMoPay.GetData(PersonnelId);
                _chargeMonthlyData = await DChargeMonthly.GetData(PersonnelId);
            }
            catch (Exception exception)
            {
                Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در دریافت اطلاعات شارژ ماهانه\n" + exception.Message);
                Close();
                return;
            }

            var chMoDelayNum = 0;
            try
            {
                var year = _persianCalendar.GetYear(PersianDateTime.Now.Date.ToDateTime());
                var month = _persianCalendar.GetMonth(PersianDateTime.Now.Date.ToDateTime());
                var chargeMonthlyData = await DChargeMonthly.GetData(PersonnelId);
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
                }
            }
            catch (Exception exception)
            {
                Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در دریافت اطلاعات\n" + exception.Message);
                Close();
                return;
            }
            DgdAccount.ItemsSource = _chMoPayInfoData;
            DgdChargeMonthly.ItemsSource = _chargeMonthlyData;

            BtnNew_Click(null, null);

            LblPerAllChMoNum.Content =
                _chargeMonthlyData.Sum(
                    t =>
                        _persianCalendar.GetMonth(PersianDateTime.Parse(t.ChargeMonthlyEndDate).ToDateTime()) -
                        _persianCalendar.GetMonth(PersianDateTime.Parse(t.ChargeMonthlyStartDate).ToDateTime()) + 1)
                    .ToString();

            LblPerAllChMoPayNum.Content = _chMoPayInfoData.Count.ToString();


            LblPerAllChMoUnpaidNum.Content =
                (Convert.ToInt32(LblPerAllChMoNum.Content) - Convert.ToInt32(LblPerAllChMoPayNum.Content)).ToString();

            LblPerAllChMoDelayedNum.Content = chMoDelayNum;
        }

        private async void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckSelectChargeMonthly()) return;
            if (!CheckEmpty() || !CheckPersonnelMembershipDate()) return;

            var selectChargeMonthlyItem = _chargeMonthlyData[DgdChargeMonthly.SelectedIndex];
            var payChMo = Convert.ToInt64(Regex.Replace(TxtPayChMo.Text, "[\\W]", ""));
            var chargeMonthlyCharge = selectChargeMonthlyItem.ChargeMonthlyCharge.GetValueOrDefault();
            var chMoCanPayNum = payChMo / chargeMonthlyCharge;
            var remaining = payChMo % chargeMonthlyCharge;
            var repeatNum = chMoCanPayNum;

            if (!ChekMore(chMoCanPayNum, remaining)) return;

            try
            {
                _accountData = await DAccount.GetData(PersonnelId);
            }
            catch (Exception exception)
            {
                Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در دریافت اطلاعات حساب پس انداز\n" + exception.Message);
                return;
            }

            var chMoReceiptNum = TxtPayChMoReceiptNum.Text.Trim() == string.Empty ? null : TxtPayChMoReceiptNum.Text;
            var chMoDescription = TxtPayChMoDescription.Text.Trim() == string.Empty
                ? null
                : TxtPayChMoDescription.Text;

            var chMoMaxPay = int.Parse(LblPerChMoUnpaidNum.Content.ToString());
            var payChMoDate = Utility.CurrectDate(TxtPayChMoDate.Text);

            if (payChMo >= chargeMonthlyCharge)
            {
                if (chMoMaxPay < chMoCanPayNum)
                {
                    repeatNum = chMoMaxPay;
                }
                var chMoYear =
                    _persianCalendar.GetYear(
                        PersianDateTime.Parse(selectChargeMonthlyItem.ChargeMonthlyStartDate).ToDateTime());
                var chMoMonth =
                    _persianCalendar.GetMonth(
                        PersianDateTime.Parse(selectChargeMonthlyItem.ChargeMonthlyStartDate).ToDateTime());
                var year = _persianCalendar.GetYear(PersianDateTime.Parse(payChMoDate).ToDateTime());
                var month = _persianCalendar.GetMonth(PersianDateTime.Parse(payChMoDate).ToDateTime());

                var chMoPayNum = Convert.ToInt32(LblPerChMoPayNum.Content);

                for (var i = 0; i < repeatNum; i++)
                {
                    #region AddAccount

                    int accountId;
                    try
                    {
                        var addAccount = new DAccount
                        {
                            DPersonnelId = PersonnelId,
                            DPaymentTypeId = Convert.ToByte(CboPayChMoType.SelectedIndex),
                            DTransactionTypeId = 3,
                            DAmount = chargeMonthlyCharge,
                            DReceiptNumber = chMoReceiptNum,
                            DCurrentBalance =
                                _accountData.Count == 0
                                    ? chargeMonthlyCharge
                                    : (long)_accountData[_accountData.Count - 1].AccountCurrentBalance +
                                      chargeMonthlyCharge,

                            DPaymentDate = payChMoDate,
                            DDescription = chMoDescription
                        };
                        accountId = await addAccount.Add();
                    }
                    catch (Exception exception)
                    {
                        Utility.MyMessageBox("خطا در بانک اطلاعاتی",
                            $"خطا در ثبت اطلاعات واریز شارژ ماهانه در حساب پس انداز\n مبلغ {payChMo} ریال در حساب پس انداز ثبت نشد\n {exception.Message}");
                        return;
                    }

                    #endregion

                    #region AddChMoPay

                    try
                    {
                        var addChMoPay = new DChMoPay
                        {
                            DChargeMonthlyId = selectChargeMonthlyItem.Id,
                            DAccountId = Convert.ToInt32(accountId),
                            DDueAmount = chargeMonthlyCharge,
                            DDueDate =
                                $"{chMoYear}/{((chMoMonth + chMoPayNum).ToString().Length == 1 ? "0" + (chMoMonth + chMoPayNum) : (chMoMonth + chMoPayNum).ToString())}/{_persianCalendar.GetDaysInMonth(chMoYear, chMoMonth + chMoPayNum)}",
                            DDelayMonth = Convert.ToInt16((year - chMoYear) * 12 + month - (chMoMonth + chMoPayNum))
                        };
                        await Task.Run(() => addChMoPay.Add());
                    }
                    catch (Exception exception)
                    {
                        Utility.MyMessageBox("خطا در بانک اطلاعاتی",
                            $"خطا در ثبت اطلاعات واریز شارژ ماهانه\n مبلغ {payChMo} ریال در حساب پس انداز ثبت نشد\n {exception.Message}");
                        try
                        {
                            var deleteAccount = new DAccount
                            {
                                DId = Convert.ToInt32(accountId)
                            };
                            deleteAccount.Delete();
                            return;
                        }
                        catch (Exception)
                        {
                            Utility.MyMessageBox("خطا در بانک اطلاعاتی",
                                "خطا در حذف اطلاعات شارژ ماهانه از حساب پس انداز" + exception.Message);
                        }
                    }
                    payChMo -= chargeMonthlyCharge;
                    chMoPayNum = chMoPayNum + 1;

                    #endregion

                    try
                    {
                        _accountData = await DAccount.GetData(PersonnelId);
                    }
                    catch (Exception exception)
                    {
                        Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در دریافت اطلاعات حساب پس انداز\n" + exception.Message);
                    }
                }

                if (payChMo < chargeMonthlyCharge && payChMo != 0 || chMoMaxPay < chMoCanPayNum)
                {
                    try
                    {
                        var addAccount = new DAccount
                        {
                            DPersonnelId = PersonnelId,
                            DPaymentTypeId = Convert.ToByte(CboPayChMoType.SelectedIndex),
                            DTransactionTypeId = 9,
                            DAmount = payChMo,
                            DReceiptNumber = chMoReceiptNum,
                            DCurrentBalance =
                                _accountData.Count == 0
                                    ? payChMo
                                    : (long)_accountData[_accountData.Count - 1].AccountCurrentBalance + payChMo,
                            DPaymentDate = payChMoDate,
                            DDescription = chMoDescription
                        };
                        await addAccount.Add();
                        Utility.MyMessageBox("پیام","باقی مانده پول در حساب کاربر ثبت شد","Correct.png");
                    }
                    catch (Exception exception)
                    {
                        Utility.MyMessageBox("خطا در بانک اطلاعاتی",
                            $"خطا در ثبت اطلاعات واریز شارژ ماهانه در حساب پس انداز\n مبلغ {payChMo} ریال در حساب پس انداز ثبت نشد\n {exception.Message}");
                    }
                }
                Utility.Message("پیام", "اطلاعات واریز شارژ ماهانه با موفقیت ثبت گردید", "Correct.png");
                Window_Loaded(null, null);
            }
            else
            {
                Utility.Message("پیام", "مبلغ وارد شده از مبلغ یک شارژ ماهانه کمتر است", "Stop.png");
            }
        }

        private async void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckSelect()) return;

            _canReceive = (long)await DAccount.CanReceive(PersonnelId, InfoId);
            if (!CheckCanDelete()) return;

            var selectAccountItem = _chMoPayInfoData[DgdAccount.SelectedIndex];
            Utility.MyMessageBox("هشدار", "آیا از حذف تراکنش اطمینان دارید؟\nتوجه داشته باشید در صورتی که شارژ ماهانه مورد نظر هنگام ثبت باقی مانده داشته است، این باقی مانده از حساب شخص حذف نخواهد شد","Warning.png",false);
            if (!Utility.YesNo) return;
            try
            {
                var deleteChMoPay = new DChMoPay
                {
                    DAccountId = selectAccountItem.Id
                };
                await Task.Run(() => deleteChMoPay.Delete());

                var deleteAccount = new DAccount
                {
                    DId = selectAccountItem.Id,
                    DPersonnelId = (int)selectAccountItem.Account_Personnel_Id
                };
                await Task.Run(() => deleteAccount.Delete());
                Utility.Message("پیام", "اطلاعات آخرین تراکنش با موفقیت حذف گردید", "Correct.png");
            }
            catch (Exception exception)
            {
                Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در حذف اطلاعات" + exception.Message);
                return;
            }
            Window_Loaded(null, null);

        }

        private void BtnNew_Click(object sender, RoutedEventArgs e)
        {
            TxtPayChMo.Focus();

            LblPerChMoNum.Content = string.Empty;
            LblPerChMoPayNum.Content = string.Empty;
            LblPerChMoUnpaidNum.Content = string.Empty;
            LblPerChMoDelayedNum.Content = string.Empty;

            TxtPayChMo.Text = "0";
            TxtPayChMoDate.Text = PersianDate.Today.ToString();
            TxtPayChMoReceiptNum.Text = string.Empty;
            TxtPayChMoDescription.Text = string.Empty;

            CboPayChMoType.SelectedIndex = 1;

            DgdChargeMonthly.SelectedIndex = -1;
            DgdAccount.SelectedIndex = -1;
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
        }

        private async void DgdChargeMonthly_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DgdChargeMonthly.SelectedIndex == -1) return;

            var selectChargeMonthlyItem = _chargeMonthlyData[DgdChargeMonthly.SelectedIndex];
            TxtPayChMo.Text = selectChargeMonthlyItem.ChargeMonthlyCharge.ToString();
            LblPerChMoNum.Content =
            (_persianCalendar.GetMonth(
                 PersianDateTime.Parse(selectChargeMonthlyItem.ChargeMonthlyEndDate).ToDateTime()) -
             _persianCalendar.GetMonth(
                 PersianDateTime.Parse(selectChargeMonthlyItem.ChargeMonthlyStartDate).ToDateTime()) + 1).ToString();

            LblPerChMoPayNum.Content =
                _chMoPayInfoData.Count(t => t.ChMoPay_ChargeMonthly_Id == selectChargeMonthlyItem.Id).ToString();

            LblPerChMoUnpaidNum.Content =
                (Convert.ToInt32(LblPerChMoNum.Content) - Convert.ToInt32(LblPerChMoPayNum.Content)).ToString();

            int chMoDelayNum;
            try
            {
                var year = _persianCalendar.GetYear(PersianDateTime.Now.Date.ToDateTime());
                var month = _persianCalendar.GetMonth(PersianDateTime.Now.Date.ToDateTime());
                var chMoYear =
                    _persianCalendar.GetYear(PersianDateTime.Parse(selectChargeMonthlyItem.ChargeMonthlyStartDate)
                        .ToDateTime());
                var chMoStartMonth =
                    _persianCalendar.GetMonth(PersianDateTime.Parse(selectChargeMonthlyItem.ChargeMonthlyStartDate)
                        .ToDateTime());
                var chMoEndMonth =
                    _persianCalendar.GetMonth(PersianDateTime.Parse(selectChargeMonthlyItem.ChargeMonthlyEndDate)
                        .ToDateTime());
                var chMoPayNum = (await DChMoPay.GetChMoPayData(selectChargeMonthlyItem.Id)).Count;
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
                chMoDelayNum = chMoMustPay;
            }
            catch (Exception exception)
            {
                Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در دریافت اطلاعات\n" + exception.Message);
                return;
            }

            LblPerChMoDelayedNum.Content = chMoDelayNum;

            if (DgdAccount.SelectedIndex != -1)
            {
                DgdAccount.SelectedIndex = -1;
                BtnAdd.IsEnabled = true;
            }
        }

        private void TxtPayChMo_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TxtPayChMo.Text.Trim() == string.Empty)
            {
                TxtPayChMo.Text = "0";
            }
            else
            {
                decimal number;
                if (!decimal.TryParse(TxtPayChMo.Text, out number)) return;
                TxtPayChMo.Text = $"{number:N0}";
                TxtPayChMo.SelectionStart = TxtPayChMo.Text.Length;
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
            if (Convert.ToInt32(LblPerChMoPayNum.Content) == Convert.ToInt32(LblPerChMoNum.Content))
            {
                Utility.Message("خطا", "تمام شارژهای ماهیانه این دوره پرداخت شده است", "Stop.png");
                return false;
            }
            if (string.IsNullOrEmpty(TxtPayChMo.Text.Trim()) || long.Parse(Regex.Replace(TxtPayChMo.Text, "[\\W]", "")) == 0)
            {
                Utility.Message("خطا", "لطفا مبلغ شارژ ماهانه را وارد کنید", "Stop.png");
                return false;
            }
            if (TxtPayChMoDate.Text.Trim() == string.Empty)
            {
                Utility.Message("خطا", "لطفا تاریخ پرداخت شارژ ماهانه را وارد کنید", "Stop.png");
                return false;
            }
            if (!Utility.CheckDate(TxtPayChMoDate.Text))
            {
                Utility.Message("خطا", "لطفا یک تاریخ صحیح برای پرداخت شارژ ماهانه وارد کنید", "Stop.png");
                return false;
            }
            if (CboPayChMoType.SelectedIndex == 0)
            {
                Utility.Message("خطا", "لطفا نوع پرداخت را مشخص کنید", "Stop.png");
                return false;
            }

            return true;
        }

        private bool CheckPersonnelMembershipDate()
        {
            if (string.CompareOrdinal(PersonnelMembershipDate, Utility.CurrectDate(TxtPayChMoDate.Text)) <= 0)
                return true;
            Utility.Message("خطا", "تاریخ وارد شده قبل از عضویت شخص می باشد", "Stop.png");
            return false;
        }

        private bool CheckSelectChargeMonthly()
        {
            if (DgdChargeMonthly.SelectedIndex == -1)
            {
                Utility.Message("خطا", "شارژ ماهانه ای را برای پرداخت انتخاب کنید", "Stop.png");
                return false;
            }
            return true;
        }

        private bool ChekMore(long repeatNum, long remaining)
        {
            if (Convert.ToInt32(LblPerChMoUnpaidNum.Content) < repeatNum)
            {
                Utility.MyMessageBox("هشدار",
                    " مبلغ واریزی، بیشتر از مبلغ مورد نیاز برای پرداخت کل باقی مانده ی شارژ ماهانه است. آیا مایل به واریز باقی مانده مبلغ واریزی در حساب پس انداز هستید؟",
                    "Warning.png", false);
                return Utility.YesNo;
            }

            if (remaining > 0 && repeatNum > 0)
            {
                Utility.MyMessageBox("هشدار",
                    $"مبلغ واریزی، بیشتر از مبلغ مورد نیاز برای پرداخت {repeatNum} عدد شارژ ماهانه است\n آیا مایل به واریز باقی مانده مبلغ واریزی در حساب پس انداز هستید؟",
                    "Warning.png", false);
                return Utility.YesNo;
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

        private bool CheckCanDelete()
        {
            
            var selectAccountItem = _chMoPayInfoData[DgdAccount.SelectedIndex];
            var selectchMoPayInfoItem = _chMoPayInfoData.Where(x => x.ChMoPay_ChargeMonthly_Id == selectAccountItem.ChMoPay_ChargeMonthly_Id).ToList();
            if (selectAccountItem.Expr1 != selectchMoPayInfoItem[selectchMoPayInfoItem.Count - 1].Expr1)
            {
                Utility.Message("خطا", "شما تنها مجاز به حذف آخرین پرداخت های هر دوره شارژ ماهانه هستید", "Stop.png");
                return false;
            }
            if (selectAccountItem.AccountAmount > _canReceive)
            {
                Utility.Message("خطا", "مبلغ تراکنش مورد نظر برای حذف از مبلغ قابل برداشت بیشتر است",
                    "Stop.png");
                return false;
            }

            var index = _accountData.FindIndex(x => x.Id == selectAccountItem.Id);
            var lastIndex = _accountData.Count - 1;
            if (lastIndex <= index) return true;

            var remaining = _accountData[index + 1].AccountCurrentBalance - selectAccountItem.AccountAmount;

            if (remaining < 0)
            {
                Utility.Message("خطا",
                    "حذف تراکنش در تاریخ موردنظر به دلیل منفی کردن تراکنش های بعدی میسر نمی باشد",
                    "Stop.png");
                return false;
            }

            if (lastIndex <= index + 1) return true;
            for (var i = index + 2; i <= lastIndex; i++)
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
        
        #endregion
    }
}