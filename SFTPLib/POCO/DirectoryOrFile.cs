using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFTPEntities
{
    public class DirectoryOrFile
    {
        public DirectoryOrFile()
        {
            
        }
        public int NodeLevel { get; set; }
        public eFileType FileType { get; set; }
        public string FullPath { get; set; }
        public string ParentFullPath { get; set; }
        public string Name { get; set; }
        public bool IsDirectory { get; set; }
        public List<DirectoryOrFile> files { get; set; }

    }
}
