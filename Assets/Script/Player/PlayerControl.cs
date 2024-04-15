using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Windows;

public class PlayerControl : MonoBehaviour
{
    private Rigidbody2D rb;

    //移动速度
    public float speed;
    //x方向的移动
    private float inputX;
    //y方向的移动
    private float inputY;

    //移动向量
    private Vector2 movementInput;

    //获取Animator
    private Animator[] animators;
    //是否在走或者是跑步
    private bool isMoving;
    //判断此时是否在更换场景
    private bool isChange;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animators = GetComponentsInChildren<Animator>();
    }


    void Update()
    {
        if (!isChange)
            PlayerInput();
        else
            isMoving = false;
        SwitchAnimation();
    }

    private void OnEnable()
    {
        EventHandler.MoveToPosition += OnMoveToPosition;
        EventHandler.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
        EventHandler.AfterSceneUnloadEvent += OnAfterSceneUnloadEvent;
    }

    private void OnDisable()
    {
        EventHandler.MoveToPosition -= OnMoveToPosition;
        EventHandler.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadEvent;
        EventHandler.AfterSceneUnloadEvent -= OnAfterSceneUnloadEvent;
    }

    private void OnBeforeSceneUnloadEvent()
    {
        isChange = true;
    }

    private void OnAfterSceneUnloadEvent()
    {
        isChange = false;
    }

    //更改玩家位置
    private void OnMoveToPosition(Vector3 nextPosition)
    {
        this.gameObject.transform.position = nextPosition;
    }

    private void FixedUpdate()
    {
        if (!isChange)
            Movement();
    }

    private void PlayerInput()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");

        

        if (inputX != 0 && inputY != 0)
        {
            inputX *= 0.6f;
            inputY *= 0.6f;
        }

        //长按shift键切换为walk动画
        if (Input.GetKey(KeyCode.LeftShift))
        {
            inputX *= 0.5f;
            inputY *= 0.5f;
        }

        movementInput = new Vector2(inputX, inputY);
        isMoving = movementInput != Vector2.zero;
    }

    //移动
    private void Movement()
    {
        rb.MovePosition(rb.position + movementInput * speed * Time.deltaTime);
    }

    //改变动画
    private void SwitchAnimation()
    {
        foreach(var animator in animators)
        {
            animator.SetBool("isMoving", isMoving);
            if (isMoving)
            {
                animator.SetFloat("InputX", inputX);
                animator.SetFloat("InputY", inputY);
            }

        }
    }
}
