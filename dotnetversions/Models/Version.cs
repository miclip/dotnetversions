using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetversions.Models
{
    public class DotnetVersion
    {
        string KeyName { get; set; }
        public string Name { get; set; }
        public string SubKeyName { get; set; }
        public string ServicePack { get; set; }
        public string VersionKeyName { get; set; }
        public string Install { get; set; }

        public IList<DotnetVersion> SubVersions { get; set; }
    }

    
}
