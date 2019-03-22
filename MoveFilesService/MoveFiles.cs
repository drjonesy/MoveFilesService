using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace MoveFilesService
{
    class MoveFiles
    {
        private FolderConfig foldersDict;
        private LogConfig log;

        public MoveFiles(string folder_settings_filepath, string log_filepath)
        {
            foldersDict = new FolderConfig(folder_settings_filepath);
            log = new LogConfig(log_filepath); 
        }


        /// <summary>
        /// Writes to config["log"] directory. 
        /// Creates a file if file does not exist. Appends a data to a newline.
        /// </summary>
        /// <param name="config">Dictionary<string>, <string></param>
        /// <param name="msg">string message</param>
        public void WriteToConfig(string msg)
        {
            string filepath = this.log.LogDict["log"] + "/LogHistory__" + DateTime.Now.ToShortDateString().Replace('/', '_') + ".log";
            File.AppendAllText(filepath, msg + Environment.NewLine);
        }

        public void Move()
        {
            // Get the total number of top level keys in the dictionary
            int TotalFoldersToProcess = this.foldersDict.Dict.Keys.Count();
            for(; ; )
            {
                foreach (KeyValuePair<string, Dictionary<string, string>> kvp in this.foldersDict.Dict)
                {
                    var srcFiles = Directory.GetFiles(kvp.Value["src"]);
                    if (srcFiles.Length != 0)
                    {
                        var BaseNamesList = new List<string>();
                        foreach (string srcFilepath in srcFiles)
                        {
                            // strip basename from each filepath and creat a new path with the "dst" path
                            var basename = Path.GetFileName(srcFilepath);
                            var dstFilepath = kvp.Value["dst"] + "\\" + basename;
                            File.Move(srcFilepath, dstFilepath);
                            BaseNamesList.Add(basename);
                        }
                        string allFiles = string.Join(", ", BaseNamesList);
                        WriteToConfig(DateTime.Now + "\t|\t Moved Files: " + allFiles);
                    }
                    else
                    {
                        WriteToConfig(DateTime.Now + "\t|\t ______Empty");
                    }
                }
                // Wait "n" number of miliseconds defined in config file
                Thread.Sleep(Convert.ToInt32(this.log.LogDict["ms"]));
            }
        }// END MoveFiles
    }
}
