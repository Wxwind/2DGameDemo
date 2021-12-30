using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using Sirenix.OdinInspector;
using Unity.Collections;
using UnityEngine.PlayerLoop;

// ReSharper disable All

public class PlayerController : MonoBehaviour
{
    [Title("基础参数")] public float LRMoveSpeed;
    public float LRAirMoveSpeed;
    public float jumpSpeed;
    public float airJumpSpeed;
    public float wallSlideSpeed;
    [Tooltip("滑墙状态保持时间")] public float wallSlideHoldTime;
    public float groundHoldJumpTime;
    public float jumpNoReponseTime;


    [Space] [Title("射击")] public GameObject bubblePre;
    public float bubbleTpThresholdTime;
    public float bubbleLifeTime;
    public float bubbleSpeed;


    /// <summary>
    /// 0:发射就绪
    /// 1:已发射泡泡但不能传送
    /// 2:已发射泡泡且可以传送
    /// </summary>
    private int bubbleState = 0;

    private Bubble bubble;
    private Timer bubbleTpThreadTimer;

    [Title("能力控制")] public bool canJump = true;
    public bool canMove = true;
    public bool canShot = true;

    [Tooltip("空中的多段跳")] public bool canAirJump = true;
    public int maxAirJumpCount = 0;
    public bool canWallSliding = true;

    [Space] [Title("当前状态")] [LabelText("当前速度"), Sirenix.OdinInspector.ReadOnly]
    public Vector2 currentSpeed;

    [Sirenix.OdinInspector.ReadOnly] public bool isJumping;
    [Sirenix.OdinInspector.ReadOnly] public bool isIdling;
    [Sirenix.OdinInspector.ReadOnly] public bool isWalking;
    [Sirenix.OdinInspector.ReadOnly] public bool isWallSliding;
    [Sirenix.OdinInspector.ReadOnly] public int nowairJumpCount;
    [Sirenix.OdinInspector.ReadOnly] public int xInput = 0;
    [Sirenix.OdinInspector.ReadOnly] public int faceDir = 1;

    private int wallSlidefaceDir;

    private CollDetection collDetection;
    private PlayerAnim playerAnim;
    private Rigidbody2D rb;

    /// <summary>
    /// 0:待机状态
    /// 1:地面跳跃前几帧不受加成状态
    /// 2:地面跳跃后持续按住跳跃键获得跳跃加成
    /// </summary>
    private int groundHoldJumpState = 0;

    private Timer groundHoldJumpTimer;
    private Timer jumpNoReponseTimer;
    private Timer wallSlideHoldTimer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        collDetection = GetComponent<CollDetection>();
        playerAnim = GetComponent<PlayerAnim>();
        nowairJumpCount = canAirJump ? maxAirJumpCount : 0;
        wallSlideHoldTimer = new Timer(wallSlideHoldTime, () => isWallSliding = false);
        groundHoldJumpTimer = new Timer(groundHoldJumpTime, () => groundHoldJumpState = 0);
        jumpNoReponseTimer = new Timer(jumpNoReponseTime, () => groundHoldJumpState = 2);
        bubbleTpThreadTimer = new Timer(bubbleTpThresholdTime, () => bubbleState = 2);
    }

    private void Update()
    {
        wallSlideHoldTimer.Tick(Time.deltaTime);
        bubbleTpThreadTimer.Tick(Time.deltaTime);
        xInput = 0;
        if (Input.GetKey(InputManager.instance.leftKey))
        {
            xInput -= 1;
        }
        if (Input.GetKey(InputManager.instance.rightKey))
        {
            xInput += 1;
        }
        if (xInput!=0)
        {
            if (Input.GetKey(InputManager.instance.leftKey))
            {
                faceDir = -1;
            }
            if (Input.GetKey(InputManager.instance.rightKey))
            {
                faceDir = 1;
            
            }
        }

        WallSlide();
        Jump();
        LRMove();
        Abilitity();
        Shot();
    }

    private void Shot()
    {
        if (!canShot)
        {
            return;
        }

        if (Input.GetKeyDown(InputManager.instance.attackKey))
        {
            switch (bubbleState)
            {
                //发射就绪
                case 0:
                    playerAnim.SetTrigger("Bubble", true);
                    bubble = Instantiate(bubblePre, transform.position, transform.localRotation).GetComponent<Bubble>();
                    int x = 0;
                    int y = 0;
                    if (Input.GetKey(InputManager.instance.upKey)) y += 1;
                    if (Input.GetKey(InputManager.instance.downKey)) y -= 1;
                    if (y == 0)
                    {
                        x = faceDir;
                    }
                    else
                    {
                        if (Input.GetKey(InputManager.instance.leftKey)) x -= 1;
                        if (Input.GetKey(InputManager.instance.rightKey)) x += 1;
                    }
                    var bubblerb = bubble.GetComponent<Rigidbody2D>();
                    var dir = new Vector2(x, y);
                    bubblerb.velocity = bubbleSpeed * dir.normalized;
                    bubble.Init(bubbleLifeTime, dir,bubbleSpeed,() => bubbleState = 0);
                    bubblerb.gravityScale = 0;
                    bubbleState = 1;
                    bubbleTpThreadTimer.ReRun();
                    break;
                //已发射还不能传送
                case 1:
                    break;
                //已发射且能传送
                case 2:
                    if (TryTp())
                    {
                        bubbleState = 0; 
                        Destroy(bubble.gameObject);
                    }
                    break;
            }
        }
    }

    private bool TryTp()
    {
        var plPos = transform.position;
        var bbPos= bubble.transform.position;
      
        if (bubble.isBloacked())
        {
            Debug.Log("tp failed:sth. block");
            return false;
        }
        else
        {
            Debug.Log("tp succeed");
            transform.position = bbPos;
            return true;
        }
    }
    

    private void LateUpdate()
    {
        currentSpeed = rb.velocity;
    }

    private void LRMove()
    {
        if (!canMove || isWallSliding) return; //如果左右移动能力被关闭或者处于“墙上滑落”的状态,则禁止左右移动

        if (collDetection.OnGround) //地面移动
        {
            if (xInput == 0)
            {
                isIdling = true;
                isWalking = false;
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
            else
            {
                isWalking = true;
                isIdling = false;
                rb.velocity = new Vector2(xInput * LRMoveSpeed, rb.velocity.y);
                playerAnim.Flip(faceDir);
            }
        }
        else //空中左右移动,且不处于“墙上滑落”状态
        {
            rb.velocity = new Vector2(xInput * LRAirMoveSpeed, rb.velocity.y);
            playerAnim.Flip(faceDir);
        }
    }

    private void Jump()
    {
        if (!canJump) return;
        if (collDetection.OnGround)
        {
            nowairJumpCount = maxAirJumpCount;
        }

        if (Input.GetKeyDown(InputManager.instance.jumpKey))
        {
            playerAnim.SetTrigger("Jump", true);
            if (isWallSliding) //滑墙跳
            {
                rb.velocity = new Vector2(wallSlidefaceDir * jumpSpeed, jumpSpeed); //待修改
                StartCoroutine(StopMove(0.1f));
                //AudioManager.instance.PlaySFXAudio("Jump");
                isWallSliding = false;
            }
            else //正常跳跃
            {
                if (collDetection.OnGround) //地面跳跃
                {
                    rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
                    //AudioManager.instance.PlaySFXAudio("Jump");
                    groundHoldJumpState = 1;
                    groundHoldJumpTimer.ReRun();
                    jumpNoReponseTimer.ReRun();
                }
                else if (nowairJumpCount > 0) //空中跳跃
                {
                    nowairJumpCount--;
                    rb.velocity = new Vector2(rb.velocity.x, airJumpSpeed);
                    //AudioManager.instance.PlaySFXAudio("Jump");
                }
            }
        }

        //小跳和大跳
        switch (groundHoldJumpState)
        {
            //地面跳跃前几帧不受加成状态
            case 1:
                if (Input.GetKey(InputManager.instance.jumpKey))
                {
                    jumpNoReponseTimer.Tick(Time.deltaTime);
                }
                else groundHoldJumpState = 0;

                break;
            //地面跳跃后持续按住跳跃键获得跳跃加成
            case 2:
                if (Input.GetKey(InputManager.instance.jumpKey))
                {
                    rb.velocity = new Vector2(rb.velocity.x, Mathf.Lerp(rb.velocity.y, jumpSpeed, 1.0f));
                    groundHoldJumpTimer.Tick(Time.deltaTime);
                }
                else groundHoldJumpState = 0;

                break;

            default:
                break;
        }
    }

    private void WallSlide()
    {
        if (!canWallSliding || collDetection.OnGround)
        {
            isWallSliding = false;
            return;
        }

        //在墙上且按下相反的方向键，触发wallslide
        if ((collDetection.OnLeftWall && Input.GetKey(InputManager.instance.leftKey)) ||
            (collDetection.OnRightWall && Input.GetKey(InputManager.instance.rightKey)))
        {
            isWallSliding = true;
            if (collDetection.OnLeftWall)
            {
                wallSlidefaceDir = 1;
            }
            else wallSlidefaceDir = -1;

            playerAnim.Flip(wallSlidefaceDir);
            rb.velocity = new Vector2(rb.velocity.x, wallSlideSpeed);
        }
        //松开后如果贴着墙则仍保持一定时间的wallslide状态
        else
        {
            wallSlideHoldTimer.ReRun();
        }

        ;
        if (!collDetection.OnWall)
        {
            isWallSliding = false;
            wallSlideHoldTimer.Stop();
        }
    }

    private void Abilitity()
    {
        if (Input.GetKeyDown(InputManager.instance.abilityKey))
        {
            SwitchAbilityManager.instance.SwitchToAnother();
        }
    }

    IEnumerator StopMove(float time)
    {
        canMove = false;
        yield return new WaitForSeconds(time);
        canMove = true;
    }
}