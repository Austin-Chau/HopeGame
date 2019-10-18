using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    void OnEnable()
    {
        Time.timeScale = 0;
    }

    private void OnDisable()
    {
        Time.timeScale = 1;
    }

    public void Resume()
    {
        gameObject.SetActive(false);
    }

    public void QuitToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
