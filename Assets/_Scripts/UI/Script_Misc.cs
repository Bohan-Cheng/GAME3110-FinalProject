using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Script_Misc : MonoBehaviour
{
    [SerializeField] Text welcomeNammeText;
    // Start is called before the first frame update
    void Start()
    {
        Script_Login login = GameObject.FindGameObjectWithTag("Login").GetComponent<Script_Login>();
        if (welcomeNammeText)
        {
            welcomeNammeText.text = login.http.loginUser.user_id;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
