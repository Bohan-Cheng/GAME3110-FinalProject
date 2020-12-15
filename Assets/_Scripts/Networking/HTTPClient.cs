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

    public void Login(string Username, string Password, string Email)
    {
        StartCoroutine(LogInPost(Username, Password, Email));
    }

    public void AddScore(string Username, int score)
    {
        StartCoroutine(DoAddScore(Username, score));
    }

    IEnumerator DoAddScore(string Username, int score)
    {
        loginUser.user_id = Username;
        string jsonString = "{\"user_id\":\"" + Username + "\"," + "\"score\":\"" + score.ToString() + "\"}";
        byte[] myData = System.Text.Encoding.UTF8.GetBytes(jsonString);

        UnityWebRequest www = UnityWebRequest.Put("https://aja8khi382.execute-api.us-east-2.amazonaws.com/default/UserFunction2", myData);

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

            Debug.Log(recMsg);
        }
    }

    IEnumerator LogInPost(string Username, string Password, string Email)
    {
        loginUser.user_id = Username;
        loginUser.password = Password;
        loginUser.email = Email;
        string jsonString = "{\"user_id\":\"" + Username + "\"," + "\"password\":\"" + Password + "\"," + "\"email\":\"" + Email + "\"," + "\"rank\":\"" + "1" + "\"," + "\"score\":\"" + "0" + "\"}";
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
