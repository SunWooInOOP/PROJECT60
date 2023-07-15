using System.Collections;
using Photon.Pun;
using UnityEngine;

public class Pan : MonoBehaviourPun
{

    public ParticleSystem attackEffect;
    public Collider attackEffectCollider;

    private AudioSource panAudioSource;

    public float attackDamage = 1;

    private void Awake()
    {
        attackEffectCollider = attackEffect.GetComponent<Collider>();
        panAudioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        attackEffectCollider.enabled = false;
    }
    public void Attack()
    {
        photonView.RPC("OnAttackClients", RpcTarget.MasterClient);
    }

    [PunRPC]
    public void OnAttackClients()
    {
        StartCoroutine(OnAttack());
        photonView.RPC("AttackEffect", RpcTarget.All);
    }

    private IEnumerator OnAttack()
    {
        attackEffectCollider.enabled = true;

        yield return new WaitForSeconds(0.5f);
        attackEffectCollider.enabled = false;
    }

    [PunRPC]
    private void AttackEffect()
    {
        attackEffect.Play();
    }

}
