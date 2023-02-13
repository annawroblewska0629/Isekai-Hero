using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] string nextLevel;
    [SerializeField] int levelToUnlock;
    public event EventHandler OnGameRestarted;
    // Start is called before the first frame update
    void Start()
    {
        player.OnPlayerDead += Player_OnPlayerDead;

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            OnGameRestarted?.Invoke(this, EventArgs.Empty);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        
    }

    private void Player_OnPlayerDead(object sender, EventArgs e)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextLevel(string nextLevelName)
    {
        Debug.Log("nowy level");
        PlayerPrefs.SetInt("activeLevel", levelToUnlock);
        SceneManager.LoadScene(nextLevelName);
    }
}
