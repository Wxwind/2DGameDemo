using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thumbtack : MonoBehaviour
{
   [Tooltip("落到地面后几秒后消失")] public float fadeTime;
   public Timer fadeTimer;
   private LayerMask groundLayer;
   private float speed;
   private Vector3 pos;
   private Rigidbody2D rb;
   private Transform trans;
   private void Awake()
   {
      fadeTimer = new Timer(fadeTime, ()=>ThumbtackPool.instance.ReturnPool(this.gameObject), false);
      groundLayer=LayerMask.NameToLayer("Ground");
      rb = GetComponent<Rigidbody2D>(); 
      trans = GetComponent<Transform>();
   }

   public void Init(float speed,Vector3 pos)
   {
      this.speed = speed;
      this.pos = pos;
   }

   private void Start()
   {
      rb.velocity = new Vector2(0, speed);
      trans.position = pos;
   }

   private void Update()
   {
      fadeTimer.Tick(Time.deltaTime);
   }

   private void OnTriggerEnter2D(Collider2D other)
   {
      if (other.gameObject.layer==groundLayer)
      {
         fadeTimer.Run();
         rb.velocity = Vector2.zero;
      }
   }
}
