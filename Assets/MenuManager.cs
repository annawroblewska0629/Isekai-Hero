using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject menu;
    [SerializeField] GameObject instruction;
    // Start is called before the first frame update
    void Start()
    {
        menu.SetActive(true);
        instruction.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Play()
    {
        SceneManager.LoadScene("LevelSelector");
    }

    public void Instruction()
    {
        menu.SetActive(false);
        instruction.SetActive(true);
    }

    public void Back()
    {
        menu.SetActive(true);
        instruction.SetActive(false);
    }
}
