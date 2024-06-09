using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    private bool isPaused;
    private CanvasGroup cg;

    private void Awake()
    {
        Time.timeScale = 1.0f;
    }

    // Start is called before the first frame update
    void Start()
    {
        cg = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

    public void TogglePauseMenu()
    {
        if (!isPaused)
        {
            isPaused = true;
            Time.timeScale = 0f;

            cg.alpha = 1f;
            cg.blocksRaycasts = true;
            cg.interactable = true;
        } 
        else
        {
            isPaused = false;
            Time.timeScale = 1f;

            cg.alpha = 0f;
            cg.blocksRaycasts = false;
            cg.interactable = false;
        }
    }

    public void ReturnToMainMenu()
    {
        if (AudioController.Instance)
            AudioController.Instance.PlayBGM("MainMenuTheme");

        SceneManager.LoadSceneAsync(0);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public bool IsPaused()
    {
        return isPaused;
    }
}
