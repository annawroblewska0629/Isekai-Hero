using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{

    [SerializeField] Button[] levelButtons;
    // Start is called before the first frame update
    void Start()
    {
        int activeLevel = PlayerPrefs.GetInt("activeLevel", 1);

        for(int i = 0; i < levelButtons.Length; i++)
        {
            if(i + 1 > activeLevel)
            {
                levelButtons[i].interactable = false;
            }
        }
    }

    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }
}
