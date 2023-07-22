using System.Linq;
using System.Collections;
using UnityEngine;
using Photon.Pun;

public class Ice : MonoBehaviourPun
{
    public ParticleSystem frostParcitle;
    public ParticleSystem iceBall;
    public GameObject parentObject;
    public GameObject playerObject;
    public float speed = 13f;
    private GameObject targetObject = null;
    private bool isTry = true;

    public AudioSource audioSource;
    public AudioClip spikeSound;
    void OnEnable()
    {
        targetObject = FindGameObject();
    }

    private void Update()
    {
        if (targetObject != null)
        {
            Vector3 direction = (targetObject.transform.position - this.transform.position).normalized;
            parentObject.transform.position += direction * speed * Time.deltaTime;
        } else
        {
            targetObject = FindGameObject();
        }
    }

    private GameObject FindGameObject()
    {
        PlayerEntity[] runnerPlayers = FindObjectsOfType<PlayerEntity>().Where(player => player.teamId == 1).ToArray();
        if (runnerPlayers.Length == 0) return null;
        PlayerEntity nearestRunner = runnerPlayers.OrderBy(player => Vector3.Distance(this.transform.position, player.transform.position)).First();
        GameObject target = nearestRunner.gameObject;
        return target;
    }

    public void OnParticleCollision(GameObject other)
    {
        if (PhotonNetwork.IsMasterClient && isTry == true)
        {
            RunnerPlayer target = other.GetComponent<RunnerPlayer>();
            if (target != null)
            {
                isTry = false;
                PlayerMovement targetMove = other.GetComponent<PlayerMovement>();
                photonView.RPC("SpikeParticle", RpcTarget.All);
                targetMove.photonView.RPC("ChangeSpeed", RpcTarget.All, 2f);
                StartCoroutine(ReturnOriginalSpeed(targetMove, 4f));
            }
        }
    }

    public IEnumerator ReturnOriginalSpeed(PlayerMovement target, float delay)
    {
        yield return new WaitForSeconds(delay);
        target.photonView.RPC("ChangeSpeed", RpcTarget.All, 6.5f);
        parentObject.transform.position = playerObject.transform.position;
        gameObject.SetActive(false);
        isTry = true;
    }



    [PunRPC]
    public void SpikeParticle()
    {
        iceBall.Stop();
        frostParcitle.Play();
        audioSource.PlayOneShot(spikeSound);
    }
}
