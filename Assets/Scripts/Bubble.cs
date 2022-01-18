using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Timeline;

public class Bubble : MonoBehaviour
{
    public GameObject bubbleBreakPre;
    public float bounceAnimTime;
    public Vector2 boxSize;
    public Sprite redHintImg;
    public int attack;
    private Vector2 direction;
    private float speed;
    private Timer lifeTimer;
    private Action OnDestroySelf;
    private LayerMask groundLayer;
    private Rigidbody2D rb;
    private Sequence se;
    private SpriteRenderer sr;


    private void Awake()
    {
        groundLayer = LayerMask.NameToLayer("Ground");
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        lifeTimer.Tick(Time.deltaTime);
    }

    public void Init(float lifetime, Vector2 dir, float speed, Action action)
    {
        direction = dir;
        OnDestroySelf = action;
        this.speed = speed;
        lifeTimer = new Timer(lifetime, Break);
        lifeTimer.Run();
        se = DOTween.Sequence();
    }

    public bool isBloacked()
    {
        var res= Physics2D.OverlapBox((Vector2) transform.position, boxSize, 0, 1 << groundLayer);
        return res != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube((Vector2)transform.position, boxSize);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        //遇到墙壁和地面反弹
        if (other.gameObject.layer == groundLayer)
        {
            var hit = Physics2D.Raycast(transform.position, direction, 10, 1 << groundLayer);
            var normal = hit.normal;
            //Debug.Log("direction:"+direction+"     normal:"+normal+"     point:"+hit.point+"     collider:"+(hit.collider.gameObject==null?"null":hit.collider.gameObject.ToString()));
            //Debug.DrawRay(transform.position, direction*10, Color.red,2.0f,false);
            var dir = direction.normalized;
            var reflect = 2 * normal + dir;
            var scale = new Vector2(Mathf.Abs(dir.x),Mathf.Abs(dir.y) )* 0.4f;
            if (!se.IsActive())
            {
                se = DOTween.Sequence();
                se.SetAutoKill(false);
                se.Append(transform.DOScaleX(1f - scale.x, bounceAnimTime).SetEase(Ease.InOutSine));
                se.Join(transform.DOScaleY(1f - scale.y, bounceAnimTime).SetEase(Ease.InOutSine));
                se.Append(transform.DOScaleX(1f - scale.x, bounceAnimTime).From().SetEase(Ease.InOutSine));
                se.Join(transform.DOScaleY(1f - scale.y, bounceAnimTime).From().SetEase(Ease.InOutSine));
            }
            direction = reflect;
            rb.velocity = speed * reflect;
        }

        if (other.CompareTag("Enemy"))
        {
           other.GetComponent<EnemyHealth>().Hurt(attack);
           Break();
        }
    }

    private void Break()
    {
        se.Kill();
        OnDestroySelf();
        var trans = transform;
        Instantiate(bubbleBreakPre, trans.position, trans.rotation);
        Destroy(gameObject);
    }
    
}