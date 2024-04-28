using UnityEngine;

namespace com.horizon.store
{
    public class CashierController : MonoBehaviour
    {
        public GameObject m_Player;
        private Animator m_Animator;
        public BoxCollider m_Collider;
        private bool m_AnimationTriggered = false; // Flag to track if the animation has been triggered

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
                m_AnimationTriggered = false;
            }
        }
    }
}
