using Photon.Pun;
using UnityEngine;
using TMPro;

public class PlayerName : MonoBehaviourPun
{
    public NameData nameData;
    public TextMeshProUGUI textMeshPro;

    private void OnEnable()
    {
        if (photonView.IsMine)
        {
            if (nameData.playerName != null)
            {
                textMeshPro.text = nameData.playerName;
                photonView.RPC("ChangeName", RpcTarget.Others, nameData.playerName);
            }
        }
    }

    [PunRPC]
    public void ChangeName(string playerName)
    {
        textMeshPro.text = playerName;
    }



}
