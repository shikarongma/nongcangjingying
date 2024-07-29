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
    //是否在使用工具
    private bool useTool;
    //是否可用移动系统
    private bool inputDisable;

    //动画使用工具
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

    //更改玩家位置
    private void OnMoveToPosition(Vector3 nextPosition)
    {
        this.gameObject.transform.position = nextPosition;
    }

    private void OnMouseClickedEvent(Vector3 mouseWorldPos, ItemDetails itemDetails)
    {
        //TODO 更改Player动画
           //one 工具
        if(itemDetails.itemType!=ItemType.Furniture&& itemDetails.itemType != ItemType.Commodity&& itemDetails.itemType != ItemType.Seed)
        {
            mouseX = mouseWorldPos.x - transform.position.x;
            mouseY = mouseWorldPos.y - (transform.position.y + 0.85f);

            if (Mathf.Abs(mouseX) > Mathf.Abs(mouseY))//判断人物是该上下翻转还是左右翻转
                mouseY = 0;
            else
                mouseX = 0;

            //执行动画
            StartCoroutine(UseToolRoutine(mouseWorldPos, itemDetails));
        }
        else
        {
            //更新完动画后，在执行后续物品的动作
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
