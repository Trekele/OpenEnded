using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenEnded
{
    enum ProcessType { ToMonitor, ToStart }
    class ListItem
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public ProcessType ProcessType
        {
            get
            {
                return StringToEnum(Name[0].ToString());
            }
            set
            {
                var elements = Name.Split('|');
                Name = string.Format("{0}|{1}", EnumToString(value), elements[1]);
            }
        }

        public override string ToString()
        {
            return Name.Split('|')[1];
        }

        public ListItem(string name, string value, ProcessType processType)
        {
            Name = name;
            Value = value;
            SetInitialProcessType(processType);
        }

        public ListItem(string name, string value)
        {
            Name = name;
            Value = value;
        }

        private void SetInitialProcessType(ProcessType processType)
        {
            if(Name.Split('|').Count() <= 1)
            {
                Name = string.Format("{0}|{1}", EnumToString(processType), Name);
            }
        }

        public static string EnumToString(ProcessType processType)
        {
            switch (processType)
            {
                case ProcessType.ToMonitor:
                    return "M";
                case ProcessType.ToStart:
                    return "S";
                default:
                    return string.Empty;
            }
        }

        public static ProcessType StringToEnum(string processType)
        {
            switch (processType)
            {
                case "M":
                    return ProcessType.ToMonitor;
                case "S":
                    return ProcessType.ToStart;
                default:
                    throw new Exception("ProcessType is invalid");
        }
        }
    }
}
