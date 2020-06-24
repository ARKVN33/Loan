using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Loan.Class;
using Path = System.IO.Path;

namespace Loan.Windows
{
    /// <summary>
    /// Interaction logic for WinWait.xaml
    /// </summary>
    public partial class WinWait
    {
        public bool OkBackUp { get; set; }
        public bool OkAutoBackUp { get; set; }
        public bool OkRestore { get; set; }
        public bool CloseOk { get; set; }
        public string DirectoryName { get; set; }
        public string FileName { get; set; }
        public string WExtractPath { get; set; }
        public WinWait()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (OkBackUp)
            {
                await Task.Run(() => Backup());
                if (OkBackUp)
                {
                    Utility.Message("پیام", "پشتیبان گیری با موفقیت انجام شد", "Correct.png");
                }
            }

            if (OkAutoBackUp)
            {
                await Task.Run(() => CopyAutoBakup());
                if (OkAutoBackUp)
                {
                    Utility.Message("پیام", "فایل های پشتیبان با موفقیت در مسیر مورد نظر ذخیره شدند", "Correct.png");
                }
            }

            if (OkRestore)
            {
                await Task.Run(() => Restore());
                if (OkRestore)
                {
                    Utility.Message("پیام", "بازنشانی اطلاعات با موفقیت انجام شد", "Correct.png");
                }
            }
            Close();
        }

        public void Backup()
        {
            if (CloseOk)
            {
                DeleteOldestAutoBackup();
            }
            var backup = new BackupRestore
            {
                DirectoryName = DirectoryName,
                BackupPath = DirectoryName + "\\" + FileName + ".Bak"
            };
            backup.BackUpDb();
            OkBackUp = backup.BackUpOk;
        }

        public void Restore()
        {
            var restore = new BackupRestore
            {
                ExtractPath = WExtractPath,
                // ReSharper disable once AssignNullToNotNullAttribute
                BackupPath = Path.Combine(WExtractPath, Path.GetFileNameWithoutExtension(FileName)) + ".Bak"
            };
            restore.RestoreDb();
            OkRestore = restore.RestoreOk;
        }

        private void DeleteOldestAutoBackup()
        {
            var directoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                @"ARKVN\BackUp\");
            var dir = new DirectoryInfo(directoryPath);
            var count = dir.GetFiles().Length;
            if (count < 30) return;
            var autoBackUp = Directory.GetFiles(directoryPath, "*.zip").Select(Path.GetFileName).ToArray();
            Array.Sort(autoBackUp);
            File.Delete(directoryPath + autoBackUp[0]);
        }

        private void CopyAutoBakup()
        {
            try
            {
                var directoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    @"ARKVN\BackUp\");
                if (Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(DirectoryName + @"\BackUp");
                    Utility.DirectoryCopy(directoryPath, DirectoryName + @"\BackUp", true);
                    OkAutoBackUp = true;
                }
                else
                {
                    OkAutoBackUp = false;
                    Utility.MyMessageBox("خطا", "فایل پشتیبان پیدا نشد");
                }
                
            }
            catch (Exception exception)
            {
                OkAutoBackUp = false;
                Utility.MyMessageBox("خطا", "خطا در ذخیره فایل های پشتیبان" + "\n" + exception);
            }
        }
    }
}
