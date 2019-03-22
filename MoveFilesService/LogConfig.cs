using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MoveFilesService
{
    class LogConfig
    {
        private string[] data;
        public List<string> DataOnlyList { get; set; }
        public Dictionary<string, string> LogDict { get; set; }

        public LogConfig(string filepath)
        {
            data = File.ReadAllLines(filepath);
            DataOnlyList = new List<string>();
            LogDict = new Dictionary<string, string>();
            CleanImport();
            PopulateDictionary();
        }

        private void CleanImport()
        {
            // remove empty lines and lines starting with #
            foreach (string line in this.data)
            {
                if (!String.IsNullOrWhiteSpace(line) && !line.StartsWith("#"))
                {
                    this.DataOnlyList.Add(line);
                }
            }
        }// CleanImport

        /// <summary>
        /// Builds a Dictionary Object if the List object is divisible by 3
        /// {key: {src: value}, {dst: value} }
        /// </summary>
        private void PopulateDictionary()
        {
            // loop through each three lines,only run if divisible by 2
            if (this.DataOnlyList.Count() % 2 == 0)
            {
                foreach (string line in DataOnlyList)
                {
                    string key = line.Split('=')[0];
                    string value = line.Split('=')[1];
                    this.LogDict[key] = value;
                }
            }
        }// END PopulateDictionary
    }
}
