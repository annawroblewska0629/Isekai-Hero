using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] Player player;


    // Start is called before the first frame update
    void Start()
    {
        player.OnPlayerDead += Player_OnPlayerDead;

    }


    private void Player_OnPlayerDead(object sender, EventArgs e)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextLevel(string nextLevelName)
    {
        Debug.Log("nowy level");
        SceneManager.LoadScene(nextLevelName);
    }
}
