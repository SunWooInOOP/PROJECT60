using UnityEngine.UI;
using UnityEngine;
using Photon.Pun;
using System.Collections;

public class PlayerAttack : MonoBehaviourPun
{
    public Pan pan;
    public Transform panPivot;
    public Transform rightHandMount;

    public ParticleSystem missParticle;
    private Animator playerAnimator;

    private void Start()
    {
        playerAnimator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        if (photonView.IsMine)
        {
            Debug.Log("공격버튼 할당");
            UIManager.instance.AttackOnClick(() =>
            {
                StartCoroutine(Attack());
            }
                );
            UIManager.instance.attackButton.interactable = true;
        }
        pan.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        if (photonView.IsMine)
        {
            Debug.Log("공격버튼 할당 제거");
            UIManager.instance.attackButton.onClick.RemoveAllListeners();
        }
        pan.gameObject.SetActive(false);
    }

   

    private void OnAnimatorIK(int layerIndex)
    {
        panPivot.position = playerAnimator.GetIKPosition(AvatarIKGoal.RightHand);

        playerAnimator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f);
        playerAnimator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f);

        playerAnimator.SetIKPosition(AvatarIKGoal.RightHand, rightHandMount.position);
        playerAnimator.SetIKRotation(AvatarIKGoal.RightHand, rightHandMount.rotation);
    }

    public void AttackOnClick()
    {
        if (!photonView.IsMine)
        {
            return;
        }
            StartCoroutine(Attack());
        
    }

    private IEnumerator Attack()
    {
        if (photonView.IsMine)
        {
            UIManager.instance.attackButton.interactable = false;
            playerAnimator.SetTrigger("Attack");
            yield return new WaitForSeconds(0.5f);
            pan.Attack();
            yield return new WaitForSeconds(3f);
            UIManager.instance.attackButton.interactable = true;
        }
    }

   
}

