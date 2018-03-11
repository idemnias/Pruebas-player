using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets._2D
{
    [RequireComponent(typeof (Player))]
    public class Platformer2DUserControl : MonoBehaviour
    {
        private Player myCharacter;
        private bool myJump;


        private void Awake()
        {
            myCharacter = GetComponent<Player>();
        }


        private void Update()
        {
            if (!myJump)
            {
                // Read the jump input in Update so button presses aren't missed.
                myJump = CrossPlatformInputManager.GetButtonDown("Jump");
            }
        }


        private void FixedUpdate()
        {
            // Read the inputs.
            bool crouch = Input.GetKey(KeyCode.F);
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            // Pass all parameters to the character control script.
            myCharacter.Move(h, crouch, myJump);
            myJump = false;
        }
    }
}
