using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Collections;
using Photon.Pun;

public class UIManager : MonoBehaviourPun
{
    public static UIManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<UIManager>();
            }

            return m_instance;
        }
    }

    private static UIManager m_instance;

    public AudioSource uiAudioSource;
    public AudioClip gameStartSound;
    public AudioClip gameEndSound;

    public GameObject joyStickGameObject;
    public Button attackButton;
    public Button[] skillButtons;
    public Text teamIdText;
    public Image startImage;
    public Text numberText;

    public Image itemSpawnImage01;
    public Image itemSpawnImage02;
    public Animator itemSpawnAnimator01;
    public Animator itemSpawnAnimator02;

    public Image[] skillButtonImages;
 
    public Sprite[] skillsSpriteRunner;
    public Sprite[] skillsSpriteTagger;

    public Image heart01;
    public Image heart02;

    public Sprite startText9;
    public Sprite startText8;
    public Sprite startText7;
    public Sprite startText6;
    public Sprite startText5;
    public Sprite startText4;
    public Sprite startText3;
    public Sprite startText2;
    public Sprite startText1;

    public Text timeText;

    public Image loadingImage;

    public Image runnerWinImage;
    public Image taggerWinImage;

    public Image[] iceImages;

    private void Awake()
    {
        StartImageSet();
    }

    private void StartImageSet()
    {
        foreach(Image image in skillButtonImages)
        {
            image.gameObject.SetActive(false);
        }
    }

    public void UpdateTeamIdText(int teamid)
    {
        if(teamid == 2){ teamIdText.text = "내 역할 : 추적자"; }
        if(teamid == 1) { teamIdText.text = "내 역할 : 도망자"; }
    }

    [PunRPC]
    public void GameStartText()
    {
        StartCoroutine(GameStartTextCoroutine());
    }

    private IEnumerator GameStartTextCoroutine()
    {
        loadingImage.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);
        startImage.sprite = startText9;
        yield return new WaitForSeconds(1f);
        startImage.sprite = startText8;
        yield return new WaitForSeconds(1f);
        startImage.sprite = startText7;
        yield return new WaitForSeconds(1f);
        startImage.sprite = startText6;
        yield return new WaitForSeconds(1f);
        startImage.sprite = startText5;
        yield return new WaitForSeconds(1f);
        startImage.sprite = startText4;
        yield return new WaitForSeconds(1f);
        uiAudioSource.PlayOneShot(gameStartSound);
        startImage.sprite = startText3;
        yield return new WaitForSeconds(1f);
        startImage.sprite = startText2;
        yield return new WaitForSeconds(1f);
        startImage.sprite = startText1;
        yield return new WaitForSeconds(1f);
        startImage.gameObject.SetActive(false);

    }

    public void UpdateTime(float time)
    {
        time += 1;

        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);

        timeText.text = string.Format("남은 시간 : {0:00}:{1:00}", minutes, seconds);
    }

    public void TaggerWinImage()
    {
        taggerWinImage.gameObject.SetActive(true);
        uiAudioSource.PlayOneShot(gameEndSound);
    }

    public void RunnerWinImage()
    {
        runnerWinImage.gameObject.SetActive(true);
        uiAudioSource.PlayOneShot(gameEndSound);
    }

    public void GameOutOnclik()
    {
        GameManager.instance.GameOut();
    }

    public void AttackOnClick(Action onClickAction)
    {
        attackButton.onClick.AddListener(() => onClickAction());
    }

    

    public void RandomSkillRunner()
    {
        if (skillButtonImages[0].sprite != null && skillButtonImages[1].sprite != null && skillButtonImages[2].sprite != null) return;

        Sprite skillSprite = skillsSpriteRunner[UnityEngine.Random.Range(0, skillsSpriteRunner.Length)];
        for (int i=0; i < 3; i++)
        {
            if (skillButtonImages[i].sprite == null)
            {
                skillButtonImages[i].sprite = skillSprite;
                skillButtonImages[i].gameObject.SetActive(true);
                break;
            }
        }
    }

    public void RandomSkillTagger()
    {
        if (skillButtonImages[0].sprite != null && skillButtonImages[1].sprite != null && skillButtonImages[2].sprite != null) return;
        Sprite skillSprite = skillsSpriteTagger[UnityEngine.Random.Range(0, skillsSpriteTagger.Length)];
        for (int i = 0; i < 3; i++)
        {
            if (skillButtonImages[i].sprite == null)
            {
                skillButtonImages[i].sprite = skillSprite;
                skillButtonImages[i].gameObject.SetActive(true);
                break;
            }
        }
    }

    public bool IsGetItem()
    {
        if (skillButtonImages[0].sprite == null || skillButtonImages[1].sprite == null || skillButtonImages[2].sprite == null) return true;
        else return false;
    }

    public void UpdateNumberText(int number)
    {
        numberText.text = string.Format("{0}명", number);
    }

    [PunRPC]
    public void ItemSpawnText()
    {
        StartCoroutine(ItemSpawnCoroutine());     
    }

    public IEnumerator ItemSpawnCoroutine()
    {
        itemSpawnImage01.gameObject.SetActive(true);
        itemSpawnAnimator01.SetTrigger("Down");
        yield return new WaitForSeconds(2f);
        itemSpawnAnimator01.SetTrigger("Up");
        yield return new WaitForSeconds(1f);
        itemSpawnImage01.gameObject.SetActive(false);
        yield return new WaitForSeconds(7f);
        itemSpawnImage02.gameObject.SetActive(true);
        itemSpawnAnimator02.SetTrigger("Down");
        yield return new WaitForSeconds(2f);
        itemSpawnAnimator02.SetTrigger("Up");
        yield return new WaitForSeconds(1f);
        itemSpawnImage02.gameObject.SetActive(false);
    }

    public void HeartImage(float index)
    {
        if (index == 2)
        {
            heart01.gameObject.SetActive(true);
            heart02.gameObject.SetActive(true);
        } else if (index == 1)
        {
            heart01.gameObject.SetActive(true);
            heart02.gameObject.SetActive(false);
        } else
        {
            heart01.gameObject.SetActive(false);
            heart02.gameObject.SetActive(false);
        }
    }

    public void StartIceImage()
    {
        StartCoroutine(IceImage());
    }

    public IEnumerator IceImage()
    {
        for(int i = 0; i < iceImages.Length; i++)
        {
            iceImages[i].gameObject.SetActive(true);
        }
        yield return new WaitForSeconds(4f);
        for (int i = 0; i < iceImages.Length; i++)
        {
            iceImages[i].gameObject.SetActive(false);
        }
    }

}
