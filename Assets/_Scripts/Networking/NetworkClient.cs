using UnityEngine;
using Unity.Collections;
using Unity.Networking.Transport;
using NetworkMessages;
using NetworkObjects;
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine.SceneManagement;

public class NetworkClient : MonoBehaviour
{
    public NetworkDriver m_Driver;
    public NetworkConnection m_Connection;
    public string serverIP;
    public ushort serverPort;
    private string PlayerID;

    [SerializeField] GameObject Cam1;
    [SerializeField] GameObject Cam2;

    public int CurentPlayers = 0;

    private Script_Login loginInfo;


    GameObject playerGO;
    NetInfo playerInfo;

    public List<GameObject> AllPlayersGO = new List<GameObject>();

    [SerializeField] GameObject Ball;
    [SerializeField] TextMeshProUGUI player1Name;
    [SerializeField] TextMeshProUGUI player2Name;


    void Start ()
    {
        Debug.Log("Initialized.");
        loginInfo = GameObject.FindGameObjectWithTag("Login").GetComponent<Script_Login>();
        
        if(loginInfo.serverIP.Length > 5)
        {
            if(loginInfo.serverIP == "localhost")
            {
                serverIP = "127.0.0.1";
            }
            else
            {
                serverIP = loginInfo.serverIP;
            }
        }

        m_Driver = NetworkDriver.Create();
        m_Connection = default(NetworkConnection);
        var endpoint = NetworkEndPoint.Parse(serverIP,serverPort);
        m_Connection = m_Driver.Connect(endpoint);
    }

    void SendToServer(string message){
        var writer = m_Driver.BeginSend(m_Connection);
        NativeArray<byte> bytes = new NativeArray<byte>(Encoding.ASCII.GetBytes(message),Allocator.Temp);
        writer.WriteBytes(bytes);
        m_Driver.EndSend(writer);
    }

    void OnConnect(){
        Debug.Log("Connected to the server.");
        PlayerID = GameObject.FindGameObjectWithTag("Login").GetComponent<Script_Login>().http.loginUser.user_id;
        //SpawnPlayer();
        StartMatch();
        InvokeRepeating("HandShake", 0.0f, 2.0f);
        InvokeRepeating("UpdateStats", 0.0f, 1.0f/30.0f);
    }

    void StartMatch()
    {
        if (loginInfo.IsHost)
        {
            player1Name.text = PlayerID;
            Cam1.SetActive(true);
            AllPlayersGO[0].AddComponent<Script_PlayerControl>().SetHost(true);

            playerGO = AllPlayersGO[0];
            Match match = new Match();
            match.Host = loginInfo.http.loginUser;
            match.ID = loginInfo.http.loginUser.user_id + "'s match";
            match.IsAvailable = true;

            HostGameMsg m = new HostGameMsg();
            m.Game = match;
            GetComponent<NetworkClient>().SendToServer(JsonUtility.ToJson(m));
        }
        else
        {
            player2Name.text = PlayerID;
            Cam2.SetActive(true);
            AllPlayersGO[1].AddComponent<Script_PlayerControl>().SetHost(false);

            playerGO = AllPlayersGO[1];
            Debug.Log("Get Server stuff here!");
        }

        SetPlayerInformation();
    }

    void SetPlayerInformation()
    {
        playerInfo = playerGO.GetComponent<NetInfo>();
        playerInfo.localID = m_Connection.InternalId.ToString();
        playerInfo.playerID = PlayerID;
        playerInfo.playerEmail = loginInfo.http.loginUser.email;
        playerInfo.playerRank = loginInfo.http.loginUser.rank;
        playerInfo.playerScore = loginInfo.http.loginUser.score;
        PlayerSpawnMsg sm = new PlayerSpawnMsg();
        User u = new User();
        u.user_id = PlayerID;
        u.email = loginInfo.http.loginUser.email;
        u.rank = loginInfo.http.loginUser.rank;
        u.score = loginInfo.http.loginUser.score;
        sm.user = u;
        SendToServer(JsonUtility.ToJson(sm));
    }

    void SpawnOtherPlayer(PlayerSpawnMsg msg)
    {

        if (msg.user.user_id != PlayerID)
        {
            if (loginInfo.IsHost)
            {
                if(msg.user.rank == playerInfo.playerRank)
                {
                    AllPlayersGO[1].GetComponent<NetInfo>().playerID = msg.user.user_id;
                    AllPlayersGO[1].GetComponent<NetInfo>().playerEmail = msg.user.email;
                    AllPlayersGO[1].GetComponent<NetInfo>().playerRank = msg.user.rank;
                    AllPlayersGO[1].GetComponent<NetInfo>().playerScore = msg.user.score;
                    player2Name.text = msg.user.user_id;
                }
            }
            else
            {
                if (msg.user.rank == playerInfo.playerRank)
                {
                    AllPlayersGO[0].GetComponent<NetInfo>().playerID = msg.user.user_id;
                    AllPlayersGO[0].GetComponent<NetInfo>().playerEmail = msg.user.email;
                    AllPlayersGO[0].GetComponent<NetInfo>().playerRank = msg.user.rank;
                    AllPlayersGO[0].GetComponent<NetInfo>().playerScore = msg.user.score;
                    player1Name.text = msg.user.user_id;
                }
                else
                {
                    SceneManager.LoadScene("S_CreateFind");
                }
            }
        }
    }

    void UpdateOtherPlayer(UpdateStatsMsg msg)
    {
        if (msg.ID != PlayerID)
        {
            if(!loginInfo.IsHost)
            {
                Ball.transform.position = msg.BallPosition;
            }
            GameObject Obj = FindPlayerObj(msg.ID);
            if(Obj)
            {
                Obj.transform.position = msg.Position;
            }
        }
    }

    GameObject FindPlayerObj(string ID)
    {
        foreach (GameObject go in AllPlayersGO)
        {
            if(go.GetComponent<NetInfo>().playerID == ID)
            {
                return go;
            }
        }
        return null;
    }

    void OnData(DataStreamReader stream){
        NativeArray<byte> bytes = new NativeArray<byte>(stream.Length,Allocator.Temp);
        stream.ReadBytes(bytes);
        string recMsg = Encoding.ASCII.GetString(bytes.ToArray());
        NetworkHeader header = JsonUtility.FromJson<NetworkHeader>(recMsg);

        switch(header.cmd){
            case Commands.HANDSHAKE:
                HandshakeMsg hsMsg = JsonUtility.FromJson<HandshakeMsg>(recMsg);
                //Debug.Log("Handshake message received!");
            break;

            case Commands.PLAYER_UPDATE:
                PlayerUpdateMsg puMsg = JsonUtility.FromJson<PlayerUpdateMsg>(recMsg);
                Debug.Log("Player update message received!");
            break;

            case Commands.SERVER_UPDATE:
                ServerUpdateMsg suMsg = JsonUtility.FromJson<ServerUpdateMsg>(recMsg);
                Debug.Log("Server update message received!");
            break;

            case Commands.REQUEST_ID:
                RequestIDMsg riMsg = JsonUtility.FromJson<RequestIDMsg>(recMsg);
                playerInfo.serverID = riMsg.ID;
                Debug.Log("Request ID message received!");
            break;

            case Commands.PLAYER_SPAWN:
                PlayerSpawnMsg psMsg = JsonUtility.FromJson<PlayerSpawnMsg>(recMsg);
                SpawnOtherPlayer(psMsg);
            break;

            case Commands.UPDATE_STATS:
                UpdateStatsMsg usMsg = JsonUtility.FromJson<UpdateStatsMsg>(recMsg);
                UpdateOtherPlayer(usMsg);
            break;

            case Commands.PLAYER_DC:
                PlayerDCMsg pdMsg = JsonUtility.FromJson<PlayerDCMsg>(recMsg);
                KillPlayer(pdMsg);
            break;

            default:
                Debug.Log("Unrecognized message received!");
            break;
        }
    }

    void Disconnect(){
        m_Connection.Disconnect(m_Driver);
        m_Connection = default(NetworkConnection);
    }

    void OnDisconnect(){
        Debug.Log("Client got disconnected from server");
        DC();
        m_Connection = default(NetworkConnection);
    }

    public void OnDestroy()
    {
        m_Driver.Dispose();
    }   

    void HandShake()
    {
        //// Example to send a handshake message:
        HandshakeMsg m = new HandshakeMsg();
        m.player.id = m_Connection.InternalId.ToString();
        SendToServer(JsonUtility.ToJson(m));
    }

    void KillPlayer(PlayerDCMsg msg)
    {
        Destroy(FindPlayerObj(msg.PlayerID));
    }

    void DC()
    {
        //// Example to send a handshake message:
        PlayerDCMsg m = new PlayerDCMsg();
        m.PlayerID = PlayerID;
        SendToServer(JsonUtility.ToJson(m));
        Invoke("ExitGame", 2.0f);
    }

    void UpdateStats()
    {
        UpdateStatsMsg m = new UpdateStatsMsg();
        m.ID = PlayerID;
        m.Position = playerGO.transform.position;
        if (loginInfo.IsHost)
        {
            m.BallPosition = Ball.transform.position;
        }
        SendToServer(JsonUtility.ToJson(m));
    }

    void Update()
    {
        if(AllPlayersGO[0].GetComponent<NetInfo>().playerID.Length > 0 && AllPlayersGO[1].GetComponent<NetInfo>().playerID.Length > 0)
        {
            Ball.GetComponent<Script_Ball>().ShouldStart = true;
        }

        m_Driver.ScheduleUpdate().Complete();

        if (!m_Connection.IsCreated)
        {
            return;
        }

        DataStreamReader stream;
        NetworkEvent.Type cmd;
        cmd = m_Connection.PopEvent(m_Driver, out stream);
        while (cmd != NetworkEvent.Type.Empty)
        {
            if (cmd == NetworkEvent.Type.Connect)
            {
                OnConnect();
            }
            else if (cmd == NetworkEvent.Type.Data)
            {
                OnData(stream);
            }
            else if (cmd == NetworkEvent.Type.Disconnect)
            {
                DC();
                OnDisconnect();
            }

            cmd = m_Connection.PopEvent(m_Driver, out stream);
        }

        if(Input.GetKeyDown(KeyCode.Q))
        {
            DC();
        }

    }

    void ExitGame()
    {
        Application.Quit();
    }
}