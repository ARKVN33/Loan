using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DAL;
using DAL.Class;
using Loan.Class;
using Stimulsoft.Report;
using Stimulsoft.Report.Dictionary;

namespace Loan.Windows
{
    /// <summary>
    /// Interaction logic for WinInfoSearch.xaml
    /// </summary>
    public partial class WinInfoSearch
    {
        private List<tblInfo> _infoData;
        private List<tblInfo> _infoSearchData;

        public WinInfoSearch()
        {
            InitializeComponent();
            _infoData = new List<tblInfo>();
            _infoSearchData = new List<tblInfo>();
        }
        #region Properties

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
            DgdInfo.ItemsSource = _infoSearchData;
        }

        public void BtnSelect_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckSelect()) return;
            var selectItem = _infoSearchData[DgdInfo.SelectedIndex];
            Id = selectItem.Id;
            FirstName = selectItem.InfoFirstName;
            LastName = selectItem.InfoLastName;
            NationalCode = selectItem.InfoNationalCode;
            Close();
        }

        private void DgdInfo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DgdInfo.SelectedIndex == -1) return;

            var selectItem = _infoSearchData[DgdInfo.SelectedIndex];
            LblInfoFirstName.Content = selectItem.InfoFirstName;
            LblInfoLastName.Content = selectItem.InfoLastName;
            LblInfoNationalCode.Content = selectItem.InfoNationalCode;
        }

        private void TxtInfoSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            LblInfoFirstName.Content = string.Empty;
            LblInfoLastName.Content = string.Empty;
            LblInfoNationalCode.Content = string.Empty;

            var search = TxtInfoSearch.Text;
            _infoSearchData = _infoData;
            _infoSearchData =
                _infoSearchData.FindAll(
                    t =>
                        (!string.IsNullOrEmpty(t.InfoFirstName) && t.InfoFirstName.Contains(search)) ||
                        (!string.IsNullOrEmpty(t.InfoLastName) && t.InfoLastName.Contains(search)) ||
                        (!string.IsNullOrEmpty(t.InfoNationalCode) && t.InfoNationalCode.Contains(search)) ||
                        (!string.IsNullOrEmpty(t.InfoCode) && t.InfoCode.Contains(search)) ||
                        (!string.IsNullOrEmpty(t.InfoMobile) && t.InfoMobile.Contains(search)) ||
                        (!string.IsNullOrEmpty(t.InfoTell) && t.InfoTell.Contains(search)) ||
                        (!string.IsNullOrEmpty(t.InfoPostalCode) && t.InfoPostalCode.Contains(search)) ||
                        (!string.IsNullOrEmpty(t.InfoAddress) && t.InfoAddress.Contains(search)));
            DgdInfo.ItemsSource = _infoSearchData;
        }

        private void BtnInfoRep_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckSelect()) return;
            var selectItem = _infoSearchData[DgdInfo.SelectedIndex];
            var report = new StiReport();
            report.Load("Report//ReportForPerson2.mrt");
            report.Dictionary.Databases.Clear();
            report.Dictionary.Databases.Add(new StiSqlDatabase("RepAllPer", @"Data Source=(LocalDB)\MSSQLLocalDB;Database=dbLoan;Integrated Security=True;Connect Timeout=30"));
            report.Dictionary.Variables.Add(new StiVariable("ShamsiDate",
                PersianDateTime.Now.Date.ToString("yyyy/MM/dd")));
            report.Dictionary.Variables.Add(new StiVariable("TimeNow", PersianDateTime.Now.TimeOfDay.ToHHMMSS()));
            report.Dictionary.Variables.Add(new StiVariable("programPath", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                @"ARKVN\Image\Personnel\Picture\")));
            report.Dictionary.Variables.Add(new StiVariable("InfoId", selectItem.Id));
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

        #endregion

        #region Method

        private bool CheckSelect()
        {
            if (DgdInfo.SelectedIndex == -1)
            {
                Utility.Message("خطا", "شخصی را انتخاب کنید", "Stop.png");
                return false;
            }

            return true;
        }



        #endregion


    }
}
