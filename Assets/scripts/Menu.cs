using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void Play() {
        SceneManager.LoadSceneAsync(1);
    }

    public void Exit() {
        Application.Quit();
    }
}
