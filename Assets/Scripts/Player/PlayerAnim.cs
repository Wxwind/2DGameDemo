using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerAnim : MonoBehaviour
{
    private SpriteRenderer sr;
    private Animator anim;
    private PlayerController playerController;
    private CollDetection collDetection;
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
        collDetection = GetComponent<CollDetection>();
    }

    private void Update()
    {
        anim.SetFloat("XSpeed", playerController.currentSpeed.x);
        anim.SetFloat("YSpeed", playerController.currentSpeed.y);
        anim.SetBool("Idle", playerController.isIdling);
        anim.SetBool("Walk", playerController.isWalking);
        anim.SetBool("WallSlide", playerController.isWallSliding);
        anim.SetBool("OnGround", collDetection.OnGround);
    }
    
    public void Flip(int faceDir)
    {
        if (faceDir == 1)
        {
            sr.flipX = false;
        }
        else sr.flipX = true;
    } 

    public void SetTrigger(string animTriggerName,bool trigger)
    {
        anim.SetBool(animTriggerName, trigger);
    }
}
