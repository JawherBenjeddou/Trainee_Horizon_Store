
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Trainee_Horizon_FeridSpaceShip_InputManager : MonoBehaviour
{
    [Header("Inputs")]
    private float _horizontalInput;
    private float _verticalInput;
    


    [Header("Mouse Input")]
    private float _MouseY;
    private float _MouseX;

    [Header("Scripts Parameters")]
    private Trainee_Horizon_FeridSpaceShip_Mouvement _movement;
    private Trainee_Horizon_FeridSpaceShip_PlayerInteraction _interactionRay;
    private Trainee_Horizon_FeridSpaceShip_MouseLook _mouseLook;





    void Start()
    {
        _movement = GetComponent<Trainee_Horizon_FeridSpaceShip_Mouvement>();
        _mouseLook = GetComponent<Trainee_Horizon_FeridSpaceShip_MouseLook>();
        _interactionRay = GetComponent<Trainee_Horizon_FeridSpaceShip_PlayerInteraction>();

    }
    
    void FixedUpdate()
    {
#if UNITY_EDITOR

        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");
        Debug.Log(_verticalInput);
        _MouseX = Input.GetAxis("Mouse X");
        _MouseY = Input.GetAxis("Mouse Y");
        Vector2 currentInput = new Vector2(_verticalInput, _horizontalInput);
        Vector2 LookInput = new Vector3(_MouseX, _MouseY);

        
        _movement.HandleMouvement(currentInput);
        _mouseLook.HandleMouseLook(LookInput);
        _interactionRay.InteractionRay();


#else
        //_horizontalInput = _joystick.Horizontal;
        //_verticalInput = _joystick.Vertical;
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                int index = Input.touchCount - 1;
                if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                    return;
                _MouseX = Input.GetTouch(0).deltaPosition.x;
                _MouseY = Input.GetTouch(0).deltaPosition.y;
                Vector2 LookInput = new Vector3(_MouseX, _MouseY);
                _mouseLook.HandleMouseLook(LookInput);
            }
           // Vector2 currentInput = new Vector2(_verticalInput, _horizontalInput);
            _movement.HandleMouvement();
            _mouseLook.HandleGyro();
            _interactionRay.InteractionRay();
#endif
    }

}
