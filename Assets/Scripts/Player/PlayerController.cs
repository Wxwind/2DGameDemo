using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PlayerController : MonoBehaviour
{
    [Title("基础参数")]
    public float LRMoveSpeed;
    public float LRAirMoveSpeed;
    public float jumpSpeed;
    public float airJumpSpeed;
    public float wallSlideSpeed;
    [Tooltip("滑墙状态保持时间")]public float wallSlideHoldTime;
    public float groundHoldJumpTime;
    public float jumpNoReponseTime;


    [Space]

    [Title("能力控制")]
    public bool canJump = true;
    public bool canMove = true;

    [Tooltip("空中的多段跳")]
    public bool canAirJump = true;
    public int maxAirJumpCount = 0;
    public bool canWallSliding = true;

    [Space]  

    [Title("当前状态")]
    [LabelText("当前速度"), ReadOnly] public Vector2 currentSpeed;
    [ReadOnly] public bool isJumping;
    [ReadOnly] public bool isIdling;
    [ReadOnly] public bool isDashing;
    [ReadOnly] public bool isWalking;
    [ReadOnly] public bool isWallSliding;
    [ReadOnly] public int nowairJumpCount;
    [ReadOnly] public int xInput = 0;
    [ReadOnly] public int faceDir = 1;

    private int wallSlidefaceDir;

    private CollDetection collDetection;
    private PlayerAnim playerAnim;
    private Rigidbody2D rb;
    private Timer wallSlideHoldTimer;
    /// <summary>
    /// 0:待机状态
    /// 1:地面跳跃前几帧不受加成状态
    /// 2:地面跳跃后持续按住跳跃键获得跳跃加成
    /// </summary>
    /// 
    private int groundHoldJumpState;
    private Timer groundHoldJumpTimer;
    private Timer jumpNoReponseTimer;
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        collDetection = GetComponent<CollDetection>();
        playerAnim = GetComponent<PlayerAnim>();
        nowairJumpCount = canAirJump ? maxAirJumpCount : 0;
        wallSlideHoldTimer = new Timer(wallSlideHoldTime, () => isWallSliding = false);
        groundHoldJumpTimer = new Timer(groundHoldJumpTime, () => groundHoldJumpState = 0);
        jumpNoReponseTimer = new Timer(jumpNoReponseTime, () => groundHoldJumpState = 2);
        groundHoldJumpState = 0;
    }

    private void Update()
    {
        wallSlideHoldTimer.Tick(Time.deltaTime);

        if (Input.GetKey(InputManager.instance.leftKey))
        {
            xInput = -1; faceDir = -1;
        }
        else if (Input.GetKey(InputManager.instance.rightKey))
        {
            xInput = 1; faceDir = 1;
        }
        else xInput = 0;
        WallSlide();
        Jump();
        LRMove();    
        Abilitity();
    }

    private void LateUpdate()
    {
        currentSpeed = rb.velocity;
    }

    private void LRMove()
    {
        if (!canMove || isWallSliding) return;//如果左右移动能力被关闭或者处于“墙上滑落”的状态,则禁止左右移动

        if (collDetection.OnGround)//地面移动
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
        else//空中左右移动,且不处于“墙上滑落”状态
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
            if (isWallSliding)//滑墙跳
            {
                rb.velocity =  new Vector2(wallSlidefaceDir*jumpSpeed, jumpSpeed);//待修改
                StartCoroutine(StopMove(0.1f));
                AudioManager.instance.PlaySFXAudio("Jump");
                isWallSliding = false;
            }
            else//正常跳跃
            {
                if (collDetection.OnGround)//地面跳跃
                {
                    rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
                    AudioManager.instance.PlaySFXAudio("Jump");
                    groundHoldJumpState = 1;
                    groundHoldJumpTimer.ResetAndRun();
                    jumpNoReponseTimer.ResetAndRun();
                }
                else if (nowairJumpCount > 0)//空中跳跃
                {
                    nowairJumpCount--;
                    rb.velocity = new Vector2(rb.velocity.x, airJumpSpeed);
                    AudioManager.instance.PlaySFXAudio("Jump");
                }
            }

        }

        switch (groundHoldJumpState) 
        {
            case 1:
                if(Input.GetKey(InputManager.instance.jumpKey))
                {
                    jumpNoReponseTimer.Tick(Time.deltaTime);
                }
                else groundHoldJumpState = 0;
                break;

            case 2:
                if (Input.GetKey(InputManager.instance.jumpKey))
                {
                    rb.velocity = new Vector2(rb.velocity.x, Mathf.Lerp(rb.velocity.y,jumpSpeed,1.0f));
                    groundHoldJumpTimer.Tick(Time.deltaTime);
                }
                else groundHoldJumpState = 0;
                break;

            default:
                break;
        }


        //地面大跳加成
        
    }
    private void WallSlide()
    {
        if (!canWallSliding || collDetection.OnGround)
        {
            isWallSliding = false;
            return;
        }
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
        else {
            wallSlideHoldTimer.ResetAndRun();
        };
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
