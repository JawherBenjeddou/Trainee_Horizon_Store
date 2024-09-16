using UnityEngine;

namespace com.horizon.store
{
    public class NPCController : MonoBehaviour
    {
        public Transform[] waypoints; // Array to hold the path waypoints
        public float moveSpeed = 3f; // Speed at which the NPC moves
        private int currentWaypointIndex = 0; // Index of the current waypoint

        void Update()
        {
            // Move NPC towards the current waypoint
            transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypointIndex].position, moveSpeed * Time.deltaTime);

            // Check if NPC has reached the current waypoint
            if (Vector3.Distance(transform.position, waypoints[currentWaypointIndex].position) < 0.1f)
            {
                // Move to the next waypoint
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            }
        }
    }
}
