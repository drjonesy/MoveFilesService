using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace MoveFilesService
{
    class External
    {
        private Dictionary<string, string> config;
        private DictConfig Dict;

        public External(string filepath)
        {
            ReadConfigFile(filepath);
        }

        /// <summary>
        /// Creates a dictionary from the external filepath: ex: config.txt
        /// </summary>
        /// <param name="filepath"></param>
        private void ReadConfigFile(string filepath)
        {
            string[] rows = File.ReadAllLines(filepath);
            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (var line in rows)
            {
                var KeyValue = line.Split(',');
                dict.Add(key: KeyValue[0], value: KeyValue[1]);
            }
            this.config = dict;
        }// END ReadConfigFile

        /// <summary>
        /// Writes to config["log"] directory. 
        /// Creates a file if file does not exist. Appends a data to a newline.
        /// </summary>
        /// <param name="config">Dictionary<string>, <string></param>
        /// <param name="msg">string message</param>
        public void WriteToConfig(string msg)
        {
            string filepath = this.config["log"] + "/LogHistory__" + DateTime.Now.ToShortDateString().Replace('/', '_') + ".log";
            File.AppendAllText(filepath, msg + Environment.NewLine);
        }

        public void MoveFiles()
        {
            //check if directory exists
            if (Directory.Exists(this.config["src"]) && Directory.Exists(this.config["dst"]))
            {
                for (; ; )
                {
                    //Create New List from all files found in "src"
                    var srcFiles = Directory.GetFiles(this.config["src"]);
                    if (srcFiles.Length != 0)
                    {
                        var BaseNames = new List<string>();
                        foreach (string srcFilepath in srcFiles)
                        {
                            //strip basename from filepath and create new path with the "dst" path
                            var basename = Path.GetFileName(srcFilepath);
                            var dstFilepath = this.config["dst"] + "\\" + basename;
                            File.Move(srcFilepath, dstFilepath);
                            BaseNames.Add(basename);
                        }
                        string allFiles = string.Join(", ", BaseNames);
                        WriteToConfig(DateTime.Now + "\t|\t Moved Files: " + allFiles);
                    }
                    else
                    {
                        WriteToConfig(DateTime.Now + "\t|\t ______Empty");
                    }
                    // Wait "n" number of miliseconds defined in config file
                    Thread.Sleep(Convert.ToInt32(this.config["ms"]));
                }
            }
        }// END MoveFiles
    }
}
