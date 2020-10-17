using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_NetworkManager : MonoBehaviour
{
    public bool IsHost = true;

    public GameObject Cam1;
    public GameObject Cam2;

    public GameObject Player1;
    public GameObject Player2;


    void ActivatePlayer(bool host)
    {
        if (host)
        {
            Cam1.SetActive(true);
            Player1.AddComponent<Script_PlayerControl>().IsHost = host;
        }
        else
        {
            Cam2.SetActive(true);
            Player2.AddComponent<Script_PlayerControl>().IsHost = host;
        }
    }


    void Start()
    {
        ActivatePlayer(IsHost);
    }
}
