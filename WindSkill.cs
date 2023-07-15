using Photon.Pun;
using UnityEngine;

public class WindSkill : MonoBehaviourPun
{
    public float force = 300f;

    private void OnTriggerEnter(Collider other)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PlayerEntity player = other.GetComponent<PlayerEntity>();
            if (player != null)
            {

                if (player != null && player.teamId == 2)
                {
                    Vector3 direction = player.transform.position - transform.position;
                    direction.Normalize();

                    Debug.Log("windSkill");
                    float pushForce = force;
                    player.photonView.RPC("KnockBack", RpcTarget.All, direction * pushForce);
                }
            }
        }
    }
}

