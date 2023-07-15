using UnityEngine.UI;
using UnityEngine;
using Photon.Pun;
using System.Collections;

public class SkillController : MonoBehaviourPun
{
    public ParticleSystem[] runnerSkills;
    public ParticleSystem[] taggerSkills;
    public ParticleSystem fireworks;

    public AudioSource playerAudioSource;

    public AudioClip[] runnerSkillsSound;
    public AudioClip[] taggerSkillsSound;
    public AudioClip outInvisible;

    private Animator playerAnimator;
    private UIManager uiManager;
    private PlayerEntity playerEntity;
    private RunnerPlayer runnerPlayer;

    public Collider windSkillCollider;
    public Collider tpSkillCollider;

    public GameObject playerCanvas;
    public GameObject player;
    public GameObject pan;
    public GameObject IceSkillEffect;
    public ParticleSystem outInvisibleParticle;

    public FireWork fireworkSkill;

    private void Awake()
    {
        uiManager = UIManager.instance;

        playerAudioSource = GetComponent<AudioSource>();
        playerAnimator = GetComponent<Animator>();
        playerEntity = GetComponent<PlayerEntity>();
        runnerPlayer = GetComponent<RunnerPlayer>();
    }


    private void OnEnable()
    {
        if (photonView.IsMine)
        {
            for (int i = 0; i < uiManager.skillButtons.Length; i++)
            {
                int index = i;
                uiManager.skillButtons[i].onClick.AddListener(() => OnSkillButtonClick(index));
            }
        }
    }

    public void OnSkillButtonClick(int index)
    {
        if (!photonView.IsMine) return;
        Sprite clickedSprite = uiManager.skillButtonImages[index].sprite;
        if(playerEntity.teamId == 1)
        {
            for (int i = 0; i < uiManager.skillsSpriteRunner.Length; i++)
            {
                if (clickedSprite == uiManager.skillsSpriteRunner[i])
                {
                    photonView.RPC("UseRunnerSkill", RpcTarget.All, i);
                    uiManager.skillButtonImages[index].sprite = null;
                    uiManager.skillButtonImages[index].gameObject.SetActive(false);
                }
            }
        }
        if (playerEntity.teamId == 2)
        {
            for (int i = 0; i < uiManager.skillsSpriteTagger.Length; i++)
            {
                if (clickedSprite == uiManager.skillsSpriteTagger[i])
                {
                    photonView.RPC("UseTaggerSkill", RpcTarget.All, i);
                    uiManager.skillButtonImages[index].sprite = null;
                    uiManager.skillButtonImages[index].gameObject.SetActive(false);
                }
            }
        }
    }

    [PunRPC]
    public void UseRunnerSkill(int index)
    {
        runnerSkills[index].Play();
        playerAudioSource.PlayOneShot(runnerSkillsSound[index]);
        if(index == 0)
        {
            photonView.RPC("WindSkill", RpcTarget.MasterClient);
        } else if (index == 1)
        {
            photonView.RPC("Invisible", RpcTarget.All);
        } else if (index == 2)
        {
            runnerPlayer.photonView.RPC("RestoreHealth", RpcTarget.All, 1);
        }
    }

    [PunRPC]
    public void UseTaggerSkill(int index)
    {
        taggerSkills[index].Play();
        playerAudioSource.PlayOneShot(taggerSkillsSound[index]);
        if (index == 0)
        {
            photonView.RPC("TPSkill", RpcTarget.MasterClient);
        } else if (index == 1)
        {
            fireworkSkill.UseSkill();
        } else if (index == 2)
        {
            photonView.RPC("IceSkill", RpcTarget.All);
            taggerSkills[index].Play();
        }
    }

    [PunRPC]
    public void IceSkill()
    {
        IceSkillEffect.SetActive(true);
    }

    [PunRPC]
    public void WindSkill()
    {
        StartCoroutine(WindSkillCRT());
    }

    public IEnumerator WindSkillCRT()
    {
        windSkillCollider.enabled = true;
        yield return new WaitForSeconds(1.3f);
        windSkillCollider.enabled = false;
    }

    [PunRPC]
    public void TPSkill()
    {
        StartCoroutine(TPSkillCRT());
    }

    public IEnumerator TPSkillCRT()
    {
        tpSkillCollider.enabled = true;
        yield return new WaitForSeconds(2f);
        tpSkillCollider.enabled = false;
    }

    [PunRPC]
    public void Invisible()
    {
        StartCoroutine(InvisibleCRT());
    }

    public IEnumerator InvisibleCRT()
    {
        player.SetActive(false);
        pan.SetActive(false);
        playerCanvas.SetActive(false);
        yield return new WaitForSeconds(10f);
        player.SetActive(true);
        pan.SetActive(true);
        playerCanvas.SetActive(true);
        outInvisibleParticle.Play();
        playerAudioSource.PlayOneShot(outInvisible);
    }
}
