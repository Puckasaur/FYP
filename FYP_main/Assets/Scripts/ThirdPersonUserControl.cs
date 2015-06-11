using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof (ThirdPersonCharacter))]
    public class ThirdPersonUserControl : MonoBehaviour
    {
        private ThirdPersonCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
        private Transform m_Cam;                  // A reference to the main camera in the scenes transform
        private Vector3 m_CamForward;             // The current forward direction of the camera
        private Vector3 m_Move;
        private bool m_Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.

		public float speedHat = 2.0f;
		public float jumpHat = 2.0f;


		private bool speedHatOn = false;
		private bool jumpHatOn = false;




        
        private void Start()
        {
            m_Character = GetComponent<ThirdPersonCharacter>();
        }


        private void Update()
        {
            if (!m_Jump)
            {
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");

            }

			if (Input.GetKeyDown (KeyCode.Alpha1)) 
			{
				speedHatOn = true;
				jumpHatOn = false;
			}

			if (Input.GetKeyDown (KeyCode.Alpha2))
			{
				jumpHatOn = true;
				speedHatOn = false;
			}

			speedHatMod ();
			jumpHatMod();
        }

		private void speedHatMod ()
		{
			if (speedHatOn) {
				speedHat = 2.0f;
			} else 
				speedHat = 1.5f;
		}

		private void jumpHatMod ()
		{
			if (jumpHatOn) {
				jumpHat = 2.0f;
			} else 
				jumpHat = 4.0f;
		}

        // Fixed update is called in sync with physics
        private void FixedUpdate()
        {
            // read inputs
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            float v = CrossPlatformInputManager.GetAxis("Vertical");
            bool crouch = Input.GetKey(KeyCode.C);

            // we use world-relative directions in the case of no main camera
            m_Move = (v*Vector3.forward + h*Vector3.right)*0.5f;
           
#if !MOBILE_INPUT
			// walk speed multiplier
	        if (Input.GetKey(KeyCode.LeftShift)) m_Move *= speedHat;
#endif

            // pass all parameters to the character control script
            m_Character.Move(m_Move, crouch, m_Jump);
            m_Jump = false;
        }
    }
}
