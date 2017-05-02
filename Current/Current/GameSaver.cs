using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Current
{
    class GameSaver
    {
        //Takes care of saving and loading player score, name and progress
        SavedData data;
        
        StreamWriter saver;
        StreamReader loader;
        
        
        public GameSaver(int score , string name, int progress)
        {
            data = new SavedData(score, name, progress);

        }
        public void Save()
        {
            int fileNum = 0;
            if (Directory.Exists("savedata") == true);
            {
                Directory.Delete("savedata");
                /*
                while (Directory.Exists("savedata" + fileNum) == true)
                {

                    fileNum++;
                }
                */
            }
            string sData = null;
            saver = new StreamWriter("savedata.txt");
            
            sData = JsonConvert.SerializeObject(data);
            saver.WriteLine(sData);
            saver.Close();
        }
        public SavedData Load()
        {
            if(Directory.Exists("savedata"))
            {
                string lData = null;
                loader = new StreamReader("savedata");
                lData = loader.ReadLine();
                data = JsonConvert.DeserializeObject<SavedData>(lData);
                return data;
            }
            else
            {
                return null;
            }
        }
       

    }
}
