using UnityEngine;
using UnityEngine.UI;

public class CustomUIManager : MonoBehaviour
{
    public Button gameStartButton;
    public Text projectText;
    public Image serverImage;
    public Button customText;
    public GameObject custumUI;
    public Button returnText;

    private Animator gameStartAnimator;
    private Animator projectAnimator;
    private Animator serverAnimator;
    private Animator customAnimator;
    private Animator custumUIAnimator;
    private Animator returnAnimator;

    private void Start()
    {
        gameStartAnimator = gameStartButton.GetComponent<Animator>();
        projectAnimator = projectText.GetComponent<Animator>();
        serverAnimator = serverImage.GetComponent<Animator>();
        customAnimator = customText.GetComponent<Animator>();
        custumUIAnimator = custumUI.GetComponent<Animator>();
        returnAnimator = returnText.GetComponent<Animator>();
    }

    public void CustomOnClick()
    {
        custumUI.SetActive(true);
        gameStartAnimator.SetTrigger("Start");
        projectAnimator.SetTrigger("Start");
        serverAnimator.SetTrigger("Start");
        customAnimator.SetTrigger("Start");
        Invoke("DelayCustumUI", 1f);
    }

    public void CustomOutOnClick()
    {
        gameStartAnimator.SetTrigger("Out");
        projectAnimator.SetTrigger("Out");
        serverAnimator.SetTrigger("Out");
        customAnimator.SetTrigger("Out");
        custumUIAnimator.SetTrigger("Out");
        returnAnimator.SetTrigger("Out");
        Invoke("SetFalse", 3f);
    }

    public void SetFalse()
    {
        custumUI.SetActive(false);
    }

    public void DelayCustumUI()
    {
        custumUIAnimator.SetTrigger("Start");
        returnAnimator.SetTrigger("Start");
    }
}
