using System.Collections;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public float m_shakeAmount = 0.9f;
    public float m_shakeSpeed = 0.6f;
    private Quaternion m_originalRotation;

    private static ScreenShake m_instance;

    public static ScreenShake Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<ScreenShake>();
                if (m_instance == null)
                {
                    GameObject singleton = new GameObject("ScreenShake");
                    m_instance = singleton.AddComponent<ScreenShake>();
                }
            }
            return m_instance;
        }
    }

    void Awake()
    {
        if (m_instance != null && m_instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            m_instance = this;
        }

        m_originalRotation = Camera.main.transform.rotation;
    }

    public void Shake()
    {
        StartCoroutine(ShakeScreen());
    }

    IEnumerator ShakeScreen()
    {
        float elapsedTime = 0f;

        while (elapsedTime < m_shakeSpeed)
        {
            float newYaw = m_originalRotation.eulerAngles.y + Mathf.Sin(elapsedTime * Mathf.PI * 2f / m_shakeSpeed) * m_shakeAmount;
            Quaternion newRotation = Quaternion.Euler(m_originalRotation.eulerAngles.x, newYaw, m_originalRotation.eulerAngles.z);
            Camera.main.transform.rotation = newRotation;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Camera.main.transform.rotation = m_originalRotation;
    }
}
