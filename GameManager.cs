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

    public bool successAttack = false;

    private float gameTime;
    private bool isStart = false;

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
            UIManager.instance.photonView.RPC("LoadingImageOff", RpcTarget.All);
            UIManager.instance.photonView.RPC("GameStartText", RpcTarget.All);
            isGameover = false;
        }
        Invoke("TimeSet", 10f);
        successAttack = false;
    }

    private void TimeSet()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            gameTime = 300f;
            isStart = true;
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
        TimeUpdate();
        IsTaggerWin();
    
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

    public void SuccessAttack()
    {
        successAttack = true;
    }

}
