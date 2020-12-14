using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NetworkMessages
{
    public enum Commands{
        PLAYER_UPDATE,
        PLAYER_DC,
        SERVER_UPDATE,
        HANDSHAKE,
        PLAYER_INPUT,
        PLAYER_SPAWN,
        REQUEST_ID,
        UPDATE_STATS,
        PLAYER_HOST,
        PLAYER_JOIN
    }

    [System.Serializable]
    public class NetworkHeader{
        public Commands cmd;
    }

    [System.Serializable]
    public class HandshakeMsg:NetworkHeader{
        public NetworkObjects.NetworkPlayer player;
        public HandshakeMsg(){      // Constructor
            cmd = Commands.HANDSHAKE;
            player = new NetworkObjects.NetworkPlayer();
        }
    }

    [System.Serializable]
    public class User
    {
        public string user_id = "";
        public string password = "";
    }

    [System.Serializable]
    public class Match
    {
        public string ID;
        public User Host;
        public User Chanllenger;
        public bool IsAvailable;
    }

    [System.Serializable]
    public class LoginMsg
    {
        public string code = "-1";
        public string msg = "";
    }


    [System.Serializable]
    public class RequestIDMsg : NetworkHeader
    {
        public string ID;
        public RequestIDMsg()
        {      // Constructor
            cmd = Commands.REQUEST_ID;
        }
    }

    [System.Serializable]
    public class HostGameMsg : NetworkHeader
    {
        public Match Game;
        public HostGameMsg()
        {      // Constructor
            cmd = Commands.PLAYER_HOST;
        }
    }

    [System.Serializable]
    public class JoinGameMsg : NetworkHeader
    {
        public User Challenger;
        public JoinGameMsg()
        {      // Constructor
            cmd = Commands.PLAYER_JOIN;
        }
    }

    [System.Serializable]
    public class UpdateStatsMsg : NetworkHeader
    {
        public string ID;
        public Vector3 Position;
        public Vector3 BallPosition;
        public UpdateStatsMsg()
        {      // Constructor
            cmd = Commands.UPDATE_STATS;
        }
    }

    //[System.Serializable]
    //public class UpdateBallMsg : NetworkHeader
    //{
    //    public string ID;
    //    public Vector3 Position;
    //    public UpdateStatsMsg()
    //    {      // Constructor
    //        cmd = Commands.UPDATE_STATS;
    //    }
    //}

    [System.Serializable]
    public class PlayerUpdateMsg:NetworkHeader{
        public NetworkObjects.NetworkPlayer player;
        public PlayerUpdateMsg(){      // Constructor
            cmd = Commands.PLAYER_UPDATE;
            player = new NetworkObjects.NetworkPlayer();
        }
    };

    [System.Serializable]
    public class PlayerDCMsg : NetworkHeader
    {
        public string PlayerID;
        public PlayerDCMsg()
        {      // Constructor
            cmd = Commands.PLAYER_DC;
        }
    };

    [System.Serializable]
    public class PlayerSpawnMsg:NetworkHeader{
        public string ID;
        public PlayerSpawnMsg(){      // Constructor
            cmd = Commands.PLAYER_SPAWN;
        }
    };

    public class PlayerInputMsg:NetworkHeader{
        public Input myInput;
        public PlayerInputMsg(){
            cmd = Commands.PLAYER_INPUT;
            myInput = new Input();
        }
    }
    [System.Serializable]
    public class  ServerUpdateMsg:NetworkHeader{
        public List<NetworkObjects.NetworkPlayer> players;
        public ServerUpdateMsg(){      // Constructor
            cmd = Commands.SERVER_UPDATE;
            players = new List<NetworkObjects.NetworkPlayer>();
        }
    }
} 

namespace NetworkObjects
{
    [System.Serializable]
    public class NetworkObject{
        public string id;
    }
    [System.Serializable]
    public class NetworkPlayer : NetworkObject{
        public Color cubeColor;
        public Vector3 cubPos;

        public NetworkPlayer(){
            cubeColor = new Color();
        }
    }
}
