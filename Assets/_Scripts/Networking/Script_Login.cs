using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NetworkMessages;
using UnityEngine.SceneManagement;

public class Script_Login : MonoBehaviour
{
    public HTTPClient http;

    [SerializeField] string GameMapName;
    [SerializeField] Text UsernameText;
    [SerializeField] Text PasswordText;
    [SerializeField] Text LoginMsgText;
    [SerializeField] Text IPText;

    public string serverIP;

    public bool IsHost = false;
    // Start is called before the first frame update
    void Start()
    {
        http = GetComponent<HTTPClient>();
    }

    public void LoginButton()
    {
        serverIP = IPText.text;
        http.Login(UsernameText.text, PasswordText.text);
    }

    public void LogedIn()
    {
        if (http.loginUser != null)
        {
            Debug.Log("You have loged in as: " + http.loginUser.user_id);
            FindMatch();
        }
    }

    public void LogInEvent(LoginMsg login)
    {
        Debug.Log(login.msg);
        LoginMsgText.text = login.msg;
        Invoke("ClearMsg", 3.0f);
        switch (login.code)
        {
            // Failed to login
            case "0":

                break;

            // Wrong password
            case "1":

                break;

            // User created
            case "2":
                FindMatch();
                break;

            // Failed to create user
            case "3":

                break;
        }
    }

    void FindMatch()
    {
        SceneManager.LoadScene(GameMapName);
    }

    void ClearMsg()
    {
        LoginMsgText.text = "";
    }
}
