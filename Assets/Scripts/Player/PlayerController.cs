using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PlayerController : MonoBehaviour
{
    [Header("基础参数")]
    public float LRMoveSpeed;
    public float LRAirMoveSpeed;
    public float jumpSpeed;
    public float doubleJumpSpeed;
    [Space]
    [Header("能力控制")]
    public bool canJump = true;
    public bool canMove = true;
    public bool canDoubleJump = true;
    public bool canWallSliding = true;
    [Space]
    [LabelText("当前速度")]
    [ReadOnly]
    public Vector2 currentSpeed;
    [Header("当前状态")]
    [ReadOnly] public bool isJumping;
    [ReadOnly] public bool isIdling;
    [ReadOnly] public bool isDashing;
    [ReadOnly] public bool isWalking;
    [ReadOnly] public bool isWallSliding;
    [ReadOnly] public int jumpCount;

    private int xInput;
    private int maxJumpCount;
    private int WallSlidefaceDir;

    private CollDetection collDetection;
    private PlayerAnim playerAnim;
    private Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        collDetection = GetComponent<CollDetection>();
        playerAnim = GetComponent<PlayerAnim>();
        maxJumpCount = canDoubleJump ? 2 : (canJump ? 1 : 0);
        jumpCount =maxJumpCount;
    }

    private void Update()
    {
        xInput = InputManager.instance.xInput;
        if (collDetection.OnGround)
        {
            jumpCount = maxJumpCount;
        }
        LRMove();
        Jump();
        WallSlide();
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
                playerAnim.Flip(InputManager.instance.faceDir);
            }
        }
        else//空中左右移动,且不处于“墙上滑落”状态
        {
            rb.velocity = new Vector2(xInput * LRAirMoveSpeed, rb.velocity.y);
            playerAnim.Flip(InputManager.instance.faceDir);
        }
    }
    private void Jump()
    {
        if (!canJump || (jumpCount == 0)) return;
        if (Input.GetKeyDown(InputManager.instance.jumpKey))
        {
            playerAnim.SetTrigger("Jump", true);
            if (isWallSliding)//滑墙跳
            {
                rb.velocity = WallSlidefaceDir*new Vector2(jumpSpeed, jumpSpeed);//待修改
                StartCoroutine(StopMove());
            }
            else //正常跳跃
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
                jumpCount--;
            }
        }
    }
    private void WallSlide()
    {
        if (collDetection.OnGround)
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
                WallSlidefaceDir = 1;
            }
            else WallSlidefaceDir = -1;
            playerAnim.Flip(WallSlidefaceDir);
        }
        else isWallSliding = false;
    }

    IEnumerator StopMove()
    {
        canMove = false;
        yield return new WaitForSeconds(0.2f);
        canMove = true;
    }
}
