using Photon.Realtime;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviourPunCallbacks, IPunObservable
{
    public static GameManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<GameManager>();
            }

            return m_instance;
        }
    }

   
    private static GameManager m_instance;

    public GameObject runnerPrefab;
    public GameObject taggerPrefab;

    private float gameTime;
    private bool isStart = false;
    private bool isSpawnText = false;

    public Vector3 spawnPoint;

    public bool isGameover { get; private set; }

    private void Awake()
    {
        if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    
    private void Start()
    {
        GameStart();
        if (PhotonNetwork.IsMasterClient)
        {
            UIManager.instance.photonView.RPC("GameStartText", RpcTarget.All);
            isGameover = false;
        }
        Invoke("TimeSet", 10f);
    }

    private void TimeSet()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            isStart = true;
            gameTime = 240f;
        }
    }

    private void TimeUpdate()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (isStart && !isGameover)
            {
                if (gameTime > 0)
                {
                    gameTime -= Time.deltaTime;
 
                }
                else
                {
                    gameTime = 0;
                    photonView.RPC("RunnerWin", RpcTarget.All);
                    isGameover = true;
                }
            }
        }
        UIManager.instance.UpdateTime(gameTime);
    }

    public void IsTaggerWin()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            RunnerPlayer[] runners = FindObjectsOfType<RunnerPlayer>();
            if (runners.Length == 0 && isStart == true && !isGameover)
            {
                photonView.RPC("TaggerWin", RpcTarget.All);
                isGameover = true;
            }
        }
    }

    public void NumberTextUpdate()
    {
        RunnerPlayer[] runners = FindObjectsOfType<RunnerPlayer>();
        UIManager.instance.UpdateNumberText(runners.Length);
    }

    [PunRPC]
    public void TaggerWin()
    {
        UIManager.instance.TaggerWinImage();
        
    }

    [PunRPC]
    public void RunnerWin()
    {
        UIManager.instance.RunnerWinImage();
        
    }

    private void Update()
    {
        NumberTextUpdate();
        TimeUpdate();
        IsTaggerWin();
        ItemSpawnTextImageUpdate();   
    }

    private void GameStart()
    {
        SpawnPlayer(spawnPoint);

    }

    [PunRPC]
    private void RPC_SpawnPlayer(int playerActorNumber, Vector3 spawnpoint, string prefabName, int teamID)
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == playerActorNumber)
        {
            GameObject player = PhotonNetwork.Instantiate(prefabName, spawnpoint, Quaternion.identity, 0);
            UIManager.instance.UpdateTeamIdText(teamID);
        }
    }

    private void SpawnPlayer(Vector3 spawnpoint)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Player tagPlayer = PhotonNetwork.PlayerList[Random.Range(0, PhotonNetwork.PlayerList.Length)];

            foreach (var player in PhotonNetwork.PlayerList)
            {
                if (player == tagPlayer)
                {
                    photonView.RPC("RPC_SpawnPlayer", player, player.ActorNumber, spawnpoint, "TagPlayer", 2);
                }
                else
                {
                    photonView.RPC("RPC_SpawnPlayer", player, player.ActorNumber, spawnpoint, "RunnerPlayer", 1);
                }
            }
        }
    }



    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(gameTime);
            stream.SendNext(isGameover);
            stream.SendNext(isStart);
        }
        else
        {
            this.gameTime = (float)stream.ReceiveNext();
            this.isGameover = (bool)stream.ReceiveNext();
            this.isStart = (bool)stream.ReceiveNext();
        }
    }

    public void GameOut()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Lobby");
    }

    public void ItemSpawnTextImageUpdate()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        if (gameTime < 230 && isSpawnText == false && isStart == true)
        {
            Debug.Log("게임매니저 실행");
            UIManager.instance.photonView.RPC("ItemSpawnText", RpcTarget.All);
            isSpawnText = true;
        }
    }

    public bool GetIsStart()
    {
        return isStart;
    }
    
    public float GetGameTime()
    {
        return gameTime;
    }
}
