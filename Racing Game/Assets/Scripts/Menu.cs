using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Menu : MonoBehaviour
{
    private void Start() {
        if(AudioManager.instance.isPlaying("Rev")) {
            AudioManager.instance.Stop("Rev");
        }

        if(AudioManager.instance.isPlaying("Crash")) {
            AudioManager.instance.Stop("Crash");
        }

        Cursor.lockState = CursorLockMode.None;
    }
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
