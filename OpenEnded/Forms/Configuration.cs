using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenEnded.Properties;
using System.Configuration;

namespace OpenEnded.Forms
{
    public partial class Configuration : Form
    {
        private ManageConfiguration manageConfiguration;

        public Configuration()
        {
            InitializeComponent();
            manageConfiguration = new ManageConfiguration();
            LoadSettings();
        }

        private void LoadSettings()
        {
            lbPaths.Items.Clear();

            manageConfiguration.Settings.ForEach(f =>
            {
                if(f.ProcessType == ProcessType.ToStart)
                    lbPaths.Items.Add(f);
            });

            var monitorPrograms = manageConfiguration.Settings.Where(w => w.ProcessType == ProcessType.ToMonitor);

            if (monitorPrograms.Any())
                txtMonitorProgramPath.Text = monitorPrograms.FirstOrDefault().Value;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            switch (ofdProgramsToRun.ShowDialog())
            {
                case DialogResult.OK:
                    manageConfiguration.AddSetting(new ListItem(ofdProgramsToRun.SafeFileName, ofdProgramsToRun.FileName, ProcessType.ToStart));
                    break;
                default:
                    //do nothing
                    break;
            }

            LoadSettings();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lbPaths.SelectedItem != null && lbPaths.SelectedItem is ListItem item)
            {
                manageConfiguration.RemoveSetting(item);   
            }

            LoadSettings();
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            switch (ofdProgramToMonitor.ShowDialog())
            {
                case DialogResult.OK:
                    manageConfiguration.AddMonitorSetting(new ListItem(ofdProgramToMonitor.SafeFileName, ofdProgramToMonitor.FileName, ProcessType.ToMonitor));
                    break;
                default:
                    //do nothing
                    break;
            }

            LoadSettings();
        }
    }
}
