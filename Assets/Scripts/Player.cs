using System;
using UnityEngine;

namespace UnityStandardAssets._2D
{
    public class Player : MonoBehaviour
    {

        [SerializeField] private float m_MaxSpeed = 10f;                    // The fastest the player can travel in the x axis.
        [SerializeField] private float m_JumpForce = 400f;                  // Amount of force added when the player jumps.
        [Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;  // Amount of maxSpeed applied to crouching movement. 1 = 100%
        [SerializeField] private bool m_AirControl = false;                 // Whether or not a player can steer while jumping;
        [SerializeField] private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character

        private Transform GroundCheck;    // A position marking where to check if the player is grounded.
        const float GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
        private bool Grounded;            // Whether or not the player is grounded.
        private Transform CeilingCheck;   // A position marking where to check for ceilings
        const float CeilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
        private Animator Anim;            // Reference to the player's animator component.
        private Rigidbody2D myrigidbody2D;
        private bool FacingRight = true;  // For determining which way the player is currently facing.

        [SerializeField]
        private Transform knifePosition; //posicion del cuchillo;

        [SerializeField]
        private GameObject knifePrefab; //tirar cuchillo

        private bool attack; // para atacar

        private bool attackthrow; //tirar daga

        private bool slide; // deslizarse

        private bool jump; // saltar

        private bool jumpAttack;// atacar saltando

        private void Awake()
        {
            // Setting up references.
            GroundCheck = transform.Find("GroundCheck");
            CeilingCheck = transform.Find("CeilingCheck");
            Anim = GetComponent<Animator>();
            myrigidbody2D = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            HandleInput();
        }

        private void FixedUpdate()
        {
            

            Grounded = false;

           
            // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
            // This can be done using layers instead but Sample Assets will not overwrite your project settings.
            Collider2D[] colliders = Physics2D.OverlapCircleAll(GroundCheck.position, GroundedRadius, m_WhatIsGround);
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


        public void Move(float move, bool crouch, bool jump)
        {

            //only control the player if grounded or airControl is turned on
            if ((!Anim.GetBool("slide") && Grounded || m_AirControl)&& !this.Anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
            {
                // Reduce the speed if crouching by the crouchSpeed multiplier
                move = (crouch ? move*m_CrouchSpeed : move );

                // The Speed animator parameter is set to the absolute value of the horizontal input.
                Anim.SetFloat("Speed", Mathf.Abs(move));

                // Move the character
                myrigidbody2D.velocity = new Vector2(move*m_MaxSpeed, myrigidbody2D.velocity.y);

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
                myrigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
            }
            
        }

        private void HandleAttacks()
        {
            if (attack && !this.Anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack") && Grounded)
            {
                Anim.SetTrigger("attack");
                myrigidbody2D.velocity = Vector2.zero;
            }
            else if (jumpAttack && !this.Anim.GetCurrentAnimatorStateInfo(0).IsName("Jump_Attack") &&!Grounded)
            {
                Anim.SetTrigger("jumpAttack");
                //myrigidbody2D.velocity = Vector2.zero;
            }
            else if (attackthrow && !this.Anim.GetCurrentAnimatorStateInfo(0).IsTag("Throw") && Grounded)
            {
                Anim.SetTrigger("throw");
                myrigidbody2D.velocity = Vector2.zero;
            }
        }

        private void HandleInput()
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                attack = true;
                jumpAttack = true;
            }
            if (Input.GetKeyDown(KeyCode.G))
            {
                slide = true;
            }
            if (Input.GetKeyDown(KeyCode.H))
            {
                Anim.SetTrigger("throw");
            }
        }        

        private void Flip()
        {
            // Switch the way the player is labelled as facing.
            FacingRight = !FacingRight;

            // Multiply the player's x local scale by -1.
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }

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


        private void ResetValues()
        {
            attack = false;
            slide = false;
            jumpAttack = false;
            attackthrow = false;
        }

        //private void OnCollisionEnter2D(Collision2D collision)
        //{
        //    if(collision.transform.tag== "MovingPlataform")
        //    {
        //        collision.collider.transform.SetParent(transform);
        //    }
        //}

        //private void OnCollisionExit2D(Collision2D collision)
        //{
        //    if (collision.transform.tag == "MovingPlataform")
        //    {
        //        transform.parent = null;
        //    }
        //}




    }
}
