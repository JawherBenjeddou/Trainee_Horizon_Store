using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace com.horizon.store
{
    public class TargetIndicator : MonoBehaviour
    {
        public Transform m_Target;
        [SerializeField]
        private float m_RotationSpeed;

        void Update()
        {
            if (m_Target != null)
            {
                Vector3 direction = m_Target.position - transform.position;
                direction.y = 0f;

                if (direction != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(direction);

                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, targetRotation.eulerAngles.y, 0), m_RotationSpeed * Time.deltaTime);
                }
            }
            else
            {
                Debug.LogWarning("Target is not assigned to the TargetIndicator script.");
            }
        }
    }
}