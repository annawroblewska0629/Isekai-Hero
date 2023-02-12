using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameResumed;
    private bool isGamePaused = false;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject playerUI;
    public static PauseMenu Instance { get; private set; }
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one PauseMenu! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;

    }
    // Update is called once per frame
    private void Start()
    {
        pauseMenu.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("klik");
            if (isGamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        playerUI.SetActive(true);
        Time.timeScale = 1f;
        isGamePaused = false;
        OnGameResumed?.Invoke(this, EventArgs.Empty);
    }

    private void Pause()
    {
        pauseMenu.SetActive(true);
        playerUI.SetActive(false);
        Time.timeScale = 0f;
        isGamePaused = true;
        OnGamePaused?.Invoke(this, EventArgs.Empty);
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
