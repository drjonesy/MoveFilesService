using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;

namespace MoveFilesService
{
    public partial class Service1 : ServiceBase
    {
        static string folderSettings = AppDomain.CurrentDomain.BaseDirectory + @"\settings\folder.settings";
        static string logSettings = AppDomain.CurrentDomain.BaseDirectory + @"\settings\log.settings";

        MoveFiles moveFiles = new MoveFiles(folderSettings, logSettings);

        public Service1() => InitializeComponent();
        protected override void OnStart(string[] args)
        {
            Thread t1 = new Thread(new ThreadStart(moveFiles.Move));
            t1.Start();
        }
        protected override void OnStop()
        {
            moveFiles.WriteToConfig("Process stopped at " + DateTime.Now);
        }

    }
}
