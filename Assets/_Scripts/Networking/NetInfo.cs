using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetInfo : MonoBehaviour
{
    public string playerID;
    public string playerEmail;
    public string playerRank;
    public string playerScore;
    public string localID;
    public string serverID;


    public int AddScore()
    {
        int score = int.Parse(playerScore);
        score++;
        playerScore = score.ToString();
        return score;
    }
}
