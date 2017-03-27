using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SaveGameTest
{
    class StufftoSave
    {
        
        public int level {get; set;}
        public string current { get; set; }
        
        public StufftoSave(int l, string d)
        {
            level = l;
            current = d;
        }
            
        

    }
}
