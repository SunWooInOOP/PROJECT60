using Photon.Pun;
using UnityEngine;

public class CameraManager : MonoBehaviourPun
{
    public GameObject playerCamera;

    private void Awake()
    {
        if (photonView.IsMine)
        {
            playerCamera.SetActive(true);
        }
        else
        {
            playerCamera.SetActive(false);
        }
    }
}
