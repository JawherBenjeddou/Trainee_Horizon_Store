using UnityEngine;
using UnityEngine.SceneManagement;

namespace com.horizon.store
{
    public class Trainee_Horizon_FeridSpaceShip_Mouvement : MonoBehaviour
    {
        public BoxCollider m_LeaveStore;
        private CharacterController _characterController;
        //private Trainee_Horizon_FeridSpaceShip_SoundManager _soundsMan;
        private Game_Over_2_AudioManager _audioManager;
        private Camera _cam;
        public CharacterController playerController;
        public float moveSpeed = 10f;
        public Game_Over_2_GameInteraction m_GameInteraction;
        [Header("Mouvemnt Parameters")]
        [SerializeField] private float _walkSpeed = 3.0f;
        private Vector3 _moveDirection;
        private float _gravity = 30f;
        //private bool _canMove = true;

        [Header("HeadBob Parameters")]
        [SerializeField] private float _walkBobSpeed;
        [Range(0, 0.1f)]
        [SerializeField] private float _walkBobAmount;
        private float _timer;
        private float _defaultpos = 0;

        [Header("Field of view Parmeters")]
        [SerializeField] private float _fovValue = 95f;
        private float _initialFOV;

        [Header("Footsteps Parameters")]
        [SerializeField] private float _baseStepSpeed = 0.5f;
        private float _footStepTimer = 0;
        private float _getCurrentOffset => _baseStepSpeed;

        private bool move = false;
        private void Start()
        {
            //_soundsMan = Trainee_Horizon_FeridSpaceShip_SoundManager.instance;
            _audioManager = Game_Over_2_AudioManager.audioManInstance;
            _characterController = GetComponent<CharacterController>();
            _cam = Camera.main;
            _initialFOV = _cam.fieldOfView;
            _defaultpos = _cam.transform.localPosition.y;
        }


        public void PointerDown()
        {
            move = true;
        }

        public void PointerUp()
        {
            move = false;
        }

        public void HandleMouvement(Vector2 input)
        {



            input *= _walkSpeed;
            float moveDirectionY = _moveDirection.y;
            _moveDirection = (transform.TransformDirection(Vector3.forward * input.x) + (transform.TransformDirection(Vector3.right * input.y)));
            _moveDirection.y = moveDirectionY;

            _characterController.Move(_moveDirection * Time.deltaTime);
            if (Mathf.Abs(_moveDirection.x) > 0.1f || Mathf.Abs(_moveDirection.z) > 0.1f)
            {
                HandleHeadBob();
                DynamicFOV(_fovValue);
                ApplyGravity();
                HandleFootSteps();
            }
            else
            {
                DynamicFOV(_initialFOV);
            }
        }

        public void HandleMouvementPhone()
        {
            if (move)
            {
                _characterController.SimpleMove(transform.forward * _walkSpeed);
                if (_characterController == null)
                {
                    Debug.Log(_characterController.name + "is null");

                }
                else
                {
                    Debug.Log(_characterController.name + "is NOT null");

                }
                HandleHeadBob();
                DynamicFOV(_fovValue);
                ApplyGravity();
                HandleFootSteps();
            }
            else
            {
                DynamicFOV(_initialFOV);
            }
        }

        private void HandleHeadBob()
        {

            _timer += Time.deltaTime * _walkBobSpeed;
            _cam.transform.localPosition = new Vector3(_cam.transform.localPosition.x, _defaultpos + Mathf.Sin(_timer) * _walkBobAmount, _cam.transform.localPosition.z);
        }

        private void DynamicFOV(float fovvalue)
        {
            _cam.fieldOfView = Mathf.Lerp(_cam.fieldOfView, fovvalue, 10f * Time.deltaTime);
        }

        private void ApplyGravity()
        {
            if (!_characterController.isGrounded)
                _moveDirection.y -= _gravity * Time.deltaTime;
        }

        private void HandleFootSteps()
        {
            if (!_characterController.isGrounded) return;
            if (_moveDirection == Vector3.zero) return;
            _footStepTimer -= Time.deltaTime;
            if (_footStepTimer <= 0)
            {
                //_soundsMan.playOneShot(Trainee_Horizon_FeridSpaceShip_Constants.FOOT_STEPS);
                _audioManager.Play_Sfx(Trainee_Horizon_FeridSpaceShip_Constants.FOOT_STEPS);
                _footStepTimer = _getCurrentOffset;
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other == m_LeaveStore)
            {
                //if (SceneManager.GetActiveScene().buildIndex == 9)
                //{
                //    GameManager.instance.RestartScene();
                //}
                //else
                //{
                m_GameInteraction.Game_Over(true);
                //}
            }
        }
    }
}
