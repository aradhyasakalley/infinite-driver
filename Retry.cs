using UnityEngine;
using UnityEngine.SceneManagement;
public class Retry : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(2);
    }
    public void backToMainMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }
    public void changeVehicle()
    {
        SceneManager.LoadSceneAsync(1);
    }
}
