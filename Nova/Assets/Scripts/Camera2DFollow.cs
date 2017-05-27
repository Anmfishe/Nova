using UnityEngine;
using System.Collections;

namespace UnitySampleAssets._2D
{

    public class Camera2DFollow : MonoBehaviour
    {
        //Public members//
        public Transform target;
        public float damping = 1;
        public float moveUpThreshold = 1;
        public float lookAheadFactor = 3;
        public float lookAheadReturnSpeed = 0.5f;
        public float lookAheadMoveThreshold = 0.1f;
        public float aboveOffset = 2;
        public float lookRightOffset = 4;
        public bool showTitle = true;
        public bool startBlack = false;
        public bool startWhite = false;
        [HideInInspector]
        public bool posFixed = false;
        [HideInInspector]
        public bool freeze;
        [HideInInspector]
        public bool starting = false;
        [HideInInspector]
        public bool stopMoving = false;
        //Private members//
        private float offsetZ;
        private float novaHeightFollowFactor = 6;
        private float novaHeightFollowSave;
        private Vector3 lastTargetPosition;
        private Vector3 currentVelocity;
        private Vector3 lookAheadPos;
        private Vector3 newPos;
        private SpriteRenderer whiteScreen;
        [HideInInspector]
        public float aboveNovaConst = -4f;
        private bool opening = false;
        private bool fadeOut;
        private bool fadeIn;
        private bool first = true;
        private int r = 0, b = 0, g = 0;
        
        
        public float fadeRate = 0.0001f;
        Camera cam;
        private SpriteRenderer title;

        // Use this for initialization
        private void Start()
        {
            //moveCameraHeight(target.position.y - aboveNovaConst);
            lastTargetPosition = target.position;
            offsetZ = (transform.position - target.position).z;
            transform.parent = null;
            //novaHeightFollowFactor = target.position.y - aboveNovaConst;
            novaHeightFollowSave = novaHeightFollowFactor;
            moveCameraHeight(target.position.y - aboveNovaConst);
            cam = GetComponent<Camera>();
            whiteScreen = transform.GetChild(1).GetComponent<SpriteRenderer>();
            if (startBlack)
            {
                whiteScreen.color = new Color(0, 0, 0, 1);
            }
            if (startWhite)
            {
                whiteScreen.color = new Color(1, 1, 1, 1);
            }

            title = transform.Find("TitleSprite").GetComponent<SpriteRenderer>();
        }

        // Update is called once per frame
        float x = 0;
        float rate = 0.009f;
        private void FixedUpdate()
        {
            
            if (showTitle && title.color.a < 1)
            {
                title.color = new Color(1, 1, 1, title.color.a + fadeRate);
            }
            else if(!showTitle && title.color.a > 0)
            {
                title.color = new Color(1, 1, 1, title.color.a - fadeRate);
            }
            if (fadeOut)
            {
                if(whiteScreen.color.a < 1)
                {
                    whiteScreen.color = new Color(r, b, g, whiteScreen.color.a + fadeRate);
                }
                else
                {
                    fadeOut = false;
                }
            }
            else if(fadeIn)
            {
               
                if (whiteScreen.color.a > 0)
                {
                    whiteScreen.color = new Color(r, b, g, whiteScreen.color.a - fadeRate);
                }
                else
                {
                    
                    fadeIn = false;
                }
            }
            if(opening)
            {
                if(starting)
                {
                    damping = 0.9f;
                    x += rate;
                    cam.orthographicSize = Mathf.Lerp(5, 10, x);
                    
                    Vector3 aheadTargetPos = target.position + lookAheadPos + Vector3.forward * offsetZ + Vector3.up * (aboveOffset - target.position.y) + Vector3.right * lookRightOffset;
                    newPos = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref currentVelocity, damping);
                    transform.position = newPos;
                    if (x >= 1)
                    {
                        damping = 0.3f;
                        opening = false;
                        
                    }
                }
            }
            else if (!posFixed && !stopMoving)
            {
                // only update lookahead pos if accelerating or changed direction
                float xMoveDelta = (target.position - lastTargetPosition).x;

                bool updateLookAheadTarget = Mathf.Abs(xMoveDelta) > lookAheadMoveThreshold;

                if (updateLookAheadTarget)
                {
                    lookAheadPos = lookAheadFactor * Vector3.right * Mathf.Sign(xMoveDelta);
                }
                else
                {
                    lookAheadPos = Vector3.MoveTowards(lookAheadPos, Vector3.zero, lookAheadReturnSpeed);
                }
                //Get the ahead of target position
                Vector3 aheadTargetPos = new Vector3();
                // if (target.position.y >= transform.position.y + aboveOffset + moveUpThreshold)
                if (target.position.y >= novaHeightFollowFactor )
                {
                    //damping = 0.3f;
                    //aheadTargetPos = target.position + lookAheadPos + Vector3.forward * offsetZ + Vector3.up * aboveOffset + Vector3.right * lookRightOffset;
                    aheadTargetPos = target.position + lookAheadPos + Vector3.forward * offsetZ + Vector3.up * (aboveOffset - target.position.y + (target.position.y - (novaHeightFollowFactor - 3))) + Vector3.right * lookRightOffset;
                }
                else if(target.position.y  < transform.position.y + aboveNovaConst )
                {
                    //damping = 0.1f;
                    moveCameraHeight(target.position.y - aboveNovaConst);
                    //aheadTargetPos = target.position + lookAheadPos + Vector3.forward * offsetZ + Vector3.up * (aboveOffset - target.position.y + (target.position.y - (novaHeightFollowFactor - 3))) + Vector3.right * lookRightOffset;
                    aheadTargetPos = target.position + lookAheadPos + Vector3.forward * offsetZ + Vector3.up * (aboveOffset - target.position.y) + Vector3.right * lookRightOffset;
                }
                else
                {
                    //damping = 0.3f;
                    aheadTargetPos = target.position + lookAheadPos + Vector3.forward * offsetZ + Vector3.up * (aboveOffset - target.position.y) + Vector3.right * lookRightOffset;
                }

                //But smooth to it, don't jump to it
                newPos = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref currentVelocity, damping);

                transform.position = newPos;
                


            }
            else if (posFixed && !stopMoving)
            {
                Vector3 targetWithZ = target.position;
                targetWithZ.z = -40;
                newPos = Vector3.SmoothDamp(transform.position, targetWithZ, ref currentVelocity, damping);
                transform.position = newPos;
            }
            
        }
        public void moveCameraHeight(float newY)
        {
            aboveOffset = newY;
            novaHeightFollowFactor = newY + novaHeightFollowSave;
        }
        public void resetCameraHeight()
        {
            aboveOffset = 0;
            novaHeightFollowFactor = novaHeightFollowSave;
            
        }
        public void startFadeOut()
        {
            r = 0;
            b = 0;
            g = 0;
            fadeOut = true;
        }
        public void startFadeIn()
        {
            r = 0;
            b = 0;
            g = 0;
            fadeIn = true;
        }
        public void whiteFadeOut()
        {
            r = 1;
            b = 1;
            g = 1;
            fadeOut = true;
        }
        public void whiteFadeIn()
        {
            r = 1;
            b = 1;
            g = 1;
            fadeIn = true;
        }
        IEnumerator stopTitle(float time)
        {
            yield return new WaitForSeconds(time);
            showTitle = false;
        }
        public Vector3 getAheadofTarget()
        {
            Vector3 aheadTargetPos;
            aheadTargetPos = target.position + lookAheadPos + Vector3.forward * offsetZ + Vector3.up * (aboveOffset - target.position.y) + Vector3.right * lookRightOffset;
            return aheadTargetPos;
        }
        public void shiftCamToNova()
        {
            moveCameraHeight(target.position.y - aboveNovaConst);
        }
        
    } 
}