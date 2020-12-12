using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.Networking;
using NetworkMessages;

public class HTTPClient : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LogIn("Bohan Player3", "125555555haha*"));
    }

    IEnumerator LogIn(string Username, string Password)
    {
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
            Debug.Log("Username: " + user.user_id + " Password: " + user.password);
        }
    }
}
