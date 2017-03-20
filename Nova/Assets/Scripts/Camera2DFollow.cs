using UnityEngine;
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
        [HideInInspector]
        public bool posFixed = false;
        [HideInInspector]
        public bool freeze;
        [HideInInspector]
        public bool starting = false;
        //Private members//
        private float offsetZ;
        private float novaHeightFollowFactor = 7;
        private float novaHeightFollowSave;
        private Vector3 lastTargetPosition;
        private Vector3 currentVelocity;
        private Vector3 lookAheadPos;
        private Vector3 newPos;
        private SpriteRenderer blackScreen;
        private float aboveNovaConst = -4f;
        private bool opening = true;
        private bool fadeOut;
        private bool fadeIn;
        private bool first = true;
        [HideInInspector]
        public bool showTitle = false;
        public float fadeRate = 0.0001f;
        Camera cam;
        private SpriteRenderer title;

        // Use this for initialization
        private void Start()
        {
            lastTargetPosition = target.position;
            offsetZ = (transform.position - target.position).z;
            transform.parent = null;
            novaHeightFollowSave = novaHeightFollowFactor;
            cam = GetComponent<Camera>();
            blackScreen = transform.GetChild(1).GetComponent<SpriteRenderer>();
            blackScreen.color = new Color(0, 0, 0, 1);
            title = transform.FindChild("TitleSprite").GetComponent<SpriteRenderer>();
        }

        // Update is called once per frame
        float x = 0;
        float rate = 0.009f;
        private void FixedUpdate()
        {
            if(showTitle && title.color.a < 1)
            {
                title.color = new Color(1, 1, 1, title.color.a + fadeRate);
            }
            else if(!showTitle && title.color.a > 0)
            {
                title.color = new Color(1, 1, 1, title.color.a - fadeRate);
            }
            if (fadeOut)
            {
                if(blackScreen.color.a < 1)
                {
                    blackScreen.color = new Color(0, 0, 0, blackScreen.color.a + fadeRate);
                }
                else
                {
                    fadeOut = false;
                }
            }
            else if(fadeIn)
            {
                if (blackScreen.color.a > 0)
                {
                    blackScreen.color = new Color(0, 0, 0, blackScreen.color.a - fadeRate);
                }
                else
                {
                    if(first)
                    {
                        first = false;
                        showTitle = true;
                        StartCoroutine(stopTitle(10));
                    }
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
            else if (!posFixed)
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

                lastTargetPosition = target.position;
            }
            else if (posFixed)
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
            fadeOut = true;
        }
        public void startFadeIn()
        {
            fadeIn = true;
        }
        IEnumerator stopTitle(float time)
        {
            yield return new WaitForSeconds(time);
            showTitle = false;
        }
        
    } 
}