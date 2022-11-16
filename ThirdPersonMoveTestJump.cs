using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ThirdPersonMoveTestJump : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;
    public float speed = 10;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    public float jumpHeight;
 
    private float verticalVelocity;
    private float gravityValue = 9.81f;
    private Animator anim;
 
    Vector3 moveDir = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        anim=GetComponent<Animator>();
 
    }
    // Update is called once per frame
    void Update()
    {
        bool groundedPlayer = controller.isGrounded;
 
        // slam into the ground
        if (groundedPlayer && verticalVelocity < 0)
        {
            // hit ground
            verticalVelocity = 0f;

            //Jump
            if ( Input.GetKeyDown(KeyCode.Space))
            {
                verticalVelocity = Mathf.Sqrt(jumpHeight * 2 * gravityValue);
              
            }
        }
        verticalVelocity -= gravityValue * Time.deltaTime;
        
        
        
        
        // For Movement
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            horizontal *= 20 ;
           
        }
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
 
        // For camera 
        if (direction.sqrMagnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
 
            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        }
        else
        {
            moveDir.x = 0f;
            moveDir.z = 0f;
        }
 
        moveDir.y = verticalVelocity;
 
        controller.Move(moveDir.normalized * speed * Time.deltaTime);
        //Idle animation
        if(direction == new Vector3(0,0,0))
        {
            anim.SetFloat("Speed",0);
        }
        //Walking animation
        
        if(direction != new Vector3(0,0,0) && groundedPlayer)
        {
            anim.SetFloat("Speed",.5f);
           


        }
        if( verticalVelocity >= 0f)
        {
            anim.SetFloat("Speed",1f);
            
           


        }
      
      
       
    }



}