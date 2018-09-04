using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenEnded.Forms;
using System.Diagnostics;
using System.Configuration;
using System.IO;

namespace OpenEnded
{
    class MyApplicationContext : ApplicationContext
    {
        private NotifyIcon notifyIcon = new NotifyIcon();
        private System.Threading.Timer timerThread;
        private bool HelpersStarted = false;

        public MyApplicationContext()
        {
            MenuItem configMenuItem = new MenuItem("Configuration", new EventHandler(ShowConfig));
            MenuItem exitMenuItem = new MenuItem("Exit", new EventHandler(Exit));

            notifyIcon.Icon = Properties.Resources.AppIcon;
            notifyIcon.ContextMenu = new ContextMenu(new MenuItem[]
                { configMenuItem, exitMenuItem });
            notifyIcon.Visible = true;

            Wait();
        }

        private void Wait()
        {
            timerThread = new System.Threading.Timer(new TimerCallback(ProgramRunCheck), new AutoResetEvent(false), 0, 10000);
        }

        private void ProgramRunCheck(object stateInfo)
        {
            try
            {
                var monitoredProgram = ManageConfiguration.GetMonitoredProgram();
                if (monitoredProgram == null)
                    return;

                FileInfo file = new FileInfo(monitoredProgram.Value);
                Process process = Process.GetProcessesByName(file.Name.Replace(file.Extension, string.Empty)).FirstOrDefault();

                if (process != null)
                {
                    if (!HelpersStarted)
                        StartPrograms();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
                throw;
            }
        }

        private void StartPrograms()
        {
            try
            {
                System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                foreach (KeyValueConfigurationElement item in config.AppSettings.Settings)
                {
                    Process.Start(item.Value);
                    HelpersStarted = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ShowConfig(object sender, EventArgs e)
        {
            var configuration = new Forms.Configuration();
            configuration.ShowDialog();
            HelpersStarted = false;
        }

        private void Exit(object sender, EventArgs e)
        {
            // We must manually tidy up and remove the icon before we exit.
            // Otherwise it will be left behind until the user mouses over.
            notifyIcon.Visible = false;
            Application.Exit();
        }
    }
}
