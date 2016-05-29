using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    private Vector2 originPos;
    [HideInInspector]public Vector2 targetPos;
    private Rigidbody2D m_rigidBody2d;
    private BoxCollider2D m_collider;
    private Animator m_animator;
    public float smoothing = 1.0f;
    public float restTime = 1.0f;
    private float restTimer = 0f;
    public AudioClip chop1Audio;    //攻击音效1
    public AudioClip chop2Audio;    //攻击音效2
    public AudioClip footStep1Audio;    //脚步声1
    public AudioClip footStep2Audio;    //脚步声2
    public AudioClip soda1Audio;        //苏打吃声1
    public AudioClip soda2Audio;        //苏打吃声2
    public AudioClip fruit1Audio;       //樱桃吃声1
    public AudioClip fruit2Audio;       //樱桃吃声2
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
        //食物为0时 角色停止移动
        if(GameManager.Instance.food < 0 || GameManager.Instance.isEnd) return;
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
                AudioManager.Instance.RandomPlayClips(footStep1Audio,footStep2Audio);
                targetPos += new Vector2(h,v);
            }
            else
            {
                switch (hit.collider.tag)
                {
                    case "OutWall":
                    break;
                    case "Wall":
                    AudioManager.Instance.RandomPlayClips(chop1Audio,chop2Audio);
                    m_animator.SetTrigger("Attack");
                    hit.collider.SendMessage("TakeDamage");
                    break;
                    case "Soda":
                    AudioManager.Instance.RandomPlayClips(soda1Audio,soda2Audio);
                    GameManager.Instance.AddFood(20);
                    targetPos += new Vector2(h,v);
                    Destroy(hit.transform.gameObject);
                    break;
                    case "Food":
                    AudioManager.Instance.RandomPlayClips(fruit1Audio,fruit2Audio);
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
            GameManager.Instance.ReduceFood(GameManager.Instance.level);
            restTimer = 0;
        }
	}
    private void TakeDamage(int lossFood)
    {
        GameManager.Instance.ReduceFood(lossFood);
        m_animator.SetTrigger("Damage");
    }
}
