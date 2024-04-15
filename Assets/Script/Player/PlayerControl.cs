using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Windows;

public class PlayerControl : MonoBehaviour
{
    private Rigidbody2D rb;

    //�ƶ��ٶ�
    public float speed;
    //x������ƶ�
    private float inputX;
    //y������ƶ�
    private float inputY;

    //�ƶ�����
    private Vector2 movementInput;

    //��ȡAnimator
    private Animator[] animators;
    //�Ƿ����߻������ܲ�
    private bool isMoving;
    //�жϴ�ʱ�Ƿ��ڸ�������
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

    //�������λ��
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

        //����shift���л�Ϊwalk����
        if (Input.GetKey(KeyCode.LeftShift))
        {
            inputX *= 0.5f;
            inputY *= 0.5f;
        }

        movementInput = new Vector2(inputX, inputY);
        isMoving = movementInput != Vector2.zero;
    }

    //�ƶ�
    private void Movement()
    {
        rb.MovePosition(rb.position + movementInput * speed * Time.deltaTime);
    }

    //�ı䶯��
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
