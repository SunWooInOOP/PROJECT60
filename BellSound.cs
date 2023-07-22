using Photon.Pun;
using UnityEngine;

public class BellSound : MonoBehaviourPun
{
    public AudioSource audioSource;

    [PunRPC]
    public void SoundStart()
    {
        audioSource.Play();
    }
}
