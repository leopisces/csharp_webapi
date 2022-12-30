using System;
using System.Collections.Generic;
using System.Text;

namespace DataService.Auto
{
    public class AutoConfig
    {
        public List<string> TableNames { get; set; }
        public Dictionary<string, string> Namespaces { get; set; }
    }
}