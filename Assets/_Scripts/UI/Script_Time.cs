using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Script_Time : MonoBehaviour
{
    public int Min;
    private int Sec = 59;
    public TextMeshProUGUI ShowGameTime;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CountDown());
    }

    IEnumerator CountDown()
    {
        while(Sec > 0 && Min > 0)
        {
            Sec--;
            if(Sec <= 0)
            {
                Min--;
                Sec = 59;
            }
            string timeText = Min.ToString() + " : " + Sec.ToString();
            ShowGameTime.text = timeText;
            yield return new WaitForSeconds(1f);
        }
    }
}
