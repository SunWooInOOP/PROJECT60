using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInput : MonoBehaviourPun
{
    public Joystick joystick;

    public float rotateHorizontal { get; private set; }
    public float moveVertical { get; private set; }

    private void OnEnable()
    {
        joystick = UIManager.instance.joyStickGameObject.GetComponent<Joystick>();
    }
    private void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        if (GameManager.instance != null && GameManager.instance.isGameover)
        {
            rotateHorizontal = 0;
            moveVertical = 0;
        }


        rotateHorizontal = joystick.Horizontal;
        moveVertical = joystick.Vertical;
        
        
    }




}
