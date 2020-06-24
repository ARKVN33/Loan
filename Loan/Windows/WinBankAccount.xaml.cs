using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DAL;
using DAL.Class;
using Loan.Class;

namespace Loan.Windows
{
    /// <summary>
    /// Interaction logic for WinBankAccount.xaml
    /// </summary>
    public partial class WinBankAccount
    {
        private List<tblBankAccount> _bankAccountData;

        public WinBankAccount()
        {
            InitializeComponent();
            _bankAccountData = new List<tblBankAccount>();
        }

        #region Event

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _bankAccountData = await DBankAccount.GetData();
            }
            catch (Exception exception)
            {
                Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در دریافت اطلاعات\n" + exception.Message);
                return;
            }
            DgdBankAccount.ItemsSource = _bankAccountData;

            BtnNew_Click(null, null);
        }

        private async void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckEmpty() || !CheckRepeatAdd()) return;
            try
            {
                var addBankAccount = new DBankAccount
                {
                    DBankName = TxtBankName.Text.Trim() == string.Empty ? null : TxtBankName.Text,
                    DBranchName = TxtBranchName.Text.Trim() == string.Empty ? null : TxtBranchName.Text,
                    DAccountNum = TxtAccountNum.Text.Trim() == string.Empty ? null : TxtAccountNum.Text,
                    DCardNum = TxtCardNum.Text.Trim() == string.Empty ? null : TxtCardNum.Text,
                    DInitialBalance = long.Parse(Regex.Replace(TxtInitialBalance.Text, "[\\W]", "")),
                    DDescription = TxtDescription.Text.Trim() == string.Empty ? null : TxtDescription.Text,
                };
                await Task.Run(() => addBankAccount.Add());
            }
            catch (Exception exception)
            {
                Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در ثبت اطلاعات\n" + exception.Message);
                return;
            }

            Window_Loaded(null, null);
            Utility.Message("پیام", "اطلاعات با موفقیت ثبت گردید", "Correct.png");
        }

        private async void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckSelect()) return;

            var selectItem = _bankAccountData[DgdBankAccount.SelectedIndex];

            if (!CheckEmpty() || !CheckRepeatEdit(selectItem.BankAccountNum)) return;

            try
            {
                var editBankAccount = new DBankAccount
                {
                    DId = selectItem.Id,
                    DBankName = TxtBankName.Text.Trim() == string.Empty ? null : TxtBankName.Text,
                    DBranchName = TxtBranchName.Text.Trim() == string.Empty ? null : TxtBranchName.Text,
                    DAccountNum = TxtAccountNum.Text.Trim() == string.Empty ? null : TxtAccountNum.Text,
                    DCardNum = TxtCardNum.Text.Trim() == string.Empty ? null : TxtCardNum.Text,
                    DInitialBalance = long.Parse(Regex.Replace(TxtInitialBalance.Text, "[\\W]", "")),
                    DDescription = TxtDescription.Text.Trim() == string.Empty ? null : TxtDescription.Text
                };
                await Task.Run(() => editBankAccount.Edit());
            }
            catch (Exception exception)
            {
                Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در ویرایش اطلاعات\n" + exception.Message);
                return;
            }

            Window_Loaded(null, null);
            Utility.Message("پیام", "اطلاعات با موفقیت ثبت گردید", "Correct.png");
        }

        private async void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckSelect()) return;
            Utility.MyMessageBox("هشدار", "آیا از حذف این حساب بانکی اطمینان دارید؟","Warning.png", false);

            if (!Utility.YesNo) return;
            var selectItem = _bankAccountData[DgdBankAccount.SelectedIndex];
            try
            {
                var deleteBankAccount = new DBankAccount
                {
                    DId = selectItem.Id
                };
                await Task.Run(() => deleteBankAccount.Delete());
            }
            catch (Exception exception)
            {
                Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در حذف اطلاعات\n" + exception.Message);
            }

            Utility.Message("پیام", "اطلاعات حساب بانکی مورد نظر با موفقیت حذف گردید", "Correct.png");
            Window_Loaded(null, null);
        }

        private void BtnNew_Click(object sender, RoutedEventArgs e)
        {
            TxtBankName.Text = string.Empty;
            TxtBranchName.Text = string.Empty;
            TxtAccountNum.Text = string.Empty;
            TxtCardNum.Text = string.Empty;
            TxtInitialBalance.Text = "0";
            TxtDescription.Text = string.Empty;
            BtnAdd.IsEnabled = true;
            DgdBankAccount.SelectedIndex = -1;
        }

        private void TxtInitialBalance_TextChanged(object sender, TextChangedEventArgs e)
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

        private void DgdBankAccount_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DgdBankAccount.SelectedIndex == -1) return;
            BtnAdd.IsEnabled = false;
            var selectItem = _bankAccountData[DgdBankAccount.SelectedIndex];

            TxtBankName.Text = selectItem.BankAccountBankName;
            TxtBranchName.Text = selectItem.BankAccountBranchName;
            TxtAccountNum.Text = selectItem.BankAccountNum;
            TxtCardNum.Text = selectItem.BankAccountCardNum;
            TxtInitialBalance.Text = selectItem.BankAccountInitialBalance.ToString();
            TxtDescription.Text = selectItem.BankAccountDescription;
        }

        #endregion

        #region FixedEvent

        private void DragMove(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch (Exception)
            {
                // ignored
            }
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
            if (string.IsNullOrEmpty(TxtAccountNum.Text.Trim()))
            {
                Utility.Message("خطا", "لطفا شماره حساب بانکی را وارد کنید", "Stop.png");
                return false;
            }

            return true;
        }

        private bool CheckRepeatAdd()
        {
            if (_bankAccountData.Any(t => t.BankAccountNum == TxtAccountNum.Text))
            {
                Utility.Message("اخطار", "حسابی با این شماره حساب قبلا ثبت گردیده است", "Warning.png");
                return false;
            }
            return true;
        }

        private bool CheckRepeatEdit(string accountNum)
        {
            if (!(TxtAccountNum.Text == accountNum || _bankAccountData.All(t => t.BankAccountNum != TxtAccountNum.Text)))
            {
                Utility.Message("اخطار", "حسابی با این شماره حساب قبلا ثبت گردیده است", "Warning.png");
                return false;
            }
            return true;
        }

        private bool CheckSelect()
        {
            if (DgdBankAccount.SelectedIndex == -1)
            {
                Utility.Message("اخطار", "موردی را برای ویرایش یا حذف انتخاب کنید", "Warning.png");
                return false;
            }
            return true;
        }

        #endregion
    }
}
