using System;
using System.IO;
using System.Management;
using System.Net.NetworkInformation;

namespace Loan.Class
{
    public static class SerialNumber
    {

        public static string GetHardwareSerial()
        {

            string cpuSerial = null, hardSerial, macAddresses = null;
            //cpu
            try
            {
                var mc = new ManagementClass("win32_processor");
                var moc = mc.GetInstances();
                foreach (var o in moc)
                {
                    var mo = (ManagementObject)o;
                    cpuSerial = mo.Properties["processorID"].Value.ToString();
                    break;
                }
            }
            catch
            {
                cpuSerial = string.Empty;
            }
            //hard
            try
            {
                var drive = Path.GetPathRoot(Environment.SystemDirectory).Split(':')[0];
                var dsk = new ManagementObject(@"win32_logicaldisk.deviceid=""" + drive + @":""");
                dsk.Get();
                hardSerial = dsk["VolumeSerialNumber"].ToString();
            }
            catch
            {
                hardSerial = string.Empty;
            }
            //mac
            try
            {
                foreach (var nic in NetworkInterface.GetAllNetworkInterfaces())
                {
                    macAddresses = nic.GetPhysicalAddress().ToString();
                    break;
                }
            }
            catch (Exception)
            {
                macAddresses = string.Empty;
            }

            if (!string.IsNullOrEmpty(cpuSerial))
            {
                cpuSerial += "-";
            }
            if (!string.IsNullOrEmpty(hardSerial))
            {
                hardSerial += "-";
            }
            return cpuSerial + hardSerial + macAddresses;
        }
    }
}
