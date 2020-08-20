using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 direction;
    public float forwardSpeed;
    public float maxSpeed;
    private int desiredLane = 1; //0:left 1:middle 2:right
    public float laneDistance = (float)2.35; //the distance between 2 lanes
    public float jumpForce;
    public float Gravity=-20;

    public bool isGrounded;
    //public bool isSlide;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public bool slide = false;
    public bool jump = false;
    public bool left = false;
    public bool right = false;
    public Animator anim;

    float slideTimer = 0f;
    public float maxSlideTime = 3f;

   // [SerializeField]
    //GameObject healthCollider;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (!PlayerManager.isGameStarted)
            return;
        
        if(forwardSpeed<maxSpeed)
        {
            forwardSpeed += 0.1f * Time.deltaTime;
        }
        direction.z = forwardSpeed;
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.15f, groundLayer);
        if (controller.isGrounded)
        {   
            if (SwipeManager.swipeUp)
            {
                //Jump();
                jump = true;
            }
            else
            {
                jump = false;
            }
            if (jump == true)
            {
                anim.SetBool("isJump", jump);
                direction.z = forwardSpeed;
                direction.y = jumpForce;
            }
            else if (jump == false)
            {
                anim.SetBool("isJump", jump);
            }
            if (SwipeManager.swipeDown && !slide)
            {
                slideTimer = 0f;
                anim.SetBool("isSlide", true);
                gameObject.GetComponent<CapsuleCollider>().enabled = false;
                controller.center = new Vector3(0, 0.66f, 0);
                controller.height = 1;
                //gameObject.GetComponent<CharacterController>().enabled = false;
                slide = true;
            }
            
            if(slide)
            {
                slideTimer += Time.deltaTime;
                if(slideTimer > maxSlideTime)
                {
                    slide = false;
                    anim.SetBool("isSlide", false);
                    gameObject.GetComponent<CapsuleCollider>().enabled = true;
                    controller.center = new Vector3(0, 0.83f, 0);
                    controller.height = (float)1.8;
                    //gameObject.GetComponent<CharacterController>().enabled = true;
                }
            }
            if (SwipeManager.swipeLeft)
            {
                //Jump();
                left = true;
            }
            else
            {
                left = false;
            }
            if (left == true)
            {
                anim.SetBool("isLeft", left);
                direction.z = forwardSpeed;
                
            }
            else if (left == false)
            {
                anim.SetBool("isLeft", left);
            }
            if (SwipeManager.swipeRight)
            {
                right = true;
            }
            else
            {
                right = false;
            }
            if (right == true)
            {
                anim.SetBool("isRight", right);
                direction.z = forwardSpeed;

            }
            else if (right == false)
            {
                anim.SetBool("isRight", right);
            }
        }
        else
        {
            direction.y += Gravity * Time.deltaTime;
        }
        if(SwipeManager.swipeRight)
        {
            desiredLane++;
            if (desiredLane == 3)
            {
                desiredLane = 2;
            }
        }
        if (SwipeManager.swipeLeft)
        {
            desiredLane--;
            if (desiredLane == -1)
            {
                desiredLane = 0;
            }
        }

        Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;
        if(desiredLane == 0)
        {
            targetPosition += Vector3.left * laneDistance;
        }else if(desiredLane==2)
        {
            targetPosition += Vector3.right * laneDistance;
        }

        if (transform.position != targetPosition)
        {
            Vector3 diff = targetPosition - transform.position;
            Vector3 moveDir = diff.normalized * 25 * Time.deltaTime;
            if (moveDir.sqrMagnitude < diff.sqrMagnitude)
                controller.Move(moveDir);
            else
                controller.Move(diff);
        }  
        
        controller.Move(direction * Time.deltaTime);
    }
    
    private void Jump()
    {
        direction.y = jumpForce;
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.transform.tag=="Obstacle")
        {
            PlayerManager.gameOver = true;
            FindObjectOfType<AudioManager>().PlaySound("GameOver");
        }
    }
}
