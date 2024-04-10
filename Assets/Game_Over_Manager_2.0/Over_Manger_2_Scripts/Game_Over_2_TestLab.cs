using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game_Over_2_TestLab : MonoBehaviour
{
    private bool _startFetching = false;

    private bool _userIdExists = false;
    private bool _userExists = false;
    private bool _userDoesNotExists = false;

    public InputField gameRefInput, userIDInput, childIDInput, nameInput, surnameInput, avatarIndexInput;
    public InputField bestScoreInput, xpInput, starsInput;

    private Dictionary<string, object> _dbUserData;

    private UserData _myUserData;
    private bool _isUserFetched;

    private void Update()
    {
        Debug.Log(gameObject.name);
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
            Debug.Log("Test1");
            _isUserFetched = true;
        }

        if (_userDoesNotExists)
        {
            //Create new user in database
            _userDoesNotExists = false;
            Create_New_User();
            Debug.Log("Test2");
            _isUserFetched = true;
        }

        if (_isUserFetched)
        {
            _isUserFetched = false;
            Game_Over_2_Constants.SET_KEYS(_myUserData);
            Game_Over_2_SaveSystem.Set_User_Data(_myUserData);
            Debug.Log(_myUserData.exp);
        }
    }

    private void Create_New_User()
    {
        StartCoroutine(Update_Player_Elements(int.Parse(bestScoreInput.text), int.Parse(xpInput.text), starsInput.text));
    }

    private void Load_User_Data(UserData userData)
    {
        userData.bestScore = int.Parse(_dbUserData[Game_Over_2_Constants.DB_BEST_SCORE].ToString());
        userData.stars = _dbUserData[Game_Over_2_Constants.DB_STARS].ToString();
        userData.exp = int.Parse(_dbUserData[Game_Over_2_Constants.DB_XP].ToString());
    }

    public IEnumerator Check_If_User_ID_Exists(string uId)
    {
        Debug.Log(_myUserData.gameRef);
        //Here you can add a loading animation
        Game_Over_2_CoroutineWithData cd = new Game_Over_2_CoroutineWithData(this, Game_Over_2_FB_Interactions.Get_Data(Game_Over_2_Constants.DB_LEADERBOARD + "/" + _myUserData.gameRef + "/" + uId));
        yield return cd.coroutine;

        //Here you can add a close of loading animation
        if (cd.result != null)
        {
            _userIdExists = true;
        }
        else
        {
            _userDoesNotExists = true;
        }
    }

    public IEnumerator Check_If_Child_Exists(string user)
    {
        Debug.Log(_myUserData.gameRef);
        //Here you can add a loading animation
        Game_Over_2_CoroutineWithData cd = new Game_Over_2_CoroutineWithData(this, Game_Over_2_FB_Interactions.Get_Data(Game_Over_2_Constants.DB_LEADERBOARD + "/" + _myUserData.gameRef + "/" + _myUserData.uId + "/" + user));
        yield return cd.coroutine;

        //Here you can add a close of loading animation
        if (cd.result != null)
        {
            Debug.Log("Test0");
            _dbUserData = (Dictionary<string, object>)cd.result;
            _userExists = true;
        }
        else
        {
            Debug.Log("Test4");
            _userDoesNotExists = true;
        }
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
        yield return Game_Over_2_FB_Interactions.Update_Dictionnary(path, childUpdates);
#endif
    }

    public void Insert_In_DB()
    {
        _myUserData = new UserData(gameRefInput.text, userIDInput.text, childIDInput.text, nameInput.text, surnameInput.text, int.Parse(avatarIndexInput.text), 0);
        _startFetching = true;
        StartCoroutine(Check_If_User_ID_Exists(_myUserData.uId));
    }

    public void Load_Real_Scene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}