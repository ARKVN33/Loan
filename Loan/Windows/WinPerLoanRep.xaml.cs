using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Loan.Class;
using Microsoft.Win32;

namespace Loan.Windows
{
    /// <summary>
    /// Interaction logic for WinPerLoanRep.xaml
    /// </summary>
    public partial class WinPerLoanRep
    {
        private OpenFileDialog _openFileDialog;
        private bool _selectedPicture;
        private string _filePath;

        public WinPerLoanRep()
        {
            InitializeComponent();
            Directory.GetCurrentDirectory();
        }
        
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

        #endregion

        private void BtnPerRep_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            var winPersonnelSearch = new WinPersonnelSearch
            {
                LblTitle = { Content = "چاپ گزارش فردی" },
                BtnPerRep = { Visibility = Visibility.Visible },
                RepPicPath = _selectedPicture ? _filePath : null,
                RepMessage = TxtMessage.Text
            };
            winPersonnelSearch.TxtSearch.Focus();
            winPersonnelSearch.ShowDialog();
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
                ImgPersonnelImage.Source = new BitmapImage(new Uri(_openFileDialog.FileName));
                //استخراج نام کامل عکس
                _filePath = Path.GetFullPath(_openFileDialog.FileName);
                _selectedPicture = true;
            }
            catch (Exception)
            {
                ImgPersonnelImage.Source = null;
                Utility.Message("اخطار", "عکس یافت نشد", "Stop.png");
            }
        }
    }
}
