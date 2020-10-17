using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Script_MainMenu : MonoBehaviour
{
    public string StartMap;


    // Go to the game map
    public void OnStartPressed()
    {
        SceneManager.LoadScene(StartMap);
    }

    // Quit the game
    public void OnExitPressed()
    {
        Application.Quit();
    }
}
