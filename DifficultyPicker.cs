using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficultyPicker : MonoBehaviour
{
    public void SetDifficulty(string difficulty)
    {
        PlayerPrefs.SetString("SelectedDifficulty", difficulty);
        PlayerPrefs.Save();
        Debug.Log("Difficulty set to: " + difficulty);
        SceneManager.LoadSceneAsync(3);
    }
}
