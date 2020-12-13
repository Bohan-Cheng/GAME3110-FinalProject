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
        if(welcomeNammeText)
        {
            welcomeNammeText.text = GameObject.FindGameObjectWithTag("Login").GetComponent<Script_Login>().http.loginUser.user_id;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
