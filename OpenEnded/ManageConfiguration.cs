using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenEnded
{
    class ManageConfiguration
    {
        public List<ListItem> Settings
        {
            get
            {
                List<ListItem> returnList = new List<ListItem>();

                foreach (KeyValueConfigurationElement item in ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).AppSettings.Settings)
                {
                    returnList.Add(new ListItem(item.Key, item.Value));
                }

                return returnList;
            }
        }

        public void AddSetting(ListItem item)
        {
            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings.Add(item.Name, item.Value);
            config.Save();
        }

        public void AddMonitorSetting(ListItem item)
        {
            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            foreach (KeyValueConfigurationElement setting in config.AppSettings.Settings)
            {
                if(ListItem.StringToEnum(setting.Key[0].ToString()) == ProcessType.ToMonitor)
                    config.AppSettings.Settings.Remove(setting.Key);
            }

            config.AppSettings.Settings.Add(item.Name, item.Value);
            config.Save();
        }

        public void RemoveSetting(ListItem item)
        {
            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings.Remove(item.Name);
            config.Save();
        }

        public static ListItem GetMonitoredProgram()
        {
            return GetPrograms(ProcessType.ToMonitor).FirstOrDefault();
        }

        public static List<ListItem> GetStartPrograms()
        {
            return GetPrograms(ProcessType.ToStart);
        }

        private static List<ListItem> GetPrograms(ProcessType type)
        {
            List<ListItem> returnList = new List<ListItem>();

            foreach (KeyValueConfigurationElement item in ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).AppSettings.Settings)
            {
                returnList.Add(new ListItem(item.Key, item.Value));
            }

            return returnList.Where(w => w.ProcessType == type).ToList();
        }
    }
}
