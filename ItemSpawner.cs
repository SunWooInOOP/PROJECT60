using System.Collections;
using Photon.Pun;
using UnityEngine;

public class ItemSpawner : MonoBehaviourPun
{
    public GameObject randomBox;

    public float spawnTimeMax = 40;
    public float spawnTimeMin = 30;

    public AudioSource spawnAudioSource;
    public AudioClip spawnSound;

    private bool firstSpawnEffect = true;

    public ParticleSystem spawnParticle;

    private float spawnTime;
    private float lastUseTime;
    private bool isSpawn;

    private void Start()
    {
        spawnTime = 0;
        lastUseTime = 1000;
    }

    private void Update()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        if (Time.time >= lastUseTime + spawnTime || firstSpawnEffect == true)
        {
            photonView.RPC("SpawnParticlePlay", RpcTarget.All);
            Spawn();          
        }
   
    }

    private void Spawn()
    {
        photonView.RPC("SetBox", RpcTarget.All, true);
        spawnAudioSource.PlayOneShot(spawnSound);
        isSpawn = true;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (PhotonNetwork.IsMasterClient && isSpawn == true)
        {
            PlayerEntity player = other.GetComponent<PlayerEntity>();
            if(player != null && UIManager.instance.IsGetItem() == true)
            {
                photonView.RPC("UsingPlay", RpcTarget.All);
                photonView.RPC("SetBox", RpcTarget.All, false);
                player.photonView.RPC("UseItem", RpcTarget.All);
                firstSpawnEffect = false;
                lastUseTime = Time.time;
                spawnTime = Random.Range(spawnTimeMin, spawnTimeMax);
                isSpawn = false;
            }
        }
    }

    [PunRPC]
    private void UsingPlay()
    {
        randomBox.GetComponent<Animator>().SetTrigger("Open");
    }

    [PunRPC]
    private void SpawnParticlePlay()
    {
        spawnParticle.Play();
        firstSpawnEffect = false;
    }

    [PunRPC]
    private void SetBox(bool set)
    {
        randomBox.SetActive(set);
    }
}
