using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Move : MonoBehaviour
{
    // Start is called before the first frame update
    public float maxspeed;
    public float jumpPower;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Update() {//단발적 입력 > 업데이트
        
        //jump
        if (Input.GetButtonDown("Jump") && !anim.GetBool("isJumping"))
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetBool("isJumping", true);
        }

        //Stop Speed(미끄러지지 않게 조정함)
        if (Input.GetButtonUp("Horizontal")) {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f , rigid.velocity.y);
        }

        //Direction Sprite
        if (Input.GetButtonDown("Horizontal")){
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;
        }
        //walking motion
        if (Mathf.Abs( rigid.velocity.x )< 0.3)
            anim.SetBool("isWalking", false);
        else
            anim.SetBool("isWalking", true);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Move by key controler
        float h = Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right*h*2, ForceMode2D.Impulse);

        //max speed
        if (rigid.velocity.x > maxspeed)
            rigid.velocity = new Vector2(maxspeed, rigid.velocity.y);  
        else if (rigid.velocity.x < maxspeed*(-1)) //음수라
            rigid.velocity = new Vector2(maxspeed*(-1), rigid.velocity.y);

        //Landing platform
        if (rigid.velocity.y < 0) {
           Debug.DrawRay(rigid.position, Vector3.down, new Color(0,1,0));
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down , 1 , LayerMask.GetMask("Platform"));
            if (rayHit.collider != null) {
                if (rayHit.distance < 0.5f)
                { //바닥에 닿았다
                    anim.SetBool("isJumping", false);
                }
            }
        }
 
    }
}
