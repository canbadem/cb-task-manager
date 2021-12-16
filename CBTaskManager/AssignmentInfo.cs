using CBTwelveInterface;
using Newtonsoft.Json;
using System;

namespace CBTaskManager
{
    class AssignmentInfo
    {
        public Guid AssignmentID;
        public string Title;
        public DateTime StartDate;
        public DateTime DueDate;

        [JsonConstructor]
        public AssignmentInfo(Guid assignmentID, string title, DateTime startDate, DateTime dueDate)
        {
            AssignmentID = assignmentID;
            Title = title;
            StartDate = startDate;
            DueDate = dueDate;
        }

        public AssignmentInfo(TwelveAssignment ta) : this(ta.ID, ta.Title, ta.StartDate, ta.DueDate) { }

    }
}
