using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.Utils
{
    public class DateTime
    {
        [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetSystemTime([System.Runtime.InteropServices.In] ref SYSTEMTIME st);

        [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
        public static extern void GetSystemTime([System.Runtime.InteropServices.Out] out SYSTEMTIME st);
    }

    public struct SYSTEMTIME
    {
        public short wYear;
        public short wMonth;
        public short wDayOfWeek;
        public short wDay;
        public short wHour;
        public short wMinute;
        public short wSecond;
        public short wMilliseconds;
    }

}
