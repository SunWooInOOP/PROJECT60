using UnityEngine;

public class LobbyUISound : MonoBehaviour
{
    public AudioSource uiAudioSource;
    public AudioClip clickSound;

    public void UIClick()
    {
        uiAudioSource.PlayOneShot(clickSound);
    }
}
