using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.Networking;
using NetworkMessages;

public class HTTPClient : MonoBehaviour
{
    public User loginUser;

    private static HTTPClient instance;
    void Awake()
    {
        DontDestroyOnLoad(this);
        if (instance == null) { instance = this; }
        else { Destroy(this); }
    }

    public void Login(string Username, string Password)
    {
        StartCoroutine(LogInPost(Username, Password));
    }

    IEnumerator LogInPost(string Username, string Password)
    {
        loginUser.user_id = Username;
        loginUser.password = Password;
        string jsonString = "{\"user_id\":\"" + Username + "\"," + "\"password\":\"" + Password + "\"}";
        byte[] myData = System.Text.Encoding.UTF8.GetBytes(jsonString);

        UnityWebRequest www = UnityWebRequest.Put("https://gawmvof8wi.execute-api.us-east-2.amazonaws.com/default/UserFunction", myData);

        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;

            string recMsg = Encoding.ASCII.GetString(results);
            User user = JsonUtility.FromJson<User>(recMsg);
            LoginMsg login = JsonUtility.FromJson<LoginMsg>(recMsg);
            if (user.user_id != "")
            {
                loginUser = user;
                GetComponent<Script_Login>().LogedIn();
            }

            if(login.code != "-1")
            {
                GetComponent<Script_Login>().LogInEvent(login);
            }
            
            //Debug.Log("Username: " + user.user_id + " Password: " + user.password);
        }
    }

}
