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
        UIManager.instance.AttackOnClick(() =>
        {
            StartCoroutine(Attack());
        }
            );
        pan.gameObject.SetActive(true);
        UIManager.instance.attackButton.interactable = true;
    }

    private void OnDisable()
    {
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
            pan.Attack();
            if (GameManager.instance.successAttack == false)
            {
                missParticle.Play();
            }
            GameManager.instance.successAttack = false;
            yield return new WaitForSeconds(3f);
            UIManager.instance.attackButton.interactable = true;
        }
    }

   
}

