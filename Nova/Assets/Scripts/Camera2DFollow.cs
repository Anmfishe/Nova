using UnityEngine;

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
        public bool posFixed = false;
        //Private members//
        private float offsetZ;
        private Vector3 lastTargetPosition;
        private Vector3 currentVelocity;
        private Vector3 lookAheadPos;

        // Use this for initialization
        private void Start()
        {
            lastTargetPosition = target.position;
            offsetZ = (transform.position - target.position).z;
            transform.parent = null;
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            if (!posFixed)
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
                Vector3 aheadTargetPos;
                // if (target.position.y >= transform.position.y + aboveOffset + moveUpThreshold)
                if (target.position.y >= 8)
                {
                    //aheadTargetPos = target.position + lookAheadPos + Vector3.forward * offsetZ + Vector3.up * aboveOffset + Vector3.right * lookRightOffset;
                    aheadTargetPos = target.position + lookAheadPos + Vector3.forward * offsetZ + Vector3.up * (aboveOffset - target.position.y + (target.position.y - 5)) + Vector3.right * lookRightOffset;

                }
                else
                {
                    aheadTargetPos = target.position + lookAheadPos + Vector3.forward * offsetZ + Vector3.up * (aboveOffset - target.position.y)  + Vector3.right * lookRightOffset;
                }
                
                //But smooth to it, don't jump to it
                Vector3 newPos = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref currentVelocity, damping);

                transform.position = newPos;

                lastTargetPosition = target.position;
            }
            else if (posFixed)
            {
                Vector3 newPos = Vector3.SmoothDamp(transform.position, target.position, ref currentVelocity, damping);
                transform.position = newPos;
            }
        }
    }
}