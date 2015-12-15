using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTube.Api
{
    public class FSItemInfo
    {
        public FSItemInfo Parent { get; set; }
        public string Path { get; set; }
        public int Level { get; set; }
        public bool isFile { get; set; }
        public string HashPath { get; set; }
        public string Extension { get; set; }
        public long Size { get; set; }
    }
}
