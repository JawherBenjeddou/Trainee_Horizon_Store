
using UnityEngine;
using UnityEngine.EventSystems;

namespace com.horizon.store
{
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
        // private Trainee_Horizon_FeridSpaceShip_PlayerInteraction _interactionRay;
        private Trainee_Horizon_FeridSpaceShip_MouseLook _mouseLook;


        [SerializeField] private Game_Over_2_SpaceShipOptionPanel _spaceShipOptionPanel;


        void Start()
        {
            _movement = GetComponent<Trainee_Horizon_FeridSpaceShip_Mouvement>();
            _mouseLook = GetComponent<Trainee_Horizon_FeridSpaceShip_MouseLook>();
            //_interactionRay = GetComponent<Trainee_Horizon_FeridSpaceShip_PlayerInteraction>();

        }

        void FixedUpdate()
        {

#if UNITY_EDITOR

            _horizontalInput = Input.GetAxis("Horizontal");
            _verticalInput = Input.GetAxis("Vertical");

            _MouseX = Input.GetAxis("Mouse X");
            _MouseY = Input.GetAxis("Mouse Y");
            Vector2 currentInput = new Vector2(_verticalInput, _horizontalInput);
            Vector2 LookInput = new Vector3(_MouseX, _MouseY);


            _movement.HandleMouvement(currentInput);
            if (!_spaceShipOptionPanel.gyroOff)
            {
                _mouseLook.HandleGyro();
            }
            _mouseLook.HandleMouseLook(LookInput);
            //_interactionRay.InteractionRay();


#else
        _movement.HandleMouvementPhone();

        if (!_spaceShipOptionPanel.gyroOff)
        {
            Debug.Log("gyroOn");
            _mouseLook.HandleGyro();
        }
       //_interactionRay.InteractionRay();

        if (Input.touchCount > 0)
        {

            Touch touch;
            touch = Input.GetTouch(0);

            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                if (Input.touchCount == 1)
                {
                    return;
                }
                else touch = Input.GetTouch(1);
            }

            // Handle camera rotation
            if (touch.phase == TouchPhase.Moved)
            {
                _MouseX = touch.deltaPosition.x;
                _MouseY = touch.deltaPosition.y;
                Vector2 LookInput = new Vector2(_MouseX, _MouseY);
                _mouseLook.HandleMouseLook(LookInput);
            }

        }
#endif


        }
    }
}
