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

    public ParticleSystem spawnParticle;

    private float spawnTime;
    private float lastUseTime;
    private bool isSpawn;
    private bool isFirstSpawn = false;

    private void Awake()
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

        if ((Time.time >= lastUseTime + spawnTime && isSpawn == false) || (GameManager.instance.GetGameTime() <= 220 && isFirstSpawn == false && GameManager.instance.GetIsStart() == true))
        {
            isFirstSpawn = true;
            Debug.Log("½ºÆù");
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
            Debug.Log("1");
            PlayerEntity player = other.GetComponent<PlayerEntity>();
            if(player != null && UIManager.instance.IsGetItem())
            {
                Debug.Log("2");
                photonView.RPC("UsingPlay", RpcTarget.All);
                photonView.RPC("SetBox", RpcTarget.All, false);
                player.photonView.RPC("UseItem", RpcTarget.All);
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
    }

    [PunRPC]
    private void SetBox(bool set)
    {
        randomBox.SetActive(set);
    }
}
