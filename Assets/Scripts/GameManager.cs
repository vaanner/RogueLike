using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class GameManager : MonoBehaviour {
    
    public int level = 1;
    public int food = 100;
    [HideInInspector]public bool isEnd = false;
    public List<Enemy> enemyList = new List<Enemy>();
    private static GameManager _instance;
    private bool sleepStep = true;
    private Text foodText;
    private Text failText;
    private Player m_player;
    private MapManager m_mapManager;
    public AudioClip playerDieAudio;
	// Use this for initialization
    void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(gameObject);
        InitGame();
    }
    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
    }
    void InitGame()
    {
        //初始化地图
        m_mapManager = GetComponent<MapManager>();
        m_mapManager.InitMap();
        //初始化 UI
        foodText = GameObject.Find("FoodText").GetComponent<Text>();
        UpdateFoodText(food);
        failText = GameObject.Find("FailText").GetComponent<Text>();
        failText.enabled = false;
        m_player = GameObject.Find("Player").GetComponent<Player>();
        //初始化参数
        isEnd = false;
        enemyList.Clear();
    }
	void Start () {
        failText.gameObject.SetActive(false);
	    UpdateFoodText(food);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void AddFood(int count)
    {
        food += count;
        UpdateFoodText(food);
    }
    public void ReduceFood(int count)
    {
        food -= count;
        UpdateFoodText(food);
    }
    public void OnPlayerMove()
    {
        if(sleepStep == true)
        {
            sleepStep = false;
        }
        else
        {
            foreach(var enemy in enemyList)
            {
                enemy.Move();
            }
            sleepStep = true;
        }
        //检测是否到达终点
        Vector2 exitPos = new Vector2(m_mapManager.cols-2,m_mapManager.rows-2);
        if(m_player.targetPos == exitPos)
        {
            isEnd = true;
            //加载下一个关卡
            Application.LoadLevel(Application.loadedLevel);
        }
    }
    void UpdateFoodText(int food)
    {
        foodText.text = "Food:" + food;
        if (food <= 0)
        {
            AudioManager.Instance.RandomPlayClips(playerDieAudio);
            AudioManager.Instance.bgMusicStop();
            failText.enabled = true;
            failText.gameObject.SetActive(true);
            foodText.enabled = false;
        }
    }
    void OnLevelWasLoaded(int level)
    {
       this.level++;
       InitGame();
    }
}
