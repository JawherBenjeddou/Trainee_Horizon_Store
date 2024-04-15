using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Runtime.CompilerServices;
using UnityEngine.EventSystems;

public class Trainee_Horizon_FeridSpaceShip_PlayerInteraction : MonoBehaviour
{
    [Header("Scripts References")]
    private Camera _cam;
    [SerializeField] private Trainee_Horizon_FeridSpaceShip_UIManager _uiManeger;

    [Header("Raycast Parameters")]
    [SerializeField] private float _rayDistance = 10f;
    [SerializeField] private LayerMask _interactableLayer;
    [SerializeField] private Button _button;
    [SerializeField] private Image _image;
    //[SerializeField] private GameObject _joystick;

    private Trainee_Horizon_FeridSpaceShip_Outline _outline;
    private bool _playSfx = true;
    public float newAlpha;
    private bool buttonPressed;



    void Start()
    {
        _cam = Camera.main;

    }

    public void OnPointerDown()
    {
        buttonPressed = true;
    }
    public void InteractionRay()
    {
        _uiManeger.UpdatePrompMessage(string.Empty);
        //Create a ray at the center of the camera, shooting outwards
        Ray ray = new Ray(_cam.transform.position, _cam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * _rayDistance, Color.red);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, _rayDistance, _interactableLayer))
        {

            if (hitInfo.collider.GetComponent<Trainee_Horizon_FeridSpaceShip_Interactable>() != null)
            {

                Trainee_Horizon_FeridSpaceShip_Interactable interactable = hitInfo.collider.GetComponent<Trainee_Horizon_FeridSpaceShip_Interactable>();
                _outline = interactable.myOutline;
                interactable.myOutline.OutlineMode = Trainee_Horizon_FeridSpaceShip_Outline.Mode.OutlineAll;
                _uiManeger.UpdatePrompMessage(interactable.prompMessage);
                Color newColor = _button.image.color;
                newColor.a = newAlpha;
                _button.image.color = newColor;
                _image.color = newColor;
                

                if (_playSfx)
                {
                    Trainee_Horizon_FeridSpaceShip_SoundManager.instance.play(Trainee_Horizon_FeridSpaceShip_Constants.HIGHLIGHT);
                    _playSfx = false;
                }
                if (buttonPressed)
                {
                    interactable.BaseInteract();
                }
                buttonPressed = false;



            }


        }
        else
        {
            _playSfx = true;
            if (_outline != null)
                _outline.OutlineMode = Trainee_Horizon_FeridSpaceShip_Outline.Mode.SilhouetteOnly;
            Color oldColor = _button.image.color;
            oldColor.a = 0.3f;
            _button.image.color = oldColor;
            _image.color = oldColor;
        }
    }
}
