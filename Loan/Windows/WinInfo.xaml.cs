using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using DAL;
using DAL.Class;
using Loan.Class;
using Clipboard = System.Windows.Clipboard;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;

namespace Loan.Windows
{
    /// <summary>
    /// Interaction logic for WinInfo.xaml
    /// </summary>
    public partial class WinInfo
    {
        private OpenFileDialog _openFileDialog;
        private bool _selectedPicture;
        private string _fileName;
        private string _filePath;
        private readonly string _currentDirectory;
        private readonly string _directoryPath;
        private List<tblInfo> _infoData;
        private List<tblInfo> _infoSearchData;
        private InputLanguage _currentInputLanguage;

        public WinInfo()
        {
            InitializeComponent();
            _currentDirectory = Directory.GetCurrentDirectory();//estekhrag masir jari ejra barnameh
            _directoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "ARKVN");
            _infoData = new List<tblInfo>();
            _infoSearchData = new List<tblInfo>();
        }
        #region Properties

        public bool ConfirmAdd { get; set; }

        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string NationalCode { get; set; }

        #endregion

        #region Event

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _infoData = await DInfo.GetData();
            }
            catch (Exception exception)
            {
                Utility.MyMessageBox("خطا در بانک اطلاعاتی", "خطا در دریافت اطلاعات\n" + exception.Message);
                Close();
                return;
            }
            _infoSearchData = _infoData;
            DgdPersonnel.ItemsSource = _infoSearchData;

            BtnNew_Click(null, null);
        }

        private async void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckEmpty() || !Utility.CheckNationalCode(TxtNationalCode.Text) ||
                !Utility.CheckEmail(TxtEmail.Text) || !await CheckRepeatAdd()) return;

            #region AddInfo

            int infoId;
            try
            {
                var addInfo = new DInfo
                {
                    DInfoFirstName = TxtFirstName.Text,
                    DInfoLastName = TxtLastName.Text,
                    DInfoFatherName =
                        TxtFatherName.Text.Trim() == string.Empty ? null : TxtFatherName.Text,
                    DInfoGender = CboGender.SelectedIndex.ToString(),
                    DInfoNationalCode =
                        TxtNationalCode.Text.Trim() == string.Empty ? null : TxtNationalCode.Text,
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

            ConfirmAdd = true;
            Id = infoId;
            FirstName = TxtFirstName.Text;
            LastName = TxtLastName.Text;
            NationalCode = TxtNationalCode.Text;
            Close();
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
                _fileName = Path.GetFileName(_openFileDialog.FileName);
                _filePath = Path.GetFullPath(_openFileDialog.FileName);
                _selectedPicture = true;
            }
            catch (Exception)
            {
                ImgPersonnelImage.Source = new BitmapImage(new Uri(_currentDirectory + @"\Icon\Perssonel.png"));
                Utility.Message("اخطار", "عکس یافت نشد", "Stop.png");
            }
        }

        private void BtnNew_Click(object sender, RoutedEventArgs e)
        {
            if (TxtSearch.Visibility == Visibility.Visible)
            {
                TxtSearch.Focus();
            }
            else
            {
                TxtFirstName.Focus();
            }
            _selectedPicture = false;
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
            TxtDescription.Text = string.Empty;
            ImgPersonnelImage.Source = new BitmapImage(new Uri(_currentDirectory + @"\Icon\Personnel.png"));

        }

        private void DgdPersonnel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DgdPersonnel.SelectedIndex == -1) return;

            var selectItem = _infoData[DgdPersonnel.SelectedIndex];
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

            TxtDescription.Text = selectItem.InfoDescription;
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            var search = TxtSearch.Text;
            _infoSearchData = _infoData;
            _infoSearchData =
                _infoSearchData.FindAll(
                     t =>
                        !string.IsNullOrEmpty(t.InfoFirstName) && t.InfoFirstName.Contains(search) ||
                        !string.IsNullOrEmpty(t.InfoLastName) && t.InfoLastName.Contains(search) ||
                        !string.IsNullOrEmpty(t.InfoNationalCode) && t.InfoNationalCode.Contains(search) ||
                        !string.IsNullOrEmpty(t.InfoCode) && t.InfoCode.Contains(search) ||
                        !string.IsNullOrEmpty(t.InfoMobile) && t.InfoMobile.Contains(search) ||
                        !string.IsNullOrEmpty(t.InfoTell) && t.InfoTell.Contains(search) ||
                        !string.IsNullOrEmpty(t.InfoPostalCode) && t.InfoPostalCode.Contains(search) ||
                        !string.IsNullOrEmpty(t.InfoAddress) && t.InfoAddress.Contains(search));

            DgdPersonnel.ItemsSource = _infoSearchData;
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

        private void EnglishInput(object sender, KeyEventArgs e)
        {
            var language = new CultureInfo("en-US");
            InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(language);
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

        private bool CheckEmpty()
        {

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
            //taeen tekrari naboodan cod meli va ya nadashtane meghdar
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
            if (checkNationalCode.Count != 0)
            {
                Utility.Message("اخطار", "شخصی با این کد ملی قبلا ثبت گردیده است", "Warning.png");
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
                File.Copy(_filePath,
                    _directoryPath + @"\Image\Personnel\Picture\" + randomFileName + _fileName);

                //استخراج مسیر و نام کامل عکس مورد نظر
                return randomFileName + _fileName;
            }
            catch (Exception)
            {
                ImgPersonnelImage.Source =
                    new BitmapImage(new Uri(_currentDirectory + @"\Icon\Personnel.png"));
                Utility.Message("خطا", "عکس یافت نشد", "Warning.png");
                return null;
            }
        }


        #endregion


    }
}
