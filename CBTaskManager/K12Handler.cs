using CBTwelveInterface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CBTaskManager
{
    class K12Handler
    {

        public TwelveCredentials Credentials;

        public TwelveUserInfo UserInfo;
        public TwelveStudentPersonal StudentPersonal;
        public TwelveUser User;
        public TwelveStudentSchoolEnrollment Enrollment;

        public string CredentialsPath = "TwelveCredentials.json";

        public K12Handler()
        {

            CredentialsPath = AppContext.BaseDirectory + "\\TwelveCredentials.json";
            Console.WriteLine(CredentialsPath);
            if (File.Exists(CredentialsPath))
            {
                string json = File.ReadAllText(CredentialsPath);
                Credentials = JsonConvert.DeserializeObject<TwelveCredentials>(json);
            }
            else
            {
                Credentials = new TwelveCredentials("Username", "Password");
                using (StreamWriter file = File.CreateText(CredentialsPath))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Formatting = Formatting.Indented;
                    serializer.Serialize(file, Credentials);
                }
            }

        }

        public async Task Login()
        {
            Console.WriteLine("Twelve API initializing..");
            CBTwelveInterface.TwelveAPI.Initialize();

            Console.WriteLine("K12 Logging in..");
            await TwelveAPI.PostLoginRequest(Credentials);
            await TwelveAPI.GetDefaultWebpage();
            UserInfo = await TwelveAPI.PostUserInfo();
            await TwelveAPI.PostMenu();
            await TwelveAPI.GetSPTSWebpage();
            await TwelveAPI.PostGetProcessDomainResult();
            StudentPersonal = await TwelveAPI.GetStudentPersonal();
            User = await TwelveAPI.GetUser();
            Enrollment = await TwelveAPI.GetSchoolEnrollment();
            Console.WriteLine("K12 Logged in.");
        }

        public async Task<List<TwelveAssignment>> GetAssignments(bool limitByTime)
        {
            int itemCountToRetrieve = 50;
            Console.WriteLine("K12 Retrieving last " + itemCountToRetrieve + " calendar items");
            IList<TwelveCalendarItem> calendarItems = await TwelveAPI.PostStudentCalendarItems(0, itemCountToRetrieve, Enrollment.ID);
            Console.WriteLine("K12 Retrieved calendar items.");

            List<TwelveAssignment> assignments = new List<TwelveAssignment>();

            foreach (TwelveCalendarItem calendarItem in calendarItems)
            {
                if (calendarItem.GetItemType() != TwelveCalendarItem.TwelveCalendarItemType.ASSIGNMENT)
                    continue;

                IList<TwelveAssignmentPersonal> apList = await TwelveAPI.PostGetAssignments(calendarItem.ID, StudentPersonal.ID);

                if (apList.Count == 0)
                    continue;

                TwelveAssignmentPersonal assignmentPersonal = apList[0];
                TwelveAssignment assignment = assignmentPersonal.Assignment;

                if (limitByTime && assignment.DueDate < (DateTime.Now - TimeSpan.FromDays(7)))
                {
                    continue;
                }

                assignments.Add(assignment);

            }

            Console.WriteLine("K12 Retrieved past assignments");
            return assignments;
        }

    }
}
