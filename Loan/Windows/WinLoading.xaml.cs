using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using DAL.Class;
using Loan.Class;

namespace Loan.Windows
{
    /// <summary>
    /// Interaction logic for Loading.xaml
    /// </summary>
    public partial class WinLoading
    {
        public DispatcherTimer DispatcherTimer;
        public bool OkDbChange;
        public bool OkTime;
        public bool ShowWinLicense;
        public bool OkLogin;
        public bool OkShutdownApp;

        private readonly string _directoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),"ARKVN");

        public WinLoading()
        {
            InitializeComponent();
            DispatcherTimer = new DispatcherTimer();
            OkDbChange = false;
            OkTime = false;
            ShowWinLicense = false;
            OkLogin = false;
            OkShutdownApp = false;
            if (!IsRunningProject()) return;
            Utility.MyMessageBox("خطا","نرم افزار درحال اجرا است");
            Application.Current.Shutdown();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DispatcherTimer.Tick += dispatcherTimer_Tick;
            DispatcherTimer.Interval = new TimeSpan(0, 0, 0, 10);
            DispatcherTimer.Start();
            await Task.Run(() => new LocalDbConfiguration().Configurate());
            await Task.Run(() => new LoanDbChanges().Configurate());

            var licenseData = (await DLicense.GetData())[0].AppLicense;
            var serialNumber = await Task.Run(() => SerialNumber.GetHardwareSerial());

            var salt = new SaltyPasswordHashing
            {
                MaxHashSize = 40,
                SaltSize = 0
            };

            if (!await Task.Run(() => salt.VerifyHash(serialNumber, licenseData)))
            {
                var checkTime = await Task.Run(CheckWinTimeUpdate);
                var checkTrial = await Task.Run(Check100DayTrial);

                if ((await DLicense.GetData())[0].AppVersion == null)
                {
                    var dLicense = new DLicense
                    {
                        DAppLicense = licenseData,
                        DAppVersion = "1.1.0"
                    };
                    await Task.Run(() => dLicense.Edit());
                }

                if (!checkTime || !checkTrial)
                {
                    if (OkTime && OkShutdownApp)
                    {
                        OkDbChange = true;
                        Application.Current.Shutdown();
                    }
                    if (OkTime && !OkShutdownApp)
                    {
                        var winLicense = new WinLicense();
                        OkDbChange = true;
                        winLicense.Show();
                        Close();
                    }
                    else
                    {
                        ShowWinLicense = true;
                    }
                    return;
                }
                
                if (OkTime)
                {
                    var winLogin = new WinLogin();
                    OkDbChange = true;
                    winLogin.Show();
                    Close();
                }
                else
                {
                    OkLogin = true;
                }
            }
            else
            {
                if (OkTime)
                {
                    var winLogin = new WinLogin();
                    OkDbChange = true;
                    winLogin.Show();
                    Close();
                }
                else
                {
                    OkLogin = true;
                }
            }
        }

        public async Task<bool> CheckWinTimeUpdate()
        {
            var appVersion = (await DLicense.GetData())[0].AppVersion;
            var filePath = _directoryPath + @"\IntTi.dat";
            var existsIntTi = File.Exists(filePath);
            if ((appVersion == null || !existsIntTi) && (appVersion != null || existsIntTi)) return false;

            string internetTime;
            bool okInternetTime;
            try
            {
                internetTime = new PersianDateTime(GetNistTime()).ToString(PersianDateTimeFormat.DateTime);
                okInternetTime = true;
            }
            catch (Exception)
            {
                internetTime = string.Empty;
                okInternetTime = false;
            }

            if (appVersion == null)
            {
                if (internetTime == string.Empty)
                {
                    internetTime = "1396/08/20 00:00:00";
                }

                try
                {
                    using (var fileStream = new FileStream(filePath, FileMode.OpenOrCreate))
                    using (var streamWriter = new StreamWriter(fileStream))
                    {
                        streamWriter.WriteLine(StringCipher.Encrypt(internetTime, "RAYMON33"));
                        streamWriter.Close();
                    }
                }
                catch (Exception)
                {
                    return false;
                }
            }

            var intTiData = string.Empty;
            foreach (var line in File.ReadLines(filePath))
            {
                try
                {
                    intTiData = StringCipher.Decrypt(line, "RAYMON33");
                }
                catch (Exception)
                {
                    return false;
                }
            }

            if (internetTime == string.Empty)
            {
                internetTime = intTiData;
            }
            else
            {

                try
                {
                    EditFile(StringCipher.Encrypt(internetTime, "RAYMON33"), filePath, 1);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            var persianDateTime = PersianDateTime.Now.ToString(PersianDateTimeFormat.DateTime);
            var dPersianDateTime =
                PersianDateTime.Parse(persianDateTime.Split(' ')[0], persianDateTime.Split(' ')[1]);
            var dInternetTime = PersianDateTime.Parse(internetTime.Split(' ')[0], internetTime.Split(' ')[1]);
            if (okInternetTime && appVersion != null)
            {

                if (dPersianDateTime <= dInternetTime.AddMinutes(-30))
                {
                    var timeSpan = dInternetTime - dPersianDateTime;
                    Utility.MyMessageBox("خطا",
                        $"تاریخ و ساعت سیستم شما\nتقریبا {timeSpan.Days} روز و {timeSpan.Hours} ساعت و {timeSpan.Minutes} دقیقه عقب میباشد\nبرای اجرای برنامه لطفا تاریخ سیستم را بروزرسانی کنید");
                    OkShutdownApp = true;
                    return false;
                }
                if (dPersianDateTime >= dInternetTime.AddMinutes(30))
                {
                    var timeSpan = dPersianDateTime - dInternetTime;
                    Utility.MyMessageBox("خطا",
                        $"تاریخ و ساعت سیستم شما\nتقریبا {timeSpan.Days} روز و {timeSpan.Hours} ساعت و {timeSpan.Minutes} دقیقه جلو میباشد\nبرای اجرای برنامه لطفا تاریخ سیستم را بروزرسانی کنید");
                    OkShutdownApp = true;
                    return false;
                }
            }
            else if (dPersianDateTime <= dInternetTime.AddMinutes(-30) && appVersion != null)
            {
                Utility.MyMessageBox("خطا",
                    "تاریخ و ساعت سیستم بروز نمی باشد\nبرای اجرا شدن برنامه لطفا تاریخ سیستم را بروزرسانی کنید");
                OkShutdownApp = true;
                return false;
            }
            return true;
        }

        public async Task<bool> Check100DayTrial()
        {
            var filePath = _directoryPath + @"\VeryImportant.dat";

            var persianDate = PersianDateTime.Now.ToString(PersianDateTimeFormat.DateTime);
            var appVersion = (await DLicense.GetData())[0].AppVersion;
            var existsFilePath = File.Exists(filePath);

            if ((appVersion == null || !existsFilePath) && (appVersion != null || existsFilePath)) return false;

            if (appVersion == null)
            {
                try
                {
                    using (var fileStream = new FileStream(filePath, FileMode.OpenOrCreate))
                    using (var streamWriter = new StreamWriter(fileStream))
                    {
                        streamWriter.WriteLine(StringCipher.Encrypt(persianDate, "RAYMON33"));
                        streamWriter.WriteLine(StringCipher.Encrypt(persianDate, "RAYMON33"));
                        streamWriter.Close();
                    }
                }
                catch (Exception)
                {
                    return false;
                }
            }

            if (File.Exists(filePath))
            {
                var i = 0;
                var saveDate = new string[2];
                foreach (var line in File.ReadLines(filePath))
                {
                    try
                    {
                        saveDate[i] = StringCipher.Decrypt(line, "RAYMON33");
                        i++;
                    }

                    catch (Exception)
                    {
                        return false;
                    }
                }
                var dPersianDate = PersianDateTime.Parse(persianDate.Split(' ')[0], persianDate.Split(' ')[1]);
                var dSaveDate0 = PersianDateTime.Parse(saveDate[0].Split(' ')[0], saveDate[0].Split(' ')[1]);
                var dSaveDate1 = PersianDateTime.Parse(saveDate[1].Split(' ')[0], saveDate[1].Split(' ')[1]);

                if (dPersianDate < dSaveDate1)
                {
                    Utility.MyMessageBox("خطا", "تاریخ سیستم بروز نمی باشد\nبرای اجرای برنامه لطفا تاریخ سیستم را بروزرسانی کنید");
                    OkShutdownApp = true;
                    return false;
                }

                try
                {
                    EditFile(StringCipher.Encrypt(persianDate, "RAYMON33"), filePath, 2);
                }
                catch (Exception)
                {
                    return false;
                }

                if ((dPersianDate - dSaveDate0).Days >= 60)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
            return true;
        }

        public static DateTime GetNistTime()
        {
            DateTime localDateTime;
            var client = new TcpClient("time.nist.gov", 13);
            using (var streamReader = new StreamReader(client.GetStream()))
            {
                var response = streamReader.ReadToEnd();
                var utcDateTimeString = response.Substring(7, 17);
                localDateTime = DateTime.ParseExact(utcDateTimeString, "yy-MM-dd HH:mm:ss",
                    CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
            }
            return localDateTime;
        }

        private static void EditFile(string newText, string fileName, int lineToEdit)
        {
                var arrLine = File.ReadAllLines(fileName);
                arrLine[lineToEdit - 1] = newText;
                File.WriteAllLines(fileName, arrLine);
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            OkTime = true;
            DispatcherTimer.Stop();
            if (ShowWinLicense)
            {
                var winLicense = new WinLicense();
                OkDbChange = true;
                winLicense.Show();
                Close();
            }
            if (OkLogin)
            {
                var winLogin = new WinLogin();
                OkDbChange = true;
                winLogin.Show();
                Close();
            }
            if (OkShutdownApp)
            {
                OkDbChange = true;
                Application.Current.Shutdown();
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = !OkDbChange || !OkTime;
        }

        private static bool IsRunningProject()
        {
            var processName = Process.GetCurrentProcess().ProcessName;
            var instances = Process.GetProcessesByName(processName);
            return instances.Length > 1;
        }
    }
}
