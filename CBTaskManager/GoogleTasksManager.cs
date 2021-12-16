using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Tasks.v1;
using Google.Apis.Tasks.v1.Data;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace CBTaskManager
{
    class GoogleTasksManager
    {

        private string[] Scopes = { TasksService.Scope.Tasks };
        private const string ApplicationName = "CB Task Manager";

        public TasksService service;
        public TaskList taskList;

        public void AddNewTask(Task task)
        {
            service.Tasks.Insert(task, taskList.Id).Execute();
            Console.WriteLine("Google Tasks, Added new task.");
        }

        public void Login()
        {
            Console.WriteLine("Google Tasks, Logging in..");
            UserCredential credential;

            using (var stream =
                new FileStream(AppContext.BaseDirectory + "\\credentials.json", FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = AppContext.BaseDirectory + "\\token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.FromStream(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Google Tasks API service.
            service = new TasksService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            Console.WriteLine("Google Tasks, Logged in.");

            TasklistsResource.ListRequest listRequest = service.Tasklists.List();
            listRequest.MaxResults = 10;

            // List task lists.
            IList<TaskList> taskLists = listRequest.Execute().Items;
            if (taskLists != null && taskLists.Count > 0)
            {
                taskList = taskLists[0];
                Console.WriteLine("Google Tasks, Selected tasklist: " + taskList.Title);
            }
        }
    }
}
