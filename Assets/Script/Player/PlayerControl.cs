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
    //�Ƿ���ʹ�ù���
    private bool useTool;
    //�Ƿ�����ƶ�ϵͳ
    private bool inputDisable;

    //����ʹ�ù���
    private float mouseX;
    private float mouseY;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animators = GetComponentsInChildren<Animator>();
    }


    void Update()
    {
        if (!inputDisable)
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
        EventHandler.MouseClickedEvent += OnMouseClickedEvent;
    }

    private void OnDisable()
    {
        EventHandler.MoveToPosition -= OnMoveToPosition;
        EventHandler.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadEvent;
        EventHandler.AfterSceneUnloadEvent -= OnAfterSceneUnloadEvent;
        EventHandler.MouseClickedEvent -= OnMouseClickedEvent;
    }


    private void OnBeforeSceneUnloadEvent()
    {
        inputDisable = true;
    }

    private void OnAfterSceneUnloadEvent()
    {
        inputDisable = false;
    }

    //�������λ��
    private void OnMoveToPosition(Vector3 nextPosition)
    {
        this.gameObject.transform.position = nextPosition;
    }

    private void OnMouseClickedEvent(Vector3 mouseWorldPos, ItemDetails itemDetails)
    {
        //TODO ����Player����
           //one ����
        if(itemDetails.itemType!=ItemType.Furniture&& itemDetails.itemType != ItemType.Commodity&& itemDetails.itemType != ItemType.Seed)
        {
            mouseX = mouseWorldPos.x - transform.position.x;
            mouseY = mouseWorldPos.y - (transform.position.y + 0.85f);

            if (Mathf.Abs(mouseX) > Mathf.Abs(mouseY))//�ж������Ǹ����·�ת�������ҷ�ת
                mouseY = 0;
            else
                mouseX = 0;

            //ִ�ж���
            StartCoroutine(UseToolRoutine(mouseWorldPos, itemDetails));
        }
        else
        {
            //�����궯������ִ�к�����Ʒ�Ķ���
            EventHandler.CallExecuteActionAfterAnimation(mouseWorldPos, itemDetails);
        }
    }

    private IEnumerator UseToolRoutine(Vector3 mouseWorldPos, ItemDetails itemDetails)
    {
        useTool = true;
        inputDisable = true;
        yield return null;
        foreach(var anim in animators)
        {
            anim.SetTrigger("useTool");
            anim.SetFloat("InputX", mouseX);
            anim.SetFloat("InputY", mouseY);
        }
        yield return new WaitForSeconds(0.45f);
        EventHandler.CallExecuteActionAfterAnimation(mouseWorldPos, itemDetails);
        yield return new WaitForSeconds(0.45f);

        useTool = false;
        inputDisable = false;
    }

    private void FixedUpdate()
    {
        if (!inputDisable)
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
            animator.SetFloat("mouseX", mouseX);
            animator.SetFloat("mouseY", mouseY);
            
            if (isMoving)
            {
                animator.SetFloat("InputX", inputX);
                animator.SetFloat("InputY", inputY);
            }

        }
    }
}
