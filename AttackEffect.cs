using Photon.Pun;
using UnityEngine;

public class AttackEffect : MonoBehaviourPun
{
    public float attackDamage = 1;
    public float attackDelay = 1.5f;
    private float lastAttackTime;

    public void OnTriggerEnter(Collider other)
    {
        if (Time.time - lastAttackTime < attackDelay) return;
        if (PhotonNetwork.IsMasterClient)
        {
            PlayerEntity player = other.GetComponent<PlayerEntity>();
            if (player != null)
            {
                if (this.GetComponentInParent<PlayerEntity>().teamId != player.GetComponent<PlayerEntity>().teamId)
                {
                    Vector3 hitPosition = other.ClosestPoint(transform.position);
                    player.OnDamage(attackDamage, hitPosition, Vector3.zero);
                    Debug.Log("공격처리");
                    lastAttackTime = Time.time;
                }
            }
        }
        PlayerEntity atplayer = other.GetComponent<PlayerEntity>();
        if (atplayer != null)
        {
            if (this.GetComponentInParent<PlayerEntity>().teamId != atplayer.GetComponent<PlayerEntity>().teamId)
            {
                GameManager.instance.SuccessAttack();
                lastAttackTime = Time.time;
            }
        }

    }

   


}
