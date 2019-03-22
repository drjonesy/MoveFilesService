using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MoveFilesService
{
    /// <summary>
    /// Builds a Dictionary object from an external file.
    /// Ex: {"key": 
    ///         {"src", value}, 
    ///         {"dst": value} 
    ///     }
    /// </summary>
    class FolderConfig
    {
        // read in an external file
        private string[] data;
        public List<string> DataOnlyList { get; set; }
        public Dictionary<string, Dictionary<string, string>> Dict { get; set; }

        public FolderConfig(string filepath)
        {
            data = File.ReadAllLines(filepath);
            DataOnlyList = new List<string>();
            Dict = new Dictionary<string, Dictionary<string, string>>();
            CleanImport();
            PopulateDictionary();
        }

        /// <summary>
        /// Adds any line NOT starting with "#" and that is NOT an empty line to at a private List<string>
        /// </summary>
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
            // loop through each three lines,only run if divisible by 3
            if (this.DataOnlyList.Count() % 3 == 0)
            {
                for (int i = 0; i < this.DataOnlyList.Count(); i += 3)
                {
                    string key = this.DataOnlyList[i].Split('=')[1];
                    string src = this.DataOnlyList[i + 1].Split('=')[1];
                    string dst = this.DataOnlyList[i + 2].Split('=')[1];
                    this.Dict[key] = new Dictionary<string, string> { { "src", src }, { "dst", dst } };

                }
            }
        }// END PopulateDictionary

        public override string ToString()
        {
            string results = "";
            // Loop Through Each Dictionary Key:Value Pair
            foreach (KeyValuePair<string, Dictionary<string, string>> kvp in this.Dict)
            {
                results += kvp.Key + "\n";
                results += "> src: " + kvp.Value["src"] + "\n";
                results += "> dst: " + kvp.Value["dst"] + "\n";
            }
            return results;
        }
    }// END CLASS
}
