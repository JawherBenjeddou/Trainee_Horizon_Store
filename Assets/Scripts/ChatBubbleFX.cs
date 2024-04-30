using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.horizon.store {

    public class ChatBubbleFX : MonoBehaviour
    {
        public GameObject target;
        public Vector3 offset;
        public Canvas m_Chat;
        void Start()
        {
            gameObject.SetActive(false);
            m_Chat.gameObject.SetActive(false);
        }

        void Update()
        {
            if (target != null)
                transform.position = target.transform.position + offset;

            Camera cam = Camera.main;
            if (cam != null)
            {
                transform.rotation = Quaternion.LookRotation(cam.transform.forward, Vector3.up);
            }
        }

        public void EnableChat()
        {
            gameObject.SetActive(true);
            m_Chat.gameObject.SetActive(true);
        }
        public void DisableChat()
        {
            gameObject.SetActive(false);
            m_Chat.gameObject.SetActive(false);
        }
    }
}
