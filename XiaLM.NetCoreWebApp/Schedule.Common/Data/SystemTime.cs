using System.Runtime.InteropServices;

namespace Schedule.Common.Data
{
    [StructLayoutAttribute(LayoutKind.Sequential)]
    public class SystemTime
    {
        public ushort vDay;
        public ushort vDayOfWeek;
        public ushort vHour;
        public ushort vMinute;
        public ushort vMonth;
        public ushort vSecond;
        public ushort vYear;
    }
}
