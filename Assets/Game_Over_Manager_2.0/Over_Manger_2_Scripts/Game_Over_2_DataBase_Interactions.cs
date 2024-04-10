using System.Collections.Generic;
using System.Collections;
using System;
using System.Linq;
using UnityEngine;
using Unity.Mathematics;

public class Game_Over_2_DataBase_Interactions : MonoBehaviour
{
    [SerializeField] private Game_Over_2_DataBase_Manager _dbManager;

    [SerializeField] private Game_Over_2_AlertPanel _alertPanel;

    [SerializeField] private Game_Over_2_User_Element _userElement;
    [SerializeField] private GameObject _elementPrefab;
    [SerializeField] private Transform _tableFrame;
    [SerializeField] private GameObject _rankingPanel;

    public void Open_close_Scoreboard(bool isOpned, string gameRef)
    {
        if (isOpned)
        {
            if (Application.internetReachability != NetworkReachability.NotReachable)
                _dbManager.LoadScoreBoardData();
            else
            {
                _userElement.gameObject.SetActive(false);
                _alertPanel.Internet_Down(true);
            }
        }
        else
        {
            foreach (Transform child in _tableFrame)
            {
                Destroy(child.gameObject);
            }
        }
        _rankingPanel.SetActive(isOpned);
        _alertPanel.Content(isOpned);
    }

    private void Update()
    {
        Transaction_Behavior();
    }

    private void Transaction_Behavior()
    {
        if (_dbManager.isSuccessTransaction)
        {
            //Do the success transaction code
            _dbManager.isSuccessTransaction = false;

            _dbManager.Fech_Players_Data();
            Set_Player_Data_Element(_dbManager.myUserData);
            Debug.Log("Oh YES..");
            _alertPanel.Loading(false);
        }

        if (_dbManager.isFailureTransaction)
        {
            _alertPanel.Loading(false);
            Debug.Log("Oh NO..");
            _dbManager.isFailureTransaction = false;
        }
    }

    private void Set_Player_Data_Element(UserData userData)
    {
        List<UserData> users = _dbManager.Users_Data();
        IEnumerable<UserData> user = users.OrderByDescending(PlayerData => PlayerData.bestScore);
        int rank = 1;
        foreach (UserData PlayerData in user)
        {
            GameObject newElement = Instantiate(_elementPrefab, _tableFrame);

            if (PlayerData.uId == userData.uId && PlayerData.child == userData.child)
            {
                _userElement.Assgin_Player_Stats(PlayerData.name, PlayerData.bestScore, rank, PlayerData.avatarUpperAccessories);
            }

            newElement.GetComponent<Game_Over_2_ScoreboardElement>().Set_User_Data(PlayerData.name, PlayerData.bestScore, PlayerData.exp, PlayerData.avatarUpperAccessories, rank);
            rank++;
        }

        users.Clear();
    }
}