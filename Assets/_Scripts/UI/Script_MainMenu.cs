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

    public void OnHostPressed()
    {
        GameObject.FindGameObjectWithTag("Login").GetComponent<Script_Login>().IsHost = true;
        SceneManager.LoadScene(StartMap);
    }

    public void OnFindPressed()
    {
        GameObject.FindGameObjectWithTag("Login").GetComponent<Script_Login>().IsHost = false;
        SceneManager.LoadScene(StartMap);

    }
}
