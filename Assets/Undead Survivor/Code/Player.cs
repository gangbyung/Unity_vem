using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    //움직임을 입력받을 값
    public Vector2 inputVec;
    //속도 정의
    public float speed;
    public Scanner scanner;
    public Hand[] hands;
    public RuntimeAnimatorController[] animCon;

    //지정
    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;
    void Awake()
    {
        //초기화
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
        hands = GetComponentsInChildren<Hand>(true);
    }

    void OnEnable()
    {
        speed *= Character.Speed;
        anim.runtimeAnimatorController = animCon[GameManager.Instance.playerId];
    }
    void Update()
    {
        if (!GameManager.Instance.isLive)
            return;

        //inputVec.x = Input.GetAxisRaw("Horizontal");
        //inputVec.y = Input.GetAxisRaw("Vertical");
    }
    void OnMove(InputValue Value)
    {
        inputVec = Value.Get<Vector2>();
    }
    
    private void FixedUpdate()
    {
        if (!GameManager.Instance.isLive)
            return;

        //normalized는 피타고라스 대각선은 루트2, fixedDeltaTime는 픽시드업데이트 1번만큼 소모된시간(deltatime은 업데이트에서 사용함)
        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;
        //위치이동
        rigid.MovePosition(rigid.position + nextVec);
    }

    private void LateUpdate()
    {
        if (!GameManager.Instance.isLive)
            return;

        anim.SetFloat("Speed", inputVec.magnitude);

        if (inputVec.x != 0)
        {
            spriter.flipX = inputVec.x < 0;
        }
    }
    void OnCollisionStay2D(Collision2D collision)
    {
        if (!GameManager.Instance.isLive)
            return;


        GameManager.Instance.health -= Time.deltaTime * 10;

        if (GameManager.Instance.health < 0)
        {
            for (int index = 2; index < transform.childCount; index++)
            {
                transform.GetChild(index).gameObject.SetActive(false);
            }

            anim.SetTrigger("Dead");
            GameManager.Instance.GameOver();
        }
    }
}   
