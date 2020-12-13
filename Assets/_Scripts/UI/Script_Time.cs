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
        GameTime = 59;
        StartCoroutine(CountDown());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator CountDown()
    {
        while(GameTime >= 0)
        {
            ShowGameTime.text = GameTime.ToString();
            yield return new WaitForSeconds(1f);
            GameTime--;
        }
    }
}
