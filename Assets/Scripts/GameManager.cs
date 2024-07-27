using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton instance

    public int currentDay = 1; // Variable to store the current day

    public Image m_DarkImage;
    public GameObject m_moveButton;
    public float fadeDuration = 100.0f; // Increased duration of the fade effect

    private void Awake()
    {
        // Singleton pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Don't destroy GameManager when loading new scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        StartNewDay();
    }

    public void StartNewDay()
    {
        Debug.Log("Starting Day " + currentDay);
        // You can put any initialization code for the new day here

        // Increment the day counter
        currentDay++;
        // Start the fade-out effect
        m_moveButton.SetActive(false); // Ensure the move button is inactive at the start
        StartCoroutine(FadeOutDarkImage());
    }

    private IEnumerator FadeOutDarkImage()
    {
        Color color = m_DarkImage.color;
        float startAlpha = color.a;
        float rate = fadeDuration;
        float progress = 0.0f;

        while (progress < 1.0f)
        {
            color.a = Mathf.Lerp(startAlpha, 0, progress);
            m_DarkImage.color = color;
            progress += rate * Time.deltaTime;
            yield return null;
        }

        color.a = 0;
        m_DarkImage.color = color;
        m_moveButton.SetActive(true); // Activate the move button after the fade-out
    }

    public void RestartScene()
    {
        // Restart the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        StartNewDay();
    }
}
