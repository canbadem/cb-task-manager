using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace CBTaskManager
{
    class AssignmentManager
    {

        public string AssignmentsJsonPath = "assignments.json";

        public Dictionary<Guid, AssignmentInfo> Assignments = new Dictionary<Guid, AssignmentInfo>();

        public AssignmentManager()
        {
            AssignmentsJsonPath = AppContext.BaseDirectory + "\\assignments.json";

            LoadAssignments();
        }

        public void AddAssignment(AssignmentInfo ass)
        {
            Assignments.TryAdd(ass.AssignmentID, ass);
        }

        public void LoadAssignments()
        {
            if (!File.Exists(AssignmentsJsonPath))
                return;

            string json = File.ReadAllText(AssignmentsJsonPath);
            Assignments = JsonConvert.DeserializeObject<Dictionary<Guid, AssignmentInfo>>(json);
        }

        public void SaveAssignments()
        {
            using (StreamWriter file = File.CreateText(AssignmentsJsonPath))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Formatting = Formatting.Indented;
                serializer.Serialize(file, Assignments);
            }
        }

    }
}
