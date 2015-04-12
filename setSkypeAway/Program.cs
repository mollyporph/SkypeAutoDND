using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using SKYPE4COMLib;

namespace setSkypeDnd
{
    class Program
    {
        private static Timer timer;
        private static Skype skype;
        static void Main(string[] args)
        {
            Console.WriteLine("Switch over to skype and accept setSkypeDND :D");
            skype = new Skype();
            skype.Attach();

            timer = new Timer(2000);
            timer.Elapsed += (sender, eventArgs) => ChangeStatusToAway();
            timer.Enabled = true;

            Console.WriteLine("Press the Enter key to exit the program... ");
            Console.ReadLine();
            Console.WriteLine("Terminating the application...");
        }

        private static void ChangeStatusToAway()
        {
            if (skype.CurrentUserStatus != TUserStatus.cusDoNotDisturb)
            {
                skype.ChangeUserStatus(TUserStatus.cusDoNotDisturb);
            }
        }
    }
}
