using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CollDetection : MonoBehaviour
{
    private LayerMask groundLayer;

    [Header("碰撞盒偏移")]
    public Vector2 groundBoxOffset;
    public Vector2 groundBoxSize;
    public Vector2 leftWallBoxOffset;
    public Vector2 rightWallBoxOffset;
    public Vector2 lrWallBoxSize;

    [Header("状态信息")]
    [ShowInInspector,ReadOnly] private bool onGround;
    [ShowInInspector,ReadOnly] private bool onWall;
    [ShowInInspector,ReadOnly] private bool onLeftWall;
    [ShowInInspector,ReadOnly] private bool onRightWall;

    public bool OnGround { get { return onGround; } }
    public bool OnAir { get { return !onGround; } }
    public bool OnWall { get { return onWall; } }
    public bool OnLeftWall { get { return onLeftWall; } }
    public bool OnRightWall { get { return onRightWall; } }

    private void Awake()
    {
        groundLayer = LayerMask.NameToLayer("Ground");
    }

    private void Update()
    {
        Vector2 position = transform.position;
        onGround = Physics2D.OverlapBox(position+ groundBoxOffset, groundBoxSize, 0, 1<<groundLayer);
        onLeftWall = Physics2D.OverlapBox(position+ leftWallBoxOffset, lrWallBoxSize, 0, 1<<groundLayer);
        onRightWall = Physics2D.OverlapBox(position+ rightWallBoxOffset, lrWallBoxSize, 0, 1<<groundLayer);
        onWall = OnLeftWall || OnRightWall;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector2 position = transform.position;
        Gizmos.DrawWireCube(position + groundBoxOffset, groundBoxSize);
        Gizmos.DrawWireCube(position + leftWallBoxOffset, lrWallBoxSize);
        Gizmos.DrawWireCube(position + rightWallBoxOffset, lrWallBoxSize);
    }
}
