using System;
using System.Data.SqlClient;
using System.IO;
using System.IO.Compression;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace Loan.Class
{
    public class BackupRestore
    {
        public string BackupPath { get; set; }
        public string DirectoryName { get; set; }
        public string ExtractPath { get; set; }
        public bool BackUpOk { get; set; }
        public bool RestoreOk { get; set; }

        public void BackUpDb()
        {
            BackUpOk = false;
            try
            {
                var sqlConnection = new SqlConnection
                {
                    ConnectionString = @"Data Source = (LocalDB)\.; DataBase = master"
                };
                sqlConnection.Open();
                
                var backupQuery = $@"BACKUP DATABASE dbLoan TO  DISK = N'{BackupPath}' WITH NOFORMAT, NOINIT,  NAME = N'dbLoan-Full Database Backup', SKIP, NOREWIND, NOUNLOAD,  STATS = 10";
                var backupSqlCommand = new SqlCommand
                {
                    CommandText = backupQuery,
                    Connection = sqlConnection
                };
                backupSqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
                try
                {
                    CheckBackupFile(BackupPath);
                }
                catch (Exception exception)
                {
                    Utility.MyMessageBox("خطا در فایل پشتیبان", exception.Message);
                    return;
                }

                var directoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    @"ARKVN\Image");
                if (File.Exists(directoryPath))
                {
                    Utility.DirectoryCopy(directoryPath, DirectoryName + @"\Image", true);
                }

                ZipFile.CreateFromDirectory(DirectoryName, DirectoryName + ".zip");
                Directory.Delete(DirectoryName, true);
                BackUpOk = true;
            }
            catch (Exception exception)
            {
                Utility.MyMessageBox("خطا در پشتیبانگیری", exception.Message);
                if (Directory.Exists(DirectoryName))
                {
                    try
                    {
                        Directory.Delete(DirectoryName, true);
                    }
                    catch (Exception exception1)
                    {
                        Utility.MyMessageBox("خطا در حذف فایل ایجاد شده", exception1.Message);
                    }
                    return;
                }
            }
            CheckZipBackupFile(DirectoryName, BackupPath);
        }

        public void RestoreDb()
        {
            RestoreOk = false;

            try
            {
                CheckBackupFile(BackupPath);
            }
            catch (Exception exception)
            {
                Utility.MyMessageBox("خطا در فایل پشتیبان", exception.Message);
                if (!Directory.Exists(ExtractPath)) return;
                try
                {
                    Directory.Delete(BackupPath, true);
                }
                catch (Exception exception1)
                {
                    Utility.MyMessageBox("خطا در حذف فایل ایجاد شده", exception1.Message);
                }
                return;
            }

            var sqlConnection = new SqlConnection
            {
                ConnectionString = @"Data Source = (LocalDB)\.; DataBase = master"
            };

            sqlConnection.Open();
            var serverConnection = new ServerConnection(sqlConnection);
            var server = new Server(serverConnection);
            var currentDatabase = server.Databases[@"dbLoan"];

            if (currentDatabase != null)
            {
                server.KillAllProcesses(@"dbLoan");
            }
            var query = $@"RESTORE DATABASE[dbLoan] FROM  DISK = N'{BackupPath}' WITH  FILE = 1,  NOUNLOAD,  STATS = 5";
            var sqlCommand = new SqlCommand
            {
                CommandText = query,
                Connection = sqlConnection
            };
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            var directoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                @"ARKVN\Image");
            if (File.Exists(directoryPath))
            {
                Directory.Delete(directoryPath, true);
                if (File.Exists(ExtractPath + @"\Image"))
                {
                    Utility.DirectoryCopy(ExtractPath + @"\Image", directoryPath, true);
                }
            }

            Directory.Delete(ExtractPath, true);
            RestoreOk = true;
        }

        private static void CheckBackupFile(string backupPath)
        {

            var sqlConnection = new SqlConnection
            {
                ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Database=dbLoan;Integrated Security=True;Connect Timeout=30"
            };
            sqlConnection.Open();

            var checkBackupQuery = $"RESTORE VERIFYONLY FROM DISK = '{backupPath}'";
            var checkBackupSqlCommand = new SqlCommand
            {
                CommandText = checkBackupQuery,
                Connection = sqlConnection
            };
            checkBackupSqlCommand.ExecuteNonQuery();

            sqlConnection.Close();
        }

        private static void CheckZipBackupFile(string directoryName, string backupPath)
        {
            try
            {
                Directory.CreateDirectory(directoryName);

                ZipFile.ExtractToDirectory(directoryName + ".zip", directoryName);

                var sqlConnection = new SqlConnection
                {
                    ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Database=dbLoan;Integrated Security=True;Connect Timeout=30"
                };
                sqlConnection.Open();

                var checkBackupQuery = $"RESTORE VERIFYONLY FROM DISK = '{backupPath}'";
                var checkBackupSqlCommand = new SqlCommand
                {
                    CommandText = checkBackupQuery,
                    Connection = sqlConnection
                };
                checkBackupSqlCommand.ExecuteNonQuery();

                sqlConnection.Close();

                Directory.Delete(directoryName, true);

            }
            catch (Exception exception)
            {
                Utility.MyMessageBox("خطا در فایل پشتیبان",
                    "فایل پشتیبان پس از فشرده سازی دچار مشکل شده است لطفا نرم افزار فشرده سازی خود را چک کنید.\n توجه داشته باشید که فایل پشتیبان ایجاد شده به درستی کار نمیکند در صورت بازنشانی فایل پشتیبان دچار خطا خواهید شد\n" +
                    exception.Message);
                if (Directory.Exists(directoryName))
                {
                    try
                    {
                        Directory.Delete(directoryName, true);
                    }
                    catch (Exception exception1)
                    {
                        Utility.MyMessageBox("خطا در حذف فایل ایجاد شده", exception1.Message);
                    }
                }
            }
        }
    }
}
