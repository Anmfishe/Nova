using UnityEngine;
using UnitySampleAssets.CrossPlatformInput;

namespace UnitySampleAssets._2D
{

    [RequireComponent(typeof (CharacterController))]
    public class Platformer2DUserControl : MonoBehaviour
    {
        private CharacterController character;
        private bool jump;

        private void Awake()
        {
            character = GetComponent<CharacterController>();
        }

        private void Update()
        {
            if(!jump)
            // Read the jump input in Update so button presses aren't missed.
            jump = CrossPlatformInputManager.GetButtonDown("Jump");
           
        }

        private void FixedUpdate()
        {
            // Read the inputs.
            bool crouch = Input.GetKey(KeyCode.LeftControl);
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            float i = CrossPlatformInputManager.GetAxis("Vertical");
            // Pass all parameters to the character control script.
            character.Move(h, i, crouch, jump);
            jump = false;
        }
    }
}