using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DAL;
using DAL.Class;
using Loan.Class;

namespace Loan.Windows
{
    /// <summary>
    /// Interaction logic for WinChargeMonthly.xaml
    /// </summary>
    public partial class WinChargeMonthly
    {

        private List<spSelectChMoPayInfo_Result> _accountData;
        private List<tblChargeMonthly> _chargeMonthlyData;
        private readonly PersianCalendar _persianCalendar;
        private DChargeMonthly _chargeMonthly;

        public WinChargeMonthly()
        {
            InitializeComponent();
            _persianCalendar = new PersianCalendar();
            _chargeMonthlyData = new List<tblChargeMonthly>();
            _accountData = new List<spSelectChMoPayInfo_Result>();
        }

        #region Properties

        public int PersonnelId { get; set; }

        public string PersonnelMembershipDate { get; set; }

        #endregion

        #region Event

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BtnNew_Click(null, null);
            try
            {
                _chargeMonthlyData = await DChargeMonthly.GetData(PersonnelId);
            }
            catch (Exception exception)
            {
                Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در دریافت اطلاعات شارژ ماهانه\n" + exception.Message);
                Close();
                return;
            }
            DgdChargeMonthly.ItemsSource = _chargeMonthlyData;
        }

        private async void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckEmpty() || !CheckYear() || !CheckPersonnelMembershipDate() || !CheckPeriod())
                return;

            var year = int.Parse(TxtStartYear.Text);
            var startMonth = CboStartMonth.SelectedIndex;
            var endMonth = CboEndMonth.SelectedIndex;
            var totalYear = int.Parse(TxtEndYear.Text) - int.Parse(TxtStartYear.Text);

            for (var i = 0; i <= totalYear; i++)
            {
                if (totalYear >= 1)
                {
                    if (i == 0)
                    {
                        startMonth = CboStartMonth.SelectedIndex;
                        endMonth = 12;
                    }
                    else if (i == totalYear)
                    {
                        year = int.Parse(TxtEndYear.Text);
                        startMonth = 1;
                        endMonth = CboEndMonth.SelectedIndex;
                    }
                    else
                    {
                        year++;
                        startMonth = 1;
                        endMonth = 12;
                    }
                }
                try
                {
                    _chargeMonthly = new DChargeMonthly
                    {
                        DPersonnelId = PersonnelId,
                        DChargeMonthlyStartDate =
                            $"{year}/{(startMonth.ToString().Length == 1 ? "0" + startMonth : startMonth.ToString())}/" +
                            "01",
                        DChargeMonthlyEndDate =
                            $"{year}/{(endMonth.ToString().Length == 1 ? "0" + endMonth : endMonth.ToString())}/{_persianCalendar.GetDaysInMonth(year, endMonth)}",
                        DChargeMonthlyCharge = long.Parse(Regex.Replace(TxtCharge.Text, "[\\W]", "")),
                        DChargeMonthlyDescription = TxtChargeMonthlyDescription.Text.Trim() == string.Empty
                            ? null
                            : TxtChargeMonthlyDescription.Text
                    };
                    await Task.Run(() => _chargeMonthly.Add());
                }
                catch (Exception exception)
                {
                    Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در ثبت اطلاعات\n" + exception.Message);
                    return;
                }
            }
            Window_Loaded(null, null);
            Utility.Message("پیام", "اطلاعات با موفقیت ثبت گردید", "Correct.png");
        }

        private async void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckSelect() || !CheckEmpty() || !CheckPersonnelMembershipDate() || !CheckPeriod() || !CheckYear())
                return;

            var selectChargeMonthlyItem = _chargeMonthlyData[DgdChargeMonthly.SelectedIndex];

            var beforeMonth =
                _persianCalendar.GetMonth(
                    PersianDateTime.Parse(selectChargeMonthlyItem.ChargeMonthlyEndDate).ToDateTime());
            var newMonth = CboEndMonth.SelectedIndex;

            if (_accountData.Count(t => t.ChMoPay_ChargeMonthly_Id == selectChargeMonthlyItem.Id) != 0 &&
                !CheckMonth(beforeMonth, newMonth))
                return; //check kardan in ke aya charge mahane-e ba in Id pardakht shode dar sora
            try
            {
                _chargeMonthly = new DChargeMonthly
                {
                    DId = selectChargeMonthlyItem.Id,
                    DPersonnelId = PersonnelId,
                    DChargeMonthlyStartDate =
                        $"{TxtStartYear.Text}/{(CboStartMonth.SelectedIndex.ToString().Length == 1 ? "0" + CboStartMonth.SelectedIndex : CboStartMonth.SelectedIndex.ToString())}/" +
                        "01",

                    DChargeMonthlyEndDate =
                        $"{TxtStartYear.Text}/{(CboEndMonth.SelectedIndex.ToString().Length == 1 ? "0" + CboEndMonth.SelectedIndex : CboEndMonth.SelectedIndex.ToString())}/{_persianCalendar.GetDaysInMonth(int.Parse(TxtStartYear.Text), CboEndMonth.SelectedIndex)}",

                    DChargeMonthlyCharge = long.Parse(Regex.Replace(TxtCharge.Text, "[\\W]", "")),
                    DChargeMonthlyDescription = TxtChargeMonthlyDescription.Text.Trim() == string.Empty
                        ? null
                        : TxtChargeMonthlyDescription.Text
                };
                await Task.Run(() => _chargeMonthly.Edit());
            }
            catch (Exception exception)
            {
                Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در ویرایش اطلاعات\n" + exception.Message);
                return;
            }
            Window_Loaded(null, null);
            Utility.Message("پیام", "اطلاعات با موفقیت ویرایش گردید", "Correct.png");
        }

        private async void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckSelect()) return;

            var selectChargeMonthlyItem = _chargeMonthlyData[DgdChargeMonthly.SelectedIndex];

            if (_accountData.Count(t => t.ChMoPay_ChargeMonthly_Id == selectChargeMonthlyItem.Id) == 0)
                //check kardan in ke hich gone pardakhti sorat nagerefteh bashad
            {
                Utility.MyMessageBox("هشدار", "آیا از حذف این شارژ ماهانه اطمینان دارید؟ ","Warning.png",false);
                if (!Utility.YesNo) return;
                try
                {
                    _chargeMonthly = new DChargeMonthly
                    {
                        DId = selectChargeMonthlyItem.Id
                    };
                    await Task.Run(() => _chargeMonthly.Delete());
                }
                catch (Exception exception)
                {
                    Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در حذف اطلاعات\n" + exception.Message);
                    return;
                }
                Window_Loaded(null, null);

                Utility.Message("پیام", "اطلاعات با موفقیت حذف گردید", "Correct.png");
            }
            else
            {
                Utility.Message("خطا", "به دلیل موجود بودن سوابق مالی برای این شارژ ماهانه قادر به حذف آن نیستید",
                    "Stop.png");
            }
        }

        private void BtnNew_Click(object sender, RoutedEventArgs e)
        {
            TxtStartYear.Text = TxtEndYear.Text = _persianCalendar.GetYear(PersianDateTime.Now.ToDateTime()).ToString();
            TxtCharge.Text = "0";
            CboStartMonth.SelectedIndex = CboEndMonth.SelectedIndex = 1;
            CboStartMonth.IsEnabled = CboEndMonth.IsEnabled = true;
            TxtEndYear.IsEnabled = TxtStartYear.IsEnabled = true;
            BtnUp.IsEnabled = BtnUp1.IsEnabled = BtnDown.IsEnabled = BtnDown1.IsEnabled = true;
            BtnAdd.IsEnabled = true;
            TxtCharge.IsEnabled = true;
            TxtChargeMonthlyDescription.IsEnabled = true;

            DgdChargeMonthly.SelectedIndex = -1;
        }

        private void BtnUp_Click(object sender, RoutedEventArgs e)
        {
            if (TxtStartYear.Text.Trim() == string.Empty || int.Parse(TxtStartYear.Text) < 1350)
            {
                TxtStartYear.Text = _persianCalendar.GetYear(PersianDateTime.Now.ToDateTime()).ToString();
            }
            else if (int.Parse(TxtStartYear.Text) >= 9999)
            {
                TxtStartYear.Text = _persianCalendar.GetYear(PersianDateTime.Now.ToDateTime()).ToString();
            }
            else
            {
                TxtStartYear.Text = (int.Parse(TxtStartYear.Text) + 1).ToString();
            }
            if (BtnEdit.IsEnabled)
            {
                TxtEndYear.Text = TxtStartYear.Text;
            }
        }

        private void BtnDown_Click(object sender, RoutedEventArgs e)
        {
            if (TxtStartYear.Text.Trim() == string.Empty || int.Parse(TxtStartYear.Text) < 1350)
            {
                TxtStartYear.Text = _persianCalendar.GetYear(PersianDateTime.Now.ToDateTime()).ToString();
            }
            else
            {
                TxtStartYear.Text = (int.Parse(TxtStartYear.Text) - 1).ToString();
            }
            if (BtnEdit.IsEnabled)
            {
                TxtEndYear.Text = TxtStartYear.Text;
            }
        }

        private void BtnUp1_Click(object sender, RoutedEventArgs e)
        {
            if (TxtEndYear.Text.Trim() == string.Empty || int.Parse(TxtEndYear.Text) < 1350)
            {
                TxtEndYear.Text = _persianCalendar.GetYear(PersianDateTime.Now.ToDateTime()).ToString();
            }
            else if (int.Parse(TxtEndYear.Text) >= 9999)
            {
            }
            else
            {
                TxtEndYear.Text = (int.Parse(TxtEndYear.Text) + 1).ToString();
            }
        }

        private void BtnDown1_Click(object sender, RoutedEventArgs e)
        {
            if (TxtEndYear.Text.Trim() == string.Empty || int.Parse(TxtEndYear.Text) < 1350)
            {
                TxtEndYear.Text = _persianCalendar.GetYear(PersianDateTime.Now.ToDateTime()).ToString();
            }
            else
            {
                TxtEndYear.Text = (int.Parse(TxtEndYear.Text) - 1).ToString();
            }
        }

        private void TxtCharge_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TxtCharge.Text.Trim() == string.Empty)
            {
                TxtCharge.Text = "0";
            }
            else
            {
                decimal number;
                if (!decimal.TryParse(TxtCharge.Text, out number)) return;
                TxtCharge.Text = $"{number:N0}";
                TxtCharge.SelectionStart = TxtCharge.Text.Length;
            }
        }

        private async void DgdChargeMonthly_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (DgdChargeMonthly.SelectedIndex == -1)
            {
                BtnNew_Click(null, null);
                return;
            }

            TxtEndYear.IsEnabled = false;
            BtnUp1.IsEnabled = false;
            BtnDown1.IsEnabled = false;
            BtnAdd.IsEnabled = false;

            var selectChargeMonthlyItem = _chargeMonthlyData[DgdChargeMonthly.SelectedIndex];
            TxtStartYear.Text =
                _persianCalendar.GetYear(
                    PersianDateTime.Parse(selectChargeMonthlyItem.ChargeMonthlyStartDate).ToDateTime()).ToString();
            TxtEndYear.Text =
                _persianCalendar.GetYear(
                    PersianDateTime.Parse(selectChargeMonthlyItem.ChargeMonthlyStartDate).ToDateTime()).ToString();
            CboStartMonth.SelectedIndex =
                _persianCalendar.GetMonth(
                    PersianDateTime.Parse(selectChargeMonthlyItem.ChargeMonthlyStartDate).ToDateTime());
            CboEndMonth.SelectedIndex =
                _persianCalendar.GetMonth(
                    PersianDateTime.Parse(selectChargeMonthlyItem.ChargeMonthlyEndDate).ToDateTime());
            TxtCharge.Text = selectChargeMonthlyItem.ChargeMonthlyCharge.ToString();
            TxtChargeMonthlyDescription.Text = selectChargeMonthlyItem.ChargeMonthlyDescription;

            if (BtnAdd.IsEnabled) return;
            try
            {
                _chargeMonthlyData = await DChargeMonthly.GetData(PersonnelId);
                _accountData = await DChMoPay.GetData(PersonnelId);
            }
            catch (Exception exception)
            {
                Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در دریافت اطلاعات شارژ ماهانه\n" + exception.Message);
                return;
            }

            if (_accountData.Count(t => t.ChMoPay_ChargeMonthly_Id == selectChargeMonthlyItem.Id) == 0)
            {
                BtnUp.IsEnabled = true;
                BtnDown.IsEnabled = true;
                TxtStartYear.IsEnabled = true;
                CboStartMonth.IsEnabled = true;
                CboStartMonth.IsEnabled = true;
                CboEndMonth.IsEnabled = true;
                TxtCharge.IsEnabled = true;
                TxtChargeMonthlyDescription.IsEnabled = true;
            }
            else
            {
                BtnUp.IsEnabled = false;
                BtnDown.IsEnabled = false;
                TxtStartYear.IsEnabled = false;
                CboStartMonth.IsEnabled = false;
                CboStartMonth.IsEnabled = false;
                CboEndMonth.IsEnabled = true;
                TxtCharge.IsEnabled = false;
                TxtChargeMonthlyDescription.IsEnabled = true;
            }
        }

        #endregion

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

        //baraye shomare gozari datagrid
        private void dataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        #endregion

        #region Method

        private bool CheckEmpty()
        {

            if (TxtStartYear.Text.Trim() == string.Empty)
            {
                Utility.Message("خطا", "لطفا سال شروع پرداخت را وارد کنید", "Stop.png");
                return false;
            }
            if (TxtEndYear.Text.Trim() == string.Empty)
            {
                Utility.Message("خطا", "لطفا سال اتمام پرداخت را وارد کنید", "Stop.png");
                return false;
            }
            if (int.Parse(TxtStartYear.Text) < 1350)
            {
                Utility.Message("خطا", "سال شروع پرداخت نباید از سال 1350 کمتر باشد", "Stop.png");
                return false;
            }
            if (int.Parse(TxtEndYear.Text) < 1350)
            {
                Utility.Message("خطا", "سال اتمام پرداخت نباید از سال 1350 کمتر باشد", "Stop.png");
                return false;
            }
            if (CboStartMonth.SelectedIndex == 0)
            {
                Utility.Message("خطا", "لطفا ماه شروع پرداخت را انتخاب کنید", "Stop.png");
                return false;
            }
            if (CboEndMonth.SelectedIndex == 0)
            {
                Utility.Message("خطا", "لطفا ماه اتمام پرداخت را انتخاب کنید", "Stop.png");
                return false;
            }

            if (TxtCharge.Text.Trim() == string.Empty || long.Parse(Regex.Replace(TxtCharge.Text, "[\\W]", "")) == 0)
            {
                Utility.Message("خطا", "لطفا مبلغ شارژ ماهانه را وارد کنید", "Stop.png");
                return false;
            }

            return true;
        }

        private bool CheckSelect()
        {
            if (DgdChargeMonthly.SelectedIndex != -1) return true;
            Utility.Message("خطا", "شارژ ماهانه مورد نظر را انتخاب کنید", "Stop.png");
            return false;
        }

        private static bool CheckMonth(int beforeMonth, int newMonth)
        {
            if (beforeMonth <= newMonth) return true;
            Utility.Message("خطا", "در هنگام ویرایش ماه پایان پرداخت نمی تواند قبل از مقدار قبلی آن باشد",
                "Stop.png");
            return false;
        }

        private bool CheckYear()
        {
            if (int.Parse(TxtStartYear.Text) > int.Parse(TxtEndYear.Text))
            {
                Utility.Message("خطا", "سال پایان پرداخت نمی تواند قبل از سال شروع پرداخت باشد", "Stop.png");
                return false;
            }
            if (TxtStartYear.Text != TxtEndYear.Text) return true;
            if (CboStartMonth.SelectedIndex <= CboEndMonth.SelectedIndex) return true;
            Utility.Message("خطا", "ماه پایان پرداخت نمی تواند قبل از ماه شروع پرداخت باشد", "Stop.png");
            return false;
        }

        private bool CheckPersonnelMembershipDate()
        {
            var year = _persianCalendar.GetYear(PersianDateTime.Parse(PersonnelMembershipDate).ToDateTime());
            var month = _persianCalendar.GetMonth(PersianDateTime.Parse(PersonnelMembershipDate).ToDateTime());

            if (year <= int.Parse(TxtStartYear.Text) &&
                (year != int.Parse(TxtStartYear.Text) || month <= CboStartMonth.SelectedIndex)) return true;
            Utility.Message("خطا", "دوره انتخابی قبل از عضویت شخص می باشد", "Stop.png");
            return false;
        }

        private bool CheckPeriod()
        {

            foreach (var t in _chargeMonthlyData)
            {
                var year = _persianCalendar.GetYear(PersianDateTime.Parse(t.ChargeMonthlyStartDate).ToDateTime());
                var startMonth = _persianCalendar.GetMonth(PersianDateTime.Parse(t.ChargeMonthlyStartDate).ToDateTime());
                var endMonth = _persianCalendar.GetMonth(PersianDateTime.Parse(t.ChargeMonthlyEndDate).ToDateTime());

                if (BtnAdd.IsEnabled == false)
                {
                    var selectChargeMonthlyItem = _chargeMonthlyData[DgdChargeMonthly.SelectedIndex];
                    if (t.Id == selectChargeMonthlyItem.Id || year != int.Parse(TxtStartYear.Text) ||
                        (CboStartMonth.SelectedIndex < startMonth || CboStartMonth.SelectedIndex > endMonth) &&
                        (CboEndMonth.SelectedIndex < startMonth || CboEndMonth.SelectedIndex > endMonth) &&
                        (startMonth < CboStartMonth.SelectedIndex || startMonth > CboEndMonth.SelectedIndex) &&
                        (endMonth < CboStartMonth.SelectedIndex || endMonth > CboEndMonth.SelectedIndex)) continue;
                    Utility.Message("خطا", "تاریخ دوره وارد شده با تاریخ دوره ی دیگر تداخل دارد", "Stop.png");
                    return false;
                }

                if (int.Parse(TxtStartYear.Text) == int.Parse(TxtEndYear.Text) &&
                    BtnAdd.IsEnabled)
                {
                    if (year != int.Parse(TxtStartYear.Text) ||
                        (CboStartMonth.SelectedIndex < startMonth || CboStartMonth.SelectedIndex > endMonth) &&
                        (CboEndMonth.SelectedIndex < startMonth || CboEndMonth.SelectedIndex > endMonth) &&
                        (startMonth < CboStartMonth.SelectedIndex || startMonth > CboEndMonth.SelectedIndex) &&
                        (endMonth < CboStartMonth.SelectedIndex || endMonth > CboEndMonth.SelectedIndex)) continue;
                    Utility.Message("خطا", "تاریخ دوره وارد شده با تاریخ دوره ی دیگر تداخل دارد", "Stop.png");
                    return false;
                }

                if (!BtnAdd.IsEnabled ||
                    (year <= int.Parse(TxtStartYear.Text) || year >= int.Parse(TxtEndYear.Text)) &&
                    (year != int.Parse(TxtStartYear.Text) ||
                     (CboStartMonth.SelectedIndex < startMonth || CboStartMonth.SelectedIndex > endMonth) &&
                     (12 < startMonth || 12 > endMonth) && (startMonth < CboStartMonth.SelectedIndex || startMonth > 12) &&
                     (endMonth < CboStartMonth.SelectedIndex || endMonth > 12)) &&
                    (year != int.Parse(TxtEndYear.Text) ||
                     (1 < startMonth || 1 > endMonth) &&
                     (CboEndMonth.SelectedIndex < startMonth || CboEndMonth.SelectedIndex > endMonth) &&
                     (startMonth < 1 || startMonth > CboEndMonth.SelectedIndex) &&
                     (endMonth < 1 || endMonth > CboEndMonth.SelectedIndex))) continue;
                Utility.Message("خطا", "تاریخ دوره وارد شده با تاریخ دوره ی دیگر تداخل دارد", "Stop.png");
                return false;
            }
            return true;
        }

        #endregion
    }
}
