using UnityEngine;

namespace com.horizon.store
{
    public class CashierController : MonoBehaviour
    {
        public GameObject m_Player;
        private Animator m_Animator;
        public BoxCollider m_Collider;
        private bool m_AnimationTriggered = false; // Flag to track if the animation has been triggered

        public ChatBubbleFX m_ChatBubbleFX;
        public Camera m_MainCamera;
        public Camera m_SecondCamera;

        // Start is called before the first frame update
        void Start()
        {
            m_Animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            if (m_Collider.bounds.Intersects(m_Player.GetComponent<CharacterController>().bounds))
            {
                // Check if the animation has not been triggered yet
                if (!m_AnimationTriggered)
                {
                    m_Animator.SetBool("InRange", true);
                    m_ChatBubbleFX.EnableChat();
                    m_AnimationTriggered = true; // Set the flag to true to indicate that the animation has been triggered
                }

                // Make the cashier character look at the player
                Vector3 directionToPlayer = m_Player.transform.position - transform.position;
                directionToPlayer.y = 0f; // Lock rotation only on the horizontal plane
                Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            }
            else
            {
                // If the player is not in range, reset the animation trigger flag
                m_Animator.SetBool("InRange", false);
                m_Animator.SetBool("IsPaying", false);
                m_ChatBubbleFX.DisableChat();
                m_AnimationTriggered = false;
            }
            // Check if the animation has ended
            if (IsAnimationFinished("Cashier_FlatSceen_Inputs_OneHandType"))
            {
                SwitchCameras();
            }

        }
        public void IsPaying()
        {
            m_Animator.SetBool("IsPaying", true);
        }
        public void IsNotPaying()
        {
            m_ChatBubbleFX.DisableChat();
        }

        // Function to check if the animation with the specified name has ended
        private bool IsAnimationFinished(string animationName)
        {
            AnimatorStateInfo currentState = m_Animator.GetCurrentAnimatorStateInfo(0);
            AnimatorClipInfo[] currentClipInfo = m_Animator.GetCurrentAnimatorClipInfo(0);

            foreach (var clip in currentClipInfo)
            {
                if (clip.clip.name == animationName && currentState.normalizedTime >= 1 && !m_Animator.IsInTransition(0))
                {
                    return true;
                }
            }
            return false;
        }
        // Function to switch between cameras
        private void SwitchCameras()
        {
            // Disable the main camera
            m_MainCamera.gameObject.SetActive(false);
            m_ChatBubbleFX.DisableChat();
            // Enable the second camera
            m_SecondCamera.gameObject.SetActive(true);
        }
    }
}
