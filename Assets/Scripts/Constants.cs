using System.IO;
using System;
using UnityEngine;

public class Constants
{
    #region PlayerPrefs Keys
    public static string ACCOUNT_ID = "accountID";

    public static string LEVEL_REACHED() // 000000-000010
    {
        return  "Level_Reached";
    }

    public static string GAME_DATA_KEY()
    {
        return "game_data_level";
    }

    public static string DLC_IS_OPEN_FROM_MAP = "Dlc_Is_Open_From_Map"; // if 0 then quit to main menu else map


    #endregion

    #region Categories
    public static string FEEDBACK_CATEGORY = "FeedBacks";
    public static string DELETE_CATEGORY = "DeleteRequests";
    public static string BANK_DIRECT_TRANSFER = "DirectTransactions";
    public static string DELIVERY = "Deliveries";

    public static string NODE_GAMES = "Games";
    public static string NODE_STORIES = "Stories";
    public static string NODE_VIDEOS = "Videos";

    public static string NODE_EXPERIMENTAL_GAMES = "ExperimentalGames";

    public static string GAME_CATEGORY = "GamesGen2";
    public static string QUIZ_CATEGORY = "Quizzes";
    public static string STORY_CATEGORY = "StoriesGen2";
    public static string MAGAZINE_CATEGORY = "Magazines";
    public static string CHARACTER_CATEGORY = "Character";
    public static string VIDEO_CATEGORY = "VideosGen2";
    public static string EVENT_CATEGORY = "Events";
    #endregion

    #region Tools
    public static string UPPER_SPRITE_RESOLVER_CATEGORY = "Upper_Accessory";
    public static string DOWN_SPRITE_RESOLVER_CATEGORY = "Down_Accessory";
    #endregion

    #region Scene Names
    public static string SCENE_MAIN_MENU = "1-Main_Menu";

    public static string SCENE_GAMES = "1-Main_Menu_Games";
    public static string SCENE_STORIES = "1-Main_Menu_Stories";
    public static string SCENE_VIDEOS = "1-Main_Menu_Videos";

    public static string SCENE_MAP_SCENE = "4_Map_Scene";
    #endregion
}