using UnityEngine;
using System.Collections;

public class CharacterController : MonoBehaviour {
    
    //All public fields
    [SerializeField]
    private float maxSpeed = 10f; // Maximum speed Nova can move
    [SerializeField]
    private float jumpForce = 400f; // Jump force
    public float jumpForceHold = 10;
    public float maxJumpTime = 60;
    [SerializeField]
    private float doubleJumpForce = 400f; // Jump force
    [SerializeField]
    private bool airControl = false; // Can the player influence jumping?
    [SerializeField]
    private bool doubleJump = false; // Does the player have the ability to double jump
    [SerializeField]
    private LayerMask whatIsGround; // A mask determining what is ground to the character
    [SerializeField]
    private LayerMask whatIsObstacle; // A mask determining what is obstacle to the character
    [SerializeField]
    private LayerMask whatIsLedge;
    [SerializeField]
    private LayerMask whatIsRegrowth;
    public bool canClimb; // A public bool to see if the player is in a climbable area
    public float climbSpeed = 10; // What speed will the player climb at

    //Private fields
    //These are all for Nova specifically//
    private bool facingRight = true; // What direction is Nova facing
    private Animator anim; // Reference to the player's animator component.
    private Rigidbody2D rb2d; // Reference to the player's rigidbody
    private bool doubleJumpReady = true;// Is the player ready to double jump
    private float gravityScaleSave; // A float we will use to save the gravity scale of the player
    private float maxJumpTimeInternal;
    private ImmediateStateMachine sm = new ImmediateStateMachine(); 
    float move;
    float vMove;
    bool crouch;
    bool jump;
    BoxCollider2D bc2d;
    CircleCollider2D cc2d;
    PhysicsMaterial2D physMat;


    //Ground items//
    private Transform groundCheck; // A position marking where to check if the player is grounded.
    private float groundedRadius = .1f; // Radius of the overlap circle to determine if grounded
    private bool grounded = false; // Whether or not the player is grounded.

    //Ceiling items//
    private Transform ceilingCheck; // A position marking where to check for ceilings
    private float ceilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up

    //Grapple Items//
    private float grappleRadius = .02f;// The radius to detect a grabable ledge
    private Transform grappleCheck; // The transform to see if the player is overlapping with a ledge grab
    private bool canGrapple; // Bool to see if the player can grab a ledge
    private bool climbingUp = false; // Is the player moving up a ledge
    private float xForce_1 = 1f;
    private float xForce_2 = 4f;
    private float yForce = 2.3f;

    //Her death variable//
    private bool bumped = false; // Whether or not the player is bumped into the obstacle.
    
    //Climbing Items//
    private bool climbing = false; // Is the character currently climbing?
    private bool letGo = false; //Did Nova jump off?
    private float climbVel; // Used in conjuction with climbspeed

    //Crouching//
    private bool crouching = false;
    private Transform regrowthCheck;
    private Collider2D c2D;
    private float regrowthradius = 1f;
    private float timer = 0;





    void Awake()
    {
        //set up all reference
        rb2d = GetComponent<Rigidbody2D>();
        groundCheck = transform.Find("GroundCheck");
        ceilingCheck = transform.Find("CeilingCheck");
        grappleCheck = transform.Find("GrappleCheck");
        regrowthCheck = transform.Find("RegrowthCheck");
        anim = GetComponent<Animator>();
        bc2d = GetComponent<BoxCollider2D>();
        cc2d = GetComponent<CircleCollider2D>();
        gravityScaleSave = rb2d.gravityScale;
        sm.ChangeState(enterBASIC, updateBASIC, exitBASIC);
        physMat = new PhysicsMaterial2D();
    }


    



    void Update()
    {
        Debug.Log(bc2d.friction);
        //This checks to see if Nova is on the ground
        grounded = Physics2D.Raycast(groundCheck.position, -Vector2.up, 0.3f, whatIsGround);
        if (grounded && rb2d.velocity.y < 4)
        {
            doubleJumpReady = true;
            if (!climbing)
            {
                anim.SetBool("Jumped", false);
                anim.SetBool("Ground", true);
                physMat.friction = 1;
                cc2d.sharedMaterial = physMat;
                bc2d.sharedMaterial = physMat;
                rb2d.sharedMaterial = physMat;
            }
        }
        else 
        {
            anim.SetBool("Ground", false);
            physMat.friction = 0;
            cc2d.sharedMaterial = physMat;
            bc2d.sharedMaterial = physMat;
            rb2d.sharedMaterial = physMat;
        }
        

        //Check is Nova is dead
        bumped = Physics2D.OverlapCircle(groundCheck.position, groundedRadius * 4, whatIsObstacle);

        //Check is see if Nova can ledge climb
        canGrapple = Physics2D.OverlapCircle(grappleCheck.position, grappleRadius, whatIsLedge);

        //Gonna try this
        c2D = Physics2D.OverlapCircle(regrowthCheck.position, regrowthradius, whatIsRegrowth);
    }




    //Gets input
    public void Move(float move, float vMove, bool crouch, bool jump)
    {
        this.move = move;
        this.vMove = vMove;
        this.crouch = crouch;
        this.jump = jump;
        sm.Execute();
    }
        


    //Flips the player's scale and grapple check
    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        xForce_1 *= -1;
        xForce_2 *= -1;
    }



    //----------------------------STATES-------------------------------//
    void enterBASIC()
    {

    }
    void updateBASIC()
    {
        anim.SetFloat("Speed", Mathf.Abs(rb2d.velocity.x));
        //anim.SetFloat("Speed", Mathf.Abs(move));
        rb2d.velocity = new Vector2(move * maxSpeed, rb2d.velocity.y);
        if (grounded || airControl)
        {
            // Move the character
            
            


            if (move > 0 && !facingRight)
                Flip();
            else if (move < 0 && facingRight)
                Flip();
        }

        // If the player should jump...
        if (grounded && jump && anim.GetBool("Ground"))
        {
            grounded = false;
            anim.SetBool("Ground", false);
            anim.SetBool("Jumped", true);
            rb2d.AddForce(new Vector2(0f, jumpForce));
            maxJumpTimeInternal = maxJumpTime;
        }
        //Double jump?
        else if (!grounded && jump && doubleJump && doubleJumpReady)
        {
            doubleJumpReady = false;
            //Set y Velocity to 0
            rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
            rb2d.AddForce(new Vector2(0f, doubleJumpForce));
            maxJumpTimeInternal = maxJumpTime;
        }
        if (Input.GetButton("Jump") && rb2d.velocity.y > 1 && maxJumpTimeInternal > 0)
        {
            rb2d.AddForce(new Vector2(0, jumpForceHold));
            maxJumpTimeInternal -= 1;
        }
        //TRANSITIONS
        if (!climbing && vMove > 0 && canClimb || !climbing && !grounded && canClimb)
        {
            sm.ChangeState(enterCLIMBING, updateCLIMBING, exitCLIMBING);
        }
        if (!grounded && rb2d.velocity.y < 1 && !climbing && canGrapple && move != 0)
        {
            sm.ChangeState(enterCLIMBINGUP, updateCLIMBINGUP, exitCLIMBINGUP);
        }
        if (grounded && anim.GetCurrentAnimatorStateInfo(0).IsName("NovaRigIdle") && crouch && !climbing && !jump)
        {
            sm.ChangeState(enterCROUCH, updateCROUCH, exitCROUCH);
        }
        if (bumped)
        {
            sm.ChangeState(enterDEATH, updateDEATH, exitDEATH);
        }

    }
    void exitBASIC()
    {

    }

    void enterCLIMBING()
    {
        rb2d.gravityScale = 0f;
        anim.SetBool("Climbing", true);
    }
    void updateCLIMBING()
    {
        climbVel = climbSpeed * vMove;
        if (canClimb)
        {
            anim.SetFloat("Speed", rb2d.velocity.y / 2);
            rb2d.velocity = new Vector2(0, climbVel);
        }
        else if(!canClimb && climbVel < 0)
        {
            anim.SetFloat("Speed", rb2d.velocity.y / 2);
            rb2d.velocity = new Vector2(0, climbVel);
        }
        else
        {
            anim.SetFloat("Speed", 0);
            rb2d.velocity = new Vector2(0, 0);
        }


        if (jump)
        {
            doubleJumpReady = true;
            canClimb = false;
            grounded = false;
            rb2d.velocity = new Vector2(0, 0);
            rb2d.AddForce(new Vector2(0f, jumpForce));
            sm.ChangeState(enterBASIC, updateBASIC, exitBASIC);
        }

        if (grounded && canClimb && vMove < 0)
        { 
            sm.ChangeState(enterBASIC, updateBASIC, exitBASIC);
        }
    }
    void exitCLIMBING()
    {
        rb2d.gravityScale = gravityScaleSave;
        anim.SetBool("Climbing", false);
    }
    void enterCLIMBINGUP()
    {
        rb2d.gravityScale = 0f;
        rb2d.velocity = new Vector2(0, 0);
        anim.SetBool("ClimbUp", true);
    }
    void updateCLIMBINGUP()
    {
        
        Vector2 vel = rb2d.velocity;
        if (vel.x == 0)
        {
            rb2d.velocity = new Vector2(xForce_1, yForce);
        }
        else
        {
            rb2d.velocity = new Vector2(xForce_2, 0);
        }
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("NovaRigIdle"))
        {
            sm.ChangeState(enterBASIC, updateBASIC, exitBASIC);
        }
    }
    void exitCLIMBINGUP()
    {
        //if (move == 0)
        rb2d.velocity = new Vector2(0, 0);
        anim.SetBool("ClimbUp", false);
        rb2d.gravityScale = gravityScaleSave;
    }
    void enterCROUCH()
    {
        anim.SetBool("Crouching", true);
        timer = 0;
    }
    void updateCROUCH()
    {
        timer++;
        if (c2D != null && timer > 60)
        {
            c2D.gameObject.GetComponent<RegrowthScript>().grow = true;
        }
        if (!crouch)
        {
            sm.ChangeState(enterBASIC, updateBASIC, exitBASIC);
        }
    }
    void exitCROUCH()
    {
        anim.SetBool("Crouching", false);
    }
    void enterDEATH()
    {
        rb2d.velocity = Vector3.zero;
        anim.SetBool("Death", bumped);
    }
    void updateDEATH()
    {
        //is ded
    }
    void exitDEATH()
    {

    }
   
}





//-------------------------OLD CODE-----------------------------//
/*//Check to see if we want to transition states
        if (!climbing && vMove > 0 && canClimb || !climbing && !grounded && canClimb)
        {
            rb2d.gravityScale = 0f;
            climbing = true;
            anim.SetBool("Climbing", true);
        }
        else if (climbing && grounded && canClimb && vMove < 0 || climbing && !canClimb)
        {
            rb2d.gravityScale = gravityScaleSave;
            climbing = false;
            anim.SetBool("Climbing", false);
        }
        else if (!grounded && rb2d.velocity.y < 1 && !climbing && canGrapple && move != 0)
        {
            rb2d.gravityScale = 0f;
            rb2d.velocity = new Vector2(0, 0);
            climbingUp = true;
            anim.SetBool("ClimbUp", true);
        }
        else if (grounded && anim.GetCurrentAnimatorStateInfo(0).IsName("NovaRigIdle") && crouch && !climbing && !jump)
        {
            crouching = true;
            anim.SetBool("Crouching", true);
        }



        //This is the behavior of the states
        if (!climbing && !climbingUp && !crouching)
        {
            //only control the player if grounded or airControl is turned on
            if (grounded || airControl)
            {
                // Move the character
                anim.SetFloat("Speed", Mathf.Abs(rb2d.velocity.x));
                rb2d.velocity = new Vector2(move * maxSpeed, rb2d.velocity.y);


                if (move > 0 && !facingRight)
                    Flip();
                else if (move < 0 && facingRight)
                    Flip();
            }

            // If the player should jump...
            if (grounded && jump && anim.GetBool("Ground"))
            {
                grounded = false;
                anim.SetBool("Ground", false);
                anim.SetBool("Jumped", true);
                rb2d.AddForce(new Vector2(0f, jumpForce));
                maxJumpTimeInternal = maxJumpTime;
            }
            //Double jump?
            else if (!grounded && jump && doubleJump && doubleJumpReady)
            {
                doubleJumpReady = false;
                //Set y Velocity to 0
                rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
                rb2d.AddForce(new Vector2(0f, doubleJumpForce));
                maxJumpTimeInternal = maxJumpTime;
            }
            if (Input.GetButton("Jump") && rb2d.velocity.y > 1 && maxJumpTimeInternal > 0)
            {
                rb2d.AddForce(new Vector2(0, jumpForceHold));
                maxJumpTimeInternal -= 1;
            }
        }
        //Check if Nova is in a climbing state
        else if (climbing)
        {
            climbVel = climbSpeed * vMove;
            anim.SetFloat("Speed", rb2d.velocity.y / 2);
            rb2d.velocity = new Vector2(0, climbVel);
            if (jump)
            {
                doubleJumpReady = true;
                canClimb = false;
                grounded = false;
                rb2d.velocity = new Vector2(0, 0);
                rb2d.AddForce(new Vector2(0f, jumpForce));
                anim.SetBool("Climbing", false);
            }
        }
        //Check if Nova is in a ledge grab state
        else if (climbingUp)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("NovaRigIdle"))
            {
                if (move == 0)
                    rb2d.velocity = new Vector2(0, 0);
                climbingUp = false;
                anim.SetBool("ClimbUp", false);
                rb2d.gravityScale = gravityScaleSave;
            }
            else
            {
                Vector2 vel = rb2d.velocity;
                if (vel.x == 0)
                {
                    rb2d.velocity = new Vector2(xForce_1, yForce);
                }
                else
                {
                    rb2d.velocity = new Vector2(xForce_2, 0);
                }
            }
        }
        else if (crouching)
        {
            if (!crouch)
            {
                crouching = false;
                anim.SetBool("Crouching", false);
            }
            else if(c2D != null)
            {
                c2D.gameObject.GetComponent<RegrowthScript>().grow = true;
            }
        }
    }*/
