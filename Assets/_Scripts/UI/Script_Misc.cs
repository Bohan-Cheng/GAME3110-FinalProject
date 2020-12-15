using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Script_Misc : MonoBehaviour
{
    [SerializeField] Text welcomeNammeText;
    [SerializeField] Text profileText;
    [SerializeField] Text emailText;
    // Start is called before the first frame update
    void Start()
    {
        Script_Login login = GameObject.FindGameObjectWithTag("Login").GetComponent<Script_Login>();
        if (welcomeNammeText)
        {
            welcomeNammeText.text = login.http.loginUser.user_id;
        }
        if (profileText)
        {
            profileText.text = "Rank: " + login.http.loginUser.rank + "      Score: "  + login.http.loginUser.score;
        }
        if (emailText)
        {
            emailText.text = login.http.loginUser.email;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
