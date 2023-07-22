using System;
using Photon.Pun;
using UnityEngine;

public class PlayerEntity : MonoBehaviourPun
{
    public int teamId; // 1 : runner, 2 : tagger
    public float startHealth;
    public float health { get; protected set; }
    public bool dead { get; protected set; }
    public event Action onDeath;

    private Rigidbody playerRigidbody;

    [PunRPC]
    public virtual void ApplyUpdatedHealth(float newHealth, bool newDead)
    {
        health = newHealth;
        dead = newDead;
        Debug.Log("업데이트 슈퍼");
    }

    protected virtual void OnEnable()
    {
        dead = false;
        health = startHealth;
        playerRigidbody = GetComponent<Rigidbody>();
    }

    [PunRPC]
    public virtual void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            health -= damage;
            photonView.RPC("ApplyUpdatedHealth", RpcTarget.Others, health, dead);
            photonView.RPC("OnDamage", RpcTarget.Others, damage, hitPoint, hitNormal);
        }

        if(health <= 0 && !dead)
        {
            Die(); 
        }
    }

    [PunRPC]
    public virtual void RestoreHealth(float newHealth)
    {
        if (dead)
        {
            return;
        }

        if (PhotonNetwork.IsMasterClient)
        {
            health += newHealth;
            if (health > 2) health = 2;
            photonView.RPC("ApplyUpdatedHealth", RpcTarget.All, health, dead);
            Debug.Log("회복2");
        }
    }

    public virtual void Die()
    {
        if (onDeath != null)
        {
            onDeath();
        }

        dead = true;
    }

    [PunRPC]
    public virtual void UseItem()
    {
        if (photonView.IsMine)
        {
            if (teamId == 1)
            {
                UIManager.instance.RandomSkillRunner();
            } else
            {
                UIManager.instance.RandomSkillTagger();
            }
        }
    }

    [PunRPC]
    public void KnockBack(Vector3 force)
    {
        if (photonView.IsMine)
        {
            playerRigidbody.AddForce(force, ForceMode.Impulse);
            playerRigidbody.AddForce(Vector3.up * 90f, ForceMode.Impulse);
        }
    }

    public bool IsLocal()
    {
        return photonView.IsMine;
    }

    [PunRPC]
    public void ChangePosition(Vector3 newPosition)
    {
        if (photonView.IsMine)
        {
            this.transform.position = newPosition;
        }
    }

    


}
