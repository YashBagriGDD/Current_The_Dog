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
        
        
        public GameSaver(int score , string name, int progress, int highscore)
        {
            data = new SavedData(score, name, progress, highscore);

        }
        public void Save()
        {
            string sData = null;
            saver = new StreamWriter("savedata.txt");
            
            sData = JsonConvert.SerializeObject(data);
            saver.WriteLine(sData);
            saver.Close();
        }
        public SavedData Load()
        {
            if (!File.Exists("savedata.txt"))
                return null;

            string lData = null;
            loader = new StreamReader("savedata.txt");
            lData = loader.ReadLine();

            if (lData == null)
            {
                loader.Close();
                return null;
            }
                

            data = JsonConvert.DeserializeObject<SavedData>(lData);
            loader.Close();
            return data;

        }
       

    }
}
