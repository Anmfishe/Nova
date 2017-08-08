using UnityEngine;
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
    public float extendedJumpTime = 30;
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
    [SerializeField]
    private LayerMask whatIsPickUp;
    [HideInInspector]
    public bool canClimb; // A public bool to see if the player is in a climbable area
    [HideInInspector]
    public bool canMove = false;
    [HideInInspector]
    public bool startEndScene = false;
    [HideInInspector]
    public bool pauseNova = false;
    [HideInInspector]
    public float speedCoef = 1;
    [HideInInspector]
    public bool canBurn = false;
    [HideInInspector]
    public bool hasBurned = false;
    [HideInInspector]
    public bool hasPushed = false;
    public float climbSpeed = 10; // What speed will the player climb at
    public Camera mainCam;
    public Camera cutsceneCam;
    public bool skipOpening = false;
    public AudioClip[] footsteps;
    public AudioClip[] climbing;
    public AudioClip[] jumping;
    public AudioClip[] ledgeGrab;
    public AudioClip landingSound;
    public AudioClip climbingLandingSound;
    public AudioClip fireDeath;
    public AudioClip spikeDeath;

    //Private fields
    //These are all for Nova specifically//
    private bool facingRight = true; // What direction is Nova facing
    [HideInInspector]
    public Animator anim; // Reference to the player's animator component.
    private Rigidbody2D rb2d; // Reference to the player's rigidbody
    private AudioSource novaAS;
    private Vector3 respawnPoint;
    private Transform rightHand;
    private bool doubleJumpReady = true;// Is the player ready to double jump
    private float gravityScaleSave; // A float we will use to save the gravity scale of the player
    private float maxJumpTimeInternal; // Used for variable jump length
    private ImmediateStateMachine sm = new ImmediateStateMachine(); // The FSM we use for Nova's behavior
    float move; //These all capture the player input
    float vMove;
    bool eHit;
    bool jump;
    private float maxFall = -25;
    private float vSpeedThreshold = 2.5f;
    private float extendedJumpTimer = 0;
    private float runnerTimer = 0;
    private float runnerCoolDown = 20;
    private bool dontVector = false;
    private bool dirSave;
    private PolygonCollider2D[] polyColliders;
    private Vector3 startPos = new Vector3(8, -4.54f, 0);
    private bool readyToRestart;
    private bool freezeNova = false;
    [HideInInspector]
    public GameObject web;
    public Transform novaFlower;
    private Transform flowerSpawn;


    //Holding variables//
    [HideInInspector]
    public bool holdingSomething = false;
    [HideInInspector]
    public bool checkStick = false;
    [HideInInspector]
    public bool failToLight = false;
    private GameObject heldItem;
    private RaycastHit2D pickUpHit;
    private bool qUP = false;





    //Ground items//
    private Transform groundCheck; // A position marking where to check if the player is grounded.
    private Transform groundCheck2;
    private float groundedRadius = .1f; // Radius of the overlap circle to determine if grounded
    private bool grounded = false; // Whether or not the player is grounded.
    private bool grounded2 = false;


    void Awake()
    {
        //set up all reference
        rb2d = GetComponent<Rigidbody2D>();
        novaAS = GetComponent<AudioSource>();
        groundCheck = transform.Find("GroundCheck");
        groundCheck2 = transform.Find("GroundCheck2");
        grappleCheck = transform.Find("GrappleCheck");
        grappleCheck2 = transform.Find("GrappleCheck2");
        regrowthCheck = transform.Find("RegrowthCheck");
        pushCheck = transform.Find("PushCheck");
        pushAnchor = transform.Find("PushAnchor");
        flowerSpawn = transform.Find("FlowerSpawn");
        rightHand = transform.Find("LowerBody/UpperBody/UpperArm/LowerArm/StraightHand");
        novaPS = GetComponentInChildren<ParticleSystem>();
        anim = GetComponent<Animator>();
        gravityScaleSave = rb2d.gravityScale;
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        respawnPoint = transform.position;
        fireColor = novaPS.startColor;
        white = new Color(1, 1, 1, 1);
        polyColliders = GetComponentsInChildren<PolygonCollider2D>();
        //novaFlower = Resources.Load("NovaFlower") as Transform;
        if (!skipOpening)
        {
            rb2d.isKinematic = true;

            sm.ChangeState(enterINTRO, updateINTRO, exitINTRO);
            transform.position = startPos;
        }
        else
        {
            canMove = true;
            rb2d.isKinematic = false;
            sm.ChangeState(enterBASIC, updateBASIC, exitBASIC);
            anim.Play("NovaIdle 0");
            StartCoroutine(polysOff());
        }
        Cursor.visible = false;
    }



    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawLine(pushCheck.position, (Vector2)pushCheck.position + Vector2.right * transform.localScale.x * pushDist);
    //}




    void Update()
    {

        //This checks to see if Nova is on the ground
        //Debug.DrawRay(groundCheck.position, new Vector2(0, -0.4f), Color.red);
        if (Physics2D.Raycast(groundCheck.position, -Vector2.up, 0.8f, whatIsGround) && rb2d.velocity.y < vSpeedThreshold)
        {
            grounded = true;
            anim.SetBool("Ground", true);
        }
        else
        {
            grounded = false;
            anim.SetBool("Ground", false);
            //rb2d.gravityScale = gravityScaleSave;
        }
        if (Physics2D.Raycast(groundCheck2.position, -Vector2.up, 1f, whatIsGround) && rb2d.velocity.y < vSpeedThreshold)
        {
            grounded2 = true;
        }
        else
        {
            grounded2 = false;
        }



        //Check is Nova is dead
        spikeCheck = Physics2D.OverlapCircle(groundCheck.position, groundedRadius * 4, whatAreSpikes);



        //Check is see if Nova can ledge climb
        highGrappleCheck = Physics2D.OverlapCircle(grappleCheck.position, grappleRadius, whatIsLedge);
        //lowGrappleCheck = Physics2D.OverlapCircle(grappleCheck2.position, grappleRadius/2, whatIsLedge);
        lowGrappleCheck = Physics2D.OverlapPoint(grappleCheck2.position, whatIsLedge);


        //Regrowth
        regrowthHit = Physics2D.Raycast(groundCheck.position, Vector2.right, regrowthDist, whatIsRegrowth);
        Debug.DrawRay(groundCheck.position, new Vector2(regrowthDist, 0), Color.green);


        //PickUp
        pickUpHit = Physics2D.Raycast(groundCheck.position, Vector2.right, regrowthDist, whatIsPickUp);


        //Push
        Physics2D.queriesStartInColliders = false;
        pushHit = Physics2D.RaycastAll(pushCheck.position, Vector2.right * transform.localScale.x, pushDist);
        Debug.DrawRay(pushCheck.position, Vector2.right * transform.localScale.x, Color.yellow);
        if (pushHit.Length > 0)
        {
            for (int i = 0; i < pushHit.Length; i++)
            {
                if (pushHit[i].collider != null && pushHit[i].collider.gameObject.tag == "Pushable" && !pushing
                    && pushHit[i].collider.gameObject.GetComponent<PushableController>().grounded)
                {
                    canPush = true;
                    pushedObj = pushHit[i].collider.gameObject;
                    //pushedObj = pushHit.collider.gameObject;
                    //pushHit.collider.gameObject.GetComponent<PushableController>().beingPushed = true;
                    break;
                }
                else
                {
                    canPush = false;
                }
            }
        }
        else
        {
            canPush = false;
        }


        Physics2D.queriesStartInColliders = true;
    }
    private float mRightSpeed = 0.01f;
    public void FixedUpdate()
    {
        sm.Execute();
        if (Input.GetKeyUp(KeyCode.Q))
        {
            qUP = true;
        }
        if (mRight)
        {
            transform.position = new Vector3(transform.position.x + mRightSpeed, transform.position.y, transform.position.z);
        }
        if(Input.GetKey(KeyCode.I) && Input.GetKey(KeyCode.O) && Input.GetKey(KeyCode.P))
        {
            Application.LoadLevel("Intro");
        }
    }















    //Gets input
    public void Move(float move, float vMove, bool crouch, bool jump)
    {
        this.move = move;
        this.vMove = vMove;
        this.eHit = crouch;
        this.jump = jump;


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
        regrowthDist *= -1;
        pushMultiplier *= -1;
    }
    public void toggleElevating()
    {
        elevating = !elevating;
    }
    public bool canGrow()
    {
        return regrowthHit != false;
    }
    

    public void switchBack()
    {
        cutsceneCam.enabled = false;
        mainCam.enabled = true;
        mainCam.GetComponent<UnitySampleAssets._2D.Camera2DFollow>().startFadeIn();
        StartCoroutine(returnControl(2));
    }
    public void playFootstep()
    {
        if (!elevating && grounded)
        {
            novaAS.volume = 0.325f;
            novaAS.clip = footsteps[Random.Range(0, footsteps.Length)];
            novaAS.Play();
            int i = Random.Range(1, 3);
            if (i == 2)
            {
                //float offset = Random.Range(-.01f, .01f);
                Instantiate(novaFlower, flowerSpawn.position, Quaternion.identity);
            }
        }
    }
    public void hardStopNova(bool b)
    {
        freezeNova = b;
    }
    public void playJumpingSFX()
    {
        if (jumping.Length > 0) 
        {
            novaAS.volume = 0.3f;
            novaAS.clip = jumping[Random.Range (0, jumping.Length)];
            novaAS.Play();
        }
    }
    public void playLedgeGrabSFX()
    {
        if (ledgeGrab.Length >0)
        {
            novaAS.volume = 0.4f;
            novaAS.clip = ledgeGrab[Random.Range (0, ledgeGrab.Length)];
            novaAS.Play();
        }
    }
    public void playClimbingSFX()
    {
        novaAS.volume = 0.2f;
        novaAS.clip = climbing[Random.Range(0, climbing.Length)];
        novaAS.Play();
    }
    private bool mRight = false;
    void moveRight(float speed = 0.1f)
    {
        mRight = !mRight;
        mRightSpeed = speed;
    }
    private void moveNova(int dir, float multiplier = 1, bool flip = true, bool airborn = true)
    {

        rb2d.constraints = RigidbodyConstraints2D.None;
        rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
        if (dir == 1 && rb2d.velocity.x < minWalkSpeed * dir / 2
            || dir == -1 && rb2d.velocity.x > minWalkSpeed * dir / 2)
        {
            //Debug.Log("Case 1");
            if (rb2d.velocity.y > 1 && !airborn)
            {
                Vector2 hVel = new Vector2(minWalkSpeed * dir, 0);
                rb2d.velocity = hVel;
            }
            else
            {
                Vector2 hVel = new Vector2(minWalkSpeed * dir, rb2d.velocity.y);
                rb2d.velocity = hVel;
            }

        }
        else
        {
            if (dir > 0 && rb2d.velocity.x > 0 || dir < 0 && rb2d.velocity.x < 0)
            {
                //Debug.Log("Case 2");
                Vector2 temp = new Vector2(Mathf.Abs(rb2d.velocity.x) * acc * dir, rb2d.velocity.y);
                if (Mathf.Abs(temp.x) > maxVel * multiplier)
                {
                    temp.x = maxVel * dir * multiplier;
                }
                
                rb2d.velocity = temp;
            }
            else if (runnerTimer > runnerCoolDown)
            {
                //Debug.Log("Case 3");
                runnerTimer = 0;
                rb2d.velocity = new Vector2(0, rb2d.velocity.y);
            }
            /*else
            {
                Debug.Log("Case 4");
                Vector2 decelVec = new Vector2(rb2d.velocity.x * decel, rb2d.velocity.y);
                rb2d.velocity = decelVec;
            }*/
        }
        if (flip)
        {
            if (rb2d.velocity.x > 0 && !facingRight)
                Flip();
            else if (rb2d.velocity.x < 0 && facingRight)
                Flip();
        }
        runnerTimer++;
    }
    public void pickUpFunc()
    {
        qUP = false;
        holdingSomething = true;
        heldItem = pickUpHit.collider.gameObject;
        heldItem.GetComponent<PickUpController>().pickUp();
        heldItem.transform.parent = rightHand;
        heldItem.transform.position = rightHand.position;
        anim.SetBool("PickUp", false);
        StartCoroutine(returnControl(1.5f));
    }
    public void drop()
    {
        if (holdingSomething)
        {
            holdingSomething = false;
            heldItem.transform.parent = null;
            heldItem.GetComponent<PickUpController>().drop();
        }
    }

    IEnumerator playCutscene(int sceneNumber)
    {
        mainCam.GetComponent<UnitySampleAssets._2D.Camera2DFollow>().startFadeOut();
        yield return new WaitForSeconds(4);
        mainCam.enabled = false;
        cutsceneCam.enabled = true;
    }
    IEnumerator returnControl(float time)
    {
        yield return new WaitForSeconds(time);
        canMove = true;
    }
    IEnumerator polysOff()
    {
        mainCam.GetComponent<UnitySampleAssets._2D.Camera2DFollow>().starting = true;
        yield return new WaitForSeconds(5);
        foreach (PolygonCollider2D pg2d in polyColliders)
        {
            pg2d.enabled = false;
        }
    }
    IEnumerator EndRoutine(float time)
    {
        yield return new WaitForSeconds(time);
        mainCam.GetComponent<UnitySampleAssets._2D.Camera2DFollow>().startFadeOut();
        yield return new WaitForSeconds(time / 2);
        mainCam.GetComponent<UnitySampleAssets._2D.Camera2DFollow>().showTitle = true;
        readyToRestart = true;
        //play some music?
    }
    IEnumerator pauseNovaRoutine(float time)
    {
        
        anim.SetBool("Hilltop", true);
        moveRight(0.005f);
        yield return new WaitForSeconds(2);
        mainCam.GetComponent<UnitySampleAssets._2D.Camera2DFollow>().fadeRate = 0.0075f;
        mainCam.GetComponent<UnitySampleAssets._2D.Camera2DFollow>().showTitle = true;
        yield return new WaitForSeconds(2);
        moveRight();
        anim.SetBool("Hilltop", false);
        yield return new WaitForSeconds(4);
        mainCam.GetComponent<UnitySampleAssets._2D.Camera2DFollow>().showTitle = false;
        canMove = true;
        mainCam.GetComponent<UnitySampleAssets._2D.Camera2DFollow>().fadeRate = 0.01f;
        yield return new WaitForSeconds(3);
        
        
    }
    IEnumerator burnWeb()
    {
        canMove = false;
        hasBurned = true;
        yield return new WaitForSeconds(0.25f);
        anim.Play("BurnWall");
        yield return new WaitForSeconds(1f);
        web.GetComponent<WebController>().burnAway();
        yield return new WaitForSeconds(4f);
        canMove = true;
    }
    IEnumerator failToBurn()
    {
        canMove = false;
        yield return new WaitForSeconds(0.25f);
        anim.Play("NovaFailToIgnite");
        yield return new WaitForSeconds(6f);
        canMove = true;
    }
    IEnumerator reloadLevel()
    {
        mainCam.GetComponent<UnitySampleAssets._2D.Camera2DFollow>().startFadeOut();
        mainCam.GetComponent<UnitySampleAssets._2D.Camera2DFollow>().fadeRate = 0.1f;
        yield return new WaitForSeconds(1.5f);
        Application.LoadLevel("ElderTreeEndingScenePart2");
    }





















    //----------------------------STATES-------------------------------//
    void enterINTRO()
    {

    }
    void updateINTRO()
    {

        if ((move != 0 || vMove != 0 || jump) && canMove)
        {
            mainCam.GetComponent<UnitySampleAssets._2D.Camera2DFollow>().showTitle = false;
            cutsceneCam.transform.GetChild(0).GetComponent<CutsceneController>().fadeMusic = true;
            anim.SetBool("GetUp", true);
        }
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("NovaIdle 0") || anim.GetCurrentAnimatorStateInfo(0).IsName("NovaRigIdle"))
        {
            rb2d.isKinematic = false;
            sm.ChangeState(enterBASIC, updateBASIC, exitBASIC);
        }
    }
    void exitINTRO()
    {
        StartCoroutine(polysOff());
    }






    void enterBASIC()
    {

    }
    void updateBASIC()
    {

        anim.SetFloat("Speed", Mathf.Abs(rb2d.velocity.x));
        anim.SetFloat("vSpeed", rb2d.velocity.y);
        int dir = (int)move;
        if (freezeNova)
        {
            rb2d.velocity = Vector2.zero;
        }
        else if (canMove && (dir != 0 && !dontVector
            || dontVector && dirSave && dir != 1
            || dontVector && !dirSave && dir != -1))
        {
            moveNova(dir, speedCoef, true, false);
        }
        else
        {

            //physMat.friction = 1;
            Vector2 decelVec = new Vector2(rb2d.velocity.x * decel, rb2d.velocity.y);
            rb2d.velocity = decelVec;
            if (grounded)
            {
                rb2d.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            }
            else
            {
                rb2d.constraints = RigidbodyConstraints2D.None;
                rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
            }


            //Debug.Log("X Vel: " + rb2d.velocity.x + " Y Vel: " + rb2d.velocity.y + " " + Time.time);


        }




        //TRANSITIONS
        if (canPush && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
        {
            sm.ChangeState(enterPUSH, updatePUSH, exitPUSH);
        }
        if (!grounded && extendedJumpTimer < extendedJumpTime)
        {
            extendedJumpTimer++;
        }
        else if (grounded)
        {
            extendedJumpTimer = 0;
        }
        //if (canMove && (grounded && jump && anim.GetBool("Ground") || !grounded && jump && extendedJumpTimer < extendedJumpTime))
        if (jump && canMove)
        {
            sm.ChangeState(enterJUMP, updateJUMP, exitJUMP);
        }
        if (!grounded && !jump && rb2d.velocity.y < -5)
        {
            sm.ChangeState(enterFALL, updateFALL, exitFALL);
        }

        if (pauseNova)
        {
            pauseNova = false;
            StartCoroutine(pauseNovaRoutine(0));
        }
        if (startEndScene)
        {
            startEndScene = false;
            anim.Play("NovaEndingScene");
            StartCoroutine(EndRoutine(5));
        }
        if (readyToRestart && Input.GetKeyDown(KeyCode.R))
        {
            Application.LoadLevel(0);
        }
        if (elevating)
        {
            sm.ChangeState(enterELEVATING, updateELEVATING, exitELEVATING);
        }
        if (fire)
        {
            sm.ChangeState(enterFIREDEATH, updateFIREDEATH, exitFIREDEATH);
        }
        if (vMove > 0 && canClimb && !holdingSomething)
        {
            sm.ChangeState(enterCLIMBING, updateCLIMBING, exitCLIMBING);
        }
        if (grounded && rb2d.velocity.x == 0 && rb2d.velocity.y == 0 && eHit && !jump
            && (anim.GetCurrentAnimatorStateInfo(0).IsName("NovaRigIdle") || anim.GetCurrentAnimatorStateInfo(0).IsName("NovaIdle 0")))
        {
            sm.ChangeState(enterCROUCH, updateCROUCH, exitCROUCH);
        }
        if (grounded && !jump && pickUpHit.collider != null && !holdingSomething)
        {


            canMove = false;
            anim.Play("NovaPickup");

            //play some kind of anim
        }
        if (spikeCheck)
        {
            sm.ChangeState(enterSPIKEDEATH, updateSPIKEDEATH, exitSPIKEDEATH);
        }
        if (!grounded && canClimb && !holdingSomething)
        {
            sm.ChangeState(enterCLIMBING, updateCLIMBING, exitCLIMBING);
        }
        if (canBurn && Input.GetKey(KeyCode.E) && canMove && grounded && holdingSomething)
        {
            
            StartCoroutine(burnWeb());
        }
        if (failToLight && Input.GetKey(KeyCode.E) && canMove && grounded && holdingSomething)
        {

            StartCoroutine(failToBurn());
        }


    }
    void exitBASIC()
    {
        rb2d.constraints = RigidbodyConstraints2D.None;
        rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
    }







    void enterFALL()
    {
        //anim.SetBool("Fall", true);
    }
    void updateFALL()
    {
        anim.SetFloat("Speed", Mathf.Abs(rb2d.velocity.x));
        anim.SetFloat("vSpeed", rb2d.velocity.y);
        int dir = (int)move;
        if (freezeNova)
        {
            rb2d.velocity = Vector2.zero;
        }
        else if (canMove && (dir != 0 && !dontVector
            || dontVector && dirSave && dir != 1
            || dontVector && !dirSave && dir != -1))
        {
            moveNova(dir, speedCoef);
        }
        else
        {
            rb2d.constraints = RigidbodyConstraints2D.None;
            rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        if (rb2d.velocity.y < maxFall)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, maxFall);
        }

        //TRANSITIONS
        if (grounded || rb2d.velocity.y > -5)
        {
            GetComponent<AudioSource> ().PlayOneShot (landingSound, 1.0f);
            sm.ChangeState(enterBASIC, updateBASIC, exitBASIC);
        }
        if (canClimb && !grounded && !holdingSomething)
        {
            sm.ChangeState(enterCLIMBING, updateCLIMBING, exitCLIMBING);
        }
        if (!grounded && (highGrappleCheck && !holdingSomething || lowGrappleCheck) && move != 0)
        {
            sm.ChangeState(enterCLIMBINGUP, updateCLIMBINGUP, exitCLIMBINGUP);
        }
        if (spikeCheck)
        {
            sm.ChangeState(enterSPIKEDEATH, updateSPIKEDEATH, exitSPIKEDEATH);
        }
        if (fire)
        {
            sm.ChangeState(enterFIREDEATH, updateFIREDEATH, exitFIREDEATH);
        }
    }
    void exitFALL()
    {
        //anim.SetBool("Fall", false);
    }






    void enterJUMP()
    {
        grounded = false;
        anim.SetBool("Ground", false);
        anim.SetBool("Jumped", true);
        //anim.Play("NovaRigJumpingAnim");
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
            if (canMove && (dir != 0 && !dontVector
            || dontVector && dirSave && dir != 1
            || dontVector && !dirSave && dir != -1))
            {
                moveNova(dir, speedCoef);
            }
            else
            {
                if (canMove)
                {
                    Vector2 decelVec = new Vector2(rb2d.velocity.x * decel, rb2d.velocity.y);
                    rb2d.velocity = decelVec;
                }
                else
                {
                    rb2d.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
                }
            }

        }
        //TRANSITIONS
        if (elevating)
        {
            sm.ChangeState(enterELEVATING, updateELEVATING, exitELEVATING);
        }
        if (fire)
        {
            sm.ChangeState(enterFIREDEATH, updateFIREDEATH, exitFIREDEATH);
        }
        if (spikeCheck)
        {
            sm.ChangeState(enterSPIKEDEATH, updateSPIKEDEATH, exitSPIKEDEATH);
        }
        if (!grounded && canClimb && !holdingSomething)
        {
            sm.ChangeState(enterCLIMBING, updateCLIMBING, exitCLIMBING);
        }
        if (!grounded && (highGrappleCheck && !holdingSomething || lowGrappleCheck) && move != 0)
        {
            sm.ChangeState(enterCLIMBINGUP, updateCLIMBINGUP, exitCLIMBINGUP);
        }
        if (grounded && rb2d.velocity.y < vSpeedThreshold && rb2d.velocity.y > -5)
        {
            GetComponent<AudioSource> ().PlayOneShot (landingSound, 1.0f);
            sm.ChangeState(enterBASIC, updateBASIC, exitBASIC);
        }
    }
    void exitJUMP()
    {
        doubleJumpReady = true;
        anim.SetBool("Jumped", false);
    }






    //Push and pull items
    private Transform pushCheck;
    private Transform pushAnchor;
    private bool pushing = false;
    [HideInInspector]
    public bool canPush = false;
    private float pushDist = 0.4f;
    private RaycastHit2D[] pushHit;
    private GameObject pushedObj = null;
    private float pushMultiplier = 1;

    void enterPUSH()
    {
        speedCoef = 0.6f;
        //Debug.Log("Entering Push: " + Time.time);
        anim.SetBool("Pushing", true);
        pushing = true;
        //pushedObj.transform.parent = transform;
        pushedObj.GetComponent<PushableController>().beingPushed = true;
        pushedObj.GetComponent<FixedJoint2D>().enabled = true;
        //pushedObj.GetComponent<FixedJoint2D>().anchor = pushAnchor.position;
        pushedObj.GetComponent<FixedJoint2D>().connectedBody = rb2d;
    }
    void updatePUSH()
    {
        if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift) 
            || !pushedObj.GetComponent<PushableController>().grounded)
        {
            sm.ChangeState(enterBASIC, updateBASIC, exitBASIC);
        }
        int dir = (int)move;
        anim.SetFloat("Speed", rb2d.velocity.x * pushMultiplier);
        if (canMove && grounded2 && grounded && move != 0 || canMove && !grounded2 && facingRight && dir == 1
            || canMove && !grounded2 && !facingRight && dir == -1)
            moveNova(dir, speedCoef, false);
        else
            rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
        //Debug.Log(rb2d.velocity.x + " " + Time.time);
        if (rb2d.velocity.y < maxFall)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, maxFall);
        }
    }
    void exitPUSH()
    {
        //Debug.Log("Exiting Push: " + Time.time);
        //pushedObj.transform.parent = transform.parent;
        anim.SetFloat("Speed", 1);
        pushedObj.GetComponent<PushableController>().beingPushed = false;
        pushedObj.GetComponent<FixedJoint2D>().enabled = false;
        pushing = false;
        anim.SetBool("Pushing", false);
        speedCoef = 1;
    }






    //Climbing Items//
    private float climbVel; // Used in conjuction with climbspeed
    private int prevDir;
    void enterCLIMBING()
    {
        rb2d.gravityScale = 0f;
        anim.SetBool("Climbing", true);
        GetComponent<AudioSource> ().PlayOneShot (climbingLandingSound, 1.0f);
    }
    void updateCLIMBING()
    {
        climbVel = climbSpeed * vMove;
        if (canClimb)
        {
            anim.SetFloat("vSpeed", rb2d.velocity.y / 3);
            rb2d.velocity = new Vector2(0, climbVel);
            prevDir = (int)climbVel;
        }
        else if (!canClimb && climbVel < 0 && prevDir > 0)
        {
            anim.SetFloat("Speed", rb2d.velocity.y / 3);
            rb2d.velocity = new Vector2(0, climbVel);
        }
        else if (!canClimb && climbVel > 0 && prevDir < 0)
        {
            anim.SetFloat("Speed", rb2d.velocity.y / 3);
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
        if (highGrappleCheck && vMove > 0)
        {
            sm.ChangeState(enterCLIMBINGUP, updateCLIMBINGUP, exitCLIMBINGUP);
        }
    }
    void exitCLIMBING()
    {
        rb2d.gravityScale = gravityScaleSave;
        anim.SetBool("Climbing", false);
    }










    //Grapple Items//
    private float grappleRadius = .02f;// The radius to detect a grabable ledge
    private Transform grappleCheck; // The transform to see if the player is overlapping with a ledge grab
    private Transform grappleCheck2;
    private bool highGrappleCheck; // Bool to see if the player can grab a ledge
    private bool lowGrappleCheck;
    private float xForce_1 = 8f;
    private float xForce_2 = 3f;
    private float yForce = 7.5f;
    private float horizConst = 40;
    private float horizTimer = 0;
    void enterCLIMBINGUP()
    {
        horizTimer = 0;
        rb2d.gravityScale = 0f;
        rb2d.velocity = new Vector2(0, 0);
        if (highGrappleCheck)
            anim.SetBool("ClimbUp", true);
        else if (lowGrappleCheck)
            anim.SetBool("ClimbUp2", true);
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
    }
    void exitCLIMBINGUP()
    {
        //if (move == 0)
        anim.SetBool("ClimbUp", false);
        anim.SetBool("ClimbUp2", false);
        rb2d.velocity = new Vector2(0, 0);
        rb2d.gravityScale = gravityScaleSave;
    }












    //fvgfhjkjmjnhgngnh - Iman, 2017
    //Crouching//
    private Transform regrowthCheck;
    private RaycastHit2D regrowthHit;
    private float regrowthDist = 1f;
    private float timer = 0;
    void enterCROUCH()
    {
        rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
        anim.SetBool("Crouching", true);
        timer = 0;
    }
    void updateCROUCH()
    {
        timer++;
        if (regrowthHit != false)
        {
            if (timer > 80)
            {
                regrowthHit.transform.gameObject.GetComponent<RegrowthScript>().grow = true;
            }
        }
        if (!eHit)
        {
            sm.ChangeState(enterBASIC, updateBASIC, exitBASIC);
        }
    }
    void exitCROUCH()
    {
        anim.SetBool("Crouching", false);
    }













    //Her spike death variables//
    private bool spikeCheck = false; // Whether or not the player is spikeCheck into the obstacle.
    private float fadeOutRate = 0.025f;
    private SpriteRenderer[] spriteRenderers;
    private Color white;
    //private float fadeOutTimer = 1.5f;
    void enterSPIKEDEATH()
    {
        velX = rb2d.velocity.x;
        velY = rb2d.velocity.y;
        darkenTimer = 4f;
        fireOne = 1;
        rb2d.gravityScale = 0;
        novaPS.startColor = white;
        novaPS.Play();
        novaAS.clip = spikeDeath;
        novaAS.volume = 0.4f;
        novaAS.Play();
    }
    void updateSPIKEDEATH()
    {
        if (darkenTimer > 0)
        {
            if (fireOne > 0)
            {
                fireOne -= fadeOutRate;
                anim.speed = Mathf.Lerp(1, 0, 1 - fireOne);
                rb2d.velocity = new Vector2(Mathf.Lerp(velX, 0, 1 - fireOne), Mathf.Lerp(velY, 0, 1 - fireOne));
            }
            darkenTimer -= fadeOutRate;
            foreach (SpriteRenderer sr in spriteRenderers)
            {
                sr.color = new Color(1, 1, 1, fireOne);
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
    void exitSPIKEDEATH()
    {
        rb2d.gravityScale = gravityScaleSave;
        anim.speed = 1;
        novaPS.Stop();
        spikeCheck = false;
    }












    //Fire Death Items//
    private bool fire = false;
    private float darkenRate = 0.025f;
    private float darkenTimer;
    private float velX, velY;
    private float fireOne = 1;
    private ParticleSystem novaPS;
    private Color fireColor;
    void enterFIREDEATH()
    {
        velX = rb2d.velocity.x;
        velY = rb2d.velocity.y;
        darkenTimer = 4f;
        fireOne = 1;
        rb2d.gravityScale = 0;
        novaPS.startColor = fireColor;
        novaPS.Play();
        novaAS.clip = fireDeath;
        novaAS.volume = 0.6f;
        novaAS.Play();
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
            //transform.position = respawnPoint;
            //foreach (SpriteRenderer sr in spriteRenderers)
            //{
            //    sr.color = new Color(1f, 1f, 1f, 1f);
            //}
            //sm.ChangeState(enterBASIC, updateBASIC, exitBASIC);
            StartCoroutine(reloadLevel());
        }
    }
    void exitFIREDEATH()
    {
        fire = false;
        rb2d.gravityScale = gravityScaleSave;
        anim.speed = 1;
        novaPS.Stop();
    }








    //Elevating items
    private bool elevating;
    void enterELEVATING()
    {
        anim.Play("NovaClimb2");
        foreach (SpriteRenderer sr in spriteRenderers)
        {
            sr.color = new Color(1f, 1f, 1f, 0f);
        }
    }
    void updateELEVATING()
    {
        rb2d.velocity = Vector2.zero;
        //anim.Play("NovaRigIdle");
        if (!elevating)
        {
            sm.ChangeState(enterCLIMBINGUP, updateCLIMBINGUP, exitCLIMBINGUP);
        }
    }
    void exitELEVATING()
    {
        anim.Play("NovaRun");
        foreach (SpriteRenderer sr in spriteRenderers)
        {
            sr.color = new Color(1f, 1f, 1f, 1f);
        }
    }
}






   
