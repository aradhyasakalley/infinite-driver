using UnityEngine;
using UnityEngine.SceneManagement;
public class Retry : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(2);
    }
}
