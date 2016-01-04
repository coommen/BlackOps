namespace BlackOps
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.awProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.awInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // awProcessInstaller
            // 
            this.awProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.awProcessInstaller.Password = null;
            this.awProcessInstaller.Username = null;
            // 
            // awInstaller
            // 
            this.awInstaller.Description = "Application Blacklisting Service";
            this.awInstaller.DisplayName = "BlackOps App Watcher";
            this.awInstaller.ServiceName = "AppWatcher";
            this.awInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.awProcessInstaller,
            this.awInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller awProcessInstaller;
        private System.ServiceProcess.ServiceInstaller awInstaller;
    }
}