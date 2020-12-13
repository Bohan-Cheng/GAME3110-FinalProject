using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Script_Score : MonoBehaviour
{
    public static int Player1Score;
    public static int Player2Score;
    public TextMeshProUGUI ShowPlayer1Score;
    public TextMeshProUGUI ShowPlayer2Score;
    // Start is called before the first frame update
    void Start()
    {
        Player1Score = 0;
        Player2Score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        ShowPlayer1Score.text = Player1Score.ToString();
        ShowPlayer2Score.text = Player2Score.ToString();
    }
}
