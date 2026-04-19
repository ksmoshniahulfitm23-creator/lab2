using UnityEngine;
using UnityEngine.SceneManagement;

public class WinUI : MonoBehaviour
{
    public GameObject winPanel;

    public void ShowWin()
    {
        winPanel.SetActive(true);
        Time.timeScale = 0f; // остановить игру
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0); // вернуться на Level1
    }
}