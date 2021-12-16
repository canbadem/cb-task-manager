using Microsoft.Win32.TaskScheduler;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;

namespace CBTaskManager
{
    class Scheduler
    {
        public Task Task;
        public TaskDefinition TaskDefinition;
        public string TaskName = "CBTM";

        public SchedulerSettings Settings;
        public string SettingsPath = "schedulerSettings.json";

        public Scheduler()
        {
            SettingsPath = AppContext.BaseDirectory + "\\schedulerSettings.json";
            LoadSettings();
            Run();
        }

        public void Run()
        {
            Task = TaskService.Instance.GetTask(TaskName);

            if (Task == null)
            {
                Console.WriteLine("Task not registered");




            }

            TaskDefinition = TaskService.Instance.NewTask();
            TaskDefinition.RegistrationInfo.Description = "CB Task Manager";

            if (Settings.UseSyncOnUserLogon)
            {
                LogonTrigger lt = new LogonTrigger();
                //TaskDefinition.Triggers.Add(lt);

            }

            if (Settings.UseSyncTimeTrigger)
            {
                TimeTrigger tt = new TimeTrigger();
                tt.Repetition.Interval = TimeSpan.FromMinutes(Settings.SyncEveryMinutes);
                TaskDefinition.Triggers.Add(tt);
            }

            string filename = Process.GetCurrentProcess().MainModule.FileName;


            TaskDefinition.Actions.Add(filename);

            // Register the task in the root folder of the local machine
            TaskService.Instance.RootFolder.RegisterTaskDefinition("CBTM", TaskDefinition);
        }

        public void LoadSettings()
        {
            if (File.Exists(SettingsPath))
            {
                string json = File.ReadAllText(SettingsPath);
                Settings = JsonConvert.DeserializeObject<SchedulerSettings>(json);
            }
            else
            {
                Settings = new SchedulerSettings();
                using (StreamWriter file = File.CreateText(SettingsPath))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Formatting = Formatting.Indented;
                    serializer.Serialize(file, Settings);
                }
            }
        }

    }
}
