using System;
using System.Timers;
using System.Linq;
using SKYPE4COMLib;
using System.Drawing;
using System.Windows.Forms;
using Application = System.Windows.Forms.Application;
using Timer = System.Timers.Timer;

namespace setSkypeDnd
{
    public class SysTrayApp : Form
    {

        
        [STAThread]
        public static void Main()
        {
            Application.Run(new SysTrayApp());
        }
        private NotifyIcon trayIcon;
        private ContextMenu trayMenu;
        private static Timer timer;
        private static Skype skype;
        private static TUserStatus activeStatus = TUserStatus.cusDoNotDisturb;

        public SysTrayApp()
        {
            
            trayMenu = new ContextMenu();
            trayMenu.MenuItems.Add("Exit", OnExit);
            var statusMenuItem = new MenuItem("Set status to");
            var dndMenuItem = new MenuItem("Do not disturb", OnDnd) {Checked = true};
            statusMenuItem.MenuItems.Add(dndMenuItem);
            statusMenuItem.MenuItems.Add("Away", OnAway);
            statusMenuItem.MenuItems.Add("Invisible", OnInvisible);
            statusMenuItem.MenuItems.Add("Online", OnOnline);
            trayMenu.MenuItems.Add(statusMenuItem);


            trayIcon = new NotifyIcon();
            trayIcon.Text = "SkypeAutoDND";
            trayIcon.Icon = new Icon("skypednd.ico", 40, 40);
            trayIcon.BalloonTipTitle =
                "SkypeAutoDND is running minimized!";
            trayIcon.BalloonTipText = "Remember to accept the app in skype! :)";
            trayIcon.BalloonTipIcon = ToolTipIcon.Info;

            trayIcon.ContextMenu = trayMenu;
            trayIcon.Visible = true;

            skype = new Skype();
            skype.Attach();

            timer = new Timer(2000);
            timer.Elapsed += (sender, eventArgs) => ForceStatus();
            timer.Enabled = true;
            trayIcon.ShowBalloonTip(3000);
        }

        private void OnOnline(object sender, EventArgs e)
        {
            CheckMenuItem(sender);
            SwitchStatus(TUserStatus.cusOnline);
        }

        private void CheckMenuItem(object sender)
        {
            foreach (var item in trayMenu.MenuItems[1].MenuItems.OfType<MenuItem>())
            {
                item.Checked = false;
            }
            var menuItem = sender as MenuItem;
            if (menuItem != null) menuItem.Checked = true;
        }

        private void OnInvisible(object sender, EventArgs e)
        {
            CheckMenuItem(sender);
            SwitchStatus(TUserStatus.cusInvisible);
        }

        private void OnAway(object sender, EventArgs e)
        {
            CheckMenuItem(sender);
            SwitchStatus(TUserStatus.cusAway);
        }

        private void OnDnd(object sender, EventArgs e)
        {
            CheckMenuItem(sender);
            SwitchStatus(TUserStatus.cusDoNotDisturb);
        }

        private void SwitchStatus(TUserStatus status)
        {
            activeStatus = status;
        }

        private static void ForceStatus()
        {
            if (skype.CurrentUserStatus != activeStatus)
            {
                skype.ChangeUserStatus(activeStatus);
            }
        }
        protected override void OnLoad(EventArgs e)
        {
            Visible = false; // Hide form window.
            ShowInTaskbar = false; // Remove from taskbar.

            base.OnLoad(e);
        }

        private void OnExit(object sender, EventArgs e)
        {
            Application.Exit();
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                // Release the icon resource.
                trayIcon.Dispose();
            }

            base.Dispose(isDisposing);
        }


    }
}
