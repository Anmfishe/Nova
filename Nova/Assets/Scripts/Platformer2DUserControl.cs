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
            // Read the inputs.
            jump = Input.GetButtonDown("Jump");
            bool crouch = Input.GetKey(KeyCode.E);
            float h = Input.GetAxisRaw("Horizontal");
            float i = Input.GetAxisRaw("Vertical");
            // Pass all parameters to the character control script.
            character.Move(h, i, crouch, jump);
            jump = false;
        }
    }
}