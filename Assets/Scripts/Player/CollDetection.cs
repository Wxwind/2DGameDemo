using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CollDetection : MonoBehaviour
{
    [Tooltip("检测地面和墙壁")]
    public LayerMask groundLayer;

    [Header("碰撞盒偏移")]
    public Vector2 groundBoxOffset;
    public Vector2 groundBoxSize;
    public Vector2 leftWallBoxOffset;   
    public Vector2 rightWallBoxOffset;
    public Vector2 lrWallBoxSize;
    [ReadOnly]public bool OnGround { private set; get; }
    [ReadOnly]public bool OnAir { get { return !OnGround; } }
    [ReadOnly]public bool OnWall { private set; get; }
    [ReadOnly]public bool OnLeftWall { private set; get; }
    [ReadOnly]public bool OnRightWall { private set; get; }

    private void Update()
    {
        OnGround = Physics2D.OverlapBox((Vector2)transform.position + groundBoxOffset, groundBoxSize, 0, groundLayer);
        OnLeftWall= Physics2D.OverlapBox((Vector2)transform.position + leftWallBoxOffset, lrWallBoxSize, 0, groundLayer);
        OnRightWall= Physics2D.OverlapBox((Vector2)transform.position + rightWallBoxOffset, lrWallBoxSize, 0, groundLayer);
        OnWall = OnLeftWall || OnRightWall;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube((Vector2)transform.position + groundBoxOffset, groundBoxSize);
        Gizmos.DrawWireCube((Vector2)transform.position + leftWallBoxOffset, lrWallBoxSize);
        Gizmos.DrawWireCube((Vector2)transform.position + rightWallBoxOffset, lrWallBoxSize);
    }
}
