using System;
using System.Collections.Generic;
using System.Text;

namespace CBTwelveInterface
{
    public class TwelveAssignmentPersonal
    {
        public string AssignmentID;
        public string StudentPersonalID;
        public string SectionInfoID;
        public string StatusID;
        public TwelveAssignment Assignment;

        public TwelveAssignmentPersonalStatus GetStatus()
        {
            if (StatusID == null)
                return TwelveAssignmentPersonalStatus.UNKNOWN;

            if (StatusID.Length == 0)
                return TwelveAssignmentPersonalStatus.UNKNOWN;

            switch (StatusID)
            {
                case "987b889e-8559-dd11-a2b7-001d601af6b7":
                    return TwelveAssignmentPersonalStatus.YAPMADI;
                case "b6a21926-6616-df11-92a3-002219a888e2":
                    return TwelveAssignmentPersonalStatus.GETIRMEDI;
                case "927b889e-8559-dd11-a2b7-001d601af6b7":
                    return TwelveAssignmentPersonalStatus.YAPTI;
                case "937b889e-8559-dd11-a2b7-001d601af6b7":
                    return TwelveAssignmentPersonalStatus.EKSIK;
                default:
                    return TwelveAssignmentPersonalStatus.UNKNOWN;
            }

            
        }

        public enum TwelveAssignmentPersonalStatus
        {
            YAPTI,
            YAPMADI,
            GETIRMEDI,
            EKSIK,
            UNKNOWN
        }

    }
}
