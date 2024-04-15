using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Trainee_Horizon_FeridSpaceShip_Interactable : MonoBehaviour
{
    //read Only
    public string prompMessage;

    public Trainee_Horizon_FeridSpaceShip_Outline myOutline;

    public bool isInteracted = true;


    private void Start()
    {
        isInteracted = true;
        myOutline = GetComponent<Trainee_Horizon_FeridSpaceShip_Outline>();
    }

    public void BaseInteract()
    {
        Interact();
    }

    public virtual void Interact()
    {
        isInteracted = true;
    }


    //Write Only
    public string GetPrompMessage()
    {
        return prompMessage;
    }


}
