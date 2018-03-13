using System;
using UnityEngine;

namespace UnityStandardAssets._2D
{
    public class Player : MonoBehaviour
    {

        [SerializeField] private float MaxSpeed = 10f;                    // The fastest the player can travel in the x axis.
        [SerializeField] private float JumpForce = 400f;                  // Amount of force added when the player jumps.
        [Range(0, 1)] [SerializeField] private float CrouchSpeed = .36f;  // Amount of maxSpeed applied to crouching movement. 1 = 100%
        [SerializeField] private bool AirControl = false;                 // Whether or not a player can steer while jumping;
        [SerializeField] private LayerMask WhatIsGround;                  // A mask determining what is ground to the character

        private Transform GroundCheck;    // A position marking where to check if the player is grounded.
        const float GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
        private bool Grounded;            // Whether or not the player is grounded.
        private Transform CeilingCheck;   // A position marking where to check for ceilings
        const float CeilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
        private Animator Anim;            // Reference to the player's animator component.
        private Rigidbody2D myrigidbody2D;
        private bool FacingRight = true;  // For determining which way the player is currently facing.

/*MOVIMIENTO*/ 
        private bool slide; // deslizarse
        private bool jump; // saltar

/*ATAQUES*/
        [SerializeField]
        private Transform knifePosition; //posicion del cuchillo;
        [SerializeField]
        private GameObject knifePrefab; //tirar cuchillo

        private bool attack; // para atacar
        private bool attackthrow; //tirar daga

/*AWAKE*/
        private void Awake()
        {
            // Setting up references.
            GroundCheck = transform.Find("GroundCheck");
            CeilingCheck = transform.Find("CeilingCheck");
            Anim = GetComponent<Animator>();
            myrigidbody2D = GetComponent<Rigidbody2D>();
        }
        
/*UPDATE*/
        void Update()
        {
            HandleInput();
        }
/*FIXEDUPDATE*/
        private void FixedUpdate()
        {
            

            Grounded = false;

           
            // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
            // This can be done using layers instead but Sample Assets will not overwrite your project settings.
            Collider2D[] colliders = Physics2D.OverlapCircleAll(GroundCheck.position, GroundedRadius, WhatIsGround);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                {
                    Grounded = true;
                }                  
            }
            Anim.SetBool("Ground", Grounded);

            // Set the vertical animation
            Anim.SetFloat("vSpeed", myrigidbody2D.velocity.y);
            HandleAttacks();
            ResetValues();
        }

/*MOVIMIENTO*/
        public void Move(float move, bool crouch, bool jump)
        {

            //only control the player if grounded or airControl is turned on
            if ((!Anim.GetBool("slide") && Grounded || AirControl)&& !this.Anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
            {
                // Reduce the speed if crouching by the crouchSpeed multiplier
                move = (crouch ? move* CrouchSpeed : move );

                // The Speed animator parameter is set to the absolute value of the horizontal input.
                Anim.SetFloat("Speed", Mathf.Abs(move));

                // Move the character
                myrigidbody2D.velocity = new Vector2(move*MaxSpeed, myrigidbody2D.velocity.y);

                // If the input is moving the player right and the player is facing left...
                if (move > 0 && !FacingRight)
                {
                    // ... flip the player.
                    Flip();
                }
                    // Otherwise if the input is moving the player left and the player is facing right...
                else if (move < 0 && FacingRight)
                {
                    // ... flip the player.
                    Flip();
                }
            }
            if ((slide) && !this.Anim.GetCurrentAnimatorStateInfo(0).IsTag("Slide"))
            {
                Anim.SetBool("slide", true);
            }
            else if (!Anim.GetCurrentAnimatorStateInfo(0).IsTag("Slide"))
            {
                Anim.SetBool("slide", false);
            }
            // If the player should jump...
            if (Grounded && jump && Anim.GetBool("Ground"))
            {
                // Add a vertical force to the player.
                Grounded = false;
                Anim.SetBool("Ground", false);
                myrigidbody2D.AddForce(new Vector2(0f, JumpForce));
            }

        }

/*ACCION*/
        private void HandleAttacks()
        {
            if (attack && !this.Anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack") && Grounded)
            {
                Anim.SetTrigger("attack");
                myrigidbody2D.velocity = Vector2.zero;
            }
            else if (attack && !this.Anim.GetCurrentAnimatorStateInfo(0).IsName("Jump_Attack") && !Grounded)
            {
                Anim.SetTrigger("attack");
                //myrigidbody2D.velocity = Vector2.zero;
            }
            else if (attackthrow && !this.Anim.GetCurrentAnimatorStateInfo(0).IsTag("Throw") && Grounded)
            {
                Anim.SetTrigger("throw");
                myrigidbody2D.velocity = Vector2.zero;
            }
            else { }
        }

/*CUANDO ACCIONAS UNA TECLA*/
        private void HandleInput()
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                attack = true;
            }
            if (Input.GetKeyDown(KeyCode.G))
            {
                slide = true;
            }
            if (Input.GetKeyDown(KeyCode.H))
            {
                //Anim.SetTrigger("throw");
                attackthrow = true;
            }
        }        

/*ROTAR*/
        private void Flip()
        {
            // Switch the way the player is labelled as facing.
            FacingRight = !FacingRight;

            // Multiply the player's x local scale by -1.
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }

/*TIRAR CUCHILLO*/
        public void ThrowKnive(int value)
        {
            if(!Grounded && value == 1 || Grounded && value == 0)
            {
                if (FacingRight)
                {
                    GameObject aux = (GameObject)Instantiate(knifePrefab, knifePosition.position, Quaternion.Euler(new Vector3(0, 0, 0)));
                    aux.GetComponent<Knife>().Initialize(Vector2.right);
                }
                else
                {
                    GameObject aux = (GameObject)Instantiate(knifePrefab, knifePosition.position, Quaternion.Euler(new Vector3(0, 0, 180)));
                    aux.GetComponent<Knife>().Initialize(Vector2.left);
                }
            }
      
        }

/*RESETEAR VALORES*/
        private void ResetValues()
        {
            attack = false;
            slide = false;
            attackthrow = false;
        }

    }
}
