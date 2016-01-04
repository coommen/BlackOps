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

        public AppWatcher()
        {
            InitializeComponent();


        }

        protected override void OnStart(string[] args)
        {
            // EventLog.WriteEntry("Service Started", EventLogEntryType.Information);
            mgmtWtch = new System.Management.ManagementEventWatcher("Select * From Win32_ProcessStartTrace where ProcessName='javaws.exe'");
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
            stopProcessByID(Convert.ToInt32(processID));
        }

        void stopProcessByID(int ID)
        {
            try
            {
                Process process = Process.GetProcessById(ID);
                String userid = process.StartInfo.UserName;
                if (userid=="ruben_000")
                    process.Kill();
                EventLog.WriteEntry("Stopped Process: " + process.ProcessName + " Process ID:" + process.Id, EventLogEntryType.Warning);
            }
            catch(Exception ex)
            {
                EventLog.WriteEntry("Encountered Errors killing Process:\n" + ex.ToString(), EventLogEntryType.Error);

            }
        }

}

}

