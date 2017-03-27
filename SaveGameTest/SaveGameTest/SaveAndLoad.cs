using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace SaveGameTest
{
    class SaveAndLoad
    {
        
        JsonSerializer ser;
        StreamReader stRead;
        StreamWriter stWrite;
        StufftoSave stuff = new StufftoSave(10, "dog");
        public SaveAndLoad()
        {
             
            if (Directory.Exists("savedata") == false)
            {
                File.Create("savedata.txt");
            }
            ser = new JsonSerializer();
            stWrite = new StreamWriter("savadata");
            stRead = new StreamReader("savedata");
           
        }
       /* 
        public void setdata()
        {
            int lev;
            int.TryParse(Console.ReadLine(), out lev);
            current = Console.ReadLine();
            Console.WriteLine(current + "  " + lev + " " + fun);

        }
        */
        public void Save()
        {
            string data = "";

            data = JsonConvert.SerializeObject(stuff);
            stWrite.WriteLine(data);
            stWrite.Close();
        }
        public void load()
        {
            string lData = "";
            lData = stRead.ReadToEnd();
            stuff = JsonConvert.DeserializeObject<StufftoSave>(lData);
            lData =  stuff.ToString();
        }
    }
}
