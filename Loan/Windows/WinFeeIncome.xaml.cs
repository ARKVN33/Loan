using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Arash;
using DAL;
using DAL.Class;
using Loan.Class;

namespace Loan.Windows
{
    /// <summary>
    /// Interaction logic for WinFeeIncome.xaml
    /// </summary>
    public partial class WinFeeIncome
    {
        private List<spSelectFeeIncomeInfo_Result> _feeIncomeData;

        public WinFeeIncome()
        {
            InitializeComponent();
            _feeIncomeData = new List<spSelectFeeIncomeInfo_Result>();
        }

        #region Event

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _feeIncomeData = await DFeeIncome.GetData();
            }
            catch (Exception exception)
            {
                Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در دریافت اطلاعات\n" + exception.Message);
                Close();
                return;
            }
            DgdFeeIncome.ItemsSource = _feeIncomeData;

            BtnNew_Click(null, null);
        }

        private void DgdFeeIncome_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DgdFeeIncome.SelectedIndex == -1) return;
            BtnAdd.IsEnabled = false;
            var selectItem = _feeIncomeData[DgdFeeIncome.SelectedIndex];

            CboFeeIncomeType.SelectedIndex = selectItem.FeeIncome_FeeIncomeType_Id ?? 0;
            TxtDate.Text = selectItem.FeeIncomeDate;
            TxtAmount.Text = selectItem.FeeIncomeAmount.ToString();
            CboPayType.SelectedIndex = selectItem.FeeIncome_PaymentType_Id ?? 0;
            TxtReceiptNumber.Text = selectItem.FeeIncomeReceiptNumber;
            TxtDescription.Text = selectItem.FeeIncomeDescription;
        }

        private void TxtAmount_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TxtAmount.Text.Trim() == string.Empty)
            {
                TxtAmount.Text = "0";
            }
            else
            {
                decimal number;
                if (!decimal.TryParse(TxtAmount.Text, out number)) return;
                TxtAmount.Text = $"{number:N0}";
                TxtAmount.SelectionStart = TxtAmount.Text.Length;
            }
        }

        private async void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckEmpty()) return;

            try
            {
                var addFeeIncome = new DFeeIncome
                {
                    DFeeIncomeTypeId = (byte) CboFeeIncomeType.SelectedIndex,
                    DPaymentTypeId = (byte) CboPayType.SelectedIndex,
                    DDate = Utility.CurrectDate(TxtDate.Text),
                    DAmount = long.Parse(Regex.Replace(TxtAmount.Text, "[\\W]", "")),
                    DReceiptNumber = TxtReceiptNumber.Text.Trim() == string.Empty ? null : TxtReceiptNumber.Text,
                    DDescription = TxtDescription.Text.Trim() == string.Empty ? null : TxtDescription.Text
                };
                await Task.Run(() => addFeeIncome.Add());
            }
            catch (Exception exception)
            {
                Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در ثبت اطلاعات\n" + exception.Message);
                return;
            }
            Window_Loaded(null, null);
            Utility.Message("پیام", "اطلاعات با موفقیت ویرایش گردید", "Correct.png");
        }

        private void BtnNew_Click(object sender, RoutedEventArgs e)
        {
            CboFeeIncomeType.SelectedIndex = 0;
            TxtDate.Text = PersianDate.Today.ToString();
            TxtAmount.Text = "0";
            CboPayType.SelectedIndex = 1;
            TxtReceiptNumber.Text = string.Empty;
            TxtDescription.Text = string.Empty;
            BtnAdd.IsEnabled = true;

            DgdFeeIncome.SelectedIndex = -1;
        }

        private async void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckSelectEdit() || !CheckEmpty()) return;
            var selectItem = _feeIncomeData[DgdFeeIncome.SelectedIndex];
            try
            {
                var editFeeIncome = new DFeeIncome
                {
                    DId = selectItem.Id,
                    DFeeIncomeTypeId = (byte) CboFeeIncomeType.SelectedIndex,
                    DPaymentTypeId = (byte) CboPayType.SelectedIndex,
                    DDate = Utility.CurrectDate(TxtDate.Text),
                    DAmount = long.Parse(Regex.Replace(TxtAmount.Text, "[\\W]", "")),
                    DReceiptNumber = TxtReceiptNumber.Text.Trim() == string.Empty ? null : TxtReceiptNumber.Text,
                    DDescription = TxtDescription.Text.Trim() == string.Empty ? null : TxtDescription.Text
                };
                await Task.Run(() => editFeeIncome.Edit());
            }
            catch (Exception exception)
            {
                Utility.MyMessageBox("خطا در بانک اطلاعاتی",
                    "خطا در ثبت اطلاعات موجودی اولیه حساب\n" + exception.Message);
                return;
            }
            Window_Loaded(null, null);
            Utility.Message("پیام", "اطلاعات با موفقیت ویرایش گردید", "Correct.png");
        }

        private async void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckSelectDelete()) return;
            var selectItem = _feeIncomeData[DgdFeeIncome.SelectedIndex];
            Utility.MyMessageBox("هشدار", "آیا از حذف اطمینان دارید؟ ", "Warning.png", false);
            if (!Utility.YesNo) return;
            try
            {
                var deleteFeeIncome = new DFeeIncome
                {
                    DId = selectItem.Id
                };
                await Task.Run(() => deleteFeeIncome.Delete());
            }
            catch (Exception exception)
            {
                Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در حذف اطلاعات\n" + exception.Message);
                return;
            }
            Window_Loaded(null, null);
            Utility.Message("پیام", "اطلاعات با موفقیت حذف گردید", "Correct.png");
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

        //baraye shomare gozari datagrid
        private void dataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        #endregion

        #region Method

        private bool CheckSelectDelete()
        {
            if (DgdFeeIncome.SelectedIndex == -1)
            {
                Utility.Message("اخطار", "موردی را برای حذف انتخاب کنید", "Warning.png");
                return false;
            }
            return true;
        }

        private bool CheckSelectEdit()
        {
            if (DgdFeeIncome.SelectedIndex == -1)
            {
                Utility.Message("اخطار", "موردی را برای ویرایش انتخاب کنید", "Warning.png");
                return false;
            }
            return true;
        }

        private bool CheckEmpty()
        {
            if (CboFeeIncomeType.SelectedIndex == 0)
            {
                Utility.Message("خطا", "لطفا نوع هزینه یا درآمد را مشخص کنید", "Stop.png");
                return false;
            }

            if (string.IsNullOrEmpty(TxtDate.Text))
            {
                Utility.Message("خطا", "لطفا تاریخ وارد کنید", "Stop.png");
                return false;
            }

            if (!Utility.CheckDate(TxtDate.Text))
            {
                Utility.Message("خطا", "لطفا یک تاریخ صحیح انتخاب کنید", "Stop.png");
                return false;
            }

            if (string.IsNullOrEmpty(TxtAmount.Text.Trim()) || long.Parse(Regex.Replace(TxtAmount.Text, "[\\W]", "")) == 0)
            {
                Utility.Message("خطا", "لطفا مبلغ را وارد کنید", "Stop.png");
                return false;
            }

            if (string.IsNullOrEmpty(CboPayType.Text.Trim()))
            {
                Utility.Message("خطا", "لطفا نوع پرداخت را مشخص کنید", "Stop.png");
                return false;
            }
            return true;
        }

        #endregion
    }
}
