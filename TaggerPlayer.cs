using Photon.Pun;
using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class TaggerPlayer : PlayerEntity
{
    public Image taggerImage;

    public AudioClip hitClip;
    public AudioClip itemPickupClip;

    private AudioSource playerAudioPlayer;
    private Animator playerAnimator;

    private PlayerMovement runnerPlayerMovement;
    private PlayerAttack playerAttack;

    public ParticleSystem hitParticle;

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        playerAudioPlayer = GetComponent<AudioSource>();

        runnerPlayerMovement = GetComponent<PlayerMovement>();
        playerAttack = GetComponent<PlayerAttack>();
    }

    protected override void OnEnable()
    {
        teamId = 2;
        startHealth = 10000;
        StartCoroutine(StartSpeed());
        base.OnEnable();

        taggerImage.gameObject.SetActive(true);

        runnerPlayerMovement.enabled = true;
        if (photonView.IsMine)
        {
            UIManager.instance.HeartImage(0);
        }
    }

    [PunRPC]
    public override void RestoreHealth(float newHealth)
    {
        base.RestoreHealth(newHealth);

    }

    [PunRPC]
    public override void UseItem()
    {
        base.UseItem();
        playerAudioPlayer.PlayOneShot(itemPickupClip);
    }

    [PunRPC]
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitDirection)
    {
        if (!dead)
        {
            playerAudioPlayer.PlayOneShot(hitClip);
            photonView.RPC("HitParticlePlay", RpcTarget.All, hitPoint);
            StartCoroutine(HittedAnimation());

        }
        base.OnDamage(damage, hitPoint, hitDirection);

    }

    [PunRPC]
    public void HitParticlePlay(Vector3 hitPosition)
    {
        hitParticle.transform.position = hitPosition;
        hitParticle.Play();
    }

    private IEnumerator HittedAnimation()
    {
        if (photonView.IsMine)
        {
            playerAnimator.SetTrigger("Hitted");
        }
        runnerPlayerMovement.photonView.RPC("ChangeSpeed", RpcTarget.All, 1f);
        yield return new WaitForSeconds(2f);
        runnerPlayerMovement.photonView.RPC("ChangeSpeed", RpcTarget.All, 7.5f);
        if (photonView.IsMine)
        {
            playerAnimator.SetTrigger("NonHitted");
        }
  
    }

    private IEnumerator StartSpeed()
    {
        runnerPlayerMovement.photonView.RPC("ChangeSpeed", RpcTarget.All, 0f);
        yield return new WaitForSeconds(10f);
        runnerPlayerMovement.photonView.RPC("ChangeSpeed", RpcTarget.All, 7.5f);
    }
}
