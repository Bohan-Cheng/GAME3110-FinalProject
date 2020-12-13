using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_NetworkManager : MonoBehaviour
{
    public GameObject Cam1;
    public GameObject Cam2;

    public GameObject Player1;
    public GameObject Player2;

    public int CurentPlayers = 0;

    private Script_Login loginInfo;


    public void ActivatePlayer(bool host)
    {
        if (host)
        {
            Cam1.SetActive(true);
            Player1.AddComponent<Script_PlayerControl>().SetHost(host);
        }
        else
        {
            Cam2.SetActive(true);
            Player2.AddComponent<Script_PlayerControl>().SetHost(host);
        }
    }


    void Start()
    {
        loginInfo = GameObject.FindGameObjectWithTag("Login").GetComponent<Script_Login>();
        ActivatePlayer(loginInfo.IsHost);
    }
}
