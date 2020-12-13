using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Script_Time : MonoBehaviour
{
    public int GameTime;
    public TextMeshProUGUI ShowGameTime;
    // Start is called before the first frame update
    void Start()
    {
        GameTime = 60;
    }

    // Update is called once per frame
    void Update()
    {
        while(GameTime > 0)
        {
            StartCoroutine(CountDown());
        }
        ShowGameTime.text = GameTime.ToString();
    }

    IEnumerator CountDown()
    {
        yield return new WaitForSeconds(1.0f);
        GameTime --;
    }
}
