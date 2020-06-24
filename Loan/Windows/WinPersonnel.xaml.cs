using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Arash;
using DAL;
using DAL.Class;
using Loan.Class;
using Clipboard = System.Windows.Clipboard;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;

namespace Loan.Windows
{
    public partial class WinPersonnel
    {
        private OpenFileDialog _openFileDialog;
        private bool _selectedPicture;
        private bool _selectedSignature;
        private string _pictureFileName;
        private string _signatureFileName;
        private string _pictureFilePath;
        private string _signatureFilePath;
        private readonly string _currentDirectory;
        private readonly string _directoryPath;
        private List<spSelectPersonnelInfo_Result> _personnelData;
        private List<spSelectPersonnelInfo_Result> _personnelSearchData;
        private List<spSelectAccountInfo_Result> _accountData;
        private List<tblChargeMonthly> _chargeMonthlyData;
        private List<tblLoan> _loanData;
        private List<tblPerAccType> _perAccTypeData;
        private List<tblGuarantor> _guarantorData;
        private List<tblIntroducer> _introducerData;

        private InputLanguage _currentInputLanguage;

        public int Counter;
        private bool _add = true;

        public WinPersonnel()
        {
            InitializeComponent();
            _currentDirectory = Directory.GetCurrentDirectory(); //estekhrag masir jari ejra barnameh
            _directoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "ARKVN");
            _personnelData = new List<spSelectPersonnelInfo_Result>();
            _personnelSearchData = new List<spSelectPersonnelInfo_Result>();
            _accountData = new List<spSelectAccountInfo_Result>();
            _chargeMonthlyData = new List<tblChargeMonthly>();
            _loanData = new List<tblLoan>();
            _perAccTypeData = new List<tblPerAccType>();
            _guarantorData = new List<tblGuarantor>();
            _introducerData = new List<tblIntroducer>();
        }

        public int InfoId { get; set; }

        public string InfoIMage { get; set; }

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
            if (string.IsNullOrEmpty(TxtSearch.Text.Trim()) || _add)
            {
                DgdPersonnel.ItemsSource = _personnelSearchData;
                TxtSearch.Text = string.Empty;
            }
            else
            {
                TxtSearch_TextChanged(null, null);
            }
            BtnNew_Click(null, null);
        }

        private async void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckEmpty() || !Utility.CheckNationalCode(TxtNationalCode.Text) || !Utility.CheckEmail(TxtEmail.Text) || !await CheckRepeatAdd()) return;

            var infoId = 0;
            if (DgdPersonnel.IsEnabled)
            {
                #region AddInfo

                try
                {
                    var addInfo = new DInfo
                    {
                        DInfoFirstName = TxtFirstName.Text,
                        DInfoLastName = TxtLastName.Text,
                        DInfoFatherName =
                            TxtFatherName.Text.Trim() == string.Empty ? null : TxtFatherName.Text,
                        DInfoGender = CboGender.SelectedIndex.ToString(),
                        DInfoNationalCode = TxtNationalCode.Text,
                        DInfoCode = TxtCode.Text.Trim() == string.Empty ? null : TxtCode.Text,
                        DInfoBirthDay =
                            TxtBirthDay.Text == string.Empty
                                ? null
                                : Utility.CurrectDate(TxtBirthDay.Text),
                        DInfoBirthPlace =
                            TxtBirthPlace.Text.Trim() == string.Empty ? null : TxtBirthPlace.Text,
                        DInfoMarried = CboMarried.SelectedIndex.ToString(),
                        DInfoTell = TxtTell.Text.Trim() == string.Empty ? null : TxtTell.Text,
                        DInfoMobile = TxtMobile.Text.Trim() == string.Empty ? null : TxtMobile.Text,
                        DInfoEmail = TxtEmail.Text.Trim() == string.Empty ? null : TxtEmail.Text,
                        DInfoPostalCode =
                            TxtPostalCode.Text.Trim() == string.Empty ? null : TxtPostalCode.Text,
                        DInfoAddress = TxtAddress.Text.Trim() == string.Empty ? null : TxtAddress.Text,
                        DInfoJobName = TxtJobName.Text.Trim() == string.Empty ? null : TxtJobName.Text,
                        DInfoJobPlaceName =
                            TxtJobPlaceName.Text.Trim() == string.Empty ? null : TxtJobPlaceName.Text,
                        DInfoJobTell = TxtJobTell.Text.Trim() == string.Empty ? null : TxtJobTell.Text,
                        DInfoJobFax = TxtJobFax.Text.Trim() == string.Empty ? null : TxtJobFax.Text,
                        DInfoJobAddress =
                            TxtJobAddress.Text.Trim() == string.Empty ? null : TxtJobAddress.Text,
                        DInfoImage = _selectedPicture ? SelectePicture() : null,
                        DInfoDescription =
                            TxtDescription.Text.Trim() == string.Empty ? null : TxtDescription.Text
                    };
                    infoId = Convert.ToInt32(await addInfo.Add());
                }
                catch (Exception exception)
                {
                    Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در ثبت اطلاعات شخصی\n" + exception.Message);
                    return;
                }

                #endregion
            }
            else
            {
                #region EditInfo

                try
                {
                    var editinfo = new DInfo
                    {
                        DId = InfoId,
                        DInfoFirstName = TxtFirstName.Text,
                        DInfoLastName = TxtLastName.Text,
                        DInfoFatherName =
                            TxtFatherName.Text.Trim() == string.Empty ? null : TxtFatherName.Text,
                        DInfoGender = CboGender.SelectedIndex.ToString(),
                        DInfoNationalCode = TxtNationalCode.Text,
                        DInfoCode = TxtCode.Text.Trim() == string.Empty
                            ? null
                            : TxtCode.Text,
                        DInfoBirthDay =
                            TxtBirthDay.Text == string.Empty
                                ? null
                                : Utility.CurrectDate(TxtBirthDay.Text),
                        DInfoBirthPlace =
                            TxtBirthPlace.Text.Trim() == string.Empty ? null : TxtBirthPlace.Text,
                        DInfoMarried = CboMarried.SelectedIndex.ToString(),
                        DInfoTell = TxtTell.Text.Trim() == string.Empty
                            ? null
                            : TxtTell.Text,
                        DInfoMobile =
                            TxtMobile.Text.Trim() == string.Empty ? null : TxtMobile.Text,
                        DInfoEmail =
                            TxtEmail.Text.Trim() == string.Empty ? null : TxtEmail.Text,
                        DInfoPostalCode =
                            TxtPostalCode.Text.Trim() == string.Empty ? null : TxtPostalCode.Text,
                        DInfoAddress =
                            TxtAddress.Text.Trim() == string.Empty ? null : TxtAddress.Text,
                        DInfoJobName =
                            TxtJobName.Text.Trim() == string.Empty ? null : TxtJobName.Text,
                        DInfoJobPlaceName = TxtJobPlaceName.Text.Trim() == string.Empty
                            ? null
                            : TxtJobPlaceName.Text,
                        DInfoJobTell =
                            TxtJobTell.Text.Trim() == string.Empty ? null : TxtJobTell.Text,
                        DInfoJobFax =
                            TxtJobFax.Text.Trim() == string.Empty ? null : TxtJobFax.Text,
                        DInfoJobAddress =
                            TxtJobAddress.Text.Trim() == string.Empty ? null : TxtJobAddress.Text,
                        DInfoImage = _selectedPicture ? SelectePicture() : InfoIMage,
                        DInfoDescription = TxtDescription.Text.Trim() == string.Empty
                            ? null
                            : TxtDescription.Text
                    };
                    await Task.Run(() => editinfo.Edit());
                }
                catch (Exception exception)
                {
                    Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در بروز رسانی اطلاعات شخصی\n" + exception.Message);
                }

                #endregion
            }

            #region AddPersonnel

            int personnelId;
            try
            {
                var addPersonnel = new DPersonnel
                {
                    DInfoId = DgdPersonnel.IsEnabled ? infoId : InfoId,
                    DPersonnelId = TxtPerId.Text,
                    DPersonnelMembership = CboMembership.SelectedIndex.ToString(),
                    DPersonnelMembershipDate = Utility.CurrectDate(TxtMembershipDate.Text),
                    DPersonnelBarCode = null,
                    DPersonnelQrCode = null,
                    DPersonnelSignature = _selectedSignature ? SelecteSignature() : null
                };
                personnelId = Convert.ToInt32(await addPersonnel.Add());
            }
            catch (Exception exception)
            {
                Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در ثبت اطلاعات عضویت\n" + exception.Message);
                return;
            }

            #endregion

            #region AddPerAccType

            try
            {
                var addPerAccType = new DPerAccType
                {
                    DPersonnelId = personnelId,
                    DAccountTypeId = 1,
                    DAccountNumber = TxtAccountNum.Text
                };
                await Task.Run(() => addPerAccType.Add());
            }
            catch (Exception exception)
            {
                Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در ثبت اطلاعات حساب\n" + exception.Message);
                try
                {
                    var deletePersonnel = new DPersonnel
                    {
                        DId = personnelId
                    };
                    deletePersonnel.Delete();
                }
                catch (Exception exception1)
                {
                    Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در حذف اطلاعات عضویت\n" + exception1.Message);
                }
                return;
            }

            #endregion

            #region AddAccount

            if (
                !(string.IsNullOrEmpty(TxtInitialBalance.Text.Trim()) || long.Parse(Regex.Replace(TxtInitialBalance.Text, "[\\W]", "")) == 0))
            {
                try
                {
                    var addAccount = new DAccount
                    {
                        DPersonnelId = personnelId,
                        DPaymentTypeId = Convert.ToByte(CboPayType.SelectedIndex),
                        DTransactionTypeId = 1,
                        DAmount = long.Parse(Regex.Replace(TxtInitialBalance.Text, "[\\W]", "")),
                        DReceiptNumber = TxtReceiptNumber.Text.Trim() == string.Empty ? null : TxtReceiptNumber.Text,
                        DCurrentBalance = long.Parse(Regex.Replace(TxtInitialBalance.Text, "[\\W]", "")),
                        DPaymentDate = Utility.CurrectDate(TxtPayDate.Text),
                        DDescription = TxtFeeDescription.Text.Trim() == string.Empty ? null : TxtPayDescription.Text
                    };
                    await addAccount.Add();
                }
                catch (Exception exception)
                {
                    Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در ثبت اطلاعات موجودی اولیه حساب\n" + exception.Message);
                }
            }

            #endregion

            #region AddMembershipFee

            if (!(string.IsNullOrEmpty(TxtFee.Text.Trim()) || long.Parse(Regex.Replace(TxtFee.Text, "[\\W]", "")) == 0))
            {
                try
                {
                    var addAccount = new DAccount
                    {
                        DPersonnelId = personnelId,
                        DPaymentTypeId = Convert.ToByte(CboFeeType.SelectedIndex),
                        DTransactionTypeId = 11,
                        DAmount = long.Parse(Regex.Replace(TxtFee.Text, "[\\W]", "")),
                        DReceiptNumber = TxtFeeReceiptNumber.Text.Trim() == string.Empty
                            ? null
                            : TxtFeeReceiptNumber.Text,
                        DCurrentBalance =
                            string.IsNullOrEmpty(TxtInitialBalance.Text.Trim()) || long.Parse(Regex.Replace(TxtInitialBalance.Text, "[\\W]", "")) == 0
                                ? long.Parse(Regex.Replace(TxtFee.Text, "[\\W]", ""))
                                : long.Parse(Regex.Replace(TxtFee.Text, "[\\W]", "")) +
                                  long.Parse(Regex.Replace(TxtInitialBalance.Text, "[\\W]", "")),
                        DPaymentDate = Utility.CurrectDate(TxtFeeDate.Text),
                        DDescription = TxtFeeDescription.Text.Trim() == string.Empty ? null : TxtFeeDescription.Text
                    };
                    await addAccount.Add();
                }
                catch (Exception exception)
                {
                    Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در ثبت اطلاعات حق عضویت\n" + exception.Message);
                }
            }

            #endregion

            Window_Loaded(null, null);
            DgdPersonnel.IsEnabled = true;
            TxtNationalCode.IsEnabled = true;
            BtnNew.IsEnabled = true;
            Utility.Message("پیام", "مشخصات با موفقیت ثبت گردید", "Correct.png");
        }

        private async void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckSelect() || !CheckEmpty() || !Utility.CheckNationalCode(TxtNationalCode.Text) ||
                !Utility.CheckEmail(TxtEmail.Text)) return;

            var selectItem = _personnelSearchData[DgdPersonnel.SelectedIndex];
            try
            {
                _perAccTypeData = await DPerAccType.GetData(selectItem.Id);
            }
            catch (Exception exception)
            {
                Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در دریافت اطلاعات\n" + exception.Message);
                return;
            }
            var selectPerAccTypeItem = _perAccTypeData[0];

            if (!await CheckRepeatEdit(selectItem.InfoNationalCode, selectItem.PersonnelId,
                selectPerAccTypeItem.PerAccTypeAccountNumber)) return;

            #region EditInfo

            try
            {
                var editinfo = new DInfo
                {
                    DId = Convert.ToInt32(selectItem.Personnel_Info_Id),
                    DInfoFirstName = TxtFirstName.Text,
                    DInfoLastName = TxtLastName.Text,
                    DInfoFatherName =
                        TxtFatherName.Text.Trim() == string.Empty ? null : TxtFatherName.Text,
                    DInfoGender = CboGender.SelectedIndex.ToString(),
                    DInfoNationalCode = TxtNationalCode.Text,
                    DInfoCode = TxtCode.Text.Trim() == string.Empty
                        ? null
                        : TxtCode.Text,
                    DInfoBirthDay =
                        TxtBirthDay.Text == string.Empty
                            ? null
                            : Utility.CurrectDate(TxtBirthDay.Text),
                    DInfoBirthPlace =
                        TxtBirthPlace.Text.Trim() == string.Empty ? null : TxtBirthPlace.Text,
                    DInfoMarried = CboMarried.SelectedIndex.ToString(),
                    DInfoTell = TxtTell.Text.Trim() == string.Empty
                        ? null
                        : TxtTell.Text,
                    DInfoMobile =
                        TxtMobile.Text.Trim() == string.Empty ? null : TxtMobile.Text,
                    DInfoEmail =
                        TxtEmail.Text.Trim() == string.Empty ? null : TxtEmail.Text,
                    DInfoPostalCode =
                        TxtPostalCode.Text.Trim() == string.Empty ? null : TxtPostalCode.Text,
                    DInfoAddress =
                        TxtAddress.Text.Trim() == string.Empty ? null : TxtAddress.Text,
                    DInfoJobName =
                        TxtJobName.Text.Trim() == string.Empty ? null : TxtJobName.Text,
                    DInfoJobPlaceName = TxtJobPlaceName.Text.Trim() == string.Empty
                        ? null
                        : TxtJobPlaceName.Text,
                    DInfoJobTell =
                        TxtJobTell.Text.Trim() == string.Empty ? null : TxtJobTell.Text,
                    DInfoJobFax =
                        TxtJobFax.Text.Trim() == string.Empty ? null : TxtJobFax.Text,
                    DInfoJobAddress =
                        TxtJobAddress.Text.Trim() == string.Empty ? null : TxtJobAddress.Text,
                    DInfoImage = _selectedPicture ? SelectePicture() : selectItem.InfoImage,
                    DInfoDescription = TxtDescription.Text.Trim() == string.Empty
                        ? null
                        : TxtDescription.Text

                };
                await Task.Run(() => editinfo.Edit());
            }
            catch (Exception exception)
            {
                Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در ویرایش اطلاعات شخصی\n" + exception.Message);
                return;
            }

            #endregion

            #region EditPersonnel

            try
            {
                var editPersonnel = new DPersonnel
                {
                    DId = selectItem.Id,
                    DPersonnelId = TxtPerId.Text,
                    DPersonnelMembership = CboMembership.SelectedIndex.ToString(),
                    DPersonnelMembershipDate = Utility.CurrectDate(TxtMembershipDate.Text),
                    DPersonnelBarCode = null,
                    DPersonnelQrCode = null,
                    DPersonnelSignature = _selectedSignature ? SelecteSignature() : selectItem.PersonnelSignature
                };
                await Task.Run(() => editPersonnel.Edit());
            }
            catch (Exception exception)
            {
                Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در ویرایش اطلاعات عضویت\n" + exception.Message);
                return;
            }

            #endregion

            #region EditPerAccType

            if (selectPerAccTypeItem.PerAccTypeAccountNumber != TxtAccountNum.Text)
            {
                try
                {
                    var perAccType = new DPerAccType
                    {
                        DId = selectPerAccTypeItem.Id,
                        DPersonnelId = selectItem.Id,
                        DAccountTypeId = 1,
                        DAccountNumber = TxtAccountNum.Text
                    };
                    await Task.Run(() => perAccType.Edit());
                }
                catch (Exception exception)
                {
                    Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در ویرایش اطلاعات حساب\n" + exception.Message);
                    return;
                }
            }

            #endregion

            #region EditAccount

            if (!(string.IsNullOrEmpty(TxtInitialBalance.Text.Trim()) || long.Parse(Regex.Replace(TxtInitialBalance.Text, "[\\W]", "")) == 0) &&
                _accountData.Count == 0)
            {
                try
                {
                    var editAccount = new DAccount
                    {
                        DPersonnelId = selectItem.Id,
                        DPaymentTypeId = Convert.ToByte(CboPayType.SelectedIndex),
                        DTransactionTypeId = 1,
                        DAmount = long.Parse(Regex.Replace(TxtInitialBalance.Text, "[\\W]", "")),

                        DReceiptNumber = TxtReceiptNumber.Text.Trim() == string.Empty ||
                                        TxtReceiptNumber.IsEnabled == false
                            ? null
                            : TxtReceiptNumber.Text,

                        DCurrentBalance = long.Parse(Regex.Replace(TxtInitialBalance.Text, "[\\W]", "")),
                        DPaymentDate = Utility.CurrectDate(TxtPayDate.Text),
                        DDescription = TxtPayDescription.Text.Trim() == string.Empty ? null : TxtPayDescription.Text
                    };
                    await editAccount.Add();
                }
                catch (Exception exception)
                {
                    Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در ویرایش اطلاعات موجودی اولیه حساب\n" + exception.Message);
                    return;
                }
            }
            else if (
                !(string.IsNullOrEmpty(TxtInitialBalance.Text.Trim()) ||
                  long.Parse(Regex.Replace(TxtInitialBalance.Text, "[\\W]", "")) == 0))
            {
                var selectAccountItem = _accountData[0];
                if (_accountData.Count == 1 && selectAccountItem.Account_TransactionType_Id == 1)
                {
                    try
                    {
                        var editAccount = new DAccount
                        {
                            DId = selectAccountItem.Id,
                            DPersonnelId = selectItem.Id,
                            DPaymentTypeId = Convert.ToByte(CboPayType.SelectedIndex),
                            DTransactionTypeId = 1,
                            DAmount = long.Parse(Regex.Replace(TxtInitialBalance.Text, "[\\W]", "")),
                            DReceiptNumber = TxtReceiptNumber.Text.Trim() == string.Empty ||
                                            TxtReceiptNumber.IsEnabled == false
                                ? null
                                : TxtReceiptNumber.Text,
                            DCurrentBalance = long.Parse(Regex.Replace(TxtInitialBalance.Text, "[\\W]","")),
                            DPaymentDate = Utility.CurrectDate(TxtPayDate.Text),
                            DDescription = TxtPayDescription.Text.Trim() == string.Empty
                                ? null
                                : TxtPayDescription.Text
                        };
                        await Task.Run(() => editAccount.Edit());
                    }
                    catch (Exception exception)
                    {
                        Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در ویرایش اطلاعات موجودی اولیه حساب\n" + exception.Message);
                        return;
                    }
                }
            }

            #endregion

            #region GetAccountData
            try
            {
                _accountData = await DAccount.GetData(selectItem.Id);
            }
            catch (Exception exception)
            {
                Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در دریافت اطلاعات حساب پس انداز\n" + exception.Message);
                return;
            }
            #endregion

            #region EditMembershipFee

            var value = _accountData.Exists(item => item.Account_TransactionType_Id == 11);

            if (!(string.IsNullOrEmpty(TxtFee.Text.Trim()) || long.Parse(Regex.Replace(TxtFee.Text, "[\\W]", "")) == 0) && !value)
            {
                try
                {
                    var editAccount = new DAccount
                    {
                        DPersonnelId = selectItem.Id,
                        DPaymentTypeId = Convert.ToByte(CboFeeType.SelectedIndex),
                        DTransactionTypeId = 11,
                        DAmount = long.Parse(Regex.Replace(TxtFee.Text, "[\\W]", "")),

                        DReceiptNumber = TxtFeeReceiptNumber.Text.Trim() == string.Empty ||
                                         TxtFeeReceiptNumber.IsEnabled == false
                            ? null
                            : TxtFeeReceiptNumber.Text,

                        DCurrentBalance = _accountData.Count == 0
                            ? long.Parse(Regex.Replace(TxtFee.Text, "[\\W]", ""))
                            // ReSharper disable once PossibleInvalidOperationException
                            : (long)_accountData[_accountData.Count - 1].AccountCurrentBalance +
                              long.Parse(Regex.Replace(TxtFee.Text, "[\\W]", "")),

                        DPaymentDate = Utility.CurrectDate(TxtFeeDate.Text),
                        DDescription = TxtFeeDescription.Text.Trim() == string.Empty ? null : TxtFeeDescription.Text
                    };
                    await editAccount.Add();
                }
                catch (Exception exception)
                {
                    Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در ویرایش اطلاعات حق عضویت\n" + exception.Message);
                    return;
                }
            }
            else if (!(string.IsNullOrEmpty(TxtFee.Text.Trim()) || long.Parse(Regex.Replace(TxtFee.Text, "[\\W]", "")) == 0))
            {

                var selectAccountItem = _accountData[_accountData.Count - 1];
                if (selectAccountItem.Account_TransactionType_Id == 11)
                {
                    try
                    {
                        var editAccount = new DAccount
                        {
                            DId = selectAccountItem.Id,
                            DPersonnelId = selectItem.Id,
                            DPaymentTypeId = Convert.ToByte(CboFeeType.SelectedIndex),
                            DTransactionTypeId = 11,
                            DAmount = long.Parse(Regex.Replace(TxtFee.Text, "[\\W]", "")),
                            DReceiptNumber = TxtFeeReceiptNumber.Text.Trim() == string.Empty ||
                                             TxtFeeReceiptNumber.IsEnabled == false
                                ? null
                                : TxtFeeReceiptNumber.Text,
                            DCurrentBalance = _accountData.Count == 1
                                ? long.Parse(Regex.Replace(TxtFee.Text, "[\\W]", ""))
                                // ReSharper disable once PossibleInvalidOperationException
                                : (long)_accountData[_accountData.Count - 2].AccountCurrentBalance +
                                  long.Parse(Regex.Replace(TxtFee.Text, "[\\W]", "")),
                            DPaymentDate = Utility.CurrectDate(TxtFeeDate.Text),
                            DDescription = TxtFeeDescription.Text.Trim() == string.Empty
                                ? null
                                : TxtFeeDescription.Text
                        };
                        await Task.Run(() => editAccount.Edit());
                    }
                    catch (Exception exception)
                    {
                        Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در ویرایش اطلاعات حق عضویت\n" + exception.Message);
                        return;
                    }
                }
            }

            #endregion

            _add = false;
            Window_Loaded(null, null);
            Utility.Message("پیام", "مشخصات با موفقیت ویرایش گردید", "Correct.png");
        }

        private async void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckSelect()) return;
            var selectItem = _personnelSearchData[DgdPersonnel.SelectedIndex];
            try
            {
                _loanData = await DLoan.GetLoanData(selectItem.Id);
                _guarantorData = await DGuarantor.GetInfoGuaData(selectItem.Personnel_Info_Id);
                _introducerData = await DIntroducer.GetInfoIntroData(selectItem.Personnel_Info_Id);
            }
            catch (Exception exception)
            {
                Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در دریافت اطلاعات\n" + exception.Message);
                return;
            }

            if (!CheckCanDelete()) return;
            Utility.MyMessageBox(
                "آیا از حذف " + TxtFirstName.Text + " " + TxtLastName.Text + " با شماره ملی " + TxtNationalCode.Text +
                " اطمینان دارید؟ ", "هشدار", "Warning.png", false);
            if (!Utility.YesNo) return;
            try
            {
                ImgPersonnelImage.Source = new BitmapImage(new Uri(_currentDirectory + @"\Icon\Personnel.png"));
                ImgSignatureImage.Source = new BitmapImage(new Uri(_currentDirectory + @"\Icon\Signature.png"));
                try
                {
                    File.Delete($@"{_directoryPath}\Image\Personnel\Signature\" + selectItem.PersonnelSignature);
                    File.Delete($@"{_directoryPath}\Image\Personnel\Picture\" + selectItem.InfoImage);

                }
                catch (Exception)
                {
                    //
                }
                var deletePersonnel = new DPersonnel
                {
                    DId = selectItem.Id
                };
                await Task.Run(() => deletePersonnel.Delete());
                if (selectItem.Personnel_Info_Id != null)
                {
                    var deletePersonnelInfo = new DInfo
                    {
                        DId = (int) selectItem.Personnel_Info_Id
                    };
                    await Task.Run(() => deletePersonnelInfo.Delete());
                }
                var deletepPerAccType = new DPerAccType
                {
                    DPersonnelId = selectItem.Id
                };
                await Task.Run(() => deletepPerAccType.Delete());
            }
            catch (Exception exception)
            {
                Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در حذف اطلاعات\n" + exception.Message);
                return;
            }
            Window_Loaded(null, null);
            Utility.Message("پیام", "اطلاعات با موفقیت حذف گردید", "Correct.png");

        }

        private void BtnChoosePicture_Click(object sender, RoutedEventArgs e)
        {
            _selectedPicture = false;
            _openFileDialog = new OpenFileDialog
            {
                Filter = @"Image Files (*.jpg;*.png;*.bmp) |*.jpg;*.png;*.bmp",
                Title = "انتخاب عکس"
            };
            try
            {
                if (_openFileDialog.ShowDialog() != true) return;
                ImgPersonnelImage.Source = Utility.DisplayImage(_openFileDialog.FileName);
                //استخراج نام کامل عکس
                _pictureFileName = Path.GetFileName(_openFileDialog.FileName);
                _pictureFilePath = Path.GetFullPath(_openFileDialog.FileName);
                _selectedPicture = true;
            }
            catch (Exception)
            {
                ImgPersonnelImage.Source = new BitmapImage(new Uri(_currentDirectory + @"\Icon\Perssonel.png"));
                Utility.Message("اخطار", "عکس یافت نشد", "Stop.png");
            }
        }

        private void BtnChooseSignature_Click(object sender, RoutedEventArgs e)
        {
            _selectedSignature = false;
            _openFileDialog = new OpenFileDialog
            {
                Filter = @"Image Files (*.jpg;*.png;*.bmp) |*.jpg;*.png;*.bmp",
                Title = "انتخاب امضاء"
            };
            try
            {
                if (_openFileDialog.ShowDialog() != true) return;
                _selectedSignature = true;
                ImgSignatureImage.Source = Utility.DisplayImage(_openFileDialog.FileName);
                //استخراج نام کامل عکس
                _signatureFileName = Path.GetFileName(_openFileDialog.FileName);
                _signatureFilePath = Path.GetFullPath(_openFileDialog.FileName);
            }
            catch (Exception)
            {
                ImgSignatureImage.Source = new BitmapImage(new Uri(_currentDirectory + @"\Icon\Signature.png"));
                Utility.Message("اخطار", "عکس یافت نشد", "Stop.png");
            }
        }

        private void BtnNew_Click(object sender, RoutedEventArgs e)
        {
            TxtSearch.Focus();
            _selectedPicture = false;
            _selectedSignature = false;
            TxtPerId.Text = string.Empty;
            TxtFirstName.Text = string.Empty;
            TxtLastName.Text = string.Empty;
            TxtFatherName.Text = string.Empty;
            CboGender.SelectedIndex = 0;
            TxtNationalCode.Text = string.Empty;
            TxtCode.Text = string.Empty;
            TxtBirthDay.Text = string.Empty;
            TxtBirthPlace.Text = string.Empty;
            CboMarried.SelectedIndex = 0;
            TxtTell.Text = string.Empty;
            TxtMobile.Text = string.Empty;
            TxtEmail.Text = string.Empty;
            TxtPostalCode.Text = string.Empty;
            TxtAddress.Text = string.Empty;
            TxtJobName.Text = string.Empty;
            TxtJobPlaceName.Text = string.Empty;
            TxtJobTell.Text = string.Empty;
            TxtJobFax.Text = string.Empty;
            TxtJobAddress.Text = string.Empty;
            CboMembership.SelectedIndex = 0;
            CboMembership.IsEnabled = true;
            TxtMembershipDate.Text = PersianDate.Today.ToString();
            TxtMembershipDate.IsEnabled = true;
            TxtPayDate.Text = PersianDate.Today.ToString();
            TxtPayDate.IsEnabled = true;
            TxtFeeDate.Text = PersianDate.Today.ToString();
            TxtFeeDate.IsEnabled = true;
            CboPayType.SelectedIndex = 0;
            CboFeeType.SelectedIndex = 0;
            TxtAccountNum.Text = string.Empty;
            TxtInitialBalance.Text = "0";
            TxtInitialBalance.IsEnabled = true;
            TxtFee.Text = "0";
            TxtFee.IsEnabled = true;
            TxtReceiptNumber.Text = string.Empty;
            TxtFeeReceiptNumber.Text = string.Empty;
            TxtDescription.Text = string.Empty;
            TxtPayDescription.Text = string.Empty;
            TxtFeeDescription.Text = string.Empty;
            ImgPersonnelImage.Source = new BitmapImage(new Uri(_currentDirectory + @"\Icon\Personnel.png"));
            ImgSignatureImage.Source = new BitmapImage(new Uri(_currentDirectory + @"\Icon\Signature.png"));
            BtnAutoId_Click(null, null);
            BtnAutoAccountNum_Click(null, null);
            BtnAdd.IsEnabled = true;
            DgdPersonnel.SelectedIndex = -1;
            Counter = 1;
            _add = true;
        }

        private async void BtnAutoId_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TxtPerId.Text = await DPersonnel.GetPersonnelId();
            }
            catch (Exception exception)
            {
                Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در دریافت اطلاعات\n" + exception.Message);
            }
        }

        private async void BtnAutoAccountNum_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TxtAccountNum.Text = await DPersonnel.GetAccountId();
            }
            catch (Exception exception)
            {
                Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در دریافت اطلاعات\n" + exception.Message);
            }
        }

        private async void DgdPersonnel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DgdPersonnel.SelectedIndex == -1) return;
            BtnAdd.IsEnabled = false;
            var selectItem = _personnelSearchData[DgdPersonnel.SelectedIndex];
            try
            {
                _perAccTypeData = await DPerAccType.GetData(selectItem.Id);
            }
            catch (Exception exception)
            {
                Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در دریافت اطلاعات\n" + exception.Message);
                return;
            }
            var selectPerAccTypeItem = _perAccTypeData[0];
            TxtAccountNum.Text = selectPerAccTypeItem.PerAccTypeAccountNumber;
            TxtPerId.Text = selectItem.PersonnelId;
            TxtFirstName.Text = selectItem.InfoFirstName;
            TxtLastName.Text = selectItem.InfoLastName;
            TxtFatherName.Text = selectItem.InfoFatherName;
            TxtNationalCode.Text = selectItem.InfoNationalCode;
            TxtCode.Text = selectItem.InfoCode;
            CboGender.SelectedIndex = selectItem.InfoGender == "0" ? 0 : 1;
            TxtBirthDay.Text = selectItem.InfoBirthDay;
            TxtBirthPlace.Text = selectItem.InfoBirthPlace;
            CboMarried.SelectedIndex = selectItem.InfoMarried == "0" ? 0 : 1;
            TxtTell.Text = selectItem.InfoTell;
            TxtMobile.Text = selectItem.InfoMobile;
            TxtEmail.Text = selectItem.InfoEmail;
            TxtPostalCode.Text = selectItem.InfoPostalCode;
            TxtAddress.Text = selectItem.InfoAddress;
            TxtJobName.Text = selectItem.InfoJobName;
            TxtJobPlaceName.Text = selectItem.InfoJobPlaceName;
            TxtJobTell.Text = selectItem.InfoJobTell;
            TxtJobFax.Text = selectItem.InfoJobFax;
            TxtJobAddress.Text = selectItem.InfoJobAddress;
            CboMembership.SelectedIndex = int.Parse(selectItem.PersonnelMembership);
            TxtMembershipDate.Text = selectItem.PersonnelMembershipDate;

            #region SignatureImg

            if (string.IsNullOrEmpty(selectItem.PersonnelSignature))
            {
                ImgSignatureImage.Source =
                    new BitmapImage(new Uri(_currentDirectory + @"\Icon\Signature.png"));
            }
            else
            {
                try
                {
                    ImgSignatureImage.Source =
                        Utility.DisplayImage($@"{_directoryPath}\Image\Personnel\Signature\" + selectItem.PersonnelSignature);
                }
                catch (Exception)
                {
                    ImgSignatureImage.Source =
                        new BitmapImage(new Uri(_currentDirectory + @"\Icon\Signature.png"));
                }
            }
            #endregion

            #region PerImg

            if (string.IsNullOrEmpty(selectItem.InfoImage))
            {
                ImgPersonnelImage.Source =
                    new BitmapImage(new Uri(_currentDirectory + @"\Icon\Personnel.png"));
            }
            else
            {
                try
                {
                    ImgPersonnelImage.Source =
                        Utility.DisplayImage($@"{_directoryPath}\Image\Personnel\Picture\" + selectItem.InfoImage);
                }
                catch (Exception)
                {
                    ImgPersonnelImage.Source =
                        new BitmapImage(new Uri(_currentDirectory + @"\Icon\Personnel.png"));
                }
            }

            #endregion

            TxtDescription.Text = selectItem.InfoDescription;
            try
            {
                _accountData = await DAccount.GetData(selectItem.Id);
            }
            catch (Exception exception)
            {
                Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در دریافت اطلاعات حساب پس انداز\n" + exception.Message);
                return;
            }
            try
            {
                _chargeMonthlyData = await DChargeMonthly.GetData(selectItem.Id);
            }
            catch (Exception exception)
            {
                Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در دریافت اطلاعات شارژ ماهانه\n" + exception.Message);
                return;
            }
            TxtMembershipDate.IsEnabled = _accountData.Count == 0 && _chargeMonthlyData.Count == 0;
            CboMembership.IsEnabled = _chargeMonthlyData.Count == 0;

            if (_accountData.Count > 0)
            {
                var selectAccountItem = _accountData[0];
                if (selectAccountItem.Account_TransactionType_Id == 1)
                {
                    CboPayType.SelectedIndex = Convert.ToInt32(selectAccountItem.Account_PaymentType_Id);
                    TxtInitialBalance.Text = selectAccountItem.AccountAmount.ToString();
                    TxtReceiptNumber.Text = selectAccountItem.AccountReceiptNumber;
                    TxtPayDescription.Text = selectAccountItem.AccountDescription;
                    TxtPayDate.Text = selectAccountItem.AccountPaymentDate;
                    if (_accountData.Count == 1)
                    {
                        CboPayType.IsEnabled = true;
                        TxtInitialBalance.IsEnabled = true;
                        TxtReceiptNumber.IsEnabled = true;
                        TxtPayDescription.IsEnabled = true;
                        TxtPayDate.IsEnabled = true;
                    }
                    if (_accountData.Count > 1)
                    {
                        CboPayType.IsEnabled = false;
                        TxtInitialBalance.IsEnabled = false;
                        TxtReceiptNumber.IsEnabled = false;
                        TxtPayDescription.IsEnabled = false;
                        TxtPayDate.IsEnabled = false;
                    }
                }
                var selectAccountFee = _accountData.Find(item => item.Account_TransactionType_Id == 11);
                if (selectAccountFee != null)
                {
                    CboFeeType.SelectedIndex = Convert.ToInt32(selectAccountFee.Account_PaymentType_Id);
                    TxtFee.Text = selectAccountFee.AccountAmount.ToString();
                    TxtFeeReceiptNumber.Text = selectAccountFee.AccountReceiptNumber;
                    TxtFeeDescription.Text = selectAccountFee.AccountDescription;
                    TxtFeeDate.Text = selectAccountFee.AccountPaymentDate;

                    if (_accountData[_accountData.Count - 1].Id != selectAccountFee.Id)
                    {
                        CboFeeType.IsEnabled = false;
                        TxtFee.IsEnabled = false;
                        TxtFeeReceiptNumber.IsEnabled = false;
                        TxtFeeDescription.IsEnabled = false;
                        TxtFeeDate.IsEnabled = false;
                    }
                    else
                    {
                        var index = _accountData.FindIndex(item => item.Account_TransactionType_Id == 11);
                        if (index >= 2 && _accountData[index - 1].Account_TransactionType_Id == 6 &&
                            _accountData[index - 1].AccountDescription == "برداشت از وام برای حق عضویت" &&
                            _accountData[index - 2].Account_TransactionType_Id == 5)
                        {
                            CboFeeType.IsEnabled = false;
                            TxtFee.IsEnabled = false;
                            TxtFeeReceiptNumber.IsEnabled = false;
                            TxtFeeDescription.IsEnabled = false;
                            TxtFeeDate.IsEnabled = false;
                        }
                        else
                        {
                            CboFeeType.IsEnabled = true;
                            TxtFee.IsEnabled = true;
                            TxtFeeReceiptNumber.IsEnabled = true;
                            TxtFeeDescription.IsEnabled = true;
                            TxtFeeDate.IsEnabled = true;
                        }
                    }
                }
                else
                {
                    CboFeeType.SelectedIndex = 0;
                    TxtFee.Text = "0";
                    TxtFeeReceiptNumber.Text = string.Empty ;
                    TxtFeeDescription.Text = string.Empty;
                    TxtFeeDate.Text = string.Empty;

                    CboFeeType.IsEnabled = false;
                    TxtFee.Text = "0";
                    TxtFeeReceiptNumber.Text = string.Empty;
                    TxtFeeDescription.Text = string.Empty;
                    TxtFeeDate.Text = string.Empty;
                }
                if (selectAccountItem.Account_TransactionType_Id != 1)
                {
                    CboPayType.IsEnabled = false;
                    TxtInitialBalance.IsEnabled = false;
                    TxtReceiptNumber.IsEnabled = false;
                    TxtPayDescription.IsEnabled = false;
                    TxtPayDate.IsEnabled = false;

                    CboFeeType.IsEnabled = false;
                    TxtFee.IsEnabled = true;
                    TxtFeeReceiptNumber.IsEnabled = false;
                    TxtFeeDescription.IsEnabled = false;
                    TxtFeeDate.IsEnabled = false;
                }
            }
            else
            {
                CboPayType.SelectedIndex = 0;
                CboFeeType.SelectedIndex = 0;
                TxtInitialBalance.Text = "0";
                TxtFee.Text = "0";
                TxtReceiptNumber.Text = string.Empty;
                TxtFeeReceiptNumber.Text = string.Empty;
                TxtPayDescription.Text = string.Empty;
                TxtFeeDescription.Text = string.Empty;
                TxtPayDate.Text = string.Empty;
                TxtFeeDate.Text = string.Empty;

                CboPayType.IsEnabled = false;
                CboFeeType.IsEnabled = false;
                TxtInitialBalance.IsEnabled = true;
                TxtFee.IsEnabled = true;
                TxtReceiptNumber.IsEnabled = false;
                TxtFeeReceiptNumber.IsEnabled = false;
                TxtPayDescription.IsEnabled = false;
                TxtFeeDescription.IsEnabled = false;
                TxtPayDate.IsEnabled = false;
                TxtFeeDate.IsEnabled = false;

            }
        }

        private async void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            var search = TxtSearch.Text;
            _personnelSearchData = _personnelData;
            _personnelSearchData =
                await Task.Run(() => _personnelSearchData.FindAll(
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

        private void TxtInitialBalance_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TxtInitialBalance.Text.Trim() == string.Empty)
            {
                TxtInitialBalance.Text = "0";
                CboPayType.SelectedIndex = 0;
                CboPayType.IsEditable = false;
                TxtReceiptNumber.Text = string.Empty;
                TxtReceiptNumber.IsEnabled = false;
                TxtPayDescription.Text = string.Empty;
                TxtPayDescription.IsEnabled = false;
                TxtPayDate.Text = string.Empty;
                TxtPayDate.IsEnabled = false;
            }
            else
            {
                decimal number;
                if (!decimal.TryParse(TxtInitialBalance.Text, out number)) return;
                TxtInitialBalance.Text = $"{number:N0}";
                TxtInitialBalance.SelectionStart = TxtInitialBalance.Text.Length;

                if (long.Parse(Regex.Replace(TxtInitialBalance.Text, "[\\W]", "")) == 0)
                {
                    CboPayType.SelectedIndex = 0;
                    CboPayType.IsEnabled = false;
                    TxtReceiptNumber.Text = string.Empty;
                    TxtReceiptNumber.IsEnabled = false;
                    TxtPayDescription.Text = string.Empty;
                    TxtPayDescription.IsEnabled = false;
                    TxtPayDate.Text = string.Empty;
                    TxtPayDate.IsEnabled = false;
                }
                else
                {
                    CboPayType.IsEnabled = true;
                    TxtReceiptNumber.IsEnabled = true;
                    TxtPayDescription.IsEnabled = true;
                    TxtPayDate.IsEnabled = true;
                }
            }
        }

        private void TxtFee_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TxtFee.Text.Trim() == string.Empty)
            {
                TxtFee.Text = "0";
                CboFeeType.SelectedIndex = 0;
                CboFeeType.IsEditable = false;
                TxtFeeReceiptNumber.Text = string.Empty;
                TxtFeeReceiptNumber.IsEnabled = false;
                TxtFeeDescription.Text = string.Empty;
                TxtFeeDescription.IsEnabled = false;
                TxtFeeDate.Text = string.Empty;
                TxtFeeDate.IsEnabled = false;
            }
            else
            {
                decimal number;
                if (!decimal.TryParse(TxtFee.Text, out number)) return;
                TxtFee.Text = $"{number:N0}";
                TxtFee.SelectionStart = TxtFee.Text.Length;

                if (long.Parse(Regex.Replace(TxtFee.Text, "[\\W]", "")) == 0)
                {
                    CboFeeType.SelectedIndex = 0;
                    CboFeeType.IsEnabled = false;
                    TxtFeeReceiptNumber.Text = string.Empty;
                    TxtFeeReceiptNumber.IsEnabled = false;
                    TxtFeeDescription.Text = string.Empty;
                    TxtFeeDescription.IsEnabled = false;
                    TxtFeeDate.Text = string.Empty;
                    TxtFeeDate.IsEnabled = false;
                }
                else
                {
                    CboFeeType.IsEnabled = true;
                    TxtFeeReceiptNumber.IsEnabled = true;
                    TxtFeeDescription.IsEnabled = true;
                    TxtFeeDate.IsEnabled = true;
                }
            }
        }

        private void TxtEmail_GotFocus(object sender, RoutedEventArgs e)
        {
            _currentInputLanguage = InputLanguage.CurrentInputLanguage;
        }

        private void TxtEmail_LostFocus(object sender, RoutedEventArgs e)
        {
            InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(_currentInputLanguage.Culture);
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

        private void EnglishInput(object sender, KeyEventArgs e)
        {
            var language = new CultureInfo("en-US");
            InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(language);
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
            if (CboMembership.SelectedIndex == 0)
            {
                Utility.Message("خطا", "لطفا نوع عضویت را مشخص کنید", "Stop.png");
                return false;
            }

            if (string.IsNullOrEmpty(TxtPerId.Text.Trim()) || TxtPerId.Text == "0")
            {
                Utility.Message("خطا", "لطفا شماره عضویت را وارد کنید", "Stop.png");
                return false;
            }

            if (string.IsNullOrEmpty(TxtMembershipDate.Text.Trim()))
            {
                Utility.Message("خطا", "لطفا تاریخ عضویت را وارد کنید", "Stop.png");
                return false;
            }

            if (!Utility.CheckDate(TxtMembershipDate.Text))
            {
                Utility.Message("خطا", "لطفا یک تاریخ صحیح برای تاریخ عضویت انتخاب کنید", "Stop.png");
                return false;
            }

            if (!(string.IsNullOrEmpty(TxtFee.Text.Trim()) || long.Parse(Regex.Replace(TxtFee.Text, "[\\W]", "")) == 0))
            {
                if (string.IsNullOrEmpty(TxtFeeDate.Text.Trim()))
                {
                    Utility.Message("خطا", "لطفا تاریخ پرداخت حق عضویت را وارد کنید", "Stop.png");
                    return false;
                }

                if (!Utility.CheckDate(TxtFeeDate.Text))
                {
                    Utility.Message("خطا", "لطفا یک تاریخ صحیح برای تاریخ حق عضویت انتخاب کنید", "Stop.png");
                    return false;
                }

                if (string.CompareOrdinal(Utility.CurrectDate(TxtMembershipDate.Text), Utility.CurrectDate(TxtFeeDate.Text)) > 0)
                {
                    Utility.Message("خطا", "تاریخ پرداخت حق عضویت قبل از تاریخ عضویت شخص می باشد", "Stop.png");
                    return false;
                }
            }

            if (string.IsNullOrEmpty(TxtAccountNum.Text.Trim()) || TxtAccountNum.Text == "0")
            {
                Utility.Message("خطا", "لطفا شماره حساب را وارد کنید", "Stop.png");
                return false;
            }
            if (!(string.IsNullOrEmpty(TxtInitialBalance.Text.Trim()) || long.Parse(Regex.Replace(TxtInitialBalance.Text, "[\\W]", "")) == 0))
            {
                if (string.IsNullOrEmpty(TxtPayDate.Text.Trim()))
                {
                    Utility.Message("خطا", "لطفا تاریخ پرداخت موجودی اولیه را وارد کنید", "Stop.png");
                    return false;
                }

                if (!Utility.CheckDate(TxtPayDate.Text))
                {
                    Utility.Message("خطا", "لطفا یک تاریخ صحیح برای تاریخ پرداخت موجودی اولیه انتخاب کنید", "Stop.png");
                    return false;
                }

                if (string.CompareOrdinal(Utility.CurrectDate(TxtMembershipDate.Text), Utility.CurrectDate(TxtPayDate.Text)) > 0)
                {
                    Utility.Message("خطا", "تاریخ پرداخت قبل از تاریخ عضویت شخص می باشد", "Stop.png");
                    return false;
                }

                if (string.CompareOrdinal(Utility.CurrectDate(TxtPayDate.Text), Utility.CurrectDate(TxtFeeDate.Text)) > 0)
                {
                    Utility.Message("خطا", "تاریخ پرداخت حق عضویت نمیتواند قبل از تاریخ پرداخت موجودی اولیه باشد", "Stop.png");
                    return false;
                }
            }
            if (!(string.IsNullOrEmpty(TxtFee.Text.Trim()) || long.Parse(Regex.Replace(TxtFee.Text, "[\\W]", "")) == 0) &&
                string.IsNullOrEmpty(CboFeeType.Text.Trim()))
            {
                Utility.Message("خطا", "لطفا نوع پرداخت حق عضویت را مشخص کنید", "Stop.png");
                return false;
            }

            if (
                !(string.IsNullOrEmpty(TxtInitialBalance.Text.Trim()) || long.Parse(Regex.Replace(TxtInitialBalance.Text, "[\\W]", "")) == 0) &&
                string.IsNullOrEmpty(CboPayType.Text.Trim()))
            {
                Utility.Message("خطا", "لطفا نوع پرداخت را مشخص کنید", "Stop.png");
                return false;
            }

            if (string.IsNullOrEmpty(TxtFirstName.Text.Trim()))
            {
                Utility.Message("خطا", "لطفا نام را وارد کنید", "Stop.png");
                return false;
            }

            if (string.IsNullOrEmpty(TxtLastName.Text.Trim()))
            {
                Utility.Message("خطا", "لطفا نام خانوادگی را وارد کنید", "Stop.png");
                return false;
            }

            if (TxtNationalCode.Text.Trim() == string.Empty)
            {
                Utility.Message("خطا", "لطفا کد ملی را وارد کنید", "Stop.png");
                return false;
            }

            if (!string.IsNullOrEmpty(TxtBirthDay.Text) && !Utility.CheckDate(TxtBirthDay.Text))
            {
                Utility.Message("خطا", "لطفا یک تاریخ صحیح برای تاریخ تولد انتخاب کنید", "Stop.png");
                return false;
            }
            return true;
        }

        private async Task<bool> CheckRepeatAdd()
        {

            if (_personnelData.Any(t => t.InfoNationalCode == TxtNationalCode.Text) && DgdPersonnel.IsEnabled)
            {
                Utility.Message("اخطار", "شخصی با این کد ملی قبلا ثبت گردیده است", "Warning.png");
                return false;
            }
            List<tblInfo> infoData;
            try
            {
                infoData = await DInfo.CheckNationalCode(TxtNationalCode.Text);
            }
            catch (Exception exception)
            {
                Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در دریافت اطلاعات\n" + exception.Message);
                return false;
            }
            //taeen tekrari naboodan cod meli va ya nadashtane meghdar
            if (infoData.Count != 0 && DgdPersonnel.IsEnabled)
            {
                Utility.MyMessageBox("هشدار",
                    "شخصی با این کد ملی قبلا به عنوان ضامن یا معرف ثبت شده است.آیا مایل به ثبت شخص به عنوان عضو هستید؟",
                     "Warning.png", false);

                if (!Utility.YesNo) return false;

                DgdPersonnel.IsEnabled = false;
                TxtNationalCode.IsEnabled = false;
                BtnNew.IsEnabled = false;

                var selectItem = infoData[0];
                InfoId = selectItem.Id;
                TxtFirstName.Text = selectItem.InfoFirstName;
                TxtLastName.Text = selectItem.InfoLastName;
                TxtFatherName.Text = selectItem.InfoFatherName;
                TxtNationalCode.Text = selectItem.InfoNationalCode;
                TxtCode.Text = selectItem.InfoCode;
                CboGender.SelectedIndex = selectItem.InfoGender == "0" ? 0 : 1;
                TxtBirthDay.Text = selectItem.InfoBirthDay;
                TxtBirthPlace.Text = selectItem.InfoBirthPlace;
                CboMarried.SelectedIndex = selectItem.InfoMarried == "0" ? 0 : 1;
                TxtTell.Text = selectItem.InfoTell;
                TxtMobile.Text = selectItem.InfoMobile;
                TxtEmail.Text = selectItem.InfoEmail;
                TxtPostalCode.Text = selectItem.InfoPostalCode;
                TxtAddress.Text = selectItem.InfoAddress;
                TxtJobName.Text = selectItem.InfoJobName;
                TxtJobPlaceName.Text = selectItem.InfoJobPlaceName;
                TxtJobTell.Text = selectItem.InfoJobTell;
                TxtJobFax.Text = selectItem.InfoJobFax;
                TxtJobAddress.Text = selectItem.InfoJobAddress;

                #region PersonnelImg

                if (string.IsNullOrEmpty(selectItem.InfoImage))
                {
                    ImgPersonnelImage.Source =
                        new BitmapImage(new Uri(_currentDirectory + @"\Icon\Personnel.png"));
                    InfoIMage = null;
                }
                else
                {
                    try
                    {
                        ImgPersonnelImage.Source =
                            Utility.DisplayImage($@"{_directoryPath}\Image\Personnel\Picture\" + selectItem.InfoImage);
                        InfoIMage = selectItem.InfoImage;
                    }
                    catch (Exception)
                    {
                        ImgPersonnelImage.Source =
                            new BitmapImage(new Uri(_currentDirectory + @"\Icon\Personnel.png"));
                        InfoIMage = null;
                    }
                }
                #endregion

                TxtDescription.Text = selectItem.InfoDescription;
                return false;
            }

            //taeen nadashtane meghdar va ya tekrari naboodan shomare ozviat
            try
            {
                if (!await DPersonnel.CheckPersonnelId(TxtPerId.Text))
                {
                    Utility.Message("اخطار", "شخصی با این شماره عضویت قبلا ثبت گردیده است", "Warning.png");
                    return false;
                }

                if (!await DPerAccType.CheckAccountNumber(TxtAccountNum.Text))
                {
                    Utility.Message("اخطار", "شماره حساب وارد شده تکراری است", "Warning.png");
                    return false;
                }
            }
            catch (Exception exception)
            {
                Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در دریافت اطلاعات\n" + exception.Message);
                return false;
            }
            return true;
        }

        private async Task<bool> CheckRepeatEdit(string nationalCode, string personnelId, string accountNumber)
        {
            List<tblInfo> checkNationalCode;
            try
            {
                checkNationalCode = await DInfo.CheckNationalCode(TxtNationalCode.Text);
            }
            catch (Exception exception)
            {
                Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در دریافت اطلاعات\n" + exception.Message);
                return false;
            }
            if (!(TxtNationalCode.Text == nationalCode || checkNationalCode.Count == 0))
            {
                Utility.Message("اخطار", "شخصی با این کد ملی قبلا ثبت گردیده است", "Warning.png");
                return false;
            }
            try
            {
                if (!(TxtPerId.Text == personnelId || await DPersonnel.CheckPersonnelId(TxtPerId.Text)))
                {
                    Utility.Message("اخطار", "شخصی با این شماره عضویت قبلا ثبت گردیده است", "Warning.png");
                    return false;
                }


                if (!(TxtAccountNum.Text == accountNumber ||
                      await DPerAccType.CheckAccountNumber(TxtAccountNum.Text)))
                {
                    Utility.Message("اخطار", "شماره حساب وارد شده تکراری است", "Warning.png");
                    return false;
                }
            }
            catch (Exception exception)
            {
                Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در دریافت اطلاعات\n" + exception.Message);
                return false;
            }
            return true;
        }

        private bool CheckSelect()
        {
            if (BtnAdd.IsEnabled != true || DgdPersonnel.SelectedIndex != -1) return true;
            Utility.Message("اخطار", "شخصی را انتخاب کنید", "Warning.png");
            return false;
        }

        private bool CheckCanDelete()
        {
            if (_accountData.Count != 0)
            {
                Utility.Message("خطا", "به دلیل موجود بودن سوابق مالی از این شخص قادر به حذف آن نیستید","Stop.png");
                return false;
            }

            if (_chargeMonthlyData.Count != 0)
            {
                Utility.Message("خطا", "به دلیل موجود بودن دوره ی شارژ ماهانه از این شخص قادر به حذف آن نیستید", "Stop.png");
                return false;
            }

            if (_loanData.Count != 0)
            {
                Utility.Message("خطا", "به دلیل موجود بودن سوابق وام از این شخص قادر به حذف آن نیستید",
                    "Stop.png");
                return false;
            }

            if (_guarantorData.Count != 0)
            {
                Utility.Message("خطا", "به دلیل این که این شخص ضامن شخص دیگر برای دریافت وام بوده است قادر به حذف آن نیستید", "Stop.png");
                return false;
            }

            if (_introducerData.Count != 0)
            {
                Utility.Message("خطا", "به دلیل این که این شخص معرف شخص دیگر برای دریافت وام بوده است قادر به حذف آن نیستید", "Stop.png");
                return false;
            }
            return true;
        }

        private string SelectePicture()
        {
            //Sakhtan masir zakhireh aks personnel
            if (!Directory.Exists($@"{_directoryPath}\Image\Personnel\Picture"))
                Directory.CreateDirectory($@"{_directoryPath}\Image\Personnel\Picture");
            try
            {
                //ایجاد نام تصادفی به منظور عدم ایجاد خطا در مواردی که عکس هایی با نام یکسان وجود دارند
                var randomFileName = Path.GetRandomFileName();

                //کپی عکس از مسیر اصلی در مسیر اجرای برنامه
                File.Copy(_pictureFilePath,
                    _directoryPath + @"\Image\Personnel\Picture\" + randomFileName + _pictureFileName);

                //استخراج مسیر و نام کامل عکس مورد نظر
                return randomFileName + _pictureFileName;
            }
            catch (Exception)
            {
                ImgPersonnelImage.Source =
                    new BitmapImage(new Uri(_currentDirectory + @"\Icon\Personnel.png"));
                Utility.Message("خطا", "عکس یافت نشد", "Warning.png");
                return null;
            }
        }

        private string SelecteSignature()
        {
            
            //Sakhtan masir zakhireh aks personnel
            if (!Directory.Exists($@"{_directoryPath}\Image\Personnel\Signature"))
                Directory.CreateDirectory($@"{_directoryPath}\Image\Personnel\Signature");
            try
            {
                //ایجاد نام تصادفی به منظور عدم ایجاد خطا در مواردی که عکس هایی با نام یکسان وجود دارند
                var randomFileName = Path.GetRandomFileName();

                //کپی عکس از مسیر اصلی در مسیر اجرای برنامه
                File.Copy(_signatureFilePath,
                    _directoryPath + @"\Image\Personnel\Signature\" + randomFileName + _signatureFileName);

                //استخراج مسیر و نام کامل عکس مورد نظر
                return randomFileName + _signatureFileName;
            }
            catch (Exception)
            {
                ImgSignatureImage.Source =
                    new BitmapImage(new Uri(_currentDirectory + @"\Icon\Signature.png"));
                Utility.Message("خطا", "عکس یافت نشد", "Warning.png");
                return null;
            }
        }

        #endregion

    }
}