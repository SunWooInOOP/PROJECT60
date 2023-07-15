using UnityEngine;
using Photon.Pun;
using System.Collections;
using System.Linq;

public class FireWork : MonoBehaviourPun
{
    public void UseSkill()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PlayerEntity[] playerEntity = FindObjectsOfType<PlayerEntity>().Where(player => player.teamId == 1).ToArray();
            foreach (PlayerEntity runnerPlayer in playerEntity)
            {
                SkillController runnerParticle = runnerPlayer.GetComponent<SkillController>();
                photonView.RPC("EffectRPC", RpcTarget.All, runnerParticle.photonView.ViewID);
            }
        }
    }

    [PunRPC]
    public void EffectRPC(int viewID)
    {
        PhotonView targetView = PhotonView.Find(viewID);
        SkillController targetSkillController = targetView.GetComponent<SkillController>();
        StartCoroutine(Effect(targetSkillController));
    }

    public IEnumerator Effect(SkillController player)
    {
        player.fireworks.Play();
        yield return new WaitForSeconds(10F);
        player.fireworks.Stop();
    }
}
