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
        if (photonView.IsMine)
        {
            UIManager.instance.UpdateTeamIdText(teamId);
        }
    }

    [PunRPC]
    public override void ApplyUpdatedHealth(float updatedHealth, bool isDead)
    {
        base.ApplyUpdatedHealth(updatedHealth, isDead);
        Debug.Log("업데이트 하위");
        ChangeHealthUI(updatedHealth);
        Debug.Log(updatedHealth);
        Debug.Log(health);
    }


    [PunRPC]
    public override void RestoreHealth(float newHealth)
    {
        Debug.Log(health);
        base.RestoreHealth(newHealth);
        Debug.Log(health);
        Debug.Log("회복1");
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

        if (photonView.IsMine)
        {
            UIManager.instance.HeartImage(changeHealthValue);
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
            Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
            PhotonNetwork.Instantiate("TagPlayer", spawnPosition, this.transform.rotation);
            UIManager.instance.UpdateTeamIdText(2);
        }
    }



}
