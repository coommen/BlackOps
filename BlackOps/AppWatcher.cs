using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;


namespace BlackOps
{
    public partial class AppWatcher : ServiceBase
    {
        public System.Management.ManagementEventWatcher mgmtWtch;
        public const String blockedApp = "javaws.exe";
        public const String blockedUser = "ruben";


        public AppWatcher()
        {
            InitializeComponent();


        }

        protected override void OnStart(string[] args)
        {
            // EventLog.WriteEntry("Service Started", EventLogEntryType.Information);
            mgmtWtch = new System.Management.ManagementEventWatcher("Select * From Win32_ProcessStartTrace");
            mgmtWtch.EventArrived += new System.Management.EventArrivedEventHandler(mgmtWtch_EventArrived);
            mgmtWtch.Start();
        }

        protected override void OnStop()
        {
            // EventLog.WriteEntry("Service Stopped", EventLogEntryType.Information);
            mgmtWtch.Stop();
        }

        void mgmtWtch_EventArrived(object sender, System.Management.EventArrivedEventArgs e)
        {

            String processID = e.NewEvent.Properties["ProcessID"].Value.ToString();
            String processName = e.NewEvent.Properties["ProcessName"].Value.ToString();
            
            #region Example to enumerate Properties
            //foreach (PropertyData pd in e.NewEvent.Properties)
            //{
            //    EventLog.WriteEntry(String.Format("{0} : {1}", pd.Name, pd.Value), EventLogEntryType.Warning);
            //}
            #endregion

            EventLog.WriteEntry("Detected Process Start: " + processName + " Process ID:" + processID, EventLogEntryType.Warning);
            if (processName== blockedApp)
            { 
                stopProcessByID(Convert.ToInt32(processID));
            }
        }

        void stopProcessByID(int ID)
        {
            try
            {
                Process process = Process.GetProcessById(ID);
                String userid = GetProcessOwner(ID);
                if (userid.Contains(blockedUser))
                {
                    process.Kill();
                    EventLog.WriteEntry("Stopped Process: " + process.ProcessName + " Process ID:" + process.Id, EventLogEntryType.Warning);
                }
                else
                {
                    EventLog.WriteEntry(String.Format("Process: {0}, Process ID: {1}, was started by {2} so it was not terminated",process.ProcessName ,process.Id, userid), EventLogEntryType.Warning);
                }
                  

            }
            catch(Exception ex)
            {
                EventLog.WriteEntry("Encountered Errors killing Process:\n" + ex.ToString(), EventLogEntryType.Error);

            }
        }

        string GetProcessOwner(int processId)
        {
            string query = "Select * From Win32_Process Where ProcessID = " + processId;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
            ManagementObjectCollection processList = searcher.Get();

            foreach (ManagementObject obj in processList)
            {
                string[] argList = new string[] { string.Empty, string.Empty };
                int returnVal = Convert.ToInt32(obj.InvokeMethod("GetOwner", argList));
                if (returnVal == 0)
                {
                    // return DOMAIN\user
                    return argList[1] + "\\" + argList[0];
                }
            }

            return "NO OWNER";
        }

    }

}

