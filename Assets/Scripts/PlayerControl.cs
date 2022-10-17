using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float moveSpeed = 1;
    public float jumpSpeed = 1;
    
    private Rigidbody2D rb;
    private Animator anim;
    
    private bool isDead = false;
    private bool isGround = true;
    private bool isJumping = false;
    private bool isClimbing = false;
    
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
        rb.freezeRotation = true;//禁用z轴旋转

        anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        if (isGround && !isDead)
        {
            rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed,rb.velocity.y);
            anim.SetBool("isGround",true);
            anim.SetFloat("moveSpeed",rb.velocity.x);
        }
        
    }

    void Jump()
    {
        if (isGround && !isJumping && !isDead)
        {
            
        }
    }
}
