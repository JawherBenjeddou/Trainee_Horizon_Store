using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game_Over_2_SaveSystem
{
    //Keys

    public static void Save_Old_Data(int starsOwned, int levelIndex)
    {

        if (PlayerPrefs.HasKey(Game_Over_2_Constants.GAME_DATA_STARS + Game_Over_2_Constants.COOL_SEPERATOR + levelIndex))
        {
            if (PlayerPrefs.GetInt(Game_Over_2_Constants.GAME_DATA_STARS + Game_Over_2_Constants.COOL_SEPERATOR + levelIndex, 0) < starsOwned)
            {
                PlayerPrefs.SetInt(Game_Over_2_Constants.GAME_DATA_STARS + Game_Over_2_Constants.COOL_SEPERATOR + levelIndex, starsOwned);
            }
        }
        else
        {
            PlayerPrefs.SetInt(Game_Over_2_Constants.GAME_DATA_STARS + Game_Over_2_Constants.COOL_SEPERATOR + levelIndex.ToString(), starsOwned);
        }
    }

    public static void Final_Level_Reached(int index)
    {
        PlayerPrefs.SetString(Constants.LEVEL_REACHED(), PlayerPrefs.GetString(Constants.LEVEL_REACHED(), string.Empty) + Game_Over_2_Constants.LEVEL_MAP_GAME_NAME + "-");
        short score = 0;

        for (int i = 0; i < index; i++)
        {
            score += (short)PlayerPrefs.GetInt(Game_Over_2_Constants.GAME_DATA_STARS + Game_Over_2_Constants.COOL_SEPERATOR + i, 0);
        }

        if (score >= (index * 3) - index)
        {
            PlayerPrefs.SetInt(Game_Over_2_Constants.LEVEL_MAP_GAME_NAME + Game_Over_2_Constants.STARS, 3);
        }
        else if (score >= (index * 3) - (index * 2))
        {
            PlayerPrefs.SetInt(Game_Over_2_Constants.LEVEL_MAP_GAME_NAME + Game_Over_2_Constants.STARS, 2);
        }
        else
        {
            PlayerPrefs.SetInt(Game_Over_2_Constants.LEVEL_MAP_GAME_NAME + Game_Over_2_Constants.STARS, 1);
        }
    }

    public static int Get_Last_Level_Index()
    {
        string[] levels = PlayerPrefs.GetString(Game_Over_2_Constants.GAME_DATA_TOTAL_STARS).Split(Game_Over_2_Constants.COOL_SEPERATOR);
        return levels.Length - 1;
    }

    public static void Set_User_Data(UserData userData)
    {
        PlayerPrefs.SetString(Game_Over_2_Constants.GAME_REFRENCE_KEY, userData.gameRef);
        PlayerPrefs.SetString(Game_Over_2_Constants.GAME_DATA_USER_ID, userData.uId);
        PlayerPrefs.SetString(Game_Over_2_Constants.GAME_DATA_USER, userData.child);
        PlayerPrefs.SetString(Game_Over_2_Constants.GAME_DATA_USER_FULL_NAME, userData.name + " " + userData.surName);
        PlayerPrefs.SetInt(Game_Over_2_Constants.GAME_DATA_UPPER_ACCESSORIE, userData.avatarUpperAccessories);
        PlayerPrefs.SetInt(Game_Over_2_Constants.GAME_DATA_LOWER_ACCESSORIE, userData.avatarLowerAccessories);
        PlayerPrefs.SetInt(Game_Over_2_Constants.GAME_DATA_BESTSCORE, userData.bestScore);
        PlayerPrefs.SetInt(Game_Over_2_Constants.GAME_DATA_EXPIRIENCE, userData.exp);
        PlayerPrefs.SetString(Game_Over_2_Constants.GAME_DATA_TOTAL_STARS, userData.stars);
    }

    public static UserData Get_User_Data()
    {
        UserData userData = new UserData();

        userData.gameRef = PlayerPrefs.GetString(Game_Over_2_Constants.GAME_REFRENCE_KEY);
        userData.uId = PlayerPrefs.GetString(Game_Over_2_Constants.GAME_DATA_USER_ID);
        userData.child = PlayerPrefs.GetString(Game_Over_2_Constants.GAME_DATA_USER);
        userData.name = PlayerPrefs.GetString(Game_Over_2_Constants.GAME_DATA_USER_FULL_NAME);
        userData.avatarUpperAccessories = PlayerPrefs.GetInt(Game_Over_2_Constants.GAME_DATA_UPPER_ACCESSORIE);
        userData.avatarLowerAccessories = PlayerPrefs.GetInt(Game_Over_2_Constants.GAME_DATA_LOWER_ACCESSORIE);
        userData.bestScore = PlayerPrefs.GetInt(Game_Over_2_Constants.GAME_DATA_BESTSCORE);
        userData.exp = PlayerPrefs.GetInt(Game_Over_2_Constants.GAME_DATA_EXPIRIENCE);
        userData.stars = PlayerPrefs.GetString(Game_Over_2_Constants.GAME_DATA_TOTAL_STARS, userData.stars);

        return userData;
    }
}