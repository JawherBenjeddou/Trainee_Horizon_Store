using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Newtonsoft.Json;

public class Game_Over_2_DataBase_Manager : MonoBehaviour
{
    //leader board game object

    [HideInInspector]
    public bool isFailureTransaction;

    [HideInInspector]
    public bool isSuccessTransaction;

    private Dictionary<string, object> _leaderBoard = new Dictionary<string, object>();

    private List<UserData> _userDataList = new List<UserData>();

    [HideInInspector]
    public UserData myUserData;

    private void Awake()
    {
        myUserData = Game_Over_2_SaveSystem.Get_User_Data();

        Game_Over_2_OptionPanel.sfxMuted = false;
        Game_Over_2_OptionPanel.musicMuted = false;
    }

    public void Fech_Players_Data()
    {
        Dictionary<string, object> user = new Dictionary<string, object>();
        foreach (KeyValuePair<string, object> kvp in _leaderBoard)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            user = JsonConvert.DeserializeObject<Dictionary<string, object>>(_leaderBoard[kvp.Key].ToString());
#else
            user = (Dictionary<string, object>)_leaderBoard[kvp.Key];
#endif
            List<string> newkeyList = Get_Accounts_User(user);

            for (int i = 0; i < newkeyList.Count; i++)
            {
                string key = newkeyList[i];
                UserData data = Set_Specfic_User_Data(key, user, kvp.Key);
                _userDataList.Add(data);
            }
        }
    }

    public List<UserData> Users_Data()
    {
        return _userDataList;
    }

    private UserData Set_Specfic_User_Data(string childKey, Dictionary<string, object> child, string userId)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        Dictionary<string, object> userData = JsonConvert.DeserializeObject<Dictionary<string, object>>(child[childKey].ToString());
#else
        Debug.Log(childKey);
        Dictionary<string, object> userData = (Dictionary<string, object>)child[childKey];
#endif
        UserData data = new UserData();
        data.uId = userId;
        data.child = childKey;
        Debug.Log(data.child);
        data.name = userData[Game_Over_2_Constants.DB_NAME].ToString() + " " + userData[Game_Over_2_Constants.DB_SURNAME].ToString();

        data.bestScore = int.Parse(userData[Game_Over_2_Constants.DB_BEST_SCORE].ToString());
        data.stars = userData[Game_Over_2_Constants.DB_STARS].ToString();
        data.exp = int.Parse(userData[Game_Over_2_Constants.DB_XP].ToString());
        data.avatarUpperAccessories = int.Parse(userData[Game_Over_2_Constants.DB_AVATR_UPPER_ACCESSORIES].ToString());

        return data;
    }

    private List<string> Get_Accounts_User(Dictionary<string, object> userKeys)
    {
        List<string> keyList = new List<string>(userKeys.Keys);
        return keyList;
    }

    public void LoadScoreBoardData()
    {
        StartCoroutine(LoadScoreBoardData_Async());
    }

    public IEnumerator LoadScoreBoardData_Async()
    {
        //Here you can add a loading animation
        Game_Over_2_CoroutineWithData cd = new Game_Over_2_CoroutineWithData(this, Game_Over_2_FB_Interactions.Get_Data(Game_Over_2_Constants.DB_LEADERBOARD + "/" + myUserData.gameRef));
        yield return cd.coroutine;

        //Debug.Log("Result: "+cd.result.ToString());
        //Here you can add a close of loading animation
        if (cd.result != null)
        {
            _leaderBoard = (Dictionary<string, object>)cd.result;
            isSuccessTransaction = true;
        }
        else
        {
            isFailureTransaction = true;
        }
    }
}