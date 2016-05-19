using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    private Vector2 originPos;
    private Vector2 targetPos;
    private Rigidbody2D m_rigidBody2d;
    private BoxCollider2D m_collider;
    private Animator m_animator;
    public float smoothing = 1.0f;
    public float restTime = 1.0f;
    private float restTimer = 0f;
	// Use this for initialization
	void Start () {
	    originPos = new Vector2(1,1);
        targetPos = originPos;
        m_rigidBody2d = GetComponent<Rigidbody2D>();
        m_collider = GetComponent<BoxCollider2D>();
        m_animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        
        m_rigidBody2d.MovePosition(Vector2.Lerp(transform.position,targetPos,smoothing*Time.deltaTime));
        restTimer += Time.deltaTime;
        if(restTimer<restTime)
        return;
	    float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        if(h > 0)
        {
            v = 0;
        }
        if( h != 0 || v !=0)
        {
            m_collider.enabled = false;
            RaycastHit2D hit = Physics2D.Linecast(targetPos,targetPos + new Vector2(h,v));
            m_collider.enabled = true;
            if(hit.transform == null)
            {
                targetPos += new Vector2(h,v);
            }
            else
            {
                switch (hit.collider.tag)
                {
                    case "OutWall":
                    break;
                    case "Wall":
                    m_animator.SetTrigger("Attack");
                    hit.collider.SendMessage("TakeDamage");
                    break;
                    case "Soda":
                    GameManager.Instance.AddFood(20);
                    targetPos += new Vector2(h,v);
                    Destroy(hit.transform.gameObject);
                    break;
                    case "Food":
                    GameManager.Instance.AddFood(10);
                    targetPos += new Vector2(h,v);
                    Destroy(hit.transform.gameObject);
                    break;
                    case "Enemy":
                    break;
                    default:
                    break;
                }
            }
            GameManager.Instance.OnPlayerMove();
            restTimer = 0;
        }
	}
}
