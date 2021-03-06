﻿using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{

    private Transform player;
    private Vector2 targetPosition;
    public int lossFood = 10;
    private Rigidbody2D m_rigidbody;
    private BoxCollider2D m_collider;
    private Animator m_animator;
    public float smoothing = 3;
    public AudioClip attackAudio;  //攻击音效
    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        m_rigidbody = GetComponent<Rigidbody2D>();
        targetPosition = transform.position;
        GameManager.Instance.enemyList.Add(this);
        m_collider = GetComponent<BoxCollider2D>();
        m_animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        m_rigidbody.MovePosition(Vector2.Lerp(transform.position,targetPosition,smoothing*Time.deltaTime));
    }
    public void Move()
    {
        Vector2 offset = player.position - transform.position;
        if (offset.magnitude < 1.0f)
        {
            //Attack
            AudioManager.Instance.RandomPlayClips(attackAudio);
            m_animator.SetTrigger("Attack");
            player.SendMessage("TakeDamage",lossFood);
        }
        else
        {
            float x = 0, y = 0;
            if (Mathf.Abs(offset.y) > Mathf.Abs(offset.x))
            {
                //按照 Y 轴移动
                if (offset.y < 0)
                {
                    y = -1;

                }
                else
                {
                    y = 1;
                }
            }
            else
            {
                //按照 X 轴移动
                if (offset.x > 0)
                {
                    x = 1;
                }
                else
                {
                    x = -1;
                }
            }
            m_collider.enabled = false;
            RaycastHit2D hit = Physics2D.Linecast(targetPosition, targetPosition + new Vector2(x, y));
            m_collider.enabled = true;
            if (hit.transform == null)
            {
                targetPosition += new Vector2(x, y);
            }
            else
            {
                if (hit.collider.tag == "Food" || hit.collider.tag == "Soda")
                {
                    targetPosition += new Vector2(x, y);
                }
            }

        }
    }
}
