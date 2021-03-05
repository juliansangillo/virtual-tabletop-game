using Zenject;

namespace NaughtyBikerGames.SDK.LevelManagement.Interfaces {
    /**
    * Keeps track of the active scene and abstracts the process for navigating between scenes using Unity's built in SceneManagement 
    * API. It will use the list of scenes added in Unity's Build Settings as the levels to track. If you want a scene to be treated 
    * as a level that can be accessed by the Level Manager, add it to the build settings! The filenames of the scenes should be unique, 
    * even if they are located in separate folders! To use, inject an ILevelManager object wherever needed using Zenject's "Inject" 
    * attribute.<br>
    *
    * A Level Manager not only keeps track of the levels, it also initializes a list of labels (display names) from the actual names of
    * the levels. This allows you to reuse the level's name for UI display purposes. For example, "UltimateLevel8-1_005" will be
    * mapped to the label "Ultimate Level 8-1 005". Below are the label rules. <br>
    *
    * <ul>
    *   <li>Any set of letters starting with a capital is delimited with a space, except for the first one</li>
    *   <li>Any set of numbers and/or special characters before the next capital or underscore is treated as its own word and delimited with a space</li>
    *   <li>All underscores are replaced with spaces</li>
    * </ul>
    *
    * @author Julian Sangillo \<https://github.com/juliansangillo\>
    * @version 3.0
    * @since 1.0
    */
    public interface ILevelManager : IInitializable {
        /// The label of the active level
        string ActiveLevelLabel { get; }

        /// The index of the active level
        int ActiveLevel { get; }

        /**
        * The name of the first level. This will always be set to level 0 by default unless otherwise stated. 
        * Is used with LoadFirstLevel to go to this level.
        *
        * @throws ArgumentNullException Raised when the name parameter is null
        * @throws ArgumentException Raised when a level by the name provided does not exist. In the event this happens, please verify
        * the scenes added to Build Settings
        */
        string FirstLevel { get; set; }

        /**
        * Gets the index of a level when given the level's name.
        * 
        * @param name The name of the level
        *
        * @return The index of the level
        *
        * @throws ArgumentNullException Raised when the name parameter is null
        * @throws ArgumentException Raised when a level by the name provided does not exist. In the event this happens, please verify
        * the scenes added to Build Settings
        */
        int GetLevel(string name);

        /**
        * Gets the label of a level when given the level's index.
        *
        * @param index The build index for the level
        *
        * @return The label of the level
        *
        * @throws ArgumentNullException Raised when the index parameter is a negative value
        * @throws ArgumentException Raised when a level by the index provided does not exist. In the event this happens, please verify
        * the scenes added to Build Settings
        */
        string GetLevelLabel(int index);

        /**
        * Gets the label of a level when given the level's name.
        *
        * @param name The name of the level
        *
        * @return The label of the level
        *
        * @throws ArgumentNullException Raised when the name parameter is null
        * @throws ArgumentException Raised when a level by the name provided does not exist. In the event this happens, please verify
        * the scenes added to Build Settings
        */
        string GetLevelLabel(string name);

        /**
        * Loads the level (or scene) selected by name and sets that as the active level, while unloading the current active level.
        * This allows you to jump to whichever level you need at any point during gameplay.
        * 
        * @param name The name of the level to load. This is the actual name of the scene as shown in Build Settings, not the display
        * name!
        * 
        * @throws ArgumentNullException Raised when the name parameter is null
        * @throws ArgumentException Raised when a level by the name provided does not exist. In the event this happens, please verify
        * the scenes added to Build Settings
        */
        void LoadLevel(string name);

        /**
        * Loads next level according to the scene's build index and the order the scenes are listed in Build Settings. This allows
        * you to let LevelManager determine the next level and take you there automatically, in the case that levels are progressed
        * sequentially in your game, without having to keep track of all level names. If there is no next level, this will take you
        * back to the first scene listed in Build Settings.
        */
        void LoadNextLevel();

        /**
        * Loads previous level according to the scene's build index and the order the scenes are listed in Build Settings. This allows
        * you to let LevelManager determine the previous level and take you there automatically, in the case that levels are progressed
        * sequentially in your game, without having to keep track of all the level names. If there is no previous level, this will raise
        * an InvalidOperationException.
        *
        * @throws InvalidOperationException Raised when you are already on level index 0 and there is no previous level to load
        */
        void LoadPreviousLevel();

        /**
        * Loads the first level, which is index 0 by default, but can be changed by setting the FirstLevel property. This is useful if
        * you want to bring a player back to the first level repeatedly during the game, but you don't want to repeatedly give the level
        * name. It is also useful to be able to choose which level is the "first level" because index 0 is typically the main menu in a
        * lot of games, which may not be where you actually want to bring the player.
        */
        void LoadFirstLevel();

        /**
        * Reloads the active level.
        */
        void ReloadLevel();
    }
}