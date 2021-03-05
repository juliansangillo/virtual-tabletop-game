using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Zenject;
using NaughtyBikerGames.SDK.LevelManagement.Interfaces;
using NaughtyBikerGames.SDK.Adapters.Interfaces;

namespace NaughtyBikerGames.SDK.LevelManagement {
	/**
    * Default implementation of ILevelManager.
    *
    * @author Julian Sangillo \<https://github.com/juliansangillo\>
    * @version 3.0
    * @since 1.0
    * 
    * @see LevelManagerInstaller
    */
	public class LevelManager : ILevelManager {
        private readonly IDictionary<string, int> levels;
        private readonly ISceneManager sceneManager;
        private readonly ISceneUtility sceneUtility;

        private string[] labels;
        private int activeLevel;
        private string firstLevel;

        /// Default implementation of ILevelManager.ActiveLevelLabel
        public string ActiveLevelLabel {
            get {
                return labels[activeLevel];
            }
        }

        /// Default implementation of ILevelManager.ActiveLevel
		public int ActiveLevel {
            get {
                return activeLevel;
            }
        }

        /**
        * Default implementation of ILevelManager.FirstLevel
        *
        * @throws ArgumentNullException Raised when the name parameter is null
        * @throws ArgumentException Raised when a level by the name provided does not exist. In the event this happens, please verify
        * the scenes added to Build Settings
        */
		public string FirstLevel {
            get {
                return this.firstLevel;
            }
            set {
                if(string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException("FirstLevel", "First level cannot be null!");
                else if(!levels.ContainsKey(value))
                    throw new ArgumentException(String.Format("The level \"{0}\" doesn't exist!", value), "FirstLevel");

                this.firstLevel = value;
            }
        }

		/**
        * Callback from Zenject.IInitializable that is invoked during Unity's Start phase. When called, it pulls the list of
        * scenes from Unity Build Settings and adds them as levels to track, it sets the first level to level 0, it calculates 
        * the labels of the levels, and it sets the active level currently.
        *
        * This function is called automatically by Zenject. Don't call this directly!
        */
		public void Initialize() {
            int level;
            int count = sceneManager.sceneCountInBuildSettings;
            string name;

            labels = new string[count];

            for(int i = 0; i < count; i++) {
                string capsOrNonLetterRegex = @"(?<!\s)(([A-Z]+)|([^A-Za-z\s]+))";
                string underscoreRegex = @"_+";

                level = i;
                name = Path.GetFileNameWithoutExtension(sceneUtility.GetScenePathByBuildIndex(i));

                levels.Add(name, level);

                if(i == 0)
                    firstLevel = name;

                name = Regex.Replace(name, underscoreRegex, " ");
                name = Regex.Replace(name, capsOrNonLetterRegex, m => (" " + m.Value));
                name = name.Trim();

                labels[i] = name;
            }

            activeLevel = sceneManager.GetActiveSceneBuildIndex();
        }

        /**
        * Default implementation of ILevelManager.GetLevel(string name)
        * 
        * @param name The name of the level
        *
        * @return The index of the level
        *
        * @throws ArgumentNullException Raised when the name parameter is null
        * @throws ArgumentException Raised when a level by the name provided does not exist. In the event this happens, please verify
        * the scenes added to Build Settings
        */
        public int GetLevel(string name) {
            int level;

            if(string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("name", "Name of level cannot be null!");
            else if(!levels.TryGetValue(name, out level))
                throw new ArgumentException(String.Format("The level \"{0}\" cannot be read because it doesn't exist!", name), "name");

			return level;
		}

        /**
        * Default implementation of ILevelManager.GetLevelLabel(int index)
        *
        * @param index The build index for the level
        *
        * @return The label of the level
        *
        * @throws ArgumentNullException Raised when the index parameter is a negative value
        * @throws ArgumentException Raised when a level by the index provided does not exist. In the event this happens, please verify
        * the scenes added to Build Settings
        */
		public string GetLevelLabel(int index) {
            if(index < 0)
                throw new ArgumentOutOfRangeException("index", index, "Level index cannot be a negative!");
            else if(index >= labels.Length)
                throw new ArgumentOutOfRangeException("index", index, "The index doesn't exist!");

			return labels[index];
		}

        /**
        * Default implementation of ILevelManager.GetLevelLabel(string name)
        *
        * @param name The name of the level
        *
        * @return The label of the level
        *
        * @throws ArgumentNullException Raised when the name parameter is null
        * @throws ArgumentException Raised when a level by the name provided does not exist. In the event this happens, please verify
        * the scenes added to Build Settings
        */
		public string GetLevelLabel(string name) {
            int index;

            if(string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("name", "Name of level cannot be null!");
            else if(!levels.TryGetValue(name, out index))
                throw new ArgumentException(String.Format("The level \"{0}\" cannot be read because it doesn't exist!", name), "name");

			return labels[index];
		}

        /**
        * Default implementation of ILevelManager.LoadLevel(string name)
        * 
        * @param name The name of the level to load. This is the actual name of the scene as shown in Build Settings, not the display
        * name!
        * 
        * @throws ArgumentNullException Raised when the name parameter is null
        * @throws ArgumentException Raised when a level by the name provided does not exist. In the event this happens, please verify
        * the scenes added to Build Settings
        */
        public void LoadLevel(string name) {
            int newLevel = GetLevel(name);

            sceneManager.LoadScene(newLevel);
            activeLevel = newLevel;
        }

        /**
        * Default implementation of ILevelManager.LoadNextLevel()
        */
        public void LoadNextLevel() {
            activeLevel++;

            if(activeLevel < sceneManager.sceneCountInBuildSettings)
                sceneManager.LoadScene(activeLevel);
            else {
                sceneManager.LoadScene(0);
                activeLevel = 0;
            }
        }

        /**
        * Default implementation of ILevelManager.LoadPreviousLevel()
        *
        * @throws InvalidOperationException Raised when you are already on level index 0 and there is no previous level to load
        */
        public void LoadPreviousLevel() {
			activeLevel--;

            if(activeLevel >= 0)
                sceneManager.LoadScene(activeLevel);
            else {
                activeLevel = 0;
                throw new InvalidOperationException("You are already on level index 0. No previous level to load!");
            }
		}

        /**
        * Default implementation of ILevelManager.LoadFirstLevel()
        */
		public void LoadFirstLevel() {
            int index = levels[firstLevel];

			sceneManager.LoadScene(index);
            activeLevel = index;
		}

        /**
        * Default implementation of ILevelManager.ReloadLevel()
        */
        public void ReloadLevel() {
            sceneManager.LoadScene(activeLevel);
        }

		[Inject]
        private LevelManager(IDictionary<string, int> levels, ISceneManager sceneManager, ISceneUtility sceneUtility) {
            this.levels = levels;
            this.sceneManager = sceneManager;
            this.sceneUtility = sceneUtility;
        }       
    }
}