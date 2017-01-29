﻿using UnityEngine;
using System.Collections;

public class CharacterController : MonoBehaviour {
    private bool facingRight = true; // What direction is Nova facing

    [SerializeField]
    private float maxSpeed = 10f; // Maximum speed Nova can move
    [SerializeField]
    private float jumpForce = 400f; // Jump force
    [SerializeField]
    private float doubleJumpForce = 400f; // Jump force
    [SerializeField]
    private bool airControl = false; // Can the player influence jumping?
    [SerializeField]
    private bool doubleJump = false;
    [SerializeField]
    private LayerMask whatIsGround; // A mask determining what is ground to the character
    private bool doubleJumpReady = true;
    private Transform groundCheck; // A position marking where to check if the player is grounded.
    private float groundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    private bool grounded = false; // Whether or not the player is grounded.
    private Transform ceilingCheck; // A position marking where to check for ceilings
    private float ceilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
    private Animator anim; // Reference to the player's animator component.
    private Rigidbody2D rb2d; //Reference to the player's rigidbody
    public bool canClimb; //A public bool to see if the player is in a climbable area

    void Awake()
    {
        //set up all reference
        groundCheck = transform.Find("GroundCheck");
        ceilingCheck = transform.Find("CeilingCheck");
        anim = GetComponent<Animator>();
    }

    
	// Update is called once per frame
	void Update () {

        
    }
    void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundedRadius, whatIsGround);
        if(grounded)
        {
            doubleJumpReady = true;
        }
        anim.SetBool("Ground", grounded);

    }
    public void Move(float move, bool crouch, bool jump)
    {

        //print(grounded);
        // If crouching, check to see if the character can stand up
        /*if (!crouch && anim.GetBool("Crouch"))
        {
            // If the character has a ceiling preventing them from standing up, keep them crouching
            if (Physics2D.OverlapCircle(ceilingCheck.position, ceilingRadius, whatIsGround))
                crouch = true;
        }

        // Set whether or not the character is crouching in the animator
        anim.SetBool("Crouch", crouch);*/

        //only control the player if grounded or airControl is turned on
        if (grounded || airControl)
        {
            // Reduce the speed if crouching by the crouchSpeed multiplier
            // move = (crouch ? move * crouchSpeed : move);

            // The Speed animator parameter is set to the absolute value of the horizontal input.


            // Move the character
            GetComponent<Rigidbody2D>().velocity = new Vector2(move * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);
            //Vector2 vel = GetComponent<Rigidbody2D>().velocity;*/
            //transform.position += new Vector3(move * maxSpeed * Time.deltaTime, 0, 0);
            anim.SetFloat("Speed", Mathf.Abs(move));


            // If the input is moving the player right and the player is facing left...
            if (move > 0 && !facingRight)
                // ... flip the player.
                Flip();
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (move < 0 && facingRight)
                // ... flip the player.
                Flip();
        }
        // If the player should jump...
        if (grounded && jump && anim.GetBool("Ground"))
        {
            // Add a vertical force to the player.
            grounded = false;
            anim.SetBool("Ground", false);
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce));
        }else if(!grounded && jump && doubleJump && doubleJumpReady)//If the player has already jumped and double jump is ready
        {
            doubleJumpReady = false;//Take away double jump
            GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, 0);//Set y Velocity to 0
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, doubleJumpForce));//Add another jump force
        }
    }
    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
