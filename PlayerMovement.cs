using Photon.Pun;
using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviourPun
{
    public float moveSpeed = 8f;
    public float rotateSpeed = 150f;

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
    }


}
  


