using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Trainee_Horizon_FeridSpaceShip_UIManager : MonoBehaviour
{
    [SerializeField] private Text _prompMessageTxt;


    [SerializeField]
    private GameObject _changeClothes;
    [SerializeField]
    private GameObject _Key;
    [SerializeField]
    private GameObject _Achievement;
    [SerializeField]
    private GameObject _Dictionary;
    


    public void OpenCloseKeyPanel(bool isOpened)
    {
        Trainee_Horizon_FeridSpaceShip_SoundManager.instance.play(Trainee_Horizon_FeridSpaceShip_Constants.INTERACT);
        isOpened = !isOpened;
        _Key.SetActive(isOpened);
    }



    public void OpenCloseClothePanel(bool isOpened)
    {
        Trainee_Horizon_FeridSpaceShip_SoundManager.instance.play(Trainee_Horizon_FeridSpaceShip_Constants.INTERACT);
        isOpened = !isOpened;
        _changeClothes.SetActive(isOpened);
    }


    public void OpenCloseAchievementPanel(bool isOpened)
    {
        Trainee_Horizon_FeridSpaceShip_SoundManager.instance.play(Trainee_Horizon_FeridSpaceShip_Constants.INTERACT);
        isOpened = !isOpened;
        _Achievement.SetActive(isOpened);
    }

    public void OpenCloseDictionaryPanel(bool isOpened)
    {
        Trainee_Horizon_FeridSpaceShip_SoundManager.instance.play(Trainee_Horizon_FeridSpaceShip_Constants.INTERACT);
        isOpened = !isOpened;
        _Dictionary.SetActive(isOpened);
    }

    public void UpdatePrompMessage(string message)
    {
        _prompMessageTxt.text = message;
    }
   
}
