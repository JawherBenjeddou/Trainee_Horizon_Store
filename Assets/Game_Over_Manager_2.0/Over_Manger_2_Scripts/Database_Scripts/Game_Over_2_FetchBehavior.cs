using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UserData
{
    public string gameRef;
    public string uId;
    public string child;
    public string name;
    public string surName;
    public int bestScore;
    public int exp;
    public string stars;
    public int avatarUpperAccessories;
    public int avatarLowerAccessories;
    public int rank;

    public Dictionary<string, object> ToDictionary()
    {
        Dictionary<string, object> result = new Dictionary<string, object>();

        result[Game_Over_2_Constants.DB_NAME] = name;
        result[Game_Over_2_Constants.DB_SURNAME] = surName;
        result[Game_Over_2_Constants.DB_BEST_SCORE] = bestScore;
        result[Game_Over_2_Constants.DB_XP] = exp;
        result[Game_Over_2_Constants.DB_STARS] = stars;
        result[Game_Over_2_Constants.DB_AVATR_UPPER_ACCESSORIES] = avatarUpperAccessories;

        return result;
    }

    public UserData(string gameRef, string userID, string child, string name, string surName, int avatarUpperAccessories, int avatarLowerAccessories)
    {
        this.gameRef = gameRef;
        this.uId = userID;
        this.child = child;
        this.name = name;
        this.surName = surName;
        this.avatarUpperAccessories = avatarUpperAccessories;
        this.avatarLowerAccessories = avatarLowerAccessories;
    }

    public void Update_Values(int bestScore, int exp, string stars)
    {
        this.bestScore = bestScore;
        this.exp = exp;
        this.stars = stars;
    }

    public UserData()
    {
    }
}

public class Game_Over_2_FetchBehavior : MonoBehaviour
{
    private bool _startFetching = false;

    private bool _userIdExists = false;
    private bool _userExists = false;
    private bool _userDoesNotExists = false;
    private bool _isUserFetched = false;

    private Dictionary<string, object> _dbUserData;

    private UserData _myUserData;

    [SerializeField] private Game_Over_2_Web_fetchBehavior _webFetchBehavior;

    [SerializeField] private GameObject _homeBtn;

    private void Start()
    {

#if UNITY_WEBGL
      _homeBtn.SetActive(false);
#endif
    }
    public void Set_Me_Up(string gameRef, string userID, string childID, string name, string surName, int avatarUpperAccessories, int avatarLowerAccessories)
    {
        _myUserData = new UserData(gameRef, userID, childID, name, surName, avatarUpperAccessories, avatarLowerAccessories);
        Debug.Log("Game Ref: " + _myUserData.gameRef + "Uid: " + _myUserData.uId);
        StartCoroutine(Check_If_User_ID_Exists(_myUserData.uId));
        Game_Over_2_Constants.SET_KEYS(_myUserData);
        _startFetching = true;
    }

    private void LateUpdate()
    {
        if (_startFetching)
        {
            Existance_Behavior();
        }
    }

    private void Existance_Behavior()
    {
        if (_userIdExists)
        {
            _userIdExists = false;
            StartCoroutine(Check_If_Child_Exists(_myUserData.child));
        }

        if (_userExists)
        {
            //Load data from database
            _userExists = false;
            Load_User_Data(_myUserData);
            _isUserFetched = true;
        }

        if (_userDoesNotExists)
        {
            //Create new user in database
            _userDoesNotExists = false;
            Create_New_User();
            _isUserFetched = true;
        }

        if (_isUserFetched)
        {
            _isUserFetched = false;
            Game_Over_2_SaveSystem.Set_User_Data(_myUserData);
#if UNITY_WEBGL
            _webFetchBehavior.Fetch_Panel(false);
#endif
        }
    }

    private void Create_New_User()
    {
        StartCoroutine(Update_Player_Elements(0, 0, ""));
    }

    private void Load_User_Data(UserData userData)
    {
        Debug.Log(PlayerPrefs.GetInt("Key! " + Game_Over_2_Constants.GAME_DATA_BESTSCORE + " " + _dbUserData[Game_Over_2_Constants.DB_BEST_SCORE].ToString()));

        userData.bestScore = Check_User_Stats(PlayerPrefs.GetInt(Game_Over_2_Constants.GAME_DATA_BESTSCORE), int.Parse(_dbUserData[Game_Over_2_Constants.DB_BEST_SCORE].ToString()));
        userData.stars = Check_User_Stats(Game_Over_2_Constants.GAME_DATA_TOTAL_STARS);
        userData.exp = Check_User_Stats(PlayerPrefs.GetInt(Game_Over_2_Constants.GAME_DATA_EXPIRIENCE), int.Parse(_dbUserData[Game_Over_2_Constants.DB_XP].ToString()));

        StartCoroutine(Update_Player_Elements(userData.bestScore, userData.exp, userData.stars));
    }

    private int Check_User_Stats(int curentStats, int dbStats)
    {
        if (curentStats > dbStats)
        {
            return curentStats;
        }
        else
        {
            return dbStats;
        }
    }

    private string Check_User_Stats(string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            return PlayerPrefs.GetString(key);
        }
        else
        {
            return _dbUserData[Game_Over_2_Constants.DB_STARS].ToString();
        }
    }

    public IEnumerator Check_If_User_ID_Exists(string uId)
    {
        //Here you can add a loading animation
        Game_Over_2_CoroutineWithData cd = new Game_Over_2_CoroutineWithData(this, Game_Over_2_FB_Interactions.Get_Data(Game_Over_2_Constants.DB_LEADERBOARD + "/" + _myUserData.gameRef + "/" + uId));

        yield return cd.coroutine;

        //Here you can add a close of loading animation

#if UNITY_WEBGL
        if (cd.result.ToString() != "null")
        {
            _userIdExists = true;
        }
        else
        {
            _userDoesNotExists = true;
        }
#else
        if (cd.result != null)
        {
            _userIdExists = true;
        }
        else
        {
            _userDoesNotExists = true;
        }
#endif
    }

    public IEnumerator Check_If_Child_Exists(string user)
    {
        //Here you can add a loading animation
        Game_Over_2_CoroutineWithData cd = new Game_Over_2_CoroutineWithData(this, Game_Over_2_FB_Interactions.Get_Data(Game_Over_2_Constants.DB_LEADERBOARD + "/" + _myUserData.gameRef + "/" + _myUserData.uId + "/" + user));
        yield return cd.coroutine;

#if UNITY_WEBGL
        //Here you can add a close of loading animation
        if (cd.result.ToString() != "null")
        {
            _dbUserData = (Dictionary<string, object>)cd.result;
            _userExists = true;
        }
        else
        {
            _userDoesNotExists = true;
        }
#else
        if (cd.result != null)
        {
            _dbUserData = (Dictionary<string, object>)cd.result;
            _userExists = true;
        }
        else
        {
            _userDoesNotExists = true;
        }
#endif
    }

    private IEnumerator Update_Player_Elements(int bestScore, int exp, string stars)
    {
        _myUserData.Update_Values(bestScore, exp, stars);
        Dictionary<string, object> entryValues = _myUserData.ToDictionary();

        Dictionary<string, object> childUpdates = new Dictionary<string, object>();
        string path = Game_Over_2_Constants.DB_LEADERBOARD + "/" + _myUserData.gameRef + "/" + _myUserData.uId + "/" + _myUserData.child;
        childUpdates[path] = entryValues;

#if UNITY_WEBGL && !UNITY_EDITOR
        yield return Game_Over_2_FB_Interactions.Update_Dictionnary("/", childUpdates);
#else
        yield return Game_Over_2_FB_Interactions.Update_Dictionnary("/", childUpdates);
#endif
        _isUserFetched = true;
    }
}