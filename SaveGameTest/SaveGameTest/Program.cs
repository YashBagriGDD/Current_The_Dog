using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace SaveGameTest
{
    class Program
    {
        static void Main(string[] args)
        {
            StufftoSave file = new StufftoSave(1, "");
            SaveAndLoad data = new SaveAndLoad();
            
            

            data.Save();
            data.load();
            
            
        }
    }
}
