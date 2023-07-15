using Photon.Pun;
using UnityEngine;
using System.Linq;
using System.Collections;

public class TpSkill : MonoBehaviourPun
{
    public ParticleSystem readyParticle;
    public ParticleSystem tpGoParticle;
    public bool isRe = true;

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("1");
        if (PhotonNetwork.IsMasterClient && isRe == true)
        {
            Debug.Log("2");
            PlayerEntity playerEntity = this.GetComponentInParent<PlayerEntity>();
            if (playerEntity != null && playerEntity.teamId == 2)
            {
                Debug.Log("3");
                PlayerEntity[] runnerPlayers = FindObjectsOfType<PlayerEntity>().Where(player => player.teamId == 1).ToArray();
                if (runnerPlayers.Length == 0) return;
                PlayerEntity nearestRunner = runnerPlayers.OrderBy(player => Vector3.Distance(this.transform.position, player.transform.position)).First();
                Vector3 randomPosition = nearestRunner.transform.position + Random.insideUnitSphere * 10f;
                randomPosition.y = nearestRunner.transform.position.y;
                StartCoroutine(TP(playerEntity, randomPosition));
                isRe = false;            
            }
        }
    }

    public IEnumerator TP(PlayerEntity player, Vector3 newPosition)
    {
        yield return new WaitForSeconds(2f);
        player.photonView.RPC("ChangePosition",RpcTarget.All, newPosition);
        yield return new WaitForSeconds(0.2f); 
        photonView.RPC("StopParticle", RpcTarget.All);
        isRe = true;
    }


    [PunRPC]
    public void StopParticle()
    {
        readyParticle.Stop();
        tpGoParticle.Play();
    }
}