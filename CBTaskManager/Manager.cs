using CBTwelveInterface;
using Google.Apis.Tasks.v1.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CBTaskManager
{
    class Manager
    {

        public AssignmentManager AssignmentManager;
        public K12Handler TwelveHandler;
        public GoogleTasksManager GoogleTasksManager;
        public Scheduler Scheduler;

        public async System.Threading.Tasks.Task Run()
        {
            Scheduler = new Scheduler();
            GoogleTasksManager = new GoogleTasksManager();
            AssignmentManager = new AssignmentManager();
            TwelveHandler = new K12Handler();

            await TwelveHandler.Login();

            List<TwelveAssignment> acquiredAssignments = await TwelveHandler.GetAssignments(true);

            List<TwelveAssignment> assignmentsToSync = new List<TwelveAssignment>();

            foreach (TwelveAssignment ta in acquiredAssignments)
            {
                //Already synced.
                if (AssignmentManager.Assignments.ContainsKey(ta.ID))
                    continue;

                Console.WriteLine("Unsynced assignment found: " + ta.ID);
                assignmentsToSync.Add(ta);
                AssignmentManager.AddAssignment(new AssignmentInfo(ta));
            }

            if (assignmentsToSync.Count == 0)
            {
                Console.WriteLine("No new assignments to be synced were found.");
                Console.WriteLine("Routine completed.");
                return;
            }

            GoogleTasksManager.Login();

            foreach (TwelveAssignment ta in assignmentsToSync)
            {
                Google.Apis.Tasks.v1.Data.Task task = new Google.Apis.Tasks.v1.Data.Task()
                {
                    Title = ta.Title,
                    Notes = ConstructAssignmentDescription(ta),
                    Due = JsonConvert.SerializeObject(ta.DueDate).Replace('"', ' ').Trim()
                };
                Console.WriteLine("Google Tasks, Adding new task. " + task.Title);
                GoogleTasksManager.AddNewTask(task);
            }


            AssignmentManager.SaveAssignments();
            Console.WriteLine("Saved assignment database.");
            Console.WriteLine("Routine completed.");

        }

        public string ConstructAssignmentDescription(TwelveAssignment ta)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(Utils.CleanAssignmentDescription(ta.Preamble).Trim());
            sb.AppendLine();
            sb.AppendLine("CBTM " + DateTime.Now.ToString("dd/MM HH:mm"));

            return sb.ToString();
        }

    }
}
