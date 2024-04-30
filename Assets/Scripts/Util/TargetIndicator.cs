using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.horizon.store
{
    public class TargetIndicator : MonoBehaviour
    {
        public List<Transform> m_Targets; // List of targets
        [SerializeField]
        private float m_RotationSpeed;

        private int currentTargetIndex = 0; // Index of the current target

        void Update()
        {
            // Check if there are any targets in the list
            if (m_Targets.Count > 0)
            {
                // Get the current target from the list
                Transform currentTarget = m_Targets[currentTargetIndex];

                // Check if the current target is null
                if (currentTarget != null)
                {
                    // Calculate direction towards the current target
                    Vector3 direction = currentTarget.position - transform.position;
                    direction.y = 0f;

                    // Rotate towards the current target
                    if (direction != Vector3.zero)
                    {
                        Quaternion targetRotation = Quaternion.LookRotation(direction);
                        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, targetRotation.eulerAngles.y, 0), m_RotationSpeed * Time.deltaTime);
                    }
                }
                else
                {
                    // If current target is null, find the next non-null target
                    FindNextTarget();
                }
            }
            else
            {
                Debug.LogWarning("No targets assigned to the TargetIndicator script.");
            }
        }

        // Method to find the next non-null target
        private void FindNextTarget()
        {
            // Increment the index to move to the next target
            currentTargetIndex++;

            // Check if the index exceeds the bounds of the list
            if (currentTargetIndex >= m_Targets.Count)
            {
                // Reset index if it exceeds the bounds
                currentTargetIndex = 0;

                // Check if all targets are null
                bool allTargetsNull = true;
                foreach (Transform target in m_Targets)
                {
                    if (target != null)
                    {
                        allTargetsNull = false;
                        break;
                    }
                }

                // Destroy the GameObject if all targets are null
                if (allTargetsNull)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
