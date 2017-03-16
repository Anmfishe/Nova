﻿using UnityEngine;
using System.Collections;

public class CharacterController : MonoBehaviour
{


    //All public fields
    public float maxVel = 5f; // Maximum speed Nova can move
    public float jumpVel = 9f; // Jump force
    public float minWalkSpeed = 0.2f;
    public float acc = 1.1f;
    public float decel = 0.95f;
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
    private LayerMask whatAreSpikes; // A mask determining what is obstacle to the character
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
    private Vector3 respawnPoint;
    private bool doubleJumpReady = true;// Is the player ready to double jump
    private float gravityScaleSave; // A float we will use to save the gravity scale of the player
    private float maxJumpTimeInternal; // Used for variable jump length
    private ImmediateStateMachine sm = new ImmediateStateMachine(); // The FSM we use for Nova's behavior
    float move; //These all capture the player input
    float vMove;
    bool crouch;
    bool jump;
    BoxCollider2D bc2d; // References to Nova's Components
    CircleCollider2D cc2d;
    PhysicsMaterial2D physMat;
    private float vSpeedThreshold = 2.5f;
    private float extendedJumpTimer = 0;
    public float extendedJumpTime = 30;
    private float runnerTimer = 0;
    private float runnerCoolDown = 10;
    private bool dontVector = false;
    private bool dirSave;




    //Ground items//
    private Transform groundCheck; // A position marking where to check if the player is grounded.
    private float groundedRadius = .1f; // Radius of the overlap circle to determine if grounded
    private bool grounded = false; // Whether or not the player is grounded.



    //Grapple Items//
    private float grappleRadius = .02f;// The radius to detect a grabable ledge
    private Transform grappleCheck; // The transform to see if the player is overlapping with a ledge grab
    private bool canGrapple; // Bool to see if the player can grab a ledge
    private float xForce_1 = 1f;
    private float xForce_2 = 2f;
    private float yForce = 2.4f;
    private float horizConst = 30;
    private float horizTimer = 0;

    //Her spike death variables//
    private bool spikeCheck = false; // Whether or not the player is spikeCheck into the obstacle.
    private float fadeOutRate = 0.005f;
    private SpriteRenderer[] spriteRenderers;
    private float fadeOutTimer = 1.5f;

    //Fire Death Items//
    private bool fire = false;
    private float darkenRate = 0.009f;
    private float darkenTimer;
    private float velX, velY;
    private float fireOne = 1;
    private ParticleSystem novaPS;

    //Climbing Items//
    private float climbVel; // Used in conjuction with climbspeed
    private int prevDir;

    //Crouching//
    private Transform regrowthCheck;
    private Collider2D c2D;
    private float regrowthradius = 1f;
    private float timer = 0;
    private bool hasGrown = false;

    //Elevating items
    private bool elevating;
    

    void Awake()
    {
        //set up all reference
        rb2d = GetComponent<Rigidbody2D>();
        groundCheck = transform.Find("GroundCheck");
        grappleCheck = transform.Find("GrappleCheck");
        regrowthCheck = transform.Find("RegrowthCheck");
        novaPS = GetComponentInChildren<ParticleSystem>();
        anim = GetComponent<Animator>();
        bc2d = GetComponent<BoxCollider2D>();
        cc2d = GetComponent<CircleCollider2D>();
        gravityScaleSave = rb2d.gravityScale;
        sm.ChangeState(enterBASIC, updateBASIC, exitBASIC);
        physMat = new PhysicsMaterial2D();
        physMat.bounciness = 0;
        physMat.friction = 0;
        cc2d.sharedMaterial = physMat;
        bc2d.sharedMaterial = physMat;
        rb2d.sharedMaterial = physMat;
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        respawnPoint = transform.position;
    }






    void Update()
    {

        //This checks to see if Nova is on the ground
        grounded = Physics2D.Raycast(groundCheck.position, -Vector2.up, 0.4f, whatIsGround);
        //grounded = Physics2D.Raycast(groundCheck.position, -Vector2.up, 0.1f, whatIsGround);
        Debug.DrawRay(groundCheck.position, new Vector2(0, -0.4f), Color.red);
        if (Physics2D.Raycast(groundCheck.position, -Vector2.up, 0.4f, whatIsGround) && rb2d.velocity.y < vSpeedThreshold)
        {
            grounded = true;
            anim.SetBool("Ground", true);
            //rb2d.gravityScale = 0;
           

        }
        else
        {
            grounded = false;
            anim.SetBool("Ground", false);
            //rb2d.gravityScale = gravityScaleSave;
            
        }
       


        //Check is Nova is dead
        spikeCheck = Physics2D.OverlapCircle(groundCheck.position, groundedRadius * 4, whatAreSpikes);

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




    //These are some of Nova's getters and setters
    public void setRespawnPoint(Vector3 newRespawnTransform)
    {
        respawnPoint = newRespawnTransform;
    }
    public bool getDir()
    {
        return facingRight;
    }
    public void setFire(bool f)
    {
        fire = f;
    }
    public void setVector(bool val)
    {
        if (val == false)
        {
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
            dirSave = facingRight;
        }
        dontVector = !val;
    }
    //Flips the player's scale and grapple check
    public void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        xForce_1 *= -1;
        xForce_2 *= -1;
    }
    public void toggleElevating()
    {
        elevating = !elevating;
    }
    public bool canGrow()
    {
        return c2D != null;
    }
    public bool getHasGrown()
    {
        return hasGrown;
    }



    //----------------------------STATES-------------------------------//
    void enterBASIC()
    {

    }
    void updateBASIC()
    {

        anim.SetFloat("Speed", Mathf.Abs(rb2d.velocity.x));
        anim.SetFloat("vSpeed", rb2d.velocity.y);
        int dir = (int)move;
        if (dir != 0 && !dontVector
            || dontVector && dirSave && dir != 1
            || dontVector && !dirSave && dir != -1)
        {
            //Debug.Log(dontVector + " " + dirSave + " " + dir + " " + Time.time);
            runnerTimer = 0;
            anim.SetBool("Running", true);
            physMat.friction = 0;
            //Debug.Log(rb2d.velocity.x);
            rb2d.constraints = RigidbodyConstraints2D.None;
            rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
            if (Mathf.Abs(rb2d.velocity.x) < minWalkSpeed * 3/4)
            {
                Vector2 hVel = new Vector2(minWalkSpeed * dir, rb2d.velocity.y);
                rb2d.velocity = hVel;
            }
            else
            {
                if (dir > 0 && rb2d.velocity.x > 0 || dir < 0 && rb2d.velocity.x < 0)
                {
                    Vector2 temp = new Vector2(Mathf.Abs(rb2d.velocity.x) * acc * dir, rb2d.velocity.y);
                    if (Mathf.Abs(temp.x) > maxVel)
                    {
                        temp.x = maxVel * dir;
                    }
                    rb2d.velocity = temp;
                }
                else
                {
                    runnerTimer = 0;
                    //anim.SetBool("Running", false);
                    Vector2 decelVec = new Vector2(rb2d.velocity.x * decel, rb2d.velocity.y);
                    rb2d.velocity = decelVec;
                }
            }
        }
        else
        {
            
            anim.SetBool("Running", false);
            //physMat.friction = 1;
            Vector2 decelVec = new Vector2(rb2d.velocity.x * decel, rb2d.velocity.y);
            if (grounded)
            {
                rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
            }
            else
            {
                rb2d.constraints = RigidbodyConstraints2D.None;
                rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
            }       
            rb2d.velocity = decelVec;
            
            //Debug.Log("X Vel: " + rb2d.velocity.x + " Y Vel: " + rb2d.velocity.y + " " + Time.time);
            
            
        }




        if (rb2d.velocity.x > 0 && !facingRight)
            Flip();
        else if (rb2d.velocity.x < 0 && facingRight)
            Flip();
        if (!grounded && extendedJumpTimer < extendedJumpTime)
        {
            extendedJumpTimer++;
        }
        else if(grounded)
        {
            extendedJumpTimer = 0;
        }
        if (grounded && jump && anim.GetBool("Ground") || !grounded && jump && extendedJumpTimer < extendedJumpTime)
        {
            sm.ChangeState(enterJUMP, updateJUMP, exitJUMP);
        }

        //TRANSITIONS
        if(elevating)
        {
            sm.ChangeState(enterELEVATING, updateELEVATING, exitELEVATING);
        }
        if (fire)
        {
            sm.ChangeState(enterFIREDEATH, updateFIREDEATH, exitFIREDEATH);
        }
        if (vMove > 0 && canClimb)
        {
            sm.ChangeState(enterCLIMBING, updateCLIMBING, exitCLIMBING);
        }
        if (grounded && rb2d.velocity.x == 0 && rb2d.velocity.y == 0 && crouch && !jump
            && anim.GetCurrentAnimatorStateInfo(0).IsName("NovaRigIdle"))
        {
            sm.ChangeState(enterCROUCH, updateCROUCH, exitCROUCH);
        }
        if (spikeCheck)
        {
            sm.ChangeState(enterSPIKEDEATH, updateSPIKEDEATH, exitSPIKEDEATH);
        }
        if (!grounded && canClimb)
        {
            sm.ChangeState(enterCLIMBING, updateCLIMBING, exitCLIMBING);
        }
        if (!grounded && canGrapple && move != 0)
        {
            sm.ChangeState(enterCLIMBINGUP, updateCLIMBINGUP, exitCLIMBINGUP);
        }

    }
    void exitBASIC()
    {
        rb2d.constraints = RigidbodyConstraints2D.None;
        rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
    }






    void enterJUMP()
    {
        grounded = false;
        anim.SetBool("Ground", false);
        anim.SetBool("Jumped", true);
        anim.Play("NovaRigJumpingAnim");
        //rb2d.AddForce(new Vector2(0f, jumpForce));
        //physMat.friction = 0;
        rb2d.velocity = new Vector2(rb2d.velocity.x, jumpVel);
        maxJumpTimeInternal = maxJumpTime;
        rb2d.gravityScale = gravityScaleSave;
    }
    void updateJUMP()
    {
        if (!grounded && jump && doubleJump && doubleJumpReady)
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
        if (airControl)
        {
            anim.SetFloat("Speed", Mathf.Abs(rb2d.velocity.x));
            //anim.SetFloat("Speed", Mathf.Abs(move));
            int dir = (int)move;
            if (dir != 0)
            {
                if (Mathf.Abs(rb2d.velocity.x) < minWalkSpeed / 2)
                {
                    Vector2 hVel = new Vector2(minWalkSpeed * dir, rb2d.velocity.y);
                    rb2d.velocity = hVel;
                }
                else
                {
                    if (dir > 0 && rb2d.velocity.x > 0 || dir < 0 && rb2d.velocity.x < 0)
                    {
                        Vector2 temp = new Vector2(Mathf.Abs(rb2d.velocity.x) * acc * dir, rb2d.velocity.y);
                        if (Mathf.Abs(temp.x) > maxVel)
                        {
                            temp.x = maxVel * dir;
                        }
                        rb2d.velocity = temp;
                    }
                    else
                    {
                        Vector2 decelVec = new Vector2(rb2d.velocity.x * decel, rb2d.velocity.y);
                        rb2d.velocity = decelVec;
                    }
                }
            }
            else
            {
                Vector2 decelVec = new Vector2(rb2d.velocity.x * decel, rb2d.velocity.y);
                rb2d.velocity = decelVec;
            }




            if (rb2d.velocity.x > 0 && !facingRight)
                Flip();
            else if (rb2d.velocity.x < 0 && facingRight)
                Flip();
        }
        //TRANSITIONS
        if (fire)
        {
            sm.ChangeState(enterFIREDEATH, updateFIREDEATH, exitFIREDEATH);
        }
        if (spikeCheck)
        {
            sm.ChangeState(enterSPIKEDEATH, updateSPIKEDEATH, exitSPIKEDEATH);
        }
        if (!grounded && canClimb)
        {
            sm.ChangeState(enterCLIMBING, updateCLIMBING, exitCLIMBING);
        }
        if (!grounded && rb2d.velocity.y < 1 && canGrapple && move != 0)
        {
            sm.ChangeState(enterCLIMBINGUP, updateCLIMBINGUP, exitCLIMBINGUP);
        }
        if (grounded && rb2d.velocity.y < vSpeedThreshold)
        {
            sm.ChangeState(enterBASIC, updateBASIC, exitBASIC);
        }
    }
    void exitJUMP()
    {
        doubleJumpReady = true;
        anim.SetBool("Jumped", false);
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
            anim.SetFloat("vSpeed", rb2d.velocity.y / 2);
            rb2d.velocity = new Vector2(0, climbVel);
            prevDir = (int)climbVel;
        }
        else if (!canClimb && climbVel < 0 && prevDir > 0)
        {
            anim.SetFloat("Speed", rb2d.velocity.y / 2);
            rb2d.velocity = new Vector2(0, climbVel);
        }
        else if (!canClimb && climbVel > 0 && prevDir < 0)
        {
            anim.SetFloat("Speed", rb2d.velocity.y / 2);
            rb2d.velocity = new Vector2(0, climbVel);
        }
        else if (!canClimb && climbVel < 0 && prevDir < 0 && !grounded)
        {
            anim.SetBool("Jumped", false);
            sm.ChangeState(enterBASIC, updateBASIC, exitBASIC);
        }
        else
        {
            anim.SetFloat("vSpeed", 0);
            rb2d.velocity = new Vector2(0, 0);
        }


        if (jump)
        {
            doubleJumpReady = true;
            canClimb = false;
            sm.ChangeState(enterJUMP, updateJUMP, exitJUMP);
        }
        if (fire)
        {
            sm.ChangeState(enterFIREDEATH, updateFIREDEATH, exitFIREDEATH);
        }
        if (grounded && canClimb && vMove < 0)
        {
            sm.ChangeState(enterBASIC, updateBASIC, exitBASIC);
        }
        if (canGrapple && vMove > 0)
        {
            sm.ChangeState(enterCLIMBINGUP, updateCLIMBINGUP, exitCLIMBINGUP);
        }
    }
    void exitCLIMBING()
    {
        rb2d.gravityScale = gravityScaleSave;
        anim.SetBool("Climbing", false);
    }











    void enterCLIMBINGUP()
    {
        horizTimer = 0;
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
        else if (horizTimer < horizConst)
        {
            horizTimer++;
            rb2d.velocity = new Vector2(xForce_2, 0);
        }
        else if (horizTimer >= horizConst)
        {
            sm.ChangeState(enterBASIC, updateBASIC, exitBASIC);
        }
        //}

    }
    void exitCLIMBINGUP()
    {
        //if (move == 0)
        anim.SetBool("ClimbUp", false);
        anim.SetBool("Running", false);
        rb2d.velocity = new Vector2(0, 0);
        rb2d.gravityScale = gravityScaleSave;
    }












    //fvgfhjkjmjnhgngnh - Iman, 2017
        
    void enterCROUCH()
    {
        rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
        anim.SetBool("Crouching", true);
        timer = 0;
    }
    void updateCROUCH()
    {
        timer++;
        if (c2D != null)
        {
            if (timer > 60)
            {
                hasGrown = true;
                c2D.gameObject.GetComponent<RegrowthScript>().grow = true;
            }
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














    void enterSPIKEDEATH()
    {
        rb2d.velocity = Vector3.zero;
        anim.SetBool("SpikeDeath", spikeCheck);
        //anim.Play("NovaDeadOnSpikeBellyUpAnim");
        fadeOutTimer = 1.5f;
    }
    void updateSPIKEDEATH()
    {
        fadeOutTimer -= fadeOutRate;
        foreach (SpriteRenderer sr in spriteRenderers)
        {
            sr.color = new Color(1f, 1f, 1f, sr.color.a - fadeOutRate);
        }
        if (fadeOutTimer <= 0)
        {
            foreach (SpriteRenderer sr in spriteRenderers)
            {
                sr.color = new Color(1f, 1f, 1f, 1f);
            }
            transform.position = respawnPoint;
            anim.SetBool("SpikeDeath", false);
            anim.Play("NovaRigIdle");
            
            sm.ChangeState(enterBASIC, updateBASIC, exitBASIC);
        }
    }
    void exitSPIKEDEATH()
    {
        //anim.SetBool("SpikeDeath", false);
        spikeCheck = false;
    }













    void enterFIREDEATH()
    {
        velX = rb2d.velocity.x;
        velY = rb2d.velocity.y;
        darkenTimer = 2f;
        fireOne = 1;
        rb2d.gravityScale = 0;
        novaPS.Play();
    }
    void updateFIREDEATH()
    {
        if (darkenTimer > 0)
        {
            if (fireOne > 0)
            {
                fireOne -= darkenRate * 2;
                anim.speed = Mathf.Lerp(1, 0, 1 - fireOne);
                rb2d.velocity = new Vector2(Mathf.Lerp(velX, 0, 1 - fireOne), Mathf.Lerp(velY, 0, 1 - fireOne));
            }
            darkenTimer -= darkenRate;
            foreach (SpriteRenderer sr in spriteRenderers)
            {
                sr.color = new Color(sr.color.r - darkenRate, sr.color.g - darkenRate, sr.color.b - darkenRate, 1f);
            }


        }
        else
        {
            transform.position = respawnPoint;
            foreach (SpriteRenderer sr in spriteRenderers)
            {
                sr.color = new Color(1f, 1f, 1f, 1f);
            }
            sm.ChangeState(enterBASIC, updateBASIC, exitBASIC);
        }

    }
    void exitFIREDEATH()
    {
        fire = false;
        rb2d.gravityScale = gravityScaleSave;
        anim.speed = 1;
        novaPS.Stop();
    }
    void enterELEVATING()
    {
        foreach(SpriteRenderer sr in spriteRenderers)
        {
            sr.color = new Color(1f, 1f, 1f, 0f);
        }
    }
    void updateELEVATING()
    {
        rb2d.velocity = Vector2.zero;
        anim.Play("NovaRigIdle");
        if (!elevating)
        {
            
            sm.ChangeState(enterCLIMBINGUP, updateCLIMBINGUP, exitCLIMBINGUP);
        }
    }
    void exitELEVATING() {
        anim.Play("NovaRigRunningAnim");
        foreach (SpriteRenderer sr in spriteRenderers)
        {
            sr.color = new Color(1f, 1f, 1f, 1f);
        }
    }
}
