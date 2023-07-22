using Photon.Pun;
using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviourPun
{
    public float moveSpeed = 8f;
    public float rotateSpeed = 150f;

    public AudioSource playerAudio;

    private Animator playerAnimator;
    private PlayerInput playerInput;
    private Rigidbody playerRigidbody;

    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        Move();
        Rotate();
        RunSoundOnOff();
        playerAnimator.SetFloat("VerticalMove", playerInput.moveVertical);
        

    }

   

    private void Move()
    {
        Vector3 moveVector = transform.forward * playerInput.moveVertical * moveSpeed * Time.deltaTime;
        playerRigidbody.MovePosition(playerRigidbody.position + moveVector);

    }

    private void Rotate()
    {
        float turn = playerInput.rotateHorizontal * rotateSpeed * Time.deltaTime;
        playerRigidbody.rotation = playerRigidbody.rotation * Quaternion.Euler(0, turn, 0);
    }

    [PunRPC]
    public void ChangeSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
        if(newSpeed == 2f && photonView.IsMine)
        {
            IceUI();
        }
    }

    public void RunSoundOnOff()
    {
        if (playerInput.moveVertical >= 0.3 || playerInput.moveVertical <= -0.3)
        {
            photonView.RPC("RunSoundPlay", RpcTarget.Others);
        }
        else
        {
            photonView.RPC("RunSoundStop", RpcTarget.Others);
        }
    }

    [PunRPC]
    public void RunSoundPlay()
    {
        if (!playerAudio.isPlaying)
        {
            playerAudio.Play();
        }
    }

    [PunRPC]
    public void RunSoundStop()
    {
        if (playerAudio.isPlaying)
        {
            playerAudio.Stop();
        }
    }

    public void IceUI()
    {
        UIManager.instance.StartIceImage();
    }

}
  


