﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Current
{

    /// <summary>
    /// Very general state of the game
    /// </summary>
    enum GameState
    {
        Game,
        MainMenu,
        GameOver
    }
    /// <summary>
    /// States for the main menu
    /// </summary>
    enum MainMenuState
    {
        MainMenu,
        Options
    }

    /// <summary>
    /// General states for actual gameplay
    /// </summary>
    enum GameplayState
    {
        Normal,
        Paused
    }
    /// <summary>
    /// Manages the game.
    /// </summary>
    static class GameManager
    {
        //This should hold ALL GameObjects in the game. 
        private static Dictionary<string, GameObject> Objects { get; }
            = new Dictionary<string, GameObject>();

        //This should hold ALL CollidableObjects in the game
        public static List<CollidableObject> CollidableObjects { get; }
            = new List<CollidableObject>();
        //The current score of the game
        public static int Score { get; set; }
        //Time of the game. (Not gameTime, just for like score and stuff)
        public static float Time { get; set; }

        //Various states for the game
        public static GameState gameState = GameState.MainMenu;
        public static MainMenuState mainMenuState = MainMenuState.MainMenu;
        public static GameplayState gameplayState = GameplayState.Normal;


        //Dictionary of texture names to cooresponding class names
        public static readonly Dictionary<string, string> TextureMapping
            = new Dictionary<string, string>
            {
                {"deep water tile", "Water" },
                {"grass tile", "Platform" },
                {"grass to sand tile", "Platform" },
                {"sand tile",  "Platform" },
                {"shore tile" , "Platform" },
                {"upper water tile", "Platform" }
            };

        /// <summary>
        /// Adds a GameObject reference to the Objects dictionary
        /// If a CollidableObject is passed, it is also added to the CollidableObjects List
        /// </summary>
        /// <param name="name">GameObject name</param>
        /// <param name="g">GameObject reference</param>
        public static void Add(string name, GameObject g)
        {
            Objects.Add(name, g);
            if (g is CollidableObject)
            {
                CollidableObject c = (CollidableObject)g;
                CollidableObjects.Add(c);
            }
                
        }
        /// <summary>
        /// Get a GameObject named name from the Objects dictionary
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static GameObject Get(string name)
        {
            return Objects[name];
        }
        /// <summary>
        /// Get all of the known GameObjects in the game as a Dictionary
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, GameObject> GetAll()
        {
            return Objects;
        }

        /// <summary>
        /// Parses the given JSON file into a list of SaveTiles
        /// </summary>
        /// <param name="path">Path to the level text file</param>
        /// <returns>A list of SaveTiles</returns>
        public static List<SaveTile> ParseLevel(string path)
        {
            List<SaveTile> tiles;
            using (StreamReader reader = new StreamReader(path))
            {
                string rData = reader.ReadToEnd();
                tiles = JsonConvert.DeserializeObject<List<SaveTile>>(rData);
            }

            return tiles;
        }
        
    }
}
