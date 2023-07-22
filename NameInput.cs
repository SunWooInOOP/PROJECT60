using UnityEngine.UI;
using System.Collections;
using UnityEngine;

public class NameInput : MonoBehaviour
{
    public Image backGroundImage;
    public InputField nameInputField;
    public NameData nameData;
    public Button nameFinishButton;
    public Text nameFinishText;

    public Animator nameFinishButtonAnimator;
    public Animator backGroundImageAnimator;

    public bool isOn = false;

    public void InputName()
    {
        if(nameInputField.text != null && nameInputField.text != "")
        {
            nameData.playerName = nameInputField.text;
            nameFinishText.text = $"\n[{nameData.playerName}] \n\n멋있는 이름이네요!";
            StartCoroutine(OnNameUI());
        }
        else
        {
            nameFinishText.text = "\n이름을 설정하지 않으면 \n\n무작위로 설정되요!";
            StartCoroutine(OnNameUI());
        }
    }

    public void OnUI()
    {
        backGroundImage.gameObject.SetActive(true);
        backGroundImageAnimator.SetTrigger("On");
    }

    public IEnumerator OffUI()
    {     
        backGroundImageAnimator.SetTrigger("Off");
        nameFinishButtonAnimator.SetTrigger("Off");
        yield return new WaitForSeconds(2f);
        backGroundImage.gameObject.SetActive(false);
    }

    public IEnumerator OnNameUI()
    {
        nameFinishButtonAnimator.SetTrigger("On");
        yield return new WaitForSeconds(3f);
        StartCoroutine(OffUI());
    }

    public void OnOff()
    {
        if(isOn == false)
        {
            OnUI();
            isOn = true;
        }
        else
        {
            StartCoroutine(OffUI());
            isOn = false;
        }
    }
}
