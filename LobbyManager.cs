using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    private string gameVersion = "Prototype";

    public Text InfoText;
    public Text gameversionText;
    public Button startButton;

    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();
        startButton.interactable = false;
        InfoText.text = "서버 접속 시도...";
        gameversionText.text = $"Version: {gameVersion}";
    }

    public override void OnConnectedToMaster()
    {
        startButton.interactable = true;
        InfoText.text = "온라인:서버연결";
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        startButton.interactable = false;
        InfoText.text = "오프라인:서버접속 재시도..";
        PhotonNetwork.ConnectUsingSettings();
    }

    public void Connect()
    {
        startButton.interactable = false;
        if (PhotonNetwork.IsConnected)
        {
            InfoText.text = "게임 룸 매칭..";
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            InfoText.text = "오프라인 : 서버 접속 재시도";
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnJoinRandomFailed(short returncode, string message)
    {
        InfoText.text = "룸 매칭 실패 : 새로운 룸 생성...";
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });
    }

    public override void OnJoinedRoom()
    {
        TextUpdate();
        CheckPlayersCount();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        TextUpdate();
        CheckPlayersCount();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        TextUpdate();
        CheckPlayersCount();
    }

    private void CheckPlayersCount()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount >= 3)
        {
            PhotonNetwork.LoadLevel("Main");
        }
    }

    public void TextUpdate()
    {
        int roomPlayerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        InfoText.text = $"룸 접속 : 룸 접속인원 ({roomPlayerCount})";
    }
}

