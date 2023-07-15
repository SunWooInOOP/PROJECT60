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
    public Image loadingImage;

    public Image[] skillButtonImages;
 
    public Sprite[] skillsSpriteRunner;
    public Sprite[] skillsSpriteTagger;

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

    public Image runnerWinImage;
    public Image taggerWinImage;

    private void Awake()
    {
        loadingImage.gameObject.SetActive(true);
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
        LoadingImageOff();
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

    public void LoadingImageOff()
    {
        loadingImage.gameObject.SetActive(false);
    }

    public void RandomSkillRunner()
    {
        if (skillButtonImages[0].sprite != null && skillButtonImages[1].sprite != null && skillButtonImages[2].sprite != null) return;
        for (int i=2; i>0; i--)
        {
            if (skillButtonImages[i - 1].sprite == null)
            {
                skillButtonImages[i - 1].sprite = skillButtonImages[i].sprite;
                skillButtonImages[i].sprite = null;
            }
        }

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
        for (int i = 2; i > 0; i--)
        {
            if (skillButtonImages[i - 1].sprite == null)
            {
                skillButtonImages[i - 1].sprite = skillButtonImages[i].sprite;
                skillButtonImages[i].sprite = null;
            }
        }

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


}
