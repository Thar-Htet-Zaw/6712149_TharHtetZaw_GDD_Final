using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    void Start()
    {
        Time.timeScale = 1f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void PlayDriving() => SceneManager.LoadScene("Prototype 1");
    public void PlayFlying()  => SceneManager.LoadScene("Challenge 1");
    public void PlaySumo()    => SceneManager.LoadScene("Prototype 4");

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}