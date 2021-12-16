using System;
using System.Collections.Generic;
using System.Text;

namespace CBTwelveInterface
{
    public class TwelveCalendarItem
    {
        public string ID;
        public string Title;
        public DateTime StartDate;
        public string Time;
        public string Type;
        public string TypeID;

        public TwelveCalendarItemType GetItemType()
        {
            if (Type.Contains("Assignment") || Type.Contains("Ödev"))
                return TwelveCalendarItemType.ASSIGNMENT;

            return TwelveCalendarItemType.OTHER;
        }

        public enum TwelveCalendarItemType
        {
            ASSIGNMENT,
            OTHER
        }

    }

}
