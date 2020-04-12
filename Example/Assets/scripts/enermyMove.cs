using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enermyMove : MonoBehaviour
{
    Rigidbody2D rigid;
    public int nextMove;
    Animator anim;
    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Invoke("Think", 5);//일정시간 후에 함수 실행
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);

        //platform check
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove*0.1f , rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));
        if (rayHit.collider == null)
        {
            Turn();
        }

    }

    void Think() {
        //다음 동작 지정
        nextMove = Random.Range(-1 , 2);

        //애니메이션
        anim.SetInteger("WalkSpeed", nextMove);
        //문워크 방지
        if(nextMove != 0)
         spriteRenderer.flipX = nextMove == 1;

        float nextThinkTime = Random.Range(2f, 5f);
        Invoke("Think", nextThinkTime);

    }

    void Turn() {
            nextMove = nextMove * -1;//절벽 앞에서 방향 바꿈
            CancelInvoke();
            Invoke("Think", 2);
            spriteRenderer.flipX = nextMove == 1; 

            
    }
}
