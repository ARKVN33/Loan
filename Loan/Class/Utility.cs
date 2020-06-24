using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media.Imaging;
using Loan.Windows;

namespace Loan.Class
{
    public static class Utility
    {
        public static bool Ok { get; set; }
        public static bool YesNo { get; set; }

        public static void Message(string title, string message, string path)
        {
            var notifyMessage = new WinNotifyMessage();
            notifyMessage.Notify(title, message, path);
            notifyMessage.ShowInTaskbar = false;
            notifyMessage.Show();
        }

        public static void MyMessageBox(string title, string message, string messageImage = "Stop.png",
            bool ok = true)
        {
            var myMessageBox = new WinMessageBox
            {
                Message = message,
                MessageTitle = title,
                MessageImage = messageImage
            };
            if (!ok)
            {
                myMessageBox.BtnOk.Visibility = Visibility.Hidden;
                myMessageBox.BtnYes.Visibility = Visibility.Visible;
                myMessageBox.BtnNo.Visibility = Visibility.Visible;

            }

            myMessageBox.ShowDialog();
            YesNo = myMessageBox.YesNo;
            Ok = myMessageBox.Ok;
        }

        public static bool CheckEmail(string email)
        {
            var regex = new Regex(@"^\S+@\S+\.\S+$");
            if (regex.IsMatch(email) || email == string.Empty)
            {
                return true;
            }
            Message("خطا", "لطفا آدرس ایمیل را به درستی وارد کنید", "Stop.png");
            return false;
        }

        public static bool CheckNationalCode(string nationalCode)
        {
            if (nationalCode == string.Empty) return true;

            try
            {
                var chArray = nationalCode.ToCharArray();
                var numArray = new int[chArray.Length];
                for (var i = 0; i < chArray.Length; i++)
                {
                    numArray[i] = (int)char.GetNumericValue(chArray[i]);
                }
                var num2 = numArray[9];
                var num3 = numArray[0] * 10 + numArray[1] * 9 + numArray[2] * 8 + numArray[3] * 7 + numArray[4] * 6 +
                           numArray[5] * 5 + numArray[6] * 4 + numArray[7] * 3 + numArray[8] * 2;
                var num4 = num3 - num3 / 11 * 11;
                if (nationalCode == "0000000000" || nationalCode == "1111111111" ||
                    nationalCode == "2222222222" || nationalCode == "3333333333" ||
                    nationalCode == "4444444444" || nationalCode == "5555555555" ||
                    nationalCode == "6666666666" || nationalCode == "7777777777" ||
                    nationalCode == "8888888888" || nationalCode == "9999999999")
                {
                    Message("خطا", "یک کد ملی صحیح وارد کنید", "Warning.png");
                    return false;
                }

                if (num4 == 0 && num2 == num4 || num4 == 1 && num2 == 1 ||
                    num4 > 1 && num2 == Math.Abs(num4 - 11))
                {
                    return true;
                }
                Message("خطا", "کد ملی نامعتبر است", "Stop.png");
                return false;
            }
            catch (Exception)
            {
                Message("خطا", "کد ملی باید 10 رقم باشد", "Stop.png");
                return false;
            }
        }

        public static bool CheckDate(string date)
        {
            var currectDate = CurrectDate(date);
            try
            {
                PersianDateTime.Parse(currectDate);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            //DateTime a;
            //return DateTime.TryParse(date, out a);
        }

        public static string CurrectDate(string date)
        {
            var currectDate = string.Empty;
            //استفاه از تابع برای شکستن متن
            if (string.IsNullOrEmpty(date)) return null;
            var authors = date.Split('/');
            if (authors.Length != 3) return null;
            //پیمایش آرایه برای نشان دادن در خروجی
            for (var i = 0; i < authors.Length; i++)
            {
                if (authors[i] == string.Empty) return null;
                currectDate += authors[i].Length > 1
                    ? (i == 2 ? authors[i] : authors[i] + "/")
                    : (i == 2 ? "0" + authors[i] : "0" + authors[i] + "/");

            }
            return currectDate;
        }

        public static string PersianNum(this string inputString)
        {
            var arabicDigits = CultureInfo.GetCultureInfo("fa-IR").NumberFormat.NativeDigits;
            var arabicNumberBuilder = new StringBuilder();
            foreach (var c in inputString)
            {
                if (char.IsDigit(c))
                    arabicNumberBuilder.Append(arabicDigits[int.Parse(c.ToString())]);
                else
                    arabicNumberBuilder.Append(c);
            }
            return arabicNumberBuilder.ToString();
        }

        public static BitmapImage DisplayImage(string uri)
        {
            var image = new BitmapImage();
            using (var stream = File.OpenRead(uri))
            {
                image.BeginInit();
                image.StreamSource = stream;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.EndInit(); // load the image from the stream
            }
            return image;
        }

        public static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            var dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);

            var dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
                Directory.CreateDirectory(destDirName);

            // Get the files in the directory and copy them to the new location.
            var files = dir.GetFiles();
            foreach (var file in files)
            {
                var temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (!copySubDirs) return;
            foreach (var subdir in dirs)
            {
                var temppath = Path.Combine(destDirName, subdir.Name);
                DirectoryCopy(subdir.FullName, temppath, true);
            }
        }
    }
}
