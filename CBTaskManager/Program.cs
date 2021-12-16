using System;
using System.Threading.Tasks;

namespace CBTaskManager
{
    class Program
    {




        static Manager Manager;
        static System.Threading.Tasks.Task MainTask;


        static async Task Main(string[] args)
        {
            Console.WriteLine("CBTaskManager " + DateTime.Now.ToString());
            Console.Title = "CBTaskManager";

            Manager = new Manager();
            MainTask = System.Threading.Tasks.Task.Run(() => Manager.Run());
            await MainTask;
        }
    }
}
