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
            
            SaveAndLoad data = new SaveAndLoad();
            
            

           
            data.load();
            
            
        }
    }
}
