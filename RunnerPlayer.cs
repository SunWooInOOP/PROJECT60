using Photon.Pun;
using UnityEngine.UI;
using UnityEngine;

public class RunnerPlayer : PlayerEntity
{
    public Image healthImage01;
    public Image healthImage02;
    public Sprite nullHealthSprite;
    public Sprite fillHealthSprite;
    public ParticleSystem hitParticle;
    public ParticleSystem deathParticle;

    public AudioClip deathClip;
    public AudioClip hitClip;
    public AudioClip itemPickupClip;

    private AudioSource playerAudioPlayer;
    private Animator playerAnimator;

    private PlayerMovement runnerPlayerMovement;
    private PlayerAttack playerAttack;

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        playerAudioPlayer = GetComponent<AudioSource>();

        runnerPlayerMovement = GetComponent<PlayerMovement>();
        playerAttack = GetComponent<PlayerAttack>();
    }

    protected override void OnEnable()
    {
        onDeath += RunnerDieRun;
        teamId = 1;
        runnerPlayerMovement.photonView.RPC("ChangeSpeed", RpcTarget.All, 7f);
        startHealth = 2;
        base.OnEnable();

        healthImage01.gameObject.SetActive(true);
        healthImage02.gameObject.SetActive(true);

        runnerPlayerMovement.enabled = true;
    }

    [PunRPC]
    public override void RestoreHealth(float newHealth)
    {
        base.RestoreHealth(newHealth);
        ChangeHealthUI(health);
  
    }

    [PunRPC]
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitDirection)
    {
        if (!dead)
        {
            playerAudioPlayer.PlayOneShot(hitClip);
            photonView.RPC("HitParticlePlay", RpcTarget.All, hitPoint);

        }
        base.OnDamage(damage, hitPoint, hitDirection);
        ChangeHealthUI(health);

    }

    [PunRPC]
    public override void UseItem()
    {
        base.UseItem();
        playerAudioPlayer.PlayOneShot(itemPickupClip);
    }

    public override void Die()
    {
        base.Die();
        healthImage01.gameObject.SetActive(false);
        healthImage02.gameObject.SetActive(false);
        photonView.RPC("DeathParticlePlay", RpcTarget.All);
        playerAudioPlayer.PlayOneShot(deathClip);     
        playerAnimator.SetTrigger("Die");        
        runnerPlayerMovement.enabled = false;
    }

    public void ChangeHealthUI(float changeHealthValue)
    {
        if (changeHealthValue == 2)
        {
            healthImage01.sprite = fillHealthSprite;
            healthImage02.sprite = fillHealthSprite;
        }else if(changeHealthValue == 1)
        {
            healthImage01.sprite = fillHealthSprite;
            healthImage02.sprite = nullHealthSprite;
        }
        else
        {
            healthImage01.sprite = nullHealthSprite;
            healthImage02.sprite = nullHealthSprite;
        }

    }

    [PunRPC]
    public void HitParticlePlay(Vector3 hitPosition)
    {
        hitParticle.transform.position = hitPosition;
        hitParticle.Play();
    }

    [PunRPC]
    public void DeathParticlePlay()
    {
        deathParticle.Play();
    }
    private void RunnerDieRun()
    {
        Invoke("RunnerDie", 2f);
    }
    private void RunnerDie()
    {
        if (photonView.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
            PhotonNetwork.Instantiate("TagPlayer", this.transform.position, this.transform.rotation);
        }
    }

    
}
