using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Arash;
using DAL;
using DAL.Class;
using Loan.Class;
// ReSharper disable PossibleInvalidOperationException

namespace Loan.Windows
{
    /// <summary>
    /// Interaction logic for WinLoan.xaml
    /// </summary>
    public partial class WinLoan
    {
        private List<spSelectLoanInfo_Result> _loanInfoData;
        private List<spSelectAccountInfo_Result> _accountData;
        private List<tblInstallment> _installmentData;
        private List<CreateInstallment> _items;
        private List<tblInstitution> _institutionsData;

        private DLoan _loan;
        private DAccount _account;
        private DWage _wage;
        private DInstallment _installment;
        private DGuarantor _guarantor;
        private DIntroducer _introducer;

        private int _counter;
        private long _canReceive;
        private bool _canDeleteMemFee;
        private int _index;

        public WinLoan()
        {
            InitializeComponent();
            _loanInfoData = new List<spSelectLoanInfo_Result>();
            _accountData = new List<spSelectAccountInfo_Result>();
            _institutionsData = new List<tblInstitution>();
        }

        #region Properties

        public int PersonnelId { get; set; }

        public int? PersonnelInfoId { get; set; }

        public string PersonnelMembershipDate { get; set; }

        public int InfoId { get; set; }

        public int InfoId2 { get; set; }

        public int InfoId3 { get; set; }

        public int InfoId4 { get; set; }

        public long CanReceive { get; set; }

        public long CanReceive2 { get; set; }

        public long CanReceive3 { get; set; }

        #endregion

        #region Event

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _loanInfoData = await DLoan.GetLoanInfoData(PersonnelId);
            }
            catch (Exception exception)
            {
                Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در دریافت اطلاعات\n" + exception.Message);
                Close();
                return;
            }
            DgdLoan.ItemsSource = _loanInfoData;
            try
            {
                CboGuaType.ItemsSource =
                    CboGuaType2.ItemsSource = CboGuaType3.ItemsSource = await DLoan.GetGuaranteeType();
            }
            catch (Exception exception)
            {
                Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در دریافت اطلاعات\n" + exception.Message);
                Close();
                return;
            }
            try
            {
                _accountData = await DAccount.GetData(PersonnelId);
                _institutionsData = await DLoan.GetInstitution();
                CboRecom.ItemsSource = _institutionsData.Select(x => x.Institution);
            }
            catch (Exception exception)
            {
                Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در دریافت اطلاعات\n" + exception.Message);
            }
            BtnNew_Click(null, null);
        }

        private async void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckEmpty() || !CheckPersonnelMembershipDate() || !CheckGua() || !CheckDate() || !CheckIns()) return;

            var totalPay = long.Parse(Regex.Replace(TxtTotalPay.Text, "[\\W]", ""));
            var loanDate = Utility.CurrectDate(TxtLoanDate.Text);

            #region AddAccount

            int accountId;
            try
            {
                _account = new DAccount
                {
                    DPersonnelId = PersonnelId,
                    DPaymentTypeId = 1,
                    DTransactionTypeId = 5,
                    DAmount = totalPay,
                    DReceiptNumber = null,
                    DCurrentBalance = _accountData.Count == 0
                        ? totalPay
                        // ReSharper disable once PossibleInvalidOperationException
                        : (long) _accountData[_accountData.Count - 1].AccountCurrentBalance + totalPay,
                    DPaymentDate = loanDate,
                    DDescription = null
                };
                accountId = Convert.ToInt32(await _account.Add());
            }
            catch (Exception exception)
            {
                Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در ثبت اطلاعات حساب پس انداز\n" + exception.Message);
                return;
            }

            #endregion

            #region AddLoan

            int loanId;
            try
            {
                _loan = new DLoan
                {
                    DPersonnelId = PersonnelId,
                    DLoanTypeId = (byte) CboLoanType.SelectedIndex,
                    DAccountId = accountId,
                    DAmount = long.Parse(Regex.Replace(TxtLoanAmount.Text, "[\\W]", "")),
                    DDate = loanDate,
                    DInstallmentNum = Convert.ToByte(TxtInstallmentNum.Text),
                    DInstallmentFirstPayDate = Utility.CurrectDate(TxtInstallmentFirstPayDate.Text),
                    DInstallmentInterspace = Convert.ToByte(TxtInstallmentInterspace.Text),
                    DPayOff = false,
                    DDescription = TxtLoanDescription.Text.Trim() == string.Empty ? null : TxtLoanDescription.Text
                };
                loanId = Convert.ToInt32(await _loan.Add());
            }
            catch (Exception exception)
            {
                Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در ثبت اطلاعات وام\n" + exception.Message);
                return;
            }

            #endregion

            #region AddWage

            try
            {
                _wage = new DWage
                {
                    DLoanId = loanId,
                    DWageTypeId = (byte) CboWageType.SelectedIndex,
                    DWageCalculationTypeId =
                        CboWageCalculationType.SelectedIndex == 0 ? null : (byte?) CboWageCalculationType.SelectedIndex,
                    DPercent = CboWageCalculationType.SelectedIndex == 2 ? TxtWagePercent.Text : null,
                    DAmount = long.Parse(Regex.Replace(TxtWageAmount.Text, "[\\W]", "")),
                    DDescription = TxtWageDescription.Text.Trim() == string.Empty ? null : TxtWageDescription.Text
                };
                await Task.Run(() => _wage.Add());
            }
            catch (Exception exception)
            {
                Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در ثبت کارمزد\n" + exception.Message);
                return;
            }

            #endregion

            #region AddInstallment

            var installmentNum = int.Parse(TxtInstallmentNum.Text);

            for (var i = 0; i < installmentNum; i++)
            {
                try
                {
                    _installment = new DInstallment
                    {
                        DLoanId = loanId,
                        DPaymentTypeId = null,
                        DDueAmount = _items[i].DueAmount,
                        DAmount = null,
                        DReceiptNumber = null,
                        DTotalPaid = null,
                        DRemaining = null,
                        DPaymentDate = null,
                        DDueDate = _items[i].DueDate,
                        DDelayDay = null,
                        DDescription = null
                    };
                    await Task.Run(() => _installment.Add());
                }
                catch (Exception exception)
                {
                    Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در ثبت اقساط\n" + exception.Message);
                    return;
                }
            }

            #endregion

            #region AddGuarantor

            if (ChkGuarantor.IsChecked == true)
            {
                try
                {
                    _guarantor = new DGuarantor
                    {
                        DLoanId = loanId,
                        DInfoId = InfoId,
                        DGuaranteeTypeId = (byte) (CboGuaType.SelectedIndex + 1),
                        DBlockTypeId = (byte) (CboGuaBlockType.SelectedIndex + 1),
                        DReceiptNumber =
                            TxtGuaReceiptNumber.Text.Trim() == string.Empty && CboGuaType.SelectedIndex == 0
                                ? null
                                : TxtGuaReceiptNumber.Text,
                        DAmount = CboGuaType.SelectedIndex != 0
                            ? (long?) long.Parse(Regex.Replace(TxtGuaAmount.Text, "[\\W]", ""))
                            : null,
                        DBlockAmount =
                            CboGuaBlockType.SelectedIndex == 2
                                ? (long?) long.Parse(Regex.Replace(TxtGuaBlockAmount.Text, "[\\W]", ""))
                                : null,
                        DBlock = CboGuaBlockType.SelectedIndex != 0,
                        DDescription = TxtGuaDescription.Text.Trim() == string.Empty ? null : TxtGuaDescription.Text
                    };
                    await Task.Run(() => _guarantor.Add());
                }
                catch (Exception exception)
                {
                    Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در ثبت ضامن\n" + exception.Message);
                    return;
                }
            }

            if (ChkGuarantor2.IsChecked == true)
            {
                try
                {
                    _guarantor = new DGuarantor
                    {
                        DLoanId = loanId,
                        DInfoId = InfoId2,
                        DGuaranteeTypeId = (byte) (CboGuaType2.SelectedIndex + 1),
                        DBlockTypeId = (byte) (CboGuaBlockType2.SelectedIndex + 1),
                        DReceiptNumber =
                            TxtGuaReceiptNumber2.Text.Trim() == string.Empty && CboGuaType2.SelectedIndex == 0
                                ? null
                                : TxtGuaReceiptNumber2.Text,
                        DAmount = CboGuaType2.SelectedIndex != 0
                            ? (long?) long.Parse(Regex.Replace(TxtGuaAmount2.Text, "[\\W]", ""))
                            : null,
                        DBlockAmount =
                            CboGuaBlockType2.SelectedIndex == 2
                                ? (long?) long.Parse(Regex.Replace(TxtGuaAmount2.Text, "[\\W]", ""))
                                : null,
                        DBlock = CboGuaBlockType2.SelectedIndex != 0,
                        DDescription = TxtGuaDescription2.Text.Trim() == string.Empty ? null : TxtGuaDescription2.Text
                    };
                    await Task.Run(() => _guarantor.Add());
                }
                catch (Exception exception)
                {
                    Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در ثبت ضامن\n" + exception.Message);
                    return;
                }
            }
            if (ChkGuarantor3.IsChecked == true)
            {
                try
                {
                    _guarantor = new DGuarantor
                    {
                        DLoanId = loanId,
                        DInfoId = InfoId3,
                        DGuaranteeTypeId = (byte) (CboGuaType3.SelectedIndex + 1),
                        DBlockTypeId = (byte) (CboGuaBlockType3.SelectedIndex + 1),
                        DReceiptNumber =
                            TxtGuaReceiptNumber3.Text.Trim() == string.Empty && CboGuaType3.SelectedIndex == 0
                                ? null
                                : TxtGuaReceiptNumber3.Text,
                        DAmount = CboGuaType3.SelectedIndex != 0
                            ? (long?) long.Parse(Regex.Replace(TxtGuaAmount3.Text, "[\\W]", ""))
                            : null,
                        DBlockAmount =
                            CboGuaBlockType3.SelectedIndex == 2
                                ? (long?) long.Parse(Regex.Replace(TxtGuaAmount3.Text, "[\\W]", ""))
                                : null,
                        DBlock = CboGuaBlockType3.SelectedIndex != 0,
                        DDescription = TxtGuaDescription3.Text.Trim() == string.Empty ? null : TxtGuaDescription3.Text
                    };
                    await Task.Run(() => _guarantor.Add());
                }
                catch (Exception exception)
                {
                    Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در ثبت ضامن\n" + exception.Message);
                    return;
                }
            }

            #endregion

            #region AddIntroducer

            if (ChkIntroducer.IsChecked == true)
            {
                var institutionId = 0;
                if (RdoLegal.IsChecked == true)
                {
                    if (_institutionsData.FirstOrDefault(x => x.Institution == CboRecom.Text) == null &&
                        !string.IsNullOrEmpty(CboRecom.Text))
                    {
                        var institution = new DInstitution
                        {
                            DInstName = CboRecom.Text
                        };
                        institutionId = Convert.ToInt32(await institution.Add());
                    }
                    else
                    {
                        var firstOrDefault = _institutionsData.FirstOrDefault(x => x.Institution == CboRecom.Text);
                        if (firstOrDefault != null)
                        {
                            institutionId = Convert.ToInt32(firstOrDefault.id);
                        }
                    }
                }
                try
                {
                    _introducer = new DIntroducer
                    {
                        DLoanId = loanId,
                        DIntroducerTypeId = (byte) (RdoReal.IsChecked == true ? 1 : 2),
                        DInfoId = RdoReal.IsChecked == true ? (int?) InfoId4 : null,
                        DInstitutionId = RdoReal.IsChecked != true ? (short?) institutionId : null,
                        DDescription = TxtRecomDescription.Text.Trim() == string.Empty ? null : TxtRecomDescription.Text
                    };
                    await Task.Run(() => _introducer.Add());
                }
                catch (Exception exception)
                {
                    Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در ثبت معرف\n" + exception.Message);
                }
            }

            #endregion

            #region GetAccountData

            try
            {
                _accountData = await DAccount.GetData(PersonnelId);
            }
            catch (Exception exception)
            {
                Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در دریافت اطلاعات حساب پس انداز\n" + exception.Message);
                return;
            }

            #endregion

            if (!_accountData.Exists(item => item.Account_TransactionType_Id == 11))
            {
                Utility.MyMessageBox("پیام",
                    "این شخص حق عضویت خود را پرداخت نکرده است. \nآیا مایل به پرداخت حق عضویت از مبلغ این وام هستید؟",
                    "Warning.png", false);

                if (Utility.YesNo)
                {
                    var winMembershipFee = new WinMembershipFee();
                    winMembershipFee.ShowDialog();
                    var membershipFee = winMembershipFee.MembershipFee;
                    if (membershipFee != null)
                    {
                        if (_accountData[_accountData.Count - 1].AccountCurrentBalance >= long.Parse(membershipFee))
                        {
                            try
                            {
                                var addAccount = new DAccount
                                {
                                    DPersonnelId = PersonnelId,
                                    DPaymentTypeId = 1,
                                    DTransactionTypeId = 6,
                                    DAmount = long.Parse(membershipFee),
                                    DReceiptNumber = null,
                                    DCurrentBalance =
                                        (long) _accountData[_accountData.Count - 1].AccountCurrentBalance -
                                        long.Parse(membershipFee),
                                    DPaymentDate = loanDate,
                                    DDescription = "برداشت از وام برای حق عضویت"
                                };
                                await addAccount.Add();

                            }
                            catch (Exception exception)
                            {
                                Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در ثبت اطلاعات برداشت\n" + exception.Message);
                            }

                            #region GetAccountData

                            try
                            {
                                _accountData = await DAccount.GetData(PersonnelId);
                            }
                            catch (Exception exception)
                            {
                                Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در دریافت اطلاعات حساب پس انداز\n" + exception.Message);
                                return;
                            }

                            #endregion

                            try
                            {
                                var addAccount = new DAccount
                                {
                                    DPersonnelId = PersonnelId,
                                    DPaymentTypeId = 1,
                                    DTransactionTypeId = 11,
                                    DAmount = long.Parse(membershipFee),
                                    DReceiptNumber = null,
                                    DCurrentBalance =
                                        (long) _accountData[_accountData.Count - 1].AccountCurrentBalance +
                                        long.Parse(membershipFee),

                                    DPaymentDate = loanDate,
                                    DDescription = null
                                };
                                await addAccount.Add();

                                Utility.Message("پیام", "اطلاعات واریز حق عضویت با موفقیت ثبت گردید", "Correct.png");
                            }
                            catch (Exception exception)
                            {
                                Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در ثبت اطلاعات واریز حق عضویت\n" + exception.Message);
                            }
                        }
                        else
                        {
                            Utility.Message("پیام", "مبلغ حق عضویت وارد شده بیشتر از موجودی حساب میباشد", "Stop.png");
                        }
                    }
                }
            }
            Utility.MyMessageBox("پیام", "آیا هم اکنون مایل به پرداخت این وام هستید؟","Warning.png",false);
            if (Utility.YesNo)
            {
                var winAccounOperation = new WinAccountOperation
                {
                    LblPerId = {Content = LblPerId.Content},
                    LblPerFirstName = {Content = LblPerFirstName.Content},
                    LblPerLastName = {Content = LblPerLastName.Content},
                    LblPerNationalCode = {Content = LblPerNationalCode.Content},

                    InfoId = PersonnelInfoId,
                    PersonnelMembershipDate = PersonnelMembershipDate,
                    PersonnelId = PersonnelId
                };
                winAccounOperation.ShowDialog();
            }
            Window_Loaded(null, null);
            Utility.Message("پیام", "اطلاعات با موفقیت ثبت گردید", "Correct.png");
        }

        private async void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckSelect()) return;
            
            _canReceive = (long)await DAccount.CanReceive(PersonnelId, PersonnelInfoId);
            var selectloanItem = _loanInfoData[DgdLoan.SelectedIndex];
            _installmentData = await DInstallment.GetData(selectloanItem.Id);
            var count = _installmentData.Count(x => x.InstallmentAmount != null);

            if (!CheckCanDelete(count)) return;
            Utility.MyMessageBox("هشدار",
                "آیا از حذف این وام اطمینان دارید؟\nدر صورتی که حق عضویت را از این وام پرداخت کرده باشید، حق عضویت نیز حذف خواهد شد","Warning.png",false);
            if (!Utility.YesNo) return;

            #region deleteMembershipFee
            
            if (_canDeleteMemFee)
            {
                try
                {
                    var deleteAccount = new DAccount
                    {
                        DId = _accountData[_index + 1].Id,
                        DPersonnelId = (int) selectloanItem.Loan_Personnel_Id
                    };
                    await Task.Run(() => deleteAccount.Delete());
                }
                catch (Exception exception)
                {
                    Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در حذف اطلاعات" + exception.Message);
                    return;
                }
                try
                {
                    var deleteAccount = new DAccount
                    {
                        DId = _accountData[_index + 2].Id,
                        DPersonnelId = (int) selectloanItem.Loan_Personnel_Id
                    };
                    await Task.Run(() => deleteAccount.Delete());
                }
                catch (Exception exception)
                {
                    Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در حذف اطلاعات" + exception.Message);
                    return;
                }
            }

            #endregion

            #region deleteAccount
            try
            {
                var deleteAccount = new DAccount
                {
                    DId = (int) selectloanItem.Loan_Account_Id,
                    DPersonnelId = (int) selectloanItem.Loan_Personnel_Id
                };
                await Task.Run(() => deleteAccount.Delete());
            }
            catch (Exception exception)
            {
                Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در حذف اطلاعات" + exception.Message);
                return;
            }
            #endregion
            
            #region deleteWage

            try
            {
                var deleteWage = new DWage
                {
                    DLoanId= selectloanItem.Id
                };
                await Task.Run(() => deleteWage.Delete());
            }
            catch (Exception exception)
            {
                Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در حذف اطلاعات" + exception.Message);
                return;
            }

            #endregion

            #region deleteInstallment

            try
            {
                var deleteInstallment = new DInstallment
                {
                    DLoanId = selectloanItem.Id
                };
                await Task.Run(() => deleteInstallment.Delete());
            }
            catch (Exception exception)
            {
                Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در حذف اطلاعات" + exception.Message);
                return;
            }

            #endregion

            #region deleteGuarantor

            try
            {
                var deleteGuarantor = new DGuarantor
                {
                    DLoanId = selectloanItem.Id
                };
                await Task.Run(() => deleteGuarantor.Delete());
            }
            catch (Exception exception)
            {
                Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در حذف اطلاعات" + exception.Message);
                return;
            }

            #endregion

            #region deleteIntroducer

            try
            {
                var deleteIntroducer = new DIntroducer
                {
                    DLoanId = selectloanItem.Id
                };
                await Task.Run(() => deleteIntroducer.Delete());
            }
            catch (Exception exception)
            {
                Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در حذف اطلاعات" + exception.Message);
                return;
            }

            #endregion

            #region deleteLoan

            try
            {
                _loan = new DLoan
                {
                    DId = selectloanItem.Id
                };
                _loan.Delete();
            }
            catch (Exception exception)
            {
                Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در حذف اطلاعات" + exception.Message);
                return;
            }

            #endregion

            Window_Loaded(null, null);
            Utility.Message("پیام", "اطلاعات با موفقیت حذف گردید", "Correct.png");
        }

        private void BtnNew_Click(object sender, RoutedEventArgs e)
        {
            TxtInstallmentAmount.Text = "0";
            TxtTotalDebt.Text = "0";
            TxtTotalPay.Text = "0";
            TxtLoanDate.Text = PersianDate.Today.ToString();
            TxtLoanAmount.Text = "0";
            
            TxtInstallmentNum.Text = "1";
            TxtInstallmentFirstPayDate.Text = string.Empty;
            TxtInstallmentInterspace.Text = "1";
            TxtLoanDescription.Text = string.Empty;
            TxtWagePercent.Text = "2";
            TxtWageAmount.Text = "0";
            TxtWageDescription.Text = string.Empty;
            TxtGuaBlockAmount.Text = "0";
            TxtGuaBlockAmount2.Text = "0";
            TxtGuaBlockAmount3.Text = "0";
            TxtGuaAmount.Text = "0";
            TxtGuaAmount2.Text = "0";
            TxtGuaAmount3.Text = "0";
            TxtGuaReceiptNumber.Text = string.Empty;
            TxtGuaReceiptNumber2.Text = string.Empty;
            TxtGuaReceiptNumber3.Text = string.Empty;
            TxtGuaDescription.Text = string.Empty;
            TxtGuaDescription2.Text = string.Empty;
            TxtGuaDescription3.Text = string.Empty;
            TxtRecomDescription.Text = string.Empty;
            TxtInsRound.Text = "0";

            LblGuaPerFirstName.Content = string.Empty;
            LblGuaPerFirstName2.Content = string.Empty;
            LblGuaPerFirstName3.Content = string.Empty;
            LblRecomPerFirstName.Content = string.Empty;
            LblGuaPerLastName.Content = string.Empty;
            LblGuaPerLastName2.Content = string.Empty;
            LblGuaPerLastName3.Content = string.Empty;
            LblRecomPerLastName.Content = string.Empty;
            LblGuaPerNationalCode.Content = string.Empty;
            LblGuaPerNationalCode2.Content = string.Empty;
            LblGuaPerNationalCode3.Content = string.Empty;
            LblRecomPerNationalCode.Content = string.Empty;

            CboLoanType.SelectedIndex = 1;
            CboWageType.SelectedIndex = 0;
            CboWageCalculationType.SelectedIndex = 2;
            CboGuaBlockType.SelectedIndex = 0;
            CboGuaBlockType2.SelectedIndex = 0;
            CboGuaBlockType3.SelectedIndex = 0;
            CboGuaType.SelectedIndex = 0;
            CboGuaType2.SelectedIndex = 0;
            CboGuaType3.SelectedIndex = 0;
            CboRecom.SelectedIndex = -1;

            ChkGuarantor.IsChecked = false;
            ChkGuarantor2.IsChecked = false;
            ChkGuarantor3.IsChecked = false;
            ChkIntroducer.IsChecked = false;

            ChkGuarantor2.IsEnabled = false;
            ChkGuarantor3.IsEnabled = false;

            TabGuarantor.IsEnabled = false;
            TabGuarantor2.IsEnabled = false;
            TabGuarantor3.IsEnabled = false;
            TabIntroducer.IsEnabled = false;
            TabLoan.IsSelected = true;

            RdoReal.IsChecked = true;

            InfoId = 0;
            InfoId2 = 0;
            InfoId3 = 0;
            InfoId4 = 0;

            CanReceive = 0;
            CanReceive2 = 0;
            CanReceive3 = 0;

            DgdInstallment.SelectedIndex = -1;
            DgdLoan.SelectedIndex = -1;

            DgdInstallment.ItemsSource = null;

            BtnAdd.IsEnabled = true;
            _items = null;
            _counter = 1;

        }

        private void DgdLoan_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DgdLoan.SelectedIndex == -1) return;
            BtnAdd.IsEnabled = false;
        }

        private void BtnCreatIns_Click(object sender, RoutedEventArgs e)
        {

            if (!CheckEmpty()) return;

            dataGrid_DataContextChanged(null, new DependencyPropertyChangedEventArgs());

            var installmentNum = int.Parse(TxtInstallmentNum.Text);
            var installmentInterspace = int.Parse(TxtInstallmentInterspace.Text);
            var totalMonth = 0;
            var persianDateTime =
                new PersianDateTime(
                    PersianDateTime.Parse(Utility.CurrectDate(TxtInstallmentFirstPayDate.Text)).ToDateTime());
            var insRound = int.Parse(TxtInsRound.Text);
            var loanAmount = long.Parse(Regex.Replace(TxtLoanAmount.Text, "[\\W]", ""));
            var wageAmount = long.Parse(Regex.Replace(TxtWageAmount.Text, "[\\W]", ""));
            long installmentAmount;
            _items = new List<CreateInstallment>();
            DgdInstallment.ItemsSource = null;

            switch (CboWageType.SelectedIndex)
            {
                case 1:
                case 2:
                    installmentAmount = loanAmount / installmentNum;

                    for (var i = 0; i < insRound; i++)
                        installmentAmount = installmentAmount / 10;

                    if (Math.Abs(installmentAmount) <= 0) return;

                    for (var i = 0; i < insRound; i++)
                        installmentAmount = installmentAmount * 10;

                    for (var i = 0; i < installmentNum - 1; i++)
                    {
                        _items.Add(new CreateInstallment(installmentAmount,
                            persianDateTime.AddMonths(totalMonth).ToString(PersianDateTimeFormat.Date)));
                        totalMonth += installmentInterspace;
                        loanAmount = loanAmount - installmentAmount;
                    }
                    _items.Add(new CreateInstallment(loanAmount,
                        persianDateTime.AddMonths(totalMonth).ToString(PersianDateTimeFormat.Date)));

                    break;
                case 3:

                    installmentAmount = loanAmount / installmentNum;
                    for (var i = 0; i < insRound; i++)
                        installmentAmount = installmentAmount / 10;

                    if (Math.Abs(installmentAmount) <= 0) return;

                    for (var i = 0; i < insRound; i++)
                        installmentAmount = installmentAmount * 10;

                    for (var i = 0; i < installmentNum - 1; i++)
                    {
                        if (i != 0)
                        {
                            _items.Add(new CreateInstallment(installmentAmount,
                                persianDateTime.AddMonths(totalMonth).ToString(PersianDateTimeFormat.Date)));
                            totalMonth += installmentInterspace;
                            loanAmount = loanAmount - installmentAmount;
                        }
                        else
                        {
                            _items.Add(
                                new CreateInstallment(installmentAmount + wageAmount,
                                    persianDateTime.AddMonths(totalMonth).ToString(PersianDateTimeFormat.Date)));
                            totalMonth += installmentInterspace;
                            loanAmount = loanAmount - installmentAmount;
                        }
                    }

                    if (installmentNum > 1)
                    {
                        _items.Add(new CreateInstallment(loanAmount,
                            persianDateTime.AddMonths(totalMonth).ToString(PersianDateTimeFormat.Date)));
                    }
                    else
                    {
                        _items.Add(
                            new CreateInstallment(installmentAmount + wageAmount,
                                persianDateTime.AddMonths(totalMonth).ToString(PersianDateTimeFormat.Date)));
                    }
                    break;
                case 4:
                    loanAmount = loanAmount + wageAmount;
                    installmentAmount = loanAmount / installmentNum;

                    for (var i = 0; i < insRound; i++)
                        installmentAmount = installmentAmount / 10;

                    if (Math.Abs(installmentAmount) <= 0) return;

                    for (var i = 0; i < insRound; i++)
                        installmentAmount = installmentAmount * 10;

                    for (var i = 0; i < installmentNum - 1; i++)
                    {
                        _items.Add(new CreateInstallment(installmentAmount,
                            persianDateTime.AddMonths(totalMonth).ToString(PersianDateTimeFormat.Date)));
                        totalMonth += installmentInterspace;
                        loanAmount = loanAmount - installmentAmount;
                    }
                    _items.Add(new CreateInstallment(loanAmount,
                        persianDateTime.AddMonths(totalMonth).ToString(PersianDateTimeFormat.Date)));

                    break;
                case 5:
                    installmentAmount = loanAmount / installmentNum;

                    for (var i = 0; i < insRound; i++)
                        installmentAmount = installmentAmount / 10;

                    if (Math.Abs(installmentAmount) <= 0) return;

                    for (var i = 0; i < insRound; i++)
                        installmentAmount = installmentAmount * 10;

                    for (var i = 0; i < installmentNum - 1; i++)
                    {
                        _items.Add(new CreateInstallment(installmentAmount,
                            persianDateTime.AddMonths(totalMonth).ToString(PersianDateTimeFormat.Date)));
                        totalMonth += installmentInterspace;
                        loanAmount = loanAmount - installmentAmount;
                    }
                    _items.Add(new CreateInstallment(loanAmount + wageAmount,
                        persianDateTime.AddMonths(totalMonth).ToString(PersianDateTimeFormat.Date)));
                    break;
            }
            
            DgdInstallment.ItemsSource = _items;
        }

        private void dataGrid_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var scrollViewer = GetVisualChild<ScrollViewer>(DgdInstallment);
            scrollViewer?.ScrollToTop();
        }

        private static T GetVisualChild<T>(DependencyObject parent) where T : Visual
        {
            var child = default(T);

            var numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (var i = 0; i < numVisuals; i++)
            {
                var v = (Visual)VisualTreeHelper.GetChild(parent, i);
                child = v as T ?? GetVisualChild<T>(v);
                if (child != null)
                    break;
            }
            return child;
        }

        private void BtnUp_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TxtInstallmentNum.Text.Trim()) || int.Parse(TxtInstallmentNum.Text) < 1)
            {
                TxtInstallmentNum.Text = "1";
            }
            else if (int.Parse(TxtInstallmentNum.Text) >= 60)
            {
                TxtInstallmentNum.Text = "1";
            }
            else
            {
                TxtInstallmentNum.Text = (int.Parse(TxtInstallmentNum.Text) + 1).ToString();
            }
        }

        private void BtnDown_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TxtInstallmentNum.Text.Trim()) || int.Parse(TxtInstallmentNum.Text) - 1 < 1)
            {
                TxtInstallmentNum.Text = "60";
            }
            else if (int.Parse(TxtInstallmentNum.Text) > 60)
            {
                TxtInstallmentNum.Text = "1";
            }
            else
            {
                TxtInstallmentNum.Text = (int.Parse(TxtInstallmentNum.Text) - 1).ToString();
            }
        }

        private void BtnUp1_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TxtInstallmentInterspace.Text.Trim()) ||
                int.Parse(TxtInstallmentInterspace.Text) < 1)
            {
                TxtInstallmentInterspace.Text = "1";
            }
            else if (int.Parse(TxtInstallmentInterspace.Text) >= 12)
            {
                TxtInstallmentInterspace.Text = "1";
            }
            else
            {
                TxtInstallmentInterspace.Text = (int.Parse(TxtInstallmentInterspace.Text) + 1).ToString();
            }
        }

        private void BtnDown1_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TxtInstallmentInterspace.Text.Trim()) ||
                int.Parse(TxtInstallmentInterspace.Text) - 1 < 1)
            {
                TxtInstallmentInterspace.Text = "12";
            }
            else if (int.Parse(TxtInstallmentInterspace.Text) > 12)
            {
                TxtInstallmentInterspace.Text = "1";
            }
            else
            {
                TxtInstallmentInterspace.Text = (int.Parse(TxtInstallmentInterspace.Text) - 1).ToString();
            }
        }

        private void BtnUp2_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TxtWagePercent.Text.Trim()) || double.Parse(TxtWagePercent.Text) < 0.1 || double.Parse(TxtWagePercent.Text) >= 100)
            {
                const double wagePercent = 0.1;
                TxtWagePercent.Text = wagePercent.ToString(ContentStringFormat);
            }
            else
            {
                TxtWagePercent.Text = (double.Parse(TxtWagePercent.Text) + 0.1).ToString(ContentStringFormat);
            }
        }

        private void BtnDown2_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TxtWagePercent.Text.Trim()) || double.Parse(TxtWagePercent.Text) - 0.1 < 0.1)
            {
                const double wagePercent = 100;
                TxtWagePercent.Text = wagePercent.ToString(ContentStringFormat);
            }
            else if (double.Parse(TxtWagePercent.Text) > 100)
            {
                const double wagePercent = 0.1;
                TxtWagePercent.Text = wagePercent.ToString(ContentStringFormat);
            }
            else
            {
                TxtWagePercent.Text = (double.Parse(TxtWagePercent.Text) - 0.1).ToString(ContentStringFormat);
            }
        }

        private void BtnUp3_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TxtInsRound.Text.Trim()) || int.Parse(TxtInsRound.Text) < 0)
            {
                TxtInsRound.Text = "0";
            }
            else if (int.Parse(TxtInsRound.Text) >= 9)
            {
                TxtInsRound.Text = "0";
            }
            else
            {
                TxtInsRound.Text = (int.Parse(TxtInsRound.Text) + 1).ToString();
            }
        }

        private void BtnDown3_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TxtInsRound.Text.Trim()) || int.Parse(TxtInsRound.Text) - 1 < 0)
            {
                TxtInsRound.Text = "9";
            }
            else if (int.Parse(TxtInsRound.Text) > 9)
            {
                TxtInsRound.Text = "9";
            }
            else
            {
                TxtInsRound.Text = (int.Parse(TxtInsRound.Text) - 1).ToString();
            }
        }

        private void CboLoanType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (CboLoanType.SelectedIndex)
            {
                case 2:
                    TxtInstallmentNum.IsEnabled = false;
                    TxtInstallmentNum.Text = "1";
                    TxtInstallmentInterspace.Visibility = Visibility.Hidden;
                    BtnUp.IsEnabled = false;
                    BtnDown.IsEnabled = false;
                    BtnUp1.Visibility = Visibility.Hidden;
                    BtnDown1.Visibility = Visibility.Hidden;
                    LblInstallmentInterspace.Visibility = Visibility.Hidden;
                    LblMonth.Visibility = Visibility.Hidden;
                    TxtInstallmentAmount.Text = TxtLoanAmount.Text;
                    ChkGuarantor.IsEnabled = true;
                    break;
                case 3:
                    TxtInstallmentNum.IsEnabled = true;
                    TxtInstallmentInterspace.Visibility = Visibility.Visible;
                    BtnUp.IsEnabled = true;
                    BtnDown.IsEnabled = true;
                    BtnUp1.Visibility = Visibility.Visible;
                    BtnDown1.Visibility = Visibility.Visible;
                    LblInstallmentInterspace.Visibility = Visibility.Visible;
                    LblMonth.Visibility = Visibility.Visible;
                    ChkGuarantor.IsEnabled = false;
                    ChkGuarantor.IsChecked = true;
                    break;
                default:
                    if (_counter != 0)
                    {
                        TxtInstallmentNum.IsEnabled = true;
                        TxtInstallmentInterspace.Visibility = Visibility.Visible;
                        BtnUp.IsEnabled = true;
                        BtnDown.IsEnabled = true;
                        BtnUp1.Visibility = Visibility.Visible;
                        BtnDown1.Visibility = Visibility.Visible;
                        LblInstallmentInterspace.Visibility = Visibility.Visible;
                        LblMonth.Visibility = Visibility.Visible;
                        ChkGuarantor.IsEnabled = true;
                    }
                    break;
            }
        }

        private void CboWageType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (CboWageType.SelectedIndex)
            {
                case 1:
                    CboWageCalculationType.SelectedIndex = 0;
                    CboWageCalculationType.IsEnabled = false;
                    TxtWageAmount.IsEnabled = false;
                    TxtWageAmount.Text = "0";
                    TxtInstallmentWage.Text = "0";
                    TxtWageNum.Text = "0";
                    TxtTotalDebt.Text = TxtLoanAmount.Text;
                    TxtTotalPay.Text = TxtLoanAmount.Text;

                    break;
                case 2:
                    CboWageCalculationType.IsEnabled = true;
                    TxtInstallmentWage.Text = TxtWageAmount.Text;
                    TxtWageNum.Text = "1";
                    TxtTotalDebt.Text = TxtLoanAmount.Text;
                    TxtTotalPay.Text =
                        (long.Parse(Regex.Replace(TxtLoanAmount.Text, "[\\W]", "")) - long.Parse(Regex.Replace(TxtWageAmount.Text, "[\\W]", ""))).ToString(
                            CultureInfo.InvariantCulture);
                    break;
                case 4:
                    if (_counter != 0)
                        CboWageCalculationType.IsEnabled = true;
                    TxtInstallmentWage.Text =
                        (long.Parse(Regex.Replace(TxtWageAmount.Text, "[\\W]", "")) / long.Parse(Regex.Replace(TxtInstallmentNum.Text, "[\\W]", ""))).ToString(
                            CultureInfo.InvariantCulture);
                    TxtWageNum.Text = TxtInstallmentNum.Text;
                    TxtTotalDebt.Text =
                        (long.Parse(Regex.Replace(TxtLoanAmount.Text, "[\\W]", "")) + long.Parse(Regex.Replace(TxtWageAmount.Text, "[\\W]", ""))).ToString(
                            CultureInfo.InvariantCulture);
                    TxtTotalPay.Text = TxtLoanAmount.Text;
                    break;
                default:
                    if (_counter != 0)
                    {
                        CboWageCalculationType.IsEnabled = true;
                        TxtInstallmentWage.Text = TxtWageAmount.Text;
                        TxtWageNum.Text = "1";
                        TxtTotalDebt.Text =
                            (long.Parse(Regex.Replace(TxtLoanAmount.Text, "[\\W]", "")) + long.Parse(Regex.Replace(TxtWageAmount.Text, "[\\W]", ""))).ToString(
                                CultureInfo.InvariantCulture);
                        TxtTotalPay.Text = TxtLoanAmount.Text;
                    }
                    break;
            }
        }

        private void CboWageCalculationType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (CboWageCalculationType.SelectedIndex)
            {
                case 2:
                    LblWagePercent.Visibility = Visibility.Visible;
                    TxtWagePercent.Visibility = Visibility.Visible;
                    BtnUp2.Visibility = Visibility.Visible;
                    BtnDown2.Visibility = Visibility.Visible;
                    TxtWageAmount.IsEnabled = false;
                    TxtWageAmount.Text =
                        (long.Parse(Regex.Replace(TxtLoanAmount.Text, "[\\W]", "")) * long.Parse(Regex.Replace(TxtWagePercent.Text, "[\\W]", "")) / 100).ToString(
                            CultureInfo.InvariantCulture);
                    break;
                default:
                    if (_counter != 0)
                    {
                        LblWagePercent.Visibility = Visibility.Hidden;
                        TxtWagePercent.Visibility = Visibility.Hidden;
                        BtnUp2.Visibility = Visibility.Hidden;
                        BtnDown2.Visibility = Visibility.Hidden;
                        TxtWageAmount.IsEnabled = true;
                        TxtWageAmount.Text = "0";
                    }
                    break;
            }
        }

        private void CboGuaBlockType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (CboGuaBlockType.SelectedIndex)
            {
                case 2:
                    TxtGuaBlockAmount.Visibility = Visibility.Visible;
                    LblGuaBlockAmount.Visibility = Visibility.Visible;
                    LblRial6.Visibility = Visibility.Visible;
                    break;

                default:
                    if (_counter != 0)
                    {
                        TxtGuaBlockAmount.Visibility = Visibility.Hidden;
                        LblGuaBlockAmount.Visibility = Visibility.Hidden;
                        LblRial6.Visibility = Visibility.Hidden;
                    }
                    break;
            }
        }

        private void CboGuaBlockType2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (CboGuaBlockType2.SelectedIndex)
            {
                case 2:
                    TxtGuaBlockAmount2.Visibility = Visibility.Visible;
                    LblGuaBlockAmount2.Visibility = Visibility.Visible;
                    LblRial8.Visibility = Visibility.Visible;
                    break;

                default:
                    if (_counter != 0)
                    {
                        TxtGuaBlockAmount2.Visibility = Visibility.Hidden;
                        LblGuaBlockAmount2.Visibility = Visibility.Hidden;
                        LblRial8.Visibility = Visibility.Hidden;
                    }
                    break;
            }
        }

        private void CboGuaBlockType3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (CboGuaBlockType3.SelectedIndex)
            {
                case 2:
                    TxtGuaBlockAmount3.Visibility = Visibility.Visible;
                    LblGuaBlockAmount3.Visibility = Visibility.Visible;
                    LblRial10.Visibility = Visibility.Visible;
                    break;

                default:
                    if (_counter != 0)
                    {
                        TxtGuaBlockAmount3.Visibility = Visibility.Hidden;
                        LblGuaBlockAmount3.Visibility = Visibility.Hidden;
                        LblRial10.Visibility = Visibility.Hidden;
                    }
                    break;
            }
        }

        private void CboGuaType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (CboGuaType.SelectedIndex)
            {
                case 0:
                    TxtGuaAmount.Visibility = Visibility.Hidden;
                    TxtGuaReceiptNumber.Visibility = Visibility.Hidden;
                    LblGuaAmount.Visibility = Visibility.Hidden;
                    LblRial7.Visibility = Visibility.Hidden;
                    LblGuaReceiptNumber.Visibility = Visibility.Hidden;
                    break;

                default:
                    if (_counter != 0)
                    {
                        TxtGuaAmount.Visibility = Visibility.Visible;
                        TxtGuaReceiptNumber.Visibility = Visibility.Visible;
                        LblGuaAmount.Visibility = Visibility.Visible;
                        LblRial7.Visibility = Visibility.Visible;
                        LblGuaReceiptNumber.Visibility = Visibility.Visible;
                    }
                    break;
            }
        }

        private void CboGuaType2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (CboGuaType2.SelectedIndex)
            {
                case 0:
                    TxtGuaAmount2.Visibility = Visibility.Hidden;
                    TxtGuaReceiptNumber2.Visibility = Visibility.Hidden;
                    LblGuaAmount2.Visibility = Visibility.Hidden;
                    LblRial9.Visibility = Visibility.Hidden;
                    LblGuaReceiptNumber2.Visibility = Visibility.Hidden;
                    break;

                default:
                    if (_counter != 0)
                    {
                        TxtGuaAmount2.Visibility = Visibility.Visible;
                        TxtGuaReceiptNumber2.Visibility = Visibility.Visible;
                        LblGuaAmount2.Visibility = Visibility.Visible;
                        LblRial9.Visibility = Visibility.Visible;
                        LblGuaReceiptNumber2.Visibility = Visibility.Visible;
                    }
                    break;
            }
        }

        private void CboGuaType3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (CboGuaType3.SelectedIndex)
            {
                case 0:
                    TxtGuaAmount3.Visibility = Visibility.Hidden;
                    TxtGuaReceiptNumber3.Visibility = Visibility.Hidden;
                    LblGuaAmount3.Visibility = Visibility.Hidden;
                    LblRial11.Visibility = Visibility.Hidden;
                    LblGuaReceiptNumber3.Visibility = Visibility.Hidden;
                    break;

                default:
                    if (_counter != 0)
                    {
                        TxtGuaAmount3.Visibility = Visibility.Visible;
                        TxtGuaReceiptNumber3.Visibility = Visibility.Visible;
                        LblGuaAmount3.Visibility = Visibility.Visible;
                        LblRial11.Visibility = Visibility.Visible;
                        LblGuaReceiptNumber3.Visibility = Visibility.Visible;
                    }
                    break;
            }
        }

        private void TxtWageAmount_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TxtWageAmount.Text.Trim() == string.Empty)
            {
                TxtWageAmount.Text = "0";
            }
            else
            {
                decimal number;
                if (!decimal.TryParse(TxtWageAmount.Text, out number)) return;
                TxtWageAmount.Text = $"{number:N0}";
                TxtWageAmount.SelectionStart = TxtWageAmount.Text.Length;

                TxtTotalDebt.Text = CboWageType.SelectedIndex == 1 || CboWageType.SelectedIndex == 2
                    ? TxtLoanAmount.Text
                    : (long.Parse(Regex.Replace(TxtLoanAmount.Text, "[\\W]", "")) + long.Parse(Regex.Replace(TxtWageAmount.Text, "[\\W]", ""))).ToString(
                        CultureInfo.InvariantCulture);

                TxtTotalPay.Text = CboWageType.SelectedIndex == 2
                    ? (long.Parse(Regex.Replace(TxtLoanAmount.Text, "[\\W]", "")) - long.Parse(Regex.Replace(TxtWageAmount.Text, "[\\W]", ""))).ToString(
                        CultureInfo.InvariantCulture)
                    : TxtLoanAmount.Text;

                switch (CboWageType.SelectedIndex)
                {
                    case 1:
                        TxtInstallmentWage.Text = "0";
                        TxtWageNum.Text = "0";
                        break;
                    case 4:
                        TxtInstallmentWage.Text =
                            (long.Parse(Regex.Replace(TxtWageAmount.Text, "[\\W]", "")) / long.Parse(Regex.Replace(TxtInstallmentNum.Text, "[\\W]", ""))).ToString(
                                CultureInfo.InvariantCulture);
                        TxtWageNum.Text = TxtInstallmentNum.Text;
                        break;
                    default:
                        TxtInstallmentWage.Text = TxtWageAmount.Text;
                        TxtWageNum.Text = "1";
                        break;
                }
            }
        }

        private void TxtLoanAmount_TextChanged(object sender, TextChangedEventArgs e)
         {
            if (TxtLoanAmount.Text.Trim() == string.Empty)
            {
                TxtLoanAmount.Text = "0";
            }
            else
            {
                decimal number;
                if (!decimal.TryParse(TxtLoanAmount.Text, out number)) return;
                TxtLoanAmount.Text = $"{number:N0}";
                TxtLoanAmount.SelectionStart = TxtLoanAmount.Text.Length;

                if (_counter != 0)
                {
                    TxtTotalDebt.Text = CboWageType.SelectedIndex == 1 || CboWageType.SelectedIndex == 2
                        ? TxtLoanAmount.Text
                        : (long.Parse(Regex.Replace(TxtLoanAmount.Text, "[\\W]", "")) + long.Parse(Regex.Replace(TxtWageAmount.Text, "[\\W]", ""))).ToString(
                            CultureInfo.InvariantCulture);

                    TxtTotalPay.Text = CboWageType.SelectedIndex == 2
                        ? (long.Parse(Regex.Replace(TxtLoanAmount.Text, "[\\W]", "")) - long.Parse(Regex.Replace(TxtWageAmount.Text, "[\\W]", ""))).ToString(
                            CultureInfo.InvariantCulture)
                        : TxtLoanAmount.Text;
                }


                if (CboLoanType.SelectedIndex == 2)
                {
                    TxtInstallmentAmount.Text = TxtLoanAmount.Text;
                }
                else if (_counter != 0)
                {
                    TxtInstallmentAmount.Text =
                        (long.Parse(Regex.Replace(TxtLoanAmount.Text, "[\\W]", "")) / long.Parse(Regex.Replace(TxtInstallmentNum.Text, "[\\W]", ""))).ToString(
                            CultureInfo.InvariantCulture);
                }

                if (CboWageCalculationType.SelectedIndex == 2)
                {
                    TxtWageAmount.Text =
                        (long.Parse(Regex.Replace(TxtLoanAmount.Text, "[\\W]", "")) * long.Parse(Regex.Replace(TxtWagePercent.Text, "[\\W]", "")) / 100).ToString(
                            CultureInfo.InvariantCulture);
                }

            }
        }

        private void TxtInstallmentAmount_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TxtInstallmentAmount.Text.Trim() == string.Empty)
            {
                TxtInstallmentAmount.Text = "0";
            }
            else
            {
                decimal number;
                if (!decimal.TryParse(TxtInstallmentAmount.Text, out number)) return;
                TxtInstallmentAmount.Text = $"{number:N0}";
                TxtInstallmentAmount.SelectionStart = TxtInstallmentAmount.Text.Length;
            }
        }

        private void TxtTotalDebt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TxtTotalDebt.Text.Trim() == string.Empty)
            {
                TxtTotalDebt.Text = "0";
            }
            else
            {
                decimal number;
                if (!decimal.TryParse(TxtTotalDebt.Text, out number)) return;
                TxtTotalDebt.Text = $"{number:N0}";
                TxtTotalDebt.SelectionStart = TxtTotalDebt.Text.Length;

            }
        }

        private void TxtTotalPay_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TxtTotalPay.Text.Trim() == string.Empty)
            {
                TxtTotalPay.Text = "0";
            }
            else
            {
                decimal number;
                if (!decimal.TryParse(TxtTotalPay.Text, out number)) return;
                TxtTotalPay.Text = $"{number:N0}";
                TxtTotalPay.SelectionStart = TxtTotalPay.Text.Length;
            }
        }

        private void TxtWagePercent_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (TxtWagePercent.Text.Trim() == string.Empty)
            {
                TxtWagePercent.Text = "2";
            }
            else
            {
                if (CboWageCalculationType.SelectedIndex == 2)
                {
                    TxtWageAmount.Text =
                        (long.Parse(Regex.Replace(TxtLoanAmount.Text, "[\\W]", "")) * long.Parse(Regex.Replace(TxtWagePercent.Text, "[\\W]", "")) / 100).ToString(
                            CultureInfo.InvariantCulture);
                }
            }
        }

        private void TxtInstallmentNum_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TxtInstallmentNum.Text.Trim() == string.Empty)
            {
                TxtInstallmentNum.Text = "1";
            }
            else
            {
                TxtInstallmentAmount.Text =
                    (long.Parse(Regex.Replace(TxtLoanAmount.Text, "[\\W]", "")) / long.Parse(Regex.Replace(TxtInstallmentNum.Text, "[\\W]", ""))).ToString(
                        CultureInfo.InvariantCulture);
                if (CboWageType.SelectedIndex != 4) return;
                TxtInstallmentWage.Text =
                    (long.Parse(Regex.Replace(TxtWageAmount.Text, "[\\W]", "")) / long.Parse(Regex.Replace(TxtInstallmentNum.Text, "[\\W]", ""))).ToString(
                        CultureInfo.InvariantCulture);
                TxtWageNum.Text = TxtInstallmentNum.Text;
            }
        }

        private void TxtInstallmentWage_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TxtInstallmentWage.Text.Trim() == string.Empty)
            {
                TxtInstallmentWage.Text = "0";
            }
            else
            {
                decimal number;
                if (!decimal.TryParse(TxtInstallmentWage.Text, out number)) return;
                TxtInstallmentWage.Text = $"{number:N0}";
                TxtInstallmentWage.SelectionStart = TxtInstallmentWage.Text.Length;
            }
        }

        private void TxtGuaAmount_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TxtGuaAmount.Text.Trim() == string.Empty)
            {
                TxtGuaAmount.Text = "0";
            }
            else
            {
                decimal number;
                if (!decimal.TryParse(TxtGuaAmount.Text, out number)) return;
                TxtGuaAmount.Text = $"{number:N0}";
                TxtGuaAmount.SelectionStart = TxtGuaAmount.Text.Length;
            }
        }

        private void TxtGuaAmount2_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TxtGuaAmount2.Text.Trim() == string.Empty)
            {
                TxtGuaAmount2.Text = "0";
            }
            else
            {
                decimal number;
                if (!decimal.TryParse(TxtGuaAmount2.Text, out number)) return;
                TxtGuaAmount2.Text = $"{number:N0}";
                TxtGuaAmount2.SelectionStart = TxtGuaAmount2.Text.Length;
            }
        }

        private void TxtGuaAmount3_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TxtGuaAmount3.Text.Trim() == string.Empty)
            {
                TxtGuaAmount3.Text = "0";
            }
            else
            {
                decimal number;
                if (!decimal.TryParse(TxtGuaAmount3.Text, out number)) return;
                TxtGuaAmount3.Text = $"{number:N0}";
                TxtGuaAmount3.SelectionStart = TxtGuaAmount3.Text.Length;
            }
        }

        private void TxtGuaBlockAmount_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TxtGuaBlockAmount.Text.Trim() == string.Empty)
            {
                TxtGuaBlockAmount.Text = "0";
            }
            else
            {
                decimal number;
                if (!decimal.TryParse(TxtGuaBlockAmount.Text, out number)) return;
                TxtGuaBlockAmount.Text = $"{number:N0}";
                TxtGuaBlockAmount.SelectionStart = TxtGuaBlockAmount.Text.Length;
            }
        }

        private void TxtGuaBlockAmount2_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TxtGuaBlockAmount2.Text.Trim() == string.Empty)
            {
                TxtGuaBlockAmount2.Text = "0";
            }
            else
            {
                decimal number;
                if (!decimal.TryParse(TxtGuaBlockAmount2.Text, out number)) return;
                TxtGuaBlockAmount2.Text = $"{number:N0}";
                TxtGuaBlockAmount2.SelectionStart = TxtGuaBlockAmount2.Text.Length;
            }
        }

        private void TxtGuaBlockAmount3_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TxtGuaBlockAmount3.Text.Trim() == string.Empty)
            {
                TxtGuaBlockAmount3.Text = "0";
            }
            else
            {
                decimal number;
                if (!decimal.TryParse(TxtGuaBlockAmount3.Text, out number)) return;
                TxtGuaBlockAmount3.Text = $"{number:N0}";
                TxtGuaBlockAmount3.SelectionStart = TxtGuaBlockAmount3.Text.Length;
            }
        }

        private void TxtInsRound_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TxtInsRound.Text.Trim() == string.Empty)
            {
                TxtInsRound.Text = "0";
            }
            if (_counter != 0)
            {
                BtnCreatIns_Click(null, null);
            }
        }

        private void ChkGuarantor_Checked(object sender, RoutedEventArgs e)
        {
            if (_counter == 0) return;
            ChkGuarantor2.IsEnabled = true;
            TabGuarantor.IsEnabled = true;
            if (!ChkGuarantor.IsEnabled && ChkGuarantor.IsChecked == true) return;
            TabGuarantor.IsSelected = true;
        }

        private void ChkGuarantor_Unchecked(object sender, RoutedEventArgs e)
        {
            if (_counter == 0) return;
            ChkGuarantor2.IsEnabled = false;
            ChkGuarantor2.IsChecked = false;
            TabGuarantor.IsEnabled = false;
            TabWage.IsSelected = true;
        }

        private void ChkGuarantor2_Checked(object sender, RoutedEventArgs e)
        {
            if (_counter == 0) return;
            ChkGuarantor3.IsEnabled = true;
            TabGuarantor2.IsEnabled = true;
            TabGuarantor2.IsSelected = true;
        }

        private void ChkGuarantor2_Unchecked(object sender, RoutedEventArgs e)
        {
            if (_counter == 0) return;
            ChkGuarantor3.IsEnabled = false;
            ChkGuarantor3.IsChecked = false;
            TabGuarantor2.IsEnabled = false;
            TabGuarantor.IsSelected = true;
        }

        private void ChkGuarantor3_Checked(object sender, RoutedEventArgs e)
        {
            if (_counter == 0) return;
            TabGuarantor3.IsEnabled = true;
            TabGuarantor3.IsSelected = true;
        }

        private void ChkGuarantor3_Unchecked(object sender, RoutedEventArgs e)
        {
            if (_counter == 0) return;
            TabGuarantor3.IsEnabled = false;
            TabGuarantor2.IsSelected = true;
        }

        private void ChkIntroducer_Checked(object sender, RoutedEventArgs e)
        {
            if (_counter == 0) return;
            TabIntroducer.IsEnabled = true;
            TabIntroducer.IsSelected = true;
        }

        private void ChkIntroducer_Unchecked(object sender, RoutedEventArgs e)
        {
            if (_counter == 0) return;
            TabIntroducer.IsEnabled = false;
            TabWage.IsSelected = true;
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            GrpReal.IsEnabled = true;
            GrpLegal.IsEnabled = false;
        }

        private void RdoLegal_Checked(object sender, RoutedEventArgs e)
        {
            GrpLegal.IsEnabled = true;
            GrpReal.IsEnabled = false;
        }

        private void BtnGuaSelect_Click(object sender, RoutedEventArgs e)
        {
            InfoId = 0;
            CanReceive = 0;
            LblGuaPerFirstName.Content = string.Empty;
            LblGuaPerLastName.Content = string.Empty;
            LblGuaPerNationalCode.Content = string.Empty;
            CboGuaBlockType.IsEnabled = true;

            var winInfoSearch = new WinInfoSearch();
            winInfoSearch.ShowDialog();
            InfoId = winInfoSearch.Id;

            if (InfoId == 0) return;
            if (!CheckGuarantor())
            {
                InfoId = 0;
                return;
            }

            CheckGuarantorIsPer();

            LblGuaPerFirstName.Content = winInfoSearch.FirstName;
            LblGuaPerLastName.Content = winInfoSearch.LastName;
            LblGuaPerNationalCode.Content = winInfoSearch.NationalCode;
        }

        private void BtnGuaAdd_Click(object sender, RoutedEventArgs e)
        {
            InfoId = 0;
            CanReceive = 0;
            LblGuaPerFirstName.Content = string.Empty;
            LblGuaPerLastName.Content = string.Empty;
            LblGuaPerNationalCode.Content = string.Empty;
            CboGuaBlockType.IsEnabled = true;

            var winInfo = new WinInfo();
            winInfo.ShowDialog();
            InfoId = winInfo.Id;

            if (InfoId == 0) return;

            LblGuaPerFirstName.Content = winInfo.FirstName;
            LblGuaPerLastName.Content = winInfo.LastName;
            LblGuaPerNationalCode.Content = winInfo.NationalCode;

            CboGuaBlockType.SelectedIndex = 0;
            CboGuaBlockType.IsEnabled = false;
        }

        private void BtnGuaSelect2_Click(object sender, RoutedEventArgs e)
        {
            InfoId2 = 0;
            CanReceive2 = 0;
            LblGuaPerFirstName2.Content = string.Empty;
            LblGuaPerLastName2.Content = string.Empty;
            LblGuaPerNationalCode2.Content = string.Empty;
            CboGuaBlockType2.IsEnabled = true;

            var winInfoSearch = new WinInfoSearch();
            winInfoSearch.ShowDialog();
            InfoId2 = winInfoSearch.Id;

            if (InfoId2 == 0) return;
            if (!CheckGuarantor())
            {
                InfoId2 = 0;
                return;
            }

            CheckGuarantorIsPer2();

            LblGuaPerFirstName2.Content = winInfoSearch.FirstName;
            LblGuaPerLastName2.Content = winInfoSearch.LastName;
            LblGuaPerNationalCode2.Content = winInfoSearch.NationalCode;
        }

        private void BtnGuaAdd2_Click(object sender, RoutedEventArgs e)
        {
            InfoId2 = 0;
            CanReceive2 = 0;
            LblGuaPerFirstName2.Content = string.Empty;
            LblGuaPerLastName2.Content = string.Empty;
            LblGuaPerNationalCode2.Content = string.Empty;
            CboGuaBlockType2.IsEnabled = true;

            var winInfo = new WinInfo();
            winInfo.ShowDialog();
            InfoId2 = winInfo.Id;

            if (InfoId2 == 0) return;

            LblGuaPerFirstName2.Content = winInfo.FirstName;
            LblGuaPerLastName2.Content = winInfo.LastName;
            LblGuaPerNationalCode2.Content = winInfo.NationalCode;

            CboGuaBlockType2.SelectedIndex = 0;
            CboGuaBlockType2.IsEnabled = false;
        }

        private void BtnGuaSelect3_Click(object sender, RoutedEventArgs e)
        {
            InfoId3 = 0;
            CanReceive3 = 0;
            LblGuaPerFirstName3.Content = string.Empty;
            LblGuaPerLastName3.Content = string.Empty;
            LblGuaPerNationalCode3.Content = string.Empty;
            CboGuaBlockType3.IsEnabled = true;

            var winInfoSearch = new WinInfoSearch();
            winInfoSearch.ShowDialog();
            InfoId3 = winInfoSearch.Id;

            if (InfoId3 == 0) return;
            if (!CheckGuarantor())
            {
                InfoId3 = 0;
                return;
            }

            CheckGuarantorIsPer3();

            LblGuaPerFirstName3.Content = winInfoSearch.FirstName;
            LblGuaPerLastName3.Content = winInfoSearch.LastName;
            LblGuaPerNationalCode3.Content = winInfoSearch.NationalCode;
        }

        private void BtnGuaAdd3_Click(object sender, RoutedEventArgs e)
        {
            InfoId3 = 0;
            CanReceive3 = 0;
            LblGuaPerFirstName3.Content = string.Empty;
            LblGuaPerLastName3.Content = string.Empty;
            LblGuaPerNationalCode3.Content = string.Empty;
            CboGuaBlockType3.IsEnabled = true;

            var winInfo = new WinInfo();
            winInfo.ShowDialog();
            InfoId3 = winInfo.Id;

            if (InfoId3 == 0) return;

            LblGuaPerFirstName3.Content = winInfo.FirstName;
            LblGuaPerLastName3.Content = winInfo.LastName;
            LblGuaPerNationalCode3.Content = winInfo.NationalCode;

            CboGuaBlockType3.SelectedIndex = 0;
            CboGuaBlockType3.IsEnabled = false;
        }

        private void BtnRecomSelect_Click(object sender, RoutedEventArgs e)
        {
            InfoId4 = 0;
            LblRecomPerFirstName.Content = string.Empty;
            LblRecomPerLastName.Content = string.Empty;
            LblRecomPerNationalCode.Content = string.Empty;

            var winInfoSearch = new WinInfoSearch();
            winInfoSearch.ShowDialog();
            InfoId4 = winInfoSearch.Id;

            if (InfoId4 == 0) return;
            if (!CheckGuarantor())
            {
                InfoId4 = 0;
                return;
            }

            LblRecomPerFirstName.Content = winInfoSearch.FirstName;
            LblRecomPerLastName.Content = winInfoSearch.LastName;
            LblRecomPerNationalCode.Content = winInfoSearch.NationalCode;
        }

        private void BtnRecomAdd_Click(object sender, RoutedEventArgs e)
        {
            InfoId4 = 0;
            LblRecomPerFirstName.Content = string.Empty;
            LblRecomPerLastName.Content = string.Empty;
            LblRecomPerNationalCode.Content = string.Empty;

            var winInfo = new WinInfo();
            winInfo.ShowDialog();

            InfoId4 = winInfo.Id;

            if (InfoId4 == 0) return;

            LblRecomPerFirstName.Content = winInfo.FirstName;
            LblRecomPerLastName.Content = winInfo.LastName;
            LblRecomPerNationalCode.Content = winInfo.NationalCode;
        }

        private void TabInstallment_GotFocus(object sender, RoutedEventArgs e)
        {
            BtnCreatIns_Click(null, null);
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
        private void LoandataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();

            e.Row.Foreground = _loanInfoData[e.Row.GetIndex()].LoanPayOff == true ? new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Red);
        }
        private void dataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }
        #endregion

        #region Method

        private bool CheckIns()
        {
            if (_items != null && _items.Count != 0)
            {
                BtnCreatIns_Click(null, null);
                if (_items != null && _items.Count != 0) return true;
                Utility.Message("خطا", "مبلغ وام و تعداد اقساط آن به گونه ای است که نمی توان اقساط درستی برای آن تولید کرد", "Stop.png");
                return false;
            }
            Utility.MyMessageBox("پیام", "اقساط این وام با عدد گرد سازی 0 محاسبه می شود.\n آیا مایل به ادامه هستید؟",
                "Warning.png", false);
            if (!Utility.YesNo) return false;

            TxtInsRound.Text = "0";
            BtnCreatIns_Click(null, null);
            if (_items != null && _items.Count != 0) return true;
            Utility.Message("خطا", "مبلغ وام و تعداد اقساط آن به گونه ای است که نمی توان اقساط درستی برای آن تولید کرد", "Stop.png");
            return false;
        }

        private bool CheckPersonnelMembershipDate()
        {
            if (string.CompareOrdinal(PersonnelMembershipDate, Utility.CurrectDate(TxtLoanDate.Text)) > 0)
            {
                Utility.Message("خطا", "تاریخ وام وارد شده قبل از عضویت شخص می باشد", "Stop.png");
                return false;
            }
            return true;
        }

        private bool CheckGua()
        {
            if (TabGuarantor.IsEnabled)
            {
                if (LblGuaPerFirstName.Content.ToString() == string.Empty)
                {
                    Utility.Message("خطا",
                        "لظفا شخصی را برای ضامن اول انتخاب کنید. (اگر مایل به تعیین ضامن اول نیستید تیک آن را بردارید)",
                        "Stop.png");
                    return false;
                }
                if (CboLoanType.SelectedIndex == 3 && CboGuaType.SelectedIndex == 0 && CboGuaBlockType.SelectedIndex == 0)
                {
                    Utility.Message("خطا", "لطفا نوع ضمانت را مشخص کنید", "Stop.png");
                    return false;
                }
                if (CboGuaBlockType.SelectedIndex == 2 && long.Parse(Regex.Replace(TxtGuaBlockAmount.Text, "[\\W]", "")) == 0)
                {
                    Utility.Message("خطا", "لطفا مبلغ مورد نظر جهت مسدود سازی ضامن اول را وارد کنید", "Stop.png");
                    return false;
                }

                if (InfoId != 0 && CboGuaBlockType.SelectedIndex == 1 && CanReceive - long.Parse(Regex.Replace(TxtTotalDebt.Text, "[\\W]", "")) < 0)
                {
                    Utility.Message("خطا", "موجودی قابل برداشت حساب ضامن اول کمتر از مجموع بدهی وام می باشد. لذا قابل مسدود سازی نیست", "Stop.png");
                    return false;
                }

                if (InfoId != 0 && CboGuaBlockType.SelectedIndex == 2 && CanReceive - long.Parse(Regex.Replace(TxtGuaBlockAmount.Text, "[\\W]", "")) < 0)
                {
                    Utility.Message("خطا", "موجودی قابل برداشت حساب ضامن اول کمتر از مبلغ مورد نظر جهت مسدود سازی می باشد", "Stop.png");
                    return false;
                }
            }

            if (TabGuarantor2.IsEnabled)
            {
                if (LblGuaPerFirstName2.Content.ToString() == string.Empty)
                {
                    Utility.Message("خطا",
                        "لظفا شخصی را برای ضامن دوم انتخاب کنید. (اگر مایل به تعیین ضامن دوم نیستید تیک آن را بردارید)",
                        "Stop.png");
                    return false;
                }
                if (CboGuaBlockType2.SelectedIndex == 2 && long.Parse(Regex.Replace(TxtGuaBlockAmount2.Text, "[\\W]", "")) == 0)
                {
                    Utility.Message("خطا", "لطفا مبلغ مورد نظر جهت مسدود سازی ضامن دوم را وارد کنید", "Stop.png");
                    return false;
                }

                if (InfoId2 != 0 && CboGuaBlockType2.SelectedIndex == 1 && CanReceive2 - long.Parse(Regex.Replace(TxtTotalDebt.Text, "[\\W]", "")) < 0)
                {
                    Utility.Message("خطا", "موجودی قابل برداشت حساب ضامن دوم کمتر از مجموع بدهی وام می باشد. لذا قابل مسدود سازی نیست", "Stop.png");
                    return false;
                }

                if (InfoId2 != 0 && CboGuaBlockType2.SelectedIndex == 2 && CanReceive2 - long.Parse(Regex.Replace(TxtGuaBlockAmount2.Text, "[\\W]", "")) < 0)
                {
                    Utility.Message("خطا", "موجودی قابل برداشت حساب ضامن دوم کمتر از مبلغ مورد نظر جهت مسدود سازی می باشد", "Stop.png");
                    return false;
                }
            }

            if (TabGuarantor3.IsEnabled)
            {
                if (LblGuaPerFirstName3.Content.ToString() == string.Empty)
                {
                    Utility.Message("خطا",
                        "لظفا شخصی را برای ضامن سوم انتخاب کنید. (اگر مایل به تعیین ضامن سوم نیستید تیک آن را بردارید)",
                        "Stop.png");
                    return false;
                }
                if (CboGuaBlockType3.SelectedIndex == 2 && long.Parse(Regex.Replace(TxtGuaBlockAmount3.Text, "[\\W]", "")) == 0)
                {
                    Utility.Message("خطا", "لطفا مبلغ مورد نظر جهت مسدود سازی ضامن سوم را وارد کنید", "Stop.png");
                    return false;
                }

                if (InfoId3 != 0 && CboGuaBlockType3.SelectedIndex == 1 && CanReceive3 - long.Parse(Regex.Replace(TxtTotalDebt.Text, "[\\W]", "")) < 0)
                {
                    Utility.Message("خطا", "موجودی قابل برداشت حساب ضامن سوم کمتر از مجموع بدهی وام می باشد. لذا قابل مسدود سازی نیست", "Stop.png");
                    return false;
                }

                if (InfoId3 != 0 && CboGuaBlockType3.SelectedIndex == 2 && CanReceive3 - long.Parse(Regex.Replace(TxtGuaBlockAmount3.Text, "[\\W]", "")) < 0)
                {
                    Utility.Message("خطا", "موجودی قابل برداشت حساب ضامن سوم کمتر از مبلغ مورد نظر جهت مسدود سازی می باشد", "Stop.png");
                    return false;
                }
            }

            if (TabIntroducer.IsEnabled)
            {
                if (RdoReal.IsChecked == true && string.IsNullOrEmpty(LblRecomPerFirstName.Content.ToString()))
                {
                    Utility.Message("خطا", "لظفا شخصی را برای معرف انتخاب کنید. (اگر مایل به تعیین معرف نیستید تیک آن را بردارید)", "Stop.png");
                    return false;
                }
                if (RdoLegal.IsChecked==true && string.IsNullOrEmpty(CboRecom.Text))
                {
                    Utility.Message("خطا", "لظفا موسسه ای را برای معرف انتخاب کنید یا یک موسسه ی جدید وارد کنید. (اگر مایل به تعیین معرف نیستید تیک آن را بردارید)", "Stop.png");
                    return false;
                }
            }
            return true;
        }

        private bool CheckEmpty()
        {

            if (CboLoanType.SelectedIndex == 0)
            {
                Utility.Message("خطا", "لطفا نوع وام را مشخص کنید", "Stop.png");
                return false;
            }

            if (string.IsNullOrEmpty(TxtLoanAmount.Text.Trim()) || long.Parse(Regex.Replace(TxtLoanAmount.Text, "[\\W]", "")) == 0)
            {
                Utility.Message("خطا", "لطفا مبلغ وام را وارد کنید", "Stop.png");
                return false;
            }

            if (string.IsNullOrEmpty(TxtLoanDate.Text.Trim()))
            {
                Utility.Message("خطا", "لطفا تاریخ پرداخت را وارد کنید", "Stop.png");
                return false;
            }
            if (!Utility.CheckDate(TxtLoanDate.Text))
            {
                Utility.Message("خطا", "لطفا یک تاریخ معتبر برای تاریخ پرداخت وام وارد کنید", "Stop.png");
                return false;
            }


            if (int.Parse(TxtInstallmentNum.Text) < 1)
            {
                Utility.Message("خطا", "تعداد اقساط نباید از 1 قسط کمتر باشد", "Stop.png");
                return false;
            }
            if (int.Parse(TxtInstallmentNum.Text) > 60)
            {
                Utility.Message("خطا", "تعداد اقساط نباید از 60 قسط بیشتر باشد", "Stop.png");
                return false;
            }
            if (string.IsNullOrEmpty(TxtInstallmentFirstPayDate.Text.Trim()))
            {
                Utility.Message("خطا", "لطفا تاریخ اولین قسط را وارد کنید", "Stop.png");
                return false;
            }
            if (!Utility.CheckDate(TxtInstallmentFirstPayDate.Text))
            {
                Utility.Message("خطا", "لطفا یک تاریخ معتبر برای تاریخ اولین قسط وارد کنید", "Stop.png");
                return false;
            }

            if (int.Parse(TxtInstallmentInterspace.Text) < 1)
            {
                Utility.Message("خطا", "فاصله بین اقساط نباید از 1 ماه کمتر باشد", "Stop.png");
                return false;
            }

            if (int.Parse(TxtInstallmentInterspace.Text) > 12)
            {
                Utility.Message("خطا", "فاصله بین اقساط نباید از 1 سال (12 ماه) بیشتر باشد", "Stop.png");
                return false;
            }

            if (CboWageType.SelectedIndex == 0)
            {
                Utility.Message("خطا", "لطفا نوع دریافت کارمزد را مشخص کنید", "Stop.png");
                return false;
            }

            if (CboWageType.SelectedIndex != 1 &&
                (string.IsNullOrEmpty(TxtWageAmount.Text.Trim()) || long.Parse(Regex.Replace(TxtWageAmount.Text, "[\\W]", "")) == 0))
            {
                Utility.Message("خطا", "لطفا مبلغ کارمزد را مشخص کنید", "Stop.png");
                return false;
            }

            if (CboWageType.SelectedIndex != 1 && CboWageCalculationType.SelectedIndex == 0)
            {
                Utility.Message("خطا", "لطفا نوع محاسبه کارمزد را مشخص کنید", "Stop.png");
                return false;
            }

            if (CboWageCalculationType.SelectedIndex == 2 && float.Parse(TxtWagePercent.Text) < 1)
            {
                Utility.Message("خطا", "درصد کارمزد باید از 0 بیشتر باشد", "Stop.png");
                return false;
            }

            if (CboWageCalculationType.SelectedIndex == 2 && float.Parse(TxtWagePercent.Text) > 100)
            {
                Utility.Message("خطا", "درصد کارمزد باید از 100 کمتر باشد", "Stop.png");
                return false;
            }

            return true;
        }

        private bool CheckDate()
        {
            if (PersianDateTime.Parse(Utility.CurrectDate(TxtLoanDate.Text)) >
                PersianDateTime.Parse(Utility.CurrectDate(TxtInstallmentFirstPayDate.Text)))
            {
                Utility.Message("خطا", "تاریخ اولین قسط نمیتواند قبل از تاریخ پرداخت وام باشد",
                    "Stop.png");
                return false;
            }
            return true;
        }

        private bool CheckSelect()
        {
            if (DgdLoan.SelectedIndex == -1)
            {
                Utility.Message("خطا", "وام مورد نظر را جهت ویرایش یا حذف انتخاب کنید", "Stop.png");
                return false;
            }
            return true;
        }

        private bool CheckGuarantor()
        {
            if (InfoId != 0 && InfoId == PersonnelInfoId || InfoId2 != 0 && InfoId2 == PersonnelInfoId ||
                InfoId3 != 0 && InfoId3 == PersonnelInfoId)
            {
                Utility.Message("خطا", "وام گیرنده نمی تواند به عنوان ضامن انتخاب شود", "Stop.png");
                return false;
            }

            if (InfoId4 != 0 && InfoId4 == PersonnelInfoId)
            {
                Utility.Message("خطا", "وام گیرنده نمی تواند به عنوان معرف انتخاب شود", "Stop.png");
                return false;
            }

            if (InfoId != 0 && InfoId2 != 0 && InfoId == InfoId2 || InfoId != 0 && InfoId3 != 0 && InfoId == InfoId3 ||
                InfoId2 != 0 && InfoId3 != 0 && InfoId2 == InfoId3)
            {
                Utility.Message("خطا", "این شخص یک بار به عنوان ضامن این وام انتخاب شده است", "Stop.png");
                return false;
            }
            return true;
        }

        private async void CheckGuarantorIsPer()
        {
            var personnelId = await DGuarantor.CheckInfoIsPer(InfoId);
            if (personnelId == null)
            {
                CboGuaBlockType.SelectedIndex = 0;
                CboGuaBlockType.IsEnabled = false;
            }
            else
            {
                CanReceive = (long)await DAccount.CanReceive((int)personnelId, InfoId);
                CboGuaBlockType.IsEnabled = true;
            }
        }

        private async void CheckGuarantorIsPer2()
        {
            var personnelId2 = await DGuarantor.CheckInfoIsPer(InfoId2);
            if (personnelId2 == null)
            {
                CboGuaBlockType2.SelectedIndex = 0;
                CboGuaBlockType2.IsEnabled = false;
            }
            else
            {
                CanReceive2 = (long)await DAccount.CanReceive((int)personnelId2, InfoId2);
                CboGuaBlockType2.IsEnabled = true;
            }
        }

        private async void CheckGuarantorIsPer3()
        {
            var personnelId3 = await DGuarantor.CheckInfoIsPer(InfoId3);
            if (personnelId3 == null)
            {
                CboGuaBlockType3.SelectedIndex = 0;
                CboGuaBlockType3.IsEnabled = false;
            }
            else
            {
                CanReceive3 = (long)await DAccount.CanReceive((int)personnelId3, InfoId3);
                CboGuaBlockType3.IsEnabled = true;
            }
        }

        private bool CheckCanDelete(int count)
        {
            if (count > 0)
            {
                Utility.Message("خطا", "به دلیل موجود بودن سوابق مالی برای این وام قادر به حذف آن نیستید","Stop.png");
                return false;
            }
            var selectloanItem = _loanInfoData[DgdLoan.SelectedIndex];
            if (selectloanItem.Loan_Account_Id == null)
            {
                Utility.Message("خطا", "شما قادر به حذف این وام نیستید",
                    "Stop.png");
                return false;
            }

            _index = _accountData.FindIndex(x => x.Id == selectloanItem.Loan_Account_Id);
            long memFeeAmount = 0;
            if (_accountData.Count >= _index + 3)
            {
                if (_accountData[_index + 1].Account_TransactionType_Id == 6 &&
                    _accountData[_index + 1].AccountDescription == "برداشت از وام برای حق عضویت" &&
                    _accountData[_index + 2].Account_TransactionType_Id == 11)
                {
                    memFeeAmount = (long) _accountData[_index + 2].AccountAmount;
                    _canDeleteMemFee = true;
                }
            }
            var selectloanAccItem = _accountData.SingleOrDefault(x => x.Id == selectloanItem.Loan_Account_Id);

            if (selectloanAccItem?.AccountAmount > _canReceive + memFeeAmount)
            {
                Utility.Message("خطا",
                    "به دلیل بیشتر بودن مبلغ وام واریز شده در حساب از مبلغ قابل برداشت قادر به حذف این وام نیستید",
                    "Stop.png");
                return false;
            }

            var index = _index;
            var lastIndex = _accountData.Count - 1;
            if (lastIndex <= index) return true;
            if (_canDeleteMemFee)
            {
                index = index + 2;
                if (lastIndex <= index + 1) return true;
            }
            var remaining = _accountData[index + 1].AccountCurrentBalance - _accountData[_index].AccountAmount;

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
