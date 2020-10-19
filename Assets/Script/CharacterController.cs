using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask ground;
    //in order to select one or more of our ground's layers;

    private Rigidbody2D myRigidbody2D;
    //this variable refers to a RigidBody2D Component;


    private Vector2 stickDirection;
    private bool isOnGround = false;
    //this bool will verify if the player is on the ground;
    private bool isFacingLeft = true;
    //this bool will check which direction the Player's facing;



    private void OnEnable()
    {
        //we're setting up each control from our InputSystem;
        var playerController = new PlayerController();
        playerController.Enable();
        playerController.Main.Move.performed += MoveOnPerformed;
        playerController.Main.Move.canceled += MoveOnCanceled;
        playerController.Main.Jump.performed += JumpOnPerformed;

    }

    private void JumpOnPerformed(InputAction.CallbackContext obj)
    {
        //this function will be executed when the "Jump" button is pressed (here SPACEBAR);
        if (isOnGround)
        //we're checking if the Player's touching the ground in order to prevent it for infinite jumping;
        {
            myRigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            //this is the jump, we can change the force of the jump in the inspector since it's a serialized variable;
            isOnGround = false;
            //since the player is jumping (and not touching the ground anymore), we set up the "isOnGround" boolean to false;
            //Is it on ground ? NO -> False;
        }

    }

    private void MoveOnPerformed(InputAction.CallbackContext obj)
    //this function will be executed when the arrows keys/or the joystick will be used;
    {
        stickDirection = obj.ReadValue<Vector2>();
    }

    private void MoveOnCanceled(InputAction.CallbackContext obj)
    //this function will be executed as soon as the arrows keys/the joystick will be released;
    {
        stickDirection = Vector2.zero;
        //the direction is set to 0 which means the player isn't moving anymore;
    }




    // Start is called before the first frame update
    void Start()
    {
        //we assign each Component to it's variable;
        //we're taking them from the GameObject which holds the script;
        myRigidbody2D = GetComponent<Rigidbody2D>();
       
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var direction = new Vector2 { x = stickDirection.x, y = 0 };

        //we set up the movement by specifying the Player will go faster till it reaches their maximum speed;
        if (myRigidbody2D.velocity.sqrMagnitude < maxSpeed)
        {
            myRigidbody2D.AddForce(direction * speed);
        }
       
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        //we're checking if the Player lands on a GameObject being on the "Ground" layer; 
        var touchingGround = ground == (ground | (1 << other.gameObject.layer));
        //we're cheking if the Player lands on a horizontal surface;
        var touchFromAbove = other.contacts[0].normal.y > other.contacts[0].normal.x;

        if (touchingGround && touchFromAbove)
        //if the Player lands on a "Ground" layer which has an horizontal surface,
        //the boolean will be set to true;
        {
            isOnGround = true;
        }
    }
}