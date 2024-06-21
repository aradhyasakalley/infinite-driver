using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public TextMeshProUGUI highScoreText;

    void Start()
    {
        UpdateHighScoreText();
    }

    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    void UpdateHighScoreText()
    {
        if (highScoreText != null)
        {
            highScoreText.text = "High Score: " + HighScoreManager.GetHighScore().ToString();
        }
        else
        {
            Debug.LogError("High Score Text component not assigned in the Inspector!");
        }
    }
}
