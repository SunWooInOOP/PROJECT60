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
        InfoText.text = "���� ���� �õ�...";
        gameversionText.text = $"Version: {gameVersion}";
    }

    public override void OnConnectedToMaster()
    {
        startButton.interactable = true;
        InfoText.text = "�¶���:��������";
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        startButton.interactable = false;
        InfoText.text = "��������:�������� ��õ�..";
        PhotonNetwork.ConnectUsingSettings();
    }

    public void Connect()
    {
        startButton.interactable = false;
        if (PhotonNetwork.IsConnected)
        {
            InfoText.text = "���� �� ��Ī..";
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            InfoText.text = "�������� : ���� ���� ��õ�";
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnJoinRandomFailed(short returncode, string message)
    {
        InfoText.text = "�� ��Ī ���� : ���ο� �� ����...";
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
        InfoText.text = $"�� ���� : �� �����ο� ({roomPlayerCount})";
    }
}

