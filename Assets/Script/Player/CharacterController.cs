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
   

    private Rigidbody2D myRigidbody2D;
    //this variable refers to a RigidBody2D Component;

    private Animator myAnimator;
    //this variable refers to an Animator Component;
    private SpriteRenderer myRenderer;
    //this variable refers to a SpriteRenderer Component;
    private Vector2 stickDirection;
    private bool isOnGround = false;
    //this bool will verify if the player is on the ground;
    private bool isFacingLeft = true;
    //this bool will check which direction the Player's facing;
    private bool myTouch = true;
    // this bool will check if the player is touching a collider with the raycast;

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
        //this function will be executed when the jump button is pressed;
        if (isOnGround)
        //we're checking if the Player is touching the ground;
        {
            myRigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            //this is the jump, in the inspector we can change the force because it's a serialized variable;
            isOnGround = false;
            myAnimator.SetBool("IsJumping", true);
            // Debug.Log("jump")
        }

    }

    private void MoveOnPerformed(InputAction.CallbackContext obj)
    //this function will be executed when we pressed the keys;
    {
        stickDirection = obj.ReadValue<Vector2>();
    }

    private void MoveOnCanceled(InputAction.CallbackContext obj)
    //this function will be executed when the keys will be realesed;
    {
        stickDirection = Vector2.zero;
        //the direction is set to 0 which means the player isn't moving anymore;
    }


    // Start is called before the first frame update
    void Start()
    {
        //we assign each Component to it's variable;
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myRenderer = GetComponent<SpriteRenderer>();
    }

             // Update is called once per frame
    void FixedUpdate()
    {
        var direction = new Vector2 { x = stickDirection.x, y = 0 };
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up);
            // This variable is checking if the player is on the ground;
        if (hit.collider != null)
        {

            myTouch = true;
            // if myTouch is true the player can jump;
        }

            //we set up the movement by specifying the Player will go faster till it reaches their maximum speed;
        if (myRigidbody2D.velocity.sqrMagnitude < maxSpeed)
        {
            myRigidbody2D.AddForce(direction * speed);
        }

           //the following lines are controlling the animator;
        var isRunning = isOnGround && Mathf.Abs(myRigidbody2D.velocity.x) > 0.1f;
        myAnimator.SetBool("IsRunning", isRunning);

        Flip();

    }


    private void Flip()
    {
        //this function will use the player's position to flip the player Sprites;
        if (stickDirection.x < -0.1f)
        {
            isFacingLeft = true;
        }

        if (stickDirection.x > 0.1f)
        {
            isFacingLeft = false;
        }
        myRenderer.flipX = isFacingLeft;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        //we're checking if the Player lands on a GameObject being on the "Ground" layer; 
        var touchingGround = ground == (ground | (1 << other.gameObject.layer));
        //var touchFromAbove = other.contacts[0].normal.y > other.contacts[0].normal.x;

        if (touchingGround && myTouch)
        {
            myAnimator.SetBool("IsJumping", false);
            isOnGround = true;
        }

    }
}
